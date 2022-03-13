using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizRoundModel
    {
        public DateTime? StartAt { get; init; }
        public DateTime? EndAt { get; init; }
        public QuestionContent? QuestionContent { get; init; }
    }
}
