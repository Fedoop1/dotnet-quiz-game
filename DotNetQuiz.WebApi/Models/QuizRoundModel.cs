using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizRoundModel
    {
        public TimeOnly StartAt { get; init; }
        public TimeOnly EndAt { get; init; }
        public QuestionContent? QuestionContent { get; init; }
    }
}
