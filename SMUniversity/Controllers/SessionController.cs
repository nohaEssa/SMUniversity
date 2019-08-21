using QRCoder;
using SMUModels;
using SMUModels.Enum;
using SMUModels.Handlers;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class SessionController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Session
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult SessionsList()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult CoursesList()
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
                List<TblHall> _HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();

                ViewBag._UniversityList = _UniversityList;
                ViewBag._HallsList = _HallsList;

                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
            
        }

        [HttpPost]
        public ActionResult Create(SessionObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.Name) && !string.IsNullOrEmpty(_Data.FromDate) && !string.IsNullOrEmpty(_Data.ToDate) && _Data.Type > 0 && (_Data.Cost > 0 || (_Data.Price1 > 0 && _Data.Price2 > 0 && _Data.Price3 > 0)) && _Data.SubjectID > 0 && _Data.LecturerID > 0 && _Data.HallID > 0)
                {
                    if(_Data.Price1>_Data.Price2 && _Data.Price1 > _Data.Price3 && _Data.Price2 > _Data.Price3)
                    {
                        Random generator = new Random();
                        TblSession _SessionObj = new TblSession()
                        {
                            Name = _Data.Name,
                            NameEn = _Data.NameEn,
                            Description = _Data.Description,
                            DescriptionEn = _Data.DescriptionEn,
                            FromDate = DateTime.Parse(_Data.FromDate),
                            ToDate = DateTime.Parse(_Data.ToDate),
                            LecturerAccountMethod = _Data.LecturerAccountMethod,
                            Type = _Data.Type == 1 ? true : false,
                            SubjectID = _Data.SubjectID,
                            LecturerID = _Data.LecturerID,
                            //BranchID = _Data.BranchID,
                            BranchID = 3,
                            LecturesCount = _Data.Type == 1 ? 1 : _Data.CourseFromTime.Length,
                            HallID = _Data.HallID,
                            SessionCode = generator.Next(0, 99999).ToString("D5"),
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        if (Request.Files["Picture"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                            if (Respo.Code == 1)
                            {
                                _SessionObj.Picture = Respo.Message;
                            }
                        }

                        _Context.TblSessions.Add(_SessionObj);
                        _Context.SaveChanges();


                        TblSessionDetail _SessionDetailsObj = new TblSessionDetail()
                        {
                            SessionID = _SessionObj.ID,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        if (_Data.Type == 1) //session
                        {
                            _SessionDetailsObj.Price1 = _Data.Price1;
                            _SessionDetailsObj.Price2 = _Data.Price2;
                            _SessionDetailsObj.Price3 = _Data.Price3;
                            _SessionDetailsObj.Cost = _Data.Price3;

                        }
                        else //cource
                        {
                            _SessionDetailsObj.Cost = _Data.Price1;
                        }

                        _Context.TblSessionDetails.Add(_SessionDetailsObj);
                        _Context.SaveChanges();

                        _SessionDetailsObj.QRCode = SaveQRCode((_SessionDetailsObj.ID.ToString() + "," + _Data.Type), string.Format("{0}_QRCode_", _SessionObj.Name));
                        _Context.SaveChanges();

                        if (_Data.Type == 1)
                        {
                            TblSessionTime _SessionTimesObj = new TblSessionTime()
                            {
                                SessionID = _SessionObj.ID,
                                FromTime = DateTime.Parse(_Data.FromDate),
                                ToTime = DateTime.Parse(_Data.ToDate),
                                LectureAr = "المحاضره الأولي",
                                LectureEn = "1st Lecture",
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };
                            _Context.TblSessionTimes.Add(_SessionTimesObj);
                        }
                        else
                        {
                            for (int i = 0; i < _Data.CourseFromTime.ToArray().Length; i++)
                            {
                                TblSessionTime _SessionTimesObj = new TblSessionTime()
                                {
                                    SessionID = _SessionObj.ID,
                                    FromTime = DateTime.Parse(_Data.CourseFromTime[i]),
                                    ToTime = DateTime.Parse(_Data.CourseToTime[i]),
                                    Ended = false,
                                    IsDeleted = false,
                                    CreatedDate = DateTime.Now
                                };

                                switch (i)
                                {
                                    case 0:
                                        _SessionTimesObj.LectureAr = "المحاضره الأولي";
                                        _SessionTimesObj.LectureEn = "1st Lecture";
                                        break;
                                    case 1:
                                        _SessionTimesObj.LectureAr = "المحاضره الثانيه";
                                        _SessionTimesObj.LectureEn = "2nd Lecture";
                                        break;
                                    case 2:
                                        _SessionTimesObj.LectureAr = "المحاضره الثالثه";
                                        _SessionTimesObj.LectureEn = "3rd Lecture";
                                        break;
                                    case 3:
                                        _SessionTimesObj.LectureAr = "المحاضره الرابعه";
                                        _SessionTimesObj.LectureEn = "4th Lecture";
                                        break;
                                    case 4:
                                        _SessionTimesObj.LectureAr = "المحاضره الخامسه";
                                        _SessionTimesObj.LectureEn = "5th Lecture";
                                        break;
                                    case 5:
                                        _SessionTimesObj.LectureAr = "المحاضره السادسه";
                                        _SessionTimesObj.LectureEn = "6th Lecture";
                                        break;
                                    case 6:
                                        _SessionTimesObj.LectureAr = "المحاضره السابعه";
                                        _SessionTimesObj.LectureEn = "7th Lecture";
                                        break;
                                    case 7:
                                        _SessionTimesObj.LectureAr = "المحاضره الثامنه";
                                        _SessionTimesObj.LectureEn = "8th Lecture";
                                        break;
                                    case 8:
                                        _SessionTimesObj.LectureAr = "المحاضره التاسعه";
                                        _SessionTimesObj.LectureEn = "9th Lecture";
                                        break;
                                    case 9:
                                        _SessionTimesObj.LectureAr = "المحاضره العاشره";
                                        _SessionTimesObj.LectureEn = "10th Lecture";
                                        break;
                                }

                                _Context.TblSessionTimes.Add(_SessionTimesObj);
                            }
                        }

                        _Context.SaveChanges();

                        TempData["notice"] = "تم إضافة المحاضره بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "من فضلك ادخل اسعار صحيحه بحيث يكون المبلغ الاول اكبر من المبلغ الثاني والثالث والملبغ الثاني اكبر من المبلغ الثالث";
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

        public ActionResult Edit(int SessionID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                ViewBag.Flag = 0;

                List<TblUniversity> _UniversityList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                List<TblHall> _HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();
                TblSession _Session = _Context.TblSessions.Where(a => a.ID == SessionID).FirstOrDefault();

                ViewBag._UniversityList = _UniversityList;
                ViewBag._HallsList = _HallsList;
                if(_Session.FromDate.Date < DateTime.Now.Date || (_Session.FromDate.Date == DateTime.Now.Date && _Session.FromDate.Hour >= DateTime.Now.Hour))
                {
                    ViewBag.Flag = 1;
                    TempData["notice"] = "لقد انتهت المحاضره انت غير قادر علي التعديل عليها";
                }
                else
                {
                    ViewBag.Flag = 0;
                }

                return View(_Session);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { SessionID = SessionID });
            }

        }

        [HttpPost]
        public ActionResult Edit(SessionObj _Data)
        {
            try
            {
                bool Flag = false;
                if (_Data.SessionID > 0 && !string.IsNullOrEmpty(_Data.Name) && !string.IsNullOrEmpty(_Data.FromDate) && !string.IsNullOrEmpty(_Data.ToDate) && _Data.Type > 0 && (_Data.Cost > 0 || (_Data.Price1 > 0 && _Data.Price2 > 0 && _Data.Price3 > 0)) && _Data.SubjectID > 0 && _Data.LecturerID > 0 && _Data.HallID > 0)
                {
                    if (_Data.Type == 1 && (_Data.Price1 > _Data.Price2 && _Data.Price1 > _Data.Price3 && _Data.Price2 > _Data.Price3))
                    {
                        Flag = true;
                    }
                    else if (_Data.Type == 0 && _Data.Cost > 0) 
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                    if (Flag)
                    {
                        Random generator = new Random();
                        TblSession _SessionObj = new TblSession()
                        {
                            Name = _Data.Name,
                            NameEn = _Data.NameEn,
                            Description = _Data.Description,
                            DescriptionEn = _Data.DescriptionEn,
                            FromDate = DateTime.Parse(_Data.FromDate),
                            ToDate = DateTime.Parse(_Data.ToDate),
                            LecturerAccountMethod = _Data.LecturerAccountMethod,
                            Type = _Data.Type == 1 ? true : false,
                            SubjectID = _Data.SubjectID,
                            LecturerID = _Data.LecturerID,
                            //BranchID = _Data.BranchID,
                            BranchID = 3,
                            LecturesCount = _Data.Type == 1 ? 1 : _Data.CourseFromTime.Length,
                            HallID = _Data.HallID,
                            SessionCode = generator.Next(0, 99999).ToString("D5"),
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        if (Request.Files["Picture"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                            if (Respo.Code == 1)
                            {
                                _SessionObj.Picture = Respo.Message;
                            }
                        }

                        _Context.TblSessions.Add(_SessionObj);
                        _Context.SaveChanges();


                        TblSessionDetail _SessionDetailsObj = new TblSessionDetail()
                        {
                            SessionID = _SessionObj.ID,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        if (_Data.Type == 1) //session
                        {
                            _SessionDetailsObj.Price1 = _Data.Price1;
                            _SessionDetailsObj.Price2 = _Data.Price2;
                            _SessionDetailsObj.Price3 = _Data.Price3;
                            _SessionDetailsObj.Cost = _Data.Price3;

                        }
                        else //cource
                        {
                            _SessionDetailsObj.Cost = _Data.Price1;
                        }

                        _Context.TblSessionDetails.Add(_SessionDetailsObj);
                        _Context.SaveChanges();

                        _SessionDetailsObj.QRCode = SaveQRCode((_SessionDetailsObj.ID.ToString() + "," + _Data.Type), string.Format("{0}_QRCode_", _SessionObj.Name));
                        _Context.SaveChanges();

                        if (_Data.Type == 1)
                        {
                            TblSessionTime _SessionTimesObj = new TblSessionTime()
                            {
                                SessionID = _SessionObj.ID,
                                FromTime = DateTime.Parse(_Data.FromDate),
                                ToTime = DateTime.Parse(_Data.ToDate),
                                LectureAr = "المحاضره الأولي",
                                LectureEn = "1st Lecture",
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };
                            _Context.TblSessionTimes.Add(_SessionTimesObj);
                        }
                        else
                        {
                            for (int i = 0; i < _Data.CourseFromTime.ToArray().Length; i++)
                            {
                                TblSessionTime _SessionTimesObj = new TblSessionTime()
                                {
                                    SessionID = _SessionObj.ID,
                                    FromTime = DateTime.Parse(_Data.CourseFromTime[i]),
                                    ToTime = DateTime.Parse(_Data.CourseToTime[i]),
                                    Ended = false,
                                    IsDeleted = false,
                                    CreatedDate = DateTime.Now
                                };

                                switch (i)
                                {
                                    case 0:
                                        _SessionTimesObj.LectureAr = "المحاضره الأولي";
                                        _SessionTimesObj.LectureEn = "1st Lecture";
                                        break;
                                    case 1:
                                        _SessionTimesObj.LectureAr = "المحاضره الثانيه";
                                        _SessionTimesObj.LectureEn = "2nd Lecture";
                                        break;
                                    case 2:
                                        _SessionTimesObj.LectureAr = "المحاضره الثالثه";
                                        _SessionTimesObj.LectureEn = "3rd Lecture";
                                        break;
                                    case 3:
                                        _SessionTimesObj.LectureAr = "المحاضره الرابعه";
                                        _SessionTimesObj.LectureEn = "4th Lecture";
                                        break;
                                    case 4:
                                        _SessionTimesObj.LectureAr = "المحاضره الخامسه";
                                        _SessionTimesObj.LectureEn = "5th Lecture";
                                        break;
                                    case 5:
                                        _SessionTimesObj.LectureAr = "المحاضره السادسه";
                                        _SessionTimesObj.LectureEn = "6th Lecture";
                                        break;
                                    case 6:
                                        _SessionTimesObj.LectureAr = "المحاضره السابعه";
                                        _SessionTimesObj.LectureEn = "7th Lecture";
                                        break;
                                    case 7:
                                        _SessionTimesObj.LectureAr = "المحاضره الثامنه";
                                        _SessionTimesObj.LectureEn = "8th Lecture";
                                        break;
                                    case 8:
                                        _SessionTimesObj.LectureAr = "المحاضره التاسعه";
                                        _SessionTimesObj.LectureEn = "9th Lecture";
                                        break;
                                    case 9:
                                        _SessionTimesObj.LectureAr = "المحاضره العاشره";
                                        _SessionTimesObj.LectureEn = "10th Lecture";
                                        break;
                                }

                                _Context.TblSessionTimes.Add(_SessionTimesObj);
                            }
                        }

                        _Context.SaveChanges();

                        TempData["notice"] = "تم إضافة المحاضره بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "من فضلك ادخل اسعار صحيحه بحيث يكون المبلغ الاول اكبر من المبلغ الثاني والثالث والملبغ الثاني اكبر من المبلغ الثالث";
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
                    FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Images/Session/") + fileName, FileMode.Create);
                    ms.WriteTo(fs);

                    ms.Close();
                    fs.Close();
                    fs.Dispose();
                    return "Content/Images/Session/" + fileName;
                }
            }

            return "";
        }

        public ActionResult ViewSessions(int SessionID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<TblSessionTime> _Sessions = _Context.TblSessionTimes.Where(a => a.SessionID == SessionID).ToList();
            return View(_Sessions);
        }

        public JsonResult EndSession(int SessionTimeID)
        {
            try
            {
                TblSessionTime _SessionTime = _Context.TblSessionTimes.Where(a => a.ID == SessionTimeID).SingleOrDefault();
                _SessionTime.Ended = true;
                _Context.SaveChanges();

                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
            
        }

        public ActionResult PrivateSessions()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            _Context.SaveChanges();

            return View();
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
                        var uploadUrl = Server.MapPath("~/Content/Images/Session/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));
                        //Session["ProfilePic"] = fileName;

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/Session/" + fileName;
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