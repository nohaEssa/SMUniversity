using SMUModels;
using SMUModels.Classes;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class StudentController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage Register(StudentObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                if (ModelState.IsValid)
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Params.UserName && a.UserType == 1).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Params.PhoneNumber).FirstOrDefault();
                        if (Std == null)
                        {
                            TblStudent StudentObj = new TblStudent()
                            {
                                FirstName = _Params.FirstName,
                                //SecondName = _Params.SecondName,
                                SecondName = "",
                                ThirdName = _Params.ThirdName,
                                FirstNameEn = !string.IsNullOrEmpty(_Params.FirstNameEn) ? _Params.FirstNameEn : _Params.FirstName,
                                //SecondNameEn = !string.IsNullOrEmpty(_Params.SecondNameEn) ? _Params.SecondNameEn : _Params.SecondName,
                                SecondNameEn = "",
                                ThirdNameEn = !string.IsNullOrEmpty(_Params.ThirdNameEn) ? _Params.ThirdNameEn : _Params.ThirdName,
                                PhoneNumber = _Params.PhoneNumber,
                                Email = _Params.Email,
                                Gender = _Params.Gender, // 0 : Female , 1 : Male
                                UniversityID = _Params.UniversityID != 0 ? _Params.UniversityID : null,
                                CollegeID = _Params.CollegeID != 0 ? _Params.CollegeID : null,
                                MajorID = _Params.MajorID != 0 ? _Params.MajorID : null,
                                GovernorateID = _Params.GovernorateID,
                                //GovernorateID = _Params.GovernateID,
                                AreaID = _Params.AreaID,
                                BranchID = _Params.BranchID == 0 ? 3 : _Params.BranchID,
                                //CredentialsID = _UserCred.ID,
                                ProfilePic = "https://uni.smartmindkw.com/Content/Images/Student/DefaultStudentPhoto.png",
                                Verified = false,
                                StudentType = _Params.StudentType, //true isa normal student, false is a course student
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };
                            try
                            {
                                //StudentObj.DateOfBirth = DateTime.Parse(_Params.DateOfBirth).Date;
                                StudentObj.DateOfBirth = _Params.DateOfBirth.Date;
                            }
                            catch (Exception ex)
                            {
                                //_resultHandler.IsSuccessful = false;
                                //_resultHandler.MessageAr = ex.Message;
                                //_resultHandler.MessageEn = "Not Valid";
                                //_resultHandler.Result = ex;

                                //return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                            }
                            _Context.TblStudents.Add(StudentObj);
                            _Context.SaveChanges();

                            _UserCred = new TblUserCredential()
                            {
                                UserName = _Params.UserName,
                                Password = _Params.Password,
                                UserType = 1,
                            };

                            _Context.TblUserCredentials.Add(_UserCred);
                            _Context.SaveChanges();

                            StudentObj.CredentialsID = _UserCred.ID;
                            _Context.SaveChanges();

                            string AddTokenResult = AddDevice(StudentObj.ID, _Params.Token, _Params.DeviceTypeID);

                            ResultHandler res = SendCodeSMS(StudentObj.PhoneNumber);
                            if (res.IsSuccessful)
                            {
                                //StudentObj.Verified = true;
                                StudentObj.VerificationCode = res.Result.ToString();

                                _Context.SaveChanges();
                            }

                            string TitleAr = "سمارت مايند الجامعه";
                            string TitleEn = "SmartMind University";
                            string DescriptionAr = "لقد قمت بتسجيل حساب في سمارت مايند الجامعه..اسم المستخدم : " + _Params.UserName + " والرقم السري : " + _Params.Password + " , كود التفعيل : " + StudentObj.VerificationCode;
                            string DescriptionEn = "You've been registered in Smart Mind University..UserName is : " + _Params.UserName + " Password is : " + _Params.Password;

                            Push(StudentObj.ID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);
                            try
                            {
                                SendEmail(StudentObj.Email, StudentObj.FirstName + " " + StudentObj.SecondName + " " + StudentObj.ThirdName, _Params.UserName, _Params.Password, res.Result.ToString());
                            }
                            catch (Exception ex)
                            {
                            }
                            _resultHandler.IsSuccessful = true;
                            _resultHandler.Result = getStudentData(StudentObj.ID);
                            _resultHandler.MessageAr = "OK";
                            _resultHandler.MessageEn = "OK";

                            return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                        }
                        else
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
                            _resultHandler.MessageEn = "Phone Number is already exists, please provide another one";

                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                        }

                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "Username is already used, please enter another one";
                        _resultHandler.MessageEn = "اسم المستخدم موجود بالفعل, من فضلك ادخل واحد آخر";

                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Not Valid";
                    _resultHandler.MessageEn = "Not Valid";
                    _resultHandler.Result = ModelState.Values.Select(a => a.Errors);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }
            }
            catch (Exception ex)
            {

                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.InnerException.Message;
                _resultHandler.MessageEn = ex.InnerException.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpPost]
        public HttpResponseMessage RegisterV2(StudentObj _Params)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                if (ModelState.IsValid)
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Params.UserName && a.UserType == 1).SingleOrDefault();
                    if (_UserCred == null)
                    {
                        TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Params.PhoneNumber).FirstOrDefault();
                        if (Std == null)
                        {
                            TblStudent StudentObj = new TblStudent()
                            {
                                FirstName = _Params.FirstName,
                                SecondName = _Params.SecondName,
                                ThirdName = _Params.ThirdName,
                                FirstNameEn = !string.IsNullOrEmpty(_Params.FirstNameEn) ? _Params.FirstNameEn : _Params.FirstName,
                                SecondNameEn = !string.IsNullOrEmpty(_Params.SecondNameEn) ? _Params.SecondNameEn : _Params.SecondName,
                                ThirdNameEn = !string.IsNullOrEmpty(_Params.ThirdNameEn) ? _Params.ThirdNameEn : _Params.ThirdName,
                                PhoneNumber = _Params.PhoneNumber,
                                Email = _Params.Email,
                                Gender = _Params.Gender, // 0 : Female , 1 : Male
                                UniversityID = _Params.UniversityID != 0 ? _Params.UniversityID : null,
                                CollegeID = _Params.CollegeID != 0 ? _Params.CollegeID : null,
                                MajorID = _Params.MajorID != 0 ? _Params.MajorID : null,
                                GovernorateID = _Params.GovernorateID,
                                //GovernorateID = _Params.GovernateID,
                                AreaID = _Params.AreaID,
                                BranchID = _Params.BranchID == 0 ? 3 : _Params.BranchID,
                                //CredentialsID = _UserCred.ID,
                                ProfilePic = "https://uni.smartmindkw.com/Content/Images/Student/DefaultStudentPhoto.png",
                                Verified = false,
                                StudentType = _Params.StudentType, //true isa normal student, false is a course student
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };
                            try
                            {
                                //StudentObj.DateOfBirth = DateTime.Parse(_Params.DateOfBirth).Date;
                                StudentObj.DateOfBirth = _Params.DateOfBirth.Date;
                            }
                            catch (Exception ex)
                            {
                                //_resultHandler.IsSuccessful = false;
                                //_resultHandler.MessageAr = ex.Message;
                                //_resultHandler.MessageEn = "Not Valid";
                                //_resultHandler.Result = ex;

                                //return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                            }
                            _Context.TblStudents.Add(StudentObj);
                            _Context.SaveChanges();

                            _UserCred = new TblUserCredential()
                            {
                                UserName = _Params.UserName,
                                Password = _Params.Password,
                                UserType = 1,
                            };

                            _Context.TblUserCredentials.Add(_UserCred);
                            _Context.SaveChanges();

                            StudentObj.CredentialsID = _UserCred.ID;
                            _Context.SaveChanges();

                            string AddTokenResult = AddDevice(StudentObj.ID, _Params.Token, _Params.DeviceTypeID);

                            ResultHandler res = SendCodeSMS(StudentObj.PhoneNumber);
                            if (res.IsSuccessful)
                            {
                                StudentObj.Verified = true;
                                StudentObj.VerificationCode = res.Result.ToString();

                                _Context.SaveChanges();
                            }

                            string TitleAr = "سمارت مايند الجامعه";
                            string TitleEn = "SmartMind University";
                            string DescriptionAr = "لقد قمت بتسجيل حساب في سمارت مايند الجامعه..اسم المستخدم : " + _Params.UserName + " والرقم السري : " + _Params.Password;
                            string DescriptionEn = "You've been registered in Smart Mind University..UserName is : " + _Params.UserName + " Password is : " + _Params.Password;

                            Push(StudentObj.ID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                            _resultHandler.IsSuccessful = true;
                            _resultHandler.Result = getStudentData(StudentObj.ID);
                            _resultHandler.MessageAr = "OK";
                            _resultHandler.MessageEn = "OK";

                            return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                        }
                        else
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
                            _resultHandler.MessageEn = "Phone Number is already exists, please provide another one";

                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                        }

                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "Username is already used, please enter another one";
                        _resultHandler.MessageEn = "اسم المستخدم موجود بالفعل, من فضلك ادخل واحد آخر";

                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Not Valid";
                    _resultHandler.MessageEn = "Not Valid";
                    _resultHandler.Result = ModelState.Values.Select(a => a.Errors);

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }

            }
            catch (Exception ex)
            {

                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.InnerException.Message;
                _resultHandler.MessageEn = ex.InnerException.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }


        }

        [HttpPost]
        public HttpResponseMessage UpdateStudent(StudentObj _Params)
        {
            var _resultHandler = new ResultHandler();

            TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
            if (std != null)
            {
                TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Params.PhoneNumber && a.ID != _Params.StudentID).FirstOrDefault();
                if (Std == null)
                {
                    std.FirstName = _Params.FirstName;
                    std.SecondName = _Params.SecondName;
                    std.ThirdName = _Params.ThirdName;
                    std.PhoneNumber = _Params.PhoneNumber;
                    std.Email = _Params.Email;
                    std.Gender = _Params.Gender; // 0 : Female , 1 : Male
                    std.UniversityID = _Params.UniversityID;
                    std.CollegeID = _Params.CollegeID;
                    std.MajorID = _Params.MajorID;
                    //std.GovernorateID = _Params.GovernorateID;
                    std.GovernorateID = _Params.GovernorateID;
                    std.AreaID = _Params.AreaID;
                    std.UpdatedDate = DateTime.Now;

                    _Context.SaveChanges();

                    try
                    {
                        std.DateOfBirth = _Params.DateOfBirth.Date;
                    }
                    catch (Exception)
                    {
                    }
                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Params.StudentID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
                    _resultHandler.MessageEn = "Phone Number is already exists, please provide another one";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }
            }
            else
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = "Record is not found";
                _resultHandler.MessageEn = "هذا الحساب غير موجود";

                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
            }


        }

        [HttpPost]
        public HttpResponseMessage UpdateNameAndDOB(NameAndDOBObj _Params)
        {
            var _resultHandler = new ResultHandler();
            object Data = new object();

            TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
            if (_Student != null)
            {
                _Student.FirstName = _Params.FirstName;
                _Student.FirstNameEn = _Params.FirstNameEn;
                _Student.SecondName = _Params.SecondName;
                _Student.SecondNameEn = _Params.SecondNameEn;
                _Student.ThirdName = _Params.ThirdName;
                _Student.ThirdNameEn = _Params.ThirdNameEn;
                try
                {
                    _Student.DateOfBirth = _Params.DateOfBirth.Date;
                }
                catch (Exception)
                {
                }
                _Student.UpdatedDate = DateTime.Now;

                _Context.SaveChanges();

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = getStudentData(_Params.StudentID);
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            else
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = "Record is not found";
                _resultHandler.MessageEn = "هذا الحساب غير موجود";

                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
            }


        }

        [HttpPost]
        public HttpResponseMessage UpdateStageOfEdu(StageOfEduObj _Params)
        {
            var _resultHandler = new ResultHandler();

            TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
            if (std != null)
            {
                std.UniversityID = _Params.UniversityID;
                std.CollegeID = _Params.CollegeID;
                std.MajorID = _Params.MajorID;
                //std.GovernorateID = _Params.GovernorateID;
                std.GovernorateID = _Params.GovernorateID;
                std.AreaID = _Params.AreaID;
                std.BranchID = _Params.BranchID;
                std.UpdatedDate = DateTime.Now;

                _Context.SaveChanges();

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = getStudentData(_Params.StudentID);
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            else
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = "Record is not found";
                _resultHandler.MessageEn = "هذا الحساب غير موجود";

                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePhoneNumber(PhoneNumberObj _Params)
        {
            var _resultHandler = new ResultHandler();

            TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
            if (std != null)
            {
                TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Params.PhoneNumber && a.ID != _Params.StudentID).FirstOrDefault();
                if (Std == null)
                {
                    std.PhoneNumber = _Params.PhoneNumber;
                    std.UpdatedDate = DateTime.Now;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Params.StudentID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
                    _resultHandler.MessageEn = "Phone Number is already exists, please provide another one";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }
            }
            else
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = "Record is not found";
                _resultHandler.MessageEn = "هذا الحساب غير موجود";

                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateEmail(EmailObj _Params)
        {
            var _resultHandler = new ResultHandler();

            TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
            if (std != null)
            {
                TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Params.Email && a.ID != _Params.StudentID).FirstOrDefault();
                if (Std == null)
                {
                    std.Email = _Params.Email;
                    std.UpdatedDate = DateTime.Now;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Params.StudentID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "تم ادخال البريد الإلكتروني من قبل, أدخل رقم آخر";
                    _resultHandler.MessageEn = "Email is already exists, please provide another one";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }
            }
            else
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = "Record is not found";
                _resultHandler.MessageEn = "هذا الحساب غير موجود";

                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePassword(PasswordObj _Params)
        {
            var _resultHandler = new ResultHandler();
            if (_Params.Password.Equals(_Params.ConfirmPassword))
            {
                TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
                if (std != null)
                {
                    std.TblUserCredential.Password = _Params.Password;
                    std.UpdatedDate = DateTime.Now;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Params.StudentID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Record is not found";
                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                }
            }
            else
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = "الرقم السري وتأكيد الرقم السري غير متطابقين";
                _resultHandler.MessageEn = "Password and Confirm Password is not identical";

                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateGenderAndPic(GenderAndPicObj _Params)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                TblStudent student = _Context.TblStudents.Where(a => a.ID == 115).SingleOrDefault();
                //student.ProfilePic = _Params.ProfilePic;
                //_Context.SaveChanges();

                TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
                if (std != null)
                {
                    if (!string.IsNullOrEmpty(_Params.ProfilePic))
                    {
                        try
                        {
                            string base64String = _Params.ProfilePic;

                            String path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Student/"); //Path

                            //Check if directory exist
                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                            }
                            string imageName = Guid.NewGuid().ToString() + ".png";
                            //set the image path
                            string imgPath = Path.Combine(path, imageName);
                            byte[] imageBytes = Convert.FromBase64String(base64String);
                            File.WriteAllBytes(imgPath, imageBytes);

                            std.ProfilePic = "/Content/Images/Student/" + imageName;

                            //try
                            //{
                            //    string theFileName = Path.GetFileName(ImagePath.FileName);
                            //    byte[] thePictureAsBytes = new byte[ImagePath.ContentLength];
                            //    using (BinaryReader theReader = new BinaryReader(ImagePath.InputStream))
                            //    {
                            //        thePictureAsBytes = theReader.ReadBytes(ImagePath.ContentLength);
                            //    }
                            //    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                            //    //InvokeService("MenuCategory", thePictureDataAsString);
                            //    UploadFile.InvokeService("Branch", thePictureDataAsString, fileName);
                            //}
                            //catch (Exception)
                            //{
                            //}
                        }
                        catch (Exception ex)
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.Result = ex.InnerException;
                            _resultHandler.MessageAr = ex.Message;
                            _resultHandler.MessageEn = ex.Message;

                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                        }
                    }

                    std.Gender = _Params.Gender; // 0 : Female , 1 : Male
                    std.UpdatedDate = DateTime.Now;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Params.StudentID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Record is not found";
                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                }
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.Result = ex.InnerException;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
           }

        }

        public async Task<HttpResponseMessage> UpdatePic()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + extension);

                            postedFile.SaveAs(filePath);
                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdatePicInBytes(GenderAndPicObj _Params)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
                if (std != null)
                {
                    try
                    {
                        //string base64String = _Params.ProfilePic;

                        String path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/Student/"); //Path

                        //Check if directory exist
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                        }
                        string imageName = Guid.NewGuid().ToString() + ".png";
                        //set the image path
                        string imgPath = Path.Combine(path, imageName);
                        byte[] imageBytes = _Params.Image;
                        File.WriteAllBytes(imgPath, imageBytes);

                        std.ProfilePic = "/Content/Images/Student/" + imageName;

                    }
                    catch (Exception ex)
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.Result = ex.InnerException;
                        _resultHandler.MessageAr = ex.Message;
                        _resultHandler.MessageEn = ex.Message;

                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                    }

                    std.Gender = _Params.Gender; // 0 : Female , 1 : Male
                    std.UpdatedDate = DateTime.Now;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Params.StudentID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Record is not found";
                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                }
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.Result = ex.InnerException;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }

        }

        //[HttpPost]
        //public HttpResponseMessage UpdateStudent(StudentObj _Params)
        //{
        //    var _resultHandler = new ResultHandler();

        //    try
        //    {
        //        TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
        //        if (std != null)
        //        {
        //            TblStudent Std = _Context.TblStudents.Where(a => a.PhoneNumber == _Params.PhoneNumber && a.ID != _Params.StudentID).FirstOrDefault();
        //            if (Std == null)
        //            {
        //                std.FirstName = _Params.FirstName;
        //                std.SecondName = _Params.SecondName;
        //                std.ThirdName = _Params.ThirdName;
        //                std.PhoneNumber = _Params.PhoneNumber;
        //                std.Email = _Params.Email;
        //                std.DateOfBirth = _Params.DateOfBirth.Date;
        //                std.Gender = _Params.Gender; // 0 : Female , 1 : Male
        //                std.UniversityID = _Params.UniversityID;
        //                std.CollegeID = _Params.CollegeID;
        //                std.MajorID = _Params.MajorID;
        //                std.GovernorateID = _Params.GovernorateID;
        //                std.AreaID = _Params.AreaID;
        //                std.UpdatedDate = DateTime.Now;

        //                _Context.SaveChanges();

        //                _resultHandler.IsSuccessful = true;
        //                _resultHandler.MessageAr = "OK";
        //                _resultHandler.MessageEn = "OK";

        //                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //            }
        //            else
        //            {
        //                _resultHandler.IsSuccessful = false;
        //                _resultHandler.MessageAr = "تم ادخال رقم التليفون من قبل, أدخل رقم آخر";
        //                _resultHandler.MessageEn = "Phone Number is already exists, please provide another one";

        //                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //            }
        //        }
        //        else
        //        {
        //            _resultHandler.IsSuccessful = false;
        //            _resultHandler.MessageAr = "Record is not found";
        //            _resultHandler.MessageEn = "هذا الحساب غير موجود";

        //            return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _resultHandler.IsSuccessful = false;
        //        _resultHandler.MessageAr = ex.Message;
        //        _resultHandler.MessageEn = ex.Message;

        //        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //    }


        //}

        [HttpPost]
        public HttpResponseMessage VerifyAccount(string PhoneNumber, string VerificationCode)
        {
            var _resultHandler = new ResultHandler();
            try
            {
                TblStudent _Student = _Context.TblStudents.Where(a => a.PhoneNumber.Equals(PhoneNumber) && a.VerificationCode == VerificationCode && a.IsDeleted != true).SingleOrDefault();
                if (_Student != null)
                {
                    _Student.Verified = true;
                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = getStudentData(_Student.ID);
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Account is not found.";
                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }


            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpPost]
        public HttpResponseMessage ReSendVerificationCode(string PhoneNumber)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                ResultHandler res = SendCodeSMS(PhoneNumber);
                if (res.IsSuccessful)
                {
                    TblStudent _Student = _Context.TblStudents.Where(a => a.PhoneNumber.Equals(PhoneNumber) && a.IsDeleted != true).SingleOrDefault();
                    if (_Student != null)
                    {
                        //Verify student account
                        _Student.Verified = true;
                        _Student.VerificationCode = res.Result.ToString();
                        _Context.SaveChanges();

                        string TitleAr = "سمارت مايند الجامعه";
                        string TitleEn = "SmartMind University";
                        string DescriptionAr = "كود التفعيل " + res;
                        string DescriptionEn = "Your verification code is " + res;

                        Push(_Student.ID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.MessageAr = "OK";
                        _resultHandler.MessageEn = "OK";
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "Account is not found.";
                        _resultHandler.MessageEn = "هذا الحساب غير موجود";

                    }

                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = res.MessageAr;
                    _resultHandler.MessageEn = res.MessageEn;
                }

                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStudentBalance(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblStudent _StudentObj = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();
                if (_StudentObj != null)
                {
                    var Balance = _Context.TblStudents.Where(a => a.ID == StudentID).Select(a => a.Balance);

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = Balance;
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "Student account is not found";
                    _resultHandler.MessageEn = "حساب الطالب غير موجود";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                }
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }
        }

        private ResultHandler SendCodeSMS(string PhoneNumber)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                ////send sms for verification code
                Random generator = new Random();

                string Code = generator.Next(0, 99999).ToString("D5");
                string Message = "Verification Code : " + Code + " For more information call : 1881880";

                string TargetPhone = PhoneNumber.StartsWith("965") ? PhoneNumber : ("965" + PhoneNumber);
                //WebRequest request = WebRequest.Create("http://api-server3.com/api/send.aspx?username=eze-sms&password=ezesms2547&language=2&sender=SmartMind&mobile=" + TargetPhone + "&message=" + Code);
                WebRequest request = WebRequest.Create("http://api-server3.com/api/send.aspx?username=smartmind&password=smart254mind&language=1&sender=smartmind&mobile=" + TargetPhone + "&message=" + Message);

                // If required by the server, set the credentials.  
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                var status = ((HttpWebResponse)response).StatusDescription;
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                var content = responseFromServer;
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();

                _resultHandler.IsSuccessful = true;
                _resultHandler.MessageAr = "OK";
                _resultHandler.Result = Code;
            }
            catch (Exception ex)
            {
                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;
            }

            return _resultHandler;
        }

        private object getStudentData(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblStudent _Student = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();

                StudentData Data = new StudentData
                {
                    StudentID = _Student.ID,
                    UniversityID = _Student.UniversityID,
                    CollegeID = _Student.CollegeID != null ? _Student.CollegeID : 0,
                    MajorID = _Student.MajorID,
                    MajorNameAr = _Student.MajorID != null ? _Context.TblMajors.Where(a => a.ID == _Student.MajorID).FirstOrDefault().NameAr : "",
                    MajorNameEn = _Student.MajorID != null ? _Context.TblMajors.Where(a => a.ID == _Student.MajorID).FirstOrDefault().NameEn : "",
                    Name = _Student.FirstName + " " + _Student.SecondName + " " + _Student.ThirdName,
                    FirstNameAr = _Student.FirstName,
                    FirstNameEn = _Student.FirstNameEn,
                    SecondNameAr = _Student.SecondName,
                    SecondNameEn = _Student.SecondNameEn,
                    ThirdNameAr = _Student.ThirdName,
                    ThirdNameEn = _Student.ThirdNameEn,
                    Gender = _Student.Gender,
                    PhoneNumber = _Student.PhoneNumber,
                    AreaID = _Student.AreaID,
                    //GovernorateID = _Student.GovernorateID,
                    GovernorateID = _Student.GovernorateID,
                    Balance = _Student.Balance,
                    UniversityNameAr = _Student.UniversityID != null ? _Context.TblUniversities.Where(a => a.ID == _Student.UniversityID).FirstOrDefault().NameAr : "",
                    UniversityNameEn = _Student.UniversityID != null ? _Context.TblUniversities.Where(a => a.ID == _Student.UniversityID).FirstOrDefault().NameEn : "",
                    CollegeNameAr = _Student.CollegeID != null ? _Context.TblColleges.Where(a => a.ID == _Student.CollegeID).FirstOrDefault().NameAr : "",
                    CollegeNameEn = _Student.CollegeID != null ? _Context.TblColleges.Where(a => a.ID == _Student.CollegeID).FirstOrDefault().NameEn : "",
                    Email = _Student.Email,
                    GovernorateNameAr = _Student.GovernorateID != null ? _Context.TblGovernorates.Where(a => a.ID == _Student.GovernorateID).FirstOrDefault().NameAr : "",
                    GovernorateNameEn = _Student.GovernorateID != null ? _Context.TblGovernorates.Where(a => a.ID == _Student.GovernorateID).FirstOrDefault().NameEn : "",
                    AreaNameAr = _Student.AreaID != null ? _Context.TblAreas.Where(a => a.ID == _Student.AreaID).FirstOrDefault().NameAr : "",
                    AreaNameEn = _Student.AreaID != null ? _Context.TblAreas.Where(a => a.ID == _Student.AreaID).FirstOrDefault().NameEn : "",
                    BranchNameAr = _Student.BranchID != null ? _Context.TblBranches.Where(a => a.ID == _Student.BranchID).FirstOrDefault().NameAr : "",
                    BranchNameEn = _Student.BranchID != null ? _Context.TblBranches.Where(a => a.ID == _Student.BranchID).FirstOrDefault().NameEn : "",
                    UserType = _Student.TblUserCredential.UserType,
                    Verified = _Student.Verified,
                    StudentType = _Student.StudentType,
                    ProfilePic = _Student.ProfilePic.StartsWith("http://") ? _Student.ProfilePic : ("http://smuapitest.smartmindkw.com" + _Student.ProfilePic),
                    VerificationCode = _Student.VerificationCode,
                };
                try
                {
                    Data.DateOfBirth = DateTime.Parse(_Student.DateOfBirth.ToString());
                }
                catch (Exception)
                {
                }
                return Data;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private string AddDevice(int StudentID, string Token, int DeviceTypeID)
        {
            string Result = "";
            try
            {
                if (StudentID > 0 && !string.IsNullOrEmpty(Token) && DeviceTypeID > 0)
                {
                    //TblRegisterDevice obj = _Context.TblRegisterDevices.Where(a => a.Token.Equals(Token) || (a.Token.Equals(Token) && a.LecturerID == null)).FirstOrDefault();
                    TblRegisterDevice obj = _Context.TblRegisterDevices.Where(a => a.Token.Equals(Token) && a.StudentID == StudentID).FirstOrDefault();

                    if (obj != null)
                    {
                        obj.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        obj = new TblRegisterDevice();

                        obj.IsDeleted = false;
                        obj.CreatedDate = DateTime.Now;

                        _Context.TblRegisterDevices.Add(obj);
                    }

                    obj.Token = Token;
                    obj.DeviceTypeID = DeviceTypeID;
                    obj.StudentID = StudentID;

                    return "OK";
                }
                else
                {
                    Result = "Please Provide user data or notification data";

                    return Result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void Push(int StudentID, int LecturerID, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, int NotTypeID)
        {
            try
            {
                bool res = PushNotification.Push(StudentID, LecturerID, TitleAr, TitleEn, DescriptionAr, DescriptionEn, NotTypeID);
                var regNotification = new TblNotification { StudentID = StudentID, TitleAr = TitleAr, TitleEn = TitleEn, DescriptionAr = DescriptionAr, DescriptionEn = DescriptionEn, CreatedDate = DateTime.Now };

                _Context.TblNotifications.Add(regNotification);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
            }

        }

        public void SendEmail(string Email, string Name, string UserName, string Password, string VerificationCode)
        {
            try
            {
                string body1 = string.Empty;
                string body2 = string.Empty;

                //using (var sr = new StreamReader(Server.MapPath("\\App_Data\\Templates/emailPart1.html")))
                //{
                //    body1 = sr.ReadToEnd();
                //}
                //using (var sr = new StreamReader(Server.MapPath("\\App_Data\\Templates/emailPart2.html")))
                //{
                //    body2 = sr.ReadToEnd();
                //}
                string Linssss = "smartmindkw.com";
                string Linkssname = " موقعنا  ";
                //string Name = Stu.firstName + Stu.SecondName;

                string emailSubject = " مرحباً بك فى سمارت مايند الجامعه ";
                //string Message = "مرحباً بك فى اسرة سمارت مايند يمكنك الدخول اللى حسابك على موقعه من اللينك السابق بيانات الحساب : " + "\n" + "اسم السمتخدم:" + Stu.UserName + "--" + ":كلمة المرور" + Stu.Password;
                string Message = "تم التسجيل بنجاح‬ ‫شكرا لثقتكم ونعدكم بالأفضل‬‫.‬‫للشكاوى : ‬‫واتساب 66662617 " + " يمكنك الدخول إلي حسابك على الموقع والتطبيق من اللينك السابق بيانات الحساب : " + "\n" + "اسم المستخدم : " + UserName + " -- " + " كلمة المرور : " + Password + "\n" + " كود التفعيل : " + VerificationCode;
                string sender = ConfigurationManager.AppSettings["EmailFromAddress"];

                string messageBody = body1 + "<br/>" + Message + "<br/>" + Linkssname + Linssss + body2;
                var MailHelper = new MailHelper
                {
                    Sender = sender, //email.Sender,
                    Recipient = Email,
                    RecipientCC = null,
                    Subject = emailSubject,
                    Body = messageBody
                };
                MailHelper.Send();

            }
            catch (Exception)
            {
            }
        }

    }
}