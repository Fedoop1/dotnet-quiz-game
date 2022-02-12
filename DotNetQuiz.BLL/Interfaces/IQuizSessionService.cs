using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuizSessionService
    {
        QuizRound CurrentRound { get; }
        IReadOnlyCollection<QuizPlayer> SessionPlayers { get; }
        SessionState SessionState { get; }

        void UploadQuizConfiguration(QuizConfiguration configuration);
        void AddPlayerToSession(int playerId, string? playerNickName);
        void RemovePlayerFromSession(int playerId);
        void StartGame();
        void SubmitAnswer(QuizPlayerAnswer answer);
        void NextRound();
        RoundStatistic BuildCurrentRoundStatistic();
    }
}
