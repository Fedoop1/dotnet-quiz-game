using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Models
{
    public class QuestionContent
    {
        public QuestionType QuestionType { get; init; } 
        public string[]? QuestionOptions { get; init; }
        public string? QuestionText { get; init; }
        public byte[]? QuestionBlob { get; init; }
    }
}
