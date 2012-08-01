using System;
using System.Linq;
using System.Data;
namespace DataAccess
{
    internal class ExcelDataAccess
    {
        public  object GetDataSetFromExcel(string strFileName)
        {
            bool blnIsXlsx = false;
            if (System.IO.Path.GetExtension(strFileName).ToLower() == ".xlsx")
            {
                blnIsXlsx = true;
            }
            else
            {
                blnIsXlsx = false;
            }
            return ReadExcelFile(strFileName, blnIsXlsx);
        }
        private DataSet ReadExcelFile(string strPath, bool blnXslx = false)
        {
            try
            {
                DataSet myDataset = new DataSet();

                string strConn = null;

                if (blnXslx)
                {
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strPath + ";" + "Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
                }
                else
                {
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strPath + ";" + "Extended Properties=\"Excel 8.0;HDR=YES\"";
                }
                System.Data.OleDb.OleDbDataAdapter myData = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + GetExcelSheetNames(strPath, blnXslx)[0] + "];", strConn);
                //myData.TableMappings.Add("Table", "ExcelTest")
                myData.Fill(myDataset);
                return myDataset;

            }
            catch (Exception ex)
            {
                throw (new Exception("Cannot Read Excel  -" + ex.Message));
            }
        }
        private String[] GetExcelSheetNames(string excelFile, bool blnXlsx = false)
        {
            System.Data.OleDb.OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                String connString = null;
                if (blnXlsx)
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + excelFile + ";Extended Properties=\"Excel 12.0 Xml;HDR=NO;IMEX=1\"";
                }
                else
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelFile + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"";
                }

                objConn = new System.Data.OleDb.OleDbConnection(connString);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i += 1;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                throw (new Exception("Cannot Read Excel Sheet Names -" + ex.Message));
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

    }
}
