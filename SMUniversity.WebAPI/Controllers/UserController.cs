using SMUModels;
using SMUModels.ObjectData;
using SMUniversity.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SMUniversity.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage Login(LoginData _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            Object Data = new object();
            try
            {
                TblUserCredential _UserCred = _Context.TblUserCredentials.Where(a => a.UserName == _Params.UserName && a.Password == _Params.Password).SingleOrDefault();
                if (_UserCred != null)
                {
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
                                    Name = _Student.FirstName + _Student.SecondName + _Student.ThirdName,
                                    Balance = _Student.Balance,
                                    UniversityNameAr = _Student.TblUniversity.NameAr,
                                    UniversityNameEn = _Student.TblUniversity.NameEn,
                                    CollegeNameAr = _Student.TblCollege.NameAr,
                                    CollegeNameEn = _Student.TblCollege.NameEn,
                                    Email = _Student.Email,
                                    GovernorateNameAr = _Student.TblGovernorate.NameAr,
                                    GovernorateNameEn = _Student.TblGovernorate.NameEn,
                                    AreaNameAr = _Student.TblArea.NameAr,
                                    AreaNameEn = _Student.TblArea.NameEn,
                                    BranchNameAr = _Student.TblBranch.NameAr,
                                    BranchNameEn = _Student.TblBranch.NameEn,
                                    UserType = _UserCred.UserType,
                                    Verified = _Student.Verified,
                                };

                                //try
                                //{
                                //    Data. = DateTime.Parse(_Student.DateOfBirth.ToString());
                                //}
                                //catch (Exception)
                                //{

                                //    throw;
                                //}

                                _resultHandler.IsSuccessful = true;
                                _resultHandler.Result = Data;
                                _resultHandler.MessageAr = "OK";
                                _resultHandler.MessageEn = "OK";

                            }
                            else
                            {
                                _resultHandler.IsSuccessful = true;
                                _resultHandler.Result = Data;
                                _resultHandler.MessageAr = "Student account is not found";
                                _resultHandler.MessageEn = "حساب الطالب غير موجود";
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
                                    Address = _Lecturer.Address,
                                    Email = _Lecturer.Email,
                                    Gender = _Lecturer.Gender,
                                    PhoneNumber = _Lecturer.PhoneNumber,
                                    BranchNameAr = _Lecturer.TblBranch.NameAr,
                                    BranchNameEn = _Lecturer.TblBranch.NameEn,
                                    ProfilePic = _Lecturer.ProfilePic,
                                    UserType = _UserCred.UserType,
                                };

                                _resultHandler.IsSuccessful = true;
                                _resultHandler.Result = Data;
                                _resultHandler.MessageAr = "OK";
                                _resultHandler.MessageEn = "OK";
                            }
                            else
                            {
                                _resultHandler.IsSuccessful = true;
                                _resultHandler.Result = Data;
                                _resultHandler.MessageAr = "Lecturer account is not found";
                                _resultHandler.MessageEn = "حساب المحاضر غير موجود";
                            }

                            break;
                        //case 3:
                        //    TblHall _Hall = _Context.TblHalls.Where(a => a.CredentialsID == _UserCred.ID).SingleOrDefault();
                        //    if (_Hall != null)
                        //    {
                        //        Data = new HallData
                        //        {
                        //            HallID = _Hall.ID,
                        //            HallCode = _Hall.HallCode,
                        //            Capacity = _Hall.Capacity,
                        //            BranchNameAr = _Hall.TblBranch.NameAr,
                        //            BranchNameEn = _Hall.TblBranch.NameEn,
                        //            UserType = _UserCred.UserType,
                        //        };

                        //        _resultHandler.IsSuccessful = true;
                        //        _resultHandler.Result = Data;
                        //        _resultHandler.MessageAr = "OK";
                        //        _resultHandler.MessageEn = "OK";
                        //    }
                        //    else
                        //    {
                        //        _resultHandler.IsSuccessful = true;
                        //        _resultHandler.Result = Data;
                        //        _resultHandler.MessageAr = "Hall account is not found";
                        //        _resultHandler.MessageEn = "حساب القاعه غير موجود";
                        //    }
                        //    break;
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
                                    ProfilePic = _Employee.ProfilePic,
                                    UserCategoryID = _Employee.UserCategoryID,
                                    UserType = _UserCred.UserType,
                                };

                                _resultHandler.IsSuccessful = true;
                                _resultHandler.Result = Data;
                                _resultHandler.MessageAr = "OK";
                                _resultHandler.MessageEn = "OK";
                            }
                            else
                            {
                                _resultHandler.IsSuccessful = true;
                                _resultHandler.Result = Data;
                                _resultHandler.MessageAr = "Employee account is not found";
                                _resultHandler.MessageEn = "حساب الموظف غير موجود";
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
            catch (Exception ex)
            {

                _resultHandler.IsSuccessful = false;
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }


        }

        [HttpPost]
        public HttpResponseMessage RecoverPassword(UserObj _Params)
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
    }
}
