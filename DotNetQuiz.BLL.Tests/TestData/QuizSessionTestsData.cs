using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Tests.TestData;

internal static class QuizSessionTestsData
{
    internal static readonly QuizQuestionPack DefaultQuestionPack;
    internal static readonly QuizConfiguration DefaultQuizConfiguration;
    internal static readonly QuizPlayer[] QuizPlayers;

    static QuizSessionTestsData()
    {
        DefaultQuestionPack = new()
        {
            QuestionPackId = 1,
            Questions = new QuizQuestion[]
            {
                new()
                {
                    QuestionId = 1,
                    Answer = new QuestionAnswer()
                    {
                        AnswerContent = "rightAnswer"
                    },
                    QuestionReward = 100
                }
            }
        };

        DefaultQuizConfiguration = new()
        {
            QuestionPack = DefaultQuestionPack,
        };

        QuizPlayers = new QuizPlayer[] 
        {
            new () { Id = 1, NickName = "nickname1" },
            new () { Id = 2, NickName = "nickname2" },
            new () { Id = 3, NickName = "nickname3" },
            new () { Id = 4, NickName = "nickname4" },
        };
    }
}
