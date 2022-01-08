namespace DotNetQuiz.BLL.Models
{
    public class QuizRound
    {
        public QuizQuestion CurrentQuestion { get; init; }
        public TimeOnly StartAt { get; init; }
        public TimeOnly EndAt { get; init; }
        public List<QuizPlayerAnswer> Answers { get; init; }
    }
}
