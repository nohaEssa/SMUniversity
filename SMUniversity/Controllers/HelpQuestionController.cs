using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class HelpQuestionController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: HelpQuestion
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Create()
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(HelpQuestionData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.QuestionAr) && !string.IsNullOrEmpty(_Data.QuestionEn))
                {
                    TblHelpQuestion HelpQuesObj = new TblHelpQuestion()
                    {
                        QuestionAr = _Data.QuestionAr,
                        QuestionEn = _Data.QuestionEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblHelpQuestions.Add(HelpQuesObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه سؤال المساعده بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Create");
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        public ActionResult Edit(int HelpQuesID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (HelpQuesID > 0)
                {
                    TblHelpQuestion HelpQuesObj = _Context.TblHelpQuestions.Where(a => a.ID == HelpQuesID).SingleOrDefault();

                    return View(HelpQuesObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { HelpQuesID = HelpQuesID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { HelpQuesID = HelpQuesID });
            }
        }

        [HttpPost]
        public ActionResult Edit(HelpQuestionData _Data)
        {
            try
            {
                if (_Data.HelpQuestionID > 0 && !string.IsNullOrEmpty(_Data.QuestionAr) && !string.IsNullOrEmpty(_Data.QuestionEn))
                {
                    TblHelpQuestion HelpQuesObj = _Context.TblHelpQuestions.Where(a => a.ID == _Data.HelpQuestionID).SingleOrDefault();
                    if (HelpQuesObj != null)
                    {
                        HelpQuesObj.QuestionAr = _Data.QuestionAr;
                        HelpQuesObj.QuestionEn = _Data.QuestionEn;
                        HelpQuesObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات سؤال المساعده بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { HelpQuestionID = _Data.HelpQuestionID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { HelpQuestionID = _Data.HelpQuestionID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { HelpQuestionID = _Data.HelpQuestionID });
            }
        }

        public ActionResult StudentComplaints()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

    }
}