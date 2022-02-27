using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.WebApi.Models;

public class QuizSessionModel
{
    public int CountOfPlayers { get; init; }
    public int MaxPlayers { get; init; }
    public Guid SessionId { get; init; }
    public SessionState SessionState { get; init; }
}

