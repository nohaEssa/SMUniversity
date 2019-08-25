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
        public ActionResult Index()
        {
            if (Session["UserID"] == null || Session["BranchID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Create()
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
                return RedirectToAction("Create");
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
                        CreatedDate = DateTime.Now
                    };
                    
                    _Context.TblCardCategories.Add(CardCatObj);
                    _Context.SaveChanges();

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

                    TempData["notice"] = "تم إضافه فئة الكارت بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Create");
            }
        }

        public ActionResult Edit(int CardCatID)
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
                    return RedirectToAction("Edit", new { CardCatID = CardCatID });
                }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { CardCatID = CardCatID });
            }
        }

        [HttpPost]
        public ActionResult Edit(CardCategoryData _Data)
        {
            try
            {
                if (_Data.ID > 0 && !string.IsNullOrEmpty(_Data.Title) && !string.IsNullOrEmpty(_Data.TitleEn) && _Data.Price > 0)
                {
                    TblCardCategory CardCatObj = _Context.TblCardCategories.Where(a => a.ID == _Data.ID).SingleOrDefault();
                    if (CardCatObj != null)
                    {
                        CardCatObj.Title = _Data.Title;
                        CardCatObj.TitleEn = _Data.TitleEn;
                        CardCatObj.Price = _Data.Price;
                        CardCatObj.UpdateDate = DateTime.Now;

                        _Context.SaveChanges();

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

                        TempData["notice"] = "تم تعديل بيانات كارت الفئه بنجاح";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "بيانات خاطئه";
                        return RedirectToAction("Edit", new { CardCatID = _Data.ID });
                    }
                }
                else
                {
                    TempData["notice"] = "من فضلك ادخل البيانات المطلوبه كاملةً";
                    return RedirectToAction("Edit", new { CardCatID = _Data.ID });
                }

            }
            catch (Exception ex)
            {
                TempData["notice"] = "ERROR while processing!";
                return RedirectToAction("Edit", new { CardCatID = _Data.ID });
            }
        }

        public ActionResult ChargeCardsList()
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

    }
}