using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Services
{
    public class QuizSessionService: IQuizSessionService
    {
        private const string SessionIsNotStartedErrorMessage = "Session is not established!";
        private const string SessionAlreadyStartedErrorMessage = "Session is already started!";

        private readonly Dictionary<int, QuizPlayer> sessionPlayers = new();
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;

        private QuizSession quizSession;
        private QuizConfiguration configuration;

        public QuizSessionService(IQuestionHandler questionHandler, IRoundStatisticAnalyzer roundStatisticAnalyzer)
        {
            ArgumentNullException.ThrowIfNull(questionHandler, nameof(questionHandler));
            ArgumentNullException.ThrowIfNull(roundStatisticAnalyzer, nameof(roundStatisticAnalyzer));

            this.questionHandler = questionHandler;
            this.roundStatisticAnalyzer = roundStatisticAnalyzer;
        }

        public QuizRound CurrentRound { get; private set; }
        public IReadOnlyCollection<QuizPlayer> SessionPlayers => this.sessionPlayers.Values;
        public SessionState SessionState { get; private set; } = SessionState.NotStarted;

        public void UploadQuizConfiguration(QuizConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            this.configuration = configuration;
        }

        public void AddPlayerToSession(int playerId, string? playerNickName)
        {
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            playerNickName ??= "player" + playerId;

            if (!this.sessionPlayers.TryAdd(playerId,
                    new QuizPlayer { PlayerId = playerId, PlayerNickName = playerNickName }))
            {
                throw new ArgumentException($"Player with id [{playerId}] already exists");
            }
        }

        public void RemovePlayerFromSession(int playerId)
        {
            if (!this.sessionPlayers.Remove(playerId))
            {
                throw new ArgumentException($"Player with id [{playerId}] doesn't exist");
            }
        }

        public RoundStatistic BuildCurrentRoundStatistic()
        {
            this.ValidateSessionState(SessionState.Running, SessionIsNotStartedErrorMessage);

            return this.quizSession.BuildRoundStatistic(this.CurrentRound);
        }

        public void StartGame()
        {
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            this.SessionState = SessionState.Running;
            this.quizSession = new QuizSession(this.configuration, this.sessionPlayers.Values.AsEnumerable(),
                this.questionHandler, this.roundStatisticAnalyzer);
        }

        public void SubmitAnswer(QuizPlayerAnswer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));
            this.ValidateSessionState(SessionState.Running, SessionIsNotStartedErrorMessage);

            this.quizSession.SubmitAnswer(answer);
        }

        public void NextRound()
        {
            this.ValidateSessionState(SessionState.Running, SessionIsNotStartedErrorMessage);
            this.CurrentRound = this.quizSession.NextRound();
        }

        private void ValidateSessionState(SessionState expectedResult, string errorMessage)
        {
            if (this.SessionState != expectedResult)
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
