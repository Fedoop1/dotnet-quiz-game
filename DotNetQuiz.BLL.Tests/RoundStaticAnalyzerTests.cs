using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.BLL.Tests.TestData;
using NUnit.Framework;

namespace DotNetQuiz.BLL.Tests;

[TestFixture]
internal class RoundStaticAnalyzerTests
{
    private static readonly IRoundStatisticAnalyzer defaultRoundStatisticAnalyzer =
        new RoundStatisticAnalyzer();

    [Test]
    public void BuildRoundStatistic_NullQuizRound_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => defaultRoundStatisticAnalyzer.BuildRoundStatistic(null!, new QuizConfiguration()));

    [Test]
    public void BuildRoundStatistic_NullQuizConfiguration_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => defaultRoundStatisticAnalyzer.BuildRoundStatistic(new QuizRound(), null!));


    [TestCaseSource(typeof(RoundStatisticAnalyzerTestsData), nameof(RoundStatisticAnalyzerTestsData.QuizRounds))]
    public void BuildRoundStatistic_TestsCases(QuizRound quizRound, RoundStatistic expectedResult)
    {
        var actualResult =
            defaultRoundStatisticAnalyzer.BuildRoundStatistic(quizRound, QuizSessionTestsData.DefaultQuizConfiguration);

        // Verify that average answer time, answer keys, and count of specific answers is the same
        Assert.Multiple(() =>
        {
            Assert.That(() => actualResult.AverageAnswerTime, Is.EqualTo(expectedResult.AverageAnswerTime));
            CollectionAssert.AreEqual(((Dictionary<string, int>)expectedResult.AnswerStatistic).Keys, ((Dictionary<string, int>)actualResult.AnswerStatistic).Keys);
            CollectionAssert.AreEqual(((Dictionary<string, int>)expectedResult.AnswerStatistic).Values, ((Dictionary<string, int>)actualResult.AnswerStatistic).Values);
        });
    }
}

