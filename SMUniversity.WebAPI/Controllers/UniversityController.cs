using SMUModels;
using SMUniversity.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SMUniversity.WebAPI.Controllers
{
    public class UniversityController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage getUniversities()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblUniversity> UniversitiesList = _Context.TblUniversities.Where(a => a.IsDeleted != true).ToList();
                List<UniversityData> Data = new List<UniversityData>();
                foreach (var item in UniversitiesList)
                {
                    UniversityData _data = new UniversityData()
                    {
                        ProductCategoryID = item.ID,
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
