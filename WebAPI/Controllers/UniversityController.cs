using SMUModels;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Script.Serialization;
using SMUModels.ObjectData;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{
    public class UniversityController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [AllowAnonymous]
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

        public static string HttpPostJson()
        {
            string TokenExpiryDate = System.Configuration.ConfigurationManager.AppSettings["TokenExpiryDate"];

            string url = "http://localhost:56214/token";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";
            string json = "username=adminSMU7$59!@&password=9vfdjmxsmu&grant_type=password";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                // Deserializing json data to object  
                JavaScriptSerializer js = new JavaScriptSerializer();
                dynamic _TokenObj = js.Deserialize<dynamic>(result);
                string access_token = _TokenObj["access_token"];
                string token_type = _TokenObj["token_type"];
                int expires_in = _TokenObj["expires_in"];

                //int fromTime = 23;
                //TimeSpan ts4 = TimeSpan.FromHours(fromTime);

                //DateTime dt1 = new DateTime(2012, 01, 01);
                //TimeSpan ts2 = new TimeSpan(1, 0, 0, 0, 0);
                //dt1 = dt1 + ts2;

                //DateTime dt = DateTime.Now;
                //TimeSpan ts = TimeSpan.FromTicks(expires_in);
                //DateTime dtt = DateTime.Parse(expires_in.ToString());


                //dt = dt + ts;

                //save expiry date of token in web.config
                System.Configuration.ConfigurationManager.AppSettings["AccessToken"] = access_token;
                System.Configuration.ConfigurationManager.AppSettings["TokenExpiryDate"] = DateTime.Now.AddDays(30).ToString();

                return access_token;
            };
        }

    }
}
