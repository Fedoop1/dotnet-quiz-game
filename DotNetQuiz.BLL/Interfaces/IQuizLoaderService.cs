using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IQuizLoaderService
    {
        QuizQuestionPack LoadPack();
    }
}
