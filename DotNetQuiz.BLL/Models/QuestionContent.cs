using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Models
{
    public class QuestionContent
    {
        public string? QuestionText { get; init; }
        public byte[]? QuestionBlob { get; init; }
    }
}
