using SMUModels;
using SMUModels.ObjectData;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using SMUModels.Classes;

namespace WebAPI.Controllers
{
    public class UserController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Login(LoginData _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            object Data = new object();
            try
            {
                if (_Params.UserName != null && _Params.Password != null)
                {
                    TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Params.UserName && a.Password == _Params.Password).SingleOrDefault();
                    if (_UserCred != null)
                    {
                        //string TokenExpiryDate = System.Configuration.ConfigurationManager.AppSettings["TokenExpiryDate"];
                        //string _AccessToken = System.Configuration.ConfigurationManager.AppSettings["AccessToken"];
                        //if ( string.IsNullOrEmpty(TokenExpiryDate) || DateTime.Parse(TokenExpiryDate) < DateTime.Now)
                        //{
                        //    //_AccessToken = HttpPostJson();
                        //}

                        switch (_UserCred.UserType)
                        {
                            case 1:
                                TblStudent _Student = _Context.TblStudents.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();
                                if (_Student != null)
                                {
                                    //if (_Student.Verified)
                                    //{
                                    //    Data = new StudentData
                                    //    {
                                    //        StudentID = _Student.ID,
                                    //        Name = _Student.FirstName + _Student.SecondName + _Student.ThirdName,
                                    //        Balance = _Student.Balance,
                                    //        UniversityNameAr = _Student.TblUniversity.NameAr,
                                    //        UniversityNameEn = _Student.TblUniversity.NameEn,
                                    //        CollegeNameAr = _Student.TblCollege.NameAr,
                                    //        CollegeNameEn = _Student.TblCollege.NameEn,
                                    //        DateOfBirth = _Student.DateOfBirth,
                                    //        Email = _Student.Email,
                                    //        GovernorateNameAr = _Student.TblGovernorate.NameAr,
                                    //        GovernorateNameEn = _Student.TblGovernorate.NameEn,
                                    //        AreaNameAr = _Student.TblArea.NameAr,
                                    //        AreaNameEn = _Student.TblArea.NameEn,
                                    //        BranchNameAr = _Student.TblBranch.NameAr,
                                    //        BranchNameEn = _Student.TblBranch.NameEn,
                                    //        UserType = _UserCred.UserType,
                                    //        Verified = _Student.Verified,
                                    //    };

                                    //    _resultHandler.IsSuccessful = true;
                                    //    _resultHandler.Result = Data;
                                    //    _resultHandler.MessageAr = "OK";
                                    //    _resultHandler.MessageEn = "OK";
                                    //}
                                    //else
                                    //{
                                    //    _resultHandler.IsSuccessful = false;
                                    //    _resultHandler.Result = Data;
                                    //    _resultHandler.MessageAr = "لم يتم تفعيل الحساب";
                                    //    _resultHandler.MessageEn = "Account didn't verified yet";
                                    //}

                                    Data = new StudentData
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
                                        UserType = _UserCred.UserType,
                                        Verified = _Student.Verified,
                                        ProfilePic = _Student.ProfilePic != null ? ("http://smuapi.smartmindkw.com" + _Student.ProfilePic) : "",
                                        //AccessToken = _AccessToken
                                    };

                                        string AddTokenResult = AddDevice(_Student.ID, 0, _Params.Token, _Params.DeviceTypeID);
                                        string TitleAr = "مرحبا بكم في تطبيق معهد سمارت مايند الجامعه";
                                        string TitleEn = "Welcome to Smart Mind University";
                                        string DescriptionAr = "نحرص على تقديم أفضل معايير الجودة بالتعليم بمتابعة حريصة لمستويات أبنائنا الطلاب";
                                        string DescriptionEn = "We are keen to provide the best quality standards in education by following carefully the levels of our students";

                                        Push(_Student.ID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);


                                    _resultHandler.IsSuccessful = true;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "OK";
                                    _resultHandler.MessageEn = "OK";

                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);

                                }
                                else
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "Student account is not found";
                                    _resultHandler.MessageEn = "حساب الطالب غير موجود";

                                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                                }

                                break;
                            case 2:
                                TblLecturer _Lecturer = _Context.TblLecturers.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();
                                if (_Lecturer != null)
                                {
                                    Data = new LecturerData
                                    {
                                        LecturerID = _Lecturer.ID,
                                        Name = _Lecturer.FirstNameAr + _Lecturer.SecondNameAr + _Lecturer.ThirdNameAr,
                                        NameEn = _Lecturer.FirstNameEn + _Lecturer.SecondNameEn + _Lecturer.ThirdNameEn,
                                        Address = _Lecturer.Address,
                                        Email = _Lecturer.Email,
                                        Gender = _Lecturer.Gender,
                                        PhoneNumber = _Lecturer.PhoneNumber,
                                        BranchNameAr = _Lecturer.TblBranch.NameAr,
                                        BranchNameEn = _Lecturer.TblBranch.NameEn,
                                        //ProfilePic = _Lecturer.ProfilePic,
                                        //ProfilePic = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                                        ProfilePic = _Lecturer.ProfilePic.StartsWith("http://") ? _Lecturer.ProfilePic : ("http://smuapi.smartmindkw.com" + _Lecturer.ProfilePic),
                                        UserType = _UserCred.UserType,
                                        //AccessToken = _AccessToken
                                    };

                                    string AddTokenResult = AddDevice(0, _Lecturer.ID, _Params.Token, _Params.DeviceTypeID);
                                    string TitleAr = "مرحبا بك معلم الأجيال يسعدنا تعاونك معنا";
                                    string TitleEn = "Welcome to the teacher of generations";
                                    string DescriptionAr = "نحرص على تقديم أفضل معايير الجودة بالتعليم بمتابعة حريصة لمستويات أبنائنا الطلاب";
                                    string DescriptionEn = "We are keen to provide the best quality standards in education by following carefully the levels of our students";

                                    Push(0, _Lecturer.ID, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);


                                    _resultHandler.IsSuccessful = true;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "OK";
                                    _resultHandler.MessageEn = "OK";

                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                                }
                                else
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "Lecturer account is not found";
                                    _resultHandler.MessageEn = "حساب المحاضر غير موجود";

                                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                                }

