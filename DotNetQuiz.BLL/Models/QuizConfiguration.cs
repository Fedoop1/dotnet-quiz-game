namespace DotNetQuiz.BLL.Models
{
    public class QuizConfiguration
    {
        public int RoundDuration { get; set; } = 60;
        public int MaxPlayers { get; set; } = 4;
        public double StreakMultiplier { get; set; }
        public double TimeMultiplier { get; set; }
        public bool AnswerIgnoreCase { get; set; }
        public QuizQuestionPack? QuestionPack { get; set; }
    }
}
