using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;

public interface IQuizHub
{
    Task SendQuestionAsync(QuizRoundModel quizRound);
    void ReceiveQuestion(QuizPlayerAnswer answer);
    Task SendRoundStatisticAsync(RoundStatistic roundStatistic);
}

