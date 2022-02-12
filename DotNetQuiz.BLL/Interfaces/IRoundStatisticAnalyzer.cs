using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Interfaces
{
    public interface IRoundStatisticAnalyzer
    {
        RoundStatistic BuildRoundStatistic(QuizRound quizRound, QuizConfiguration quizConfiguration);
    }
}
