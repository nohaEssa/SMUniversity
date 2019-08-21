using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Classes;

namespace WebAPI.Controllers
{
    public class InstitueController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage getInistituteData()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblInstitute InstituteData = _Context.TblInstitutes.FirstOrDefault();

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = InstituteData;
                _resultHandler.Count = 1;
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
