using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuestionHandler
    {
        QuizQuestion? NextQuestion(IEnumerable<QuizQuestion> questions);
    }
}
