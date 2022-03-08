﻿using System.Reflection;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;
using DotNetQuiz.WebApi.Infrastructure.Extensions;
using DotNetQuiz.WebApi.Infrastructure.Filters;
using DotNetQuiz.WebApi.Infrastructure.Hubs;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DotNetQuiz.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ExceptionHandlerFilter]
    public partial class QuizController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IQuizSessionHandlersFactory sessionHandlerFactory;
        private readonly IQuizHandlersManager handlersManager;
        private readonly IHubContext<QuizHub> quizHubContext;

        public QuizController(ILogger<QuizController> logger, IQuizSessionHandlersFactory sessionHandlerFactory,
            IQuizHandlersManager handlersManager, IHubContext<QuizHub> quizHubContext) =>
            (this.logger, this.sessionHandlerFactory, this.handlersManager, this.quizHubContext) =
            (logger, sessionHandlerFactory, handlersManager, quizHubContext);

        [Route("")]
        public JsonResult Info()
        {
            return new JsonResult(new { controllers = nameof(QuizController), actions = new []
            {
                typeof(QuizController).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public).Select(m => m.Name)
            } });
        }

        [HttpGet]
        [Route("{sessionId:guid}/[action]/{userName}")]
        [SessionFilter]
        public IActionResult GetPlayerInfo(Guid sessionId, string userName)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            var quizPlayer = sessionHandler!.SessionPlayers.FirstOrDefault(sp => sp.NickName == userName);

            return quizPlayer != default ? Ok(quizPlayer) : NotFound();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Create()
        {
            var quizSessionHandler = this.sessionHandlerFactory.CreateSessionHandler();

            this.handlersManager.AddSessionHandler(quizSessionHandler.SessionId, quizSessionHandler);

            return Ok(quizSessionHandler.SessionId);
        }

        [HttpPost]
        [SessionFilter]
        [Route("{sessionId:guid}/[action]")]
        public IActionResult NextRound(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            sessionHandler!.NextRound();

            return Ok();
        }

        [HttpPost]
        [SessionFilter]
        [Route("{sessionId:guid}/[action]")]
        public IActionResult Remove(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);

            if (sessionHandler!.SessionState != SessionState.NotStarted)
            {
                return Conflict(new
                {
                    message = "Session is already running. " +
                              "Session is automatically closes when host leaves."
                });
            }

            this.handlersManager.RemoveSessionHandler(sessionId);

            return Ok();
        }


        [HttpPost]
        [Route("{sessionId:guid}/[action]")]
        [SessionFilter]
        public IActionResult Configure(Guid sessionId, QuizConfiguration quizConfiguration)
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

        [HttpGet]
        [Route("{sessionId:guid}/[action]")]
        [SessionFilter]
        public IActionResult GetSessionPlayers(Guid sessionId) 
        {
            return Ok(this.handlersManager.GetSessionHandler(sessionId)!.SessionPlayers);
        }


        [HttpGet]
        [Route("{sessionId:guid}/[action]")]
        [SessionFilter]
        public IActionResult GetRoundStatistic(Guid sessionId)
        {
            var sessionHandler = this.handlersManager.GetSessionHandler(sessionId);
            var currentRoundStatistic = sessionHandler!.BuildCurrentRoundStatistic();

            return Ok(currentRoundStatistic);
        }
    }
}
