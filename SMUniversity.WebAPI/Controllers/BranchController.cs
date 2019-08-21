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
    public class BranchController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage getBranches()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblBranch> BranchesList = _Context.TblBranches.Where(a => a.IsDeleted != true).ToList();
                List<BranchData> Data = new List<BranchData>();
                foreach (var item in BranchesList)
                {
                    BranchData _data = new BranchData()
                    {
                        BranchID = item.ID,
                        NameAr = item.NameAr,
                        NameEn = item.NameEn,
                        PhoneNumber1 = item.PhoneNumber1,
                        PhoneNumber2 = item.PhoneNumber2,
                        PhoneNumber3 = item.PhoneNumber3,
                        AddressAr = item.AddressAr,
                        AddressEn = item.AddressEn,
                        DescriptionAr = item.DescriptionAr,
                        DescriptionEn = item.DescriptionEn,
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
