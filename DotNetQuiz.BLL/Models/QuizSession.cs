using DotNetQuiz.BLL.Interfaces;

namespace DotNetQuiz.BLL.Models
{
    public class QuizSession
    {
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;
        private readonly QuizConfiguration quizConfiguration;
        private readonly Dictionary<string, QuizPlayer> quizPlayers = new ();

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

            this.questionsLeft = this.quizConfiguration.QuestionPack?.Questions?.Count() ?? 0;
            this.CurrentRound = NextRound();
        }

        public RoundStatistic BuildCurrentRoundStatistic() =>
            this.roundStatisticAnalyzer.BuildRoundStatistic(this.CurrentRound, this.quizConfiguration);

        public void SubmitAnswer(QuizPlayerAnswer answer)
        {
            ArgumentNullException.ThrowIfNull(answer, nameof(answer));
            ArgumentNullException.ThrowIfNull(this.CurrentRound.StartAt, nameof(answer));

            if (DateTime.Now > this.CurrentRound.EndAt) throw new ArgumentException("Answer can't be processed. Time's up");

            this.ProcessPlayerAnswer(answer);
            (this.CurrentRound.Answers as IList<QuizPlayerAnswer>)!.Add(answer);
        }

        public QuizRound NextRound()
        {
            if (this.CurrentRound is not null)
            {
                this.CurrentRound.CurrentQuestion.IsCompleted = true;
            }

            if (--questionsLeft < 0)
            {
                throw new ArgumentOutOfRangeException("There are no more questions.");
            }

            var nextQuestion = this.questionHandler.NextQuestion(this.quizConfiguration.QuestionPack?.Questions);
            

            this.CurrentRound = new QuizRound()
            {
                StartAt = null,
                EndAt = null,
                CurrentQuestion = nextQuestion,           
                Answers = new List<QuizPlayerAnswer>()
            };

            return CurrentRound;
        }

        public void StartRound()
        {
            if(this.CurrentRound.StartAt != default || this.CurrentRound.EndAt != default)
            {
                throw new ArgumentException("Round is already started");
            }

            this.CurrentRound.StartAt = DateTime.Now;
            this.CurrentRound.EndAt = this.CurrentRound.StartAt.Value
                .Add(TimeSpan.FromSeconds(this.quizConfiguration.RoundDuration));
        }

        private void ProcessPlayerAnswer(QuizPlayerAnswer playerAnswer)
        {
            if (!this.quizPlayers.TryGetValue(playerAnswer.PlayerId, out var player))
            {
                throw new ArgumentException($"Player with id [{player.Id}] doesn't exist");
            }

            if (this.CurrentRound.CurrentQuestion.Answer!.AnswerContent.Equals(playerAnswer.AnswerContent,
                    this.quizConfiguration.AnswerIgnoreCase
                        ? StringComparison.InvariantCultureIgnoreCase
                        : StringComparison.InvariantCulture))
            {
                player.Streak += 1;

                var timeBonus = this.quizConfiguration.TimeMultiplier *
                                (this.quizConfiguration.RoundDuration - playerAnswer.AnswerTime / 1000);

                var streakMultiplier = this.quizConfiguration.StreakMultiplier * player.Streak;

                player.Score += (int)(Math.Floor(this.CurrentRound.CurrentQuestion.QuestionReward +
                    (timeBonus > 0 ? timeBonus : 0)) * (streakMultiplier >= 1
                        ? streakMultiplier
                        : 1));
                return;
            }

            // Wrong answer.
            player.Streak = 0;
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

                if(!this.quizPlayers.TryAdd(player.Id, player))
                {
                    throw new ArgumentException($"Player with id [{player.Id}] already exists");
                }
            }
        }
    }
}
