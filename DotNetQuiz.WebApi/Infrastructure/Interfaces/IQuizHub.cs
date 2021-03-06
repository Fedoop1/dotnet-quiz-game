using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;

public interface IQuizHub
{
    Task NextRound(QuizRoundModel quizRound);
    Task ReceiveQuestion(QuizPlayerAnswer answer);
    Task ProcessAnswer(QuizPlayerModel quizPlayer);
    Task PlayerAdded(QuizPlayerModel quizPlayer);
    Task PlayerRemoved(QuizPlayerModel quizPlayer);
    Task SessionStateChanged(SessionState sessionState);
    Task Error(HubError hubError);

}

