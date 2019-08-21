using SMUModels;
using SMUModels.ObjectData;
using SMUniversity.WebAPI.Classes;
using SMUniversity.WebAPI.TapServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace SMUniversity.WebAPI.Controllers
{
    public class ChargeCardController : ApiController
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        [HttpGet]
        public HttpResponseMessage GetCardsCategories()
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblCardCategory> _CardCats = _Context.TblCardCategories.ToList();
                List<CardCategoryData> Data = new List<CardCategoryData>();

                foreach (var item in _CardCats)
                {
                    CardCategoryData data = new CardCategoryData()
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Price = item.Price,
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

        [HttpPost]
        public HttpResponseMessage TransformBalance(ChargeCardObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblStudent _StudentSource = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
                TblStudent _StudentDestination = _Context.TblStudents.Where(a => a.PhoneNumber.Equals(_Params.PhoneNumber)).SingleOrDefault();
                if (_StudentDestination != null)
                {
                    if (_StudentSource != null)
                    {
                        if (_Params.Balance <= _StudentSource.Balance)
                        {
                            _StudentSource.Balance -= _Params.Balance;
                            _StudentDestination.Balance += _Params.Balance;

                            _Context.SaveChanges();

                            _resultHandler.IsSuccessful = true;
                            _resultHandler.Result = _StudentSource.Balance;
                            _resultHandler.MessageAr = "OK";
                            _resultHandler.MessageEn = "OK";
                        }
                        else
                        {
                            //Balance not enough
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "رصيدك غير كافي";
                            _resultHandler.MessageEn = "Your balance is not enough";
                        }
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "هذا الحساب غير موجود";
                        _resultHandler.MessageEn = "Account is not found";
                    }
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "رقم التليفون الذي ادخلته غير موجود او غير صحيح";
                    _resultHandler.MessageEn = "The phone number you entered is not exist or not correct";
                }

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
        public HttpResponseMessage ReChargeCard(ReChargeCardObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();

            try
            {
                TblChargeCard _Card = _Context.TblChargeCards.Where(a => a.Code == _Params.CardCode).SingleOrDefault();
                if (_Card != null)
                {
                    if (_Card.Valid)
                    {
                        TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID && a.IsDeleted != true).SingleOrDefault();
                        if (_Student != null)
                        {
                            _Student.Balance += _Card.TblCardCategory.Price;
                            _Card.Valid = false;

                            _Context.SaveChanges();

                            _resultHandler.IsSuccessful = true;
                            _resultHandler.MessageAr = "OK";
                            _resultHandler.MessageEn = "OK";
                        }
                        else
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "هذا الحساب غير موجود";
                            _resultHandler.MessageEn = "Account is not found";
                        }
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "هذا الكارت تم شحنه من قبل ولم يعد صالح للإستخدام مره اخري";
                        _resultHandler.MessageEn = "This card is used before and not valid any more";
                    }
                    
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "رقم الكارت غير صحيح";
                    _resultHandler.MessageEn = "Card number is not valid";
                }

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
        public HttpResponseMessage PayForChargeCards(PaymentObj _Params)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                TblStudent _Student = _Context.TblStudents.Where(a => a.ID == _Params.StudentID).SingleOrDefault();
                if (_Student != null)
                {
                    List<TblCardCategory> _CardList = _Context.TblCardCategories.Where(a => _Params.CardCategoryIDs.Contains(a.ID)).ToList();
                    decimal TotalCardsPrice = _Params.NewPrice > 0 ? _Params.NewPrice : _CardList.Sum(a => a.Price);

                    TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
                    {
                        StudentID = _Student.ID,
                        Pending = false,
                        Price = TotalCardsPrice,
                        IsDeleted = false,
                        //Title = "",
                        //TransactionTypeID = ,
                        CreatedDate = DateTime.Now
                    };

                    _Context.TblBalanceTransactions.Add(_BalanceTrans);
                    _Context.SaveChanges();

                    if (_Params.PaymentMethod.Equals("Online"))
                    {
                        _BalanceTrans.Pending = true;
                        _BalanceTrans.PaymentMethod = "Online";

                        PayGatewayServiceClient obj = new PayGatewayServiceClient();
                        PayRequestDC req = new PayRequestDC();

                        //string strHash2 = CreateHashString("X_MerchantID230320X_UserName65fb9@tapX_ReferenceID45870225008X_Mobile" + _Student.PhoneNumber + "X_CurrencyCodeKWDX_Total" + TotalCardsPrice, "5tap23"); //production
                        //string strHash2 = CreateHashString("X_MerchantID13014X_UserNametestX_ReferenceID45870225000X_Mobile" + _Student.PhoneNumber + "X_CurrencyCodeKWDX_Total" + TotalCardsPrice, "tap1234");
                        string strHash2 = CreateHashString("X_MerchantID13014X_UserNametestX_ReferenceID45870225008X_Mobile" + _Student.PhoneNumber + "X_CurrencyCodeKWDX_Total" + TotalCardsPrice, "tap1234");     //test

                        CustomerDC Customer = new CustomerDC();
                        Customer.Name = _Student.FirstName + " " + _Student.SecondName + " " + _Student.ThirdName;
                        Customer.Mobile = _Student.PhoneNumber;
                        Customer.Email = _Student.Email;

                        MerMastDC Mer = new MerMastDC();
                        Mer.HashString = strHash2;
                        Mer.MerchantID = 13014;
                        Mer.ReferenceID = "45870225000";
                        Mer.UserName = "test";
                        Mer.Password = "65fb9@q8";

                        ProductDC pro = new ProductDC();

                        pro.CurrencyCode = "KWD";
                        pro.TotalPrice = TotalCardsPrice;
                        pro.Quantity = _Params.CardCategoryIDs.Count;
                        pro.UnitName = "Smart Mind University";
                        pro.UnitPrice = TotalCardsPrice;

                        List<ProductDC> arr = new List<ProductDC>();

                        arr.Add(pro);

                        req.lstProductDC = arr.ToArray();
                        req.CustomerDC = Customer;
                        req.MerMastDC = Mer;

                        PayResponseDC res = new PayResponseDC();
                        res = obj.PaymentRequest(req);

                        var url = res.PaymentURL;
                        obj.Close();

                        _resultHandler.Result = url;
                    }
                    else
                    {
                        _Student.Balance += TotalCardsPrice;
                        _BalanceTrans.PaymentMethod = "Cash";

                        _Context.SaveChanges();
                    }

                    _resultHandler.IsSuccessful = true;
                    //_resultHandler.MessageAr = "تم شحن رصيد حسابك بمبلغ " + TotalCardsPrice;
                    _resultHandler.MessageAr = "تم شحن رصيد حسابك";
                    //_resultHandler.MessageEn = "Your Balance is successfully recharged with a price of " + TotalCardsPrice;
                    _resultHandler.MessageEn = "Your Balance is successfully recharged";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "هذا الحساب غير موجود";
                    _resultHandler.MessageEn = "Account is not found";

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
        public HttpResponseMessage GetStudentBalanceTransactions(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                List<TblBalanceTransaction> TransactionsList = _Context.TblBalanceTransactions.Where(a => a.StudentID == StudentID && a.IsDeleted != true).ToList();
                List<TransactionsData> Data = new List<TransactionsData>();
                foreach (var item in TransactionsList)
                {
                    TransactionsData _data = new TransactionsData()
                    {
                        TransactionID = item.ID,
                        TitleAr = item.TitleAr,
                        TitleEn = item.TitleEn,
                        Price = item.Price,
                        TransTypeNameAr = item.TransactionType.NameAr,
                        TransTypeNameEn = item.TransactionType.NameEn,
                        Date = item.CreatedDate.ToString("yyyy-MM-dd"),
                        Time = item.CreatedDate.ToString("hh:mm tt")
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

        private string CreateHashString(string message, string Apikey)
        {
            string result = "";
            dynamic enc = Encoding.Default;

            byte[] baText2BeHashed = enc.GetBytes(message);
            byte[] baSalt = enc.GetBytes(Apikey);

            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);

            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);

            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());

            return result;
        }

    }
}
