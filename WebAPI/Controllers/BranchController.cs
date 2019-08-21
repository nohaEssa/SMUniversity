using SMUModels;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SMUModels.ObjectData;

namespace WebAPI.Controllers
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
                ContactUsData Data = new ContactUsData();
                Data.BranchData = new List<BranchData>();
                List<BranchData> BranchData = new List<BranchData>();
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
                        MapLink = item.MapLink,
                        DescriptionAr = item.DescriptionAr,
                        DescriptionEn = item.DescriptionEn,
                    };

                    BranchData.Add(_data);
                }

                TblInstitute InstituteData = _Context.TblInstitutes.FirstOrDefault();
                Data.InstituteData = new InstituteData()
                {
                    Email = InstituteData.Email,
                    Website = InstituteData.Website,
                };

                Data.BranchData = BranchData;

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
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

        [HttpPost]
        public HttpResponseMessage ContactUsMsg(ContactUsMsgObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblContactUsMsg _Msg = new TblContactUsMsg()
                {
                    StudentID = _Params.StudentID,
                    Name = _Params.Name,
                    PhoneNumber = _Params.PhoneNumber,
                    Email = _Params.Email,
                    Message = _Params.Message,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                };

                _Context.TblContactUsMsgs.Add(_Msg);
                _Context.SaveChanges();

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
