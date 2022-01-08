using DotNetQuiz.BLL.Interfaces;

namespace DotNetQuiz.BLL.Models
{
    public class QuizSession
    {
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;
        private readonly QuizConfiguration quizConfiguration;
        private readonly Dictionary<int, QuizPlayer> quizPlayers = new ();

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
        }
        public RoundStatistic BuildRoundStatistic() => this.roundStatisticAnalyzer.BuildRoundStatistic(this.CurrentRound);

        public void SubmitAnswer(QuizPlayerAnswer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));

            this.ProcessPlayerAnswer(answer);
            this.CurrentRound.Answers.Add(answer);
        }

        public QuizRound NextRound()
        {
            var nextQuestion = this.questionHandler.NextQuestion();
            var currentTime = DateTime.Now.Ticks;

            this.CurrentRound = new QuizRound()
            {
                CurrentQuestion = nextQuestion, 
                StartAt = new TimeOnly(currentTime),
                EndAt = new TimeOnly(currentTime + TimeSpan.FromSeconds(this.quizConfiguration.RoundDuration).Ticks)
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

                player.PlayerScore += (int)Math.Floor(this.questionHandler.CurrentQuestion.QuestionReward +
                    (timeBonus > 0 ? timeBonus : 0) * streakMultiplier >= 1
                        ? streakMultiplier
                        : 1);
                return;
            }

            // Wrong answer.
            player.PlayerStreak = 0;
        }

        private void UploadPlayers(IEnumerable<QuizPlayer> players)
        {
            foreach (var player in players)
            {
                this.quizPlayers[player.PlayerId] = player;
            }
        }
    }
}
