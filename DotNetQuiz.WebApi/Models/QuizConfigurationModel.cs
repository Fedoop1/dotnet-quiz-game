using System.ComponentModel.DataAnnotations;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizConfigurationModel
    {
        [Range(5, int.MaxValue)]
        public int RoundDuration { get; set; } = 60;

        [Range(1, int.MaxValue)]
        public int MaxPlayers { get; set; } = 4;

        [Range(1, double.MaxValue)] public double StreakMultiplier { get; set; } = 1;

        [Range(1, double.MaxValue)] public double TimeMultiplier { get; set; } = 1;

        public bool AnswerIgnoreCase { get; set; }

        [Required(ErrorMessage = "Question pack is required!")]
        public QuizQuestionPack QuestionPack { get; set; }
    }
}
