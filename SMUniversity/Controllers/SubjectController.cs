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
                List<TblMajor> MajorsList = _Context.TblMajors.Where(a => a.IsDeleted != true).ToList();
                List<TblLecturer> LecturersList = _Context.TblLecturers.Where(a => a.IsDeleted != true).ToList();

                ViewBag.LecturersList = LecturersList;
                return View(MajorsList);
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
                //if (!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && int.Parse(_Data["MajorID"]) > 0 && !string.IsNullOrEmpty(_Data["LecturerIDs []"]))
                if (!string.IsNullOrEmpty(_Data["NameAr"]) && !string.IsNullOrEmpty(_Data["NameEn"]) && int.Parse(_Data["MajorID"]) > 0)
                {
                    //string[] LecturerIDsList = _Data["LecturerIDs []"].Split(',');

                    TblSubject SubjectObj = new TblSubject()
                    {
                        NameAr = _Data["NameAr"],
                        NameEn = _Data["NameEn"],
                        MajorID = int.Parse(_Data["MajorID"]),
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    if (Request.Files["SubjectPic"] != null)
                    {
                        UploadFileResponse Respo = UploadPhoto(Request.Files["SubjectPic"]);
                        if (Respo.Code == 1)
                        {
                            SubjectObj.Picture = Respo.Message;
                        }
                    }

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
                    ViewBag.MajorsList = MajorsList;

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
                    TblSubject SubjectObj = _Context.TblSubjects.Where(a => a.ID == _Data.SubjectID).SingleOrDefault();
                    if (SubjectObj != null)
                    {
                        SubjectObj.NameAr = _Data.NameAr;
                        SubjectObj.NameEn = _Data.NameEn;
                        SubjectObj.MajorID = _Data.MajorID;
                        SubjectObj.UpdatedDate = DateTime.Now;

                        if (Request.Files["SubjectPic"] != null)
                        {
                            UploadFileResponse Respo = UploadPhoto(Request.Files["SubjectPic"]);
                            if (Respo.Code == 1)
                            {
                                SubjectObj.Picture = Respo.Message;
                            }
                        }
                        _Context.SaveChanges();

                        TempData["notice"] = "تم تعديل بيانات التخصص بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
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

        public JsonResult getSubjects(int MajorID)
        {
            try
            {
                string Result = "";
                List<TblSubject> _SubjectsList = _Context.TblSubjects.Where(a => a.MajorID == MajorID && a.IsDeleted != true).ToList();
                Result += "<option value='0'>الماده</option>";
                foreach (var subject in _SubjectsList)
                {
                    Result += "<option value=' " + subject.ID + "'>" + subject.NameAr + "</option>";
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

    }
}