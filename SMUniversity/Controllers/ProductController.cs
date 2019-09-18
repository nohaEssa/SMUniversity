using QRCoder;
using SMUModels;
using SMUModels.Handlers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class ProductController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Product
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
                List<TblProductCategory> _ProdCatList = _Context.TblProductCategories.Where(a => a.IsDeleted != true).ToList();

                return View(_ProdCatList);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(ProductObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.Cost > 0)
                {
                    TblProduct _ProductObj = new TblProduct()
                    {
                        ProductCategoryID = _Data.ProductCategoryID,
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        Cost = _Data.Cost,
                        DescriptionAr = _Data.DescriptionAr,
                        DescriptionEn = _Data.DescriptionEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        
                    };

                    if (Request.Files["Picture"] != null)
                    {
                        UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                        if (Respo.Code == 1)
                        {
                            _ProductObj.Picture = Respo.Message;
                        }
                    }

                    _Context.TblProducts.Add(_ProductObj);
                    _Context.SaveChanges();

                    _ProductObj.QRCode = SaveQRCode((_ProductObj.ID.ToString() + ",0"), string.Format("{0}_QRCode_", _ProductObj.NameAr));
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافة المنتج بنجاح";
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

        public ActionResult Edit(int ProductID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                TblProduct Prod = _Context.TblProducts.Where(a => a.ID == ProductID && a.IsDeleted != true).FirstOrDefault();
                List<TblProductCategory> ProductCatList = _Context.TblProductCategories.Where(a => a.IsDeleted != true).ToList();

                ViewBag.ProductCatList = ProductCatList;
                return View(Prod);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.Cost > 0 && _Data.ProductCategoryID > 0 && _Data.ProductID > 0)
                {
                    TblProduct _ProductObj = _Context.TblProducts.Where(a => a.ID == _Data.ProductID).FirstOrDefault();
                    if (_ProductObj != null)
                    {
                        _ProductObj.NameAr = _Data.NameAr;
                        _ProductObj.NameEn = _Data.NameEn;
                        _ProductObj.DescriptionAr = _Data.DescriptionAr;
                        _ProductObj.DescriptionEn = _Data.DescriptionEn;
                        _ProductObj.Cost = _Data.Cost;
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

                        TempData["notice"] = "تم تعديل بيانات المنتج بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "المنتج غير موجود";
                        return RedirectToAction("Index");
                    }                    
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { ProductID = _Data.ProductID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { ProductID = _Data.ProductID });
            }
        }

        private string SaveQRCode(string _Text, string _FileName)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(_Text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 150;
            imgBarCode.Width = 150;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    //string fileName = DateTimeDependsOnTimeZone.GetDate().Ticks.ToString() + "_" + Guid.NewGuid().ToString().Substring(30) + ".png";
                    string fileName = string.Format("{0}{1}_{2}.png", _FileName.Replace(" ", "_"), DateTime.Now.Ticks, Guid.NewGuid().ToString().Substring(30));
                    FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/Product/") + fileName, FileMode.Create);
                    ms.WriteTo(fs);

                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                    return "/Content/Images/Session/" + fileName;
                }
            }

            return "";
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
                        var uploadUrl = Server.MapPath("~/Content/Images/Product/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        //Session["ProfilePic"] = fileName;

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/Product/" + fileName;
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

        public JsonResult Delete(int ProductID)
        {
            try
            {
                if (ProductID > 0)
                {
                    TblProduct ProductObj = _Context.TblProducts.Where(a => a.ID == ProductID).SingleOrDefault();
                    if (ProductObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف المنتج بنجاح";

                            _Context.TblProducts.Remove(ProductObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, المنتج مرتبط ببيانات خاصه بالفواتير";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Product not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Product not found!";
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