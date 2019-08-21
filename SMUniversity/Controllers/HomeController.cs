using SMUModels;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}