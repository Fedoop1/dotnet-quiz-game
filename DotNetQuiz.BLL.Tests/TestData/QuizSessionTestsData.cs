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
            new () { Id = "id1", NickName = "nickname1" },
            new () { Id = "id2", NickName = "nickname2" },
            new () { Id = "id3", NickName = "nickname3" },
            new () { Id = "id4", NickName = "nickname4" },
        };
    }
}
