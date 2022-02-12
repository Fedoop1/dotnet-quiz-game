using System.ComponentModel.DataAnnotations;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Infrastructure.Filters;
using DotNetQuiz.WebApi.Infrastructure.Helpers;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using DotNetQuiz.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetQuiz.WebApi.Controllers
{
    [ApiController]
    [ExceptionHandlerFilter]
    public class QuizController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IQuizSessionHandlersFactory sessionHandlerFactory;
        private readonly IQuizHandlersManager handlersManager;

        public QuizController(ILogger<QuizController> logger, IQuizSessionHandlersFactory sessionHandlerFactory,
            IQuizHandlersManager handlersManager) =>
            (this.logger, this.sessionHandlerFactory, this.handlersManager) =
            (logger, sessionHandlerFactory, handlersManager);


        [HttpPost]
        public IActionResult Create()
        {
            var quizSessionHandler = this.sessionHandlerFactory.CreateSessionHandler();
            this.handlersManager.AddSessionHandler(quizSessionHandler);

            return Ok(quizSessionHandler.QuizHandlerId);
        }

        [HttpPost]
        [SessionFilter]
        public IActionResult Configure(Guid sessionId, [Required(ErrorMessage = "Quiz Configuration is required!")] QuizConfiguration quizConfiguration)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler.UploadQuizConfiguration(quizConfiguration);

            return Ok();
        }

        [HttpPost]
        [SessionFilter]
        public IActionResult AddPlayer(Guid sessionId, QuizPlayerModel player)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler.AddPlayerToSession(player.ToQuizPlayer());

            return Ok();
        }

        [HttpPost]
        [SessionFilter]
        public IActionResult RemovePlayer(Guid sessionId, [Range(1, int.MaxValue)] int playerId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler.RemovePlayerFromSession(playerId);

            return Ok();
        }

        [HttpGet]
        [SessionFilter]
        public IActionResult BuildRoundStatistic(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);

            return Ok(sessionHandler.BuildCurrentRoundStatistic());
        }

        [HttpGet]
        [SessionFilter]
        public IActionResult StartGame(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler.StartGame();

            return Ok();
        }

        [HttpGet]
        [SessionFilter]
        public IActionResult NextRound(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler.NextRound();

            return Ok();
        }
       
    }
}
