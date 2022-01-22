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
            new () { PlayerId = 1, PlayerNickName = "nickname1" },
            new () { PlayerId = 2, PlayerNickName = "nickname2" },
            new () { PlayerId = 3, PlayerNickName = "nickname3" },
            new () { PlayerId = 4, PlayerNickName = "nickname4" },
        };
    }
}
