using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;

public interface IQuizHub
{
    Task SendQuestion(QuizRoundModel quizRound);
    Task ReceiveQuestion(QuizPlayerAnswer answer);
    Task SendRoundStatistic(RoundStatistic roundStatistic);
    Task PlayerAdded(QuizPlayerModel quizPlayer);
    Task PlayerRemoved(QuizPlayerModel quizPlayer);
    Task SessionClosed();
    
}

