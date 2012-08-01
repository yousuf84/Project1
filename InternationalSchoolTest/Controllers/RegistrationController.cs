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

        public ActionResult ExcelUpload()
        {
            return View();
        }

       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ExcelUpload(HttpPostedFileBase uploadFile)
        {
            if (uploadFile.ContentLength > 0)
            {
             
              ViewData["BatchID"] = StudentRegistrationUploadService.Upload(uploadFile.FileName);
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
            int id;
            id = (int)Request["BatchID"];
           System.Data.DataTable dt = StudentRegistrationUploadService.GetBatchUpload(id, 100000, page,sidx, sord);
           return Content(Helper.JsonHelper.JsonForJqgrid(dt, rows, StudentRegistrationUploadService.GetTotalCount(id), page), "application/json"); 
        }
    }
}
