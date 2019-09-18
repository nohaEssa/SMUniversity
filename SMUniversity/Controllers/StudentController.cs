using SMUModels;
using SMUModels.Classes;
using SMUModels.Handlers;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
                List<TblCardCategory> _CardCategoryList = _Context.TblCardCategories.Where(a => a.ForApplication == true).ToList();

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
                if ((!string.IsNullOrEmpty(_Data.FirstName) && !string.IsNullOrEmpty(_Data.SecondName) && !string.IsNullOrEmpty(_Data.ThirdName) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && !string.IsNullOrEmpty(_Data.UserName) && !string.IsNullOrEmpty(_Data.Password) && _Data.AreaID > 0 && _Data.CollegeID > 0 && _Data.GovernorateID > 0 && _Data.MajorID > 0 && _Data.UniversityID > 0 && _Data.StudentType == false) || (!string.IsNullOrEmpty(_Data.FirstName) && !string.IsNullOrEmpty(_Data.SecondName) && !string.IsNullOrEmpty(_Data.ThirdName) && !string.IsNullOrEmpty(_Data.Email) && !string.IsNullOrEmpty(_Data.PhoneNumber) && !string.IsNullOrEmpty(_Data.UserName) && !string.IsNullOrEmpty(_Data.Password) && _Data.AreaID > 0 && _Data.GovernorateID > 0 && _Data.UniversityID > 0 && _Data.StudentType == true))
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.UserType == 1).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Data.PhoneNumber).FirstOrDefault();
                        if (Std == null)
                        {
                            TblStudent StudentObj = new TblStudent()
                            {
                                FirstName = _Data.FirstName,
                                SecondName = _Data.SecondName,
                                ThirdName = _Data.ThirdName,
                                FirstNameEn = !string.IsNullOrEmpty(_Data.FirstNameEn) ? _Data.FirstNameEn : _Data.FirstName,
                                SecondNameEn = !string.IsNullOrEmpty(_Data.SecondNameEn) ? _Data.SecondNameEn : _Data.SecondName,
                                ThirdNameEn = !string.IsNullOrEmpty(_Data.ThirdNameEn) ? _Data.ThirdNameEn : _Data.ThirdName,
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
                                StudentType = _Data.StudentType,
                                Balance = _Data.CardCategoryID > 0 ? _Context.TblCardCategories.Where(a => a.ID == _Data.CardCategoryID).FirstOrDefault().Price : 0,
                                CreatedDate = DateTime.Now,
                            };
                            try
                            {
                                StudentObj.DateOfBirth = _Data.DateOfBirth.Date;
                            }
                            catch (Exception)
                            {
                            }
                            if (_Data.CardCategoryID > 0)
                            {
                                StudentObj.Balance = _Context.TblCardCategories.Where(a => a.ID == _Data.CardCategoryID).FirstOrDefault().Price;
                            }
                            else if (_Data.CardCategoryID == -1)
                            {
                                StudentObj.Balance = _Data.NewChargeValue;
                            }
                            else if (_Data.CardCategoryID == 0)
                            {
                                StudentObj.Balance = 0;
                            }

                            if (Request.Files["StudentPic"] != null)
                            {
                                UploadFileResponse Respo = UploadPhoto(Request.Files["StudentPic"]);
                                if (Respo.Code == 1)
                                {
                                    StudentObj.ProfilePic = Respo.Message;
                                }
                                else
                                {
                                    StudentObj.ProfilePic = "https://uni.smartmindkw.com/Content/Images/Student/DefaultStudentPhoto.png";
                                }
                            }
                            else
                            {
                                StudentObj.ProfilePic = "https://uni.smartmindkw.com/Content/Images/Student/DefaultStudentPhoto.png";
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

                            try
                            {
                                if (StudentObj.Balance > 0)
                                {
                                    TblInvoice _invoiceObj = new TblInvoice()
                                    {
                                        StudentID = StudentObj.ID,
                                        RealCash = false,
                                        Price = StudentObj.Balance,
                                        Pending = false,
                                        PaymentMethod = "Cash",
                                        IsDeleted = false,
                                        CreatedDate = DateTime.Now
                                    };

                                    int Serial;
                                    var CountVouchers = _Context.TblInvoices.Count();
                                    if (CountVouchers > 0)
                                    {
                                        //List<TblVoucher> lastcode = _Context.TblVouchers.ToList();
                                        long MyMax = _Context.TblInvoices.Max(a => a.Serial);

                                        Serial = int.Parse(MyMax.ToString()) + 1;
                                        _invoiceObj.Serial = Serial;
                                    }
                                    else
                                    {
                                        _invoiceObj.Serial = 1;
                                    }

                                    _Context.TblInvoices.Add(_invoiceObj);
                                    _Context.SaveChanges();
                                }
                            }
                            catch (Exception)
                            {
                            }
                            
                            var res = SendCodeSMS(StudentObj.PhoneNumber, _Data.UserName, _Data.Password);
                            if (res.IsSuccessful)
                            {
                                StudentObj.Verified = true;
                                StudentObj.VerificationCode = res.Result.ToString();

                                _Context.SaveChanges();
                            }
                            try
                            {
                                SendEmail(StudentObj.Email, StudentObj.FirstName + " " + StudentObj.SecondName + " " + StudentObj.ThirdName, _Data.UserName, _Data.Password);
                            }
                            catch (Exception ex)
                            {
                            }

                            string TitleAr = "سمارت مايند الجامعه";
                            string TitleEn = "SmartMind University";
                            string DescriptionAr = "تم إنشاء حساب لك, اسم المستخدم : " + _Data.UserName + " , الرقم السري : " + _Data.Password;
                            string DescriptionEn = "An account has been created for you, UserName : " + _Data.UserName + " , Password : " + _Data.Password;

                            Push(StudentObj.ID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                            TempData["notice"] = "تم إضافه الطالب بنجاح";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["notice"] = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
                            return RedirectToAction("Create");
                        }
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
                    ViewBag.Gender = StdObj.Gender ? "1" : "0";
                    //ViewBag.StudentType = StdObj.StudentType ? "1" : "0";
                    List<TblCardCategory> _CardCategoryList = _Context.TblCardCategories.Where(a => a.ForApplication == true).ToList();

                    ViewBag.UniversityList = _UniversityList;
                    ViewBag.GovernorateList = _GovernorateList;
                    ViewBag.CardCategoryList = _CardCategoryList;

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
        public ActionResult Edit(FormCollection _Data)
        {
            int StudentID = int.Parse(_Data["StudentID"]);
            try
            {
                if (StudentID > 0 && !string.IsNullOrEmpty(_Data["FirstName"]) && !string.IsNullOrEmpty(_Data["SecondName"]) && !string.IsNullOrEmpty(_Data["ThirdName"]) && !string.IsNullOrEmpty(_Data["Email"]) && !string.IsNullOrEmpty(_Data["PhoneNumber"]) && !string.IsNullOrEmpty(_Data["Password"]) && int.Parse(_Data["AreaID"]) > 0 && int.Parse(_Data["CollegeID"]) > 0 && int.Parse(_Data["GovernorateID"]) > 0 && int.Parse(_Data["MajorID"]) > 0 && int.Parse(_Data["UniversityID"]) > 0)
                {
                    string PhoneNumber = _Data["PhoneNumber"];
                    string UserName = _Data["UserName"];
                    int CardCategoryID = int.Parse(_Data["CardCategoryID"]);
                    TblStudent std = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();
                    if (std != null)
                    {
                        TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == PhoneNumber && a.ID != StudentID).FirstOrDefault();
                        if (Std == null)
                        {
                            std.FirstName = _Data["FirstName"];
                            std.SecondName = _Data["SecondName"];
                            std.ThirdName = _Data["ThirdName"];
                            std.FirstNameEn = !string.IsNullOrEmpty(_Data["FirstNameEn"]) ? _Data["FirstNameEn"] : _Data["FirstName"];
                            std.SecondNameEn = !string.IsNullOrEmpty(_Data["SecondNameEn"]) ? _Data["SecondNameEn"] : _Data["SecondName"];
                            std.ThirdNameEn = !string.IsNullOrEmpty(_Data["ThirdNameEn"]) ? _Data["ThirdNameEn"] : _Data["ThirdName"];
                            std.PhoneNumber = PhoneNumber;
                            std.Email = _Data["Email"];
                            if (CardCategoryID > 0)
                            {
                                std.Balance += _Context.TblCardCategories.Where(a => a.ID == CardCategoryID).FirstOrDefault().Price;
                            }
                            else if (CardCategoryID == -1)
                            {
                                std.Balance += decimal.Parse(_Data["NewChargeValue"]);
                            }
                            else if (CardCategoryID == 0)
                            {
                                std.Balance += 0;
                            }
                            //std.Balance += CardCategoryID > 0 ? _Context.TblCardCategories.Where(a => a.ID == CardCategoryID).FirstOrDefault().Price : 0;
                            //std.DateOfBirth = _Data.DateOfBirth.Date;
                            if (_Data["Gender"] != null)
                            {
                                std.Gender = bool.Parse(_Data["Gender"]); // 0 : Female , 1 : Male
                            }
                            try
                            {
                                std.DateOfBirth = DateTime.Parse(_Data["DateOfBirth"]).Date;
                            }
                            catch (Exception)
                            {
                            }
                            std.UniversityID = int.Parse(_Data["UniversityID"]);
                            std.CollegeID = int.Parse(_Data["CollegeID"]);
                            std.MajorID = int.Parse(_Data["MajorID"]);
                            std.GovernorateID = int.Parse(_Data["GovernorateID"]);
                            std.AreaID = int.Parse(_Data["AreaID"]);
                            //std.StudentType = bool.Parse(_Data["StudentType"]);
                            std.UpdatedDate = DateTime.Now;

                            if (Request.Files["StudentPic"] != null)
                            {
                                UploadFileResponse Respo = UploadPhoto(Request.Files["StudentPic"]);
                                if (Respo.Code == 1)
                                {
                                    std.ProfilePic = Respo.Message;
                                }
                            }

                            TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == UserName && a.UserType == 1).SingleOrDefault();
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
                            TempData["notice"] = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
                            return RedirectToAction("Edit", new { StudentID = StudentID });
                        }

                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { StudentID = StudentID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { StudentID = StudentID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { StudentID = StudentID });
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

        public JsonResult RechargeBalance(int StudentID, decimal BalanceValue)
        {
            try
            {
                if (StudentID > 0)
                {
                    TblStudent StudentObj = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();
                    if (StudentObj != null)
                    {
                        StudentObj.Balance = BalanceValue;
                        _Context.SaveChanges();

                        TempData["notice"] = "تم شحن رصيد حساب " + StudentObj.FirstName + " " + StudentObj.SecondName + " " + StudentObj.ThirdName + " بمبلغ : " + BalanceValue + " بنجاح, الرصيد الحالي : " + StudentObj.Balance;
                        return Json("OK");
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

        public ActionResult Profile(int StudentID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        private ResultHandler SendCodeSMS(string PhoneNumber, string UserName, string Password)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                ////send sms for verification code
                //Random generator = new Random();
                //string Code = generator.Next(0, 99999).ToString("D5");
                string Message = "تم إنشاء حساب لك, اسم المستخدم : " + UserName + " , الرقم السري : " + Password;

                string TargetPhone = PhoneNumber.StartsWith("965") ? PhoneNumber : ("965" + PhoneNumber);
                //WebRequest request = WebRequest.Create("http://api-server3.com/api/send.aspx?username=eze-sms&password=ezesms2547&language=2&sender=SmartMind&mobile=" + TargetPhone + "&message=" + Code);
                WebRequest request = WebRequest.Create("http://api-server3.com/api/send.aspx?username=smartmind&password=smart254mind&language=2&sender=smartmind&mobile=" + TargetPhone + "&message=" + Message);

                // If required by the server, set the credentials.  
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                var status = ((HttpWebResponse)response).StatusDescription;
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                var content = responseFromServer;
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();

                _resultHandler.IsSuccessful = true;
                _resultHandler.MessageAr = "OK";
                _resultHandler.Result = Message;
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;
            }

            return _resultHandler;
        }

        private void Push(int StudentID, int LecturerID, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, int NotTypeID)
        {
            try
            {
                bool res = PushNotification.Push(StudentID, LecturerID, TitleAr, TitleEn, DescriptionAr, DescriptionEn, NotTypeID);
                var regNotification = new TblNotification { StudentID = StudentID, TitleAr = TitleAr, TitleEn = TitleEn, DescriptionAr = DescriptionAr, DescriptionEn = DescriptionEn, CreatedDate = DateTime.Now };

                _Context.TblNotifications.Add(regNotification);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
            }

        }

        public void SendEmail(string Email, string Name, string UserName, string Password)
        {
            try
            {
                string body1 = string.Empty;
                string body2 = string.Empty;

                //using (var sr = new StreamReader(Server.MapPath("\\App_Data\\Templates/emailPart1.html")))
                //{
                //    body1 = sr.ReadToEnd();
                //}
                //using (var sr = new StreamReader(Server.MapPath("\\App_Data\\Templates/emailPart2.html")))
                //{
                //    body2 = sr.ReadToEnd();
                //}
                string Linssss = "smartmindkw.com";
                string Linkssname = " موقعنا  ";
                //string Name = Stu.firstName + Stu.SecondName;

                string emailSubject = " مرحباً بك فى سمارت مايند الجامعه ";
                //string Message = "مرحباً بك فى اسرة سمارت مايند يمكنك الدخول اللى حسابك على موقعه من اللينك السابق بيانات الحساب : " + "\n" + "اسم السمتخدم:" + Stu.UserName + "--" + ":كلمة المرور" + Stu.Password;
                string Message = "تم التسجيل بنجاح‬ ‫شكرا لثقتكم ونعدكم بالأفضل‬‫.‬‫للشكاوى : ‬‫واتساب 66662617 " + " يمكنك الدخول إلي حسابك على الموقع والتطبيق من اللينك السابق بيانات الحساب : " + "\n" + "اسم المستخدم:" + UserName + "--" + ":كلمة المرور" + Password;
                string sender = ConfigurationManager.AppSettings["EmailFromAddress"];

                string messageBody = body1 + "<br/>" + Message + "<br/>" + Linkssname + Linssss + body2;
                var MailHelper = new MailHelper
                {
                    Sender = sender, //email.Sender,
                    Recipient = Email,
                    RecipientCC = null,
                    Subject = emailSubject,
                    Body = messageBody
                };
                MailHelper.Send();

            }
            catch (Exception)
            {
            }
        }
    }
}