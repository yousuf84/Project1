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
            string sql = @"WITH PAGED_RegistrationUploads AS (SELECT *, ROW_NUMBER() OVER (ORDER BY " + sidx + @" " + sord + @") AS RowNumber 
                    FROM RegistrationUploads WHERE BatchID = @BatchID ) SELECT FirstName,LastName,SchoolName,DateOfBirth,Mobile,
                    EmailID,FatherName,StreetAddress1,StreetAddress2,City,State,PinCode, ErrorMsg 
                 FROM PAGED_RegistrationUploads WHERE RowNumber BETWEEN " + startIndex + @" AND " + endIndex + @";";                       
            return ExecuteSelect(sql, new SqlParameter[] { new SqlParameter("@BatchID", iBatchID) }, CommandType.Text);

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
    }
}
