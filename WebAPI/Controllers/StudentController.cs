using SMUModels;
using SMUModels.ObjectData;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Classes;

namespace WebAPI.Controllers
{
    public class StudentController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage Register(StudentObj _Params)
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
                                PhoneNumber = _Params.PhoneNumber,
                                Email = _Params.Email,
                                Gender = _Params.Gender, // 0 : Female , 1 : Male
                                UniversityID = _Params.UniversityID,
                                CollegeID = _Params.CollegeID,
                                MajorID = _Params.MajorID,
                                GovernorateID = _Params.GovernorateID,
                                AreaID = _Params.AreaID,
                                BranchID = _Params.BranchID == 0 ? 3 : _Params.BranchID,
                                //CredentialsID = _UserCred.ID,
                                Verified = false,
                                StudentType = _Params.StudentType, //true isa normal student, false is a course student
                                CreatedDate = DateTime.Now,
                            };
                            try
                            {
                                //StudentObj.DateOfBirth = DateTime.Parse(_Params.DateOfBirth).Date;
                                StudentObj.DateOfBirth = _Params.DateOfBirth.Date;
                            }
                            catch (Exception ex)
                            {
                                _resultHandler.IsSuccessful = false;
                                _resultHandler.MessageAr = ex.Message;
                                _resultHandler.MessageEn = "Not Valid";
                                _resultHandler.Result = ex;

                                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
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

                            ResultHandler res = SendCodeSMS(StudentObj.PhoneNumber);
                            if (res.IsSuccessful)
                            {
                                StudentObj.Verified = true;
                                StudentObj.VerificationCode = res.Result.ToString();

                                _Context.SaveChanges();
                            }

                            _resultHandler.IsSuccessful = true;
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
                    std.DateOfBirth = _Params.DateOfBirth.Date;
                    std.Gender = _Params.Gender; // 0 : Female , 1 : Male
                    std.UniversityID = _Params.UniversityID;
                    std.CollegeID = _Params.CollegeID;
                    std.MajorID = _Params.MajorID;
                    std.GovernorateID = _Params.GovernorateID;
                    std.AreaID = _Params.AreaID;
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
        public HttpResponseMessage UpdateNameAndDOB(NameAndDOBObj _Params)
        {
            var _resultHandler = new ResultHandler();
            object Data = new object();

            TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
            if (_Student != null)
            {
                _Student.FirstName = _Params.FirstName;
                _Student.SecondName = _Params.SecondName;
                _Student.ThirdName = _Params.ThirdName;
                _Student.DateOfBirth = _Params.DateOfBirth.Date;
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
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    std.ProfilePic = "";
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

                string TargetPhone = PhoneNumber.StartsWith("965") ? PhoneNumber : ("965" + PhoneNumber);
                WebRequest request = WebRequest.Create("http://api-server3.com/api/send.aspx?username=eze-sms&password=ezesms2547&language=2&sender=SmartMind&mobile=" + TargetPhone + "&message=" + Code);

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
                    CollegeID = _Student.CollegeID,
                    MajorID = _Student.MajorID,
                    MajorNameAr = _Student.MajorID != null ? _Student.TblMajor.NameAr : "",
                    MajorNameEn = _Student.MajorID != null ? _Student.TblMajor.NameEn : "",
                    Name = _Student.FirstName + _Student.SecondName + _Student.ThirdName,
                    Gender = _Student.Gender,
                    PhoneNumber = _Student.PhoneNumber,
                    AreaID = _Student.AreaID,
                    GovernorateID = _Student.GovernorateID,
                    Balance = _Student.Balance,
                    UniversityNameAr = _Student.UniversityID != null ? _Student.TblUniversity.NameAr : "",
                    UniversityNameEn = _Student.UniversityID != null ? _Student.TblUniversity.NameEn : "",
                    CollegeNameAr = _Student.CollegeID != null ? _Student.TblCollege.NameAr : "",
                    CollegeNameEn = _Student.CollegeID != null ? _Student.TblCollege.NameEn : "",
                    DateOfBirth = _Student.DateOfBirth,
                    Email = _Student.Email,
                    GovernorateNameAr = _Student.TblGovernorate.NameAr,
                    GovernorateNameEn = _Student.TblGovernorate.NameEn,
                    AreaNameAr = _Student.TblArea.NameAr,
                    AreaNameEn = _Student.TblArea.NameEn,
                    BranchNameAr = _Student.TblBranch.NameAr,
                    BranchNameEn = _Student.TblBranch.NameEn,
                    UserType = _Student.TblUserCredential.UserType,
                    Verified = _Student.Verified,
                    ProfilePic = _Student.ProfilePic
                };

                return Data;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
