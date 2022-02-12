using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.BLL.Tests.TestData;
using Moq;
using NUnit.Framework;

namespace DotNetQuiz.BLL.Tests;

[TestFixture]
internal class QuizSessionHandlerTests
{
    private Mock<IQuestionHandler> questionHandlerMock;
    private Mock<IRoundStatisticAnalyzer> roundStatisticAnalyzerMock;
    private IQuizSessionHandler sessionHandler;

    [SetUp]
    public void Setup()
    {
        this.questionHandlerMock = new Mock<IQuestionHandler>();
        this.roundStatisticAnalyzerMock = new Mock<IRoundStatisticAnalyzer>();

        this.sessionHandler = new QuizSessionHandler(questionHandlerMock.Object, roundStatisticAnalyzerMock.Object);
        this.sessionHandler.UploadQuizConfiguration(QuizSessionTestsData.DefaultQuizConfiguration);
    }

    [Test]
    public void Ctor_NullQuizQuestionHandler_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => new QuizSessionHandler(null!, roundStatisticAnalyzerMock.Object));

    [Test]
    public void Ctor_NullRoundStatisticAnalyzer_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => new QuizSessionHandler(questionHandlerMock.Object, null!));

    [Test]
    public void AddPlayerToSession_SessionIsAlreadyStarted_ThrowsArgumentException()
    {
        this.sessionHandler.StartGame();

        Assert.Throws<ArgumentException>(() => this.sessionHandler.AddPlayerToSession(new QuizPlayer()),
            "Session is already started!");
    }

    [Test]
    public void AddPlayerToSession_PlayerWithSameIdAlreadyExists_ThrowsArgumentException()
    {
        const int playerId = 1;

        this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" });

        Assert.Throws<ArgumentException>(
            () => this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" }),
            $"Player with id [{playerId}] already exists");
    }

    [Test]
    public void AddPlayerToSession_SessionPlayersContainsNewPlayer()
    {
        const int playerId = 1;

        this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" });

        Assert.That(this.sessionHandler.SessionPlayers.Any(sp => sp.Id == playerId), Is.True);
    }

    [Test]
    public void UploadQuizConfiguration_NullQuizConfiguration_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.UploadQuizConfiguration(null!));

    [Test]
    public void RemovePlayerFromSession_PlayerWithIdDoesntExist_ThrowsArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionHandler.RemovePlayerFromSession(1),
            "Player with id [1] doesn't exist");

    [Test]
    public void RemovePlayerFromSession_SessionPlayersDoesntContainRemovedPlayer()
    {
        const int playerId = 1;

        this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" });
        this.sessionHandler.RemovePlayerFromSession(playerId);

        Assert.That(this.sessionHandler.SessionPlayers.Any(sp => sp.Id == playerId), Is.False);
    }

    [Test]
    public void SubmitAnswer_NullQuizAnswer_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.SubmitAnswer(null!));

    [Test]
    public void SubmitAnswer_SessionIsNotStarted_ThrowArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionHandler.SubmitAnswer(new QuizPlayerAnswer()),
            "Session is not established!");

    [Test]
    public void NextRound_SessionIsNotStarted_ThrowArgumentException() => Assert.Throws<ArgumentException>(
        () => this.sessionHandler.NextRound(),
        "Session is not established!");

    [Test]
    public void BuildCurrentRoundStatistic_SessionIsNotStarted_ThrowArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionHandler.BuildCurrentRoundStatistic());
}


