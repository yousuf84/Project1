using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess.ApplicationService;
using DataAccess.Models;
using System.Web;

namespace InternationalSchoolTest.Controllers
{
    public class PracticeTestController : Controller
    {
        //
        // GET: /PracticeTest/

        public ActionResult Index(int id)
        {
            Session["ClassID"] = id;
            List<Question> QuestionsList = PracticeTestService.Get(id);
            Session["QuestionList"] = QuestionsList;
            return View(QuestionsList[0]);
        }
       
        [HttpPost]
        public ActionResult NextQuestion(int QuestionID)
        {
            List<Question> QuestionsList = (List<Question>)Session["QuestionList"];
            List<Question> nextquestions = QuestionsList.FindAll(q => q.QuestionID > QuestionID);
           if (nextquestions.Count > 0)
           {
               return View("Index", nextquestions[0]);
           }
           else
           {
               return View("PracticeTestComplete");
           }
        }
        public ActionResult Upload()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase uploadFile, int SchoolClass)
        {
            try
            {

                if (uploadFile.ContentLength > 0)
                {
                    MockTestApplicationService.Upload(uploadFile.FileName, SchoolClass);
                    ViewData["PracticeTestUploadMessage"] = "Practice Test uploaded successfully.";
                }
            }
            catch (Exception ex)
            {
                ViewData["PracticeTestUploadMessage"] = "An Error Occurred - " + ex.Message;            
            }
            return View();
        }
    }
}
