using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;

public interface IQuizHub
{
    Task NextRound(QuizRoundModel quizRound);
    Task ReceiveQuestion(QuizPlayerAnswer answer);
    Task PlayerAdded(QuizPlayerModel quizPlayer);
    Task PlayerRemoved(QuizPlayerModel quizPlayer);
    Task SessionStateChanged(SessionState sessionState);

}

