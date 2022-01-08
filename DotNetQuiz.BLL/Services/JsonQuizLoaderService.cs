using System.Text.Json;
using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Models;

namespace DotNetQuiz.BLL.Services
{
    public class JsonQuizLoaderService : IQuizLoaderService
    {
        private readonly string questionContent;
        private readonly JsonSerializerOptions options;

        public JsonQuizLoaderService(string quizContent, JsonSerializerOptions? options)
        {
            ArgumentNullException.ThrowIfNull(questionContent, nameof(quizContent));

            this.questionContent = quizContent;
            this.options = options ?? new JsonSerializerOptions(JsonSerializerDefaults.General);
        }

        public QuizQuestionPack LoadPack() => JsonSerializer.Deserialize<QuizQuestionPack>(questionContent, options) ??
                                              throw new JsonException("An error happened while deserializing");
    }
}
