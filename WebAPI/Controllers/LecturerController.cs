using SMUModels;
using SMUModels.ObjectData;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class LecturerController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage UpdateLecturer(LecturerObj _Params)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                TblLecturer teacher = _Context.TblLecturers.Where(a => a.ID == _Params.LecturerID).SingleOrDefault();
                if (teacher == null)
                {
                    teacher.FirstNameAr = _Params.FirstNameAr;
                    teacher.FirstNameEn = _Params.FirstNameEn;
                    teacher.SecondNameAr = _Params.SecondNameAr;
                    teacher.SecondNameEn = _Params.SecondNameEn;
                    teacher.ThirdNameAr = _Params.ThirdNameAr;
                    teacher.ThirdNameEn = _Params.ThirdNameEn;
                    teacher.PhoneNumber = _Params.PhoneNumber;
                    teacher.Email = _Params.Email;
                    teacher.Gender = _Params.Gender; // 0 : Female , 1 : Male
                    teacher.Address = _Params.Address;
                    teacher.BranchID = _Params.BranchID;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Record not found";
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

        [HttpGet]
        public HttpResponseMessage getLecturers(int SubjectID)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                List<TblLecturer> _LecturersList = _Context.TblLecturerSubjects.Where(a => a.SubjectID == SubjectID && a.IsDeleted != true).Select(a => a.TblLecturer).ToList();
                List<LecturerDataDDL> Data = new List<LecturerDataDDL>();
                foreach (var item in _LecturersList)
                {
                    LecturerDataDDL _data = new LecturerDataDDL()
                    {
                        LecturerID = item.ID,
                        NameAr = item.FirstNameAr + " " + item.SecondNameAr + " " + item.ThirdNameAr,
                        NameEn = item.FirstNameEn + " " + item.SecondNameEn + " " + item.ThirdNameEn,
                    };

                    Data.Add(_data);
                }

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.Count = Data.Count;
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
