using SMUModels;
using SMUModels.ObjectData;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace WebAPI.Controllers
{
    public class LecturerController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage UpdateLecturer(LecturerObj _Params)
        {
            var _resultHandler = new ResultHandler();

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

        [HttpGet]
        public HttpResponseMessage getLecturers(int SubjectID)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                List<TblLecturer> _LecturersList = _Context.TblLecturerSubjects.Where(a => a.SubjectID == SubjectID && a.IsDeleted != true).Select(a => a.TblLecturer).ToList();
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
                List<TblSession> _Sessions = _Context.TblSessions.Where(a => a.LecturerID == LecturerID && DbFunctions.TruncateTime(a.FromDate) <= CurrentDate && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true).ToList();
                TodaysSessionData Data = new TodaysSessionData();
                Data.TodaysSessions = new List<TodaysSessions>();
                Data.CurrentBalance = 50;
                Data.TotalBalance = 70;

                foreach (var item in _Sessions)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    TodaysSessions data = new TodaysSessions()
                    {
                        ID = item.ID,
                        Code = item.SessionCode,
                        SubjectNameAr = item.SubjectID != null ? item.TblSubject.NameAr : "",
                        SubjectNameEn = item.SubjectID != null ? item.TblSubject.NameEn : "",
                        SessionType = item.Type,
                        HallCodeAr = item.TblHall.HallCodeAr,
                        HallCodeEn = item.TblHall.HallCodeEn,
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        StudentCount = 0,
                    };

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

        [HttpGet]
        public HttpResponseMessage Get(int CollegeID, int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                List<TblSession> _Sessions = _Context.TblSessions.Where(a => a.TblSubject.TblMajor.CollegeID == CollegeID && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true).ToList();
                //List<TblSession> _Sessions = _Context.TblSessions.Where(a => a.TblSubject.TblMajor.CollegeID == CollegeID && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true && DbFunctions.TruncateTime(a.FromDate) <= CurrentDate ).ToList();
                List<TimelineSessionData> Data = new List<TimelineSessionData>();

                foreach (var item in _Sessions)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    TimelineSessionData data = new TimelineSessionData()
                    {
                        ID = item.ID,
                        Code = item.SessionCode,
                        CollegeNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.SubjectID != null ? item.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblHall.HallCodeAr,
                        HallCodeEn = item.TblHall.HallCodeEn,
                        SessionType = item.Type,
                        SessionCost = _SessionDetail.Cost,
                        SessionDescription = item.Description,
                        //SubjectPicture = item.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSubject.Picture.StartsWith("http://") ? item.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSubject.Picture),
                        SubjectNameAr = item.SubjectID != null ? item.TblSubject.NameAr : "",
                        SubjectNameEn = item.SubjectID != null ? item.TblSubject.NameEn : "",
                        LecturerID = item.LecturerID,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        LecturesCount = item.LecturesCount,
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        Favourite = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.SessionID == item.ID).FirstOrDefault() == null ? false : true,
                        Subscribed = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.SessionID == item.ID).FirstOrDefault() == null ? false : true
                    };

                    if (item.Picture != null)
                    {
                        data.SubjectPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                        data.SessionPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com/" + item.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com/" + item.Picture;
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
    }
}
