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
    public class ReportController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        //[HttpPost]
        //public HttpResponseMessage FinancialReport(ReportObj _Params)
        //{
        //    ResultHandler _resultHandler = new ResultHandler();
        //    try
        //    {
        //        TblLecturer Lecturer = _Context.TblLecturers.Where(a => a.ID == _Params.LecturerID && a.IsDeleted != true).FirstOrDefault();
        //        List<FinancialReportData> Data = new List<FinancialReportData>();

        //        foreach (var item in _Params.PaymentMethod)
        //        {
        //            FinancialReportData data = new FinancialReportData();

        //            switch (item)
        //            {
        //                case 1:
        //                    // get all subscriptions of type "Session" for lecturer "X"
        //                    //int LecturesCountPerSession1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && a.TblSession.Type == true && a.FromLecturerSide != true && a.IsDeleted != true).Count(); //count session numbers
        //                    //int LecturesCountPerSessionV1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == LecturerID && a.TblSession.Type == true && a.FromLecturerSide == true && a.IsDeleted != true).Count(); //count session numbers
        //                    List<TblSubscription> Sub1 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && a.TblSession.Type == true && a.IsDeleted != true).ToList(); //count session numbers
        //                    int LecturesCountPerSession1 = Sub1.Count(); //count session numbers
        //                    foreach (var session in Sub1)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);
        //                    }
        //                    Data.Add(data);
        //                    break;
        //                case 2:
        //                    //get all subscriptions of type "Percentage" for lecturer "x"
        //                    List<TblSubscription> Sub2 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && a.TblSession.Type == false && a.SubscripedAsSession != true && a.IsDeleted != true).ToList(); //count session numbers
        //                    int LecturesCountPerSession2 = Sub2.Count(); //count session numbers
        //                    foreach (var session in Sub2)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);
        //                    }
        //                    Data.Add(data);
        //                    break;
        //                case 3:
        //                    //get all subscriptions of type "Percentage" for lecturer "x"
        //                    List<TblSubscription> Sub3 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.FromLecturerSide != true && a.IsDeleted == false)).ToList(); //count session numbers
        //                    List<TblSubscription> SubV3 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && (a.TblSession.LecturerAccountMethod == 2 && a.SubscripedAsSession == true && a.FromLecturerSide == true && a.IsDeleted == false)).ToList(); //count session numbers

        //                    int LecturesCountPerSession3 = Sub3.Count(); //count session numbers
        //                    int LecturesCountPerSessionV3 = SubV3.Count(); //count session numbers

        //                    foreach (var session in Sub3)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

        //                        Data.Add(data);
        //                    }
        //                    foreach (var session in SubV3)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

        //                        Data.Add(data);
        //                    }
        //                    break;
        //                case 4:
        //                    //get all subscriptions of type "Percentage" for lecturer "x"
        //                    List<TblSubscription> Sub4 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && a.TblSession.Type == true && a.FromLecturerSide != true && a.IsDeleted != true).ToList();
        //                    List<TblSubscription> SubV4 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && a.TblSession.Type == true && a.FromLecturerSide == true && a.IsDeleted != true).ToList();
        //                    List<TblSubscription> SubVV4 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && a.TblSession.Type == false && a.TblSession.LecturerAccountMethod == 3 && a.IsDeleted != true).ToList();

        //                    int LecturesCountPerSession4 = Sub4.Count(); //count session numbers
        //                    int LecturesCountPerSessionV4 = SubV4.Count(); //count session numbers
        //                    int LecturesCountPerSessionVV4 = SubVV4.Count(); //count session numbers
        //                    foreach (var session in Sub4)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

        //                        Data.Add(data);
        //                    }
        //                    foreach (var session in SubV4)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

        //                        Data.Add(data);
        //                    }
        //                    foreach (var session in SubVV4)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

        //                        Data.Add(data);
        //                    }
        //                    break;
        //                case 5:
        //                    //get all subscriptions of type "Percentage" for lecturer "x"
        //                    List<TblSubscription> SubVV5 = _Context.TblSubscriptions.Where(a => a.TblSession.LecturerID == _Params.LecturerID && a.FromLecturerSide == true && a.IsDeleted == false).ToList();
        //                    int LecturesCountPerSession5 = SubVV5.Count();//count session numbers
        //                    decimal SubVV5Total = LecturesCountPerSession5 * Lecturer.TblLecturerPaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;

        //                    foreach (var session in SubVV5)
        //                    {
        //                        data.SessionNameAr = session.TblSession.Name;
        //                        data.SessionNameEn = session.TblSession.NameEn;
        //                        data.StudentsNumber = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Count();
        //                        data.HallCodeAr = session.TblSession.TblHall.HallCodeAr;
        //                        data.HallCodeEn = session.TblSession.TblHall.HallCodeEn;
        //                        data.Date = session.TblSession.FromDate.ToString("dd-MM-yyyy");
        //                        data.Price = session.Price;
        //                        data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == session.SessionID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

        //                        Data.Add(data);
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        _resultHandler.IsSuccessful = true;
        //        _resultHandler.Result = Data;
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



        [HttpPost]
        public HttpResponseMessage FinancialReport(ReportObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblLecturer Lecturer = _Context.TblLecturers.Where(a => a.ID == _Params.LecturerID && a.IsDeleted != true).FirstOrDefault();
                List<TblLecturerPaymentMethod> _PaymentMethods = _Context.TblLecturerPaymentMethods.Where(a => a.LecturerID == _Params.LecturerID).ToList();
                List<FinancialReportData> Data = new List<FinancialReportData>();

                foreach (var item in _Params.PaymentMethod)
                {
                    //FinancialReportData data = new FinancialReportData();

                    switch (item)
                    {
                        case 1:
                            List<TblSession> _FixedPriceTypeSessions = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == true && a.LecturerAccountMethod == 1 && a.IsDeleted != true).ToList();
                            List<TblSession> _PercentageTypeSessions = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == true && a.LecturerAccountMethod == 2 && a.IsDeleted != true).ToList();

                            foreach (var s1 in _FixedPriceTypeSessions)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    //data.Date = s1.Type ? s1.FromDate.ToString("dd-MM-yyyy") : ("من " + s1.FromDate.ToString("dd-MM-yyyy") + " إلي " + s1.ToDate.ToString("dd-MM-yyyy"));
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    //data.Price = s1.Type ? _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value : _PaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                                    //if (s1.Type)
                                    //{
                                    //    switch (_SubsList.Count())
                                    //    {
                                    //        //case 0:
                                    //        //    data.Price = (decimal)_SessionDetails.Price1;
                                    //        //    break;
                                    //        case 1:
                                    //            data.Price = (decimal)_SessionDetails.Price2;
                                    //            break;
                                    //        default:
                                    //            data.Price = (decimal)_SessionDetails.Price3;
                                    //            break;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    if (s1.FromDate.Date >= DateTime.Now.Date)
                                    //    {
                                    //        data.Price = (decimal)_SessionDetails.Cost;
                                    //    }
                                    //    else
                                    //    {
                                    //        data.Price = (decimal)_SessionDetails.SessionPrice;
                                    //    }
                                    //}
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value * _SubsList.Count();
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;
                                    //data.Total = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.IsDeleted != true && a.Pending != true).Sum(a => a.Price);

                                    Data.Add(data);
                                }
                                
                            }
                            foreach (var s1 in _PercentageTypeSessions)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = (_PaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value * data.Total) / 100;
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }                            
                            break;
                        case 2:
                            List<TblSession> _CourseTypeSessions = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == false && a.IsDeleted != true).ToList();

                            foreach (var s1 in _CourseTypeSessions)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.Pending != true && a.IsDeleted != true && a.FromLecturerSide == false).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "دوره";
                                    data.SessionTypeEn = "Course";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = "من " + s1.FromDate.ToString("dd-MM-yyyy") + " إلي " + s1.ToDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = _PaymentMethods.Where(a => a.PaymentMethod == 3).FirstOrDefault().Value * _SubsList.Count();
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }
                            break;
                        case 3:
                            List<TblSession> _PercentageTypeSessionsType3 = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == true && a.LecturerAccountMethod == 2 && a.IsDeleted != true).ToList();

                            foreach (var s1 in _PercentageTypeSessionsType3)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = (_PaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value * data.Total) / 100;
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }
                            break;
                        case 4:
                            List<TblSession> _FixedPriceType1SessionsType4 = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == true && a.LecturerAccountMethod == 1 && a.IsDeleted != true).ToList();
                            List<TblSession> _FixedPriceType2SessionsType4 = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == true && a.LecturerAccountMethod == 2 && a.IsDeleted != true).ToList();
                            List<TblSession> _FixedPriceType3SessionsType4 = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == false && a.IsDeleted != true).ToList();

                            foreach (var s1 in _FixedPriceType1SessionsType4)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value * _SubsList.Count();
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }
                            foreach (var s1 in _FixedPriceType2SessionsType4)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.SubscripedAsSession == true && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = (_PaymentMethods.Where(a => a.PaymentMethod == 2).FirstOrDefault().Value * data.Total) / 100;
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }
                            foreach (var s1 in _FixedPriceType3SessionsType4)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.SubscripedAsSession == true && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = _PaymentMethods.Where(a => a.PaymentMethod == 1).FirstOrDefault().Value * _SubsList.Count();
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }
                            break;
                        case 5:
                            List<TblSession> _FromLecturerSideTypeSessionsType = _Context.TblSessions.Where(a => a.LecturerID == _Params.LecturerID && a.Type == true && a.IsDeleted != true).ToList();

                            foreach (var s1 in _FromLecturerSideTypeSessionsType)
                            {
                                TblSessionDetail _SessionDetails = _Context.TblSessionDetails.Where(a => a.SessionID == s1.ID).FirstOrDefault();
                                List<TblSubscription> _SubsList = _Context.TblSubscriptions.Where(a => a.SessionID == s1.ID && a.FromLecturerSide == true && a.Pending != true && a.IsDeleted != true).ToList();
                                if (_SubsList.Count() > 0)
                                {
                                    FinancialReportData data = new FinancialReportData();

                                    TblSubscription _FirstSub = _SubsList.FirstOrDefault();
                                    data.SessionNameAr = s1.Name;
                                    data.SessionNameEn = s1.NameEn;
                                    data.SessionType = s1.Type;
                                    data.SessionTypeAr = "محاضره";
                                    data.SessionTypeEn = "Session";
                                    data.StudentsNumber = _SubsList.Count();
                                    data.HallCodeAr = s1.TblHall.HallCodeAr;
                                    data.HallCodeEn = s1.TblHall.HallCodeEn;
                                    data.Date = s1.FromDate.ToString("dd-MM-yyyy");
                                    data.Price = _PaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value;
                                    data.Total = _SubsList.Sum(a => a.Price);
                                    data.LecturerPercentage = _PaymentMethods.Where(a => a.PaymentMethod == 4).FirstOrDefault().Value * _SubsList.Count();
                                    data.InstituePercentage = data.Total - data.LecturerPercentage;

                                    Data.Add(data);
                                }
                            }
                            break;
                        default:
                            break;
                    }
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
    }
}
