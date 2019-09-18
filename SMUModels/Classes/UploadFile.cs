using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SMUModels.Classes
{
    public class UploadFile
    {
        public static void InvokeService(string ContentFolder, string Image, string ImageName)
        {
            //Calling CreateSOAPWebRequest method    
            HttpWebRequest request = CreateSOAPWebRequest();

            XmlDocument SOAPReqBody = new XmlDocument();
            //SOAP Body Request    
            string m = "<?xml version='1.0' encoding='utf-8'?>" +
                       "<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>" +
                            "<soap:Body>" +
                                "<UploadFile xmlns='http://tempuri.org/'>" +
                                    "<ContentFolder>" + ContentFolder + "</ContentFolder>" +
                                    "<Image>" + Image + "</Image>" +
                                    "<ImageName>" + ImageName + "</ImageName>" +
                                "</UploadFile>" +
                            "</soap:Body>" +
                       "</soap:Envelope>";

            SOAPReqBody.LoadXml(m);

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }
            //Geting response from request    
            using (WebResponse Serviceres = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                {
                    //reading stream    
                    var ServiceResult = rd.ReadToEnd();
                }
            }

        }

        private static HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://diyafa.dokself.com/DiyafaService.asmx");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:http://tempuri.org/UploadFile");
            //Content_type    
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }
    }
}
