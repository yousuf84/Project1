using System;
using System.Linq;
using System.Data;
using System.Text;
namespace InternationalSchoolTest.Helper
{
    public class JsonHelper {
        public static string JsonForJqgrid(DataTable dt, int pageSize, int totalRecords,int page) {
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"total\":" + totalPages + ",\"page\":" + page + ",\"records\":" + (totalRecords) + ",\"rows\"");
            jsonBuilder.Append(":[");
            for (int i = 0; i < dt.Rows.Count; i++) {
                jsonBuilder.Append("{\"i\":"+ (i) +",\"cell\":[");
                for (int j = 0; j < dt.Columns.Count; j++) {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
    }
}