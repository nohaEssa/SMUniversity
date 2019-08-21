using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class EvaluationController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Evaluation
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult EvaluationQuestions()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult CreateEvaluationQues()
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
                return RedirectToAction("CreateEvaluationQues");
            }
        }

        [HttpPost]
        public ActionResult CreateEvaluationQues(EvaluationQuesData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.QuestionAr) && !string.IsNullOrEmpty(_Data.QuestionEn))
                {
                    TblEvaluationQuestion EvaluationQuesObj = new TblEvaluationQuestion()
                    {
                        QuestionAr = _Data.QuestionAr,
                        QuestionEn = _Data.QuestionEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblEvaluationQuestions.Add(EvaluationQuesObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه سؤال التقييم بنجاح";
                    return RedirectToAction("EvaluationQuestions");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("CreateEvaluationQues");
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("CreateEvaluationQues");
            }
        }

        public ActionResult EditEvaluationQues(int EvaluationQuesID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (EvaluationQuesID > 0)
                {
                    TblEvaluationQuestion EvaluationQuesObj = _Context.TblEvaluationQuestions.Where(a => a.ID == EvaluationQuesID).SingleOrDefault();

                    return View(EvaluationQuesObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("EditEvaluationQues", new { EvaluationQuesID = EvaluationQuesID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("EditEvaluationQues", new { EvaluationQuesID = EvaluationQuesID });
            }
        }

        [HttpPost]
        public ActionResult EditEvaluationQues(EvaluationQuesData _Data)
        {
            try
            {
                if (_Data.EvaluationQuesID > 0 && !string.IsNullOrEmpty(_Data.QuestionAr) && !string.IsNullOrEmpty(_Data.QuestionEn))
                {
                    TblEvaluationQuestion EvaluationQuesObj = _Context.TblEvaluationQuestions.Where(a => a.ID == _Data.EvaluationQuesID).SingleOrDefault();
                    if (EvaluationQuesObj != null)
                    {
                        EvaluationQuesObj.QuestionAr = _Data.QuestionAr;
                        EvaluationQuesObj.QuestionEn = _Data.QuestionEn;
                        EvaluationQuesObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات سؤال التقييم بنجاح";
                        return RedirectToAction("EvaluationQuestions");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("EditEvaluationQues", new { EvaluationQuesID = _Data.EvaluationQuesID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("EditEvaluationQues", new { EvaluationQuesID = _Data.EvaluationQuesID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("EditEvaluationQues", new { EvaluationQuesID = _Data.EvaluationQuesID });
            }
        }

        public ActionResult EvaluationQuesAns()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult CreateEvaluationQuesAns()
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
                return RedirectToAction("CreateEvaluationQuesAns");
            }
        }

        [HttpPost]
        public ActionResult CreateEvaluationQuesAns(EvalAnsData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.AnswerAr) && !string.IsNullOrEmpty(_Data.AnswerEn))
                {
                    TblEvaluationQuestionAnswer EvalQuesAnsObj = new TblEvaluationQuestionAnswer()
                    {
                        AnswerAr = _Data.AnswerAr,
                        AnswerEn = _Data.AnswerEn,
                        Value = _Data.Value,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblEvaluationQuestionAnswers.Add(EvalQuesAnsObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه اجابه التقييم بنجاح";
                    return RedirectToAction("EvaluationQuesAns");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("CreateEvaluationQuesAns");
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("CreateEvaluationQuesAns");
            }
        }

        public ActionResult EditEvaluationQuesAns(int AnswerID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (AnswerID > 0)
                {
                    TblEvaluationQuestionAnswer EvalQuesAnsObj = _Context.TblEvaluationQuestionAnswers.Where(a => a.ID == AnswerID).SingleOrDefault();

                    return View(EvalQuesAnsObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("EditEvaluationQuesAns", new { AnswerID = AnswerID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("EditEvaluationQuesAns", new { AnswerID = AnswerID });
            }
        }

        [HttpPost]
        public ActionResult EditEvaluationQuesAns(EvalAnsData _Data)
        {
            try
            {
                if (_Data.AnswerID > 0 && !string.IsNullOrEmpty(_Data.AnswerAr) && !string.IsNullOrEmpty(_Data.AnswerEn))
                {
                    TblEvaluationQuestionAnswer EvaluationQuesAnswObj = _Context.TblEvaluationQuestionAnswers.Where(a => a.ID == _Data.AnswerID).SingleOrDefault();
                    if (EvaluationQuesAnswObj != null)
                    {
                        EvaluationQuesAnswObj.AnswerAr = _Data.AnswerAr;
                        EvaluationQuesAnswObj.AnswerEn = _Data.AnswerEn;
                        EvaluationQuesAnswObj.Value = _Data.Value;
                        EvaluationQuesAnswObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات سؤال التقييم بنجاح";
                        return RedirectToAction("EditEvaluationQuesAns", new { AnswerID = _Data.AnswerID });
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("EditEvaluationQuesAns", new { AnswerID = _Data.AnswerID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("EditEvaluationQuesAns", new { AnswerID = _Data.AnswerID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("EditEvaluationQuesAns", new { AnswerID = _Data.AnswerID });
            }
        }


    }
}