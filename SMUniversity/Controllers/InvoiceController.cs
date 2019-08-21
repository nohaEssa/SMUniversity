using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class InvoiceController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Invoice
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Invoice(int InvoiceID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var Invoice = _Context.TblInvoices.Where(a => a.ID == InvoiceID).SingleOrDefault();

            return View(Invoice);
        }

        public ActionResult VoucherList()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult VoucherTemplate(int VoucherID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var Voucher = _Context.TblVouchers.Where(a => a.ID == VoucherID).SingleOrDefault();

            return View(Voucher);
        }
    }
}