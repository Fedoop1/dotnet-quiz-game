using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizRoundModel
    {
        public QuizQuestion Question {get; init; }
        public DateTime? StartAt { get; init; }
        public DateTime? EndAt { get; init; }
    }
}
