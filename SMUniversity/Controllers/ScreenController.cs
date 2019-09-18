using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class ScreenController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Screen
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

                List<TblHall> HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();
                return View(HallsList);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult Create(FormCollection _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && !string.IsNullOrEmpty(_Data["HallIDs []"]))
                {
                    string UserName = _Data["UserName"];
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == UserName && a.UserType == 3).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        string[] HallIDsList = _Data["HallIDs []"].Split(',');

                        TblScreen ScreenObj = new TblScreen()
                        {
                            NameAr = _Data["NameAr"],
                            NameEn = _Data["NameEn"],
                            //BranchID = int.Parse(_Data["BranchID"]),
                            BranchID = 3,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        _Context.TblScreens.Add(ScreenObj);

                        _UserCred = new TblUserCredential()
                        {
                            UserName = _Data["UserName"],
                            Password = _Data["Password"],
                            UserType = 3,
                        };

                        _Context.TblUserCredentials.Add(_UserCred);
                        _Context.SaveChanges();

                        ScreenObj.CredentialsID = _UserCred.ID;
                        _Context.SaveChanges();

                        //_Context.TblScreenHalls.RemoveRange()
                        for (int i = 0; i < HallIDsList.Count(); i++)
                        {
                            TblScreenHall ScreenHallObj = new TblScreenHall()
                            {
                                ScreenID = ScreenObj.ID,
                                HallID = int.Parse(HallIDsList[i]),
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };

                            _Context.TblScreenHalls.Add(ScreenHallObj);
                        }
                        _Context.SaveChanges();

                        TempData["notice"] = "تم إضافه الشاشه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "اسم المستخدم مستخدم من قبل, من فضلك ادخل اسم مستخدم آخر";
                        return RedirectToAction("Create");
                    }

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

        public ActionResult Edit(int ScreenID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (ScreenID > 0)
                {
                    TblScreen _Screen = _Context.TblScreens.Where(a => a.ID == ScreenID).SingleOrDefault();
                    List<TblHall> HallsList = _Context.TblHalls.Where(a => a.IsDeleted != true).ToList();

                    ViewBag.HallsList = HallsList;

                    return View(_Screen);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { ScreenID = ScreenID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { ScreenID = ScreenID });
            }
        }

        [HttpPost]
        public ActionResult Edit(FormCollection _Data)
        {
            try
            {
                if (int.Parse(_Data["ScreenID"]) > 0 && !string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && !string.IsNullOrEmpty(_Data["HallIDs []"]))
                {
                    int ScreenID = int.Parse(_Data["ScreenID"]);
                    TblScreen ScreenObj = _Context.TblScreens.Where(a => a.ID == ScreenID).SingleOrDefault();
                    if (ScreenObj != null)
                    {
                        string[] HallIDsList = _Data["HallIDs []"].Split(',');

                        ScreenObj.NameAr = _Data["NameAr"];
                        ScreenObj.NameEn = _Data["NameEn"];
                        //ScreenObj.BranchID = int.Parse(_Data["BranchID"]);
                        ScreenObj.BranchID = 3; //temp assign real branch
                        ScreenObj.UpdatedDate = DateTime.Now;

                        //_Context.TblScreenHalls.RemoveRange(_Context.TblScreenHalls.Where(a => a.ID == ScreenID).ToList());
                        //_Context.SaveChanges();

                        for (int i = 0; i < HallIDsList.Length; i++)
                        {
                            int _HallID = int.Parse(HallIDsList[i]);
                            if (_Context.TblScreenHalls.Where(a => a.ID == ScreenID && a.HallID == _HallID).FirstOrDefault() == null)
                            {
                                TblScreenHall ScreenHallObj = new TblScreenHall()
                                {
                                    ScreenID = ScreenObj.ID,
                                    HallID = _HallID,
                                    IsDeleted = false,
                                    CreatedDate = DateTime.Now
                                };

                                _Context.TblScreenHalls.Add(ScreenHallObj);
                            }
                        }

                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات الشاشه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { ScreenID = ScreenID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { ScreenID = _Data["ScreenID"] });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { ScreenID = _Data["ScreenID"] });
            }
        }

        public JsonResult Delete(int ScreenID)
        {
            try
            {
                if (ScreenID > 0)
                {
                    TblScreen ScreenObj = _Context.TblScreens.Where(a => a.ID == ScreenID).SingleOrDefault();
                    if (ScreenObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف الشاشه بنجاح";

                            _Context.TblUserCredentials.Remove(_Context.TblUserCredentials.Where(a => a.ID == ScreenObj.CredentialsID).FirstOrDefault());
                            _Context.SaveChanges();

                            _Context.TblScreenHalls.RemoveRange(_Context.TblScreenHalls.Where(a => a.ScreenID == ScreenID).ToList());
                            _Context.SaveChanges();

                            _Context.TblScreens.Remove(ScreenObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, الشاشه مرتبط ببيانات خاصه بالفواتير";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Screen not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Screen not found!";
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