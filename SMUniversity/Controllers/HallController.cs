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
    public class HallController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Hall
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Create()
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
        public ActionResult Create(HallObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.HallCodeAr) && !string.IsNullOrEmpty(_Data.HallCodeEn) && _Data.Capacity > 0)
                {
                    TblHall HallObj = new TblHall()
                    {
                        HallCodeAr = _Data.HallCodeAr,
                        HallCodeEn = _Data.HallCodeEn,
                        Capacity = _Data.Capacity,
                        BranchID = 3,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblHalls.Add(HallObj);
                    _Context.SaveChanges();

                    TempData["notice"] = "تم إضافه القاعه بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Create");
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        public ActionResult RentHall()
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                DateTime CurrentDate = DateTime.Now.Date;

                List<int> HallsVouchersIDs = _Context.TblVouchers.Where(a => DbFunctions.TruncateTime(a.FromDate) <= CurrentDate && DbFunctions.TruncateTime(a.ToDate) >= CurrentDate).Select(a => a.HallID).ToList();
                List<TblLecturer> LecturesList = _Context.TblLecturers.Where(a => a.IsDeleted == false).ToList();
                List<TblHall> HallsList = _Context.TblHalls.Where(a => a.IsDeleted == false && !HallsVouchersIDs.Contains(a.ID)).ToList();

                ViewBag.HallsList = HallsList;

                return View(LecturesList);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("RentHall");
            }
        }

        [HttpPost]
        public ActionResult RentHall(VoucherObj _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.FromDate.ToString()) && !string.IsNullOrEmpty(_Data.ToDate.ToString()) && _Data.HallID > 0 && (_Data.LecturerID > 0 || _Data.LecturerID == -1) )
                {
                    Random generator = new Random();
                    TblVoucher _VoucherObj = new TblVoucher()
                    {
                        //LecturerID = _Data.LecturerID,
                        HallID = _Data.HallID,
                        UserID = 1, //temp admin account
                        Cost = _Data.Cost,
                        FromDate = _Data.FromDate,
                        ToDate = _Data.ToDate,
                        Notes = _Data.Notes,
                        PaymentMethod = "Cash",
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    if (_Data.LecturerID == -1)
                    {
                        TblLecturer LecturerObj = new TblLecturer()
                        {
                            FirstNameAr = _Data.FirstName,
                            FirstNameEn = _Data.FirstName,
                            SecondNameAr = _Data.SecondName,
                            SecondNameEn = _Data.SecondName,
                            ThirdNameAr = _Data.ThirdName,
                            ThirdNameEn = _Data.ThirdName,
                            Email = _Data.Email,
                            PhoneNumber = _Data.PhoneNumber,
                            Address = "",
                            BranchID = 3,
                            Gender = false,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                        };

                        string UserName = _Data.FirstName + generator.Next(0, 99999).ToString("D5");
                        TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == UserName && a.UserType == 2).SingleOrDefault();
                        if (_UserCred == null)
                        {
                            UserName = _Data.FirstName + generator.Next(0, 99999).ToString("D5");
                        }

                        _UserCred = new TblUserCredential()
                        {
                            UserName = UserName,
                            Password = "123456",
                            UserType = 2,
                        };

                        _Context.TblUserCredentials.Add(_UserCred);
                        _Context.SaveChanges();

                        LecturerObj.CredentialsID = _UserCred.ID;
                        _Context.TblLecturers.Add(LecturerObj);
                        _Context.SaveChanges();

                        _VoucherObj.LecturerID = LecturerObj.ID;
                        _Context.SaveChanges();
                    }
                    else
                    {
                        _VoucherObj.LecturerID = _Data.LecturerID;
                    }

                    _Context.TblVouchers.Add(_VoucherObj);
                    _Context.SaveChanges();




                    TempData["notice"] = "تم إضافه البيانات بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("RentHall");
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("RentHall");
            }
        }

        public ActionResult Edit(int HallID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (HallID > 0)
                {
                    TblHall _Hall = _Context.TblHalls.Where(a => a.ID == HallID && a.IsDeleted != true).FirstOrDefault();

                    return View(_Hall);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { HallID = HallID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { HallID = HallID });
            }
        }

        [HttpPost]
        public ActionResult Edit(HallObj _Data)
        {
            try
            {
                if (_Data.HallID > 0 && !string.IsNullOrEmpty(_Data.HallCodeAr) && !string.IsNullOrEmpty(_Data.HallCodeEn))
                {
                    TblHall HallObj = _Context.TblHalls.Where(a => a.ID == _Data.HallID).SingleOrDefault();
                    if (HallObj != null)
                    {
                        HallObj.HallCodeAr = _Data.HallCodeAr;
                        HallObj.HallCodeEn = _Data.HallCodeEn;
                        HallObj.Capacity = _Data.Capacity;
                        HallObj.UpdatedDate = DateTime.Now;

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات القاعه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { HallID = _Data.HallID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { HallID = _Data.HallID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { HallID = _Data.HallID });
            }
        }

        public JsonResult Delete(int HallID)
        {
            try
            {
                if (HallID > 0)
                {
                    //TempData["notice"] = "غير قادر علي الحذف, المحافظه مرتيطه ببيانات في المناطق والطلاب";
                    //return Json("OK");
                    TblHall HallObj = _Context.TblHalls.Where(a => a.ID == HallID).SingleOrDefault();
                    if (HallObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف قاعه " + HallObj.HallCodeAr + " بنجاح";

                            _Context.TblHalls.Remove(HallObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, القاعه مرتيطه ببيانات في ببيانات الفروع او المحاضرات او السندات اليدوية او الفواتير او الشاشات";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Hall not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Hall not found!";
                    return Json("ERROR");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return Json("ERROR");
            }
        }

    }
}