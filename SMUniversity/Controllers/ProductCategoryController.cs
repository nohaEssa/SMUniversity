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
    public class ProductCategoryController : Controller
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
        public ActionResult Create(ProductCategoryData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblProductCategory ProductObj = new TblProductCategory()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    if (Request.Files["Picture"] != null)
                    {
                        UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                        if (Respo.Code == 1)
                        {
                            ProductObj.Picture = Respo.Message;
                        }
                    }

                    _Context.TblProductCategories.Add(ProductObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه فئة المنتج بنجاح";
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

        public ActionResult Edit(int ProductCategoryID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (ProductCategoryID > 0)
                {
                    TblProductCategory ProdCatObj = _Context.TblProductCategories.Where(a => a.ID == ProductCategoryID).SingleOrDefault();

                    return View(ProdCatObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { ProductCategoryID = ProductCategoryID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { ProductCategoryID = ProductCategoryID });
            }
        }

        [HttpPost]
        public ActionResult Edit(UniversityData _Data)
        {
            try
            {
                if (_Data.ProductCategoryID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblProductCategory _ProductObj = _Context.TblProductCategories.Where(a => a.ID == _Data.ProductCategoryID).SingleOrDefault();
                    if (_ProductObj != null)
                    {
                        _ProductObj.NameAr = _Data.NameAr;
                        _ProductObj.NameEn = _Data.NameEn;
                        _ProductObj.UpdatedDate = DateTime.Now;

                        if (Request.Files["Picture"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                            if (Respo.Code == 1)
                            {
                                _ProductObj.Picture = Respo.Message;
                            }
                        }

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات فئة المنتج بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { ProductCategoryID = _Data.ProductCategoryID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { ProductCategoryID = _Data.ProductCategoryID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { ProductCategoryID = _Data.ProductCategoryID });
            }
        }

        public UploadFileResponse UploadPhoto(HttpPostedFileBase Image)
        {
            UploadFileResponse Respo = new UploadFileResponse();
            try
            {
                //var ImagePath = Request.Files["StudentPic"];

                if (Image != null && Image.ContentLength > 0)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png" };
                    var fileExt = System.IO.Path.GetExtension(Image.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        Respo.Code = 0;
                        Respo.Message = "Invalid type. Only the following types (jpg, jpeg, png) are supported. ";
                        return Respo;
                    }
                    else
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                        var uploadUrl = Server.MapPath("~/Content/Images/ProductCategory/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        //Session["ProfilePic"] = fileName;

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/ProductCategory/" + fileName;
                        return Respo;
                    }
                }
                else
                {
                    Respo.Code = 2;
                    Respo.Message = "من فضلك اختر صوره ";
                    return Respo;
                }

            }
            catch (Exception ex)
            {
                Respo.Code = 4;
                Respo.Message = ex.Message;
                return Respo;
            }
        }

        public JsonResult Delete(int ProductCategoryID)
        {
            try
            {
                if (ProductCategoryID > 0)
                {
                    TblProductCategory ProductCategoryObj = _Context.TblProductCategories.Where(a => a.ID == ProductCategoryID).SingleOrDefault();
                    if (ProductCategoryObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف فئة المنتج بنجاح";

                            _Context.TblProductCategories.Remove(ProductCategoryObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, فئة المنتج مرتبطه ببيانات خاصه بالمنتجات";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "ProductCategory not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "ProductCategory not found!";
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