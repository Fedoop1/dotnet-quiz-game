using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuizSessionHandler
    {
        Guid SessionId { get; }
        QuizConfiguration QuizConfiguration { get; }
        QuizRound CurrentSessionRound { get; }
        IReadOnlyCollection<QuizPlayer> SessionPlayers { get; }
        SessionState SessionState { get; }
        void UploadQuizConfiguration(QuizConfiguration configuration);
        void AddPlayerToSession(QuizPlayer quizPlayer);
        void RemovePlayerFromSession(string playerId);
        void StartGame();
        void SubmitAnswer(QuizPlayerAnswer answer);
        void NextRound();
        RoundStatistic BuildCurrentRoundStatistic();
    }
}
