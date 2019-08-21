//using SMUModels;
//using SMUModels.ObjectData;
//using SMUniversity.WebAPI.Classes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace SMUniversity.WebAPI.Controllers
//{
//    public class SessionController : ApiController
//    {
//        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

//        [HttpPost]
//        public HttpResponseMessage EditSessionDescription(SessionDescriptionObj _Params)
//        {
//            ResultHandler _resultHandler = new ResultHandler();

//            try
//            {
//                TblSession _Session = _Context.TblSessions.Where(a => a.ID == _Params.SessionID && a.IsDeleted != true).SingleOrDefault();
//                if (_Session != null)
//                {
//                    _Session.Description = _Params.Description;
//                    _Context.SaveChanges();

//                    _resultHandler.IsSuccessful = true;
//                    _resultHandler.MessageAr = "OK";
//                    _resultHandler.MessageEn = "OK";

//                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = "هذه المحاضره غير موجوده";
//                    _resultHandler.MessageEn = "Session is not found";

//                    return Request.CreateResponse(HttpStatusCode.NotFound, _resultHandler);
//                }
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpGet]
//        public HttpResponseMessage GetQRCodeData(string QRCodeProps)
//        {
//            ResultHandler _resultHandler = new ResultHandler();

//            try
//            {
//                if (!string.IsNullOrEmpty(QRCodeProps))
//                {
//                    string[] SplittedResutl = QRCodeProps.Split(',');
//                    if (SplittedResutl.Length > 0)
//                    {
//                        SessionData Data = null;
//                        if (!string.IsNullOrEmpty(SplittedResutl[0]) && !string.IsNullOrEmpty(SplittedResutl[1]))
//                        {
//                            int SessionPeriodID = int.Parse(SplittedResutl[0]);
//                            int SessionType = int.Parse(SplittedResutl[1]);

//                            TblSessionPeriod _SessionPeriod = _Context.TblSessionPeriods.Where(a => a.ID == SessionPeriodID).FirstOrDefault();

//                            Data = new SessionData()
//                            {
//                                SessionID = _SessionPeriod.SessionID,
//                                Type = _SessionPeriod.TblSession.Type,
//                                SubjectNameAr = _SessionPeriod.TblSession.TblSubject.NameAr,
//                                SubjectNameEn = _SessionPeriod.TblSession.TblSubject.NameEn,
//                                Lecturer = _SessionPeriod.TblSession.TblLecturer.FirstName + " " + _SessionPeriod.TblSession.TblLecturer.SecondName + " " + _SessionPeriod.TblSession.TblLecturer.ThirdName,
//                                //SubjectPicture = _SessionPeriod.TblSession.TblSubject.Picture,
//                                SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
//                                Cost = decimal.Parse(_SessionPeriod.Cost.ToString())
//                            };
//                        }

//                        _resultHandler.IsSuccessful = true;
//                        _resultHandler.Result = Data;
//                        _resultHandler.MessageAr = "OK";
//                        _resultHandler.MessageEn = "OK";

//                        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//                    }
//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = "رقم المحاضره غير صحيح";
//                    _resultHandler.MessageEn = "Session ID is empty";

//                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//                }
                
//                _resultHandler.IsSuccessful = true;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpPost]
//        public HttpResponseMessage SubscripetoSession(SubscriptionObj _Params)
//        {
//            ResultHandler _resultHandler = new ResultHandler();

//            try
//            {
//                if (_Params.SessionID > 0 && _Params.StudentID > 0)
//                {
//                    TblSubscription sub = _Context.TblSubscriptions.Where(a => a.StudentID == _Params.StudentID && a.SessionID == _Params.SessionID).FirstOrDefault();
//                    if (sub == null)
//                    {
//                        TblSessionPeriod _Session = _Context.TblSessionPeriods.Where(a => a.SessionID == _Params.SessionID).FirstOrDefault();
//                        TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();

//                        if (_Student != null)
//                        {
//                            TblSubscription _Sub = new TblSubscription()
//                            {
//                                SessionID = _Params.SessionID,
//                                StudentID = _Params.StudentID,
//                                IsDeleted = false,
//                                CreatedDate = DateTime.Now
//                            };

