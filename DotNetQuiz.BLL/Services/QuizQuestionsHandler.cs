using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services;

public class QuizQuestionsHandler: IQuestionHandler
{
    public QuizQuestion? NextQuestion(IEnumerable<QuizQuestion> questions)
    {
        ArgumentNullException.ThrowIfNull(questions);

        var uncompletedQuestions = questions.Where(q => !q.isCompleted).ToArray();
        if (uncompletedQuestions.Length == 0) return null;

        Array.Sort(uncompletedQuestions);

        return uncompletedQuestions.First();
    }
}

