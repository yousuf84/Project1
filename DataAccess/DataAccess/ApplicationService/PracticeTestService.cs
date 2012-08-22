using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;

namespace DataAccess.ApplicationService
{
   public  class PracticeTestService
    {
       public static List<Question> Get(int iClassID)
       {
           QuestionDataAccess questiondataaccess = new QuestionDataAccess();
           return questiondataaccess.Get(iClassID);
       }
    }
}
