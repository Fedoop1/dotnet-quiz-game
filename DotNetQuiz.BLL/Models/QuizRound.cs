namespace DotNetQuiz.BLL.Models
{
    public class QuizRound
    {
        public QuizQuestion CurrentQuestion { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public IEnumerable<QuizPlayerAnswer> Answers { get; set; }
    }
}
