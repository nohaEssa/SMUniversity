using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class AttendanceController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Attendance
        public ActionResult Index()
        {
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


        //public ActionResult SaveStudentsFromLecturerSide(FormCollection _Data)
        //{
        //    try
        //    {
        //        string[] SubscriptionIDs = _Data["SubscriptionIDs"].Split(',');
        //        for (int i = 0; i < SubscriptionIDs.Length; i++)
        //        {
        //            int SubscriptionID = int.Parse(SubscriptionIDs[i]);
        //            TblSubscription Subscription = _Context.TblSubscriptions.Where(a => a.ID == SubscriptionID && a.IsDeleted != true && a.Pending != true).FirstOrDefault();
        //            if (Subscription != null)
        //            {
        //                Subscription.FromLecturerSide = _Data["FromLecturerSide_" + SubscriptionID] == null ? false : true;
        //            }
        //            _Context.SaveChanges();
        //        }

        //        TempData["notice"] = "تم الحفظ بنجاح";
        //        return RedirectToAction("LecturesByPercentageList");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["notice"] = "ERROR while processing!";
        //        return RedirectToAction("LecturesByPercentageList");
        //    }
        //}

        public ActionResult GetSessionTimes(int SessionID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<TblSessionTime> _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == SessionID && a.IsDeleted != true).ToList();

            return View(_SessionTimes);
        }

        public ActionResult GetSessionStudents(int SessionTimesID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            //List<TblStudent> _Students = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID).Select(a => a.TblStudent).ToList();
            TblSessionTime _SessionTimesObj = _Context.TblSessionTimes.Where(a => a.ID == SessionTimesID).FirstOrDefault();
            List<TblSubscription> _Subscriptions = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionTimesObj.SessionID && a.IsDeleted != true && a.Pending != true).ToList();
            ViewBag.SessionID = _SessionTimesObj.SessionID;
            ViewBag.SessionTimesID = SessionTimesID;

            return View(_Subscriptions);
        }

        public ActionResult SaveAttendance(FormCollection _Data)
        {
            try
            {
                string[] SubscriptionIDs = _Data["SubscriptionIDs"].Split(',');
                for (int i = 0; i < SubscriptionIDs.Length; i++)
                {
                    int SubscriptionID = int.Parse(SubscriptionIDs[i]);
                    TblSubscription Subscription = _Context.TblSubscriptions.Where(a => a.ID == SubscriptionID && a.IsDeleted != true).FirstOrDefault();

                    byte Attend = 3;
                    if(_Data["Attend_" + SubscriptionID] != null)
                    {
                        Attend = byte.Parse(_Data["Attend_" + SubscriptionID]);
                    }
                    int SessionTimesID = int.Parse(_Data["SessionTimesID"]);

                    TblAttendance _AttendanceObj = _Context.TblAttendances.Where(a => a.StudentID == Subscription.StudentID && a.SubscriptionID == SubscriptionID && a.SessionTimesID == SessionTimesID).FirstOrDefault();

                    if (_AttendanceObj == null)
                    {
                        TblAttendance _Obj = new TblAttendance()
                        {
                            StudentID = Subscription.StudentID,
                            SessionTimesID = SessionTimesID,
                            SubscriptionID = SubscriptionID,
                            Attend = Attend,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        _Context.TblAttendances.Add(_Obj);
                    }
                    else
                    {
                        _AttendanceObj.Attend = Attend;
                        _AttendanceObj.UpdatedDate = DateTime.Now;
                    }
                    _Context.SaveChanges();
                }

                TempData["notice"] = "تم الحفظ بنجاح";
                return RedirectToAction("CoursesList");
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("GetSessionStudents", new { SessionID = _Data["SessionID"] });
            }
        }

    }
}