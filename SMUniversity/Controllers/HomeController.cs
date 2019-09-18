using SMUModels;
using SMUModels.ObjectData;
using SMUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SMUniversity.Controllers
{
    public class HomeController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                int countstudents = _Context.TblStudents.Count(a => a.IsDeleted == false);
                int countBranches = _Context.TblBranches.Count(a => a.IsDeleted == false);
                int countColleges = _Context.TblColleges.Count(a => a.IsDeleted == false);
                int countLecturers = _Context.TblLecturers.Count(a => a.IsDeleted == false);
                int countHalls = _Context.TblHalls.Count(a => a.IsDeleted == false);
                int countnewstudents = _Context.TblStudents.Count(t => t.IsDeleted == false && t.CreatedDate.Day == DateTime.Now.Day && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Year == DateTime.Now.Year);
                decimal totalTransactionToday = 0;
                decimal totalHallRentToday = 0;
                decimal totalSubscriptionsToday = 0;
                List<TblBalanceTransaction> x = _Context.TblBalanceTransactions.Where(t => t.TransactionTypeID == 1 && t.CreatedDate.Day == DateTime.Now.Day && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Year == DateTime.Now.Year).ToList();
                if (x.Count > 0)
                    totalTransactionToday = (decimal)x.Sum(a => a.Price);

                List<TblManualInvoice> y = _Context.TblManualInvoices.Where(t => t.CreatedDate.Day == DateTime.Now.Day && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Year == DateTime.Now.Year).ToList();
                if (y.Count > 0)
                    totalHallRentToday = (decimal)y.Sum(a => a.Cost);
                List<TblSubscription> z = _Context.TblSubscriptions.Where(t => t.CreatedDate.Day == DateTime.Now.Day && t.CreatedDate.Month == DateTime.Now.Month && t.CreatedDate.Year == DateTime.Now.Year).ToList();
                if (z.Count > 0)
                    totalSubscriptionsToday = (decimal)z.Sum(a => a.Price);

                ViewBag.countstudents = countstudents;
                ViewBag.countBranches = countBranches;
                ViewBag.countColleges = countColleges;
                ViewBag.countLecturers = countLecturers;
                ViewBag.countHalls = countHalls;
                ViewBag.countnewstudents = countnewstudents;
                ViewBag.totalTransactionToday = totalTransactionToday;
                ViewBag.totalHallRentToday = totalHallRentToday;
                ViewBag.totalSubscriptionsToday = totalSubscriptionsToday;
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginData _Data)
        {
            try
            {
                TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Data.UserName && a.Password == _Data.Password && a.UserType == 4).SingleOrDefault();
                if (_UserCred != null)
                {
                    TblUser _User = _Context.TblUsers.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();

                    //Session["User"] = userId.ToString();
                    //string IsAdmin = securityMgr.IsAdmin(userId).ToString();
                    //string Privileges = UserMgr.UserPrivilageAsstring(userId);
                    //FormsAuthentication.SetAuthCookie(userId.ToString() + "&" + userFullName + "&" + IsAdmin + "&" + Privileges, false);
                    //return RedirectToLocal(returnUrl);

                    Session["UserID"] = _User.ID;
                    Session["BranchID"] = _User.BranchID;
                    FormsAuthentication.SetAuthCookie(_User.ID.ToString() + "&" + _User.NameAr + "&", false);
                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult GetNewStudents()
        {
            DateTime SDate = DateTime.Now;
            List<TblStudent> data = new List<TblStudent>();
            List<newStudentViewModel> result = new List<newStudentViewModel>();
            try
            {
                data = _Context.TblStudents.Where(t => t.CreatedDate.Day > (SDate.Day - 3) && t.CreatedDate.Month == SDate.Month && t.CreatedDate.Year == SDate.Year && t.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
            }
            if (data != null)
            {
                foreach (TblStudent item in data)
                {
                    newStudentViewModel x = new newStudentViewModel();

                    x.ID = item.ID;
                    x.StudentName = item.FirstName + " " + item.SecondName;
                    x.Branch = item.TblBranch.NameAr;
                    x.CollageName = item.TblCollege.NameAr;
                    x.Email = item.Email;
                    x.Phone = item.PhoneNumber;
                    result.Add(x);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetNewVoucher()
        {
            DateTime SDate = DateTime.Now;
            List<TblVoucher> data = new List<TblVoucher>();
            List<LatestVoucherViewModel> result = new List<LatestVoucherViewModel>();
            try
            {
                data = _Context.TblVouchers.Where(t => t.CreatedDate.Day > (SDate.Day - 3) && t.CreatedDate.Month == SDate.Month && t.CreatedDate.Year == SDate.Year && t.IsDeleted == false).OrderByDescending(a => a.ID).ToList();
            }
            catch (Exception ex)
            {
            }

            if (data != null)
            {
                foreach (TblVoucher item in data)
                {
                    LatestVoucherViewModel x = new LatestVoucherViewModel();
                    x.ID = item.ID;
                    x.lecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr;
                    x.HallCode = item.TblHall.HallCodeAr;
                    x.createdDate = item.CreatedDate.ToShortDateString();
                    x.Serial = (double)item.Serial;
                    x.PaymentMethod = item.PaymentMethod;
                    x.Cost = item.Cost;
                    result.Add(x);
                }
            }
            return Json(result);
        }
    }
}