using SMUModels;
using SMUModels.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class GovernateController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage getGovernate()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblGovernorate> GovernorateList = _Context.TblGovernorates.Where(a => a.IsDeleted != true).ToList();
                List<GovernorateData> Data = new List<GovernorateData>();
                foreach (var item in GovernorateList)
                {
                    GovernorateData _data = new GovernorateData()
                    {
                        GovernorateID = item.ID,
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
