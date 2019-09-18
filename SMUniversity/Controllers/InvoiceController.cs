using SMUModels;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ActionResult RealCashInvoices()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<TblInvoice> InvoicesList = _Context.TblInvoices.Where(a => a.RealCash == true && a.Pending == false && a.IsDeleted != true).ToList();

            return View(InvoicesList);
        }

        public ActionResult VirtualInvoices()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            List<TblInvoice> InvoicesList = _Context.TblInvoices.Where(a => a.RealCash == false && a.Pending == false && a.IsDeleted != true).ToList();

            return View(InvoicesList);
        }

        public ActionResult VoucherList()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult VoucherList2()
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

        public ActionResult InvoiceTemplate(int InvoiceID)
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var Invoice = _Context.TblInvoices.Where(a => a.ID == InvoiceID).SingleOrDefault();

            return View(Invoice);
        }

        public ActionResult CreateVoucher()
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                List<TblVoucherCategory> _VoucherCategories = _Context.TblVoucherCategories.Where(a => a.IsDeleted != true).ToList();
                return View(_VoucherCategories);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("CreateVoucher");
            }
        }

        [HttpPost]
        public ActionResult CreateVoucher(VoucherObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.FromDate.ToString()) && !string.IsNullOrEmpty(_Data.ToDate.ToString()) && _Data.CategoryID > 0 && _Data.Cost > 0)
                {
                    Random generator = new Random();
                    TblVoucher _VoucherObj = new TblVoucher()
                    {
                        Name = _Data.Name,
                        FromDate = _Data.FromDate,
                        ToDate = _Data.ToDate,
                        Cost = _Data.Cost,
                        CategoryID = _Data.CategoryID,
                        Type = true,
                        Notes = _Data.Notes,
                        PaymentMethod = "Cash",
                        UserID = 1, //temp admin account
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    int Serial;
                    var CountVouchers = _Context.TblVouchers.Count();
                    if (CountVouchers > 0)
                    {
                        //List<TblVoucher> lastcode = _Context.TblVouchers.ToList();
                        long MyMax = _Context.TblVouchers.Max(a => a.Serial);

                        Serial = int.Parse(MyMax.ToString()) + 1;
                        _VoucherObj.Serial = Serial;
                    }
                    else
                    {
                        _VoucherObj.Serial = 1;
                        //if (BranchID == 1)
                        //{
                        //    _VoucherObj.Serial = "1281";
                        //}
                        //else if (BranchID == 3)
                        //{
                        //    _VoucherObj.Serial = "2401";
                        //}
                        //else if (BranchID == 5)
                        //{
                        //    _VoucherObj.Serial = "1";
                        //}
                        //else if (BranchID == 6)
                        //{
                        //    _VoucherObj.Serial = "353";
                        //}
                        //else
                        //{
                        //    _VoucherObj.Serial = "1";
                        //}
                    }
                    _Context.TblVouchers.Add(_VoucherObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه البيانات بنجاح";
                    return RedirectToAction("VoucherList2");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("CreateVoucher");
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("CreateVoucher");
            }
        }

    }
}