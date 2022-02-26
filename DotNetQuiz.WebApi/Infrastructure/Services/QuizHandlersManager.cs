using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Services;

public class QuizHandlersManager : IQuizHandlersManager
{
    private readonly Dictionary<Guid, IQuizSessionHandler> sessionHandlersStorage = new ();

    public void AddSessionHandler(Guid sessionId, IQuizSessionHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        if (!this.sessionHandlersStorage.TryAdd(sessionId, handler))
        {
            throw new ArgumentException($"Quiz session handler with id [{sessionId}] already exist");
        }
    }

    public IQuizSessionHandler? GetSessionHandler(Guid sessionId) =>
        !this.sessionHandlersStorage.TryGetValue(sessionId, out var quizSessionHandler) ? null : quizSessionHandler;
}