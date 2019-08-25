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
    public class NotificationController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage GetNotifications(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblNotification> _Notifications = _Context.TblNotifications.Where(a => a.IsDeleted != true && a.StudentID == StudentID).ToList();
                List<NotificationData> Data = new List<NotificationData>();

                foreach (var item in _Notifications)
                {
                    NotificationData data = new NotificationData()
                    {
                        TitleAr = item.TitleAr,
                        TitleEn = item.TitleEn,
                        DescriptionAr = item.DescriptionAr,
                        DescriptionEn = item.DescriptionEn,
                        Picture = item.Picture,
                        Time = item.CreatedDate.ToString("yyyy-MM-dd hh:mm tt"),
                        Type = item.Type
                    };

                    Data.Add(data);
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

        [HttpPost]
        public HttpResponseMessage SaveToken(SaveTokenData _SaveTokenData)
        {
            var _resultHandler = new ResultHandler();
            TblRegisterDevice token;
            try
            {
                if (!string.IsNullOrEmpty(_SaveTokenData.Token))
                {
                    token = _Context.TblRegisterDevices.Where(a => a.Token.Equals(_SaveTokenData.Token)).SingleOrDefault();
                    if (token == null)
                    {
                        if (_SaveTokenData.Token.Length <= 74)
                        {
                            var newToken = new TblRegisterDevice { Token = _SaveTokenData.Token, DeviceTypeID = 2, IsDeleted = false, CreatedDate = DateTime.Now };
                            _Context.TblRegisterDevices.Add(newToken);//android
                        }
                        else if (_SaveTokenData.Token.Length > 64)
                        {
                            var newToken = new TblRegisterDevice { Token = _SaveTokenData.Token, DeviceTypeID = 1, IsDeleted = false, CreatedDate = DateTime.Now };
                            _Context.TblRegisterDevices.Add(newToken);//iphone
                        }
                    }
                    else
                    {
                        token.UpdatedDate = DateTime.Now;
                    }

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = "Done";
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageAr = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {

                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "Token is empty or null";
                    _resultHandler.MessageEn = "Token is empty or null";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
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
