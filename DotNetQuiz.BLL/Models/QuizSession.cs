using DotNetQuiz.BLL.Interfaces;

namespace DotNetQuiz.BLL.Models
{
    public class QuizSession
    {
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;
        private readonly QuizConfiguration quizConfiguration;
        private readonly Dictionary<int, QuizPlayer> quizPlayers = new ();
        private int questionsLeft;

        public QuizRound CurrentRound { get; private set; }

        public QuizSession(QuizConfiguration quizConfiguration, IEnumerable<QuizPlayer> quizPlayers, IQuestionHandler questionHandler, IRoundStatisticAnalyzer roundStatisticAnalyzer)
        {
            ArgumentNullException.ThrowIfNull(quizConfiguration, nameof(quizConfiguration));
            ArgumentNullException.ThrowIfNull(quizPlayers, nameof(quizPlayers));
            ArgumentNullException.ThrowIfNull(questionHandler, nameof(questionHandler));
            ArgumentNullException.ThrowIfNull(roundStatisticAnalyzer, nameof(roundStatisticAnalyzer));

            this.quizConfiguration = quizConfiguration;
            this.questionHandler = questionHandler;
            this.roundStatisticAnalyzer = roundStatisticAnalyzer;

            this.UploadPlayers(quizPlayers);

            this.questionsLeft = this.quizConfiguration.QuestionPack.Questions.Count;
        }

        public RoundStatistic BuildRoundStatistic(QuizRound round)
        {
            ArgumentNullException.ThrowIfNull(round, nameof(round));

            return this.roundStatisticAnalyzer.BuildRoundStatistic(round);
        }

        public void SubmitAnswer(QuizPlayerAnswer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));

            this.ProcessPlayerAnswer(answer);
            (this.CurrentRound.Answers as IList<QuizPlayerAnswer>)!.Add(answer);
        }

        public QuizRound NextRound()
        {
            if (--questionsLeft < 0)
            {
                throw new ArgumentOutOfRangeException("There are no more questions.");
            }

            var nextQuestion = this.questionHandler.NextQuestion();
            var currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            this.CurrentRound = new QuizRound()
            {
                CurrentQuestion = nextQuestion, 
                StartAt = currentTime,
                EndAt = currentTime.Add(TimeSpan.FromSeconds(this.quizConfiguration.RoundDuration)),
                Answers = new List<QuizPlayerAnswer>()
            };

            return CurrentRound;
        }

        private void ProcessPlayerAnswer(QuizPlayerAnswer playerAnswer)
        {
            var player = this.quizPlayers[playerAnswer.QuizPlayerId];

            if (this.questionHandler.CurrentQuestion.Answer!.AnswerContent.Equals(playerAnswer.AnswerContent,
                    this.quizConfiguration.AnswerIgnoreCase
                        ? StringComparison.InvariantCultureIgnoreCase
                        : StringComparison.InvariantCulture))
            {
                player.PlayerStreak += 1;

                var timeBonus = this.quizConfiguration.TimeMultiplier *
                                (this.quizConfiguration.RoundDuration - playerAnswer.AnswerTime / 1000);

                var streakMultiplier = this.quizConfiguration.StreakMultiplier * player.PlayerStreak;

                player.PlayerScore += (int)(Math.Floor(this.questionHandler.CurrentQuestion.QuestionReward +
                    (timeBonus > 0 ? timeBonus : 0)) * (streakMultiplier >= 1
                        ? streakMultiplier
                        : 1));
                return;
            }

            // Wrong answer.
            player.PlayerStreak = 0;
        }

        private void UploadPlayers(IEnumerable<QuizPlayer> players)
        {
            foreach (var player in players)
            {
                if (this.quizPlayers.Count + 1 > this.quizConfiguration.MaxPlayers)
                {
                    throw new ArgumentOutOfRangeException(nameof(player),
                        "The count of players can't be more than set in configuration");
                }

                if(!this.quizPlayers.TryAdd(player.PlayerId, player))
                {
                    throw new ArgumentException($"Player with id = {player.PlayerId} already exists");
                }
            }
        }
    }
}
