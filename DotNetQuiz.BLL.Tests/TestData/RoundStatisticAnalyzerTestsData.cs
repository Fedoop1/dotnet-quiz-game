using DotNetQuiz.BLL.Models;
using NUnit.Framework;

namespace DotNetQuiz.BLL.Tests.TestData;

internal class RoundStatisticAnalyzerTestsData
{
    public static IEnumerable<TestCaseData> QuizRounds()
    {
        yield return new TestCaseData(new QuizRound()
        {
            Answers = Array.Empty<QuizPlayerAnswer>()
        }, new RoundStatistic()
        {
            AnswerStatistic = new Dictionary<string, int>(),
            AverageAnswerTime = TimeSpan.FromMilliseconds(0),
        });

        yield return new TestCaseData(new QuizRound()
        {
            Answers = new QuizPlayerAnswer[]
            {
                new()
                {
                    AnswerContent = "answer",
                    AnswerTime = 1,
                },
                new()
                {
                    AnswerContent = "answer",
                    AnswerTime = 1,
                },
                new()
                {
                    AnswerContent = "answer",
                    AnswerTime = 1,
                },
                new()
                {
                    AnswerContent = "answer",
                    AnswerTime = 1,
                },
            }
        }, new RoundStatistic()
        {
            AnswerStatistic = new Dictionary<string, int>()
            {
                ["answer"] = 4,
            },
            AverageAnswerTime = TimeSpan.FromMilliseconds(1),
        });

        yield return new TestCaseData(new QuizRound()
        {
            Answers = new QuizPlayerAnswer[]
            {
                new()
                {
                    AnswerContent = "firstAnswer",
                    AnswerTime = 1,
                },
                new()
                {
                    AnswerContent = "secondAnswer",
                    AnswerTime = 2,
                },
                new()
                {
                    AnswerContent = "thirdAnswer",
                    AnswerTime = 3,
                },
                new()
                {
                    AnswerContent = "fourthAnswer",
                    AnswerTime = 4,
                },
            }
        }, new RoundStatistic()
        {
            AnswerStatistic = new Dictionary<string, int>()
            {
                ["firstAnswer"] = 1,
                ["secondAnswer"] = 1,
                ["thirdAnswer"] = 1,
                ["fourthAnswer"] = 1,
            },
            AverageAnswerTime = TimeSpan.FromMilliseconds(2.5),
        });
    }
}

