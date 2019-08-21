using SMUModels;
using SMUModels.Handlers;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class UserController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: User
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
                List<TblUserCategory> UserCategories = _Context.TblUserCategories.Where(a => a.IsDeleted != true).ToList();
                List<TblBranch> Branches = _Context.TblBranches.Where(a => a.IsDeleted != true).ToList();

                ViewBag.UserCategories = UserCategories;
                ViewBag.Branches = Branches;

                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }

        }

        [HttpPost]
        public ActionResult Create(UserObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && _Data.BranchID > 0 && _Data.UserCategoryID > 0)
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 4).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        TblUser _UserObj = new TblUser()
                        {
                            NameAr = _Data.NameAr,
                            NameEn = _Data.NameEn,
                            Email = _Data.Email,
                            PhoneNumber = _Data.PhoneNumber,
                            //BranchID = _Data.UserCategoryID,
                            BranchID = 3,
                            UserCategoryID = _Data.UserCategoryID,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        if (Request.Files["Picture"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                            if (Respo.Code == 1)
                            {
                                _UserObj.ProfilePic = Respo.Message;
                            }
                        }

                        _Context.TblUsers.Add(_UserObj);
                        _Context.SaveChanges();

                        _UserCred = new TblUserCredential()
                        {
                            UserName = _Data.UserName,
                            Password = _Data.Password,
                            UserType = 4,
                        };

                        _Context.TblUserCredentials.Add(_UserCred);
                        _Context.SaveChanges();

                        _UserObj.CredentialsID = _UserCred.ID;
                        _Context.SaveChanges();

                        TempData["notice"] = "تم إضافة المستخدم بنجاح";
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        TempData["notice"] = "اسم المستخدم مستخدم من قبل, من فضلك ادخل اسم مستخدم آخر";
                        return RedirectToAction("Create");
                    }

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

        public ActionResult Edit(int UserID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                TblUser _User = _Context.TblUsers.Where(a => a.ID == UserID && a.IsDeleted != true).FirstOrDefault();
                List<TblUserCategory> UserCategories = _Context.TblUserCategories.Where(a => a.IsDeleted != true).ToList();
                List<TblBranch> Branches = _Context.TblBranches.Where(a => a.IsDeleted != true).ToList();

                ViewBag.UserCategories = UserCategories;
                ViewBag.Branches = Branches;

                return View(_User);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { UserID = UserID });
            }

        }

        [HttpPost]
        public ActionResult Edit(UserObj _Data)
        {
            try
            {
                if (_Data.UserID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && _Data.BranchID > 0 && _Data.UserCategoryID > 0)
                {
                    TblUser _UserObj = _Context.TblUsers.Where(a => a.ID == _Data.UserID).FirstOrDefault();
                    if (_UserObj != null)
                    {
                        _UserObj.NameAr = _Data.NameAr;
                        _UserObj.NameEn = _Data.NameEn;
                        _UserObj.Email = _Data.Email;
                        _UserObj.PhoneNumber = _Data.PhoneNumber;
                        //_UserObj.BranchID = _Data.BranchID;
                        _UserObj.BranchID = 3;
                        _UserObj.UserCategoryID = _Data.UserCategoryID;
                        _UserObj.UpdatedDate = DateTime.Now;

                        if (Request.Files["Picture"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                            if (Respo.Code == 1)
                            {
                                _UserObj.ProfilePic = Respo.Message;
                            }
                        }

                        TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 4).SingleOrDefault();
                        if (_UserCred != null)
                        {
                            _UserCred.Password = _UserCred.Password;
                        }
                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات المستخدم بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "المستخدم غير موجود";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { UserID = _Data.UserID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { UserID = _Data.UserID });
            }
        }

        public ActionResult AddUserCategory()
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
        public ActionResult AddUserCategory(UserObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblUserCategory _UserCategory = new TblUserCategory()
                    {
                        NameAr = _Data.NameAr,
                        NameEn = _Data.NameEn,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };
                    _Context.TblUserCategories.Add(_UserCategory);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافة فئة المستخدم بنجاح";
                    return RedirectToAction("Create");

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

        public ActionResult EditUserCategory(int UserCategoryID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                TblUserCategory _UserCategory = _Context.TblUserCategories.Where(a => a.ID == UserCategoryID && a.IsDeleted != true).FirstOrDefault();
                return View(_UserCategory);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }

        }

        [HttpPost]
        public ActionResult EditUserCategory(UserCategoryObj _Data)
        {
            try
            {
                if (_Data.UserCategoryID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn))
                {
                    TblUserCategory _UserObj = _Context.TblUserCategories.Where(a => a.ID == _Data.UserCategoryID).FirstOrDefault();
                    if (_UserObj != null)
                    {
                        _UserObj.NameAr = _Data.NameAr;
                        _UserObj.NameEn = _Data.NameEn;
                        _UserObj.UpdatedDate = DateTime.Now;

                        TempData["notice"] = "تم تعديل بيانات فئة المستخدم بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "فئة المستخدم غير موجود";
                        return RedirectToAction("Index");
                    }
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
                        var uploadUrl = Server.MapPath("~/Content/Images/User/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        //Session["ProfilePic"] = fileName;

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/User/" + fileName;
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

    }
}