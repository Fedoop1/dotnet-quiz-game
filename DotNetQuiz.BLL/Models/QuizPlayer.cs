namespace DotNetQuiz.BLL.Models
{
    public class QuizPlayer
    {
        public string Id { get; init; }
        public string NickName { get; init; }
        public int Streak { get; set; }
        public int Score { get; set; }
    }
}
