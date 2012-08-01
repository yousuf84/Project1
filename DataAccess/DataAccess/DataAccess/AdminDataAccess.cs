using System;
using System.Linq;
using System.Data;
using System.Configuration;
using InternationalTalentTest;
using System.Data.SqlClient;

namespace DataAccess
{
    internal class AdminDataAccess : DataAccessBase
    {     
    
        protected override string ConnectionString { get { return ConfigurationManager.ConnectionStrings["TalentTestConnectionString"].ConnectionString; } }

        public bool ValidateUser(string UserName, string Password)
        {
             DataTable dt = ExecuteSelect("SELECT * FROM admins where UserName=@UserName AND Password=@Password", new SqlParameter[]{
                    CreateParameter("@UserName", SqlDbType.NVarChar, UserName),
                    CreateParameter("@Password", SqlDbType.NVarChar, Password)}, CommandType.Text);
             if (dt.Rows.Count > 0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
        }
    }
}


