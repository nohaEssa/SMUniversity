//using SMUModels;
//using SMUModels.ObjectData;
//using SMUniversity.WebAPI.Classes;
//using System;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace SMUniversity.WebAPI.Controllers
//{
//    public class StudentController : ApiController
//    {
//        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

//        [HttpPost]
//        public HttpResponseMessage Register(StudentObj _Params)
//        {
//            var _resultHandler = new ResultHandler();

//            try
//            {
//                TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Params.UserName && a.UserType == 1).SingleOrDefault();
//                if (_UserCred == null)
//                {
//                    TblStudent StudentObj = new TblStudent()
//                    {
//                        FirstName = _Params.FirstName,
//                        SecondName = _Params.SecondName,
//                        ThirdName = _Params.ThirdName,
//                        PhoneNumber = _Params.PhoneNumber,
//                        Email = _Params.Email,
//                        //DateOfBirth = _Params.DateOfBirth.Date,
//                        Gender = _Params.Gender, // 0 : Female , 1 : Male
//                        UniversityID = _Params.UniversityID,
//                        CollegeID = _Params.CollegeID,
//                        MajorID = _Params.MajorID,
//                        GovernorateID = _Params.GovernorateID,
//                        AreaID = _Params.AreaID,
//                        BranchID = _Params.BranchID,
//                        //CredentialsID = _UserCred.ID,
//                        Verified = false,
//                        CreatedDate = DateTime.Now,
//                    };

//                    _Context.TblStudents.Add(StudentObj);
//                    _Context.SaveChanges();

//                    _UserCred = new TblUserCredential()
//                    {
//                        UserName = _Params.UserName,
//                        Password = _Params.Password,
//                        UserType = 1,
//                    };

//                    _Context.TblUserCredentials.Add(_UserCred);
//                    _Context.SaveChanges();

//                    StudentObj.CredentialsID = _UserCred.ID;
//                    _Context.SaveChanges();

//                    ResultHandler res = SendCodeSMS(StudentObj.PhoneNumber);
//                    if (res.IsSuccessful)
//                    {
//                        StudentObj.Verified = true;
//                        StudentObj.VerificationCode = res.Result.ToString();

//                        _Context.SaveChanges();
//                    }

//                    _resultHandler.IsSuccessful = true;
//                    _resultHandler.MessageAr = "OK";
//                    _resultHandler.MessageEn = "OK";

//                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = "Username is already used, please enter another one";
//                    _resultHandler.MessageEn = "اسم المستخدم موجود بالفعل, من فضلك ادخل واحد آخر";

//                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//                }
//            }
//            catch (Exception ex)
//            {

//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }


//        }

//        [HttpPost]
//        public HttpResponseMessage UpdateStudent(StudentObj _Params)
//        {
//            var _resultHandler = new ResultHandler();

//            try
//            {
//                TblStudent std = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
//                if (std != null)
//                {
//                    std.FirstName = _Params.FirstName;
//                    std.SecondName = _Params.SecondName;
//                    std.ThirdName = _Params.ThirdName;
//                    std.PhoneNumber = _Params.PhoneNumber;
//                    std.Email = _Params.Email;
//                    //std.DateOfBirth = _Params.DateOfBirth.Date;
//                    std.Gender = _Params.Gender; // 0 : Female , 1 : Male
//                    std.UniversityID = _Params.UniversityID;
//                    std.CollegeID = _Params.CollegeID;
//                    std.MajorID = _Params.MajorID;
//                    std.GovernorateID = _Params.GovernorateID;
//                    std.AreaID = _Params.AreaID;
//                    std.UpdatedDate = DateTime.Now;

//                    _Context.SaveChanges();

//                    _resultHandler.IsSuccessful = true;
//                    _resultHandler.MessageAr = "OK";
//                    _resultHandler.MessageEn = "OK";

//                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = "Record is not found";
//                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

//                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
//                }
//            }
//            catch (Exception ex)
//            {

//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }


//        }

