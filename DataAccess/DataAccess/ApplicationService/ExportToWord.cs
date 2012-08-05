using System;
using System.Linq;
using System.Web;
using System.Data;

namespace DataAccess.ApplicationService
{
    public class ExportToWord
    {
        public static void Export(HttpResponseBase Response, string strStudentId)
        {
            HallTicketDataAccess dataaccess = new HallTicketDataAccess();
            DataTable dt = dataaccess.GetHallTickets(strStudentId);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "PrintHalltickets.doc");
            Response.Write("<html " + "xmlns:o='urn:schemas-microsoft-com:office:office' " + "xmlns:w='urn:schemas-microsoft-com:office:word'" + "xmlns='http://www.w3.org/TR/REC-html40'>" + "<head><title>Time</title>");
            Response.Write("<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">");
            Response.Write("<meta name=ProgId content=Word.Document>");
            Response.Write("<meta name=Generator content=\"Microsoft Word 9\">");
            Response.Write("<meta name=Originator content=\"Microsoft Word 9\">");

            Response.Write("<!--[if gte mso 9]>" + "<xml>" + "<w:WordDocument>" + "<w:View>Print</w:View>" + "<w:Zoom>90</w:Zoom>" + "<w:DoNotOptimizeForBrowser/>" + "</w:WordDocument>" + "</xml>" + "<![endif]-->");
            Response.Write("<style> @page" + "{size:8.5in 11.0in; mso-first-footer:ff1; mso-footer: f1;" + " margin:0.2in 0.2in 0.2in 0.2in ; " + " mso-header-margin:.1in; " + " mso-footer-margin:.1in; mso-paper-source:0;}" + " div.Section1" + " {page:Section1;}" + "p.MsoFooter, li.MsoFooter, div.MsoFooter{margin:0in; margin-bottom:.0001pt; mso-pagination:widow-orphan; tab-stops:center 3.0in right 6.0in; font-size:12.0pt; font-family:'Verdana';}" + "p.MsoHeader, li.MsoHeader, div.MsoHeader {margin:0in; margin-bottom:.0001pt; mso-pagination:widow-orphan; tab-stops:center 3.0in right 6.0in; font-size:12.0pt; font-family:'Verdana';}" + "table#hrdftrtbl { margin:0in 0in 0in 0.1in; }" + "-->" + "</style>" + "</head>");
            Response.Write("<body lang=EN-US style='tab-interval:.5in'>");

            Response.Write("<table stle=\"width: 8.5in;\">");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr>");
                Response.Write("<td colspan=2 style=\"height: 0.25in;\">");
                Response.Write("</td>");
                Response.Write("</tr>");

                Response.Write("<tr>");
                Response.Write("<td colspan=2 align=\"center\">Exam Name</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td colspan=2 style=\"height: 0.25in;\">");
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("Hall Ticket Number"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["HallTicketNo"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("Student Name"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["Name"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("Father's Name"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["FatherName"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("School Name"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["SchoolName"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("Course Code"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["CourseCode"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("Exam Schedule"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["ExamSchedule"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("<tr>");
                Response.Write("<td>" + GetSideHeader("Exam Center Address"));
                Response.Write("</td>");
                Response.Write("<td>" + dr["ExamCenterAddress"].ToString());
                Response.Write("</td>");
                Response.Write("</tr>");

            }
            Response.Write("</table>");

            Response.Write("</body></head>");
            Response.Write("</html>");
            Response.Flush();




        }
        private static string GetSideHeader(string str)
        {
            return "<DIV ALIGN=LEFT style='font-family: verdana;font-size: 14pt ;color:Navy'>" + str + "</DIV>";
        }
    }
}
