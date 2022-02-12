using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services
{
    public class QuizQuestionsHandler: IQuestionHandler
    {
        private Queue<QuizQuestion> quizQuestions;

        public QuizQuestion CurrentQuestion => quizQuestions.Peek();

        public QuizQuestionsHandler(IEnumerable<QuizQuestion> quizQuestions)
        {
            ArgumentNullException.ThrowIfNull(quizQuestions, nameof(quizQuestions));

            InitQuestions(quizQuestions);
        }

        public QuizQuestion NextQuestion() => this.quizQuestions.Dequeue();

        private void InitQuestions(IEnumerable<QuizQuestion> questions)
        {
            var questionsArray = questions.ToArray();
            this.quizQuestions = new Queue<QuizQuestion>(questionsArray.Length);

            Array.Sort(questionsArray);

            foreach (var question in questionsArray)
            {
                this.quizQuestions.Enqueue(question);
            }
        }
    }
}
