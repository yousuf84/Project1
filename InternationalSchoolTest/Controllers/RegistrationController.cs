using System;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Models;
using System.Web;
using DataAccess.ApplicationService;

namespace InternationalSchoolTest.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registration/

        public ActionResult Index()
        {
            return View(new Student());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ExcelUpload()
        {
            return View();
        }

       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ExcelUpload(HttpPostedFileBase uploadFile)
        {
            SetBatchUploadSessionVars();
           if (uploadFile.ContentLength > 0)
            {             
              ViewData["BatchID"] = StudentRegistrationUploadService.Upload(uploadFile.FileName);
              Session["BatchID"] = ViewData["BatchID"];
            }
            return View();
        }

        public ActionResult ViewBatch(int id)
       {
           return PartialView();
       }

       [HttpPost]
        public ActionResult BatchUpload(string sidx, string sord, int page, int rows,
                 bool _search, string searchField, string searchOper, string searchString)
        {
            System.Data.DataTable dt;
            if (Session["BatchID"] != null)
            {
                int id = int.Parse(Session["BatchID"].ToString());
                dt = StudentRegistrationUploadService.GetBatchUpload(id, 100000, page, sidx, sord);
                return Content(Helper.JsonHelper.JsonForJqgrid(dt, rows, StudentRegistrationUploadService.GetTotalCount(id), page), "application/json"); 

            }
            else
            {
                dt = StudentSearchService.Search((string)Session["StudentGivenName"], (string)Session["StudentSurName"], (string)Session["FatherName"], 
                                                    (string)Session["SchoolName"], 100000, page, sidx, sord);
                return Content(Helper.JsonHelper.JsonForJqgrid(dt, rows, dt.Rows.Count, page), "application/json"); 

            }
        }

       [HttpPost]
       public ActionResult PrintHallTicket()
       {
           string str = Request["hfSelectedRows"];
           ExportToWord.Export(Response, str);
           return View();        
       }
        
        public ActionResult Search()
       {
           return View();
       }

        [HttpPost]
        public ActionResult Search(string StudentGivenName, string StudentSurName, string FatherName, string SchoolName)
        {
           
            SetSearchSessionVars();
            SetSessionAndViewDataVars(StudentGivenName, StudentSurName, FatherName, SchoolName);
            ViewData["SearchComplete"] = true;

            return View();
        }

        private void SetBatchUploadSessionVars()
        {
            Session["StudentGivenName"] = null;
            Session["StudentSurName"] = null;
            Session["FatherName"] = null;
            Session["SchoolName"] = null;
        }

        private void SetSearchSessionVars()
        {
            Session["BatchID"] = null;
        }
        private void SetSessionAndViewDataVars(string StudentGivenName, string StudentSurName, string FatherName, string SchoolName)
        {
            Session["StudentGivenName"] = StudentGivenName;
            Session["StudentSurName"] = StudentSurName;
            Session["FatherName"] = FatherName;
            Session["SchoolName"] = SchoolName;
            ViewData["StudentGivenName"] = StudentGivenName;
            ViewData["StudentSurName"] = StudentSurName;
            ViewData["FatherName"] = FatherName;
            ViewData["SchoolName"] = SchoolName;

        }
    }
}
