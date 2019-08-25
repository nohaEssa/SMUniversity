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
    public class StudentController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Student
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
                List<TblUniversity> _UniversityList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                List<TblGovernorate> _GovernorateList = _Context.TblGovernorates.Where(a => a.IsDeleted != true).ToList();
                List<TblCardCategory> _CardCategoryList = _Context.TblCardCategories.ToList();

                ViewBag.UniversityList = _UniversityList;
                ViewBag.GovernorateList = _GovernorateList;
                ViewBag.CardCategoryList = _CardCategoryList;
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentObj _Data)
        {
            try
            {
                if(!string.IsNullOrEmpty(_Data.FirstName)&& !string.IsNullOrEmpty(_Data.SecondName)&& !string.IsNullOrEmpty(_Data.ThirdName) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && !string.IsNullOrEmpty(_Data.UserName) && !string.IsNullOrEmpty(_Data.Password)&&_Data.AreaID>0 && _Data.CollegeID > 0 && _Data.GovernorateID > 0 && _Data.MajorID > 0 && _Data.UniversityID > 0)
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 1).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        TblStudent StudentObj = new TblStudent()
                        {
                            FirstName = _Data.FirstName,
                            SecondName = _Data.SecondName,
                            ThirdName = _Data.ThirdName,
                            PhoneNumber = _Data.PhoneNumber,
                            Email = _Data.Email,
                            //DateOfBirth = _Data.DateOfBirth.Date,
                            Gender = _Data.Gender, // 0 : Female , 1 : Male
                            UniversityID = _Data.UniversityID,
                            CollegeID = _Data.CollegeID,
                            MajorID = _Data.MajorID,
                            GovernorateID = _Data.GovernorateID,
                            AreaID = _Data.AreaID,
                            //BranchID = _Data.BranchID,
                            BranchID = 3,
                            Verified = true,
                            Balance = _Data.CardCategoryID > 0 ? _Context.TblCardCategories.Where(a => a.ID == _Data.CardCategoryID).FirstOrDefault().Price : 0,
                            CreatedDate = DateTime.Now,
                        };

                        if (Request.Files["ProfilePic"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["ProfilePic"]);
                            if (Respo.Code == 1)
                            {
                                StudentObj.ProfilePic = Respo.Message;
                            }
                        }
                        else
                        {
                            StudentObj.ProfilePic = "https://smuapitest.smartmindkw.com/Content/Images/Student/DefaultStudentPhoto.png";
                        }

                        _Context.TblStudents.Add(StudentObj);
                        _Context.SaveChanges();

                        _UserCred = new TblUserCredential()
                        {
                            UserName = _Data.UserName,
                            Password = _Data.Password,
                            UserType = 1,
                        };

                        _Context.TblUserCredentials.Add(_UserCred);
                        _Context.SaveChanges();

                        StudentObj.CredentialsID = _UserCred.ID;
                        _Context.SaveChanges();

                        TempData["notice"] = "تم إضافه الطالب بنجاح";
                        return RedirectToAction("Index");
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

        public ActionResult Edit(int StudentID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (StudentID > 0)
                {
                    TblStudent StdObj = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();

                    List<TblUniversity> _UniversityList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                    List<TblGovernorate> _GovernorateList = _Context.TblGovernorates.Where(a => a.IsDeleted != true).ToList();

                    ViewBag.UniversityList = _UniversityList;
                    ViewBag.GovernorateList = _GovernorateList;

                    return View(StdObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { StudentID = StudentID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { StudentID = StudentID });
            }
        }

        [HttpPost]
        public ActionResult Edit(StudentObj _Data)
        {
            try
            {
                if (_Data.StudentID > 0 && !string.IsNullOrEmpty(_Data.FirstName) && !string.IsNullOrEmpty(_Data.SecondName) && !string.IsNullOrEmpty(_Data.ThirdName) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && !string.IsNullOrEmpty(_Data.Password) && _Data.AreaID > 0 && _Data.CollegeID > 0 && _Data.GovernorateID > 0 && _Data.MajorID > 0 && _Data.UniversityID > 0)
                {
                    TblStudent std = _Context.TblStudents.Where(a => a.ID == _Data.StudentID).SingleOrDefault();
                    if (std != null)
                    {
                        std.FirstName = _Data.FirstName;
                        std.SecondName = _Data.SecondName;
                        std.ThirdName = _Data.ThirdName;
                        std.PhoneNumber = _Data.PhoneNumber;
                        std.Email = _Data.Email;
                        //std.DateOfBirth = _Data.DateOfBirth.Date;
                        std.Gender = _Data.Gender; // 0 : Female , 1 : Male
                        std.UniversityID = _Data.UniversityID;
                        std.CollegeID = _Data.CollegeID;
                        std.MajorID = _Data.MajorID;
                        std.GovernorateID = _Data.GovernorateID;
                        std.AreaID = _Data.AreaID;
                        std.UpdatedDate = DateTime.Now;

                        if (Request.Files["StudentPic"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["StudentPic"]);
                            if (Respo.Code == 1)
                            {
                                std.ProfilePic = Respo.Message;
                            }
                        }

                        TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 1).SingleOrDefault();
                        if (_UserCred != null)
                        {
                            _UserCred.Password = _UserCred.Password;
                        }
                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات الطالب بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { StudentID = _Data.StudentID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { StudentID = _Data.StudentID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { StudentID = _Data.StudentID });
            }
        }

        public string CheckUserNameValidation(string UserName)
        {
            try
            {
                TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == UserName && a.UserType == 1).SingleOrDefault();
                if (_UserCred == null)
                {
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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
                        var uploadUrl = Server.MapPath("~/Content/Images/Student/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        //Session["ProfilePic"] = fileName;

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/Student/" + fileName;
                        //Respo.Message = "تم التحميل بنجاح ";
                        //Respo.Code = fileName;

                        //Student.ProfilePic = fileName;
                        //context.SaveChanges();

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

        public JsonResult Delete(int StudentID)
        {
            try
            {
                if (StudentID > 0)
                {
                    TblStudent StudentObj = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();
                    if (StudentObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف الطالب " + (StudentObj.FirstName + " " + StudentObj.SecondName + " " + StudentObj.ThirdName);

                            _Context.TblUserCredentials.Remove(_Context.TblUserCredentials.Where(a => a.ID == StudentObj.CredentialsID).FirstOrDefault());

                            _Context.TblNotifications.RemoveRange(_Context.TblNotifications.Where(a => a.StudentID == StudentID).ToList());

                            _Context.TblStudents.Remove(StudentObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, الطالب مرتبط ببيانات خاصه بالحضور والغياب والتعاملات الماليه و شكاوي الطلبه والأشتراكات والحصص الخاصه";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Student not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Student not found!";
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