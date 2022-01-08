using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services
{
    internal class QuizQuestionsHandler: IQuestionHandler
    {
        public QuizQuestion CurrentQuestion => quizQuestions.Peek();
        private Queue<QuizQuestion> quizQuestions;

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
