namespace DotNetQuiz.WebApi.Infrastructure.Interfaces;
public interface IQuizHubsConnectionManager
{
    public void AddQuizSessionHub(Guid sessionId, IQuizHub hub);

    public IQuizHub? GetQuizSessionHub(Guid sessionId);
}