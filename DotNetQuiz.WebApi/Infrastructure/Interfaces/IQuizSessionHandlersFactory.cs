using DotNetQuiz.BLL.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces
{
    public interface IQuizSessionHandlersFactory
    {
        public IQuizSessionHandler CreateSessionHandler();
    }
}
