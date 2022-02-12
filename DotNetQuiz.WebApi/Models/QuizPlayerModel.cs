using System.ComponentModel.DataAnnotations;

namespace DotNetQuiz.WebApi.Models
{
    public class QuizPlayerModel
    {
        [Range(0, int.MaxValue)]
        public int Id { get; init; }

        [Required]
        public string NickName { get; init; }
    }
}
