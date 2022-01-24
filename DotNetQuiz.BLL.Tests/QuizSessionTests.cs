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

        this.questionHandlerMock.Setup(qh => qh.CurrentQuestion).Returns(() => DefaultQuestionPack.Questions.First());
        this.questionHandlerMock.Setup(qh => qh.NextQuestion()).Returns(() => DefaultQuestionPack.Questions.First());

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
                new QuizPlayer[] { new() { PlayerId = 1 }, new() { PlayerId = 1 } },
                questionHandlerMock.Object, roundStatisticAnalyzerMock.Object));

    [Test]
    public void BuildRoundStatistic_NullQuizRound_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => defaultQuizSession.BuildRoundStatistic(null!));

    [Test]
    public void BuildRoundStatistic_CallRoundStatisticAnalyzerOnce()
    {
        this.defaultQuizSession.BuildRoundStatistic(new());
        this.roundStatisticAnalyzerMock.Verify(qs => qs.BuildRoundStatistic(It.IsAny<QuizRound>()), () => Times.Once());
    }

    [Test]
    public void SubmitAnswer_NullQuizPlayerAnswer_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.defaultQuizSession.SubmitAnswer(null!));


    [TestCase( 0, 1, 0, 0, "wrongAnswer")]
    [TestCase(0, 0, 100, 1, "rightAnswer")]
    public void SubmitAnswer_TestCases(int playerInitialScore, int playerInitialStreak, int expectedPlayerScore, int expectedPlayerStreak, string questionAnswer)
    {
        QuizPlayer player = new()
            { PlayerId = 1, PlayerNickName = "name", PlayerScore = playerInitialScore, PlayerStreak = playerInitialStreak };

        QuizPlayerAnswer wrongAnswer = new() { AnswerContent = questionAnswer, QuizPlayerId = 1 };

        QuizSession session = new(
            DefaultQuizConfiguration,
            new[]
            {
                player
            },
            questionHandlerMock.Object,
            roundStatisticAnalyzerMock.Object
        );

        // This step is necessary to set first question as current
        session.NextRound();
        session.SubmitAnswer(wrongAnswer);

        Assert.Multiple(() =>
        {
            Assert.That(() => player.PlayerScore, Is.EqualTo(expectedPlayerScore), () => "The Player's score must match the expected result");
            Assert.That(() => player.PlayerStreak, Is.EqualTo(expectedPlayerStreak), () => "The Player's streak must equals expected result");
        });
    }

    [Test]
    public void NextRound_LeftQuestionIsEqualsZero_ThrowsArgumentOutOfRangeException()
    {
        // It's need to set current question e.g last question (in this configuration)
        this.defaultQuizSession.NextRound();

        Assert.Throws<ArgumentOutOfRangeException>(() => this.defaultQuizSession.NextRound(),
            "There are no more questions.");
    }

    [Test]
    public void NextRound_CreateNewRoundAndSetNextQuestion_SessionCurrrentRoundQuestionEqualsQuestionHandlerCurrentQuestion()
    {
        var expectedResult = this.questionHandlerMock.Object.CurrentQuestion;

        this.defaultQuizSession.NextRound();

        Assert.That(() => this.defaultQuizSession.CurrentRound.CurrentQuestion, Is.EqualTo(expectedResult));
    }
}

