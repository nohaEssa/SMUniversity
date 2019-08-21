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
    public class MajorController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage getMajors(int CollegeID)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblMajor> MajorsList = _Context.TblMajors.Where(a => a.IsDeleted != true && a.CollegeID == CollegeID).ToList();
                List<MajorData> Data = new List<MajorData>();
                foreach (var item in MajorsList)
                {
                    MajorData _data = new MajorData()
                    {
                        MajorID = item.ID,
                        NameAr = item.NameAr,
                        NameEn = item.NameEn,
                        UniversityID = item.TblCollege.UniversityID,
                        CollegeID = item.CollegeID,
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
