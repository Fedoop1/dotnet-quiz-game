using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.WebApi.Infrastructure.Hubs;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Services
{
    public class QuizHubsFactory : IQuizHubsFactory
    {
        public IQuizHub CreateQuizHub(IQuizSessionHandler quizSessionHandler) => new QuizHub(quizSessionHandler);
    }
}
