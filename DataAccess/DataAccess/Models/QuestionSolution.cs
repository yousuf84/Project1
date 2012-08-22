using System;
using System.Linq;

namespace DataAccess.Models
{
    [Serializable]
    public class QuestionSolution
    {
        public string Choice { get; set; }
        public string Solution { get; set; }
    }
}
