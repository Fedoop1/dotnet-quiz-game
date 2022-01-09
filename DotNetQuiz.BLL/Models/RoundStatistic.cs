namespace DotNetQuiz.BLL.Models
{
    public class RoundStatistic
    {
        public TimeSpan AverageAnswerTime { get; init; }
        public IEnumerable<KeyValuePair<string, int>> AnswerStatistic { get; init; }
    }
}
