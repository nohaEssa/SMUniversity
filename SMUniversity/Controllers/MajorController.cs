using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class MajorController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Major
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public JsonResult getMajors(int CollegeID)
        {
            try
            {
                string Result = "";
                List<TblMajor> _MajorsList = _Context.TblMajors.Where(a => a.CollegeID == CollegeID && a.IsDeleted != true).ToList();
                Result += "<option value='0'>التخصص</option>";
                foreach (var major in _MajorsList)
                {
                    Result += "<option value=' " + major.ID + "'>" + major.NameAr + "</option>";
                }

                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
        }

        public ActionResult Create()
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                ViewBag.UniversitiesList = UniversitiesList;

                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(MajorData _Data)
        {
            try
            {
                if(!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.CollegeID > 0)
                {
                    TblMajor MajorObj = new TblMajor()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        CollegeID = _Data.CollegeID,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblMajors.Add(MajorObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه التخصص بنجاح";
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

        public ActionResult Edit(int MajorID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (MajorID > 0)
                {
                    TblMajor MajorObj = _Context.TblMajors.Where(a => a.ID == MajorID).SingleOrDefault();
                    List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                    ViewBag.UniversitiesList = UniversitiesList;

                    return View(MajorObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { MajorID = MajorID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { MajorID = MajorID });
            }
        }

        [HttpPost]
        public ActionResult Edit(MajorData _Data)
        {
            try
            {
                if (_Data.MajorID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.CollegeID > 0)
                {
                    TblMajor MajorObj = _Context.TblMajors.Where(a => a.ID == _Data.MajorID).SingleOrDefault();
                    if (MajorObj != null)
                    {
                        MajorObj.NameAr = _Data.NameAr;
                        MajorObj.NameEn = _Data.NameEn;
                        MajorObj.CollegeID = _Data.CollegeID;
                        MajorObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات التخصص بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { MajorID = _Data.MajorID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { MajorID = _Data.MajorID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { MajorID = _Data.MajorID });
            }
        }
    }
}