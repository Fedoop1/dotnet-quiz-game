using System.ComponentModel.DataAnnotations;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizPlayerModel
    {
        [Required]
        public string Id { get; init; }

        [Required]
        public string NickName { get; init; }
    }
}
