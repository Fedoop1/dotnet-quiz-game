using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using DotNetQuiz.WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace DotNetQuiz.WebApi.Infrastructure.Hubs
{
    public class QuizHub : Hub, IQuizHub
    {
        private readonly IQuizSessionHandler sessionHandler;
        public QuizHub(IQuizSessionHandler sessionHandler) => this.sessionHandler = sessionHandler;

        public async Task SendQuestionAsync(QuizRoundModel quizRound) => await Clients.Users(GetQuizPlayersIds()).SendCoreAsync(
            "roundQuestion", new[] { quizRound });

        public void ReceiveQuestion(QuizPlayerAnswer answer) => this.sessionHandler.SubmitAnswer(answer);

        public async Task SendRoundStatisticAsync(RoundStatistic roundStatistic) => await Clients.Users(GetQuizPlayersIds())
            .SendCoreAsync("roundStatistic", new[] { roundStatistic });

        private IEnumerable<string> GetQuizPlayersIds() => this.sessionHandler.SessionPlayers.Select(p => p.Id.ToString());
    }
}
