using SMUModels;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using SMUModels.Classes;

namespace WebAPI.Controllers
{
    public class LecturerController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage UpdateLecturer(LecturerObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblLecturer teacher = _Context.TblLecturers.Where(a => a.ID == _Params.LecturerID).SingleOrDefault();
                if (teacher == null)
                {
                    teacher.FirstNameAr = _Params.FirstNameAr;
                    teacher.FirstNameEn = _Params.FirstNameEn;
                    teacher.SecondNameAr = _Params.SecondNameAr;
                    teacher.SecondNameEn = _Params.SecondNameEn;
                    teacher.ThirdNameAr = _Params.ThirdNameAr;
                    teacher.ThirdNameEn = _Params.ThirdNameEn;
                    teacher.PhoneNumber = _Params.PhoneNumber;
                    teacher.Email = _Params.Email;
                    teacher.Gender = _Params.Gender; // 0 : Female , 1 : Male
                    teacher.Address = _Params.Address;
                    teacher.BranchID = _Params.BranchID;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Record not found";
                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                }
            }
            catch (Exception ex)
            {

                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }


        }

        [HttpPost]
        public HttpResponseMessage UpdatePassword(LecturerPasswordObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                if (_Params.Password.Equals(_Params.ConfirmPassword))
                {
                    TblLecturer LecturerObj = _Context.TblLecturers.Where(a => a.ID == _Params.LecturerID).SingleOrDefault();
                    if (LecturerObj == null)
                    {
                        LecturerObj.TblUserCredential.Password = _Params.Password;
                        _Context.SaveChanges();

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.MessageAr = "OK";
                        _resultHandler.MessageEn = "OK";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "Record not found";
                        _resultHandler.MessageEn = "هذا الحساب غير موجود";

                        return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "الرقم السري وتأكيد الرقم السري غير متطابقين";
                    _resultHandler.MessageEn = "Password and Confirm Password is not identical";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                }
            }
            catch (Exception ex)
            {

                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }


        }

        [HttpGet]
        public HttpResponseMessage getLecturers(int SubjectID)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                List<TblLecturer> _LecturersList = _Context.TblLecturerSubjects.Where(a => a.SubjectID == SubjectID && a.IsDeleted != true).Select(a => a.TblLecturer).OrderByDescending(a => a.ID).ToList();
                List<LecturerDataDDL> Data = new List<LecturerDataDDL>();
                foreach (var item in _LecturersList)
                {
                    LecturerDataDDL _data = new LecturerDataDDL()
                    {
                        LecturerID = item.ID,
                        NameAr = item.FirstNameAr + " " + item.SecondNameAr + " " + item.ThirdNameAr,
                        NameEn = item.FirstNameEn + " " + item.SecondNameEn + " " + item.ThirdNameEn,
                    };

                    Data.Add(_data);
                }

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.Count = Data.Count;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {

                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTodaysSessions(int LecturerID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                List<TblLecturerPaymentMethod> PaymentMethodList = _Context.TblLecturerPaymentMethods.Where(a => a.LecturerID == LecturerID).ToList();
                List<TblSessionTime> _Sessions = _Context.TblSessionTimes.Where(a => DbFunctions.TruncateTime(a.FromTime) == CurrentDate && a.TblSession.LecturerID == LecturerID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                TodaysSessionData Data = new TodaysSessionData();
                Data.TodaysSessions = new List<TodaysSessions>();

                List<TblSubscription> LecturerSubs = _Context.TblSubscriptions.Where(a => DbFunctions.TruncateTime(a.CreatedDate) == CurrentDate && a.TblSession.LecturerID == LecturerID && a.Pending != true).ToList();

                decimal TotalVal1 = _Context.TblSubscriptions.Where
                    (a => a.TblSession.LecturerID == LecturerID
                    && (a.TblSession.LecturerAccountMethod == 1 || (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true))
                    && a.IsDeleted != true && a.Pending != true).Count() * PaymentMethodList.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;

                decimal TotalVal2 = 0;
                var c = _Context.TblSubscriptions.Where
                    (a => a.TblSession.LecturerID == LecturerID
                    && a.TblSession.LecturerAccountMethod == 2 && a.IsDeleted != true
                    && a.Pending != true).ToList();
                if (c.Count>0)
                {
                    TotalVal2 = (c.Sum(a => a.Price) * PaymentMethodList.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100;
                }
                
                decimal TotalVal3 = _Context.TblSubscriptions.Where
                    (a => a.TblSession.LecturerID == LecturerID
                    && a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted != true
                    && a.Pending != true).Count() * PaymentMethodList.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;

                decimal TotalVal4 = _Context.TblSubscriptions.Where
                    (a => a.TblSession.LecturerID == LecturerID
                    && a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.FromLecturerSide == true && a.IsDeleted != true
                    && a.Pending != true).Count() * PaymentMethodList.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                ////////////////////////////////////////////////////////////////////Total/////////////////////////////////////////
                decimal CurrentVal1 = _Context.TblSubscriptions.Where
                    (a => DbFunctions.TruncateTime(a.CreatedDate) == CurrentDate && a.TblSession.LecturerID == LecturerID
                    && (a.TblSession.LecturerAccountMethod == 1 || (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true))
                    && a.IsDeleted != true && a.Pending != true).Count() * PaymentMethodList.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;

                decimal CurrentVal2 = 0;
                var c2 = _Context.TblSubscriptions.Where
                    (a => DbFunctions.TruncateTime(a.CreatedDate) == CurrentDate && a.TblSession.LecturerID == LecturerID
                    && a.TblSession.LecturerAccountMethod == 2 && a.IsDeleted != true
                    && a.Pending != true).ToList();
                if (c2.Count > 0) 
                {
                    CurrentVal2 = (c2.Sum(a => a.Price) * PaymentMethodList.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100;
                }
                decimal CurrentVal3 = _Context.TblSubscriptions.Where
                    (a => DbFunctions.TruncateTime(a.CreatedDate) == CurrentDate && a.TblSession.LecturerID == LecturerID
                    && a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted != true
                    && a.Pending != true).Count() * PaymentMethodList.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;

                decimal CurrentVal4 = _Context.TblSubscriptions.Where
                    (a => DbFunctions.TruncateTime(a.CreatedDate) == CurrentDate && a.TblSession.LecturerID == LecturerID
                    && a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.FromLecturerSide == true && a.IsDeleted != true
                    && a.Pending != true).Count() * PaymentMethodList.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;



                Data.CurrentBalance = CurrentVal1 + CurrentVal2 + CurrentVal3 + CurrentVal4;
                Data.TotalBalance = TotalVal1 + TotalVal2 + TotalVal3 + TotalVal4;

                foreach (var item in _Sessions)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    int _SubsCout = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionTimes.SessionID && a.Pending != true && a.IsDeleted != true).Count();

                    TodaysSessions data = new TodaysSessions()
                    {
                        ID = item.ID,
                        Code = item.TblSession.SessionCode,
                        SubjectNameAr = item.TblSession.SubjectID != null ? item.TblSession.TblSubject.NameAr : "",
                        SubjectNameEn = item.TblSession.SubjectID != null ? item.TblSession.TblSubject.NameEn : "",
                        SessionNameAr = item.TblSession.Name,
                        SessionNameEn = item.TblSession.NameEn,
                        SessionType = item.TblSession.Type,
                        GeneralSession = item.TblSession.GeneralSession,
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        StudentCount = _SubsCout,
                        DescriptionAr = item.TblSession.Description,
                        DescriptionEn = item.TblSession.DescriptionEn,
                    };

                    if (item.TblSession.TblSubject.Picture != null)
                    {
                        data.SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture;
                    }

                    if (item.TblSession.Type)
                    {
                        int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == item.ID && a.Pending != true).Count();
                        //TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                        //TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
                        switch (subCount)
                        {
                            case 0:
                                data.SessionCost = (decimal)_SessionDetail.Price1;
                                break;
                            case 1:
                                data.SessionCost = (decimal)_SessionDetail.Price2;
                                break;
                            //case 2:
                            //    data.SessionCost = (decimal)_SessionDetail.Price3;
                            //    break;
                            default:
                                data.SessionCost = (decimal)_SessionDetail.Price3;
                                break;
                        }
                    }
                    else
                    {
                        if (item.TblSession.FromDate.Date >= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetail.Cost;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetail.SessionPrice;
                        }
                    }
                    Data.TodaysSessions.Add(data);
                }
                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.Count = Data.TodaysSessions.Count;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        //[HttpGet]
        //public HttpResponseMessage GetSessionStudent(int SessionID)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        DateTime CurrentDate = DateTime.Now.Date;
        //        List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
        //        //List<TblStudent> _Students = _Context.TblAttendances.Where(a => a.TblSubscription.SessionID == SessionID && a.Attend == 1 && a.IsDeleted != true).Select(a => a.TblStudent).ToList();
        //        //SessionStudentData Data = new SessionStudentData();
        //        //Data.AttendedStudents = new List<string>();
        //        //Data.StudentsData = new List<AttendanceStudentData>();
        //        List<AttendanceStudentData> Data = new List<AttendanceStudentData>();

        //        foreach (var item in _Subs)
        //        {
        //            TblAttendance _AttendancesObj = _Context.TblAttendances.Where(a => a.TblSubscription.SessionID == SessionID && a.StudentID == item.StudentID && a.Attend != 1 && a.Attend != 2 && a.IsDeleted != true).FirstOrDefault();
        //            AttendanceStudentData data = new AttendanceStudentData()
        //            {
        //                StudentID = item.StudentID,
        //                SubscriptionID = item.ID,
        //                Name = item.TblStudent.FirstName + " " + item.TblStudent.SecondName + " " + item.TblStudent.ThirdName,
        //                UniversityNameAr = item.TblStudent.TblUniversity.NameAr,
        //                UniversityNameEn = item.TblStudent.TblUniversity.NameEn,
        //                Attend = _AttendancesObj == null ? 3 : _AttendancesObj.Attend,
        //                SubscripedAsSession = item.SubscripedAsSession, //true >> session  false >> course
        //            };

        //            Data.Add(data);
        //        };

        //        _resultHandler.IsSuccessful = true;
        //        _resultHandler.Result = Data;
        //        _resultHandler.MessageAr = "OK";
        //        _resultHandler.MessageEn = "OK";

        //        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //    }
        //    catch (Exception ex)
        //    {
        //        _resultHandler.IsSuccessful = false;
        //        _resultHandler.MessageAr = ex.Message;
        //        _resultHandler.MessageEn = ex.Message;

        //        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage GetSessionStudent(int SessionID)//SessionTimesID
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.ID == SessionID).FirstOrDefault();
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionTimes.SessionID && a.Pending != true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                //List<TblStudent> _Students = _Context.TblAttendances.Where(a => a.TblSubscription.SessionID == SessionID && a.Attend == 1 && a.IsDeleted != true).Select(a => a.TblStudent).ToList();
                //SessionStudentData Data = new SessionStudentData();
                //Data.AttendedStudents = new List<string>();
                //Data.StudentsData = new List<AttendanceStudentData>();
                List<AttendanceStudentData> Data = new List<AttendanceStudentData>();

                foreach (var item in _Subs)
                {
                    //TblAttendance _AttendancesObj = _Context.TblAttendances.Where(a => a.TblSubscription.SessionID == _SessionTimes.SessionID && a.StudentID == item.StudentID && a.Attend != 1 && a.Attend != 2 && a.IsDeleted != true).FirstOrDefault();
                    TblAttendance _AttendancesObj = _Context.TblAttendances.Where(a => a.SubscriptionID == item.ID && a.StudentID == item.StudentID && a.IsDeleted != true).FirstOrDefault();
                    AttendanceStudentData data = new AttendanceStudentData()
                    {
                        StudentID = item.StudentID,
                        SubscriptionID = item.ID,
                        Name = item.TblStudent.FirstName + " " + item.TblStudent.SecondName + " " + item.TblStudent.ThirdName,
                        NameEn = item.TblStudent.FirstNameEn + " " + item.TblStudent.SecondNameEn + " " + item.TblStudent.ThirdNameEn,
                        ProfilePic = item.TblStudent.ProfilePic.StartsWith("http://") ? item.TblStudent.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblStudent.ProfilePic),
                        UniversityNameAr = item.TblStudent.TblUniversity.NameAr,
                        UniversityNameEn = item.TblStudent.TblUniversity.NameEn,
                        Attend = _AttendancesObj == null ? 3 : _AttendancesObj.Attend,
                        SubscripedAsSession = item.SubscripedAsSession, //true >> session  false >> course
                    };

                    Data.Add(data);
                };

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }
        [HttpPost]
        public HttpResponseMessage EndSession(int SessionTimesID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                string TitleAr = "", TitleEn = "", DescriptionAr = "", DescriptionEn = "";
                //TblSessionTime _Session = _Context.TblSessionTimes.Where(a => a.ID == SessionTimesID && DbFunctions.TruncateTime(a.FromTime) == CurrentDate).FirstOrDefault();
                TblSessionTime _Session = _Context.TblSessionTimes.Where(a => a.ID == SessionTimesID).FirstOrDefault();

                _Session.Ended = true;
                _Context.SaveChanges();

                TitleAr = "سمارت مايند الجامعه";
                TitleEn = "SmartMind University";
                DescriptionAr = "تم انهاء محاضرة " + _Session.TblSession.Name;
                DescriptionEn = "Lecture has been ended now";

                Push(0, _Session.TblSession.LecturerID, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                foreach (var item in _Context.TblSubscriptions.Where(a => a.SessionID == _Session.SessionID && a.IsDeleted != true && a.Pending != true).OrderByDescending(a => a.ID).ToList())
                {
                    TitleAr = "سمارت مايند الجامعه";
                    TitleEn = "SmartMind University";
                    DescriptionAr = "تم انهاء محاضرة " + _Session.TblSession.Name;
                    DescriptionEn = "Lecture has been ended now";

                    Push(item.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);
                }
                _resultHandler.IsSuccessful = true;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveAttendance(SaveAttendanceObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                string DescriptionAr = "", DescriptionEn = "";
                foreach (var item in _Params.Students)
                {
                    TblAttendance _AttendanceObj = _Context.TblAttendances.Where(a => a.StudentID == item.StudentID && a.SubscriptionID == item.SubscriptionID && a.SessionTimesID == _Params.SessionTimesID).FirstOrDefault();

                    //TblAttendance _AttendanceRecord=_Context.TblAttendances.Where(a=>a.StudentID==item.StudentID&&a.SessionTimesID==item&&a.SubscriptionID==item.SubscriptionID)
                    if (_AttendanceObj == null)
                    {
                        TblAttendance _Obj = new TblAttendance()
                        {
                            StudentID = item.StudentID,
                            SessionTimesID = _Params.SessionTimesID,
                            SubscriptionID = item.SubscriptionID,
                            Attend = byte.Parse(item.Attend.ToString()),
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        _Context.TblAttendances.Add(_Obj);                        
                    }
                    else
                    {
                        _AttendanceObj.Attend = byte.Parse(item.Attend.ToString());
                        _AttendanceObj.UpdatedDate = DateTime.Now;
                    }

                    _Context.SaveChanges();

                    string TitleAr = "سمارت مايند الجامعه";
                    string TitleEn = "SmartMind University";
                    if (byte.Parse(item.Attend.ToString()) == 1)
                    {
                        DescriptionAr = "تم تحضيرك اليوم في محاضره " + _AttendanceObj.TblSessionTime.TblSession.Name + " للمحاضر " + (_AttendanceObj.TblSessionTime.TblSession.TblLecturer.FirstNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.SecondNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.ThirdNameAr);
                        DescriptionEn = "You've been remarked as attended in Lecture : " + _AttendanceObj.TblSessionTime.TblSession.Name + " for Lecturer : " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.FirstNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.SecondNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.ThirdNameAr;
                    }
                    else if (byte.Parse(item.Attend.ToString()) == 2)
                    {
                        DescriptionAr = "تم تحضيرك اليوم في محاضره " + _AttendanceObj.TblSessionTime.TblSession.Name + " للمحاضر " + (_AttendanceObj.TblSessionTime.TblSession.TblLecturer.FirstNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.SecondNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.ThirdNameAr);
                        DescriptionEn = "You've been remarked as absent in Lecture : " + _AttendanceObj.TblSessionTime.TblSession.Name + " for Lecturer : " + (_AttendanceObj.TblSessionTime.TblSession.TblLecturer.FirstNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.SecondNameAr + " " + _AttendanceObj.TblSessionTime.TblSession.TblLecturer.ThirdNameAr);
                    }

                    Push(item.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);
                }
                
                _resultHandler.IsSuccessful = true;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPreviousSubscriptions(int LecturerID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                //List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && a.Ended == true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                //List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => DbFunctions.TruncateTime(a.TblSession.ToDate) < CurrentDate && a.TblSession.LecturerID == LecturerID && a.Pending != true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<TblSession> _Subs = _Context.TblSessions.Where(a => DbFunctions.TruncateTime(a.ToDate) < CurrentDate && a.LecturerID == LecturerID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        Code = item.SessionCode,
                        SessionType = item.Type,
                        GeneralSession = item.GeneralSession,
                        //SessionCost = _SessionDetails.Cost,
                        SubjectNameAr = item.TblSubject.NameAr,
                        SubjectNameEn = item.TblSubject.NameEn,
                        CollegeNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblHall.HallCodeAr,
                        HallCodeEn = item.TblHall.HallCodeEn,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr + " ",
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn + " ",
                        LecturerPic = "http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic,
                        LecturesCount = item.LecturesCount,
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        LecturerRate = 4.5,
                        DescriptionAr = item.Description,
                        DescriptionEn = item.DescriptionEn,
                    };

                    if (item.Picture != null)
                    {
                        data.SubjectPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                        data.SessionPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                    }

                    if (item.Type)
                    {
                        data.SessionCost = (decimal)_SessionDetails.Price1;
                    }
                    else
                    {
                        if (item.FromDate.Date <= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetails.SessionPrice;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetails.Cost;
                        }
                    }
                    Data.Add(data);
                }
                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUpComingSubscriptions(int LecturerID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                //List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => DbFunctions.TruncateTime(a.TblSession.ToDate) >= CurrentDate && a.TblSession.LecturerID == LecturerID && a.Pending != true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<TblSession> _Subs = _Context.TblSessions.Where(a => DbFunctions.TruncateTime(a.ToDate) >= CurrentDate && a.LecturerID == LecturerID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        Code = item.SessionCode,
                        SessionType = item.Type,
                        GeneralSession = item.GeneralSession,
                        //SessionCost = _SessionDetails.Cost,
                        SubjectNameAr = item.TblSubject.NameAr,
                        SubjectNameEn = item.TblSubject.NameEn,
                        CollegeNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblHall.HallCodeAr,
                        HallCodeEn = item.TblHall.HallCodeEn,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr + " ",
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn + " ",
                        LecturerPic = "http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic,
                        LecturesCount = item.LecturesCount,
                        LecturesLeft = item.LecturesCount,
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        LecturerRate = 4.5,
                        DescriptionAr = item.Description,
                        DescriptionEn = item.DescriptionEn,
                    };

                    if (item.Picture != null)
                    {
                        data.SubjectPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                        data.SessionPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                    }

                    if (item.Type)
                    {
                        data.SessionCost = (decimal)_SessionDetails.Price1;
                    }
                    else
                    {
                        if (item.FromDate.Date <= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetails.SessionPrice;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetails.Cost;
                        }
                    }
                    Data.Add(data);
                }
                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.Count = Data.Count;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCurrentSubscriptions(int LecturerID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblSession> _Subs = _Context.TblSessions.Where(a => a.LecturerID == LecturerID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    //List<TblAttendance> _Attend = _Context.TblAttendances.Where(a => a.SessionID == item.SessionID && a.StudentID == item.StudentID && a.IsDeleted != true).ToList();
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        SessionID = int.Parse(item.ID.ToString()),
                        Code = item.SessionCode,
                        SessionType = item.Type,
                        GeneralSession = item.GeneralSession,
                        //SessionCost = _SessionDetails.Cost,
                        SubjectNameAr = item.TblSubject.NameAr,
                        SubjectNameEn = item.TblSubject.NameEn,
                        CollegeNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblHall.HallCodeAr,
                        HallCodeEn = item.TblHall.HallCodeEn,
                        LecturesCount = item.LecturesCount,
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        LecturerRate = 4.5,
                    };

                    if (item.Picture != null)
                    {
                        data.SubjectPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                        data.SessionPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                    }
                    if (item.Type)
                    {
                        data.SessionCost = (decimal)_SessionDetails.Price1;
                    }
                    else
                    {
                        if (item.FromDate.Date <= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetails.SessionPrice;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetails.Cost;
                        }
                    }
                    Data.Add(data);
                }
                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTotal(int LecturerID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {


                //int LecturerID = 1;

                TblLecturer Lecturer = _Context.TblLecturers.Where(a => a.ID == LecturerID && a.IsDeleted != true).FirstOrDefault();
                // get all subscriptions of type "Session" for lecturer "X"
                int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                //get all subscriptions of type "Percentage" for lecturer "x"
                int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false)).Count(); //count session numbers

                //get all subscriptions of type "Percentage" for lecturer "x"
                decimal LecturesCountPerSession4 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false)).Sum(a => a.Price); //count session numbers



                decimal LecturesCountPerSessionVal1 = LecturesCountPerSession1 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                decimal LecturesCountPerSessionVal2 = LecturesCountPerSession2 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                decimal LecturesCountPerSessionVal3 = LecturesCountPerSession3 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                decimal LecturesCountPerSessionVal4 = (LecturesCountPerSession4 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100;


                decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4;






                //_resultHandler.IsSuccessful = true;
                //_resultHandler.Result = Data;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
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
