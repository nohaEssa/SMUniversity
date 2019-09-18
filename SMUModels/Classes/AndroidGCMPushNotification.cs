using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.Classes
{
    /// <summary>
    /// Summary description for AndroidGCMPushNotification
    /// </summary>
    public static class AndroidGCMPushNotification
    {
        public static bool SendNotification(string Token, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            string GoogleAppID = ConfigurationManager.AppSettings["GoogleAppID"];
            var SENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"];
            bool Result = false;
            var objNotification = new
            {
                to = Token,
                data = new
                {
                    //Title = "SmartMind University",
                    TitleAr = TitleAr,
                    TitleEn = TitleEn,
                    DescriptionAr = DescriptionAr,
                    DescriptionEn = DescriptionEn
                },

            };
            string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);
            FCMResponse fcm = new FCMResponse();
            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            //tRequest.Headers.Add(string.Format("Authorization: key={0}", "AIzaSyAMR04Zd97_3fMv8mIjSF1RLgNPy04n-xU"));
            //tRequest.Headers.Add(string.Format("Sender: id={0}", "174931341037"));
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                                Result = true;
                                return Result;
                            }
                            else if (response.failure == 1)
                            {
                                Result = false;
                                return Result;
                            }

                        }
                    }

                }
            }
            return Result;
        }

        public static bool SendNotificationWithPic(string Token, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, string image)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            string GoogleAppID = ConfigurationManager.AppSettings["GoogleAppID"];
            var SENDER_ID = ConfigurationManager.AppSettings["SENDER_ID"];
            bool Result = false;
            var objNotification = new
            {
                to = Token,
                data = new
                {
                    //Title = "SmartMind University",
                    TitleAr = TitleAr,
                    TitleEn = TitleEn,
                    DescriptionAr = DescriptionAr,
                    DescriptionEn = DescriptionEn,
                    Image = image
                },
            };

            string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);
            FCMResponse fcm = new FCMResponse();
            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            //tRequest.Headers.Add(string.Format("Authorization: key={0}", "AIzaSyAMR04Zd97_3fMv8mIjSF1RLgNPy04n-xU"));
            //tRequest.Headers.Add(string.Format("Sender: id={0}", "174931341037"));
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                            if (response.success == 1)
                            {
                                Result = true;
                                return Result;
                            }
                            else if (response.failure == 1)
                            {
                                Result = false;
                                return Result;
                            }

                        }
                    }

                }
            }
            return Result;
        }

    }

}

public class FCMResponse
{
    public long multicast_id { get; set; }
    public int success { get; set; }
    public int failure { get; set; }
    public int canonical_ids { get; set; }
}