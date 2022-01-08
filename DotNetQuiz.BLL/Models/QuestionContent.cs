using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetQuiz.BLL.Models
{
    public class QuestionContent
    {
        public string? QuestionText { get; set; }

        public byte[]? QuestionBlob { get; set; }
    }
}
