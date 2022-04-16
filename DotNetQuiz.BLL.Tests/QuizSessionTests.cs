using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using Moq;
using NUnit.Framework;

using static DotNetQuiz.BLL.Tests.TestData.QuizSessionTestsData;

namespace DotNetQuiz.BLL.Tests;

[TestFixture]
public class QuizSessionTests
{
    private Mock<IQuestionHandler> questionHandlerMock;
    private Mock<IRoundStatisticAnalyzer> roundStatisticAnalyzerMock;
    
    private QuizSession defaultQuizSession;

    [SetUp]
    public void Setup()
    {
        this.questionHandlerMock = new Mock<IQuestionHandler>();

        this.questionHandlerMock.Setup(mock => mock.NextQuestion(It.IsAny<IEnumerable<QuizQuestion>>()))
            .Returns(DefaultQuizConfiguration.QuestionPack!.Questions!.First());

        this.roundStatisticAnalyzerMock = new Mock<IRoundStatisticAnalyzer>();
        this.defaultQuizSession = new QuizSession(DefaultQuizConfiguration, QuizPlayers, questionHandlerMock.Object,
            roundStatisticAnalyzerMock.Object); 
    }

    [Test]
    public void Ctor_NullQuizConfiguration_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(
        () => new QuizSession(null!, QuizPlayers, questionHandlerMock.Object, roundStatisticAnalyzerMock.Object));

    [Test]
    public void Ctor_NullQuizPlayers_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(
        () => new QuizSession(DefaultQuizConfiguration, null!, questionHandlerMock.Object, roundStatisticAnalyzerMock.Object));

    [Test]
    public void Ctor_NullQuestionHandler_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(
        () => new QuizSession(DefaultQuizConfiguration, QuizPlayers, null!, roundStatisticAnalyzerMock.Object));

    [Test]
    public void Ctor_NullRoundStatisticAnalyzer_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(
        () => new QuizSession(DefaultQuizConfiguration, QuizPlayers, questionHandlerMock.Object, null!));

    [Test]
    public void Ctor_CountOfPlayersMoreThanSetInConfiguration_ThrowsArgumentOutOfRangeException() =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new QuizSession(new QuizConfiguration() { MaxPlayers = 0 }, new QuizPlayer[] { new() },
                questionHandlerMock.Object, roundStatisticAnalyzerMock.Object),
            "The count of players can't be more than set in configuration");

    [Test]
    public void Ctor_AddPlayerWithSameId_ThrowsArgumentException() =>
        Assert.Throws<ArgumentException>(
            () => new QuizSession(DefaultQuizConfiguration,
                new QuizPlayer[] { new() { Id = "id" }, new() { Id = "id" } },
                questionHandlerMock.Object, roundStatisticAnalyzerMock.Object));

    [Test]
    public void BuildRoundStatistic_CallRoundStatisticAnalyzerOnce()
    {
        this.defaultQuizSession.BuildCurrentRoundStatistic();
        this.roundStatisticAnalyzerMock.Verify(
            qs => qs.BuildRoundStatistic(It.IsAny<QuizRound>(), It.IsAny<QuizConfiguration>()), () => Times.Once());
    }

    [Test]
    public void SubmitAnswer_NullQuizPlayerAnswer_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() =>
        {
            this.defaultQuizSession.StartRound();
            this.defaultQuizSession.SubmitAnswer(null!);
        });


    [TestCase( 0, 1, 0, 0, "wrongAnswer")]
    [TestCase(0, 0, 100, 1, "rightAnswer")]
    public void SubmitAnswer_TestCases(int playerInitialScore, int playerInitialStreak, int expectedScore, int expectedStreak, string questionAnswer)
    {
        QuizPlayer player = new()
            { Id = "id", NickName = "name", Score = playerInitialScore, Streak = playerInitialStreak };

        QuizPlayerAnswer wrongAnswer = new() { AnswerContent = questionAnswer, PlayerId = "id" };

        QuizSession session = new(
            DefaultQuizConfiguration,
            new[]
            {
                player
            },
            questionHandlerMock.Object,
            roundStatisticAnalyzerMock.Object
        );

        session.StartRound();
        session.SubmitAnswer(wrongAnswer);

        Assert.Multiple(() =>
        {
            Assert.That(() => player.Score, Is.EqualTo(expectedScore), () => "The Player's score must match the expected result");
            Assert.That(() => player.Streak, Is.EqualTo(expectedStreak), () => "The Player's streak must equals expected result");
        });
    }

    [Test]
    public void NextRound_LeftQuestionIsEqualsZero_ThrowsArgumentOutOfRangeException() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => this.defaultQuizSession.NextRound(),
            "There are no more questions.");

    [Test]
    public void StartRound_RoundIsAlreadyStarted_ThrowArgumentException() =>
        Assert.Throws<ArgumentException>(() =>
            {
                this.defaultQuizSession.StartRound();
                this.defaultQuizSession.StartRound();
            },
            "Round is already started");

    [Test]
    public void StartRound_CurrentRoundStartAndEndTimeIsUpdated()
    {
        this.defaultQuizSession.StartRound();

        Assert.Multiple(() =>
        {
            Assert.That(this.defaultQuizSession.CurrentRound.StartAt, Is.Not.Null);
            Assert.That(this.defaultQuizSession.CurrentRound.EndAt, Is.Not.Null);
        });
    }
}

