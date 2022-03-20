using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Extensions;
public static class ModelMapper
{
    public static QuizRoundModel ToQuizRoundModel(this QuizRound quizRound) => new()
    {
        EndAt = quizRound.EndAt, StartAt = quizRound.StartAt, Question = quizRound.CurrentQuestion
    };

    public static QuizSessionModel ToQuizSessionModel(this IQuizSessionHandler quizSessionHandler) => new()
    {
        CountOfPlayers = quizSessionHandler.SessionPlayers.Count,
        MaxPlayers = quizSessionHandler.QuizConfiguration.MaxPlayers,
        SessionId = quizSessionHandler.SessionId,
        isOpen = quizSessionHandler.IsOpen,
    };

    public static QuizPlayerModel ToQuizPlayerModel(this QuizPlayer quizPlayer) => new ()
        { Id = quizPlayer.Id, NickName = quizPlayer.NickName };
}