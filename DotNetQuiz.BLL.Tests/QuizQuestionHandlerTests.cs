using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Services;
using NUnit.Framework;

using static DotNetQuiz.BLL.Tests.Helpers.ArrayHelper;

namespace DotNetQuiz.BLL.Tests
{
    [TestFixture]
    internal class QuizQuestionHandlerTests
    {
        private readonly QuizQuestion[] quizQuestions =
        {
            new() {QuestionId = 1},
            new() {QuestionId = 2},
            new() {QuestionId = 3},
            new() {QuestionId = 4},
            new() {QuestionId = 5},
        };

        private QuizQuestionsHandler questionsHandler;

        [SetUp]
        public void Setup()
        {
            this.questionsHandler = new QuizQuestionsHandler(quizQuestions);
        }

        [Test]
        public void Ctor_NullQuizQuestion_ThrowArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new QuizQuestionsHandler(null!));

        [Test]
        public void NextQuestion_DefaultComparison_ReturnQuestionWithLowestId()
        {
            var currentQuestion = this.questionsHandler.NextQuestion();
            var expectedResult = this.quizQuestions.First((qq) =>
                qq.QuestionId == quizQuestions.GetLowest(Comparer<QuizQuestion>.Default).QuestionId);

            Assert.That(currentQuestion, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CurrentQuestion_DefaultComparison_ReturnQuestionWithLowestId()
        {
            var currentQuestion = this.questionsHandler.CurrentQuestion;
            var expectedResult = this.quizQuestions.First((qq) =>
                qq.QuestionId == quizQuestions.GetLowest(Comparer<QuizQuestion>.Default).QuestionId);

            Assert.That(currentQuestion, Is.EqualTo(expectedResult));
        }
    }
}
