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
    public void AddPlayerToSession_PlayerWithSameIdAlreadyExists_ThrowsArgumentException()
    {
        const string playerId = "id";

        this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" });

        Assert.Throws<ArgumentException>(
            () => this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" }),
            $"Player with id [{playerId}] already exists");
    }

    [Test]
    public void AddPlayerToSession_NullQuizPlayer_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.AddPlayerToSession(null!));

    [Test]
    public void AddPlayerToSession_PlayerHasEmptyId_ThrowsArgumentException() => Assert.Throws<ArgumentException>(
        () => this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = "", NickName = "nickname"}),
        "Player id can't be null or empty");

    [Test]
    public void AddPlayerToSession_PlayerHasEmptyNickName_ThrowsArgumentException() => Assert.Throws<ArgumentException>(
        () => this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = "id", NickName = ""}),
        "Player name can't be null or empty");

    [Test]
    public void AddPlayerToSession_SessionPlayersContainsNewPlayer()
    {
        const string playerId = "id";

        this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" });

        Assert.That(this.sessionHandler.SessionPlayers.Any(sp => sp.Id == playerId), Is.True);
    }

    [Test]
    public void UploadQuizConfiguration_NullQuizConfiguration_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.UploadQuizConfiguration(null!));

    [Test]
    public void RemovePlayerFromSession_PlayerWithIdDoesntExist_ThrowsArgumentException() =>
        Assert.Throws<ArgumentException>(() => this.sessionHandler.RemovePlayerFromSession("id"),
            "Player with id [id] doesn't exist");

    [Test]
    public void RemovePlayerFromSession_SessionPlayersDoesntContainRemovedPlayer()
    {
        const string playerId = "id";

        this.sessionHandler.AddPlayerToSession(new QuizPlayer() {Id = playerId, NickName = "nickname" });
        this.sessionHandler.RemovePlayerFromSession(playerId);

        Assert.That(this.sessionHandler.SessionPlayers.Any(sp => sp.Id == playerId), Is.False);
    }

    [Test]
    public void SubmitAnswer_NullQuizAnswer_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.SubmitAnswer(null!));

    [Test]
    public void SubmitAnswer_SessionIsNotStarted_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.SubmitAnswer(new QuizPlayerAnswer()));

    [Test]
    public void NextRound_SessionIsNotStarted_ThrowArgumentNullException() => Assert.Throws<ArgumentNullException>(
        () => this.sessionHandler.NextRound());

    [Test]
    public void BuildCurrentRoundStatistic_SessionIsNotStarted_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.sessionHandler.BuildCurrentRoundStatistic());

    [Test]
    public void StartGame_GameIsAlreadyStarted_ThrowsArgumentExceptions() => Assert.Throws<ArgumentException>(() =>
    {
        this.sessionHandler.StartGame();
        this.sessionHandler.StartGame();
    }, "The game is already started");

    [Test]
    public void StartGame_CurrentRoundIsNotNull()
    {
        this.sessionHandler.StartGame();
        Assert.That(this.sessionHandler.CurrentRound, Is.Not.Null);
    }

    [Test]
    public void StartGame_ConfigurationIsNull_ThrowsArgumentExceptions() => Assert.Throws<ArgumentNullException>(() =>
            new QuizSessionHandler(this.questionHandlerMock.Object, this.roundStatisticAnalyzerMock.Object).StartGame());
}


