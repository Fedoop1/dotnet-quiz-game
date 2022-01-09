using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.Client.Infrastructure.Services
{
    public class RoundStatisticAnalyzer : IRoundStatisticAnalyzer
    {
        private readonly QuizConfiguration configuration;
        public RoundStatisticAnalyzer(QuizConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            this.configuration = configuration;
        }

        public RoundStatistic BuildRoundStatistic(QuizRound quizRound)
        {
            ArgumentNullException.ThrowIfNull(quizRound, nameof(quizRound));

            var answerStatistic = this.CalculateAnswerStatistic(quizRound.Answers);
            var averageAnswerTime = CalculateAverageAnswerTime(quizRound.Answers);

            return new RoundStatistic()
            {
                AverageAnswerTime = new TimeSpan(averageAnswerTime),
                AnswerStatistic = answerStatistic,
            };
        }

        private long CalculateAverageAnswerTime(IEnumerable<QuizPlayerAnswer> answers) =>
            (long)Math.Floor(answers.Select(a => a.AnswerTime).Average());

        private IEnumerable<KeyValuePair<string, int>> CalculateAnswerStatistic(IEnumerable<QuizPlayerAnswer> answers)
        {
            var answersStatistic = answers.GroupBy(a =>
                    this.configuration.AnswerIgnoreCase ? a.AnswerContent.ToLowerInvariant() : a.AnswerContent)
                .ToDictionary(k => k.Key, v => v.Count());

            return answersStatistic;
        }
    }
}
