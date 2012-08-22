using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using InternationalTalentTest;
using System.Collections.Generic;
using System.Configuration;

namespace DataAccess
{
    internal class MockTestUploadDataAccess : DataAccessBase
    {

        protected override string ConnectionString { get { return ConfigurationManager.ConnectionStrings["TalentTestConnectionString"].ConnectionString; } }
        public bool Upload(string strFileName, int iClass)
        {
            ExcelDataAccess exceldataaccess = new ExcelDataAccess();
            DataSet ds = (DataSet)exceldataaccess.GetDataSetFromExcel(strFileName);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                InsertIntoTempTable(iClass, ds.Tables[0].Columns, dr);
            }
            return true;
        }
        public void InsertIntoTempTable(int iClass, DataColumnCollection columns, DataRow dr)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Class", iClass));

            foreach (DataColumn dc in columns)
            {
                parameters.Add(new SqlParameter("@" + dc.ColumnName, dr[dc.ColumnName].ToString()));
            }
            ExecuteNonQuery("spN_ins_PracticeTestQuestion", parameters.ToArray(), CommandType.StoredProcedure);
        }


    }
}
