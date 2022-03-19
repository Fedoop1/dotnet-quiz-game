using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizRoundModel
    {
        public int QuestionId { get; init; }
        public QuestionContent? QuestionContent { get; init; }
        public DateTime? StartAt { get; init; }
        public DateTime? EndAt { get; init; }
    }
}
