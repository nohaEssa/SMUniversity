using SMUModels;
using SMUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class ReportsController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        public ActionResult CreateTransactionReport()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult CreateHallReport()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult CreateSubscriptionReport()
        {

            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                List<TblUniversity> _UniversityList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                //List<TblHall> _HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();

                ViewBag._UniversityList = _UniversityList;
               // ViewBag._HallsList = _HallsList;




                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }
        

        public ActionResult CreateLecturerIncomeReport()
        {

            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                List<TblLecturer> _LecturersList = _Context.TblLecturers.Where(a => a.IsDeleted != true).ToList();
                ViewBag._LecturersList = _LecturersList;

                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }


        public ActionResult CreateInstituteIncomeReport()
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
        public ActionResult GetTrancactionByDate(string CreatedDate )
        {
            DateTime selectedDate = DateTime.Now;

            if (CreatedDate != "" && CreatedDate != null)
            {
                selectedDate = DateTime.Parse(CreatedDate);
            }
            List<TblBalanceTransaction> data = new List<TblBalanceTransaction>();
            List<BalanceTransactionViewModel> result = new List<BalanceTransactionViewModel>();

            try
            {
                 data = _Context.TblBalanceTransactions.Where(t => t.TransactionTypeID ==1 && t.CreatedDate.Day == selectedDate.Day && t.CreatedDate.Month == selectedDate.Month && t.CreatedDate.Year == selectedDate.Year).ToList();
            }
            catch (Exception ex)
            {

            }
         
            if (data != null)
            {
                foreach (TblBalanceTransaction item in data)
                {
                    BalanceTransactionViewModel x = new BalanceTransactionViewModel();
                    x.ID = item.ID;
                    x.PaymentMethod = item.PaymentMethod;
                    x.Price = item.Price;
                    x.StudentID = item.StudentID;
                    x.StudentName = item.TblStudent.FirstName +" "+ item.TblStudent.SecondName;
                    x.TitleAr = item.TitleAr;
                    x.TransactionTypeID = item.TransactionTypeID;
                    result.Add(x);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetHallRentByDate(string StartDate , string EndDate)
        {
            DateTime SDate = DateTime.Now;
            DateTime EDate = DateTime.MinValue;

            if (StartDate != "" && StartDate != null)
            {
                SDate = DateTime.Parse(StartDate);

                if (EndDate != "" && EndDate != null)
                {
                    EDate = DateTime.Parse(EndDate);

                }
            }
            List<TblManualInvoice> data = new List<TblManualInvoice>();
            List<HallRentViewModel> result = new List<HallRentViewModel>();

            try
            {
                if (EDate.Date == DateTime.MinValue)
                { ////   tbnmanualinvoices
                    data = _Context.TblManualInvoices.Where(t => t.CreatedDate.Day == SDate.Day && t.CreatedDate.Month == SDate.Month && t.CreatedDate.Year == SDate.Year).ToList();
                }
                else
                {
                    data = _Context.TblManualInvoices.Where(t => DbFunctions.TruncateTime(t.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(t.CreatedDate) <= EDate.Date).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            if (data != null)
            {
                foreach (TblManualInvoice item in data)
                {
                    HallRentViewModel x = new HallRentViewModel();
                    x.ID = item.ID;
                    x.Price = item.Cost;
                    x.HallName = item.TblHall.HallCodeAr;
                    x.lectureName = item.TblLecturer.FirstNameAr +" "+ item.TblLecturer.SecondNameAr;
                    x.CreatedAt = item.CreatedDate.Date.ToShortDateString();
                    x.PhoneNumber= item.TblLecturer.PhoneNumber;
                    result.Add(x);
                }
            }
            return Json(result);
        }



        [HttpPost]
        public ActionResult GetSubscriptionBySessionWithinDate(string StartDate, string EndDate ,string SessionID)
        {
            DateTime SDate = DateTime.Now;
            DateTime EDate = DateTime.MinValue;
            int sessionId = 0;
            if (StartDate != "" && StartDate != null)
            {
                SDate = DateTime.Parse(StartDate);

                if (EndDate != "" && EndDate != null)
                {
                    EDate = DateTime.Parse(EndDate);

                }
            }
            if(SessionID !="" && SessionID != "0" && SessionID != null)
            {
                sessionId = int.Parse(SessionID);
            }
                List<TblSubscription> data = new List<TblSubscription>();
            // List<SubscriptionViewModel> result = new List<SubscriptionViewModel>();
            collectionModel result = new collectionModel();
            try 
            {
                if (sessionId != 0)
                {
                    if (EDate.Date == DateTime.MinValue)
                    { ////   tbnmanualinvoices
                        data = _Context.TblSubscriptions.Where(t => t.SessionID == sessionId && t.CreatedDate.Day == SDate.Day && t.CreatedDate.Month == SDate.Month && t.CreatedDate.Year == SDate.Year).ToList();
                    }
                    else
                    {
                          data = _Context.TblSubscriptions.Where(t => t.SessionID == sessionId && DbFunctions.TruncateTime(t.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(t.CreatedDate) <= EDate.Date).ToList();

                    }
                }
            }
            catch (Exception ex)
            {

            }

            if (data != null)
            {
                int listIndex = -1;
                int selectedCount;
                foreach (TblSubscription item in data)
                {
                    SubscriptionViewModel x = new SubscriptionViewModel();
                    x.ID = item.ID;
                    x.PaymentMethod = _Context.TransactionTypes.Find(x.ID = item.PriceType).NameAr ;
                    x.Price = item.PriceType;
                    x.StudentName= item.TblStudent.FirstName+ " " + item.TblStudent.SecondName;
                    x.CreatedAt = item.CreatedDate.Date.ToShortDateString();
                    result.Subscriptions.Add(x);

                    listIndex = result.pricesDetails.FindIndex(y => y.StartsWith(x.PaymentMethod));
                    if(listIndex < 0)
                    {
                        result.pricesDetails.Add(x.PaymentMethod);
                        result.pricesDetails.Add(1.ToString());
                    }
                    else
                    {
                        selectedCount = int.Parse(result.pricesDetails[listIndex + 1]);
                        selectedCount++;
                        result.pricesDetails[listIndex + 1] = selectedCount.ToString();
                        selectedCount = 0;
                    }
                }
            }
            return Json(result);
        }


        [HttpPost]
        public ActionResult GetLecturerIncometByDate(string StartDate, string EndDate,string LecturerID)
        {
            DateTime SDate = DateTime.MinValue; // DateTime.Now;
            DateTime EDate = DateTime.MinValue;
            int lecturerID = 0;
            if (StartDate != "" && StartDate != null)
            {
                SDate = DateTime.Parse(StartDate);

                if (EndDate != "" && EndDate != null)
                {
                    EDate = DateTime.Parse(EndDate);

                }
            }


            if (LecturerID != "" && LecturerID != "0" && LecturerID != null)
            {
                lecturerID = int.Parse(LecturerID);
            }

            LecturerIncomeViewModel lectureDetails = new LecturerIncomeViewModel();


            try
            {
                if (lecturerID != 0)
                {
                    if (EDate.Date == DateTime.MinValue && SDate.Date == DateTime.MinValue)
                    {
                        TblLecturer Lecturer = _Context.TblLecturers.Where(a => a.ID == lecturerID && a.IsDeleted != true).FirstOrDefault();
                        // get all subscriptions of type "Session" for lecturer "X"
                        int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                        //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                        int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                        //get all subscriptions of type "Course" for lecturer "x"
                        int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false)).Count(); //count session numbers

                        //get all subscriptions of type "Percentage" for lecturer "x"
                        decimal LecturesCountPerSession4Percentage = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == false && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false)).Sum(a => a.Price); //sum session prices
                        int LecturesCountPerSession4numbers = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == false && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers


                        int LecturesCountPerSession4Percentage_Student = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == true && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //sum session prices


                        decimal LecturesCountPerSessionVal1 = LecturesCountPerSession1 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal2 = LecturesCountPerSession2 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal3 = LecturesCountPerSession3 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal4 = (LecturesCountPerSession4Percentage * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100;
                        decimal LecturesCountPerSessionVal5 = LecturesCountPerSession4Percentage_Student * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;


                        decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4 + LecturesCountPerSessionVal5;


                        lectureDetails.ID = Lecturer.ID;
                        lectureDetails.LecturerName = Lecturer.FirstNameAr + " " + Lecturer.SecondNameAr;
                        lectureDetails.PhoneNumber = Lecturer.PhoneNumber;
                        lectureDetails.Address = Lecturer.Address;

                        lectureDetails.SessionsCount = LecturesCountPerSession1 + LecturesCountPerSession2;
                        lectureDetails.LecturerPricePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerSessionPrice = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2;

                        lectureDetails.CoursesCount = LecturesCountPerSession3;
                        lectureDetails.LecturerPricePerCourse = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerCoursePrice = LecturesCountPerSessionVal3;

                        lectureDetails.PercentageSessionCount = LecturesCountPerSession4numbers;
                        lectureDetails.PercentageSessionSumPrice = LecturesCountPerSession4Percentage;
                        lectureDetails.LecturerPercentagePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerPercentagePrice = LecturesCountPerSessionVal4;

                        lectureDetails.FromLecturerSideSessionCount = LecturesCountPerSession4Percentage_Student;
                        lectureDetails.FromLecturerSidePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                        lectureDetails.TotalFromLecturerSidePrice = LecturesCountPerSessionVal5;


                        lectureDetails.TotalLecturerIncome = Total;
                    }
                    else if(EDate.Date != DateTime.MinValue && SDate.Date != DateTime.MinValue)
                    {   // lecturer income throw Sdate and Edate 

                        TblLecturer Lecturer = _Context.TblLecturers.Where(a => a.ID == lecturerID && a.IsDeleted != true ).FirstOrDefault();
                        // get all subscriptions of type "Session" for lecturer "X"
                        int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                        //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                        int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                        //get all subscriptions of type "Course" for lecturer "x"
                        int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                        //get all subscriptions of type "Percentage" for lecturer "x"
                        decimal LecturesCountPerSession4Percentage = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == false && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Sum(a => a.Price); //sum session prices
                        int LecturesCountPerSession4numbers = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == false && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers


                        int LecturesCountPerSession4Percentage_Student = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == true && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //sum session prices


                        decimal LecturesCountPerSessionVal1 = LecturesCountPerSession1 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal2 = LecturesCountPerSession2 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal3 = LecturesCountPerSession3 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal4 = (LecturesCountPerSession4Percentage * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100;
                        decimal LecturesCountPerSessionVal5 = LecturesCountPerSession4Percentage_Student * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;


                        decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4 + LecturesCountPerSessionVal5;

                        lectureDetails.ID = Lecturer.ID;
                        lectureDetails.LecturerName = Lecturer.FirstNameAr + " " + Lecturer.SecondNameAr;
                        lectureDetails.PhoneNumber = Lecturer.PhoneNumber;
                        lectureDetails.Address = Lecturer.Address;

                        lectureDetails.SessionsCount = LecturesCountPerSession1 + LecturesCountPerSession2;
                        lectureDetails.LecturerPricePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerSessionPrice = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2;

                        lectureDetails.CoursesCount = LecturesCountPerSession3;
                        lectureDetails.LecturerPricePerCourse = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerCoursePrice = LecturesCountPerSessionVal3;

                        lectureDetails.PercentageSessionCount = LecturesCountPerSession4numbers;
                        lectureDetails.PercentageSessionSumPrice = LecturesCountPerSession4Percentage;
                        lectureDetails.LecturerPercentagePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerPercentagePrice = LecturesCountPerSessionVal4;


                        lectureDetails.FromLecturerSideSessionCount = LecturesCountPerSession4Percentage_Student;
                        lectureDetails.FromLecturerSidePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                        lectureDetails.TotalFromLecturerSidePrice = LecturesCountPerSessionVal5;

                        lectureDetails.TotalLecturerIncome = Total;

                    }
                    else if(EDate.Date == DateTime.MinValue && SDate.Date != DateTime.MinValue)
                    {
                        // lecturer income in single day

                        TblLecturer Lecturer = _Context.TblLecturers.Where(a => a.ID == lecturerID && a.IsDeleted != true).FirstOrDefault();
                        // get all subscriptions of type "Session" for lecturer "X"
                        int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                        //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                        int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                        //get all subscriptions of type "Course" for lecturer "x"
                        int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                        //get all subscriptions of type "Percentage" for lecturer "x"
                        decimal LecturesCountPerSession4Percentage = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == false && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Sum(a => a.Price); //sum session prices
                        int LecturesCountPerSession4numbers = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == false && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers


                        int LecturesCountPerSession4Percentage_Student = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == Lecturer.ID && a.FromLecturerSide == true && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //sum session prices


                        decimal LecturesCountPerSessionVal1 = LecturesCountPerSession1 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal2 = LecturesCountPerSession2 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal3 = LecturesCountPerSession3 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                        decimal LecturesCountPerSessionVal4 = (LecturesCountPerSession4Percentage * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100;
                        decimal LecturesCountPerSessionVal5 = LecturesCountPerSession4Percentage_Student * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;


                        decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4 + LecturesCountPerSessionVal5;


                        lectureDetails.ID = Lecturer.ID;
                        lectureDetails.LecturerName = Lecturer.FirstNameAr + " " + Lecturer.SecondNameAr;
                        lectureDetails.PhoneNumber = Lecturer.PhoneNumber;
                        lectureDetails.Address = Lecturer.Address;

                        lectureDetails.SessionsCount = LecturesCountPerSession1 + LecturesCountPerSession2;
                        lectureDetails.LecturerPricePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerSessionPrice = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2;

                        lectureDetails.CoursesCount = LecturesCountPerSession3;
                        lectureDetails.LecturerPricePerCourse = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerCoursePrice = LecturesCountPerSessionVal3;

                        lectureDetails.PercentageSessionCount = LecturesCountPerSession4numbers;
                        lectureDetails.PercentageSessionSumPrice = LecturesCountPerSession4Percentage;
                        lectureDetails.LecturerPercentagePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value;
                        lectureDetails.TotalLecturerPercentagePrice = LecturesCountPerSessionVal4;


                        lectureDetails.FromLecturerSideSessionCount = LecturesCountPerSession4Percentage_Student;
                        lectureDetails.FromLecturerSidePerSession = Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                        lectureDetails.TotalFromLecturerSidePrice = LecturesCountPerSessionVal5;

                        lectureDetails.TotalLecturerIncome = Total;


                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(lectureDetails);
        }

        [HttpPost]
        public ActionResult GetInstituteIncometByDate(string StartDate, string EndDate)
        {
            DateTime SDate = DateTime.MinValue; // DateTime.Now;
            DateTime EDate = DateTime.MinValue;
            if (StartDate != "" && StartDate != null)
            {
                SDate = DateTime.Parse(StartDate);

                if (EndDate != "" && EndDate != null)
                {
                    EDate = DateTime.Parse(EndDate);

                }
            }

            InstituteIncomeViewModel InstituteDetails = new InstituteIncomeViewModel();

            try
            {
                TblInstitute myInstitute = _Context.TblInstitutes.FirstOrDefault();
                InstituteDetails.Address = myInstitute.Email;
                InstituteDetails.PhoneNumber= myInstitute.PhoneNumber;

                if (EDate.Date == DateTime.MinValue && SDate.Date == DateTime.MinValue)
                {
                    #region get lectures income 

                    List<TblSubscription> Subscriptionlist = new List<TblSubscription>();
                    decimal NetLecturerSessionPrice = 0;
                    decimal NetLecturerCoursePrice = 0;
                    decimal NetLecturerPercentagePrice = 0;
                    decimal NetFromLecturerSidePrice = 0;

                    Subscriptionlist = _Context.TblSubscriptions.Where(a => a.IsDeleted == false).ToList();

                    foreach (TblSubscription sub in Subscriptionlist)
                    {
                        if (sub.TblSession.LecturerAccountMethod == 1 && sub.SubscripedAsSession == true)
                        {
                            try {
                                NetLecturerSessionPrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;

                            }
                            catch(Exception ex) {
                            }
                        }
                        if (sub.TblSession.LecturerAccountMethod == 3 && sub.SubscripedAsSession == true)
                        {
                            try
                            {
                                NetLecturerSessionPrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        if (sub.TblSession.LecturerAccountMethod == 3 && sub.SubscripedAsSession == false)
                        {
                            try
                            {
                                NetLecturerCoursePrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                            }
                            catch (Exception ex)
                            {
                            }

                        }
                        if (sub.TblSession.LecturerAccountMethod == 2 && sub.SubscripedAsSession == true && sub.FromLecturerSide == false)
                        {
                            try
                            {
                                NetLecturerPercentagePrice += ((sub.Price * sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        if (sub.TblSession.LecturerAccountMethod == 2 && sub.SubscripedAsSession == true && sub.FromLecturerSide == true)
                        {
                            try
                            {
                                NetFromLecturerSidePrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                    }

                    InstituteDetails.NetLecturerSessionPrice = NetLecturerSessionPrice;
                    InstituteDetails.NetLecturerCoursePrice = NetLecturerCoursePrice;
                    InstituteDetails.NetLecturerPercentagePrice = NetLecturerPercentagePrice;
                    InstituteDetails.NetFromLecturerSidePrice = NetFromLecturerSidePrice;

                    InstituteDetails.NetLecturerIncome = NetLecturerSessionPrice + NetLecturerCoursePrice + NetLecturerPercentagePrice + NetFromLecturerSidePrice;

                    //---------------------------------------------------------------------------------------------------------------------------------
                    #endregion


                    // get all subscriptions of type "Session" for lecturer "X"
                    int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                    //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                    int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                    //get all subscriptions of type "Course" for lecturer "x"
                    int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false)).Count(); //count session numbers

                    //get all subscriptions of type "Percentage" for lecturer "x"
                    int LecturesCountPerSession4 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == false && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers

                    //get all subscriptions of type "Percentage" from  lecturer side
                    int LecturesCountPerSession5 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == true && a.SubscripedAsSession == true && a.IsDeleted == false)).Count(); //count session numbers


                    decimal LecturesCountPerSessionVal1 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 1 && a.SubscripedAsSession == true && a.IsDeleted == false)).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal2 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false)).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal3 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false)).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal4 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == false && a.SubscripedAsSession == true && a.IsDeleted == false)).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal5 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == true && a.SubscripedAsSession == true && a.IsDeleted == false)).Sum(a => a.Price);


                    decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4 + LecturesCountPerSessionVal5;

                    InstituteDetails.SessionsCount = LecturesCountPerSession1 + LecturesCountPerSession2;
                    InstituteDetails.TotalInstituteSessionPrice = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2;

                    InstituteDetails.CoursesCount = LecturesCountPerSession3;
                    InstituteDetails.TotalInstituteCoursePrice = LecturesCountPerSessionVal3;

                    InstituteDetails.PercentageSessionCount = LecturesCountPerSession4;
                    InstituteDetails.TotalInstitutePercentagePrice = LecturesCountPerSessionVal4;

                    InstituteDetails.FromLecturerSideSessionCount = LecturesCountPerSession5;
                    InstituteDetails.TotalInstituteFromLecturerSidePrice = LecturesCountPerSessionVal5;


                    InstituteDetails.TotalInstituteIncome = Total;
                    InstituteDetails.NetInstituteIncome = InstituteDetails.TotalInstituteIncome - InstituteDetails.NetLecturerIncome;
                    InstituteDetails.NetInstituteSessionPrice = InstituteDetails.TotalInstituteSessionPrice - InstituteDetails.NetLecturerSessionPrice;
                    InstituteDetails.NetInstituteCoursePrice = InstituteDetails.TotalInstituteCoursePrice - InstituteDetails.NetLecturerCoursePrice;
                    InstituteDetails.NetInstitutePercentagePrice = InstituteDetails.TotalInstitutePercentagePrice - InstituteDetails.NetLecturerPercentagePrice;
                    InstituteDetails.NetInstituteFromLecturerSidePrice = InstituteDetails.TotalInstituteFromLecturerSidePrice - InstituteDetails.NetFromLecturerSidePrice;

                }
                else if (EDate.Date != DateTime.MinValue && SDate.Date != DateTime.MinValue)
                {   // Institute income throw Sdate and Edate 

                    #region get lectures income 

                    List<TblSubscription> Subscriptionlist = new List<TblSubscription>();
                    decimal NetLecturerSessionPrice = 0;
                    decimal NetLecturerCoursePrice = 0;
                    decimal NetLecturerPercentagePrice = 0;
                    decimal NetFromLecturerSidePrice = 0;

                    Subscriptionlist = _Context.TblSubscriptions.Where(a => a.IsDeleted == false && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).ToList();

                    foreach (TblSubscription sub in Subscriptionlist)
                    {
                        if (sub.TblSession.LecturerAccountMethod == 1 && sub.SubscripedAsSession == true)
                        {
                            NetLecturerSessionPrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        }
                        if (sub.TblSession.LecturerAccountMethod == 3 && sub.SubscripedAsSession == true)
                        {
                            NetLecturerSessionPrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        }
                        if (sub.TblSession.LecturerAccountMethod == 3 && sub.SubscripedAsSession == false)
                        {
                            NetLecturerCoursePrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;

                        }
                        if (sub.TblSession.LecturerAccountMethod == 2 && sub.SubscripedAsSession == false && sub.FromLecturerSide == false)
                        {
                            NetLecturerPercentagePrice += ((sub.Price * sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100);
                        }
                        if (sub.TblSession.LecturerAccountMethod == 2 && sub.SubscripedAsSession == false && sub.FromLecturerSide == true)
                        {
                            NetFromLecturerSidePrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                        }

                    }

                    InstituteDetails.NetLecturerSessionPrice = NetLecturerSessionPrice;
                    InstituteDetails.NetLecturerCoursePrice = NetLecturerCoursePrice;
                    InstituteDetails.NetLecturerPercentagePrice = NetLecturerPercentagePrice;
                    InstituteDetails.NetFromLecturerSidePrice = NetFromLecturerSidePrice;

                    InstituteDetails.NetLecturerIncome = NetLecturerSessionPrice + NetLecturerCoursePrice + NetLecturerPercentagePrice + NetFromLecturerSidePrice;

                    //---------------------------------------------------------------------------------------------------------------------------------
                    #endregion

                    // get all subscriptions of type "Session" for lecturer "X"
                    int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                    int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Course" for lecturer "x"
                    int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Percentage" for lecturer "x"
                    int LecturesCountPerSession4 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == false && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Percentage" from  lecturer side
                    int LecturesCountPerSession5 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == true && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Count(); //count session numbers


                    decimal LecturesCountPerSessionVal1 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 1 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal2 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal3 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal4 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == false && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal5 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == true && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) >= SDate.Date && DbFunctions.TruncateTime(a.CreatedDate) <= EDate.Date).Sum(a => a.Price);


                    decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4 + LecturesCountPerSessionVal5;

                    InstituteDetails.SessionsCount = LecturesCountPerSession1 + LecturesCountPerSession2;
                    InstituteDetails.TotalInstituteSessionPrice = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2;

                    InstituteDetails.CoursesCount = LecturesCountPerSession3;
                    InstituteDetails.TotalInstituteCoursePrice = LecturesCountPerSessionVal3;

                    InstituteDetails.PercentageSessionCount = LecturesCountPerSession4;
                    InstituteDetails.TotalInstitutePercentagePrice = LecturesCountPerSessionVal4;

                    InstituteDetails.FromLecturerSideSessionCount = LecturesCountPerSession5;
                    InstituteDetails.TotalInstituteFromLecturerSidePrice = LecturesCountPerSessionVal5;


                    InstituteDetails.TotalInstituteIncome = Total;
                    InstituteDetails.NetInstituteIncome = InstituteDetails.TotalInstituteIncome - InstituteDetails.NetLecturerIncome;
                    InstituteDetails.NetInstituteSessionPrice = InstituteDetails.TotalInstituteSessionPrice - InstituteDetails.NetLecturerSessionPrice;
                    InstituteDetails.NetInstituteCoursePrice = InstituteDetails.TotalInstituteCoursePrice - InstituteDetails.NetLecturerCoursePrice;
                    InstituteDetails.NetInstitutePercentagePrice = InstituteDetails.TotalInstitutePercentagePrice - InstituteDetails.NetLecturerPercentagePrice;
                    InstituteDetails.NetInstituteFromLecturerSidePrice = InstituteDetails.TotalInstituteFromLecturerSidePrice - InstituteDetails.NetFromLecturerSidePrice;

                }
                else if (EDate.Date == DateTime.MinValue && SDate.Date != DateTime.MinValue)
                {
                    // Institute income in single day

                    #region get lectures income 

                    List<TblSubscription> Subscriptionlist = new List<TblSubscription>();
                    decimal NetLecturerSessionPrice = 0;
                    decimal NetLecturerCoursePrice = 0;
                    decimal NetLecturerPercentagePrice = 0;
                    decimal NetFromLecturerSidePrice = 0;

                    Subscriptionlist = _Context.TblSubscriptions.Where(a => a.IsDeleted == false && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).ToList();

                    foreach (TblSubscription sub in Subscriptionlist)
                    {
                        if (sub.TblSession.LecturerAccountMethod == 1 && sub.SubscripedAsSession == true)
                        {
                            NetLecturerSessionPrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        }
                        if (sub.TblSession.LecturerAccountMethod == 3 && sub.SubscripedAsSession == true)
                        {
                            NetLecturerSessionPrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                        }
                        if (sub.TblSession.LecturerAccountMethod == 3 && sub.SubscripedAsSession == false)
                        {
                            NetLecturerCoursePrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;

                        }
                        if (sub.TblSession.LecturerAccountMethod == 2 && sub.SubscripedAsSession == false && sub.FromLecturerSide == false)
                        {
                            NetLecturerPercentagePrice += ((sub.Price * sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value) / 100);
                        }
                        if (sub.TblSession.LecturerAccountMethod == 2 && sub.SubscripedAsSession == false && sub.FromLecturerSide == true)
                        {
                            NetFromLecturerSidePrice += sub.TblSession.TblLecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                        }

                    }

                    InstituteDetails.NetLecturerSessionPrice = NetLecturerSessionPrice;
                    InstituteDetails.NetLecturerCoursePrice = NetLecturerCoursePrice;
                    InstituteDetails.NetLecturerPercentagePrice = NetLecturerPercentagePrice;
                    InstituteDetails.NetFromLecturerSidePrice = NetFromLecturerSidePrice;

                    InstituteDetails.NetLecturerIncome = NetLecturerSessionPrice + NetLecturerCoursePrice + NetLecturerPercentagePrice + NetFromLecturerSidePrice;

                    //---------------------------------------------------------------------------------------------------------------------------------
                    #endregion

                    // get all subscriptions of type "Session" for lecturer "X"
                    int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 1 /*&& a.TblSession.Type == true*/ && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Course" and  payed as Session for lecturer "X"
                    int LecturesCountPerSession2 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Course" for lecturer "x"
                    int LecturesCountPerSession3 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Percentage" for lecturer "x"
                    int LecturesCountPerSession4 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == false && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers

                    //get all subscriptions of type "Percentage" from  lecturer side
                    int LecturesCountPerSession5 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == true && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Count(); //count session numbers


                    decimal LecturesCountPerSessionVal1 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 1 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal2 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal3 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 3 && a.SubscripedAsSession == false && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal4 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == false && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Sum(a => a.Price);
                    decimal LecturesCountPerSessionVal5 = _Context.TblSubscriptions.Where(a => (a.TblSession.LecturerAccountMethod == 2 && a.FromLecturerSide == true && a.SubscripedAsSession == true && a.IsDeleted == false) && DbFunctions.TruncateTime(a.CreatedDate) == SDate.Date).Sum(a => a.Price);


                    decimal Total = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2 + LecturesCountPerSessionVal3 + LecturesCountPerSessionVal4 + LecturesCountPerSessionVal5;

                    InstituteDetails.SessionsCount = LecturesCountPerSession1 + LecturesCountPerSession2;
                    InstituteDetails.TotalInstituteSessionPrice = LecturesCountPerSessionVal1 + LecturesCountPerSessionVal2;

                    InstituteDetails.CoursesCount = LecturesCountPerSession3;
                    InstituteDetails.TotalInstituteCoursePrice = LecturesCountPerSessionVal3;

                    InstituteDetails.PercentageSessionCount = LecturesCountPerSession4;
                    InstituteDetails.TotalInstitutePercentagePrice = LecturesCountPerSessionVal4;

                    InstituteDetails.FromLecturerSideSessionCount = LecturesCountPerSession5;
                    InstituteDetails.TotalInstituteFromLecturerSidePrice = LecturesCountPerSessionVal5;


                    InstituteDetails.TotalInstituteIncome = Total;
                    InstituteDetails.NetInstituteIncome = InstituteDetails.TotalInstituteIncome - InstituteDetails.NetLecturerIncome;
                    InstituteDetails.NetInstituteSessionPrice = InstituteDetails.TotalInstituteSessionPrice - InstituteDetails.NetLecturerSessionPrice;
                    InstituteDetails.NetInstituteCoursePrice = InstituteDetails.TotalInstituteCoursePrice - InstituteDetails.NetLecturerCoursePrice;
                    InstituteDetails.NetInstitutePercentagePrice = InstituteDetails.TotalInstitutePercentagePrice - InstituteDetails.NetLecturerPercentagePrice;
                    InstituteDetails.NetInstituteFromLecturerSidePrice = InstituteDetails.TotalInstituteFromLecturerSidePrice - InstituteDetails.NetFromLecturerSidePrice;

                }
            }
            catch (Exception ex)
            {

            }

            return Json(InstituteDetails);
        }


    }
}
