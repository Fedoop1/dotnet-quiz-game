namespace DotNetQuiz.BLL.Models
{
    public class QuizPlayerAnswer
    {
        public int QuizPlayerId { get; set; }
        public string AnswerContent { get; set; }
        public long AnswerTime { get; set; }
    }
}
