using SMUModels;
using SMUModels.Classes;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class HelpQuestionController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage GetHelpQuestions()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblHelpQuestion> _ListOfHelpQuestion = _Context.TblHelpQuestions.Where(a => a.IsDeleted != true).ToList();
                List<HelpQuestion> Data = new List<HelpQuestion>();

                foreach (var ques in _ListOfHelpQuestion)
                {
                    HelpQuestion QuesData = new HelpQuestion();

                    QuesData.QuestionID = ques.ID;
                    QuesData.TitleAr = "ما هي المشكله ؟ من فضلك قم بكتابه الشكوي للعمل عليها";
                    QuesData.TitleEn = "What's wrong with you ? please provide us with your complaint to be able to help you";
                    QuesData.QuestionAr = ques.QuestionAr;
                    QuesData.QuestionEn = ques.QuestionEn;

                    Data.Add(QuesData);
                }

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
        public HttpResponseMessage SendComplaint(ComplaintObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                if(!string.IsNullOrEmpty(_Params.Complaint) && _Params.HelpQuestionID > 0)
                {
                    TblStudentComplaint _Complaint = new TblStudentComplaint()
                    {
                        StudentID = _Params.StudentID,
                        HelpQuestionID = _Params.HelpQuestionID,
                        Complaint = _Params.Complaint,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                    };

                    _Context.TblStudentComplaints.Add(_Complaint);
                    _Context.SaveChanges();

                    string TitleAr = "سمارت مايند الجامعه";
                    string TitleEn = "SmartMind University";
                    string DescriptionAr = "تم ارسال شكوتك وسيتم التواصل معك في اقرب وقت ممكن";
                    string DescriptionEn = "Your complaint is submitted and we'll back to you as soon as possible";

                    Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "تم ارسال شكوتك وسيتم التواصل معك في اقرب وقت ممكن";
                    _resultHandler.MessageEn = "Your complaint is submitted and we'll back to you as soon as possible";
                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "ادخل البيانات كامله";
                    _resultHandler.MessageEn = "Data is missing";
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

        private void Push(int StudentID, int LecturerID, string TitleAr, string TitleEn, string DescriptionAr, string DescriptionEn, int NotTypeID)
        {
            try
            {
                bool res = PushNotification.Push(StudentID, LecturerID, TitleAr, TitleEn, DescriptionAr, DescriptionEn, NotTypeID);
                var regNotification = new TblNotification { StudentID = StudentID, TitleAr = TitleAr, TitleEn = TitleEn, DescriptionAr = DescriptionAr, DescriptionEn = DescriptionEn, CreatedDate = DateTime.Now };

                _Context.TblNotifications.Add(regNotification);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
            }

        }

    }
}
