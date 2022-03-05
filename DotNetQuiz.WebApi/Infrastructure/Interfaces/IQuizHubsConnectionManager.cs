namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;
public interface IQuizHubsConnectionManager
{
    public void AddQuizSessionHub(Guid sessionId, IQuizHub hub);
    public void RemoveQuizSessionHub(Guid sessionId);

    public IQuizHub? GetQuizSessionHub(Guid sessionId);
}