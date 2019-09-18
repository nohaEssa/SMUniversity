using SMUModels;
using SMUModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class AttendanceController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        //[HttpGet]
        //public HttpResponseMessage GetStudentSubscription(int StudentID, int Flag)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.Ended != true && a.IsDeleted != true).ToList();
        //        List<SubscriptionData> Data = new List<SubscriptionData>();

        //        foreach (var item in _Subs)
        //        {
        //            List<TblAttendance> _Attend = _Context.TblAttendances.Where(a => a.SessionID == item.SessionID && a.StudentID == item.StudentID && a.IsDeleted != true).ToList();
        //            TblSessionPeriod _SessionPeriod = _Context.TblSessionPeriods.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

        //            SubscriptionData data = new SubscriptionData()
        //            {
        //                ID = item.ID,
        //                SessionType = item.TblSession.Type,
        //                SessionCost = _SessionPeriod.Cost,
        //                SubjectPicture = item.TblSession.TblSubject.Picture,
        //                SubjectNameAr = item.TblSession.TblSubject.NameAr,
        //                SubjectNameEn = item.TblSession.TblSubject.NameEn,
        //                HallCode = item.TblSession.TblHall.HallCode,
        //                LecturesCount = item.TblSession.LecturesCount,
        //                Attendance = _Attend.Where(a => a.Attend == true).Count(),
        //                Absence = _Attend.Where(a => a.Attend == false).Count(),
        //                LecturesLeft = item.TblSession.LecturesCount - _Attend.Where(a => a.Attend == false).Count(),
        //                StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
        //                Time = _SessionPeriod.FromTime.ToString("hh:mm tt"),
        //            };

        //            Data.Add(data);
        //        }
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
        public HttpResponseMessage GetListOfAttendanceAbsesnce(int SubscriptionID)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblAttendance> _StudentAttendance = _Context.TblAttendances.Where(a => a.SubscriptionID == SubscriptionID).ToList();
                List<AttendanceData> Data = new List<AttendanceData>();

                foreach (var item in _StudentAttendance)
                {
                    AttendanceData data = new AttendanceData()
                    {
                        AttendanceDate = item.CreatedDate.ToString("yyyy-MM-dd")
                    };
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
                throw;
            }
        }
    }
}
