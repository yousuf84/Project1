using System;
using System.Linq;
using System.Collections.Generic;

namespace DataAccess.Models
{
    [Serializable]
  public  class Question
    {
      public int QuestionID { get; set; }
      public Byte Class { get; set; }
      public string Text { get; set; }
      public List<QuestionChoice> QuestionChoices { get; set; }
      public QuestionSolution QuestionSolution { get; set; }
    }
}
