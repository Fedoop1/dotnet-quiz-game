namespace DotNetQuiz.BLL.Models
{
    public class QuizQuestionPack
    {
        public int QuestionPackId { get; set; }
        public IEnumerable<QuizQuestion>? Questions { get; set; }
    }
}
