using BC.Web.Models;
using Common.Cache;
using Core.Entities;
using Core.Infrastructure;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace BC.Web.Controllers
{
    [OutputCache(Duration = 21600, VaryByHeader = "Host")]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            BillionCompanyDbContext db = BillionCompanyDbContext.Create();
            var model = new HomeViewModel();
            model.PageTitle = Resources.Resources.PageTitle;
            model.Header = Resources.Resources.Start_Header;

            var products = CacheService.GetOrSet("products_latest", () => db.ProductRepository.GetAll().Where(p => !string.IsNullOrEmpty(p.ThumbnailUrl)).OrderByDescending(p => p.InsertDate).ToList());
            var places = CacheService.GetOrSet("places", () => db.PlaceRepository.GetAll().ToList());
            var countries = places.Where(c => c.PlaceTypeId == 1).Take(4).OrderBy(c => c.Name);
            var placeList = new List<TopPlaceItem>();

            foreach (var c in countries)
            {
                var cities = places.Where(i => i.ParentId == c.Id).Take(5).OrderBy(i => i.Name);

                var cityList = new List<TopPlaceItem>();
                foreach (var city in cities)
                {
                    cityList.Add(new TopPlaceItem()
                    {
                        Id = city.Id,
                        PlaceName = model.Host.Contains("mowido.se") ? city.NameSV : city.Name
                    });
                }

                placeList.Add(new TopPlaceItem()
                {
                    Id = c.Id,
                    PlaceName = model.Host.Contains("mowido.se") ? c.NameSV : c.Name,
                    Cities = cityList
                });
            }

            var latestProducts = new List<LatestProduct>();

            Random rdm = new Random();
            int totalRdm = products.Count - 1;

            for (int i = 0; i < 8; i++)
            {
                int index = rdm.Next(totalRdm);
                var place = places.Where(x => x.Id == products[index].PlaceId).ToList().FirstOrDefault();
                latestProducts.Add(new LatestProduct()
                {
                    Id = products[index].Id,
                    Address = products[index].Address,
                    CreatedDate = DateTime.Now.ToShortDateString(),
                    Description = products[index].Description.Length > 150 ? products[index].Description.Substring(0, 150) + "..." : products[index].Description,
                    PlaceName = model.Host.Contains("mowido.se") ? place.NameSV : place.Name,
                    ImageUrl = products[index].ThumbnailUrl
                });
            }

            model.LatestFirst = latestProducts.Take(4).ToList();
            model.LatestSecond = latestProducts.Skip(4).ToList();
            model.Countries = placeList;

            return View(model);
        }

        public ActionResult About()
        {
            var model = new BaseViewModel();
            model.PageTitle = Resources.Resources.About_PageTitle;
            model.Header = Resources.Resources.About_Header;

            return View(model);
        }

        public ActionResult BuyingHomeGuide(string place)
        {
            var model = new BaseViewModel();

            string viewName = "BuyingHomeGuide";

            if (!string.IsNullOrEmpty(place))
            {
                switch (place)
                {
                    case "spanien":
                        viewName = "BuyingHomeGuideSpanien";
                        model.PageTitle = "Att köpa bostad i Spanien - Mowido";
                        model.Header = "Guide för att köpa bostad i Spanien";
                        break;
                    case "italien":
                        viewName = "BuyingHomeGuideItalien";
                        model.PageTitle = "Att köpa bostad i Italien - Mowido";
                        model.Header = "Guide för att köpa bostad i Italien";
                        break;
                    case "portugal":
                        viewName = "BuyingHomeGuidePortugal";
                        model.PageTitle = "Att köpa bostad i Portugal - Mowido";
                        model.Header = "Guide för att köpa bostad i Portugal";
                        break;
                    default:
                        model.PageTitle = "Att köpa bostad utomlands - Mowido";
                        model.Header = "Guide för att köpa bostad utomlands";
                        break;
                }
            }
            else
            {
                model.PageTitle = "Att köpa bostad utomlands - Mowido";
                model.Header = "Guide för att köpa bostad utomlands";
            }

            return View(viewName, model);
        }
    }
}