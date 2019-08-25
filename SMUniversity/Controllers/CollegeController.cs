using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class CollegeController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: College
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
                List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                ViewBag.UniversitiesList = UniversitiesList;

                return View(UniversitiesList);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(CollegeData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.UniversityID > 0)
                {
                    TblCollege CollegeObj = new TblCollege()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        UniversityID = _Data.UniversityID,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblColleges.Add(CollegeObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه الكليه بنجاح";
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

        public ActionResult Edit(int CollegeID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (CollegeID > 0)
                {
                    TblCollege CollegeObj = _Context.TblColleges.Where(a => a.ID == CollegeID).SingleOrDefault();
                    List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                    ViewBag.UniversitiesList = UniversitiesList;

                    return View(CollegeObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { CollegeID = CollegeID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { CollegeID = CollegeID });
            }
        }

        [HttpPost]
        public ActionResult Edit(CollegeData _Data)
        {
            try
            {
                if (_Data.CollegeID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.UniversityID > 0)
                {
                    TblCollege CollegeObj = _Context.TblColleges.Where(a => a.ID == _Data.CollegeID).SingleOrDefault();
                    if (CollegeObj != null)
                    {
                        CollegeObj.NameAr = _Data.NameAr;
                        CollegeObj.NameEn = _Data.NameEn;
                        CollegeObj.UniversityID = _Data.UniversityID;
                        CollegeObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات الكليه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { CollegeID = _Data.CollegeID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { CollegeID = _Data.CollegeID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { CollegeID = _Data.CollegeID });
            }
        }

        public JsonResult Delete(int CollegeID)
        {
            try
            {
                TblCollege _College = _Context.TblColleges.Where(a => a.ID == CollegeID).FirstOrDefault();

                _Context.TblColleges.Remove(_College);
                _Context.SaveChanges();

                return Json("OK");
            }
            catch (Exception ex)
            {
                TempData["notice"] = "لم يتم حذف الكلية لإرتباطها ببيانات اخري";
                return Json("OK");
            }
        }

    }
}