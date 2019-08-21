using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class UniversityController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: University
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public JsonResult getColleges(int UniversityID)
        {
            try
            {
                string Result = "";
                List<TblCollege> _CollegesList = _Context.TblColleges.Where(a => a.UniversityID == UniversityID && a.IsDeleted != true).ToList();
                Result += "<option value='0'>اسم الكلية</option>";
                foreach (var college in _CollegesList)
                {
                    Result += "<option value=' " + college.ID + "'>" + college.NameAr + "</option>";
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
                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(UniversityData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblUniversity UniversityObj = new TblUniversity()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblUniversities.Add(UniversityObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه الجامعه بنجاح";
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

        public ActionResult Edit(int UniversityID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (UniversityID > 0)
                {
                    TblUniversity UniObj = _Context.TblUniversities.Where(a => a.ID == UniversityID).SingleOrDefault();

                    return View(UniObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { UniversityID = UniversityID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { UniversityID = UniversityID });
            }
        }

        [HttpPost]
        public ActionResult Edit(UniversityData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblUniversity UniObj = _Context.TblUniversities.Where(a => a.ID == _Data.ProductCategoryID).SingleOrDefault();
                    if (UniObj != null)
                    {
                        UniObj.NameAr = _Data.NameAr;
                        UniObj.NameEn = _Data.NameEn;
                        UniObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات الجامعه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { UniversityID = _Data.ProductCategoryID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { UniversityID = _Data.ProductCategoryID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { UniversityID = _Data.ProductCategoryID });
            }
        }

    }
}