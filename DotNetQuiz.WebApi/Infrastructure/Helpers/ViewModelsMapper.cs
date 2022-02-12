using DotNetQuiz.BLL.Models;
using DotNetQuiz.WebApi.Models;

namespace DotNetQuiz.WebApi.Infrastructure.Helpers
{
    public static class ViewModelsMapper
    {
        public static QuizPlayer ToQuizPlayer(this QuizPlayerModel playerModel) =>
            new() {Id = playerModel.Id, NickName = playerModel.NickName};
    }
}
