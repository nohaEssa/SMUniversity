using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class BalanceTransactionController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: BalanceTransaction
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult PendedChargeRequest()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public JsonResult ConfirmTransaction(int TransactionID)
        {
            try
            {
                TblBalanceTransaction _BalanceTrans = _Context.TblBalanceTransactions.Where(a => a.ID == TransactionID).SingleOrDefault();
                if (_BalanceTrans != null)
                {
                    _BalanceTrans.Pending = false;
                    _BalanceTrans.UserID = 1;
                    _BalanceTrans.UpdatedDate = DateTime.Now;
                    _BalanceTrans.TblStudent.Balance += decimal.Parse(_BalanceTrans.Price.ToString());

                    _Context.SaveChanges();

                    TempData["notice"] = "تم تأكيد شحن مبلغ " + _BalanceTrans.Price + " ودخوله في حساب الطالب " + _BalanceTrans.TblStudent.FirstName + " " + _BalanceTrans.TblStudent.SecondName + " " + _BalanceTrans.TblStudent.ThirdName + " بنجاح";
                    //return RedirectToAction("PendedChargeRequest");
                    return Json("OK");
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return Json("ERROR");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return Json("ERROR");
            }
        }

        //[HttpPost]
        public ActionResult index(jQueryDataTableParamModel param)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //int BranchID = int.Parse(Session["BranchID"].ToString());

                List<TblBalanceTransaction> oData = new List<TblBalanceTransaction>();
                IEnumerable<TblBalanceTransaction> filteredData;
                //Check whether the companies should be filtered by keyword
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    string sSearch = param.sSearch.Trim().ToLower();

                    filteredData = _Context.TblBalanceTransactions.Where(c => c.IsDeleted != true && c.Pending == true).ToList();

                    foreach (var item in filteredData)
                    {
                        if (item.StudentID.ToString().ToLower().Contains(sSearch) || (item.TblStudent.FirstName + " " + item.TblStudent.FirstName + " " + item.TblStudent.FirstName).ToLower().Contains(sSearch))
                        {
                            oData.Add(item);
                        }
                    }

                    filteredData = oData;
                }
                else
                {
                    filteredData = _Context.TblBalanceTransactions.Where(c => c.IsDeleted != true && c.Pending == true).ToList();

                }

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                Func<TblBalanceTransaction, string> orderingFunction = (c => sortColumnIndex == 0 ? Convert.ToString(c.StudentID) :
                                                               sortColumnIndex == 1 ? string.Join(c.TblStudent.FirstName, c.TblStudent.SecondName, c.TblStudent.ThirdName) :
                                                               sortColumnIndex == 2 ? c.TblStudent.PhoneNumber :
                                                               "");

                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                    filteredData = filteredData.OrderBy(orderingFunction);
                else
                    filteredData = filteredData.OrderByDescending(orderingFunction);

                if (param.iDisplayLength == -1)
                {
                    param.iDisplayLength = filteredData.Count();
                }

                var displayedData = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                var result = from c in displayedData select new[] { Convert.ToString(c.StudentID), c.TblStudent.FirstName + " " + c.TblStudent.SecondName + " " + c.TblStudent.ThirdName, c.TblStudent.PhoneNumber };
                var jsonResult = Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = _Context.TblBalanceTransactions.Count(),
                    iTotalDisplayRecords = filteredData.Count(),
                    aaData = result,

                },
                            JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
                //return Json(new
                //{
                //    sEcho = param.sEcho,
                //    iTotalRecords = _Context.Tbl_Students.Count(),
                //    iTotalDisplayRecords = filteredData.Count(),
                //    aaData = result,

                //},
                //            JsonRequestBehavior.AllowGet);
            }
        }
    }
}