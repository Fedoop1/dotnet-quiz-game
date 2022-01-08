namespace DotNetQuiz.BLL.Models
{
    public class QuizQuestionPack
    {
        public int QuestionPackId { get; set; }

        public ICollection<QuizQuestion> Questions { get; set; }
    }
}
