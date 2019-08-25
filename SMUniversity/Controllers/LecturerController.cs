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
    public class LecturerController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Lecturer
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
        public ActionResult Create(LecturerObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.FirstNameAr) && !string.IsNullOrEmpty(_Data.SecondNameAr) && !string.IsNullOrEmpty(_Data.ThirdNameAr) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && !string.IsNullOrEmpty(_Data.Address) && !string.IsNullOrEmpty(_Data.UserName) && !string.IsNullOrEmpty(_Data.Password))
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 2).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        TblLecturer LecturerObj = new TblLecturer()
                        {
                            FirstNameAr = _Data.FirstNameAr,
                            FirstNameEn = string.IsNullOrEmpty(_Data.FirstNameEn)? _Data.FirstNameAr: _Data.FirstNameEn,
                            SecondNameAr = _Data.SecondNameAr,
                            SecondNameEn = string.IsNullOrEmpty(_Data.SecondNameEn) ? _Data.SecondNameAr : _Data.SecondNameEn,
                            ThirdNameAr = string.IsNullOrEmpty(_Data.ThirdNameEn) ? _Data.ThirdNameAr : _Data.ThirdNameEn,
                            PhoneNumber = _Data.PhoneNumber,
                            Email = _Data.Email,
                            Gender = _Data.Gender, // 0 : Female , 1 : Male
                            Address = _Data.Address,
                            //BranchID = _Data.BranchID,
                            BranchID = 3,
                            CreatedDate = DateTime.Now,
                        };

                        if (Request.Files["ProfilePic"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["ProfilePic"]);
                            if (Respo.Code == 1)
                            {
                                LecturerObj.ProfilePic = Respo.Message;
                            }
                        }
                        else
                        {
                            LecturerObj.ProfilePic = "https://smuapitest.smartmindkw.com/Content/Images/Lecturer/DefaultLecturerPhoto.png";
                        }

                        _Context.TblLecturers.Add(LecturerObj);

                        _UserCred = new TblUserCredential()
                        {
                            UserName = _Data.UserName,
                            Password = _Data.Password,
                            UserType = 2,
                        };

                        _Context.TblUserCredentials.Add(_UserCred);
                        _Context.SaveChanges();

                        LecturerObj.CredentialsID = _UserCred.ID;
                        _Context.SaveChanges();

                        TblLecturerPaymentMethod _PaymentMethod1 = new TblLecturerPaymentMethod()
                        {
                            LecturerID = LecturerObj.ID,
                            PaymentMethod = _Data.LecturerAccountMethod,
                            Value = _Data.HourCost,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                        };
                        _Context.TblLecturerPaymentMethods.Add(_PaymentMethod1);
                        TblLecturerPaymentMethod _PaymentMethod2 = new TblLecturerPaymentMethod()
                        {
                            LecturerID = LecturerObj.ID,
                            PaymentMethod = _Data.LecturerAccountMethod,
                            Value = _Data.LectPercentage,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                        };
                        _Context.TblLecturerPaymentMethods.Add(_PaymentMethod2);
                        TblLecturerPaymentMethod _PaymentMethod3 = new TblLecturerPaymentMethod()
                        {
                            LecturerID = LecturerObj.ID,
                            PaymentMethod = _Data.LecturerAccountMethod,
                            Value = _Data.CoursePrice,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                        };
                        _Context.TblLecturerPaymentMethods.Add(_PaymentMethod3);
                        TblLecturerPaymentMethod _PaymentMethod4 = new TblLecturerPaymentMethod()
                        {
                            LecturerID = LecturerObj.ID,
                            PaymentMethod = _Data.LecturerAccountMethod,
                            Value = _Data.StudentPercentage,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                        };
                        _Context.TblLecturerPaymentMethods.Add(_PaymentMethod4);
                        _Context.SaveChanges();

                        TempData["notice"] = "تم إضافه المحاضر بنجاح";
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

        public ActionResult Edit(int LecturerID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (LecturerID > 0)
                {
                    TblLecturer LecturerObj = _Context.TblLecturers.Where(a => a.ID == LecturerID).SingleOrDefault();

                    return View(LecturerObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { LecturerID = LecturerID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { LecturerID = LecturerID });
            }
        }

        [HttpPost]
        public ActionResult Edit(LecturerObj _Data)
        {
            try
            {
                if (_Data.LecturerID > 0 && !string.IsNullOrEmpty(_Data.FirstNameAr) && !string.IsNullOrEmpty(_Data.SecondNameAr) && !string.IsNullOrEmpty(_Data.ThirdNameAr) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && !string.IsNullOrEmpty(_Data.Address) && !string.IsNullOrEmpty(_Data.Password))
                {
                    TblLecturer LecturerObj = _Context.TblLecturers.Where(a => a.ID == _Data.LecturerID).SingleOrDefault();
                    if (LecturerObj != null)
                    {
                        LecturerObj.FirstNameAr = _Data.FirstNameAr;
                        LecturerObj.FirstNameEn = string.IsNullOrEmpty(_Data.FirstNameEn)? _Data.FirstNameAr: _Data.FirstNameEn;
                        LecturerObj.SecondNameAr = _Data.SecondNameAr;
                        LecturerObj.SecondNameEn = string.IsNullOrEmpty(_Data.SecondNameEn) ? _Data.SecondNameAr : _Data.SecondNameAr;
                        LecturerObj.ThirdNameAr = _Data.ThirdNameAr;
                        LecturerObj.ThirdNameEn = string.IsNullOrEmpty(_Data.ThirdNameEn) ? _Data.ThirdNameEn : _Data.ThirdNameAr;
                        LecturerObj.PhoneNumber = _Data.PhoneNumber;
                        LecturerObj.Email = _Data.Email;
                        LecturerObj.Gender = _Data.Gender; // 0 : Female , 1 : Male
                        LecturerObj.Address = _Data.Address;
                        //LecturerObj.BranchID = _Data.BranchID;
                        LecturerObj.UpdatedDate = DateTime.Now;

                        if (Request.Files["ProfilePic"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["ProfilePic"]);
                            if (Respo.Code == 1)
                            {
                                LecturerObj.ProfilePic = Respo.Message;
                            }
                        }

                        TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 2).SingleOrDefault();
                        if (_UserCred != null)
                        {
                            _UserCred.Password = _UserCred.Password;
                        }
                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات المحاضر بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { LecturerID = _Data.LecturerID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { LecturerID = _Data.LecturerID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { LecturerID = _Data.LecturerID });
            }
        }

        public JsonResult getLecturers(int SubjectID)
        {
            try
            {
                string Result = "";
                List<TblLecturer> _LecturersList = _Context.TblLecturerSubjects.Where(a => a.SubjectID == SubjectID && a.IsDeleted != true).Select(a => a.TblLecturer).ToList();
                Result += "<option value='0'>المحاضر</option>";
                foreach (var lecturer in _LecturersList)
                {
                    Result += "<option value=' " + lecturer.ID + "'>" + (lecturer.FirstNameAr + " " + lecturer.SecondNameAr + " " + lecturer.ThirdNameAr) + "</option>";
                }

                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
        }

        public JsonResult getLecturersBySubjectID(int SubjectID)
        {
            try
            {
                List<TblLecturer> _LecturersList = _Context.TblLecturerSubjects.Where(a => a.SubjectID == SubjectID && a.IsDeleted != true).Select(a => a.TblLecturer).ToList();
                List<LecturerData> Data = new List<LecturerData>();

                //Result += "<option value='0'>المحاضر</option>";
                foreach (var lecturer in _LecturersList)
                {
                    LecturerData data = new LecturerData()
                    {
                        LecturerID = lecturer.ID,
                        Name = lecturer.FirstNameAr + " " + lecturer.SecondNameAr + " " + lecturer.ThirdNameAr
                    };
                    Data.Add(data);
                }

                return Json(Data);
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
        }

        [HttpGet]
        public ActionResult AssignSubjects()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<TblCollege> CollegesList = _Context.TblColleges.Where(a => a.IsDeleted != true).ToList();
            //List<TblMajor> MajorsList = _Context.TblMajors.Where(a => a.IsDeleted != true).ToList();
            //List<TblSubject> SubjectsList = _Context.TblSubjects.Where(a => a.IsDeleted != true).ToList();
            List<TblLecturer> LecturersList = _Context.TblLecturers.Where(a => a.IsDeleted != true).ToList();

            ViewBag.CollegesList = CollegesList;
            ViewBag.LecturersList = LecturersList;
            //return View(SubjectsList);
            return View();
        }

        [HttpPost]
        public ActionResult AssignSubjects(FormCollection _Data)
        {
            try
            {
                if (int.Parse(_Data["SubjectID"]) > 0 && !string.IsNullOrEmpty(_Data["LecturerIDs []"]))
                {
                    string[] LecturerIDsList = _Data["LecturerIDs []"].Split(',');

                    for (int i = 0; i < LecturerIDsList.Count(); i++)
                    {
                        TblLecturerSubject LecSubObj = new TblLecturerSubject()
                        {
                            SubjectID = int.Parse(_Data["SubjectID"]),
                            LecturerID = int.Parse(LecturerIDsList[i]),
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        _Context.TblLecturerSubjects.Add(LecSubObj);
                    }

                    _Context.SaveChanges();

                    TempData["notice"] = "تم تعيين الماده بنجاح";
                    return RedirectToAction("AssignSubjects");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("AssignSubjects");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("AssignSubjects");
            }
        }

        public string CheckUserNameValidation(string UserName)
        {
            try
            {
                TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == UserName && a.UserType == 2).SingleOrDefault();
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
                        var uploadUrl = Server.MapPath("~/Content/Images/Lecturer/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        //Session["ProfilePic"] = fileName;

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/Lecturer/" + fileName;
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

        public JsonResult Delete(int LecturerID)
        {
            try
            {
                if (LecturerID > 0)
                {
                    //TempData["notice"] = "غير قادر علي الحذف, المحافظه مرتيطه ببيانات في المناطق والطلاب";
                    //return Json("OK");
                    TblLecturer LecturerObj = _Context.TblLecturers.Where(a => a.ID == LecturerID).SingleOrDefault();
                    if (LecturerObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف المحاضر بنجاح";

                            _Context.TblUserCredentials.Remove(_Context.TblUserCredentials.Where(a => a.ID == LecturerID).FirstOrDefault());
                            _Context.SaveChanges();

                            _Context.TblLecturers.Remove(LecturerObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, المحاضر مرتبط ببيانات خاصه بالمواد وطرق الحساب والمحاضرات والسندات اليدوية والحصص الخاصه";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Lecturer not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Lecturer not found!";
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