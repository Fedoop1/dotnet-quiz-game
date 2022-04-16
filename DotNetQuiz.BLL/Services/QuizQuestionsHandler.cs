using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services;

public class QuizQuestionsHandler: IQuestionHandler
{
    public QuizQuestion? NextQuestion(IEnumerable<QuizQuestion> questions)
    {
        ArgumentNullException.ThrowIfNull(questions);

        var uncompletedQuestions = questions.Where(q => !q.IsCompleted).ToArray();
        if (uncompletedQuestions.Length == 0) return null;

        Array.Sort(uncompletedQuestions, (lhs, rhs) => lhs.QuestionId - rhs.QuestionId);

        return uncompletedQuestions.First();
    }
}

