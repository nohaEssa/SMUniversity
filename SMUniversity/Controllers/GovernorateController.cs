using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class GovernorateController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Governorate
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
        public ActionResult Create(GovernorateData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblGovernorate GovernorateObj = new TblGovernorate()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblGovernorates.Add(GovernorateObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه المحافظه بنجاح";
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

        public ActionResult Edit(int GovernorateID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (GovernorateID > 0)
                {
                    TblGovernorate GovernorateObj = _Context.TblGovernorates.Where(a => a.ID == GovernorateID).SingleOrDefault();

                    return View(GovernorateObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { GovernorateID = GovernorateID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { GovernorateID = GovernorateID });
            }
        }

        [HttpPost]
        public ActionResult Edit(GovernorateData _Data)
        {
            try
            {
                if (_Data.GovernorateID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblGovernorate GovernorateObj = _Context.TblGovernorates.Where(a => a.ID == _Data.GovernorateID).SingleOrDefault();
                    if (GovernorateObj != null)
                    {
                        GovernorateObj.NameAr = _Data.NameAr;
                        GovernorateObj.NameEn = _Data.NameEn;
                        GovernorateObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات المحافظه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { GovernorateID = _Data.GovernorateID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { GovernorateID = _Data.GovernorateID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { GovernorateID = _Data.GovernorateID });
            }
        }

        public JsonResult Delete(int GovernorateID)
        {
            try
            {
                if (GovernorateID > 0)
                {
                    //TempData["notice"] = "غير قادر علي الحذف, المحافظه مرتيطه ببيانات في المناطق والطلاب";
                    //return Json("OK");
                    TblGovernorate GovernorateObj = _Context.TblGovernorates.Where(a => a.ID == GovernorateID).SingleOrDefault();
                    if (GovernorateObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف محافظة " + GovernorateObj.NameAr + " بنجاح";

                            _Context.TblGovernorates.Remove(GovernorateObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, المحافظه مرتيطه ببيانات في المناطق والطلاب";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Governorate not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Governorate not found!";
                    return Json("ERROR");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return Json("ERROR");
            }
        }
    }
}