using SMUModels;
using SMUModels.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class VoucherCategoryController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: ProductCategory
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
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]))
                {
                    TblVoucherCategory VoucherCategoryObj = new TblVoucherCategory()
                    {
                        NameAr = _Data["NameAr"],
                        NameEn = _Data["NameEn"],
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblVoucherCategories.Add(VoucherCategoryObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه فئة السند بنجاح";
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

        public ActionResult Edit(int VoucherCategoryID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (VoucherCategoryID > 0)
                {
                    TblVoucherCategory VoucherCatObj = _Context.TblVoucherCategories.Where(a => a.ID == VoucherCategoryID).SingleOrDefault();

                    return View(VoucherCatObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { VoucherCategoryID = VoucherCategoryID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { VoucherCategoryID = VoucherCategoryID });
            }
        }

        [HttpPost]
        public ActionResult Edit(FormCollection _Data)
        {
            int VoucherCategoryID = int.Parse(_Data["VoucherCategoryID"]);
            try
            {
                if (int.Parse(_Data["VoucherCategoryID"]) > 0 && !string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]))
                {
                    TblVoucherCategory _VoucherCategoryObj = _Context.TblVoucherCategories.Where(a => a.ID == VoucherCategoryID).SingleOrDefault();
                    if (_VoucherCategoryObj != null)
                    {
                        _VoucherCategoryObj.NameAr = _Data["NameAr"];
                        _VoucherCategoryObj.NameEn = _Data["NameEn"];
                        _VoucherCategoryObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات فئة السند بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { VoucherCategoryID = VoucherCategoryID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { VoucherCategoryID = VoucherCategoryID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { VoucherCategoryID = VoucherCategoryID });
            }
        }

        public JsonResult Delete(int VoucherCategoryID)
        {
            try
            {
                if (VoucherCategoryID > 0)
                {
                    TblVoucherCategory VoucherCategoryObj = _Context.TblVoucherCategories.Where(a => a.ID == VoucherCategoryID).SingleOrDefault();
                    if (VoucherCategoryObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف فئة السند بنجاح";

                            _Context.TblVoucherCategories.Remove(VoucherCategoryObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, فئة السند مرتبطه ببيانات خاصه بالسندات";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "VoucherCategory not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "VoucherCategory not found!";
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