using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    internal interface IQuizLoaderService
    {
        QuizQuestionPack LoadPack();
    }
}
