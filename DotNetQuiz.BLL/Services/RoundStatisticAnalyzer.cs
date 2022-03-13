using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services;

public class RoundStatisticAnalyzer : IRoundStatisticAnalyzer
{
    public RoundStatistic BuildRoundStatistic(QuizRound quizRound, QuizConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(quizRound, nameof(quizRound));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var answerStatistic = this.CalculateAnswerStatistic(quizRound.Answers, configuration.AnswerIgnoreCase);
        var averageAnswerTime = CalculateAverageAnswerTime(quizRound.Answers);

        return new RoundStatistic()
        {
            AverageAnswerTime = TimeSpan.FromMilliseconds(averageAnswerTime),
            AnswerStatistic = answerStatistic,
        };
    }

    private double CalculateAverageAnswerTime(IEnumerable<QuizPlayerAnswer> answers)
    {
        if(!answers.Any()) return 0;

        return answers.Select(a => a.AnswerTime).Average();
    }

    private IEnumerable<KeyValuePair<string, int>> CalculateAnswerStatistic(IEnumerable<QuizPlayerAnswer> answers, bool ignoreCase)
    {   if(!answers.Any()) return new Dictionary<string, int>();

        var answersStatistic = answers.GroupBy(a =>
                ignoreCase ? a.AnswerContent.ToLowerInvariant() : a.AnswerContent)
            .ToDictionary(k => k.Key, v => v.Count());

        return answersStatistic;
    }
}

