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

    }
}