//        [HttpPost]
//        public HttpResponseMessage VerifyAccount(string PhoneNumber, string VerificationCode)
//        {
//            var _resultHandler = new ResultHandler();
//            try
//            {
//                TblStudent _Student = _Context.TblStudents.Where(a => a.PhoneNumber.Equals(PhoneNumber) && a.VerificationCode == VerificationCode && a.IsDeleted != true).SingleOrDefault();
//                if (_Student != null)
//                {
//                    _Student.Verified = true;
//                    _Context.SaveChanges();

//                    _resultHandler.IsSuccessful = true;
//                    _resultHandler.MessageAr = "OK";
//                    _resultHandler.MessageEn = "OK";

//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = "Account is not found.";
//                    _resultHandler.MessageEn = "هذا الحساب غير موجود";

//                }


//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpPost]
//        public HttpResponseMessage ReSendVerificationCode(string PhoneNumber)
//        {
//            ResultHandler _resultHandler = new ResultHandler();
//            try
//            {
//                ResultHandler res = SendCodeSMS(PhoneNumber);
//                if (res.IsSuccessful)
//                {
//                    TblStudent _Student = _Context.TblStudents.Where(a => a.PhoneNumber.Equals(PhoneNumber) && a.IsDeleted != true).SingleOrDefault();
//                    if (_Student != null)
//                    {
//                        //Verify student account
//                        _Student.Verified = true;
//                        _Student.VerificationCode = res.Result.ToString();
//                        _Context.SaveChanges();

//                        _resultHandler.IsSuccessful = true;
//                        _resultHandler.MessageAr = "OK";
//                        _resultHandler.MessageEn = "OK";
//                    }
//                    else
//                    {
//                        _resultHandler.IsSuccessful = false;
//                        _resultHandler.MessageAr = "Account is not found.";
//                        _resultHandler.MessageEn = "هذا الحساب غير موجود";

//                    }

//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = res.MessageAr;
//                    _resultHandler.MessageEn = res.MessageEn;
//                }

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpGet]
//        public HttpResponseMessage GetStudentBalance(int StudentID)
//        {
//            ResultHandler _resultHandler = new ResultHandler();
//            try
//            {
//                TblStudent _StudentObj = _Context.TblStudents.Where(a => a.ID == StudentID).SingleOrDefault();
//                if (_StudentObj != null)
//                {
//                    var Balance = _Context.TblStudents.Where(a => a.ID == StudentID).Select(a => a.Balance);

//                    _resultHandler.IsSuccessful = true;
//                    _resultHandler.Result = Balance;
//                    _resultHandler.MessageAr = "OK";
//                    _resultHandler.MessageEn = "OK";

//                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = true;
//                    _resultHandler.MessageAr = "Student account is not found";
//                    _resultHandler.MessageEn = "حساب الطالب غير موجود";

//                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
//                }
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        private ResultHandler SendCodeSMS(string PhoneNumber)
//        {
//            ResultHandler _resultHandler = new ResultHandler();

//            try
//            {
//                //send sms for verification code
//                Random generator = new Random();
//                string Code = generator.Next(0, 99999).ToString("D5");

//                string TargetPhone = PhoneNumber;
//                WebRequest request = WebRequest.Create("http://api-server3.com/api/send.aspx?username=eze-sms&password=ezesms2547&language=2&sender=TEST&mobile=" + TargetPhone + "&message=" + Code);

//                // If required by the server, set the credentials.  
//                request.Credentials = CredentialCache.DefaultCredentials;
//                // Get the response.  
//                WebResponse response = request.GetResponse();
//                // Display the status.  
//                var status = ((HttpWebResponse)response).StatusDescription;
//                // Get the stream containing content returned by the server.  
//                Stream dataStream = response.GetResponseStream();
//                // Open the stream using a StreamReader for easy access.  
//                StreamReader reader = new StreamReader(dataStream);
//                // Read the content.  
//                string responseFromServer = reader.ReadToEnd();
//                // Display the content.  
//                var content = responseFromServer;
//                // Clean up the streams and the response.  
//                reader.Close();
//                response.Close();

//                _resultHandler.IsSuccessful = true;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.Result = Code;
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;
//            }

//            return _resultHandler;
//        }
//    }
//}
