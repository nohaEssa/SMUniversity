using SMUModels;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class SubjectController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage getSubject(int MajorID)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                List<TblSubject> SubjectList = _Context.TblSubjects.Where(a => a.IsDeleted != true && a.MajorID == MajorID).ToList();
                List<SubjectData> Data = new List<SubjectData>();
                foreach (var item in SubjectList)
                {
                    SubjectData _data = new SubjectData()
                    {
                        SubjectID = item.ID,
                        NameAr = item.NameAr,
                        NameEn = item.NameEn,
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
