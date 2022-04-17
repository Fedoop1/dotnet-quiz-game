using DotNetQuiz.BLL.Models;
using DotNetQuiz.BLL.Services;
using NUnit.Framework;
using static DotNetQuiz.BLL.Tests.Helpers.ArrayHelper;

namespace DotNetQuiz.BLL.Tests;

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

    private readonly QuizQuestionsHandler questionsHandler = new ();

    [Test]
    public void NextQuestion_NullQuestions_ThrowArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() => this.questionsHandler.NextQuestion(null!));

    [Test]
    public void NextQuestion_DefaultComparison_ReturnQuestionWithLowestId()
    {
        var quizQuestion = this.questionsHandler.NextQuestion(quizQuestions);

        var questionWithLowestId = quizQuestions.GetLowest((lhs, rhs) => lhs.QuestionId - rhs.QuestionId);

        Assert.That(quizQuestion!.QuestionId, Is.EqualTo(questionWithLowestId.QuestionId));
    }
}

