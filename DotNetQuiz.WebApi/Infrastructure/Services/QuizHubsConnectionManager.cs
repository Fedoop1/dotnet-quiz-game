using DotNetQuiz.WebApi.Infrastructure.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Services;

public class QuizHubsConnectionManager : IQuizHubsConnectionManager
{
    private readonly Dictionary<Guid, IQuizHub> quizSessionHubsStorage = new();
    public void AddQuizSessionHub(Guid sessionId, IQuizHub hub)
    {
        ArgumentNullException.ThrowIfNull(hub, nameof(hub));

        if (!this.quizSessionHubsStorage.TryAdd(sessionId, hub))
        {
            throw new ArgumentException($"Hub for session with id {sessionId} already exists");
        }
    }

    public void RemoveQuizSessionHub(Guid sessionId) => this.quizSessionHubsStorage.Remove(sessionId);

    public IQuizHub? GetQuizSessionHub(Guid sessionId) => !this.quizSessionHubsStorage.TryGetValue(sessionId, out var quizSessionHub) ? null : quizSessionHub;
}