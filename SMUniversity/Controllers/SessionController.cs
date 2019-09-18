using QRCoder;
using SMUModels;
using SMUModels.Classes;
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
                DateTime CurrentDate = DateTime.Now.Date;
                List<TblUniversity> _UniversityList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                List<TblHall> _HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();
                List<HallData> Data = new List<HallData>();
                foreach (var item in _HallsList)
                {
                    HallData data = new HallData();
                    TblSession _SessionObj = _Context.TblSessions.Where(a => a.HallID == item.ID && a.IsDeleted != true && DbFunctions.TruncateTime(a.FromDate) <= CurrentDate && DbFunctions.TruncateTime(a.ToDate) >= CurrentDate).FirstOrDefault();
                    data.HallID = item.ID;
                    data.HallCodeAr = item.HallCodeAr;
                    if (_SessionObj == null)
                    {
                        data.Available = true;
                    }
                    else
                    {
                        data.Available = false;
                    }

                    Data.Add(data);
                }
                ViewBag._UniversityList = _UniversityList;
                ViewBag._HallsList = Data;
                //ViewBag._HallsList = _HallsList;

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
            bool Result = false;
            try
            {
                if (!string.IsNullOrEmpty(_Data.Name) && !string.IsNullOrEmpty(_Data.FromDate) && !string.IsNullOrEmpty(_Data.ToDate) && _Data.Type > 0 && _Data.SubjectID > 0 && _Data.LecturerID > 0 && _Data.HallID > 0)
                //if ((!string.IsNullOrEmpty(_Data.Name) && !string.IsNullOrEmpty(_Data.FromDate) && !string.IsNullOrEmpty(_Data.ToDate) && _Data.Type > 0 && _Data.SubjectID > 0 && _Data.LecturerID > 0 && _Data.HallID > 0 && _Data.GeneralSession == false) || (!string.IsNullOrEmpty(_Data.Name) && !string.IsNullOrEmpty(_Data.FromDate) && !string.IsNullOrEmpty(_Data.ToDate) && _Data.Type > 0 && _Data.HallID > 0 && _Data.GeneralSession == true))
                {
                    if (_Data.Type == 1)//session
                    {
                        if ((_Data.Price1 > 0 && _Data.Price2 > 0 && _Data.Price3 > 0) && (_Data.Price1 > _Data.Price2 && _Data.Price1 > _Data.Price3 && _Data.Price2 > _Data.Price3))
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            TempData["notice"] = "من فضلك ادخل اسعار صحيحه بحيث يكون المبلغ الاول اكبر من المبلغ الثاني والثالث والملبغ الثاني اكبر من المبلغ الثالث";
                            return RedirectToAction("Create");
                        }
                    }
                    else
                    {
                        if (_Data.Cost > 0 && _Data.SessionPrice > 0)
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            TempData["notice"] = "من فضلك ادخل سعر للكورس والسعر في حالة السيشن الواحده";
                            return RedirectToAction("Create");
                        }
                    }
                        //if(_Data.Price1>_Data.Price2 && _Data.Price1 > _Data.Price3 && _Data.Price2 > _Data.Price3)
                        //{
                        if (Result)
                    {
                        Random generator = new Random();
                        TblSession _SessionObj = new TblSession()
                        {
                            Name = _Data.Name,
                            NameEn = _Data.NameEn,
                            Description = !string.IsNullOrEmpty(_Data.Description) ? _Data.Description : "",
                            DescriptionEn = !string.IsNullOrEmpty(_Data.DescriptionEn) ? _Data.DescriptionEn : _Data.Description,
                            FromDate = DateTime.Parse(_Data.FromDate),
                            ToDate = DateTime.Parse(_Data.ToDate),
                            LecturerAccountMethod = _Data.LecturerAccountMethod,
                            Type = _Data.Type == 1 ? true : false,
                            GeneralSession = _Data.GeneralSession,
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
                        //if (!_Data.GeneralSession)
                        //{
                        //    _SessionObj.SubjectID = _Data.SubjectID;
                        //}
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
                            _SessionDetailsObj.Cost = _Data.Cost;
                            _SessionDetailsObj.SessionPrice = _Data.SessionPrice;
                        }

                        _Context.TblSessionDetails.Add(_SessionDetailsObj);
                        _Context.SaveChanges();

                        _SessionDetailsObj.SingleSessionQRCode = SaveQRCode((_SessionDetailsObj.ID.ToString() + "," + _Data.Type), string.Format("{0}_QRCode_", _SessionObj.Name));
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
                        try
                        {
                            if (_SessionObj.SubjectID != null) //not general session
                            {
                                List<TblStudent> Students = _Context.TblStudents.Where(a => a.MajorID == _SessionObj.TblSubject.MajorID).ToList();
                                foreach (TblStudent std in Students)
                                {
                                    string LectureTypeAr = _Data.Type == 1 ? "محاضره" : "دورة";
                                    string LectureTypeEn = _Data.Type == 1 ? " A Session" : "A Course";
                                    string TitleAr = "سمارت مايند الجامعه";
                                    string TitleEn = "SmartMind University";

                                    string DescriptionAr = "سيتم عقد " + LectureTypeAr + " بعنوان " + _SessionObj.Name + " في مادة " 
                                        + _SessionObj.TblSubject.NameAr + " يوم : " + _SessionObj.FromDate.ToString("dd-MM-yyy") 
                                        + " الساعه : " + _SessionObj.FromDate.ToString("hh:mm tt") + " للمحاضر : " 
                                        + _SessionObj.TblLecturer.FirstNameAr + " " + _SessionObj.TblLecturer.SecondNameAr + " " + _SessionObj.TblLecturer.ThirdNameAr 
                                        + " اذهب الي التايم لاين في التطبيق للمزيد من المعلومات";

                                    string DescriptionEn = LectureTypeEn + " : " + _SessionObj.NameEn + " in subject : " 
                                        + _SessionObj.TblSubject.NameEn + " will be held at " + _SessionObj.FromDate.ToString("dd-MM-yyy hh:mm tt") 
                                        + ", Lecturer : " + _SessionObj.TblLecturer.FirstNameEn + " " + _SessionObj.TblLecturer.SecondNameEn + " " 
                                        + _SessionObj.TblLecturer.ThirdNameEn + ". Please visit our application in timeline section for more information.";

                                    Push(std.ID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        TempData["notice"] = "تم إضافة المحاضره بنجاح";
                        if(_Data.Type == 1)
                        {
                            return RedirectToAction("SessionsList");
                        }
                        else
                        {
                            return RedirectToAction("CoursesList");
                        }
                    }
                    else
                    {
                        TempData["notice"] = "خطأ في إدخال البيانات";
                        return RedirectToAction("Create");
                    }
                }

                //}
                //else
                //{
                //    TempData["notice"] = "من فضلك ادخل اسعار صحيحه بحيث يكون المبلغ الاول اكبر من المبلغ الثاني والثالث والملبغ الثاني اكبر من المبلغ الثالث";
                //    return RedirectToAction("Create");
                //}

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
                DateTime CurrentDate = DateTime.Now.Date;
                List<TblUniversity> _UniversityList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                //List<TblHall> _HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();
                TblSession _Session = _Context.TblSessions.Where(a => a.ID == SessionID).FirstOrDefault();
                List<TblSessionTime> _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == SessionID).ToList();
                ViewBag._SessionFromTimes = string.Join(",", _SessionTimes.Select(a => a.FromTime).ToList());
                ViewBag._SessionToTimes = string.Join(",", _SessionTimes.Select(a => a.ToTime).ToList());
                //List<HallData> Data = new List<HallData>();
                //foreach (var item in _HallsList)
                //{
                //    HallData data = new HallData();
                //    TblSession _SessionObj = _Context.TblSessions.Where(a => a.HallID == item.ID && a.ID != SessionID && a.IsDeleted != true && DbFunctions.TruncateTime(a.FromDate) <= CurrentDate && DbFunctions.TruncateTime(a.ToDate) >= CurrentDate).FirstOrDefault();
                //    data.HallID = item.ID;
                //    data.HallCodeAr = item.HallCodeAr;
                //    if (_SessionObj == null)
                //    {
                //        data.Available = true;
                //    }
                //    else
                //    {
                //        data.Available = false;
                //    }

                //    Data.Add(data);
                //}
                //ViewBag._HallsList = Data;
                ////ViewBag._HallsList = _HallsList;
                ViewBag._UniversityList = _UniversityList;
                ViewBag.Type = _Session.Type ? "1" : "0";
                ViewBag.SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == SessionID).FirstOrDefault();
                if (_Session.FromDate.Date < DateTime.Now.Date || (_Session.FromDate.Date == DateTime.Now.Date && (_Session.FromDate.Hour - DateTime.Now.Hour == 1)))
                {
                    ViewBag.Flag = 1;
                    TempData["notice"] = "لقد انتهت المحاضره انت غير قادر علي التعديل عليها";
                }
                else
                {
                    ViewBag.Flag = 0;
                }
                //ViewBag.Flag = 0;
                return View(_Session);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { SessionID = SessionID });
            }

        }

        [HttpPost]
        public ActionResult Edit(FormCollection _Data)
        {
            try
            {
                int SessionID = int.Parse(_Data["SessionID"]);
                bool Result = false;
                if (SessionID > 0 && !string.IsNullOrEmpty(_Data["Name"]) && !string.IsNullOrEmpty(_Data["FromDate"]) && !string.IsNullOrEmpty(_Data["ToDate"]) && int.Parse(_Data["Type"]) > 0 && int.Parse(_Data["SubjectID"]) > 0 && int.Parse(_Data["LecturerID"]) > 0 && int.Parse(_Data["HallID"]) > 0)
                {
                    if (int.Parse(_Data["Type"]) == 1)//session
                    {
                        if ((decimal.Parse(_Data["Price1"]) > 0 && decimal.Parse(_Data["Price2"]) > 0 && decimal.Parse(_Data["Price3"]) > 0) && (decimal.Parse(_Data["Price1"]) > decimal.Parse(_Data["Price2"]) && decimal.Parse(_Data["Price1"]) > decimal.Parse(_Data["Price3"]) && decimal.Parse(_Data["Price2"]) > decimal.Parse(_Data["Price3"])))
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            TempData["notice"] = "من فضلك ادخل اسعار صحيحه بحيث يكون المبلغ الاول اكبر من المبلغ الثاني والثالث والملبغ الثاني اكبر من المبلغ الثالث";
                            return RedirectToAction("Edit", new { SessionID = SessionID });
                        }
                    }
                    else
                    {
                        if (decimal.Parse(_Data["Cost"]) > 0 && decimal.Parse(_Data["SessionPrice"]) > 0)
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            TempData["notice"] = "من فضلك ادخل سعر للكورس والسعر في حالة السيشن الواحده";
                            return RedirectToAction("Edit", new { SessionID = SessionID });
                        }
                    }
                    if (Result)
                    {
                        TblSession _SessionObj = _Context.TblSessions.Where(a => a.ID == SessionID).FirstOrDefault();

                        _SessionObj.Name = _Data["Name"];
                        _SessionObj.NameEn = _Data["NameEn"];
                        _SessionObj.Description = _Data["Description"];
                        _SessionObj.DescriptionEn = _Data["DescriptionEn"];
                        _SessionObj.FromDate = DateTime.Parse(_Data["FromDate"]);
                        _SessionObj.ToDate = DateTime.Parse(_Data["ToDate"]);
                        _SessionObj.LecturerAccountMethod = byte.Parse(_Data["LecturerAccountMethod"]);
                        _SessionObj.Type = int.Parse(_Data["Type"]) == 1 ? true : false;
                        //_SessionObj.GeneralSession = bool.Parse(_Data["GeneralSession"]);
                        _SessionObj.SubjectID = int.Parse(_Data["SubjectID"]);
                        _SessionObj.LecturerID = int.Parse(_Data["LecturerID"]);
                        _SessionObj.BranchID = 3;
                        //_SessionObj.LecturesCount = int.Parse(_Data["Type"]) == 1 ? 1 : _Data.CourseFromTime.Length;
                        _SessionObj.HallID = int.Parse(_Data["HallID"]);
                        _SessionObj.UpdatedDate = DateTime.Now;

                        if (Request.Files["Picture"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["Picture"]);
                            if (Respo.Code == 1)
                            {
                                _SessionObj.Picture = Respo.Message;
                            }
                        }

                        _Context.SaveChanges();

                        TblSessionDetail _SessionDetailsObj = _Context.TblSessionDetails.Where(a => a.SessionID == SessionID).FirstOrDefault();
                        if (int.Parse(_Data["Type"]) == 1) //session
                        {
                            _SessionDetailsObj.Price1 = decimal.Parse(_Data["Price1"]);
                            _SessionDetailsObj.Price2 = decimal.Parse(_Data["Price2"]);
                            _SessionDetailsObj.Price3 = decimal.Parse(_Data["Price3"]);
                            _SessionDetailsObj.Cost = decimal.Parse(_Data["Price3"]);

                        }
                        else //cource
                        {
                            _SessionDetailsObj.Cost = decimal.Parse(_Data["Cost"]);
                            _SessionDetailsObj.SessionPrice = decimal.Parse(_Data["SessionPrice"]);
                        }

                        _Context.SaveChanges();

                        _SessionDetailsObj.QRCode = SaveQRCode((_SessionDetailsObj.ID.ToString() + "," + int.Parse(_Data["Type"])), string.Format("{0}_QRCode_", _SessionObj.Name));
                        _Context.SaveChanges();

                        if (int.Parse(_Data["Type"]) == 1)
                        {
                            _Context.TblSessionTimes.Remove(_Context.TblSessionTimes.Where(a => a.SessionID == SessionID).FirstOrDefault());
                            _Context.SaveChanges();

                            TblSessionTime _SessionTimesObj = new TblSessionTime()
                            {
                                SessionID = _SessionObj.ID,
                                FromTime = DateTime.Parse(_Data["FromDate"]),
                                ToTime = DateTime.Parse(_Data["ToDate"]),
                                LectureAr = "المحاضره الأولي",
                                LectureEn = "1st Lecture",
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };
                            _Context.TblSessionTimes.Add(_SessionTimesObj);
                        }
                        else
                        {
                            _Context.TblSessionTimes.RemoveRange(_Context.TblSessionTimes.Where(a => a.SessionID == SessionID).ToList());
                            _Context.SaveChanges();

                            string[] SplittedResult1 = null;
                            string[] SplittedResult2 = null;

                            if (!string.IsNullOrEmpty(_Data["CourseFromTime"]))
                            {
                                SplittedResult1 = _Data["CourseFromTime"].Split(',');
                                SplittedResult2 = _Data["CourseToTime"].Split(',');
                            }

                            for (int i = 0; i < SplittedResult1.Length; i++)
                            {
                                TblSessionTime _SessionTimesObj = new TblSessionTime()
                                {
                                    SessionID = _SessionObj.ID,
                                    FromTime = DateTime.Parse(SplittedResult1[i]),
                                    ToTime = DateTime.Parse(SplittedResult2[i]),
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

                        TempData["notice"] = "تم تعديل المحاضره بنجاح";
                        if (int.Parse(_Data["Type"]) == 1)
                        {
                            return RedirectToAction("SessionsList");
                        }
                        else
                        {
                            return RedirectToAction("CoursesList");
                        }
                    }
                    else
                    {
                        TempData["notice"] = "خطأ في إدخال البيانات";
                        return RedirectToAction("Edit", new { SessionID = _Data["SessionID"] });
                    }

                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { SessionID = _Data["SessionID"] });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { SessionID = _Data["SessionID"] });
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
                    return "/Content/Images/Session/" + fileName;
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
                string TitleAr = "", TitleEn = "", DescriptionAr = "", DescriptionEn = "";

                TblSessionTime _SessionTime = _Context.TblSessionTimes.Where(a => a.ID == SessionTimeID).SingleOrDefault();
                _SessionTime.Ended = true;
                _Context.SaveChanges();

                TitleAr = "سمارت مايند الجامعه";
                TitleEn = "SmartMind University";
                DescriptionAr = "تم انتهاء " + _SessionTime.LectureAr + " التابعه لمحاضره " + _SessionTime.TblSession.Name;
                DescriptionEn = _SessionTime.LectureEn + " related to lecture : " + _SessionTime.TblSession.NameEn + " has ben ended";

                List<int?> StudentIDs = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionTime.SessionID && a != null).Select(a => a.StudentID).Cast<int?>().ToList();
                List<TblRegisterDevice> Devices = _Context.TblRegisterDevices.Where(a => StudentIDs.Contains(a.StudentID)).ToList();
                foreach (var device in Devices)
                {
                    Push(int.Parse(device.StudentID.ToString()), 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);
                }

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

        public JsonResult Delete(int SessionID)
        {
            try
            {
                if (SessionID > 0)
                {
                    TblSession SessionObj = _Context.TblSessions.Where(a => a.ID == SessionID).SingleOrDefault();
                    if (SessionObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف المحاضره بنجاح";

                            _Context.TblSessionPricings.RemoveRange(_Context.TblSessionPricings.Where(a => a.SessionID == SessionID).ToList());
                            _Context.SaveChanges();

                            _Context.TblSessionTimes.RemoveRange(_Context.TblSessionTimes.Where(a => a.SessionID == SessionID).ToList());
                            _Context.SaveChanges();

                            _Context.TblSessionDetails.RemoveRange(_Context.TblSessionDetails.Where(a => a.SessionID == SessionID).ToList());
                            _Context.SaveChanges();

                            _Context.TblSessions.Remove(SessionObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, المحاضره مرتبط ببيانات خاصه بالفواتير";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Session not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Session not found!";
                    return Json("ERROR");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return Json("ERROR");
            }
        }

        public ActionResult LecturesByPercentageList()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult GetSessionStudents(int SessionID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            //List<TblStudent> _Students = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID).Select(a => a.TblStudent).ToList();
            List<TblSubscription> _Students = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID && a.IsDeleted != true && a.Pending != true).ToList();
            ViewBag.SessionID = SessionID;

            return View(_Students);
        }

        public ActionResult SaveStudentsFromLecturerSide(FormCollection _Data)
        {
            try
            {
                string[] SubscriptionIDs = _Data["SubscriptionIDs"].Split(',');
                for (int i = 0; i < SubscriptionIDs.Length; i++)
                {
                    int SubscriptionID = int.Parse(SubscriptionIDs[i]);
                    TblSubscription Subscription = _Context.TblSubscriptions.Where(a => a.ID == SubscriptionID && a.IsDeleted != true && a.Pending != true).FirstOrDefault();
                    if (Subscription != null)
                    {
                        Subscription.FromLecturerSide = _Data["FromLecturerSide_" + SubscriptionID] == null ? false : true;
                    }
                    _Context.SaveChanges();
                }

                TempData["notice"] = "تم الحفظ بنجاح";
                return RedirectToAction("LecturesByPercentageList");
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("LecturesByPercentageList");
            }
        }

        public JsonResult GetHalls(DateTime FromTime, DateTime ToTime, int SessionID = 0)
        {
            try
            {
                List<TblHall> _HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();
                string Data = "<option value='0'>اختر القاعه</option>";
                foreach (var item in _HallsList)
                {
                    HallData data = new HallData();
                    //TblSession _SessionObj = _Context.TblSessions.Where(a => a.HallID == item.ID && a.IsDeleted != true && ((DbFunctions.TruncateTime(a.FromDate) <= FromTime.Date && DbFunctions.TruncateTime(a.ToDate) >= ToTime.Date) || (DbFunctions.TruncateTime(a.FromDate) == FromTime.Date || DbFunctions.TruncateTime(a.ToDate) >= ToTime.Date))).FirstOrDefault(); //check on only date
                    //TblSession _SessionObj = _Context.TblSessions.Where(a => a.HallID == item.ID && a.IsDeleted != true && ((a.FromDate <= FromTime && a.ToDate >= ToTime) || (a.FromDate == FromTime || a.ToDate >= ToTime))).FirstOrDefault(); //check on date and time
                    TblSession _SessionObj = _Context.TblSessions.Where(a => a.HallID == item.ID && a.IsDeleted != true && ((a.FromDate <= FromTime && a.ToDate >= ToTime && a.FromDate.Hour <= FromTime.Hour && a.ToDate.Hour <= ToTime.Hour) || ((a.FromDate == FromTime && a.FromDate.Hour == FromTime.Hour) || (a.ToDate >= ToTime && a.ToDate.Hour >= ToTime.Hour)))).FirstOrDefault(); //check on date and time
                    //if (SessionID > 0)
                    //{
                    //    _SessionObj = _Context.TblSessions.Where(a => a.ID == SessionID && a.IsDeleted != true && ((DbFunctions.TruncateTime(a.FromDate) <= FromTime.Date && DbFunctions.TruncateTime(a.ToDate) >= ToTime.Date) || (DbFunctions.TruncateTime(a.FromDate) == FromTime.Date || DbFunctions.TruncateTime(a.ToDate) >= ToTime.Date))).FirstOrDefault();
                    //}
                    data.HallID = item.ID;
                    data.HallCodeAr = item.HallCodeAr;
                    if (_SessionObj == null)
                    {
                        Data += "<option value='" + item.ID + "'>" + item.HallCodeAr + "</option>";
                    }
                    else if (_SessionObj != null && _SessionObj.ID == SessionID)
                    {
                        Data += "<option value='" + item.ID + "' selected>" + item.HallCodeAr + "</option>";
                    }
                    else
                    {
                        Data += "<option value='" + item.ID + "' disabled>" + item.HallCodeAr + "     (غير متاحه)</option>";
                    }
                }
                return Json(Data);
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
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

    }
}