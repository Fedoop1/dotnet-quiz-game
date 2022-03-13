namespace DotNetQuiz.WebApi.Models;

public class QuizSessionModel
{
    public int CountOfPlayers { get; init; }
    public int MaxPlayers { get; init; }
    public Guid SessionId { get; init; }
    public bool isOpen { get; init; }
}

