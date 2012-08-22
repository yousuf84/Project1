using System;
using System.Linq;

namespace DataAccess.Models
{
   [Serializable]
   public class QuestionChoice
    {
       public string Choice { get; set; }
       public string Text { get; set; }
    }
}
