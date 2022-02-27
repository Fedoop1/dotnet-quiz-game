using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Infrastructure.Extensions;
using DotNetQuiz.WebApi.Infrastructure.Filters;
using DotNetQuiz.WebApi.Infrastructure.Helpers;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using DotNetQuiz.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetQuiz.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ExceptionHandlerFilter]
    public class QuizController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IQuizSessionHandlersFactory sessionHandlerFactory;
        private readonly IQuizHandlersManager handlersManager;
        private readonly IQuizHubsFactory hubsFactory;
        private readonly IQuizHubsConnectionManager hubsConnectionManager;

        public QuizController(ILogger<QuizController> logger, IQuizSessionHandlersFactory sessionHandlerFactory,
            IQuizHandlersManager handlersManager, IQuizHubsConnectionManager hubsConnectionManager, IQuizHubsFactory hubsFactory) =>
            (this.logger, this.sessionHandlerFactory, this.handlersManager, this.hubsConnectionManager, this.hubsFactory) =
            (logger, sessionHandlerFactory, handlersManager, hubsConnectionManager, hubsFactory);

        [Route("")]
        public JsonResult Info()
        {
            return new JsonResult(new { controllers = nameof(QuizController), actions = new []
            {
                typeof(QuizController).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public).Select(m => m.Name)
            } });
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult Create()
        {
            var quizSessionHandler = this.sessionHandlerFactory.CreateSessionHandler();
            var quizHub = this.hubsFactory.CreateQuizHub(quizSessionHandler);

            this.handlersManager.AddSessionHandler(quizSessionHandler.SessionId, quizSessionHandler);
            this.hubsConnectionManager.AddQuizSessionHub(quizSessionHandler.SessionId, quizHub);

            return Ok(quizSessionHandler.SessionId);
        }

        [HttpPost]
        [Route("[action]/{sessionId:guid}")]
        [SessionFilter]
        public IActionResult Configure(Guid sessionId, [Required(ErrorMessage = "Quiz Configuration is required!")] QuizConfiguration quizConfiguration)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler!.UploadQuizConfiguration(quizConfiguration);

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetQuizSessions()
        {
            return Ok(this.handlersManager.GetAllSessionHandlers().Select(h => h.ToQuizSessionModel()));
        }

        [HttpPost]
        [Route("{sessionId:guid}/[action]/")]
        [SessionFilter]
        public IActionResult AddPlayer(Guid sessionId, QuizPlayerModel player)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler!.AddPlayerToSession(player.ToQuizPlayer());

            return Ok();
        }

        [HttpPost]
        [Route("{sessionId:guid}/[action]/{playerId:int}")]
        [SessionFilter]
        public IActionResult RemovePlayer(Guid sessionId, [Range(1, int.MaxValue)] int playerId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler!.RemovePlayerFromSession(playerId);

            return Ok();
        }

        [HttpGet]
        [Route("{sessionId:guid}/[action]")]
        [SessionFilter]
        public IActionResult BuildRoundStatistic(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            var quizHub = this.hubsConnectionManager.GetQuizSessionHub(sessionId);

            var currentRoundStatistic = sessionHandler!.BuildCurrentRoundStatistic();

            quizHub?.SendRoundStatisticAsync(currentRoundStatistic);

            return Ok(currentRoundStatistic);
        }

        [HttpGet]
        [Route("{sessionId:guid}/[action]")]
        [SessionFilter]
        public IActionResult StartGame(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler!.StartGame();

            return Ok();
        }

        [HttpGet]
        [Route("{sessionId:guid}/[action]")]
        [SessionFilter]
        public IActionResult NextRound(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            var sessionHub = this.hubsConnectionManager.GetQuizSessionHub(sessionId);

            sessionHandler!.NextRound();
            sessionHub?.SendQuestionAsync(sessionHandler.CurrentSessionRound.ToQuizRoundModel());

            return Ok();
        }
    }
}
