using SMUModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMUniversity.Controllers
{
    public class CardCategoryController : Controller
    {
        admin_SMUniversityEntities _Context = new admin_SMUniversityEntities();

        // GET: CardCategory
        public ActionResult CardCatListForPrint()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult CardCatListForApp()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Create(int ForApplication)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create", new { ForApplication = int.Parse(Request["ForApplication"]) });
            }
        }

        [HttpPost]
        public ActionResult Create(CardCategoryData _Data)
        {
            try
            {
                if (!string.IsNullOrEmpty(_Data.Title) && _Data.Price > 0)
                {
                    TblCardCategory CardCatObj = new TblCardCategory()
                    {
                        Title = _Data.Title,
                        TitleEn = _Data.TitleEn,
                        Price = _Data.Price,
                        ForApplication = Request["ForApplication"] == "0" ? false : true,
                        CreatedDate = DateTime.Now
                    };
                    
                    _Context.TblCardCategories.Add(CardCatObj);
                    _Context.SaveChanges();

                    if (Request["ForApplication"] == "0")
                    {
                        for (int i = 0; i < _Data.ChargeCardsNo; i++)
                        {
                            Random generator = new Random();
                            string Code1 = (generator.Next(0, 999).ToString("D3"));
                            string Code2 = (generator.Next(0, 999999999).ToString("D9"));

                            TblChargeCard _Card = new TblChargeCard()
                            {

                                CardCategoryID = CardCatObj.ID,
                                Valid = true,
                                Code = Code1 + Code2,
                                CreatedDate = DateTime.Now,
                            };

                            _Context.TblChargeCards.Add(_Card);
                            _Context.SaveChanges();
                        }

                    }

                    TempData["notice"] = "تم إضافه فئة الكارت بنجاح";
                    if(_Data.ForApplication == 1)
                    {
                        return RedirectToAction("CardCatListForApp");
                    }
                    else
                    {
                        return RedirectToAction("CardCatListForPrint");
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Create", new { ForApplication = int.Parse(Request["ForApplication"]) });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create", new { ForApplication = int.Parse(Request["ForApplication"]) });
            }
        }

        public ActionResult Edit(int CardCatID, int ForApplication)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                if (CardCatID > 0)
                {
                    TblCardCategory CardCatObj = _Context.TblCardCategories.Where(a => a.ID == CardCatID).SingleOrDefault();

                    return View(CardCatObj);
                }
                else
                {
                    TempData["notice"] = "ERROR while processing!";
                    return RedirectToAction("Edit", new { CardCatID = CardCatID, ForApplication = int.Parse(Request["ForApplication"]) });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { CardCatID = CardCatID, ForApplication = int.Parse(Request["ForApplication"]) });
            }
        }

        [HttpPost]
        public ActionResult Edit(CardCategoryData _Data)
        {
            try
            {
                if (_Data.ID > 0 && !string.IsNullOrEmpty(_Data.Title) && !string.IsNullOrEmpty(_Data.TitleEn))
                {
                    TblCardCategory CardCatObj = _Context.TblCardCategories.Where(a => a.ID == _Data.ID).SingleOrDefault();
                    if (CardCatObj != null)
                    {
                        CardCatObj.Title = _Data.Title;
                        CardCatObj.TitleEn = _Data.TitleEn;
                        //CardCatObj.Price = _Data.Price;
                        CardCatObj.UpdateDate = DateTime.Now;

                        _Context.SaveChanges();
                        if (Request["ForApplication"] == "0")
                        {
                            for (int i = 0; i < _Data.ChargeCardsNo; i++)
                            {
                                TblChargeCard _Card = new TblChargeCard()
                                {
                                    CardCategoryID = CardCatObj.ID,
                                    Valid = true,
                                    Code = GenerateRandomCode(),
                                    CreatedDate = DateTime.Now,
                                };

                                _Context.TblChargeCards.Add(_Card);
                                _Context.SaveChanges();
                            }
                        }

                        TempData["notice"] = "تم تعديل بيانات كارت الفئه بنجاح";
                        if (_Data.ForApplication == 1)
                        {
                            return RedirectToAction("CardCatListForApp");
                        }
                        else
                        {
                            return RedirectToAction("CardCatListForPrint");
                        }
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { CardCatID = _Data.ID, ForApplication = int.Parse(Request["ForApplication"]) });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { CardCatID = _Data.ID, ForApplication = int.Parse(Request["ForApplication"]) });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { CardCatID = _Data.ID, ForApplication = int.Parse(Request["ForApplication"]) });
            }
        }

        public ActionResult ChargeCardsList(int CardCatID)
        {
            try
            {
                if (Session["UserID"] == null || Session["BranchID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                List<TblChargeCard> ChargeCardsList = _Context.TblChargeCards.Where(a => a.CardCategoryID == CardCatID).ToList();
                return View(ChargeCardsList);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        public JsonResult Delete(int CardCatID)
        {
            try
            {
                TblCardCategory _CardCategory = _Context.TblCardCategories.Where(a => a.ID == CardCatID).FirstOrDefault();

                _Context.TblCardCategories.Remove(_CardCategory);
                _Context.SaveChanges();

                return Json("OK");
            }
            catch (Exception ex)
            {
                TempData["notice"] = "لم يتم حذف فئة الكارت لإرتباطها ببيانات اخري";
                return Json("OK");
            }
        }

        public JsonResult CreateChargeCards(int CardCatID, int CardsNumber)
        {
            try
            {
                TblCardCategory _ObjCardCat = _Context.TblCardCategories.Where(a => a.ID == CardCatID).FirstOrDefault();

                for (int i = 0; i < CardsNumber; i++)
                {
                    TblChargeCard _Card = new TblChargeCard()
                    {
                        CardCategoryID = CardCatID,
                        Valid = true,
                        Code = GenerateRandomCode(),
                        CreatedDate = DateTime.Now,
                    };

                    _Context.TblChargeCards.Add(_Card);
                }
                _Context.SaveChanges();

                TempData["notice"] = "تم إنشاء " + CardsNumber + " كارت من فئة " + _ObjCardCat.Title;
                return Json("OK");
            }
            catch (Exception ex)
            {
                TempData["notice"] = "Error while processing!";
                return Json("ERROR");
            }
        }

        private string GenerateRandomCode()
        {
            try
            {
                Random generator = new Random();
                string Code = (generator.Next(0, 999999999).ToString("D9")) + (generator.Next(0, 999).ToString("D3"));
                if (_Context.TblChargeCards.Where(a => a.Code.Equals(Code)).FirstOrDefault() != null)
                {
                    Code = GenerateRandomCode();
                }
                return Code;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}