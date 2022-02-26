using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Extensions;
public static class ModelMapper
{
    public static QuizRoundModel ToQuizRoundModel(this QuizRound quizRound) => new ()
    {
        EndAt = quizRound.EndAt, StartAt = quizRound.StartAt, QuestionContent = quizRound.CurrentQuestion.Content
    };
}