//                            _Context.TblSubscriptions.Add(_Sub);

//                            if (!_Params.Pending)
//                            {
//                                if (_Params.Pending == false && _Student.Balance >= _Session.Cost)
//                                {
//                                    _Sub.Pending = false;

//                                    TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
//                                    {
//                                        StudentID = _Params.StudentID,
//                                        Price = _Session.Cost,
//                                        Pending = false,
//                                        IsDeleted = false,
//                                        PaymentMethod = "Cash",
//                                        TransactionTypeID = 2,
//                                        TitleAr = "اشتراك في محاضرة : " + _Session.TblSession.TblSubject.NameAr,
//                                        TitleEn = "Subscription to subject : " + _Session.TblSession.TblSubject.NameEn,
//                                        CreatedDate = DateTime.Now,
//                                    };

//                                    _Context.TblBalanceTransactions.Add(_BalanceTrans);

//                                    _Student.Balance -= _Session.Cost;
//                                    _Context.SaveChanges();

//                                    _resultHandler.IsSuccessful = true;
//                                    _resultHandler.MessageAr = "تم الإشتراك في محاضره " + _Session.TblSession.TblSubject.NameAr + " وخصم مبلغ " + _Session.Cost + " من رصيدك";
//                                    _resultHandler.MessageEn = "You have been subscribed to " + _Session.TblSession.TblSubject.NameEn + ", and a balance of " + _Session.Cost + " has been deducted";

//                                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//                                }
//                                else
//                                {
//                                    _resultHandler.IsSuccessful = false;
//                                    _resultHandler.MessageAr = "رصيدك غير كافي للاشتراك في المحاضره, اشحن ثم حاول مره اخري";
//                                    _resultHandler.MessageEn = "You have not enough balance to subscripe to lecturer, Please recharge and try again";

//                                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//                                }
//                            }
//                            else
//                            {
//                                _Sub.Pending = true;
//                                _Context.SaveChanges();
//                            }
//                        }
//                        else
//                        {
//                            _resultHandler.IsSuccessful = false;
//                            _resultHandler.MessageAr = "رقم الطالب غير صحيح او غير موجود";
//                            _resultHandler.MessageEn = "Please provide a valid Student ID";

//                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//                        }
//                    }
//                    else
//                    {
//                        _resultHandler.IsSuccessful = false;
//                        _resultHandler.MessageAr = "تم الإشتراك في هذه المحاضره من قبل";
//                        _resultHandler.MessageEn = "This session hass been subscribed before";

//                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//                    }
                    
                    
//                }
//                else
//                {
//                    _resultHandler.IsSuccessful = false;
//                    _resultHandler.MessageAr = "أدخل رقم الكيو ار كود";
//                    _resultHandler.MessageEn = "Please provide QRCode ID";

//                    return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//                }

//                _resultHandler.IsSuccessful = true;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        //[HttpGet]
//        //public HttpResponseMessage GetStudentSubscriptions(int StudentID)
//        //{
//        //    ResultHandler _resultHandler = new ResultHandler();
//        //    try
//        //    {
//        //        List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).ToList();
//        //        List<SubscriptionData> Data = new List<SubscriptionData>();

//        //        foreach (var item in _Subs)
//        //        {
//        //            List<TblAttendance> _Attend = _Context.TblAttendances.Where(a => a.SessionID == item.SessionID && a.StudentID == item.StudentID && a.IsDeleted != true).ToList();
//        //            TblSessionPeriod _SessionPeriod = _Context.TblSessionPeriods.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

