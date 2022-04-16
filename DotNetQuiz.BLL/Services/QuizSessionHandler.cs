using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services
{
    public class QuizSessionHandler: IQuizSessionHandler
    {
        private readonly Dictionary<string, QuizPlayer> sessionPlayers = new();
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;

        private QuizSession? quizSession;
        private QuizConfiguration? configuration;

        public QuizSessionHandler(IQuestionHandler questionHandler, IRoundStatisticAnalyzer roundStatisticAnalyzer)
        {
            ArgumentNullException.ThrowIfNull(questionHandler, nameof(questionHandler));
            ArgumentNullException.ThrowIfNull(roundStatisticAnalyzer, nameof(roundStatisticAnalyzer));

            this.questionHandler = questionHandler;
            this.roundStatisticAnalyzer = roundStatisticAnalyzer;

            this.SessionId = Guid.NewGuid();
        }

        public Guid SessionId { get; }
        public QuizConfiguration? QuizConfiguration => this.configuration;
        public QuizRound? CurrentRound { get; private set; }
        public IReadOnlyCollection<QuizPlayer> SessionPlayers => this.sessionPlayers.Values;

        public bool IsOpen => this.quizSession is null;

        public void UploadQuizConfiguration(QuizConfiguration quizConfiguration)
        {
            ArgumentNullException.ThrowIfNull(quizConfiguration, nameof(quizConfiguration));

            this.configuration = quizConfiguration;
        }

        public void AddPlayerToSession(QuizPlayer quizPlayer)
        {
            ArgumentNullException.ThrowIfNull(quizPlayer, nameof(quizPlayer));
            ValidateQuizPlayer(quizPlayer);

            if (!this.sessionPlayers.TryAdd(quizPlayer.Id, quizPlayer))
            {
                throw new ArgumentException($"Player with id [{quizPlayer.Id}] already exists");
            }
        }

        public void RemovePlayerFromSession(string Id)
        {
            if (!this.sessionPlayers.Remove(Id))
            {
                throw new ArgumentException($"Player with id [{Id}] doesn't exist");
            }
        }

        public RoundStatistic BuildCurrentRoundStatistic()
        {
            ArgumentNullException.ThrowIfNull(this.quizSession);
            return this.quizSession.BuildCurrentRoundStatistic();
        }

        public void StartGame()
        {
            ArgumentNullException.ThrowIfNull(this.configuration);
            if (this.quizSession is not null) throw new ArgumentException("The game is already started");

            this.quizSession = new QuizSession(this.configuration!, this.sessionPlayers.Values.AsEnumerable(),
                this.questionHandler, this.roundStatisticAnalyzer);

            this.CurrentRound = this.quizSession.CurrentRound;
        }

        public void SubmitAnswer(QuizPlayerAnswer answer)
        {
            ArgumentNullException.ThrowIfNull(this.quizSession);
            this.quizSession.SubmitAnswer(answer ??
                                           throw new ArgumentNullException(nameof(answer), "Answer can't be null"));
        }
            

        public void NextRound()
        {
            ArgumentNullException.ThrowIfNull(this.quizSession);
            this.CurrentRound = this.quizSession.NextRound();
        }

        public void StartRound()
        {
            ArgumentNullException.ThrowIfNull(this.quizSession);
            this.quizSession.StartRound();
        }

        private static void ValidateQuizPlayer(QuizPlayer quizPlayer)
        {
            if (string.IsNullOrEmpty(quizPlayer.Id))
            {
                throw new ArgumentException("Player id can't be null or empty");
            }

            if (string.IsNullOrEmpty(quizPlayer.NickName))
            {
                throw new ArgumentException("Player name can't be null or empty");
            }
        }
    }
}
