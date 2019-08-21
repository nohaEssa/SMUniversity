using SMUModels;
using SMUModels.ObjectData;
using WebAPI.Classes;
using WebAPI.TapServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace WebAPI.Controllers
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
                        if(_Params.Balance > 0)
                        {
                            if (_Params.Balance <= _StudentSource.Balance)
                            {
                                _StudentSource.Balance -= _Params.Balance;
                                _StudentDestination.Balance += _Params.Balance;

                                _Context.SaveChanges();

                                _resultHandler.IsSuccessful = true;
                                //_resultHandler.Result = _StudentSource.Balance;
                                _resultHandler.Result = _Params.Balance;
                                _resultHandler.MessageAr = "تم التحويل بنجاح";
                                //_resultHandler.MessageAr = "تم تحويل مبلغ " + _Params.Balance + "الي رقم " + _Params.PhoneNumber + "بنجاح, رصيدك الحالي " + _StudentSource.Balance;
                                _resultHandler.MessageEn = "Transforming is completed successfully";
                                //_resultHandler.MessageEn = _Params.Balance + "has been transformed to " + _Params.PhoneNumber + "successfully, your current balance now is " + _StudentSource.Balance;
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
                            //Balance not valid
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "ادخل مبلغ حقيقي للتحويل";
                            _resultHandler.MessageEn = "Please enter a real balance to be transformed";
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

                            TblBalanceTransaction _BalanceTrans = new TblBalanceTransaction()
                            {
                                StudentID = _Params.StudentID,
                                Price = _Card.TblCardCategory.Price,
                                Pending = false,
                                IsDeleted = false,
                                PaymentMethod = "Cash",
                                TransactionTypeID = 1,
                                TitleAr = "شحن رصيد كارت توفير",
                                //TitleAr = "شحن رصيد كارت توفير بمبلغ : " + _Card.TblCardCategory.Price,
                                TitleEn = "Recharge new card",
                                //TitleEn = "Recharge new card with a price of : " + _Card.TblCardCategory.Price,
                                CreatedDate = DateTime.Now,
                            };

                            _Context.TblBalanceTransactions.Add(_BalanceTrans);
                            _Context.SaveChanges();

                            _resultHandler.IsSuccessful = true;
                            _resultHandler.Result = _Card.TblCardCategory.Price;
                            //_resultHandler.MessageAr = "تم الشحن بنجاح";
                            _resultHandler.MessageAr = "تم الشحن بنجاح من فضلك توجه إلي ادارة المعهد لتأكيد عملية الشحن";
                            //_resultHandler.MessageEn = "Recharge is completed successfully";
                            _resultHandler.MessageEn = "Recharge is completed successfully, please follow up with the administration to confirm the process";

                            return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                        }
                        else
                        {
                            _resultHandler.IsSuccessful = false;
                            _resultHandler.MessageAr = "هذا الحساب غير موجود";
                            _resultHandler.MessageEn = "Account is not found";

                            return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                        }
                    }
                    else
                    {
                        _resultHandler.IsSuccessful = false;
                        _resultHandler.MessageAr = "هذا الكارت تم شحنه من قبل ولم يعد صالح للإستخدام مره اخري";
                        _resultHandler.MessageEn = "This card is used before and not valid any more";

                        return Request.CreateResponse(HttpStatusCode.BadRequest, _resultHandler);
                    }
                    
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "رقم الكارت غير صحيح";
                    _resultHandler.MessageEn = "Card number is not valid";

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
                        Price = TotalCardsPrice,
                        Pending = false,
                        IsDeleted = false,
                        TransactionTypeID = 1,
                        //TitleAr = "شحن رصيد كارت توفير بمبلغ : " + TotalCardsPrice,
                        TitleAr = "شحن رصيد كارت توفير",
                        //TitleEn = "Recharge new card with a price of : " + TotalCardsPrice,
                        TitleEn = "Recharge new card",
                        CreatedDate = DateTime.Now,
                    };

                    _Context.TblBalanceTransactions.Add(_BalanceTrans);

                    if (_Params.PaymentMethod.Equals("Online"))
                    {
                        _Student.Balance += TotalCardsPrice;

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
                        Mer.ReferenceID = "45870225008";
                        //Mer.ReferenceID = "45870225000";
                        Mer.UserName = "test";
                        //Mer.Password = "65fb9@q8";

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
                        //_Student.Balance += TotalCardsPrice;
                        _BalanceTrans.Pending = true;
                        _BalanceTrans.PaymentMethod = "Cash";
                    }

                    _Context.SaveChanges();

                    _resultHandler.IsSuccessful = true;
                    //_resultHandler.MessageAr = "تم شحن رصيد حسابك بمبلغ " + TotalCardsPrice;
                    _resultHandler.MessageAr = "تم شحن رصيد حسابك";
                    _resultHandler.MessageEn = "Your Balance is successfully recharged";

                    return Request.CreateResponse(HttpStatusCode.OK, _resultHandler);
                }
                else
                {
                    _resultHandler.IsSuccessful = false;
                    _resultHandler.MessageAr = "هذا الحساب غير موجود";
                    _resultHandler.MessageEn = "Account is not found";

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
                _resultHandler.Count = int.Parse(_Context.TblStudents.Where(a => a.ID == StudentID).Select(a => a.Balance).FirstOrDefault().ToString());//Temporarily
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
        public HttpResponseMessage GetStudentBalance(int StudentID)
        {
            ResultHandler _resultHandler = new ResultHandler();
            try
            {
                var Balance = _Context.TblStudents.Where(a => a.ID == StudentID).Select(a => a.Balance);
                
                _resultHandler.IsSuccessful = true;
                _resultHandler.Result = Balance;
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
        public HttpResponseMessage GetValidCards()
        {
            var _resultHandler = new ResultHandler();

            try
            {
                List<TblChargeCard> _Cards = _Context.TblChargeCards.Where(a => a.Valid == true).ToList();
                List<ChargeCardData> Data = new List<ChargeCardData>();
                foreach (var item in _Cards)
                {
                    ChargeCardData data = new ChargeCardData()
                    {
                        Code = item.Code,
                        Price = item.TblCardCategory.Price,
                        Valid = item.Valid
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

        [HttpGet]
        public HttpResponseMessage GenerateRechargeCard(int CategoryID)
        {
            var _resultHandler = new ResultHandler();

            try
            {
                Random generator = new Random();
                string Code1 = (generator.Next(0, 999).ToString("D3"));
                string Code2 = (generator.Next(0, 999999999).ToString("D9"));

                TblChargeCard _Card = new TblChargeCard()
                {

                    CardCategoryID = CategoryID,
                    Valid = true,
                    Code = Code1 + Code2,
                    CreatedDate = DateTime.Now,
                };

                _Context.TblChargeCards.Add(_Card);
                _Context.SaveChanges();

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