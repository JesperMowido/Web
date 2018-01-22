using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Intranet.Controllers
{
    public class MigrateController : BaseController
    {
        BillionCompanyDbContext _db = BillionCompanyDbContext.Create();
        // GET: Migrate
        public ActionResult Index()
        {
            return View();
        }

        public string UpdatePriceFilterField()
        {
            string result = "No products to update";

            var products = _db.ProductRepository.GetAll().Where(p => p.PriceFilter == null).Take(1000).ToList();

            if (products != null)
            {
                var uProducts = products.ToList();

                if (uProducts != null && uProducts.Count > 0)
                {
                    foreach (var p in uProducts)
                    {
                        if (p.Price.HasValue)
                        {
                            try
                            {
                                p.PriceFilter = Convert.ToDouble(p.Price.Value);

                                _db.ProductRepository.Update(p);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }

                    _db.SaveChanges();
                }

                

                result = "Success: " + products.Count().ToString();
            }

            
            return result;
        }

        public string UpdateInsertDate()
        {
            var products = _db.ProductRepository.GetAll().ToList();

            Random rdm = new Random();

            foreach (var p in products)
            {
                int rdmNum = rdm.Next(1, 90);
                p.InsertDate = DateTime.Now.AddDays(-rdmNum);
                _db.ProductRepository.Update(p);
            }

            _db.SaveChanges();

            return "succeded:" + products.Count.ToString();
        }
    }
}