                                break;
                            case 3:
                                //TblHall _Hall = _Context.TblHalls.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();
                                TblScreen _Screen = _Context.TblScreens.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();
                                if (_Screen != null)
                                {
                                    List<HallData> _HallsData = new List<HallData>();
                                    List<TblHall> HallsList = _Context.TblScreenHalls.Where(a => a.ScreenID == _Screen.ID).Select(a => a.TblHall).ToList();
                                    foreach (var item in HallsList)
                                    {
                                        HallData data = new HallData
                                        {
                                            HallID = item.ID,
                                            HallCodeAr = item.HallCodeAr,
                                            HallCodeEn = item.HallCodeEn,
                                            Capacity = item.Capacity,
                                            BranchNameAr = item.TblBranch.NameAr,
                                            BranchNameEn = item.TblBranch.NameEn,
                                            UserType = _UserCred.UserType,
                                        };

                                        _HallsData.Add(data);
                                    }
                                    
                                    _resultHandler.IsSuccessful = true;
                                    _resultHandler.Result = _HallsData;
                                    _resultHandler.MessageAr = "OK";
                                    _resultHandler.MessageEn = "OK";

                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                                }
                                else
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "Hall account is not found";
                                    _resultHandler.MessageEn = "حساب القاعه غير موجود";

                                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                                }
                                break;
                            case 4:
                                TblEmployee _Employee = _Context.TblEmployees.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();
                                if (_Employee != null)
                                {
                                    Data = new EmployeeData
                                    {
                                        EmployeeID = _Employee.ID,
                                        NameAr = _Employee.NameAr,
                                        NameEn = _Employee.NameEn,
                                        Email = _Employee.Email,
                                        PhoneNumber = _Employee.PhoneNumber,
                                        BranchNameAr = _Employee.TblBranch.NameAr,
                                        BranchNameEn = _Employee.TblBranch.NameEn,
                                        //ProfilePic = _Employee.ProfilePic,
                                        //ProfilePic = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                                        ProfilePic = _Employee.ProfilePic.StartsWith("http://") ? _Employee.ProfilePic : ("http://smuapi.smartmindkw.com" + _Employee.ProfilePic),
                                        UserCategoryID = _Employee.UserCategoryID,
                                        UserType = _UserCred.UserType,
                                        //AccessToken = _AccessToken
                                    };

                                    string AddTokenResult = AddDevice(0, _Employee.ID, _Params.Token, _Params.DeviceTypeID);

                                    _resultHandler.IsSuccessful = true;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "OK";
                                    _resultHandler.MessageEn = "OK";

                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                                }
                                else
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.Result = Data;
                                    _resultHandler.MessageAr = "Employee account is not found";
                                    _resultHandler.MessageEn = "حساب الموظف غير موجود";

                                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                                }
                                break;


                            default:
                                break;
                        }
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
                    _resultHandler.Result = 0;
                    _resultHandler.MessageAr = "Maybe you pass the params as Query string rather than in body, please make sure that you pass the params in body";
                    _resultHandler.MessageEn = "Maybe you pass the params as Query string rather than in body, please make sure that you pass the params in body";

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
        public HttpResponseMessage RecoverPassword(UpdateEmail _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {


                _resultHandler.IsSuccessful = true;
                _resultHandler.MessageAr = "OK";
                _resultHandler.MessageEn = "OK";

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

        public static string HttpPostJson()
        {
            string url = "http://smuapi.smartmindkw.com/token";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";
            string json = "username=adminSMU7$59!@&password=9vfdjmxsmu&grant_type=password";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                // Deserializing json data to object  
                JavaScriptSerializer js = new JavaScriptSerializer();
                dynamic _TokenObj = js.Deserialize<dynamic>(result);
                string access_token = _TokenObj["access_token"];
                string token_type = _TokenObj["token_type"];
                int expires_in = _TokenObj["expires_in"];

                //int fromTime = 23;
                //TimeSpan ts4 = TimeSpan.FromHours(fromTime);

                //DateTime dt1 = new DateTime(2012, 01, 01);
                //TimeSpan ts2 = new TimeSpan(1, 0, 0, 0, 0);
                //dt1 = dt1 + ts2;

                //DateTime dt = DateTime.Now;
                //TimeSpan ts = TimeSpan.FromTicks(expires_in);
                //DateTime dtt = DateTime.Parse(expires_in.ToString());


                //dt = dt + ts;

                //save expiry date of token in web.config
                Configuration webConfig = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                webConfig.AppSettings.Settings.Remove("AccessToken");
                webConfig.AppSettings.Settings.Remove("TokenExpiryDate");
                webConfig.AppSettings.Settings.Add("AccessToken", access_token);
                webConfig.AppSettings.Settings.Add("TokenExpiryDate", DateTime.Now.AddDays(30).ToString());
                webConfig.Save();
                //System.Configuration.ConfigurationManager.AppSettings["TokenExpiryDate"] = DateTime.Now.AddDays(30).ToString();

                return access_token;
            };
        }

        //public TokenObj GetTokenObject()
        //{
        //    TokenObj _TokenObj = new TokenObj();

        //    try
        //    {
        //        string apiUrl = "http://localhost:58764/token";

        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //            HttpResponseMessage response = await client.GetAsync(apiUrl);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var data = response.Content.ReadAsStringAsync();
        //                var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //    return _TokenObj;
        //}

        private string AddDevice(int? StudentID, int? LecturerID, string Token, int DeviceTypeID)
        {
            string Result = "";
            try
            {
                if ((StudentID > 0 || LecturerID > 0) && !string.IsNullOrEmpty(Token) && DeviceTypeID > 0)
                {
                    TblRegisterDevice obj = new TblRegisterDevice();
                    if (StudentID > 0)
                    {
                        obj = _Context.TblRegisterDevices.Where(a => (a.Token.Equals(Token) && a.LecturerID == null) || (a.Token.Equals(Token) && a.StudentID == StudentID && a.LecturerID == null)).FirstOrDefault();
                        //obj = _Context.TblRegisterDevices.Where(a => a.Token.Equals(Token) && a.StudentID == StudentID).FirstOrDefault();
                    }
                    else if (LecturerID > 0)
                    {
                        obj = _Context.TblRegisterDevices.Where(a => (a.Token.Equals(Token) && a.StudentID == null) || (a.Token.Equals(Token) && a.LecturerID == LecturerID && a.StudentID == null)).FirstOrDefault();
                        //obj = _Context.TblRegisterDevices.Where(a => a.Token.Equals(Token) && a.LecturerID == LecturerID).FirstOrDefault();
                    }

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
                    obj.StudentID = StudentID > 0 ? StudentID : null;
                    obj.LecturerID = LecturerID > 0 ? LecturerID : null;

                    _Context.SaveChanges();

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

                //if (res)
                //{

                //}
                var regNotification = new TblNotification { StudentID = StudentID, TitleAr = TitleAr, TitleEn = TitleEn, DescriptionAr = DescriptionAr, DescriptionEn = DescriptionEn, CreatedDate = DateTime.Now };

                _Context.TblNotifications.Add(regNotification);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            
        }

    }
}
