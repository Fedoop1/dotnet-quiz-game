namespace DotNetQuiz.BLL.Models
{
    public class QuizQuestion : IComparable<QuizQuestion>, IComparable
    {
        public int QuestionId { get; set; }
        public int QuestionReward { get; set; }
        public QuestionContent? Content { get; set; }
        public QuestionAnswer? Answer { get; set; }
        public bool isCompleted { get; set; }

        public int CompareTo(QuizQuestion? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.QuestionId - other.QuestionId;
        }

        public int CompareTo(object? obj) => this.CompareTo(obj as QuizQuestion);
    }
}
