using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Services
{
    public class QuizSessionService: IQuizSessionService
    {
        private const string SessionIsNotStartedErrorMessage = "Session is not established!";
        private const string SessionAlreadyStartedErrorMessage = "Session is already started!";

        private QuizSession quizSession;
        private QuizConfiguration configuration;
        private Dictionary<int, QuizPlayer> sessionPlayers;
        private IQuestionHandler questionHandler;
        private IRoundStatisticAnalyzer roundStatisticAnalyzer;
        private SessionState sessionState = SessionState.NotStarted;

        public QuizSessionService(IQuestionHandler questionHandler, IRoundStatisticAnalyzer roundStatisticAnalyzer)
        {
            ArgumentNullException.ThrowIfNull(questionHandler, nameof(questionHandler));
            ArgumentNullException.ThrowIfNull(roundStatisticAnalyzer, nameof(roundStatisticAnalyzer));

            this.questionHandler = questionHandler;
            this.roundStatisticAnalyzer = roundStatisticAnalyzer;
        }

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

            if (this.sessionPlayers.TryAdd(playerId,
                    new QuizPlayer { PlayerId = playerId, PlayerNickName = playerNickName }))
            {
                throw new ArgumentException($"Player with id [{playerId}] already exists");
            }
        }

        public void RemovePlayerFromSession(int playerId)
        {
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            if (!this.sessionPlayers.Remove(playerId))
            {
                throw new ArgumentException("Player with current id doesn't exist");
            }
        }

        public RoundStatistic GetRoundStatistic(QuizRound round) => this.quizSession.BuildRoundStatistic(round);

        public void StartGame()
        {
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            this.sessionState = SessionState.Running;
            this.quizSession = new QuizSession(this.configuration, this.sessionPlayers.Values.AsEnumerable(),
                this.questionHandler, this.roundStatisticAnalyzer);
        }

        public void SubmitAnswer(QuizPlayerAnswer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));
            this.ValidateSessionState(SessionState.Running, SessionIsNotStartedErrorMessage);

            this.quizSession.SubmitAnswer(answer);
        }

        public QuizRound NextRound()
        {
            this.ValidateSessionState(SessionState.Running, SessionIsNotStartedErrorMessage);

            return this.quizSession.NextRound();
        }

        private void ValidateSessionState(SessionState expectedResult, string errorMessage)
        {
            if (this.sessionState == expectedResult)
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
