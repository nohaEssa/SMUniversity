using SMUModels;
using SMUModels.ObjectData;
using WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace WebAPI.Controllers
{
    public class SessionController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpPost]
        public HttpResponseMessage EditSessionDescription(SessionDescriptionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblSession _Session = _Context.TblSessions.Where(a => a.ID == _Params.SessionID && a.IsDeleted != true).SingleOrDefault();
                if (_Session != null)
                {
                    _Session.Description = _Params.Description;
                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "هذه المحاضره غير موجوده";
                    _resultHandler.MessageEn = "Session is not found";

                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
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

        [HttpGet]
        public HttpResponseMessage GetQRCodeData(string QRCodeProps)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                if (!string.IsNullOrEmpty(QRCodeProps))
                {
                    string[] SplittedResutl = QRCodeProps.Split(',');
                    if (SplittedResutl.Length > 0)
                    {
                        SessionData Data=null;
                        if (!string.IsNullOrEmpty(SplittedResutl[0]) && !string.IsNullOrEmpty(SplittedResutl[1]))
                        {
                            int SessionPeriodID = int.Parse(SplittedResutl[0]);
                            int SessionType = int.Parse(SplittedResutl[1]);

                            TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.ID == SessionPeriodID).FirstOrDefault();
                            int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionDetails.SessionID).Count();

                            Data = new SessionData()
                            {
                                SessionID = _SessionDetails.SessionID,
                                Type = _SessionDetails.TblSession.Type,
                                SubjectNameAr = _SessionDetails.TblSession.TblSubject.NameAr,
                                SubjectNameEn = _SessionDetails.TblSession.TblSubject.NameEn,
                                Lecturer = _SessionDetails.TblSession.TblLecturer.FirstNameAr + " " + _SessionDetails.TblSession.TblLecturer.SecondNameAr + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameAr,
                                LecturerNameEn = _SessionDetails.TblSession.TblLecturer.FirstNameEn + " " + _SessionDetails.TblSession.TblLecturer.SecondNameEn + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameEn,
                                //SubjectPicture = _SessionDetails.TblSession.TblSubject.Picture,
                                //Cost = decimal.Parse(_SessionDetails.Cost.ToString())
                            };
                            if (_SessionDetails.TblSession.TblSubject.Picture != null)
                            {
                                Data.SubjectPicture = _SessionDetails.TblSession.TblSubject.Picture.StartsWith("http://") ? _SessionDetails.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.TblSubject.Picture);
                            }
                            else
                            {
                                Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.TblSubject.Picture;
                            }
                            switch (subCount)
                            {
                                case 0:
                                    Data.Cost = (decimal)_SessionDetails.Price1;
                                    break;
                                case 1:
                                    //Data.Cost = (decimal)_SessionDetails.TblSession.Price2;
                                    Data.Cost = (decimal)_SessionDetails.Price2;
                                    break;
                                case 2:
                                    //Data.Cost = (decimal)_SessionDetails.TblSession.Price3;
                                    Data.Cost = (decimal)_SessionDetails.Price3;
                                    break;
                                default:
                                    //Data.Cost = (decimal)_SessionDetails.TblSession.Price3;
                                    Data.Cost = (decimal)_SessionDetails.Price3;
                                    break;
                            }
                        }

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.Result = Data;
                        _resultHandler.MessageAr = "OK";
                        _resultHandler.MessageEn = "OK";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "رقم المحاضره غير صحيح";
                    _resultHandler.MessageEn = "Session ID is empty";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }
                
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

        [HttpGet]
        public HttpResponseMessage GetQRCodeData2(string QRCodeProps)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                if (!string.IsNullOrEmpty(QRCodeProps))
                {
                    string[] SplittedResutl = QRCodeProps.Split(',');
                    if (SplittedResutl.Length > 0)
                    {
                        SessionData Data = null;
                        if (!string.IsNullOrEmpty(SplittedResutl[0]) && !string.IsNullOrEmpty(SplittedResutl[1]))
                        {
                            int SessionPeriodID = int.Parse(SplittedResutl[0]);
                            int SessionType = int.Parse(SplittedResutl[1]);

                            TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.ID == SessionPeriodID).FirstOrDefault();
                            TblProduct _ProductuObj = _Context.TblProducts.Where(a => a.ID == SessionPeriodID && a.IsDeleted != true).FirstOrDefault();
                            int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionDetails.SessionID).Count();

                            Data = new SessionData()
                            {
                                SessionID = _SessionDetails != null ? _SessionDetails.SessionID : _ProductuObj.ID,
                                Type = _SessionDetails != null ? _SessionDetails.TblSession.Type : false,
                                TitleAr = _SessionDetails != null ? _SessionDetails.TblSession.Name : _ProductuObj.NameAr,
                                TitleEn = _SessionDetails != null ? _SessionDetails.TblSession.NameEn : _ProductuObj.NameEn,
                                Lecturer = _SessionDetails != null ? _SessionDetails.TblSession.TblLecturer.FirstNameAr + " " + _SessionDetails.TblSession.TblLecturer.SecondNameAr + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameAr : "",
                                LecturerNameEn = _SessionDetails != null ? _SessionDetails.TblSession.TblLecturer.FirstNameEn + " " + _SessionDetails.TblSession.TblLecturer.SecondNameEn + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameEn : "",
                                //SubjectPicture = _SessionPeriod.TblSession.TblSubject.Picture,
                                //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                                ProductCostMsgAr = _SessionDetails != null ? "سعر المحاضرة" : ("سعر " + _ProductuObj.NameAr),
                                ProductCostMsgEn = _SessionDetails != null ? "Session Cost" : (_ProductuObj.NameEn + " Cost"),
                                //Cost = _SessionDetails != null ? decimal.Parse(_SessionDetails.Cost.ToString()): decimal.Parse(_ProductuObj.Cost.ToString())
                            };

                            if (_SessionDetails.TblSession.TblSubject.Picture != null)
                            {
                                Data.SubjectPicture = _SessionDetails.TblSession.TblSubject.Picture.StartsWith("http://") ? _SessionDetails.TblSession.TblSubject.Picture : ("http://smuapi.smartmindkw.com" + _SessionDetails.TblSession.TblSubject.Picture);
                            }
                            else
                            {
                                Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.TblSubject.Picture;
                            }
                            if (_SessionDetails != null)
                            {
                                switch (subCount)
                                {
                                    case 0:
                                        Data.Cost = (decimal)_SessionDetails.Price1;
                                        break;
                                    case 1:
                                        //Data.Cost = (decimal)_SessionDetails.TblSession.Price2;
                                        Data.Cost = (decimal)_SessionDetails.Price2;
                                        break;
                                    case 2:
                                        //Data.Cost = (decimal)_SessionDetails.TblSession.Price3;
                                        Data.Cost = (decimal)_SessionDetails.Price3;
                                        break;
                                    default:
                                        //Data.Cost = (decimal)_SessionDetails.TblSession.Price3;
                                        Data.Cost = (decimal)_SessionDetails.Price3;
                                        break;
                                }
                            }
                            else
                            {
                                Data.Cost = decimal.Parse(_ProductuObj.Cost.ToString());
                            }

                        }

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.Result = Data;
                        _resultHandler.MessageAr = "OK";
                        _resultHandler.MessageEn = "OK";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "رقم المحاضره غير صحيح";
                    _resultHandler.MessageEn = "Session ID is empty";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }

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

        [HttpPost]
        public HttpResponseMessage SubscripetoSession(SubscriptionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                if (_Params.SessionID > 0 && _Params.StudentID > 0)
                {
                    TblSubscription sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
                    if (sub == null)
                    {
                        TblSessionDetail _Session = _Context.TblSessionDetails.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                        TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();

                        if (_Student != null)
                        {
                            //if (subCount == 0)
                            //{
                            //    _Sub.PriceType = 1;
                            //    _Sub.Price = (decimal)_Session.Price1;
                            //}
                            //else if (subCount == 1)
                            //{
                            //    _Sub.PriceType = 2;
                            //    _Sub.Price = (decimal)_Session.Price2;

                            //    firstSub.Price -= (decimal)_Session.Price1 - (decimal)_Session.Price2;
                            //}
                            //else if (subCount == 2)
                            //{
                            //    _Sub.PriceType = 3;
                            //    _Sub.Price = (decimal)_Session.Price3;

                            //    firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3)/2;
                            //    secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3)/2;
                            //}
                            //else
                            //{
                            //    _Sub.PriceType = 3;
                            //    _Sub.Price = (decimal)_Session.Price3;
                            //}

                            TblSubscription _Sub = new TblSubscription()
                            {
                                SessionID = _Params.SessionID,
                                StudentID = _Params.StudentID,
                                IsDeleted = false,
                                CreatedDate = DateTime.Now
                            };
                            _Context.TblSubscriptions.Add(_Sub);

                            if (!_Params.Pending)
                            {
                                if (_Params.Pending == false && _Student.Balance >= _Session.Cost)
                                {
                                    int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).Count();
                                    TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                                    TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();

                                    switch (subCount)
                                    {
                                        case 0:
                                            _Sub.PriceType = 1;
                                            _Sub.Price = (decimal)_Session.Price1;
                                            break;
                                        case 1:
                                            _Sub.PriceType = 2;
                                            _Sub.Price = (decimal)_Session.Price2;

                                            firstSub.Price -= (decimal)_Session.Price1 - (decimal)_Session.Price2;

                                            TblStudent FirstStudent = _Context.TblStudents.Where(a => a.ID == firstSub.StudentID).SingleOrDefault();
                                            FirstStudent.Balance += (decimal)_Session.Price1 - (decimal)_Session.Price2;

                                            TblBalanceTransaction BalanceTrans = new TblBalanceTransaction()
                                            {
                                                StudentID = FirstStudent.ID,
                                                Price = (decimal)_Session.Price1 - (decimal)_Session.Price2,
                                                Pending = false,
                                                IsDeleted = false,
                                                PaymentMethod = "Cash",
                                                TransactionTypeID = 1,
                                                TitleAr = "استرداد رصيد",
                                                TitleEn = "Recover money",
                                                CreatedDate = DateTime.Now,
                                            };

                                            _Context.TblBalanceTransactions.Add(BalanceTrans);
                                            _Context.SaveChanges();

                                            break;
                                        case 2:
                                            _Sub.PriceType = 3;
                                            _Sub.Price = (decimal)_Session.Price3;

                                            firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
                                            secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;

                                            TblStudent _FirstStudent = _Context.TblStudents.Where(a => a.ID == firstSub.StudentID).SingleOrDefault();
                                            _FirstStudent.Balance += (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2);

                                            TblStudent SecondStudent = _Context.TblStudents.Where(a => a.ID == secondSub.StudentID).SingleOrDefault();
                                            SecondStudent.Balance += (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2);

                                            TblBalanceTransaction BalanceTrans2 = new TblBalanceTransaction()
                                            {
                                                StudentID = _FirstStudent.ID,
                                                Price = (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2),
                                                Pending = false,
                                                IsDeleted = false,
                                                PaymentMethod = "Cash",
                                                TransactionTypeID = 1,
                                                TitleAr = "استرداد رصيد",
                                                TitleEn = "Recover money",
                                                CreatedDate = DateTime.Now,
                                            };

                                            _Context.TblBalanceTransactions.Add(BalanceTrans2);
                                            _Context.SaveChanges();
                                            break;
                                        default:
                                            _Sub.PriceType = 3;
                                            _Sub.Price = (decimal)_Session.Price3;

                                            //firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
                                            //secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
                                            break;
                                    }
                                    _Sub.Pending = false;
                                    _Student.Balance -= _Session.Cost;

                                    TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
                                    {
                                        StudentID = _Params.StudentID,
                                        Price = _Session.Cost,
                                        Pending = false,
                                        IsDeleted = false,
                                        PaymentMethod = "Cash",
                                        TransactionTypeID = 2,
                                        TitleAr = "اشتراك في محاضرة : " + _Session.TblSession.TblSubject.NameAr,
                                        TitleEn = "Subscription to subject : " + _Session.TblSession.TblSubject.NameEn,
                                        CreatedDate = DateTime.Now,
                                    };

                                    _Context.TblBalanceTransactions.Add(_BalanceTrans);
                                    _Context.SaveChanges();

                                    _resultHandler.IsSuccessful = true;
                                    _resultHandler.MessageAr = "تم الإشتراك في محاضره " + _Session.TblSession.TblSubject.NameAr + " وخصم مبلغ " + _Sub.Price + " من رصيدك";
                                    _resultHandler.MessageEn = "You have been subscribed to " + _Session.TblSession.TblSubject.NameEn + ", and a balance of " + _Sub.Price + " has been deducted";

                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                                }
                                else
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.MessageAr = "رصيدك غير كافي للاشتراك في المحاضره, اشحن ثم حاول مره اخري";
                                    _resultHandler.MessageEn = "You have not enough balance to subscripe to lecturer, Please recharge and try again";

                                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                                }
                            }
                            else
                            {
                                _Sub.Pending = true;
                                _Context.SaveChanges();
                            }
                        }
                        else
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "رقم الطالب غير صحيح او غير موجود";
                            _resultHandler.MessageEn = "Please provide a valid Student ID";

                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                        }
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "تم الإشتراك في هذه المحاضره من قبل";
                        _resultHandler.MessageEn = "This session hass been subscribed to before";

                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                    }                    
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "أدخل رقم الكيو ار كود";
                    _resultHandler.MessageEn = "Please provide QRCode ID";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }

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

        [HttpGet]
        public HttpResponseMessage GetPreviousSubscriptions(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.Ended == true && a.IsDeleted != true).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblEvaluation StudentEval = _Context.TblEvaluations.Where(a => a.StudentID == StudentID && a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        SessionCost = _SessionDetails.Cost,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic,
                        CollegeNameAr = item.TblSession.TblSubject.TblMajor.TblCollege.NameAr,
                        CollegeNameEn = item.TblSession.TblSubject.TblMajor.TblCollege.NameEn,
                        MajorNameAr = item.TblSession.TblSubject.TblMajor.NameAr,
                        MajorNameEn = item.TblSession.TblSubject.TblMajor.NameEn,
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        LecturesCount = item.TblSession.LecturesCount,
                        //StartDate = _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        StartDate = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),                    
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                        Evaluated = StudentEval == null ? true : false,
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                    };

                    if (item.TblSession.Picture != null)
                    {
                        data.SubjectPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                        data.SessionPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                    }

                    Data.Add(data);
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

        [HttpGet]
        public HttpResponseMessage GetCurrentSubscriptions(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    //List<TblAttendance> _Attend = _Context.TblAttendances.Where(a => a.SessionID == item.SessionID && a.StudentID == item.StudentID && a.IsDeleted != true).ToList();
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        SessionCost = _SessionDetails.Cost,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic,
                        CollegeNameAr = item.TblSession.TblSubject.TblMajor.TblCollege.NameAr,
                        CollegeNameEn = item.TblSession.TblSubject.TblMajor.TblCollege.NameEn,
                        MajorNameAr = item.TblSession.TblSubject.TblMajor.NameAr,
                        MajorNameEn = item.TblSession.TblSubject.TblMajor.NameEn,
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        LecturesCount = item.TblSession.LecturesCount,
                        //LecturesLeft = item.TblSession.LecturesCount - _Attend.Where(a => a.Attend != 1 || a.Attend != 2).Count(),
                        LecturesLeft = item.TblSession.LecturesCount,
                        StartDate = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        //StartDate = _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                    };

                    if (item.TblSession.Picture != null)
                    {
                        data.SubjectPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                        data.SessionPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                    }

                    Data.Add(data);
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

        [HttpGet]
        public HttpResponseMessage GetUpComingSubscriptions(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.Pending == true && a.IsDeleted != true).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        SessionCost = _SessionDetails.Cost,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic,
                        CollegeNameAr = item.TblSession.TblSubject.TblMajor.TblCollege.NameAr,
                        CollegeNameEn = item.TblSession.TblSubject.TblMajor.TblCollege.NameEn,
                        MajorNameAr = item.TblSession.TblSubject.TblMajor.NameAr,
                        MajorNameEn = item.TblSession.TblSubject.TblMajor.NameEn,
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        LecturesCount = item.TblSession.LecturesCount,
                        //StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                        StartDate = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                    };

                    if (item.TblSession.Picture != null)
                    {
                        data.SubjectPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                        data.SessionPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                    }

                    Data.Add(data);
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

        [HttpGet]
        public HttpResponseMessage GetPendedSubsCount(int SessionID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID && a.Pending == true && a.IsDeleted != true).ToList();
                List<string> Data = new List<string>();

                foreach (var item in _Subs.Select(a => a.TblStudent))
                {
                    Data.Add(item.ProfilePic);
                }

                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Data;
                _resultHandler.Count = _Subs.Count;
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

        [HttpGet]
        public HttpResponseMessage GetTimelineSessions(int CollegeID, int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;
                List<TblSession> _Sessions = _Context.TblSessions.Where(a => a.TblSubject.TblMajor.CollegeID == CollegeID && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true).ToList();
                //List<TblSession> _Sessions = _Context.TblSessions.Where(a => a.TblSubject.TblMajor.CollegeID == CollegeID && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true && DbFunctions.TruncateTime(a.FromDate) <= CurrentDate ).ToList();
                List<TimelineSessionData> Data = new List<TimelineSessionData>();

                foreach (var item in _Sessions)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    TimelineSessionData data = new TimelineSessionData()
                    {
                        ID = item.ID,
                        Code = item.SessionCode,
                        CollegeNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.SubjectID != null ? item.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblHall.HallCodeAr,
                        HallCodeEn = item.TblHall.HallCodeEn,
                        SessionType = item.Type,
                        SessionCost = _SessionDetail.Cost,
                        SessionDescription = item.Description,
                        //SubjectPicture = item.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSubject.Picture.StartsWith("http://") ? item.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSubject.Picture),
                        SubjectNameAr = item.SubjectID != null ? item.TblSubject.NameAr : "",
                        SubjectNameEn = item.SubjectID != null ? item.TblSubject.NameEn : "",
                        LecturerID = item.LecturerID,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        LecturesCount = item.LecturesCount,
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        Favourite = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.SessionID == item.ID).FirstOrDefault() == null ? false : true,
                        Subscribed = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.SessionID == item.ID).FirstOrDefault() == null ? false : true
                    };

                    if (item.Picture != null)
                    {
                        data.SubjectPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                        data.SessionPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com/" + item.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com/" + item.Picture;
                    }
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
        public HttpResponseMessage AddToFavorite(SubscriptionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                if (_Context.TblFavoriteSessions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault() == null)
                {
                    TblFavoriteSession _FavObj = new TblFavoriteSession()
                    {
                        SessionID = _Params.SessionID,
                        StudentID = _Params.StudentID,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblFavoriteSessions.Add(_FavObj);
                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "تم إضافه المحاضره الي المفضله";
                    _resultHandler.MessageEn = "Session has been added to your favorite list";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "المحاضره موجوده في المفضله من قبل";
                    _resultHandler.MessageEn = "Session was added before in favourite list";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
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

        [HttpPost]
        public HttpResponseMessage ToggleFavorite(SubscriptionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                if (_Params.AddOrRemove)
                {
                    if (_Context.TblFavoriteSessions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault() == null)
                    {
                        TblFavoriteSession _FavObj = new TblFavoriteSession()
                        {
                            SessionID = _Params.SessionID,
                            StudentID = _Params.StudentID,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now
                        };

                        _Context.TblFavoriteSessions.Add(_FavObj);
                        _Context.SaveChanges();

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.MessageAr = "تم إضافه المحاضره الي المفضله";
                        _resultHandler.MessageEn = "Session has been added to your favorite list";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = true;
                        _resultHandler.MessageAr = "المحاضره موجوده في المفضله من قبل";
                        _resultHandler.MessageEn = "Session was added before in favourite list";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                }
                else
                {
                    TblFavoriteSession _CurrentFav = _Context.TblFavoriteSessions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();

                    _Context.TblFavoriteSessions.Remove(_CurrentFav);
                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "تم حذف المحاضره من المفضله";
                    _resultHandler.MessageEn = "Session has been removed from your favourites";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
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

        [HttpGet]
        public HttpResponseMessage GetFavorites(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblFavoriteSession> _FavObjs = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _FavObjs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        SessionCost = _SessionDetails.Cost,
                        CollegeNameAr = item.TblSession.TblSubject.TblMajor.TblCollege.NameAr,
                        CollegeNameEn = item.TblSession.TblSubject.TblMajor.TblCollege.NameEn,
                        MajorNameAr = item.TblSession.TblSubject.TblMajor.NameAr,
                        MajorNameEn = item.TblSession.TblSubject.TblMajor.NameEn,
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic,
                        LecturesCount = item.TblSession.LecturesCount,
                        StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                    };

                    if (item.TblSession.Picture != null)
                    {
                        data.SubjectPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                        data.SessionPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.TblSession.Picture;
                    }

                    Data.Add(data);
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

        [HttpGet]
        public HttpResponseMessage GetEvaluationQues()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblEvaluationQuestion> _ListOfAnswers = _Context.TblEvaluationQuestions.Where(a => a.IsDeleted != true).ToList();
                List<EvalQuesData> Data = new List<EvalQuesData>();
                foreach (var ques in _ListOfAnswers)
                {
                    EvalQuesData data = new EvalQuesData()
                    {
                        QuestionID = ques.ID,
                        QuestionAr = ques.QuestionAr,
                        QuestionEn = ques.QuestionEn
                    };

                    Data.Add(data);
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

        [HttpGet]
        public HttpResponseMessage GetQuesStaticAnswers()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblEvaluationQuestionAnswer> _ListOfAnswers = _Context.TblEvaluationQuestionAnswers.Where(a => a.IsDeleted != true).ToList();
                List<EvalAnsData> Data = new List<EvalAnsData>();
                foreach (var answer in _ListOfAnswers)
                {
                    EvalAnsData data = new EvalAnsData()
                    {
                        AnswerID = answer.ID,
                        AnswerAr = answer.AnswerAr,
                        AnswerEn = answer.AnswerEn,
                        Value = answer.Value
                    };

                    Data.Add(data);
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

        [HttpGet]
        public HttpResponseMessage GetEvaluationQuesAndAns()
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                List<TblEvaluationQuestion> _ListOfQues = _Context.TblEvaluationQuestions.Where(a => a.IsDeleted != true).ToList();
                List<TblEvaluationQuestionAnswer> _ListOfAnswers = _Context.TblEvaluationQuestionAnswers.Where(a => a.IsDeleted != true).ToList();
                EvaluationData Data = new EvaluationData();
                Data.EvalAnsData = new List<EvalAnsData>();
                Data.EvalQuesData = new List<EvalQuesData>();

                foreach (var answer in _ListOfAnswers)
                {
                    EvalAnsData AnswersData = new EvalAnsData();

                    AnswersData.AnswerID = answer.ID;
                    AnswersData.AnswerAr = answer.AnswerAr;
                    AnswersData.AnswerEn = answer.AnswerEn;
                    AnswersData.Value = answer.Value;

                    Data.EvalAnsData.Add(AnswersData);
                }

                foreach (var ques in _ListOfQues)
                {
                    EvalQuesData QuesData = new EvalQuesData();

                    QuesData.QuestionID = ques.ID;
                    QuesData.QuestionAr = ques.QuestionAr;
                    QuesData.QuestionEn = ques.QuestionEn;

                    Data.EvalQuesData.Add(QuesData);
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
        public HttpResponseMessage AnswerToQues(EvaluationObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                for (int i = 0; i < _Params.QuestionIDs.Count; i++)
                {
                    TblEvaluation EvaluationObj = new TblEvaluation()
                    {
                        QuestionID = _Params.QuestionIDs[i],
                        AnswerID = _Params.AnswerIDs[i],
                        StudentID = _Params.StudentID,
                        SessionID = _Params.SessionID,
                        EvaluationNotes = _Params.EvaluationNotes,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblEvaluations.Add(EvaluationObj);
                }
                
                _Context.SaveChanges();

                _resultHandler.IsSuccessful = true;
                _resultHandler.MessageAr = "شكراً لتقييمك";
                _resultHandler.MessageEn = "Thanks for your evaluation!";

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

        [HttpGet]
        public HttpResponseMessage GetEvaluation(int LecturerID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblEvaluation> _Evaluation = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == LecturerID).ToList();
                EvaluationListData Data = new EvaluationListData();
                List<StudentEvaluationData> EvalData = new List<StudentEvaluationData>();
                Data.LecturerEvalData = new LecturerEvaluationData()
                {
                    LecturerNameAr = _Evaluation.FirstOrDefault().TblSession.TblLecturer.FirstNameAr + " " + _Evaluation.FirstOrDefault().TblSession.TblLecturer.SecondNameAr + " " + _Evaluation.FirstOrDefault().TblSession.TblLecturer.ThirdNameAr,
                    LecturerNameEn = _Evaluation.FirstOrDefault().TblSession.TblLecturer.FirstNameEn + " " + _Evaluation.FirstOrDefault().TblSession.TblLecturer.SecondNameEn + " " + _Evaluation.FirstOrDefault().TblSession.TblLecturer.ThirdNameEn,
                    SubjectNameAr = "محاضر " + _Evaluation.FirstOrDefault().TblSession.TblSubject.NameAr,
                    SubjectNameEn = _Evaluation.FirstOrDefault().TblSession.TblSubject.NameEn + " Instructor",
                    LecturerRate = 4
                };
                foreach (var item in _Evaluation)
                {
                    StudentEvaluationData data = new StudentEvaluationData()
                    {
                        
                        StudentName = item.TblStudent.FirstName + " " + item.TblStudent.SecondName + " " + item.TblStudent.ThirdName,
                        StudentRate = 3,
                        CollageAr = item.TblStudent.TblCollege.NameAr,
                        CollageEn = item.TblStudent.TblCollege.NameEn,
                        Evaluation = item.EvaluationNotes,
                        Time = "منذ ساعتين"
                    };
                    EvalData.Add(data);
                }
                Data.StudentEvalData = EvalData;

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
        public HttpResponseMessage GetSessionTimes(StudentSessionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblSubscription Sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
                if (Sub != null)
                {
                    TblSession _SessionObj = _Context.TblSessions.Where(a => a.ID == _Params.SessionID).SingleOrDefault();
                    if (_SessionObj != null)
                    {
                        List<TblSessionTime> _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == _Params.SessionID).ToList();
                        List<SessionTimesData> _Data = new List<SessionTimesData>();

                        foreach (var item in _SessionTimes)
                        {
                            TblAttendance _Attendance = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.StudentID == _Params.StudentID).FirstOrDefault();

                            SessionTimesData data = new SessionTimesData()
                            {
                                LectureAr = item.LectureAr,
                                LectureEn = item.LectureEn,
                                Date = item.FromTime.ToString("yyyy-MM-dd"),
                                Time = item.FromTime.ToString("hh:mm tt"),
                                Attend = _Attendance == null ? 0 : _Attendance.Attend
                            };

                            _Data.Add(data);
                        }

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.Result = _Data;
                        _resultHandler.MessageAr = "OK";
                        _resultHandler.MessageEn = "OK";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "السيشن غير موجوده";
                        _resultHandler.MessageEn = "Session id not found";

                        return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "الطالب غير مشترك في المحاضره";
                    _resultHandler.MessageEn = "Student is not subscribed to this session";

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

        [HttpPost]
        public HttpResponseMessage RequestPrivateSession(PrivateSessionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                if (_Params.StudentID > 0 && _Params.SubjectID > 0 && _Params.LecturerID > 0 && !string.IsNullOrEmpty(_Params.RequestMsg))
                {
                    TblPrivateSession _PrivateSessionObj = new TblPrivateSession()
                    {
                        StudentID = _Params.StudentID,
                        SubjectID = _Params.SubjectID,
                        LecturerID = _Params.LecturerID,
                        RequestMsg = _Params.RequestMsg,
                        RequestDate = _Params.RequestDate,
                        CreatedDate = DateTime.Now,
                        IsCourse = _Params.IsCourse
                    };

                    _Context.TblPrivateSessions.Add(_PrivateSessionObj);
                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.MessageAr = "تم إرسال طلبك بنجاح وسيتم التواصل معك من قبل المعهد في اقرب وقت ممكن لنحديد موعد المحاضره";
                    _resultHandler.MessageEn = "Your request has been submitted, the administration will back to you as oosn as possible to arrange the session with you";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "من فضلك ادخل البيانات كامله";
                    _resultHandler.MessageEn = "Please enter required data";

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

        [HttpGet]
        public HttpResponseMessage SearchForSessionByCode(string Code)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblSession _Session = _Context.TblSessions.Where(a => a.SessionCode == Code).FirstOrDefault();
                if (_Session != null)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == _Session.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == _Session.ID).FirstOrDefault();

                    SessionCourseData Data = new SessionCourseData()
                    {
                        ID = _Session.ID,
                        SessionType = _Session.Type,
                        LecturesCount = _Session.LecturesCount,
                        CollegeNameAr = _Session.SubjectID != null ? _Session.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = _Session.SubjectID != null ? _Session.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = _Session.SubjectID != null ? _Session.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = _Session.SubjectID != null ? _Session.TblSubject.TblMajor.NameEn : "",
                        StartDate = _Session.Type == true ? _Session.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = _Session.Type == true ? _Session.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        SubjectNameAr = _Session.SubjectID != null ? _Session.TblSubject.NameAr : "",
                        SubjectNameEn = _Session.SubjectID != null ? _Session.TblSubject.NameEn : "",
                        SessionCost = _SessionDetail.Cost,
                        LecturerID = _Session.LecturerID,
                        LecturerName = _Session.TblLecturer.FirstNameAr + " " + _Session.TblLecturer.SecondNameAr + " " + _Session.TblLecturer.ThirdNameAr,
                        LecturerNameEn = _Session.TblLecturer.FirstNameEn + " " + _Session.TblLecturer.SecondNameEn + " " + _Session.TblLecturer.ThirdNameEn,
                        LecturerPic = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //LecturerPic = _Session.TblSubject.TblLecturer.ProfilePic,
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        QRCode = DateTime.Now > _Session.FromDate ? "http://smuapitest.smartmindkw.com/" + _SessionDetail.QRCode : "http://smuapitest.smartmindkw.com/" + _SessionDetail.QRCode,
                        IsCourse = DateTime.Now > _Session.FromDate ? false : true,
                    };

                    if (_Session.TblSubject.Picture != null)
                    {
                        Data.SubjectPicture = _Session.TblSubject.Picture.StartsWith("http://") ? _Session.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + _Session.TblSubject.Picture);
                    }
                    else
                    {
                        Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _Session.TblSubject.Picture;
                    }

                    _resultHandler.IsSuccessful = true;
                    _resultHandler.Result = Data;
                    _resultHandler.MessageAr = "OK";
                    _resultHandler.MessageEn = "OK";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "لا يوجد محاضره بالكود : " + Code;
                    _resultHandler.MessageEn = "There is no session found with code : " + Code;

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

        [HttpGet]
        public HttpResponseMessage getSessionsByHallID(int HallID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now.Date;

                //List<TblSession> _SessionsList = _Context.TblSessions.Where(a => a.HallID == HallID && DbFunctions.TruncateTime(a.FromDate) >= CurrentDate).ToList();
                List<TblSession> _SessionsList = _Context.TblSessions.Where(a => a.HallID == HallID && DbFunctions.TruncateTime(a.FromDate) <= CurrentDate).ToList();//for test, uncomment the above line
                List<SessionCourseData> Data = new List<SessionCourseData>();

                foreach (var item in _SessionsList)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    SessionCourseData data = new SessionCourseData()
                    {
                        ID = item.ID,
                        SessionType = item.Type,
                        SessionCode = item.SessionCode,
                        LecturesCount = item.LecturesCount,
                        CollegeNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.SubjectID != null ? item.TblSubject.TblMajor.NameEn : "",
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        SubjectNameAr = item.SubjectID != null ? item.TblSubject.NameAr : "",
                        SubjectNameEn = item.SubjectID != null ? item.TblSubject.NameEn : "",
                        SessionCost = _SessionDetail.Cost,
                        LecturerID = item.LecturerID,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn,
                        LecturerPic = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //LecturerPic = item.TblSubject.TblLecturer.ProfilePic,
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        QRCode = DateTime.Now > item.FromDate ? _SessionDetail.QRCode : _SessionDetail.QRCode,
                        IsCourse = DateTime.Now > item.FromDate ? false : true,
                        
                    };
                    if (item.TblSubject.Picture != null)
                    {
                        data.SubjectPicture = item.TblSubject.Picture.StartsWith("http://") ? item.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSubject.Picture);
                    }
                    else
                    {
                        data.SubjectPicture = "http://smuapitest.smartmindkw.com" + item.TblSubject.Picture;
                    }
                    Data.Add(data);
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

        //[HttpGet]
        //public HttpResponseMessage GetTimelineSessionDetails(int SessionID, int StudentID)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID && a.Pending == true && a.IsDeleted != true).ToList();
        //        TimelineSessionDetailsData Data = new TimelineSessionDetailsData();
        //        PendedSubsData _PendedSubs = new PendedSubsData();
        //        _PendedSubs.PendedSubscriptions = new List<string>();

        //        foreach (var item in _Subs.Select(a => a.TblStudent))
        //        {
        //            _PendedSubs.PendedSubscriptions.Add(item.ProfilePic);
        //        }
        //        _PendedSubs.Count = _Subs.Count;
        //        Data.PendedSubscriptions = _PendedSubs;

        //        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //    }
        //    catch (Exception ex)
        //    {
        //        _resultHandler.IsSuccessful = false;
        //        _resultHandler.MessageAr = ex.Message;
        //        _resultHandler.MessageEn = ex.Message;

        //        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage getPrivateSessionRequest(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblPrivateSession> _PrivateObjs = _Context.TblPrivateSessions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).ToList();
                List<PrivateSessionsList> Data = new List<PrivateSessionsList>();

                foreach (var item in _PrivateObjs)
                {
                    PrivateSessionsList data = new PrivateSessionsList()
                    {
                        ID = item.ID,
                        StudentID = item.StudentID,
                        SubjectID = item.SubjectID,
                        LecturerID = item.LecturerID,
                        StudentName = item.TblStudent.FirstName + " " + item.TblStudent.SecondName + " " + item.TblStudent.ThirdName,
                        LecturerNameAr = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn,
                        SubjectNameAr = item.TblSubject.NameAr,
                        SubjectNameEn = item.TblSubject.NameEn,
                        RequestMsg = item.RequestMsg,
                        Approved = item.Approved,
                        IsCourse = item.IsCourse,
                        RequestDate = item.RequestDate.Value.ToString("yyyy-MM-dd"),
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