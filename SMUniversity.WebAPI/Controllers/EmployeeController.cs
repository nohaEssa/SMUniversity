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
    public class EmployeeController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage UpdateEmployee(EmployeeObj _Params)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                TblEmployee _Employee = _Context.TblEmployees.Where(a => a.ID == _Params.EmployeeID).SingleOrDefault();
                if (_Employee == null)
                {
                    _Employee.NameAr = _Params.NameAr;
                    _Employee.NameEn = _Params.NameEn;
                    _Employee.Email = _Params.Email;
                    _Employee.PhoneNumber = _Params.PhoneNumber;
                    _Employee.BranchID = _Params.BranchID;
                    _Employee.ProfilePic = _Params.ProfilePic;

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
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
                _resultHandler.MessageAr = ex.Message;
                _resultHandler.MessageEn = ex.Message;

                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
            }


        }
    }
}
