using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuestionHandler
    {
        QuizQuestion CurrentQuestion { get; }
        QuizQuestion NextQuestion();
    }
}
