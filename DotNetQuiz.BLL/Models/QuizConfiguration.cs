namespace DotNetQuiz.BLL.Models
{
    public class QuizConfiguration
    {
        public int RoundDuration { get; set; }
        public int MaxPlayers { get; set; }
        public double StreakMultiplier { get; set; }
        public double TimeMultiplier { get; set; }
        public bool AnswerIgnoreCase { get; set; }
        public QuizQuestionPack QuestionPack { get; set; }
    }
}
