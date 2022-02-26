using DotNetQuiz.BLL.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Interfaces
{
    public interface IQuizHandlersManager
    {
        public void AddSessionHandler(Guid sessionId, IQuizSessionHandler handler);
        public IQuizSessionHandler? GetSessionHandler(Guid sessionId);
    }
}