//        //            SubscriptionData data = new SubscriptionData()
//        //            {
//        //                ID = item.ID,
//        //                Ended = item.Ended,
//        //                Pending = item.Pending,
//        //                SessionType = item.TblSession.Type,
//        //                SessionCost = _SessionPeriod.Cost,
//        //                SubjectPicture = item.TblSession.TblSubject.Picture,
//        //                SubjectNameAr = item.TblSession.TblSubject.NameAr,
//        //                SubjectNameEn = item.TblSession.TblSubject.NameEn,
//        //                HallCode = item.TblSession.TblHall.HallCode,
//        //                LecturesCount = item.TblSession.LecturesCount,
//        //                Attendance = _Attend.Where(a => a.Attend == true).Count(),
//        //                Absence = _Attend.Where(a => a.Attend == false).Count(),
//        //                LecturesLeft = item.TblSession.LecturesCount - _Attend.Where(a => a.Attend == false).Count(),
//        //                StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
//        //                Time = _SessionPeriod.FromTime.ToString("hh:mm tt"),
//        //            };

//        //            Data.Add(data);
//        //        }
//        //        _resultHandler.IsSuccessful = true;
//        //        _resultHandler.Result = Data;
//        //        _resultHandler.MessageAr = "OK";
//        //        _resultHandler.MessageEn = "OK";

//        //        return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        _resultHandler.IsSuccessful = false;
//        //        _resultHandler.MessageAr = ex.Message;
//        //        _resultHandler.MessageEn = ex.Message;

//        //        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//        //    }
//        //}

//        [HttpGet]
//        public HttpResponseMessage GetPendedSubsCount(int SessionID)
//        {
//            ResultHandler _resultHandler = new ResultHandler();
//            try
//            {
//                List<TblSubscription> _Subs = _Context.TblSubscriptions.Where(a => a.SessionID == SessionID && a.Pending == true && a.IsDeleted != true).ToList();
//                List<string> Data = new List<string>();

//                foreach (var item in _Subs.Select(a => a.TblStudent))
//                {
//                    Data.Add(item.ProfilePic);
//                }

//                _resultHandler.IsSuccessful = true;
//                _resultHandler.Result = Data;
//                _resultHandler.Count = _Subs.Count;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpGet]
//        public HttpResponseMessage GetTimelineSessions(int CollegeID)
//        {
//            ResultHandler _resultHandler = new ResultHandler();
//            try
//            {
//                List<TblSession> _Sessions = _Context.TblSessions.Where(a => a.TblSubject.TblMajor.CollegeID == CollegeID && a.TblSubject.TblMajor.IsDeleted != true && a.IsDeleted != true).ToList();
//                List<TimelineSessionData> Data = new List<TimelineSessionData>();

//                foreach (var item in _Sessions)
//                {
//                    TblSessionPeriod _SessionPeriod = _Context.TblSessionPeriods.Where(a => a.SessionID == item.ID).FirstOrDefault();

//                    TimelineSessionData data = new TimelineSessionData()
//                    {
//                        ID = item.ID,
//                        CollegeNameAr = item.TblSubject.TblMajor.TblCollege.NameAr,
//                        CollegeNameEn = item.TblSubject.TblMajor.TblCollege.NameEn,
//                        MajorNameAr = item.TblSubject.TblMajor.NameAr,
//                        MajorNameEn = item.TblSubject.TblMajor.NameEn,
//                        SessionType = item.Type,
//                        SessionCost = _SessionPeriod.Cost,
//                        //SubjectPicture = item.TblSubject.Picture,
//                        SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
//                        SubjectNameAr = item.TblSubject.NameAr,
//                        SubjectNameEn = item.TblSubject.NameEn,
//                        HallCode = item.TblHall.HallCode,
//                        LecturesCount = item.LecturesCount,
//                        StartDate = item.FromDate.ToString("yyyy-MM-dd"),
//                        Time = _SessionPeriod.FromTime.ToString("hh:mm tt"),
//                    };

//                    Data.Add(data);
//                }
//                _resultHandler.IsSuccessful = true;
//                _resultHandler.Result = Data;
//                _resultHandler.Count = Data.Count;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpPost]
//        public HttpResponseMessage AddToFavorite(SubscriptionObj _Params)
//        {
//            ResultHandler _resultHandler = new ResultHandler();
//            try
//            {
//                TblFavoriteSession _FavObj = new TblFavoriteSession()
//                {
//                    SessionID = _Params.SessionID,
//                    StudentID = _Params.StudentID,
//                    IsDeleted = false,
//                    CreatedDate = DateTime.Now
//                };

