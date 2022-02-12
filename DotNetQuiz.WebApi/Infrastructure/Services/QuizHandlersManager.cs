using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Services
{
    public class QuizHandlersManager : IQuizHandlersManager
    {
        private readonly Dictionary<Guid, IQuizSessionHandler> sessionHandler = new ();

        public void AddSessionHandler(IQuizSessionHandler handler)
        {
            ArgumentNullException.ThrowIfNull(handler);

            if (this.sessionHandler.TryAdd(handler.QuizHandlerId, handler))
            {
                throw new ArgumentException($"Quiz session handler with id [{handler.QuizHandlerId}] already exist");
            }
        }

        public IQuizSessionHandler? GetSessionHandler(Guid sessionId) =>
            !this.sessionHandler.TryGetValue(sessionId, out var quizSessionHandler) ? null : quizSessionHandler;
    }
}
