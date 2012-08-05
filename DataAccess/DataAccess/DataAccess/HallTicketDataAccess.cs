using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using InternationalTalentTest;
using System.Configuration;

namespace DataAccess
{
    internal class HallTicketDataAccess : DataAccessBase
    {
        protected override string ConnectionString { get { return ConfigurationManager.ConnectionStrings["TalentTestConnectionString"].ConnectionString; } }
        public DataTable GetHallTickets(string strStudentIds)
        {
            DataTable dt = ExecuteSelect("spN_sel_HallTickets", new SqlParameter[]{
                    CreateParameter("@StudentIds", SqlDbType.NVarChar, strStudentIds)}, CommandType.StoredProcedure);
            return dt;

        }

    }
}
