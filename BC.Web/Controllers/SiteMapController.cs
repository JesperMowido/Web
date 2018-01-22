using Common.Helpers;
using Core.Entities;
using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BC.Web.Controllers
{
    public class SiteMapController : BaseController
    {
        private BillionCompanyDbContext _db = BillionCompanyDbContext.Create();
        // GET: SiteMap
        public ActionResult Index()
        {
            var date = DateTime.Now;
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var index = "https://www.mowido.se/{0}";
            var pagesegments = new string[]
            {
                "",
                "om-oss",
                "bostader",
                "kopa-bostad",
                "kopa-bostad/spanien",
                "kopa-bostad/italien"
            };

            var siteMapXml = new XElement(ns + "urlset",
                             from segment in pagesegments
                             select new XElement(ns + "url",
                                 new XElement(ns + "loc", string.Format(index, segment)),
                                 new XElement(ns + "lastmod", date.ToString("yyyy-MM-dd")),
                                 new XElement(ns + "changefreq", "monthly"),
                                 new XElement(ns + "priority", "1.0")));

            return this.Content(siteMapXml.ToString(), "text/xml");
        }

        public ActionResult IndexCOM()
        {
            var date = DateTime.Now;
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var index = "https://www.mowido.com/{0}";
            var pagesegments = new string[]
            {
                "about",
                "houses"
            };

            var siteMapXml = new XElement(ns + "urlset",
                             from segment in pagesegments
                             select new XElement(ns + "url",
                                 new XElement(ns + "loc", string.Format(index, segment)),
                                 new XElement(ns + "lastmod", date.ToString("yyyy-MM-dd")),
                                 new XElement(ns + "changefreq", "monthly"),
                                 new XElement(ns + "priority", "1.0")));

            return this.Content(siteMapXml.ToString(), "text/xml");
        }

        public ActionResult Products()
        {
            string host = Request.Url.Host;
            var productUrls = _db.ProductRepository.GetAll().Where(p => p.IsApproved).ToArray().
                    Select(e => new
                    {
                        Url = UrlBuilder.ProductUrl(e.Id, e.Address, Resources.Resources.Url_Product, GetPlaceDisplayName(e.Place, host))
                    }).ToArray();

            var index = "https://www.mowido.com{0}";

            if (host.Contains("mowido.se"))
            {
                index = "https://www.mowido.se{0}";
            }
            else
            {
                index = "http://www.mowido.com{0}";
            }

            var date = DateTime.Now;

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var siteMapXml = new XElement(ns + "urlset",
                             from p in productUrls
                             select new XElement(ns + "url",
                                 new XElement(ns + "loc", string.Format(index, p.Url)),
                                 new XElement(ns + "lastmod", date.ToString("yyyy-MM-dd")),
                                 new XElement(ns + "changefreq", "weekly"),
                                 new XElement(ns + "priority", "1.0")));

            return this.Content(siteMapXml.ToString(), "text/xml");
        }

        public ActionResult Places()
        {
            string host = Request.Url.Host;
            var placeUrls = _db.PlaceRepository.GetAll().
                    Select(e => new
                    {
                        Url = UrlBuilder.PlaceUrl(e.Id, GetPlaceDisplayName(e, host), Resources.Resources.Url_FindHouse )
                    }).ToArray();

            var index = "https://www.mowido.com{0}";

            if (host.Contains("mowido.se"))
            {
                index = "https://www.mowido.se{0}";
            }
            else
            {
                index = "http://www.mowido.com{0}";
            }

            var date = DateTime.Now;

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var siteMapXml = new XElement(ns + "urlset",
                             from p in placeUrls
                             select new XElement(ns + "url",
                                 new XElement(ns + "loc", string.Format(index, p.Url)),
                                 new XElement(ns + "lastmod", date.ToString("yyyy-MM-dd")),
                                 new XElement(ns + "changefreq", "daily"),
                                 new XElement(ns + "priority", "1.0")));

            return this.Content(siteMapXml.ToString(), "text/xml");
        }

        private string GetPlaceDisplayName(Place p, string host)
        {
            return host.Contains("mowido.se") ? p.NameSV : p.Name;
        }
    }
}