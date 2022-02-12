using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.BLL.Tests.TestData;
using Moq;
using NUnit.Framework;

namespace DotNetQuiz.BLL.Tests;

[TestFixture]
internal class QuizSessionServiceTests
{
    private Mock<IQuestionHandler> questionHandlerMock;
    private Mock<IRoundStatisticAnalyzer> roundStatisticAnalyzerMock;
    private QuizSessionService sessionService;

    [SetUp]
    public void Setup()
    {
        this.questionHandlerMock = new Mock<IQuestionHandler>();
        this.roundStatisticAnalyzerMock = new Mock<IRoundStatisticAnalyzer>();

        this.roundStatisticAnalyzerMock.Setup(mock => mock.BuildRoundStatistic(It.IsAny<QuizRound>()))
            .Returns(new RoundStatistic());

        this.questionHandlerMock.Setup(mock => mock.NextQuestion())
            .Returns(QuizSessionTestsData.DefaultQuestionPack.Questions.First());

        this.sessionService = new QuizSessionService(questionHandlerMock.Object, roundStatisticAnalyzerMock.Object);
        this.sessionService.UploadQuizConfiguration(QuizSessionTestsData.DefaultQuizConfiguration);
    }

    [Test]
    public void Ctor_NullQuizQuestionHandler_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => new QuizSessionService(null!, roundStatisticAnalyzerMock.Object));

    [Test]
    public void Ctor_NullRoundStatisticAnalyzer_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => new QuizSessionService(questionHandlerMock.Object, null!));

    [Test]
    public void AddPlayerToSession_SessionIsAlreadyStarted_ThrowsArgumentException()
    {
        this.sessionService.StartGame();

        Assert.Throws<ArgumentException>(() => this.sessionService.AddPlayerToSession(1, "player1"),
            "Session is already started!");
    }

    [Test]
    public void AddPlayerToSession_PlayerWithSameIdAlreadyExists_ThrowsArgumentException()
    {
        const int playerId = 1;

        this.sessionService.AddPlayerToSession(playerId, "player1");

        Assert.Throws<ArgumentException>(() => this.sessionService.AddPlayerToSession(playerId, "player2"), $"Player with id [{playerId}] already exists");
    }

    [Test]
    public void AddPlayerToSession_SessionPlayersContainsNewPlayer()
    {
        const int playerId = 1;

        this.sessionService.AddPlayerToSession(playerId, "player1");

        Assert.That(this.sessionService.SessionPlayers.Any(sp => sp.PlayerId == playerId), Is.True);
    }

    [Test]
    public void UploadQuizConfiguration_NullQuizConfiguration_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionService.UploadQuizConfiguration(null!));

    [Test]
    public void RemovePlayerFromSession_PlayerWithIdDoesntExist_ThrowsArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionService.RemovePlayerFromSession(1),
            "Player with id [1] doesn't exist");

    [Test]
    public void RemovePlayerFromSession_SessionPlayersDoesntContainRemovedPlayer()
    {
        const int playerId = 1;

        this.sessionService.AddPlayerToSession(playerId, "player1");
        this.sessionService.RemovePlayerFromSession(playerId);

        Assert.That(this.sessionService.SessionPlayers.Any(sp => sp.PlayerId == playerId), Is.False);
    }

    [Test]
    public void SubmitAnswer_NullQuizAnswer_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionService.SubmitAnswer(null!));

    [Test]
    public void SubmitAnswer_SessionIsNotStarted_ThrowArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionService.SubmitAnswer(new QuizPlayerAnswer()),
            "Session is not established!");

    [Test]
    public void NextRound_SessionIsNotStarted_ThrowArgumentException() => Assert.Throws<ArgumentException>(
        () => this.sessionService.NextRound(),
        "Session is not established!");

    [Test]
    public void NextRound_UpdateCurrentRound()
    {
        this.sessionService.StartGame();
        this.sessionService.NextRound();

        var expectedResult = QuizSessionTestsData.DefaultQuestionPack.Questions.First();

        Assert.That(this.sessionService.CurrentRound.CurrentQuestion, Is.EqualTo(expectedResult));
    }

    [Test]
    public void BuildCurrentRoundStatistic_SessionIsNotStarted_ThrowArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionService.BuildCurrentRoundStatistic());

    [Test]
    public void BuildCurrentRoundStatistic_ReturnCurrentRoundStatistic()
    {
        this.sessionService.StartGame();
        this.sessionService.NextRound();

        var roundStatistic = this.sessionService.BuildCurrentRoundStatistic();

        Assert.IsInstanceOf<RoundStatistic>(roundStatistic);
    }
}


