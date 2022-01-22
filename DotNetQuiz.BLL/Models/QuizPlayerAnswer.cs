namespace DotNetQuiz.BLL.Models
{
    public class QuizPlayerAnswer
    {
        public int QuizPlayerId { get; set; }
        public string AnswerContent { get; set; }

        // In milliseconds
        public long AnswerTime { get; set; }
    }
}
