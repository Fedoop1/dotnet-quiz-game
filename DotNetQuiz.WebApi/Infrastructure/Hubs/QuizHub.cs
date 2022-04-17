using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;
using DotNetQuiz.WebApi.Infrastructure.Extensions;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using DotNetQuiz.WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace DotNetQuiz.WebApi.Infrastructure.Hubs
{
    public partial class QuizHub : Hub<IQuizHub>
    {
        private readonly IQuizHandlersManager handlersManager;
        private readonly ILogger logger;

        public QuizHub(IQuizHandlersManager handlersManager, ILogger<QuizHub> logger) =>
            (this.handlersManager, this.logger) = (handlersManager, logger);

        public async void ProcessAnswer(string sessionId, QuizPlayerAnswer answer)
        {
            this.LogProcessAnswer(sessionId, this.Context.ConnectionId);

            try
            {
                var sessionHandler = this.handlersManager.GetSessionHandler(Guid.Parse(sessionId));

                if (sessionHandler == null) return;

                sessionHandler.SubmitAnswer(answer);
                await this.Clients.OthersInGroup(sessionId).ProcessAnswer(new QuizPlayerModel { Id = answer.PlayerId });
            }
            catch (Exception e)
            {
                // TODO: Add error method calling
                this.LogError(sessionId, nameof(e), e.Message); throw;
            }
            
        }

        public async Task ChangeSessionState(string sessionId, SessionState sessionState)
        {
            await this.Clients.OthersInGroup(sessionId).SessionStateChanged(sessionState);
            this.LogChangeSessionState(sessionId, Enum.GetName(typeof(SessionState), sessionState)!);
        }

        public override async Task OnConnectedAsync()
        {
            if (this.TryExtractRouteData(out var routeData))
            {
                this.LogConnectionError("Invalid route data");
                this.Context.Abort();
                return;
            }

            var connectionId = this.Context.ConnectionId;
            var sessionHandler = this.handlersManager.GetSessionHandler(routeData.sessionId);

            if (sessionHandler == null)
            {
                this.LogConnectionError($"Session with id [{routeData.sessionId}] doesn't exist");
                this.Context.Abort();
                return;
            }

            if (routeData.isHost)
            {
                await this.Groups.AddToGroupAsync(connectionId, routeData.sessionId.ToString());
                this.LogCreateSession(routeData.sessionId, connectionId);
                await base.OnConnectedAsync();
                return;
            }

            var quizPlayer = new QuizPlayer { Id = connectionId, NickName = routeData.nickName };

            sessionHandler.AddPlayerToSession(quizPlayer);
            await this.Groups.AddToGroupAsync(connectionId, routeData.sessionId.ToString());

            await this.Clients.OthersInGroup(routeData.sessionId.ToString())
                .PlayerAdded(quizPlayer.ToQuizPlayerModel());

            this.LogJoinSession(routeData.sessionId, connectionId, routeData.nickName);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (this.TryExtractRouteData(out var routeData))
            {
                this.LogDisconnectionError("Invalid route data");
                await base.OnDisconnectedAsync(exception);
                return;
            }

            var connectionId = this.Context.ConnectionId;
            var sessionHandler = this.handlersManager.GetSessionHandler(routeData.sessionId);

            if (sessionHandler == null)
            {
                this.LogDisconnectionError($"Session with id [{routeData.sessionId}] doesn't exist");
                await base.OnDisconnectedAsync(exception);
                return;
            }

            if (routeData.isHost)
            {
                await this.Clients.OthersInGroup(routeData.sessionId.ToString()).SessionStateChanged(SessionState.Closed);
                this.LogCloseSession(routeData.sessionId, connectionId);

                foreach (var player in sessionHandler.SessionPlayers)
                {
                    await this.Groups.RemoveFromGroupAsync(player.Id, routeData.sessionId.ToString());
                }

                this.handlersManager.RemoveSessionHandler(routeData.sessionId);
                await base.OnDisconnectedAsync(exception);
                return;
            }

            var sessionPlayer = sessionHandler.SessionPlayers.FirstOrDefault(sp => sp.Id == connectionId);

            sessionHandler.RemovePlayerFromSession(connectionId);

            await this.Groups.RemoveFromGroupAsync(connectionId, routeData.sessionId.ToString());
            await this.Clients.OthersInGroup(routeData.sessionId.ToString())
                .PlayerRemoved(new QuizPlayerModel { Id = connectionId, NickName = routeData.nickName });

            this.LogLeaveSession(routeData.sessionId, connectionId, sessionPlayer?.NickName);
            await base.OnDisconnectedAsync(exception);
        }

        private bool TryExtractRouteData(out (Guid sessionId, string nickName, bool isHost) routeData)
        {
            var httpContext = this.Context.GetHttpContext();
            var routeValues = httpContext.Request.RouteValues;

            if (!routeValues.TryGetValue("sessionId", out var sessionId) ||
                !routeValues.TryGetValue("nickName", out var nickName) ||
                !routeValues.TryGetValue("isHost", out var isHost))
            {
                routeData = (Guid.Empty, string.Empty, false);
                return false;
            }

            if(!Guid.TryParse(sessionId as string, out var sessionIdValue) || !bool.TryParse(isHost as string, out var isHostValue))
            {
                routeData = (Guid.Empty, string.Empty, false);
                return false;
            }
            
            routeData = (sessionIdValue, nickName as string, isHostValue);
            return string.IsNullOrEmpty(nickName as string);
        }


        #region Log Methods

        [LoggerMessage(0, LogLevel.Information, "User connected to the hub. Session id: {sessionId}. User id: {userId}.")]
        private partial void LogOnConnected(Guid sessionId, string userId);

        [LoggerMessage(1, LogLevel.Information, "User disconnected from the hub. Session id: {sessionId}. User id: {userId}.")]
        private partial void LogOnDisconnected(Guid sessionId, string userId);

        [LoggerMessage(2, LogLevel.Information, "New session is created. Session id: {sessionId}. Host id: {userId}.")]
        private partial void LogCreateSession(Guid sessionId, string userId);

        [LoggerMessage(3, LogLevel.Information, "Session closed. Session id: {sessionId}. Host id: {userId}")]
        private partial void LogCloseSession(Guid sessionId, string userId);

        [LoggerMessage(4, LogLevel.Information, "Player joined to the session. Session id: {sessionId}. User nickname: {nickName}.")]
        private partial void LogJoinSession(Guid sessionId, string userId, string nickName);

        [LoggerMessage(5, LogLevel.Information, "Player leaved the session. Session id: {sessionId}. User id: {sessionId}. User nickname: {nickName}.")]
        private partial void LogLeaveSession(Guid sessionId, string userId, string nickName);

        [LoggerMessage(6, LogLevel.Information, "Send round statistic. Session id: {sessionId}.")]
        private partial void LogSendRoundStatistic(Guid sessionId);

        [LoggerMessage(7, LogLevel.Information, "Send question. Session id: {sessionId}.")]
        private partial void LogSendQuestion(Guid sessionId);

        [LoggerMessage(8, LogLevel.Information, "Process Answer. Session id: {sessionId}. User id {userId}.")]
        private partial void LogProcessAnswer(string sessionId, string userId);

        [LoggerMessage(9, LogLevel.Warning, "Connection error. Message: {message}.")]
        private partial void LogConnectionError(string message);

        [LoggerMessage(10, LogLevel.Warning, "Disconnection error. Message: {message}.")]
        private partial void LogDisconnectionError(string message);

        [LoggerMessage(11, LogLevel.Information, "Change session state. Session id: {sessionId}. Session state {sessionState}.")]
        private partial void LogChangeSessionState(string sessionId, string sessionState);

        [LoggerMessage(12, LogLevel.Error, "An exception happened. Session id: {sessionId}. Exception: {exception}. Exception Message: {exceptionMessage}.")]
        private partial void LogError(string sessionId, string exception, string exceptionMessage);

        #endregion
    }
}
