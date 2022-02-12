using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;

namespace DotNetQuiz.WebApi.Infrastructure.Services
{
    public class QuizSessionHandlersFactory : IQuizSessionHandlersFactory
    {
        private readonly IQuestionHandler questionHandler;
        private readonly IRoundStatisticAnalyzer roundStatisticAnalyzer;

        public QuizSessionHandlersFactory(IQuestionHandler questionHandler,
            IRoundStatisticAnalyzer roundStatisticAnalyzer) => (this.questionHandler, this.roundStatisticAnalyzer) =
            (questionHandler, roundStatisticAnalyzer);

        public IQuizSessionHandler CreateSessionHandler() =>
            new QuizSessionHandler(this.questionHandler, this.roundStatisticAnalyzer);
    }
}
