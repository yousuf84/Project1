using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using InternationalTalentTest;
using System.Collections.Generic;
using System.Configuration;
using DataAccess.Models;

namespace DataAccess
{
    internal class QuestionDataAccess : DataAccessBase
    {
        protected override string ConnectionString { get { return ConfigurationManager.ConnectionStrings["TalentTestConnectionString"].ConnectionString; } }
        public List<Question> Get(int iClass)
        {
          return GetQuestionList(ExecuteSelect("select * from questions where class = @Class", new SqlParameter[]{
                     new SqlParameter("@Class", iClass)}, CommandType.Text));
        }

        public List<Question> GetQuestionList(DataTable dt)
        {
            List<Question> questions = new List<Question>();
            foreach (DataRow dr in dt.Rows)
            {
                questions.Add(new Question { QuestionID = (int)dr["QuestionID"], Text = dr["Text"].ToString(), Class = (Byte)dr["Class"],
                                             QuestionChoices = GetChoice((int)dr["QuestionID"]),
                                             QuestionSolution = GetSolution((int)dr["QuestionID"])
                });
            }
            return questions;
        }
        public List<QuestionChoice> GetChoice(int iQuestionID)
        {
            return GetQuestionChoices(ExecuteSelect("select * from questionchoices where questionid = @questionid", new SqlParameter[]{
                     new SqlParameter("@questionid", iQuestionID)}, CommandType.Text));

        }

        public List<QuestionChoice> GetQuestionChoices(DataTable dt)
        {
            List<QuestionChoice> questionChoices = new List<QuestionChoice>();
            foreach (DataRow dr in dt.Rows)
            {
                questionChoices.Add(new QuestionChoice { Text = dr["Text"].ToString(), Choice = dr["Choice"].ToString() });
            }
            return questionChoices;
        }

        public QuestionSolution GetSolution(int iQuestionID)
        {
          DataTable dt = ExecuteSelect("select * from QuestionSolutions where questionid = @questionid", new SqlParameter[]{
                     new SqlParameter("@questionid", iQuestionID)}, CommandType.Text);
          return new QuestionSolution { Choice = dt.Rows[0]["Choice"].ToString(), Solution = dt.Rows[0]["Solution"].ToString() };
        }

    }
}