//                _Context.TblFavoriteSessions.Add(_FavObj);
//                _Context.SaveChanges();

//                _resultHandler.IsSuccessful = true;
//                _resultHandler.MessageAr = "تم إضافه المحاضره الي المفضله";
//                _resultHandler.MessageEn = "Session has been added to your favorite list";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpGet]
//        public HttpResponseMessage GetFavorites(int StudentID)
//        {
//            ResultHandler _resultHandler = new ResultHandler();
//            try
//            {
//                List<TblFavoriteSession> _FavObjs = _Context.TblFavoriteSessions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).ToList();
//                List<SubscriptionData> Data = new List<SubscriptionData>();

//                foreach (var item in _FavObjs)
//                {
//                    List<TblAttendance> _Attend = _Context.TblAttendances.Where(a => a.SessionID == item.SessionID && a.StudentID == item.StudentID && a.IsDeleted != true).ToList();
//                    TblSessionPeriod _SessionPeriod = _Context.TblSessionPeriods.Where(a => a.SessionID == item.SessionID).FirstOrDefault();

//                    SubscriptionData data = new SubscriptionData()
//                    {
//                        ID = item.ID,
//                        SessionType = item.TblSession.Type,
//                        SessionCost = _SessionPeriod.Cost,
//                        //SubjectPicture = item.TblSession.TblSubject.Picture,
//                        SubjectPicture = "http://smuapi.smartmindkw.com/content/images/photo.jpg",
//                        SubjectNameAr = item.TblSession.TblSubject.NameAr,
//                        SubjectNameEn = item.TblSession.TblSubject.NameEn,
//                        HallCode = item.TblSession.TblHall.HallCode,
//                        LecturesCount = item.TblSession.LecturesCount,
//                        StartDate = item.TblSession.FromDate.ToString("yyyy-MM-dd"),
//                        Time = _SessionPeriod.FromTime.ToString("hh:mm tt"),
//                    };

//                    Data.Add(data);
//                }
//                _resultHandler.IsSuccessful = true;
//                _resultHandler.Result = Data;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpGet]
//        public HttpResponseMessage GetEvaluationQues()
//        {
//            ResultHandler _resultHandler = new ResultHandler();

//            try
//            {
//                List<TblEvaluationQuestion> _ListOfAnswers = _Context.TblEvaluationQuestions.Where(a => a.IsDeleted != true).ToList();
//                List<EvalQuesData> Data = new List<EvalQuesData>();
//                foreach (var ques in _ListOfAnswers)
//                {
//                    EvalQuesData data = new EvalQuesData()
//                    {
//                        QuestionID = ques.ID,
//                        QuestionAr = ques.QuestionAr,
//                        QuestionEn = ques.QuestionEn
//                    };

//                    Data.Add(data);
//                }

//                _resultHandler.IsSuccessful = true;
//                _resultHandler.Result = Data;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }

//        [HttpGet]
//        public HttpResponseMessage GetQuesStaticAnswers()
//        {
//            ResultHandler _resultHandler = new ResultHandler();

//            try
//            {
//                List<TblEvaluationQuestionAnswer> _ListOfAnswers = _Context.TblEvaluationQuestionAnswers.Where(a => a.IsDeleted != true).ToList();
//                List<EvalAnsData> Data = new List<EvalAnsData>();
//                foreach (var answer in _ListOfAnswers)
//                {
//                    EvalAnsData data = new EvalAnsData()
//                    {
//                        AnswerID = answer.ID,
//                        AnswerAr = answer.AnswerAr,
//                        AnswerEn = answer.AnswerEn
//                    };

//                    Data.Add(data);
//                }

//                _resultHandler.IsSuccessful = true;
//                _resultHandler.Result = Data;
//                _resultHandler.MessageAr = "OK";
//                _resultHandler.MessageEn = "OK";

//                return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
//            }
//            catch (Exception ex)
//            {
//                _resultHandler.IsSuccessful = false;
//                _resultHandler.MessageAr = ex.Message;
//                _resultHandler.MessageEn = ex.Message;

//                return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
//            }
//        }
//    }
//}