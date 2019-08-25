using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class AreaController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Area
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public JsonResult getAreas(int GovernorateID)
        {
            try
            {
                string Result = "";
                List<TblArea> _AreasList = _Context.TblAreas.Where(a => a.GovernorateID == GovernorateID && a.IsDeleted != true).ToList();
                Result += "<option value='0'>المنطقة</option>";
                foreach (var area in _AreasList)
                {
                    Result += "<option value=' " + area.ID + "'>" + area.NameAr + "</option>";
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
                List<TblGovernorate> GovernorateList = _Context.TblGovernorates.Where(a => a.IsDeleted != true).ToList();
                ViewBag.GovernorateList = GovernorateList;

                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(AreaData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.GovernorateID > 0)
                {
                    TblArea AreaObj = new TblArea()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        GovernorateID = _Data.GovernorateID,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblAreas.Add(AreaObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه المنطقه بنجاح";
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

        public ActionResult Edit(int AreaID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (AreaID > 0)
                {
                    TblArea AreaObj = _Context.TblAreas.Where(a => a.ID == AreaID).SingleOrDefault();
                    List<TblGovernorate> GovernoratesList = _Context.TblGovernorates.Where(a => a.IsDeleted != true).ToList();
                    ViewBag.GovernoratesList = GovernoratesList;

                    return View(AreaObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { AreaID = AreaID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { AreaID = AreaID });
            }
        }

        [HttpPost]
        public ActionResult Edit(AreaData _Data)
        {
            try
            {
                if (_Data.AreaID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.GovernorateID > 0)
                {
                    TblArea AreaObj = _Context.TblAreas.Where(a => a.ID == _Data.AreaID).SingleOrDefault();
                    if (AreaObj != null)
                    {
                        AreaObj.NameAr = _Data.NameAr;
                        AreaObj.NameEn = _Data.NameEn;
                        AreaObj.GovernorateID = _Data.GovernorateID;
                        AreaObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات المنطقه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { AreaID = _Data.AreaID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { AreaID = _Data.AreaID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { AreaID = _Data.AreaID });
            }
        }

        public JsonResult Delete(int AreaID)
        {
            try
            {
                TblArea _Area = _Context.TblAreas.Where(a => a.ID == AreaID).FirstOrDefault();

                _Context.TblAreas.Remove(_Area);
                _Context.SaveChanges();

                return Json("OK");
            }
            catch (Exception ex)
            {
                TempData["notice"] = "لم يتم حذف المنطقه لإرتباطها ببيانات اخري";
                return Json("OK");
            }
        }
    }
}