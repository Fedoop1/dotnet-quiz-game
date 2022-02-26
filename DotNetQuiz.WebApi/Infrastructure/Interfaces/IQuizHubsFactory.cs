using DotNetQuiz.BLL.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;

public interface IQuizHubsFactory
{
    public IQuizHub CreateQuizHub(IQuizSessionHandler quizSessionHandler);
}

