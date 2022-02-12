using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Models.enums;

namespace DotNetQuiz.BLL.Services
{
    public class QuizSessionHandler: IQuizSessionHandler
    {
        private const string SessionIsNotStartedErrorMessage = "Session is not established!";
        private const string SessionAlreadyStartedErrorMessage = "Session is already started!";

        private readonly Dictionary<int, QuizPlayer> sessionPlayers = new();
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;

        private QuizSession quizSession;
        private QuizConfiguration configuration;

        public QuizSessionHandler(IQuestionHandler questionHandler, IRoundStatisticAnalyzer roundStatisticAnalyzer)
        {
            ArgumentNullException.ThrowIfNull(questionHandler, nameof(questionHandler));
            ArgumentNullException.ThrowIfNull(roundStatisticAnalyzer, nameof(roundStatisticAnalyzer));

            this.questionHandler = questionHandler;
            this.roundStatisticAnalyzer = roundStatisticAnalyzer;

            this.QuizHandlerId = Guid.NewGuid();
        }

        public Guid QuizHandlerId { get; }
        public QuizRound CurrentSessionRound { get; private set; }
        public IReadOnlyCollection<QuizPlayer> SessionPlayers => this.sessionPlayers.Values;
        public SessionState SessionState { get; private set; } = SessionState.NotStarted;

        public void UploadQuizConfiguration(QuizConfiguration quizConfiguration)
        {
            ArgumentNullException.ThrowIfNull(quizConfiguration, nameof(quizConfiguration));
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            this.configuration = quizConfiguration;
        }

        public void AddPlayerToSession(QuizPlayer quizPlayer)
        {
            ArgumentNullException.ThrowIfNull(quizPlayer, nameof(quizPlayer));
            this.ValidateQuizPlayer(quizPlayer);
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            if (!this.sessionPlayers.TryAdd(quizPlayer.Id, quizPlayer))
            {
                throw new ArgumentException($"Player with id [{quizPlayer.Id}] already exists");
            }
        }

        public void RemovePlayerFromSession(int Id)
        {
            if (!this.sessionPlayers.Remove(Id))
            {
                throw new ArgumentException($"Player with id [{Id}] doesn't exist");
            }
        }

        public RoundStatistic BuildCurrentRoundStatistic()
        {
            this.ValidateSessionState(SessionState.Running, SessionIsNotStartedErrorMessage);

            return this.quizSession.BuildCurrentRoundStatistic();
        }

        public void StartGame()
        {
            this.ValidateSessionState(SessionState.NotStarted, SessionAlreadyStartedErrorMessage);

            this.SessionState = SessionState.Running;

            this.quizSession = new QuizSession(this.configuration, this.sessionPlayers.Values.AsEnumerable(),
                this.questionHandler, this.roundStatisticAnalyzer);

            this.CurrentSessionRound = this.quizSession.CurrentRound;
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

            this.CurrentSessionRound = this.quizSession.NextRound();
        }

        private void ValidateSessionState(SessionState expectedResult, string errorMessage)
        {
            if (this.SessionState != expectedResult)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        private void ValidateQuizPlayer(QuizPlayer quizPlayer)
        {
            if (string.IsNullOrEmpty(quizPlayer.NickName))
            {
                throw new ArgumentException("Player name can't be null or empty");
            }

            if (quizPlayer.Id == default || quizPlayer.Id < 0)
            {
                throw new ArgumentException("Player name can't negative or equals zero");
            }
        }
    }
}
