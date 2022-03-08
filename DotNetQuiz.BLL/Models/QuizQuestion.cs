using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Models
{
    public class QuizQuestion : IComparable<QuizQuestion>, IComparable
    {
        public int QuestionId { get; init; }
        public QuestionType QuestionType { get; init; }
        public int QuestionReward { get; init; }
        public QuestionContent? Content { get; init; }
        public QuestionAnswer? Answer { get; init; }
        public string[]? Options { get; init; }
        public bool IsCompleted { get; set; }

        public int CompareTo(QuizQuestion? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.QuestionId - other.QuestionId;
        }

        public int CompareTo(object? obj) => this.CompareTo(obj as QuizQuestion);
    }
}
