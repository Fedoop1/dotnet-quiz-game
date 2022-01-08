using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuizSessionService
    {
        void UploadQuizConfiguration(QuizConfiguration configuration);
        void AddPlayerToSession(int playerId, string? playerNickName);
        void RemovePlayerFromSession(int playerId);
        void StartGame();
        void SubmitAnswer(QuizPlayerAnswer answer);
        QuizRound NextRound();
    }
}
