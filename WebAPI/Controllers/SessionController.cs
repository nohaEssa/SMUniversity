using Microsoft.Ajax.Utilities;
using SMUModels;
using SMUModels.Classes;
using SMUModels.ObjectData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
                    _resultHandler.Result = _Session.Description;
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
                string SessionNameAr = "", SessionNameEn = "";
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
                            int subCount = 0;
                            if (_SessionDetails != null)
                            {
                                subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionDetails.SessionID).Count();
                                SessionNameAr = _SessionDetails.TblSession.Name;
                                SessionNameEn = _SessionDetails.TblSession.NameEn;
                            }

                            Data = new SessionData()
                            {
                                SessionID = _SessionDetails != null ? _SessionDetails.SessionID : _ProductuObj.ID,
                                Type = _SessionDetails != null ? _SessionDetails.TblSession.Type : false,
                                GeneralSession = _SessionDetails != null ? _SessionDetails.TblSession.GeneralSession : false,
                                SubjectNameAr = _SessionDetails != null ? _SessionDetails.TblSession.Name : _ProductuObj.NameAr,
                                SubjectNameEn = _SessionDetails != null ? _SessionDetails.TblSession.NameEn : _ProductuObj.NameEn,
                                SessionNameAr = _SessionDetails != null ? _SessionDetails.TblSession.Name : _ProductuObj.NameAr,
                                SessionNameEn = _SessionDetails != null ? _SessionDetails.TblSession.NameEn : _ProductuObj.NameEn,
                                Lecturer = _SessionDetails != null ? _SessionDetails.TblSession.TblLecturer.FirstNameAr + " " + _SessionDetails.TblSession.TblLecturer.SecondNameAr + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameAr : "",
                                LecturerNameEn = _SessionDetails != null ? _SessionDetails.TblSession.TblLecturer.FirstNameEn + " " + _SessionDetails.TblSession.TblLecturer.SecondNameEn + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameEn : "",
                                //SubjectPicture = _SessionDetails.TblSession.TblSubject.Picture,
                                //Cost = decimal.Parse(_SessionDetails.Cost.ToString())
                            };
                            if (_SessionDetails != null)
                            {
                                //if (_SessionDetails.TblSession.TblSubject.Picture != null)
                                if (_SessionDetails.TblSession.Picture != null)
                                {
                                    Data.SubjectPicture = _SessionDetails.TblSession.Picture.StartsWith("http://") ? _SessionDetails.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.Picture);
                                }
                                else
                                {
                                    Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.Picture;
                                }
                            }
                            else
                            {
                                if (_ProductuObj.Picture != null)
                                {
                                    Data.SubjectPicture = _ProductuObj.Picture.StartsWith("http://") ? _ProductuObj.Picture : ("http://smuapitest.smartmindkw.com" + _ProductuObj.Picture);
                                }
                                else
                                {
                                    Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _ProductuObj.Picture;
                                }
                            }

                            if (_SessionDetails != null)
                            {
                                if (_SessionDetails.TblSession.Type)
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
                                    if (_SessionDetails.TblSession.FromDate.Date < DateTime.Now.Date)
                                    {
                                        Data.Cost = (decimal)_SessionDetails.SessionPrice;
                                    }
                                    else
                                    {
                                        Data.Cost = (decimal)_SessionDetails.Cost;
                                    }
                                }
                            }
                            else
                            {
                                Data.Cost = decimal.Parse(_ProductuObj.Cost.ToString());
                            }
                        }

                        ////StudentID must be added as a parameter in this web service to be able to send notification
                        //string TitleAr = "سمارت مايند الجامعه";
                        //string TitleEn = "SmartMind University";
                        //string DescriptionAr = "انت علي وشك الإشتراك في محاضره " + SessionNameAr + " وخصم مبلغ " + Data.Cost + " د.ك من رصيدك";
                        //string DescriptionEn = "You are about to be subscribed in " + SessionNameEn + " and a price of " + Data.Cost + " D.K will be deducted from your balance";

                        //Push(StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

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
                string SessionNameAr = "", SessionNameEn = "";
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
                            int subCount = 0;
                            if (_SessionDetails != null)
                            {
                                subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _SessionDetails.SessionID).Count();
                                SessionNameAr = _SessionDetails.TblSession.Name;
                                SessionNameEn = _SessionDetails.TblSession.NameEn;
                            }
                            Data = new SessionData()
                            {
                                SessionID = _SessionDetails != null ? _SessionDetails.SessionID : _ProductuObj.ID,
                                Type = _SessionDetails != null ? _SessionDetails.TblSession.Type : false,
                                GeneralSession = _SessionDetails != null ? _SessionDetails.TblSession.GeneralSession : false,
                                TitleAr = _SessionDetails != null ? _SessionDetails.TblSession.Name : _ProductuObj.NameAr,
                                TitleEn = _SessionDetails != null ? _SessionDetails.TblSession.NameEn : _ProductuObj.NameEn,
                                SubjectNameAr = _SessionDetails != null ? _SessionDetails.TblSession.Name : _ProductuObj.NameAr,
                                SubjectNameEn = _SessionDetails != null ? _SessionDetails.TblSession.NameEn : _ProductuObj.NameEn,
                                Lecturer = _SessionDetails != null ? _SessionDetails.TblSession.TblLecturer.FirstNameAr + " " + _SessionDetails.TblSession.TblLecturer.SecondNameAr + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameAr : "",
                                LecturerNameEn = _SessionDetails != null ? _SessionDetails.TblSession.TblLecturer.FirstNameEn + " " + _SessionDetails.TblSession.TblLecturer.SecondNameEn + " " + _SessionDetails.TblSession.TblLecturer.ThirdNameEn : "",
                                //SubjectPicture = _SessionPeriod.TblSession.TblSubject.Picture,
                                //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                                ProductCostMsgAr = _SessionDetails != null ? "سعر المحاضرة" : ("سعر " + _ProductuObj.NameAr),
                                ProductCostMsgEn = _SessionDetails != null ? "Session Cost" : (_ProductuObj.NameEn + " Cost"),
                                //Cost = _SessionDetails != null ? decimal.Parse(_SessionDetails.Cost.ToString()): decimal.Parse(_ProductuObj.Cost.ToString())
                            };

                            if (_SessionDetails != null)
                            {
                                //if (_SessionDetails.TblSession.TblSubject.Picture != null)
                                if (_SessionDetails.TblSession.Picture != null)
                                {
                                    Data.SubjectPicture = _SessionDetails.TblSession.Picture.StartsWith("http://") ? _SessionDetails.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.Picture);
                                }
                                else
                                {
                                    Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _SessionDetails.TblSession.Picture;
                                }
                            }
                            else
                            {
                                if (_ProductuObj.Picture != null)
                                {
                                    Data.SubjectPicture = _ProductuObj.Picture.StartsWith("http://") ? _ProductuObj.Picture : ("http://smuapitest.smartmindkw.com" + _ProductuObj.Picture);
                                }
                                else
                                {
                                    Data.SubjectPicture = "http://smuapitest.smartmindkw.com" + _ProductuObj.Picture;
                                }
                            }

                            if (_SessionDetails != null)
                            {
                                if (_SessionDetails.TblSession.Type)
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
                                    if (_SessionDetails.TblSession.FromDate.Date < DateTime.Now.Date)
                                    {
                                        Data.Cost = (decimal)_SessionDetails.SessionPrice;
                                    }
                                    else
                                    {
                                        Data.Cost = (decimal)_SessionDetails.Cost;
                                    }
                                }
                            }
                            else
                            {
                                Data.Cost = decimal.Parse(_ProductuObj.Cost.ToString());
                            }

                        }

                        ////StudentID must be added as a parameter in this web service to be able to send notification
                        //string TitleAr = "سمارت مايند الجامعه";
                        //string TitleEn = "SmartMind University";
                        //string DescriptionAr = "انت علي وشك الإشتراك في محاضره " + SessionNameAr + " وخصم مبلغ " + Data.Cost + " د.ك من رصيدك";
                        //string DescriptionEn = "You are about to be subscribed in " + SessionNameEn + " and a price of " + Data.Cost + " D.K will be deducted from your balance";

                        //Push(StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

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

        //[HttpPost]
        //public HttpResponseMessage SubscripetoSession(SubscriptionObj _Params)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        if (_Params.SessionID > 0 && _Params.StudentID > 0)
        //        {
        //            TblSubscription sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
        //            if (sub == null)
        //            {
        //                TblSessionDetail _Session = _Context.TblSessionDetails.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
        //                TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();

        //                if (_Student != null)
        //                {
        //                    TblSubscription _Sub = new TblSubscription()
        //                    {
        //                        SessionID = _Params.SessionID,
        //                        StudentID = _Params.StudentID,
        //                        FromLecturerSide = false,
        //                        IsDeleted = false,
        //                        CreatedDate = DateTime.Now
        //                    };
        //                    if (_Session.TblSession.Type == true)
        //                    {
        //                        _Sub.SubscripedAsSession = true;
        //                    }
        //                    else
        //                    {
        //                        if (_Session.TblSession.FromDate.Date == DateTime.Now.Date)
        //                        {
        //                            _Sub.SubscripedAsSession = true;
        //                        }
        //                        else
        //                        {
        //                            _Sub.SubscripedAsSession = false;
        //                        }
        //                    }
        //                    _Context.TblSubscriptions.Add(_Sub);

        //                    if (!_Params.Pending)
        //                    {
        //                        if (_Params.Pending == false && _Student.Balance >= _Session.Cost)
        //                        {
        //                            int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).Count();
        //                            TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
        //                            TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
        //                            if(_Session.TblSession.Type == true)
        //                            {
        //                                switch (subCount)
        //                                {
        //                                    case 0:
        //                                        _Sub.PriceType = 1;
        //                                        _Sub.Price = (decimal)_Session.Price1;
        //                                        break;
        //                                    case 1:
        //                                        _Sub.PriceType = 2;
        //                                        _Sub.Price = (decimal)_Session.Price2;

        //                                        firstSub.Price -= (decimal)_Session.Price1 - (decimal)_Session.Price2;

        //                                        TblStudent FirstStudent = _Context.TblStudents.Where(a => a.ID == firstSub.StudentID).SingleOrDefault();
        //                                        FirstStudent.Balance += (decimal)_Session.Price1 - (decimal)_Session.Price2;

        //                                        TblBalanceTransaction BalanceTrans = new TblBalanceTransaction()
        //                                        {
        //                                            StudentID = FirstStudent.ID,
        //                                            Price = (decimal)_Session.Price1 - (decimal)_Session.Price2,
        //                                            Pending = false,
        //                                            IsDeleted = false,
        //                                            PaymentMethod = "Cash",
        //                                            TransactionTypeID = 1,
        //                                            TitleAr = "استرداد رصيد",
        //                                            TitleEn = "Recover money",
        //                                            CreatedDate = DateTime.Now,
        //                                        };

        //                                        _Context.TblBalanceTransactions.Add(BalanceTrans);
        //                                        _Context.SaveChanges();

        //                                        break;
        //                                    case 2:
        //                                        _Sub.PriceType = 3;
        //                                        _Sub.Price = (decimal)_Session.Price3;

        //                                        firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
        //                                        secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;

        //                                        TblStudent _FirstStudent = _Context.TblStudents.Where(a => a.ID == firstSub.StudentID).SingleOrDefault();
        //                                        _FirstStudent.Balance += (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2);

        //                                        TblStudent SecondStudent = _Context.TblStudents.Where(a => a.ID == secondSub.StudentID).SingleOrDefault();
        //                                        SecondStudent.Balance += (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2);

        //                                        TblBalanceTransaction BalanceTrans2 = new TblBalanceTransaction()
        //                                        {
        //                                            StudentID = _FirstStudent.ID,
        //                                            Price = (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2),
        //                                            Pending = false,
        //                                            IsDeleted = false,
        //                                            PaymentMethod = "Cash",
        //                                            TransactionTypeID = 1,
        //                                            TitleAr = "استرداد رصيد",
        //                                            TitleEn = "Recover money",
        //                                            CreatedDate = DateTime.Now,
        //                                        };

        //                                        _Context.TblBalanceTransactions.Add(BalanceTrans2);
        //                                        _Context.SaveChanges();
        //                                        break;
        //                                    default:
        //                                        _Sub.PriceType = 3;
        //                                        _Sub.Price = (decimal)_Session.Price3;

        //                                        //firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
        //                                        //secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
        //                                        break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                _Sub.Price = (decimal)_Session.Cost;
        //                            }

        //                            _Sub.Pending = false;
        //                            _Student.Balance -= _Session.Cost;

        //                            TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
        //                            {
        //                                StudentID = _Params.StudentID,
        //                                Price = _Session.Cost,
        //                                Pending = false,
        //                                IsDeleted = false,
        //                                PaymentMethod = "Cash",
        //                                TransactionTypeID = 2,
        //                                TitleAr = "اشتراك في محاضرة : " + _Session.TblSession.TblSubject.NameAr,
        //                                TitleEn = "Subscription to subject : " + _Session.TblSession.TblSubject.NameEn,
        //                                CreatedDate = DateTime.Now,
        //                            };

        //                            _Context.TblBalanceTransactions.Add(_BalanceTrans);
        //                            _Context.SaveChanges();

        //                            _resultHandler.IsSuccessful = true;
        //                            _resultHandler.MessageAr = _Params.Pending ? "تم الإشتراك في المحاضره" : "تم الإشتراك في محاضره " + _Session.TblSession.TblSubject.NameAr + " وخصم مبلغ " + _Sub.Price + " من رصيدك";
        //                            _resultHandler.MessageEn = _Params.Pending ? "You have been subscribed to " + _Session.TblSession.TblSubject.NameEn : "You have been subscribed to " + _Session.TblSession.TblSubject.NameEn + ", and a balance of " + _Sub.Price + " has been deducted";

        //                            return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //                        }
        //                        else
        //                        {
        //                            _resultHandler.IsSuccessful = false;
        //                            _resultHandler.MessageAr = "رصيدك غير كافي للاشتراك في المحاضره, اشحن ثم حاول مره اخري";
        //                            _resultHandler.MessageEn = "You have not enough balance to subscripe to lecturer, Please recharge and try again";

        //                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        _Sub.Pending = true;
        //                        _Context.SaveChanges();
        //                    }
        //                }
        //                else
        //                {
        //                    _resultHandler.IsSuccessful = false;
        //                    _resultHandler.MessageAr = "رقم الطالب غير صحيح او غير موجود";
        //                    _resultHandler.MessageEn = "Please provide a valid Student ID";

        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //                }
        //            }
        //            else
        //            {
        //                _resultHandler.IsSuccessful = false;
        //                _resultHandler.MessageAr = "تم الإشتراك في هذه المحاضره من قبل";
        //                _resultHandler.MessageEn = "This session hass been subscribed to before";

        //                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //            }                    
        //        }
        //        else
        //        {
        //            _resultHandler.IsSuccessful = false;
        //            _resultHandler.MessageAr = "أدخل رقم الكيو ار كود";
        //            _resultHandler.MessageEn = "Please provide QRCode ID";

        //            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //        }

        //        _resultHandler.IsSuccessful = true;
        //        _resultHandler.MessageAr = "OK";
        //        _resultHandler.MessageEn = "OK";

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

        //[HttpPost]
        //public HttpResponseMessage SubscripetoSession(SubscriptionObj _Params)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        if (_Params.SessionID > 0 && _Params.StudentID > 0)
        //        {
        //            TblSubscription _Sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
        //            TblSessionDetail _Session = _Context.TblSessionDetails.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
        //            TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();

        //            if (_Student != null)
        //            {
        //                if (!_Params.Pending)
        //                {

        //                    //if ((_Session.TblSession.Type == true && _Session.TblSession.ToDate.Date < DateTime.Now.Date) || (_Session.TblSession.Type == false && DbFunctions.TruncateTime(_Session.TblSession.ToDate.Date) < DateTime.Now.Date))
        //                    //{
        //                    //    _resultHandler.IsSuccessful = false;
        //                    //    _resultHandler.MessageAr = "هذه المحاضره قد انتهي معادها";
        //                    //    _resultHandler.MessageEn = "This session is expired";

        //                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //                    //}
        //                    //else
        //                    //{

        //                    //}
        //                    //_Sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID && a.Pending != true).FirstOrDefault();
        //                    _Sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
        //                    //if (_Sub == null)
        //                    if (_Params.Pending == false || (_Sub.Pending == true && _Params.Pending == false))
        //                    {
        //                        //if (_Params.Pending == false && _Student.Balance >= _Session.Cost)
        //                        if (_Params.Pending == false && _Student.Balance >= _Session.Cost)
        //                        {
        //                            if (_Sub == null)
        //                            {
        //                                _Sub = new TblSubscription()
        //                                {
        //                                    SessionID = _Params.SessionID,
        //                                    StudentID = _Params.StudentID,
        //                                    FromLecturerSide = false,
        //                                    IsDeleted = false,
        //                                    CreatedDate = DateTime.Now
        //                                };
        //                            }
        //                            else
        //                            {
        //                                _Sub.UpdatedDate = DateTime.Now;
        //                            }
        //                            _Sub.Pending = _Params.Pending;

        //                            //_Sub = new TblSubscription()
        //                            //{
        //                            //    SessionID = _Params.SessionID,
        //                            //    StudentID = _Params.StudentID,
        //                            //    FromLecturerSide = false,
        //                            //    IsDeleted = false,
        //                            //    Pending = _Params.Pending,
        //                            //    CreatedDate = DateTime.Now
        //                            //};
        //                            if (_Session.TblSession.Type == true)
        //                            {
        //                                _Sub.SubscripedAsSession = true;
        //                            }
        //                            else
        //                            {
        //                                if (_Session.TblSession.FromDate.Date == DateTime.Now.Date)
        //                                {
        //                                    _Sub.SubscripedAsSession = false;
        //                                    _Sub.Price = (decimal)_Session.Cost;
        //                                }
        //                                else
        //                                {
        //                                    _Sub.SubscripedAsSession = true;
        //                                    _Sub.Price = (decimal)_Session.SessionPrice;
        //                                }
        //                            }
        //                            _Context.TblSubscriptions.Add(_Sub);

        //                            int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).Count();
        //                            TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
        //                            TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
        //                            _Context.SaveChanges();

        //                            if (_Session.TblSession.Type == true)
        //                            {
        //                                switch (subCount)
        //                                {
        //                                    case 0:
        //                                        _Sub.PriceType = 1;
        //                                        _Sub.Price = (decimal)_Session.Price1;
        //                                        break;
        //                                    case 1:
        //                                        _Sub.PriceType = 2;
        //                                        _Sub.Price = (decimal)_Session.Price2;

        //                                        firstSub.Price -= (decimal)_Session.Price1 - (decimal)_Session.Price2;

        //                                        TblStudent FirstStudent = _Context.TblStudents.Where(a => a.ID == firstSub.StudentID).SingleOrDefault();
        //                                        FirstStudent.Balance += (decimal)_Session.Price1 - (decimal)_Session.Price2;

        //                                        TblBalanceTransaction BalanceTrans = new TblBalanceTransaction()
        //                                        {
        //                                            StudentID = FirstStudent.ID,
        //                                            Price = (decimal)_Session.Price1 - (decimal)_Session.Price2,
        //                                            Pending = false,
        //                                            IsDeleted = false,
        //                                            PaymentMethod = "Cash",
        //                                            TransactionTypeID = 1,
        //                                            TitleAr = "استرداد رصيد",
        //                                            TitleEn = "Recover money",
        //                                            CreatedDate = DateTime.Now,
        //                                        };

        //                                        _Context.TblBalanceTransactions.Add(BalanceTrans);
        //                                        _Context.SaveChanges();

        //                                        string TitleAr1 = "سمارت مايند الجامعه";
        //                                        string TitleEn1 = "SmartMind University";
        //                                        string DescriptionAr1 = "تم استراد مبلغ " + ((decimal)_Session.Price1 - (decimal)_Session.Price2) + " د.ك واضافته الي رصيدك نتيجه اشتراك طالب تاني في محاضره " + _Sub.TblSession.Name;
        //                                        string DescriptionEn1 = "A balance of " + ((decimal)_Session.Price1 - (decimal)_Session.Price2) + " have been added to your balance due to a a second subscription in the lecture of " + _Sub.TblSession.NameEn;

        //                                        Push(_Sub.StudentID, 0, TitleAr1, TitleEn1, DescriptionAr1, DescriptionEn1, 0);

        //                                        break;
        //                                    case 2:
        //                                        _Sub.PriceType = 3;
        //                                        _Sub.Price = (decimal)_Session.Price3;

        //                                        firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
        //                                        secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;

        //                                        TblStudent _FirstStudent = _Context.TblStudents.Where(a => a.ID == firstSub.StudentID).SingleOrDefault();
        //                                        _FirstStudent.Balance += (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2);

        //                                        TblStudent SecondStudent = _Context.TblStudents.Where(a => a.ID == secondSub.StudentID).SingleOrDefault();
        //                                        SecondStudent.Balance += (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2);

        //                                        TblBalanceTransaction BalanceTrans2 = new TblBalanceTransaction()
        //                                        {
        //                                            StudentID = _FirstStudent.ID,
        //                                            Price = (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2),
        //                                            Pending = false,
        //                                            IsDeleted = false,
        //                                            PaymentMethod = "Cash",
        //                                            TransactionTypeID = 1,
        //                                            TitleAr = "استرداد رصيد",
        //                                            TitleEn = "Recover money",
        //                                            CreatedDate = DateTime.Now,
        //                                        };

        //                                        _Context.TblBalanceTransactions.Add(BalanceTrans2);

        //                                        TblBalanceTransaction BalanceTrans3 = new TblBalanceTransaction()
        //                                        {
        //                                            StudentID = SecondStudent.ID,
        //                                            Price = (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2),
        //                                            Pending = false,
        //                                            IsDeleted = false,
        //                                            PaymentMethod = "Cash",
        //                                            TransactionTypeID = 1,
        //                                            TitleAr = "استرداد رصيد",
        //                                            TitleEn = "Recover money",
        //                                            CreatedDate = DateTime.Now,
        //                                        };

        //                                        _Context.TblBalanceTransactions.Add(BalanceTrans3);

        //                                        _Context.SaveChanges();

        //                                        string TitleAr2 = "سمارت مايند الجامعه";
        //                                        string TitleEn2 = "SmartMind University";
        //                                        string DescriptionAr2 = "تم استراد مبلغ " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " د.ك واضافته الي رصيدك نتيجه اشتراك طالب ثالث في محاضره " + _Sub.TblSession.Name;
        //                                        string DescriptionEn2 = "A balance of " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " have been added to your balance due to a a third subscription in the lecture of " + _Sub.TblSession.NameEn;

        //                                        Push(_FirstStudent.ID, 0, TitleAr2, TitleEn2, DescriptionAr2, DescriptionEn2, 0);

        //                                        string TitleAr3 = "سمارت مايند الجامعه";
        //                                        string TitleEn3 = "SmartMind University";
        //                                        string DescriptionAr3 = "تم استراد مبلغ " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " د.ك واضافته الي رصيدك نتيجه اشتراك طالب ثالث في محاضره " + _Sub.TblSession.Name;
        //                                        string DescriptionEn3 = "A balance of " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " have been added to your balance due to a a third subscription in the lecture of " + _Sub.TblSession.NameEn;

        //                                        Push(SecondStudent.ID, 0, TitleAr3, TitleEn3, DescriptionAr3, DescriptionEn3, 0);
        //                                        break;
        //                                    default:
        //                                        _Sub.PriceType = 3;
        //                                        _Sub.Price = (decimal)_Session.Price3;

        //                                        //firstSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
        //                                        //secondSub.Price -= ((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2;
        //                                        break;
        //                                }
        //                            }
        //                            //else
        //                            //{
        //                            //    _Sub.Price = (decimal)_Session.Cost;
        //                            //}

        //                            _Sub.Pending = false;
        //                            _Student.Balance -= _Sub.Price;

        //                            TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
        //                            {
        //                                StudentID = _Params.StudentID,
        //                                Price = _Sub.Price,
        //                                Pending = false,
        //                                IsDeleted = false,
        //                                PaymentMethod = "Cash",
        //                                TransactionTypeID = 2,
        //                                TitleAr = "اشتراك في محاضرة : " + _Session.TblSession.TblSubject.NameAr,
        //                                TitleEn = "Subscription to subject : " + _Session.TblSession.TblSubject.NameEn,
        //                                CreatedDate = DateTime.Now,
        //                            };

        //                            _Context.TblBalanceTransactions.Add(_BalanceTrans);
        //                            _Context.SaveChanges();

        //                            try
        //                            {
        //                                TblInvoice _invoiceObj = new TblInvoice()
        //                                {
        //                                    StudentID = _Student.ID,
        //                                    SubscriptionID = _Sub.ID,
        //                                    RealCash = true,
        //                                    Price = _Sub.Price,
        //                                    Pending = false,
        //                                    PaymentMethod = "Cash",
        //                                    IsDeleted = false,
        //                                    CreatedDate = DateTime.Now
        //                                };

        //                                int Serial;
        //                                var CountVouchers = _Context.TblInvoices.Count();
        //                                if (CountVouchers > 0)
        //                                {
        //                                    //List<TblVoucher> lastcode = _Context.TblVouchers.ToList();
        //                                    long MyMax = _Context.TblInvoices.Max(a => a.Serial);

        //                                    Serial = int.Parse(MyMax.ToString()) + 1;
        //                                    _invoiceObj.Serial = Serial;
        //                                }
        //                                else
        //                                {
        //                                    _invoiceObj.Serial = 1;
        //                                }

        //                                _Context.TblInvoices.Add(_invoiceObj);
        //                                _Context.SaveChanges();
        //                            }
        //                            catch (Exception)
        //                            {
        //                            }

        //                            try
        //                            {
        //                                TblBalanceTransaction LecturereBalanceTrans = new TblBalanceTransaction()
        //                                {
        //                                    LecturerID = _Session.TblSession.LecturerID,
        //                                    Price = _Context.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == _Session.TblSession.LecturerAccountMethod).FirstOrDefault().Value,
        //                                    Pending = false,
        //                                    IsDeleted = false,
        //                                    PaymentMethod = "",
        //                                    TransactionTypeID = 1,
        //                                    TitleAr = "تم إشتراك طالب جديد في محاضره : " + _Session.TblSession.Name,
        //                                    TitleEn = "New Student joined your lecture : " + _Session.TblSession.NameEn,
        //                                    CreatedDate = DateTime.Now
        //                                };

        //                                _Context.TblBalanceTransactions.Add(LecturereBalanceTrans);
        //                                _Context.SaveChanges();
        //                            }
        //                            catch (Exception)
        //                            {
        //                            }

        //                            string TitleAr = "سمارت مايند الجامعه";
        //                            string TitleEn = "SmartMind University";
        //                            string DescriptionAr = "تم الإشتراك في محاضره " + _Sub.TblSession.Name + " وخصم مبلغ " + _Sub.Price + " د.ك من رصيدك";
        //                            string DescriptionEn = "You've been subscribed in " + _Sub.TblSession.NameEn + " and a price of " + _Sub.Price + " D.K have been deducted from your balance";

        //                            Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

        //                            _resultHandler.IsSuccessful = true;
        //                            _resultHandler.MessageAr = _Params.Pending ? "تم الإشتراك في المحاضره" : "تم الإشتراك في محاضره " + _Session.TblSession.Name + " وخصم مبلغ " + _Sub.Price + " من رصيدك";
        //                            _resultHandler.MessageEn = _Params.Pending ? "You have been subscribed to " + _Session.TblSession.NameEn : "You have been subscribed to " + _Session.TblSession.NameEn + ", and a balance of " + _Sub.Price + " has been deducted";

        //                            return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //                        }
        //                        else
        //                        {
        //                            _resultHandler.IsSuccessful = false;
        //                            _resultHandler.MessageAr = "رصيدك غير كافي للاشتراك في المحاضره, اشحن ثم حاول مره اخري";
        //                            _resultHandler.MessageEn = "You have not enough balance to subscripe to this lecturer, Please recharge and try again";

        //                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        _resultHandler.IsSuccessful = false;
        //                        _resultHandler.MessageAr = "تم الإشتراك في المحاضره من قبل";
        //                        _resultHandler.MessageEn = "You have been subscribed before";

        //                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //                    }
        //                }
        //                else
        //                {
        //                    _Sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID && a.Pending == true).FirstOrDefault();
        //                    if (_Sub == null)
        //                    {
        //                        _Sub = new TblSubscription()
        //                        {
        //                            SessionID = _Params.SessionID,
        //                            StudentID = _Params.StudentID,
        //                            FromLecturerSide = false,
        //                            IsDeleted = false,
        //                            Pending = _Params.Pending,
        //                            SubscripedAsSession = false,
        //                            CreatedDate = DateTime.Now
        //                        };
        //                        _Context.TblSubscriptions.Add(_Sub);
        //                        _Context.SaveChanges();

        //                        string TitleAr = "سمارت مايند الجامعه";
        //                        string TitleEn = "SmartMind University";
        //                        string DescriptionAr = "تم الإشتراك في محاضره " + _Session.TblSession.Name + " واضافتها الي المحاضرات القادمه";
        //                        string DescriptionEn = "You've been subscribed in " + _Session.TblSession.NameEn + " and added to your upcoming lectures";

        //                        Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

        //                        _resultHandler.IsSuccessful = true;
        //                        _resultHandler.MessageAr = "تم الإشتراك في المحاضره";
        //                        _resultHandler.MessageEn = "You have been subscribed to " + _Session.TblSession.TblSubject.NameEn;

        //                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //                    }
        //                    else
        //                    {
        //                        _resultHandler.IsSuccessful = false;
        //                        _resultHandler.MessageAr = "تم الإشتراك في المحاضره من قبل واضاقتها الي المحاضرات القادمه";
        //                        _resultHandler.MessageEn = "You have been subscribed before";

        //                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                _resultHandler.IsSuccessful = false;
        //                _resultHandler.MessageAr = "رقم الطالب غير صحيح او غير موجود";
        //                _resultHandler.MessageEn = "Please provide a valid Student ID";

        //                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //            }
        //        }
        //        else
        //        {
        //            _resultHandler.IsSuccessful = false;
        //            _resultHandler.MessageAr = "أدخل رقم الكيو ار كود";
        //            _resultHandler.MessageEn = "Please provide QRCode ID";

        //            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _resultHandler.IsSuccessful = false;
        //        _resultHandler.MessageAr = ex.Message;
        //        _resultHandler.MessageEn = ex.Message;

        //        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //    }
        //}


        [HttpPost]
        public HttpResponseMessage SubscripetoSession(SubscriptionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                if (_Params.SessionID > 0 && _Params.StudentID > 0)
                {
                    TblSubscription _RealSub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID && a.Pending != true).FirstOrDefault();
                    TblSubscription _VirtualSub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID && a.Pending == true).FirstOrDefault();
                    TblSessionDetail _Session = _Context.TblSessionDetails.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                    TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();

                    if (_Student != null)
                    {
                        if ((_Params.Pending == false && _RealSub == null) || (_Params.Pending == true && _VirtualSub == null))
                        {
                            if ((_Session.TblSession.Type == true && _Session.TblSession.FromDate.Date < DateTime.Now.Date) || (_Session.TblSession.Type == false && _Session.TblSession.ToDate.Date < DateTime.Now.Date))
                            {
                                _resultHandler.IsSuccessful = false;
                                _resultHandler.MessageAr = "هذه المحاضره قد انتهي معادها";
                                _resultHandler.MessageEn = "This session is expired";

                                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                            }
                            else
                            {
                                if (_Params.Pending == false && _RealSub == null)
                                {
                                    if (_Student.Balance >= _Session.Cost)
                                    {
                                        _RealSub = new TblSubscription()
                                        {
                                            SessionID = _Params.SessionID,
                                            StudentID = _Params.StudentID,
                                            FromLecturerSide = false,
                                            Pending = _Params.Pending,
                                            IsDeleted = false,
                                            CreatedDate = DateTime.Now
                                        };
                                        if (_Session.TblSession.Type == true)
                                        {
                                            _RealSub.SubscripedAsSession = true;
                                        }
                                        else
                                        {
                                            if (_Session.TblSession.FromDate.Date == DateTime.Now.Date)
                                            {
                                                _RealSub.SubscripedAsSession = false;
                                                _RealSub.Price = (decimal)_Session.Cost;
                                            }
                                            else
                                            {
                                                _RealSub.SubscripedAsSession = true;
                                                _RealSub.Price = (decimal)_Session.SessionPrice;
                                            }
                                        }
                                        _Context.TblSubscriptions.Add(_RealSub);

                                        int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).Count();
                                        TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                                        TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
                                        _Context.SaveChanges();

                                        if (_Session.TblSession.Type == true)
                                        {
                                            switch (subCount)
                                            {
                                                case 0:
                                                    _RealSub.PriceType = 1;
                                                    _RealSub.Price = (decimal)_Session.Price1;
                                                    break;
                                                case 1:
                                                    _RealSub.PriceType = 2;
                                                    _RealSub.Price = (decimal)_Session.Price2;

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

                                                    string TitleAr1 = "سمارت مايند الجامعه";
                                                    string TitleEn1 = "SmartMind University";
                                                    string DescriptionAr1 = "تم استراد مبلغ " + ((decimal)_Session.Price1 - (decimal)_Session.Price2) + " د.ك واضافته الي رصيدك نتيجه اشتراك طالب تاني في محاضره " + _RealSub.TblSession.Name;
                                                    string DescriptionEn1 = "A balance of " + ((decimal)_Session.Price1 - (decimal)_Session.Price2) + " have been added to your balance due to a a second subscription in the lecture of " + _RealSub.TblSession.NameEn;

                                                    Push(_RealSub.StudentID, 0, TitleAr1, TitleEn1, DescriptionAr1, DescriptionEn1, 0);

                                                    break;
                                                case 2:
                                                    _RealSub.PriceType = 3;
                                                    _RealSub.Price = (decimal)_Session.Price3;

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

                                                    TblBalanceTransaction BalanceTrans3 = new TblBalanceTransaction()
                                                    {
                                                        StudentID = SecondStudent.ID,
                                                        Price = (((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2),
                                                        Pending = false,
                                                        IsDeleted = false,
                                                        PaymentMethod = "Cash",
                                                        TransactionTypeID = 1,
                                                        TitleAr = "استرداد رصيد",
                                                        TitleEn = "Recover money",
                                                        CreatedDate = DateTime.Now,
                                                    };

                                                    _Context.TblBalanceTransactions.Add(BalanceTrans3);

                                                    _Context.SaveChanges();

                                                    string TitleAr2 = "سمارت مايند الجامعه";
                                                    string TitleEn2 = "SmartMind University";
                                                    string DescriptionAr2 = "تم استراد مبلغ " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " د.ك واضافته الي رصيدك نتيجه اشتراك طالب ثالث في محاضره " + _RealSub.TblSession.Name;
                                                    string DescriptionEn2 = "A balance of " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " have been added to your balance due to a a third subscription in the lecture of " + _RealSub.TblSession.NameEn;

                                                    Push(_FirstStudent.ID, 0, TitleAr2, TitleEn2, DescriptionAr2, DescriptionEn2, 0);

                                                    string TitleAr3 = "سمارت مايند الجامعه";
                                                    string TitleEn3 = "SmartMind University";
                                                    string DescriptionAr3 = "تم استراد مبلغ " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " د.ك واضافته الي رصيدك نتيجه اشتراك طالب ثالث في محاضره " + _RealSub.TblSession.Name;
                                                    string DescriptionEn3 = "A balance of " + ((((decimal)_Session.Price1 - (decimal)_Session.Price3) / 2)) + " have been added to your balance due to a a third subscription in the lecture of " + _RealSub.TblSession.NameEn;

                                                    Push(SecondStudent.ID, 0, TitleAr3, TitleEn3, DescriptionAr3, DescriptionEn3, 0);
                                                    break;
                                                default:
                                                    _RealSub.PriceType = 3;
                                                    _RealSub.Price = (decimal)_Session.Price3;
                                                    break;
                                            }
                                        }
                                        _RealSub.Pending = false;
                                        _Student.Balance -= _RealSub.Price;

                                        TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
                                        {
                                            StudentID = _Params.StudentID,
                                            Price = _RealSub.Price,
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

                                        try
                                        {
                                            TblInvoice _invoiceObj = new TblInvoice()
                                            {
                                                StudentID = _Student.ID,
                                                SubscriptionID = _RealSub.ID,
                                                RealCash = true,
                                                Price = _RealSub.Price,
                                                Pending = false,
                                                PaymentMethod = "Cash",
                                                IsDeleted = false,
                                                CreatedDate = DateTime.Now
                                            };

                                            int Serial;
                                            var CountVouchers = _Context.TblInvoices.Count();
                                            if (CountVouchers > 0)
                                            {
                                                //List<TblVoucher> lastcode = _Context.TblVouchers.ToList();
                                                long MyMax = _Context.TblInvoices.Max(a => a.Serial);

                                                Serial = int.Parse(MyMax.ToString()) + 1;
                                                _invoiceObj.Serial = Serial;
                                            }
                                            else
                                            {
                                                _invoiceObj.Serial = 1;
                                            }

                                            _Context.TblInvoices.Add(_invoiceObj);
                                            _Context.SaveChanges();
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        try
                                        {
                                            TblBalanceTransaction LecturereBalanceTrans = new TblBalanceTransaction()
                                            {
                                                LecturerID = _Session.TblSession.LecturerID,
                                                Price = _Context.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == _Session.TblSession.LecturerAccountMethod).FirstOrDefault().Value,
                                                Pending = false,
                                                IsDeleted = false,
                                                PaymentMethod = "",
                                                TransactionTypeID = 1,
                                                TitleAr = "تم إشتراك طالب جديد في محاضره : " + _Session.TblSession.Name,
                                                TitleEn = "New Student joined your lecture : " + _Session.TblSession.NameEn,
                                                CreatedDate = DateTime.Now
                                            };

                                            _Context.TblBalanceTransactions.Add(LecturereBalanceTrans);
                                            _Context.SaveChanges();
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        string TitleAr = "سمارت مايند الجامعه";
                                        string TitleEn = "SmartMind University";
                                        string DescriptionAr = "تم الإشتراك في محاضره " + _RealSub.TblSession.Name + " وخصم مبلغ " + _RealSub.Price + " د.ك من رصيدك";
                                        string DescriptionEn = "You've been subscribed in " + _RealSub.TblSession.NameEn + " and a price of " + _RealSub.Price + " D.K have been deducted from your balance";

                                        Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                                        _resultHandler.IsSuccessful = true;
                                        _resultHandler.MessageAr = _Params.Pending ? "تم الإشتراك في المحاضره" : "تم الإشتراك في محاضره " + _Session.TblSession.Name + " وخصم مبلغ " + _RealSub.Price + " من رصيدك";
                                        _resultHandler.MessageEn = _Params.Pending ? "You have been subscribed to " + _Session.TblSession.NameEn : "You have been subscribed to " + _Session.TblSession.NameEn + ", and a balance of " + _RealSub.Price + " has been deducted";

                                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                                    }
                                    else
                                    {
                                        _resultHandler.IsSuccessful = false;
                                        _resultHandler.MessageAr = "رصيدك غير كافي للاشتراك في المحاضره, اشحن ثم حاول مره اخري";
                                        _resultHandler.MessageEn = "You have not enough balance to subscripe to this lecturer, Please recharge and try again";

                                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                                    }
                                }
                                else if (_Params.Pending == false && _RealSub != null)
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.MessageAr = "تم الإشتراك في المحاضره من قبل";
                                    _resultHandler.MessageEn = "You have been subscribed before";

                                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                                }
                                else if (_Params.Pending == true && _VirtualSub == null)
                                {
                                    _VirtualSub = new TblSubscription()
                                    {
                                        SessionID = _Params.SessionID,
                                        StudentID = _Params.StudentID,
                                        FromLecturerSide = false,
                                        IsDeleted = false,
                                        Pending = _Params.Pending,
                                        SubscripedAsSession = false,
                                        CreatedDate = DateTime.Now
                                    };
                                    _Context.TblSubscriptions.Add(_VirtualSub);
                                    _Context.SaveChanges();

                                    string TitleAr = "سمارت مايند الجامعه";
                                    string TitleEn = "SmartMind University";
                                    string DescriptionAr = "تم الإشتراك في محاضره " + _Session.TblSession.Name + " واضافتها الي المحاضرات القادمه";
                                    string DescriptionEn = "You've been subscribed in " + _Session.TblSession.NameEn + " and added to your upcoming lectures";

                                    Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                                    _resultHandler.IsSuccessful = true;
                                    _resultHandler.MessageAr = "تم الإشتراك في المحاضره";
                                    _resultHandler.MessageEn = "You have been subscribed to " + _Session.TblSession.TblSubject.NameEn;

                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                                }
                                else if (_Params.Pending == true && _VirtualSub != null)
                                {
                                    _resultHandler.IsSuccessful = false;
                                    _resultHandler.MessageAr = "تم الإشتراك في المحاضره من قبل واضاقتها الي المحاضرات القادمه";
                                    _resultHandler.MessageEn = "You have been subscribed before";

                                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                                }
                            }
                        }
                        else
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "تم الإشتراك في المحاضره من قبل";
                            _resultHandler.MessageEn = "You have been subscribed before";

                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                        }
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "رقم الطالب غير صحيح او غير موجود";
                        _resultHandler.MessageEn = "Please provide a valid Student ID";

                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                    }
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "";
                    _resultHandler.MessageEn = "";

                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "أدخل رقم الكيو ار كود";
                    _resultHandler.MessageEn = "Please provide QRCode ID";

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
        public HttpResponseMessage GetPreviousSubscriptions(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                DateTime CurrentDate = DateTime.Now;
                //List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.Pending != true && a.Ended == true && a.IsDeleted != true).ToList();
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.Pending != true && a.IsDeleted != true /*|| (a.Ended == true)*/).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblEvaluation StudentEval = _Context.TblEvaluations.Where(a => a.StudentID == StudentID && a.TblSessionTime.SessionID == _SessionTimes.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        SessionID = int.Parse(item.SessionID.ToString()),
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        GeneralSession = item.TblSession.GeneralSession,
                        //SessionCost = _SessionDetails.Cost,
                        SessionCost = item.Price,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        SessionNameAr = item.TblSession.Name,
                        SessionNameEn = item.TblSession.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblSession.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblLecturer.ProfilePic),
                        CollegeNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        LecturesCount = item.TblSession.LecturesCount,
                        //StartDate = _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        StartDate = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                        Evaluated = StudentEval == null ? false : true,
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        DescriptionAr = item.TblSession.Description != null ? item.TblSession.Description : "",
                        DescriptionEn = item.TblSession.DescriptionEn != null ? item.TblSession.DescriptionEn : ""
                    };

                    List<TblEvaluation> Evaluations = _Context.TblEvaluations.Where(a => a.TblSessionTime.TblSession.LecturerID == item.TblSession.LecturerID).ToList();
                    if (Evaluations.Count > 0)
                    {
                        decimal v = (Evaluations.Sum(a => a.TblEvaluationQuestionAnswer.Value) / 24) * 100;
                    }
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
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.Pending != true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    //List<TblAttendance> _Attend = _Context.TblAttendances.Where(a => a.SessionID == item.SessionID && a.StudentID == item.StudentID && a.IsDeleted != true).ToList();
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        //ID = item.ID,
                        ID = int.Parse(item.SessionID.ToString()),
                        SessionID = int.Parse(item.SessionID.ToString()),
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        GeneralSession = item.TblSession.GeneralSession,
                        DescriptionAr = item.TblSession.Description != null ? item.TblSession.Description : "",
                        DescriptionEn = item.TblSession.DescriptionEn != null ? item.TblSession.DescriptionEn : "",
                        //SessionCost = _SessionDetails.Cost,
                        SessionCost = item.Price,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        SessionNameAr = item.TblSession.Name,
                        SessionNameEn = item.TblSession.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblSession.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblLecturer.ProfilePic),
                        CollegeNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameEn : "",
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
                        Evaluated = _Context.TblEvaluations.Where(a => a.StudentID == StudentID && a.TblSessionTime.SessionID == _SessionTimes.SessionID).FirstOrDefault() == null ? false : true
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
                DateTime CurrentDate = DateTime.Now.Date;
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => DbFunctions.TruncateTime(a.TblSession.ToDate) >= CurrentDate && a.StudentID == StudentID && a.Pending == true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _Subs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        SessionID = int.Parse(item.SessionID.ToString()),
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        GeneralSession = item.TblSession.GeneralSession,
                        //SessionCost = _SessionDetails.Cost,
                        //SessionCost = item.Price,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSession.TblSubject.Picture.StartsWith("http://") ? item.TblSession.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblSubject.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        DescriptionAr = item.TblSession.Description,
                        DescriptionEn = item.TblSession.DescriptionEn,
                        SessionNameAr = item.TblSession.Name,
                        SessionNameEn = item.TblSession.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        //LecturerPic = "http://smuapitest.smartmindkw.com" + item.TblSession.TblLecturer.ProfilePic,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblSession.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblLecturer.ProfilePic),
                        CollegeNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        LecturesCount = item.TblSession.LecturesCount,
                        //StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                        StartDate = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        Favourite = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.SessionID == item.SessionID).FirstOrDefault() == null ? false : true,
                        Subscribed = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.SessionID == item.SessionID).FirstOrDefault() == null ? false : true,
                        Evaluated = _Context.TblEvaluations.Where(a => a.StudentID == StudentID && a.TblSessionTime.SessionID == _SessionTimes.SessionID).FirstOrDefault() == null ? false : true
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

                    if (item.TblSession.Type)
                    {
                        int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == item.ID).Count();
                        //TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                        //TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
                        switch (subCount)
                        {
                            case 0:
                                data.SessionCost = (decimal)_SessionDetails.Price1;
                                break;
                            case 1:
                                data.SessionCost = (decimal)_SessionDetails.Price2;
                                break;
                            //case 2:
                            //    data.SessionCost = (decimal)_SessionDetail.Price3;
                            //    break;
                            default:
                                data.SessionCost = (decimal)_SessionDetails.Price3;
                                break;
                        }
                    }
                    else
                    {
                        if (item.TblSession.FromDate.Date >= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetails.Cost;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetails.SessionPrice;
                        }
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
                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<string> Data = new List<string>();

                foreach (var item in _Subs.Select(a => a.TblStudent))
                {
                    if (item.ProfilePic != null)
                    {
                        Data.Add(item.ProfilePic.StartsWith("http://") ? item.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.ProfilePic));
                    }
                    else
                    {
                        Data.Add("http://smuapitest.smartmindkw.com" + item.ProfilePic);
                    }
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
                //List<TblSession> _Sessions = _Context.TblSessions.Where(a => DbFunctions.TruncateTime(a.ToDate) >= CurrentDate && a.TblSubject.TblMajor.CollegeID == CollegeID && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<TblSession> _Sessions = _Context.TblSessions.Where(a => DbFunctions.TruncateTime(a.ToDate) >= CurrentDate && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                if (CollegeID == 0)
                {
                    _Sessions = _Context.TblSessions.Where(a => DbFunctions.TruncateTime(a.ToDate) >= CurrentDate && a.GeneralSession == true && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                }
                List<TimelineSessionData> Data = new List<TimelineSessionData>();

                foreach (var item in _Sessions)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    TimelineSessionData data = new TimelineSessionData()
                    {
                        ID = item.ID,
                        SessionID = item.ID,
                        Code = item.SessionCode,
                        HallCodeAr = item.TblHall != null ? item.TblHall.HallCodeAr : "",
                        HallCodeEn = item.TblHall != null ? item.TblHall.HallCodeEn : "",
                        SessionType = item.Type,
                        GeneralSession = item.GeneralSession,
                        //SessionCost = _SessionDetail.Cost,
                        SessionDescription = item.Description != null ? item.Description : "",
                        DescriptionAr = item.Description,
                        DescriptionEn = item.DescriptionEn,
                        //SubjectPicture = item.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SubjectPicture = item.TblSubject.Picture.StartsWith("http://") ? item.TblSubject.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSubject.Picture),
                        SubjectNameAr = item.SubjectID != null ? item.TblSubject.NameAr : "",
                        SubjectNameEn = item.SubjectID != null ? item.TblSubject.NameEn : "",
                        SessionNameAr = item.Name,
                        SessionNameEn = item.NameEn,
                        LecturerID = item.LecturerID,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn,
                        //LecturerPic = item.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic),
                        LecturerPic = item.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        LecturesCount = item.LecturesCount,
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        Favourite = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.SessionID == item.ID).FirstOrDefault() == null ? false : true,
                        Subscribed = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.SessionID == item.ID).FirstOrDefault() == null ? false : true,
                        Evaluated = _Context.TblEvaluations.Where(a => a.StudentID == StudentID && a.TblSessionTime.SessionID == _SessionTimes.SessionID).FirstOrDefault() == null ? false : true
                        //Evaluated = item == null ? false : true
                    };
                    //if(item.Type == false && _SessionTimes.FromTime.Date < DateTime.Now.Date)
                    //{

                    //}
                    if (item.TblSubject.MajorID != null)
                    {
                        data.CollegeNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "";
                        data.CollegeNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "";
                        data.MajorNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameAr : "";
                        data.MajorNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameEn : "";
                    }
                    else
                    {
                        data.CollegeNameAr = "";
                        data.CollegeNameEn = "";
                        data.MajorNameAr = "";
                        data.MajorNameEn = "";
                    }
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

                    if (item.Type)
                    {
                        int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == item.ID && a.Pending != true).Count();
                        //TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                        //TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
                        switch (subCount)
                        {
                            case 0:
                                data.SessionCost = (decimal)_SessionDetail.Price1;
                                break;
                            case 1:
                                data.SessionCost = (decimal)_SessionDetail.Price2;
                                break;
                            //case 2:
                            //    data.SessionCost = (decimal)_SessionDetail.Price3;
                            //    break;
                            default:
                                data.SessionCost = (decimal)_SessionDetail.Price3;
                                break;
                        }
                    }
                    else
                    {
                        if (item.FromDate.Date >= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetail.Cost;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetail.SessionPrice;
                        }
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
                TblSession _SessionObj = _Context.TblSessions.Where(a => a.ID == _Params.SessionID).FirstOrDefault();
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

                        string TitleAr = "سمارت مايند الجامعه";
                        string TitleEn = "SmartMind University";
                        string DescriptionAr = "تم إضافة محاضره " + _SessionObj.Name + " الي المفضله للمحاضر : " + (_SessionObj.TblLecturer.FirstNameAr + " " + _SessionObj.TblLecturer.SecondNameAr + " " + _SessionObj.TblLecturer.ThirdNameAr);
                        string DescriptionEn = "Lecture " + _FavObj.TblSession.NameEn + " have been added to your favorites for the lecturer : " + (_SessionObj.TblLecturer.FirstNameEn + " " + _SessionObj.TblLecturer.SecondNameEn + " " + _SessionObj.TblLecturer.ThirdNameEn);

                        Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

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

                    if(_CurrentFav!= null)
                    {
                        _Context.TblFavoriteSessions.Remove(_CurrentFav);
                        _Context.SaveChanges();

                        string TitleAr = "سمارت مايند الجامعه";
                        string TitleEn = "SmartMind University";
                        string DescriptionAr = "تم حذف محاضره " + _SessionObj.Name + " من المفضله للمحاضر : " + (_SessionObj.TblLecturer.FirstNameAr + " " + _SessionObj.TblLecturer.SecondNameAr + " " + _SessionObj.TblLecturer.ThirdNameAr);
                        string DescriptionEn = "Lecture " + _SessionObj.NameEn + " have been removed from your favorites for the lecturer : " + (_SessionObj.TblLecturer.FirstNameEn + " " + _SessionObj.TblLecturer.SecondNameEn + " " + _SessionObj.TblLecturer.ThirdNameEn);

                        Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

                        _resultHandler.IsSuccessful = true;
                        _resultHandler.MessageAr = "تم حذف المحاضره من المفضله";
                        _resultHandler.MessageEn = "Session has been removed from your favourites";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = true;
                        _resultHandler.MessageAr = "المحاضره غير موجوده في المفضله=";
                        _resultHandler.MessageEn = "Session is not exist in favourite list";

                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                    }
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
                List<TblFavoriteSession> _FavObjs = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
                List<SubscriptionData> Data = new List<SubscriptionData>();

                foreach (var item in _FavObjs)
                {
                    TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == item.SessionID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

                    SubscriptionData data = new SubscriptionData()
                    {
                        ID = item.ID,
                        SessionID = item.SessionID,
                        Code = item.TblSession.SessionCode,
                        SessionType = item.TblSession.Type,
                        GeneralSession = item.TblSession.GeneralSession,
                        //SessionCost = _SessionDetails.Cost,
                        CollegeNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSession.TblSubject.MajorID != null ? item.TblSession.TblSubject.TblMajor.NameEn : "",
                        HallCodeAr = item.TblSession.TblHall.HallCodeAr,
                        HallCodeEn = item.TblSession.TblHall.HallCodeEn,
                        //SubjectPicture = item.TblSession.TblSubject.Picture,
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        //SessionPicture = item.TblSession.Picture.StartsWith("http://") ? item.TblSession.Picture : ("http://smuapitest.smartmindkw.com" + item.TblSession.Picture),
                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
                        SessionNameAr = item.TblSession.Name,
                        SessionNameEn = item.TblSession.NameEn,
                        LecturerID = item.TblSession.LecturerID,
                        LecturerName = item.TblSession.TblLecturer.FirstNameAr + " " + item.TblSession.TblLecturer.SecondNameAr + " " + item.TblSession.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblSession.TblLecturer.FirstNameEn + " " + item.TblSession.TblLecturer.SecondNameEn + " " + item.TblSession.TblLecturer.ThirdNameEn,
                        //LecturerPic = "http://smuapitest.smartmindkw.com" + item.TblSession.TblLecturer.ProfilePic,
                        LecturerPic = item.TblSession.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblSession.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblSession.TblLecturer.ProfilePic),
                        LecturesCount = item.TblSession.LecturesCount,
                        StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
                        Time = item.TblSession.Type == true ? item.TblSession.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //Time = _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        Favourite = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.SessionID == item.SessionID).FirstOrDefault() == null ? false : true,
                        Subscribed = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.SessionID == item.SessionID).FirstOrDefault() == null ? false : true,
                        Evaluated = _Context.TblEvaluations.Where(a => a.StudentID == StudentID && a.TblSessionTime.SessionID == _SessionTimes.SessionID).FirstOrDefault() == null ? false : true
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
                    //if (item.TblSession.Type)
                    //{
                    //    data.SessionCost = (decimal)_SessionDetails.Price1;
                    //}
                    //else
                    //{
                    //    if (item.TblSession.FromDate.Date <= DateTime.Now.Date)
                    //    {
                    //        data.SessionCost = (decimal)_SessionDetails.SessionPrice;
                    //    }
                    //    else
                    //    {
                    //        data.SessionCost = (decimal)_SessionDetails.Cost;
                    //    }
                    //}
                    if (item.TblSession.Type)
                    {
                        int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == item.SessionID && a.Pending != true).Count();
                        //TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                        //TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
                        switch (subCount)
                        {
                            case 0:
                                data.SessionCost = (decimal)_SessionDetails.Price1;
                                break;
                            case 1:
                                data.SessionCost = (decimal)_SessionDetails.Price2;
                                break;
                            //case 2:
                            //    data.SessionCost = (decimal)_SessionDetail.Price3;
                            //    break;
                            default:
                                data.SessionCost = (decimal)_SessionDetails.Price3;
                                break;
                        }
                    }
                    else
                    {
                        if (item.TblSession.FromDate.Date >= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetails.Cost;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetails.SessionPrice;
                        }
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
                        //SessionID = _Params.SessionID,
                        SessionTimesID = _Params.SessionTimesID,
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
        public HttpResponseMessage GetEvaluation(int LecturerID, int SessionTimesID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.ID == SessionTimesID).FirstOrDefault();
                //List<TblEvaluation> _Evaluation = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == LecturerID).DistinctBy(a => a.SessionID).OrderByDescending(a => a.ID).ToList();
                List<TblEvaluation> _Evaluation = _Context.TblEvaluations.Where(a => a.TblSessionTime.TblSession.LecturerID == LecturerID && a.SessionTimesID == SessionTimesID).OrderByDescending(a => a.ID).ToList();
                EvaluationListData Data = new EvaluationListData();
                List<StudentEvaluationData> EvalData = new List<StudentEvaluationData>();

                Data.LecturerEvalData = new LecturerEvaluationData()
                {
                    LecturerNameAr = _SessionTimes.TblSession.TblLecturer.FirstNameAr + " " + _SessionTimes.TblSession.TblLecturer.SecondNameAr + " " + _SessionTimes.TblSession.TblLecturer.ThirdNameAr,
                    LecturerNameEn = _SessionTimes.TblSession.TblLecturer.FirstNameEn + " " + _SessionTimes.TblSession.TblLecturer.SecondNameEn + " " + _SessionTimes.TblSession.TblLecturer.ThirdNameEn,
                    LecturerPicture = _SessionTimes.TblSession.TblLecturer.ProfilePic.StartsWith("http://") ? _SessionTimes.TblSession.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + _SessionTimes.TblSession.TblLecturer.ProfilePic),
                    SubjectNameAr = "محاضر " + _SessionTimes.TblSession.TblSubject.NameAr,
                    SubjectNameEn = _SessionTimes.TblSession.NameEn + " Instructor",
                    SessionNameAr = _SessionTimes.TblSession.Name,
                    SessionNameEn = _SessionTimes.TblSession.NameEn,
                    LecturerRate = 4
                };

                //if (_Evaluation.Count() > 0)
                //{
                //    Data.LecturerEvalData = new LecturerEvaluationData()
                //    {
                //        LecturerNameAr = LecturerObj.FirstNameAr + " " + LecturerObj.SecondNameAr + " " + LecturerObj.ThirdNameAr,
                //        LecturerNameEn = _Evaluation.FirstOrDefault().TblSession.TblLecturer.FirstNameEn + " " + _Evaluation.FirstOrDefault().TblSession.TblLecturer.SecondNameEn + " " + _Evaluation.FirstOrDefault().TblSession.TblLecturer.ThirdNameEn,
                //        SubjectNameAr = "محاضر " + _Evaluation.FirstOrDefault().TblSession.TblSubject.NameAr,
                //        SubjectNameEn = _Evaluation.FirstOrDefault().TblSession.TblSubject.NameEn + " Instructor",
                //        SessionNameAr = _Evaluation.FirstOrDefault().TblSession.Name,
                //        SessionNameEn = _Evaluation.FirstOrDefault().TblSession.NameEn,
                //        LecturerRate = 4
                //    };
                //}

                foreach (var item in _Evaluation)
                {
                    //TblEvaluation sigleEval = _Context.TblEvaluations.Where(a=>a.SessionID==item.SessionID)
                    StudentEvaluationData data = new StudentEvaluationData()
                    {

                        StudentName = item.TblStudent.FirstName + " " + item.TblStudent.SecondName + " " + item.TblStudent.ThirdName,
                        ProfilePic = item.TblStudent.ProfilePic.StartsWith("https://") ? item.TblStudent.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblStudent.ProfilePic),
                        StudentRate = 3,
                        CollageAr = item.TblStudent.TblCollege.NameAr,
                        CollageEn = item.TblStudent.TblCollege.NameEn,
                        Evaluation = item.EvaluationNotes,
                        Time = "منذ ساعتين",
                        Rate = 3.5
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

        //[HttpPost]
        //public HttpResponseMessage GetSessionTimes(StudentSessionObj _Params)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        TblSubscription Sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
        //        if (Sub != null)
        //        {
        //            TblSession _SessionObj = _Context.TblSessions.Where(a => a.ID == _Params.SessionID).SingleOrDefault();
        //            if (_SessionObj != null)
        //            {
        //                List<TblSessionTime> _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == _Params.SessionID).ToList();
        //                List<SessionTimesData> _Data = new List<SessionTimesData>();

        //                foreach (var item in _SessionTimes)
        //                {
        //                    SessionTimesData data = new SessionTimesData()
        //                    {
        //                        ID = item.ID,
        //                        LectureAr = item.LectureAr,
        //                        LectureEn = item.LectureEn,
        //                        Date = item.FromTime.ToString("yyyy-MM-dd"),
        //                        Time = item.FromTime.ToString("hh:mm tt"),
        //                    };

        //                    if (_Params.StudentID > 0)//for student screen, else >> for Lecturer screen
        //                    {
        //                        TblAttendance _Attendance = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.StudentID == _Params.StudentID).FirstOrDefault();
        //                        data.Attend = _Attendance == null ? 0 : _Attendance.Attend;
        //                    }
        //                    else
        //                    {
        //                        data.AttendanceCount = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.Attend == 1).Count();
        //                        data.AbsenceCount = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.Attend == 2).Count();
        //                    }

        //                    _Data.Add(data);
        //                }

        //                _resultHandler.IsSuccessful = true;
        //                _resultHandler.Result = _Data;
        //                _resultHandler.MessageAr = "OK";
        //                _resultHandler.MessageEn = "OK";

        //                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
        //            }
        //            else
        //            {
        //                _resultHandler.IsSuccessful = false;
        //                _resultHandler.MessageAr = "السيشن غير موجوده";
        //                _resultHandler.MessageEn = "Session id not found";

        //                return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
        //            }
        //        }
        //        else
        //        {
        //            _resultHandler.IsSuccessful = false;
        //            _resultHandler.MessageAr = "الطالب غير مشترك في المحاضره";
        //            _resultHandler.MessageEn = "Student is not subscribed to this session";

        //            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _resultHandler.IsSuccessful = false;
        //        _resultHandler.MessageAr = ex.Message;
        //        _resultHandler.MessageEn = ex.Message;

        //        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
        //    }
        //}

        [HttpPost]
        public HttpResponseMessage GetSessionTimes(StudentSessionObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblSession _SessionObj = _Context.TblSessions.Where(a => a.ID == _Params.SessionID).SingleOrDefault();
                if (_SessionObj != null)
                {
                    List<TblSessionTime> _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == _Params.SessionID).ToList();
                    List<SessionTimesData> _Data = new List<SessionTimesData>();

                    foreach (var item in _SessionTimes)
                    {
                        SessionTimesData data = new SessionTimesData()
                        {
                            ID = item.ID,
                            LectureAr = item.LectureAr,
                            LectureEn = item.LectureEn,
                            Date = item.FromTime.ToString("yyyy-MM-dd"),
                            Time = item.FromTime.ToString("hh:mm tt"),
                        };

                        if (_Params.StudentID > 0)//for student screen, else >> for Lecturer screen
                        {
                            TblAttendance _Attendance = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.StudentID == _Params.StudentID).FirstOrDefault();
                            data.Attend = _Attendance == null ? 0 : _Attendance.Attend;
                        }
                        else
                        {
                            data.AttendanceCount = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.Attend == 1).Count();
                            data.AbsenceCount = _Context.TblAttendances.Where(a => a.SessionTimesID == item.ID && a.Attend == 2).Count();
                        }

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

                    string TitleAr = "سمارت مايند الجامعه";
                    string TitleEn = "SmartMind University";
                    string DescriptionAr = "تم إرسال طلبك بنجاح وسيتم التواصل معك من قبل المعهد في اقرب وقت ممكن لنحديد موعد المحاضره";
                    string DescriptionEn = "Your request has been submitted, the administration will back to you as oosn as possible to arrange the session with you";

                    Push(_Params.StudentID, 0, TitleAr, TitleEn, DescriptionAr, DescriptionEn, 0);

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
                        GeneralSession = _Session.GeneralSession,
                        LecturesCount = _Session.LecturesCount,
                        CollegeNameAr = _Session.TblSubject.MajorID != null ? _Session.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = _Session.TblSubject.MajorID != null ? _Session.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = _Session.TblSubject.MajorID != null ? _Session.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = _Session.TblSubject.MajorID != null ? _Session.TblSubject.TblMajor.NameEn : "",
                        StartDate = _Session.Type == true ? _Session.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = _Session.Type == true ? _Session.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        SubjectNameAr = _Session.SubjectID != null ? _Session.TblSubject.NameAr : "",
                        SubjectNameEn = _Session.SubjectID != null ? _Session.TblSubject.NameEn : "",
                        SessionNameAr = _Session.Name,
                        SessionNameEn = _Session.NameEn,
                        //SessionCost = _SessionDetail.Cost,
                        LecturerID = _Session.LecturerID,
                        LecturerName = _Session.TblLecturer.FirstNameAr + " " + _Session.TblLecturer.SecondNameAr + " " + _Session.TblLecturer.ThirdNameAr,
                        LecturerNameEn = _Session.TblLecturer.FirstNameEn + " " + _Session.TblLecturer.SecondNameEn + " " + _Session.TblLecturer.ThirdNameEn,
                        LecturerPic = _Session.TblLecturer.ProfilePic.StartsWith("http://") ? _Session.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + _Session.TblLecturer.ProfilePic),
                        //LecturerPic = _Session.TblSubject.TblLecturer.ProfilePic,
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        QRCode = DateTime.Now > _Session.FromDate ? "http://smuapitest.smartmindkw.com/" + _SessionDetail.QRCode : "http://smuapitest.smartmindkw.com/" + _SessionDetail.SingleSessionQRCode,
                        IsCourse = DateTime.Now > _Session.FromDate ? false : true,

                    };
                    if (_Session.Picture != null)
                    {
                        Data.SessionPicture = _Session.Picture.StartsWith("http://") ? _Session.Picture : ("http://smuapitest.smartmindkw.com" + _Session.Picture);
                    }
                    else
                    {
                        Data.SessionPicture = "http://smuapitest.smartmindkw.com" + _Session.Picture;
                    }
                    if (_Session.Type)
                    {
                        Data.SessionCost = (decimal)_SessionDetail.Price1;
                    }
                    else
                    {
                        if (_Session.FromDate.Date <= DateTime.Now.Date)
                        {
                            Data.SessionCost = (decimal)_SessionDetail.SessionPrice;
                        }
                        else
                        {
                            Data.SessionCost = (decimal)_SessionDetail.Cost;
                        }
                    }
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
                //List<TblSession> _SessionsList = _Context.TblSessions.Where(a => a.HallID == HallID && DbFunctions.TruncateTime(a.FromDate) == CurrentDate).ToList();//for test, uncomment the above line
                List<TblSession> _SessionsList = _Context.TblSessions.Where(a => a.HallID == HallID && DbFunctions.TruncateTime(a.FromDate) == CurrentDate && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();//for test, uncomment the above line
                List<SessionCourseData> Data = new List<SessionCourseData>();

                foreach (var item in _SessionsList)
                {
                    TblSessionDetail _SessionDetail = _Context.TblSessionDetails.Where(a => a.SessionID == item.ID).FirstOrDefault();
                    TblSessionTime _SessionTimes = _Context.TblSessionTimes.Where(a => a.SessionID == item.ID).FirstOrDefault();

                    SessionCourseData data = new SessionCourseData()
                    {
                        ID = item.ID,
                        SessionType = item.Type,
                        GeneralSession = item.GeneralSession,
                        SessionCode = item.SessionCode,
                        LecturesCount = item.LecturesCount,
                        //CollegeNameAr = item.SubjectID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameAr : "",
                        CollegeNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.TblCollege.NameEn : "",
                        MajorNameAr = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameAr : "",
                        MajorNameEn = item.TblSubject.MajorID != null ? item.TblSubject.TblMajor.NameEn : "",
                        StartDate = item.Type == true ? item.FromDate.ToString("yyyy-MM-dd") : _SessionTimes.FromTime.ToString("yyyy-MM-dd"),
                        Time = item.Type == true ? item.FromDate.ToString("hh:mm tt") : _SessionTimes.FromTime.ToString("hh:mm tt"),
                        //SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
                        SubjectNameAr = item.SubjectID != null ? item.TblSubject.NameAr : "",
                        SubjectNameEn = item.SubjectID != null ? item.TblSubject.NameEn : "",
                        SessionNameAr = item.Name,
                        SessionNameEn = item.NameEn,
                        //SessionCost = _SessionDetail.Cost,
                        LecturerID = item.LecturerID,
                        LecturerName = item.TblLecturer.FirstNameAr + " " + item.TblLecturer.SecondNameAr + " " + item.TblLecturer.ThirdNameAr,
                        LecturerNameEn = item.TblLecturer.FirstNameEn + " " + item.TblLecturer.SecondNameEn + " " + item.TblLecturer.ThirdNameEn,
                        LecturerPic = item.TblLecturer.ProfilePic.StartsWith("http://") ? item.TblLecturer.ProfilePic : ("http://smuapitest.smartmindkw.com" + item.TblLecturer.ProfilePic),
                        //LecturerPic = item.TblSubject.TblLecturer.ProfilePic,
                        //LecturerRate = _Context.TblEvaluations.Where(a => a.TblSession.LecturerID == item.ID).GroupBy(a => a.TblEvaluationQuestionAnswer.Value).OrderByDescending(a => a.Key).Max(a => a.Key),
                        LecturerRate = 4.5,
                        QRCode = DateTime.Now > item.FromDate ? "http://smuapitest.smartmindkw.com" + _SessionDetail.QRCode : "http://smuapitest.smartmindkw.com" + _SessionDetail.SingleSessionQRCode,
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
                    if (item.Picture != null)
                    {
                        data.SessionPicture = item.Picture.StartsWith("http://") ? item.Picture : ("http://smuapitest.smartmindkw.com" + item.Picture);
                    }
                    else
                    {
                        data.SessionPicture = "http://smuapitest.smartmindkw.com" + item.Picture;
                    }
                    //if (item.Type)
                    //{
                    //    data.SessionCost = (decimal)_SessionDetail.Price1;
                    //}
                    //else
                    //{
                    //    if (item.FromDate.Date >= DateTime.Now.Date)
                    //    {
                    //        data.SessionCost = (decimal)_SessionDetail.Cost;
                    //    }
                    //    else
                    //    {
                    //        data.SessionCost = (decimal)_SessionDetail.SessionPrice;
                    //    }
                    //}

                    if (item.Type)
                    {
                        int subCount = _Context.TblSubscriptions.Where(a => a.SessionID == item.ID && a.Pending != true).Count();
                        //TblSubscription firstSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
                        //TblSubscription secondSub = _Context.TblSubscriptions.Where(a => a.SessionID == _Params.SessionID && a.PriceType == 2).FirstOrDefault();
                        switch (subCount)
                        {
                            case 0:
                                data.SessionCost = (decimal)_SessionDetail.Price1;
                                break;
                            case 1:
                                data.SessionCost = (decimal)_SessionDetail.Price2;
                                break;
                            //case 2:
                            //    data.SessionCost = (decimal)_SessionDetail.Price3;
                            //    break;
                            default:
                                data.SessionCost = (decimal)_SessionDetail.Price3;
                                break;
                        }
                    }
                    else
                    {
                        if (item.FromDate.Date >= DateTime.Now.Date)
                        {
                            data.SessionCost = (decimal)_SessionDetail.Cost;
                        }
                        else
                        {
                            data.SessionCost = (decimal)_SessionDetail.SessionPrice;
                        }
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
                List<TblPrivateSession> _PrivateObjs = _Context.TblPrivateSessions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).OrderByDescending(a => a.ID).ToList();
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