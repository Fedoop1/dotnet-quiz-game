namespace DotNetQuiz.BLL.Models
{
    public class QuizPlayer
    {
        public int PlayerId { get; init; }
        public string PlayerNickName { get; init; }
        public int PlayerStreak { get; set; }
        public int PlayerScore { get; set; }
    }
}
