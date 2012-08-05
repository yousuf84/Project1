using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using InternationalTalentTest;
using System.Collections.Generic;
using System.Configuration;

namespace DataAccess
{
    internal class RegistrationUploadDataAccess : DataAccessBase
    {
        protected override string ConnectionString { get { return ConfigurationManager.ConnectionStrings["TalentTestConnectionString"].ConnectionString; } }
        public int Upload(string strFileName)
        {
            ExcelDataAccess exceldataaccess = new ExcelDataAccess();
            DataSet ds = (DataSet)exceldataaccess.GetDataSetFromExcel(strFileName);
            int iBatchID = GetBatchID();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                InsertIntoTempTable(iBatchID, ds.Tables[0].Columns, dr);
            }
            return ValidateUpload(iBatchID);
        }

        public DataTable GetBatchUpload(int iBatchID, int pageSize, int page, string sidx, string sord)
        {

            int startIndex = (page - 1) * pageSize;
            int endIndex = page * pageSize;
            return ExecuteSelect("spN_sel_RegistrationUploads", new SqlParameter[] { new SqlParameter("@sidx", sidx), new SqlParameter("@sord", sord), 
                                     new SqlParameter("@BatchId", iBatchID), new SqlParameter("@startindex", startIndex), new SqlParameter("@endIndex", endIndex)}, 
                                     CommandType.StoredProcedure);
            
        }

        public int GetTotalCount(int iBatchID)
        {
           DataTable dt = ExecuteSelect("SELECT * FROM RegistrationUploads WHERE BatchID = @BatchID", new SqlParameter[] { new SqlParameter("@BatchID", iBatchID) }, CommandType.Text);
            return (int)dt.Rows[0][0];
        }
        public void InsertIntoTempTable(int iBatchID, DataColumnCollection columns, DataRow dr)
        {           
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@BatchID", iBatchID));
            foreach (DataColumn dc in columns)
            {
                parameters.Add(new SqlParameter("@" + dc.ColumnName, dr[dc.ColumnName].ToString()));
            }
            ExecuteNonQuery("spN_ins_RegistrationUploads", parameters.ToArray(), CommandType.StoredProcedure);
        }

        public int ValidateUpload(int iBatchID)
        {
           DataTable dt = ExecuteSelect("ValidateRegistration", new SqlParameter[] { new SqlParameter("@BatchID", iBatchID) }, CommandType.StoredProcedure);
           return iBatchID;
        }
              
        public int GetBatchID()
        {
           string cmdText = "SELECT MAX(BatchID) FROM dbo.RegistrationUploads";
           DataTable dt = ExecuteSelect(cmdText, CommandType.Text);
           if (dt.Rows[0][0].ToString() == "")
           {
               return 1;
           }
           else
           {
               return (int)dt.Rows[0][0] + 1;
           }
        }

        public DataTable Search(string StudentGivenName, string StudentSurName, string FatherName, string SchoolName, int pageSize, int page, string sidx, string sord)
        {

            int startIndex = (page - 1) * pageSize;
            int endIndex = page * pageSize;
            return ExecuteSelect("spN_sel_SearchStudent", new SqlParameter[] { new SqlParameter("@sidx", sidx), new SqlParameter("@sord", sord), 
                                        new SqlParameter("@startindex", startIndex), new SqlParameter("@endIndex", endIndex), 
                                        new SqlParameter("@StudentGivenName", StudentGivenName), new SqlParameter("@StudentSurName", StudentSurName),
                                        new SqlParameter("@FatherName", FatherName), new SqlParameter("@SchoolName", SchoolName)},
                                        CommandType.StoredProcedure);

        }

    }
}
