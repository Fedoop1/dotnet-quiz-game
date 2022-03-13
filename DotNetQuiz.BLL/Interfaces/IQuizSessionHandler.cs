using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuizSessionHandler
    {
        bool IsOpen { get; }
        Guid SessionId { get; }
        QuizConfiguration QuizConfiguration { get; }
        QuizRound CurrentRound { get; }
        IReadOnlyCollection<QuizPlayer> SessionPlayers { get; }
        void UploadQuizConfiguration(QuizConfiguration configuration);
        void AddPlayerToSession(QuizPlayer quizPlayer);
        void RemovePlayerFromSession(string playerId);
        void StartGame();
        void SubmitAnswer(QuizPlayerAnswer answer);
        void NextRound();
        void StartRound();
        RoundStatistic BuildCurrentRoundStatistic();
    }
}
