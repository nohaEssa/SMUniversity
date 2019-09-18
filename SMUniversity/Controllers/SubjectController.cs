using SMUModels;
using SMUModels.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class SubjectController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: Subject
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
                //List<TblMajor> MajorsList = _Context.TblMajors.Where(a => a.IsDeleted != true).ToList();
                //List<TblLecturer> LecturersList = _Context.TblLecturers.Where(a => a.IsDeleted != true).ToList();
                List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();

                //ViewBag.LecturersList = LecturersList;
                return View(UniversitiesList);
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
                string SubjectCode = _Data["SubjectCode"];
                TblSubject SubjectObj = _Context.TblSubjects.Where(a => a.SubjectCode == SubjectCode).FirstOrDefault();
                if(SubjectObj== null)
                {
                    //if (!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && int.Parse(_Data["MajorID"]) > 0 && !string.IsNullOrEmpty(_Data["LecturerIDs []"]))
                    if ((!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && int.Parse(_Data["MajorID"]) > 0 && bool.Parse(_Data["GeneralSubject"]) == false) || (!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && bool.Parse(_Data["GeneralSubject"]) == true))
                    {
                        //string[] LecturerIDsList = _Data["LecturerIDs []"].Split(',');
                        Random generator = new Random();
                        SubjectObj = new TblSubject()
                        {
                            NameAr = _Data["NameAr"],
                            NameEn = _Data["NameEn"],
                            GeneralSubject = bool.Parse(_Data["GeneralSubject"]),
                            //SubjectCode = generator.Next(0, 99999).ToString("D5"),
                            SubjectCode = _Data["SubjectCode"],
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };
                        if (!bool.Parse(_Data["GeneralSubject"]))
                        {
                            SubjectObj.MajorID = int.Parse(_Data["MajorID"]);
                        }
                        //if (Request.Files["SubjectPic"] != null)
                        //{
                        //    UploadFileResponse Respo = UploadPhoto(Request.Files["SubjectPic"]);
                        //    if (Respo.Code == 1)
                        //    {
                        //        SubjectObj.Picture = Respo.Message;
                        //    }
                        //}

                        _Context.TblSubjects.Add(SubjectObj);
                        _Context.SaveChanges();

                        //for (int i = 0; i < LecturerIDsList.Count(); i++)
                        //{
                        //    TblLecturerSubject LecSubObj = new TblLecturerSubject()
                        //    {
                        //        SubjectID = SubjectObj.ID,
                        //        LecturerID = int.Parse(LecturerIDsList[i]),
                        //        IsDeleted = false,
                        //        CreatedDate = DateTime.Now
                        //    };

                        //    _Context.TblLecturerSubjects.Add(LecSubObj);
                        //}

                        //_Context.SaveChanges();

                        TempData["notice"] = "تم إضافه الماده بنجاح";
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                        return RedirectToAction("Create");
                    }
                }
                else
                {
                    TempData["notice"] = "تم إدخال الكود من قبل, ادخل كود اخر";
                    return RedirectToAction("Create");
                }
                
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        public ActionResult Edit(int SubjectID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (SubjectID > 0)
                {
                    TblSubject SubjectObj = _Context.TblSubjects.Where(a => a.ID == SubjectID).SingleOrDefault();
                    List<TblMajor> MajorsList = _Context.TblMajors.Where(a => a.IsDeleted != true).ToList();
                    List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();

                    ViewBag.UniversitiesList = UniversitiesList;
                    ViewBag.MajorsList = MajorsList;
                    ViewBag.GeneralSubject = SubjectObj.GeneralSubject ? "1" : "0";

                    return View(SubjectObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { SubjectID = SubjectID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { SubjectID = SubjectID });
            }
        }

        [HttpPost]
        public ActionResult Edit(SubjectData _Data)
        {
            try
            {
                if (_Data.SubjectID > 0 && !string.IsNullOrEmpty(_Data.NameAr) && !string.IsNullOrEmpty(_Data.NameEn) && _Data.MajorID > 0)
                {
                    TblSubject SubjectObj = _Context.TblSubjects.Where(a => a.ID != _Data.SubjectID && a.SubjectCode == _Data.SubjectCode).FirstOrDefault();
                    if (SubjectObj == null)
                    {
                        SubjectObj.NameAr = _Data.NameAr;
                        SubjectObj.NameEn = _Data.NameEn;
                        SubjectObj.GeneralSubject = _Data.GeneralSubject;
                        SubjectObj.SubjectCode = _Data.SubjectCode;
                        SubjectObj.UpdatedDate = DateTime.Now;
                        if (_Data.GeneralSubject == false)
                        {
                            SubjectObj.MajorID = _Data.MajorID;
                        }
                        if (Request.Files["SubjectPic"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["SubjectPic"]);
                            if (Respo.Code == 1)
                            {
                                SubjectObj.Picture = Respo.Message;
                            }
                        }
                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات الماده بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "تم إدخال الكود من قبل, ادخل كود اخر";
                        return RedirectToAction("Edit", new { SubjectID = _Data.SubjectID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { SubjectID = _Data.SubjectID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { SubjectID = _Data.SubjectID });
            }
        }

        public JsonResult getSubjects(int MajorID, int SubjectID = 0, int SessionType = 0)
        {
            try
            {
                string Result = "";
                List<TblSubject> _SubjectsList = _Context.TblSubjects.Where(a => a.MajorID == MajorID /*&& a.GeneralSubject == false*/ && a.IsDeleted != true).ToList();
                if (SessionType == 1)
                {
                    _SubjectsList = _Context.TblSubjects.Where(a => a.GeneralSubject == true && a.IsDeleted != true).ToList();
                }
                Result += "<option value='0'>الماده</option>";
                foreach (var subject in _SubjectsList)
                {
                    if (subject.ID != SubjectID)
                    {
                        Result += "<option value='" + subject.ID + "'>" + subject.NameAr + "</option>";
                    }
                    else
                    {
                        Result += "<option value='" + subject.ID + "' selected>" + subject.NameAr + "</option>";
                    }
                }

                return Json(Result);
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
        }

        public UploadFileResponse UploadPhoto(HttpPostedFileBase Image)
        {
            UploadFileResponse Respo = new UploadFileResponse();
            try
            {
                //var ImagePath = Request.Files["StudentPic"];

                if (Image != null && Image.ContentLength > 0)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png" };
                    var fileExt = System.IO.Path.GetExtension(Image.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        Respo.Code = 0;
                        Respo.Message = "Invalid type. Only the following types (jpg, jpeg, png) are supported. ";
                        return Respo;
                    }
                    else
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                        var uploadUrl = Server.MapPath("~/Content/Images/Subject/");

                        Image.SaveAs(Path.Combine(uploadUrl, fileName));

                        Respo.Code = 1;
                        Respo.Message = "/Content/Images/Subject/" + fileName;

                        return Respo;
                    }
                }
                else
                {
                    Respo.Code = 2;
                    Respo.Message = "من فضلك اختر صوره ";
                    return Respo;
                }

            }
            catch (Exception ex)
            {
                Respo.Code = 4;
                Respo.Message = ex.Message;
                return Respo;
            }
        }

        public JsonResult Delete(int SubjectID)
        {
            try
            {
                if (SubjectID > 0)
                {
                    TblSubject SubjectObj = _Context.TblSubjects.Where(a => a.ID == SubjectID).SingleOrDefault();
                    if (SubjectObj != null)
                    {
                        try
                        {
                            TempData["notice"] = "تم حذف الماده : " + SubjectObj.NameAr + " بنجاح";

                            _Context.TblSubjects.Remove(SubjectObj);
                            _Context.SaveChanges();

                            return Json("OK");
                        }
                        catch (Exception ex)
                        {
                            TempData["notice"] = "غير قادر علي الحذف, الماده مرتبطه ببيانات خاصه بالمحاضرات الخاصه والمحاضرين";
                            return Json("OK");
                        }

                    }
                    else
                    {
                        TempData["notice"] = "Subject not found!";
                        return Json("ERROR");
                    }
                }
                else
                {
                    TempData["notice"] = "Subject not found!";
                    return Json("ERROR");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return Json("ERROR");
            }
        }

        public JsonResult CheckSubjectCodeAvailability(string SubjectCode, int SubjectID = 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(SubjectCode))
                {
                    TblSubject SubjectObj = _Context.TblSubjects.Where(a => a.SubjectCode == SubjectCode).SingleOrDefault();
                    if (SubjectID > 0)
                    {
                        SubjectObj = _Context.TblSubjects.Where(a => a.ID != SubjectID && a.SubjectCode == SubjectCode).SingleOrDefault();
                    }
                    if (SubjectObj == null)
                    {
                        return Json("OK");
                    }
                    else
                    {
                        return Json("ERROR");
                    }
                }
                else
                {
                    return Json("ERROR");
                }
            }
            catch (Exception ex)
            {
                return Json("ERROR");
            }
        }

    }
}