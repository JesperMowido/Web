using BC.Search;
using BC.Search.Helper;
using BC.Search.SearchModel;
using BC.Web.Models;
using Common.Cache;
using Common.Helpers;
using Core.Entities;
using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace BC.Web.Controllers
{
    public class SearchController : BaseController
    {
        private BillionCompanyDbContext _db = BillionCompanyDbContext.Create();
        List<SearchProductItem> _placeProducts = new List<SearchProductItem>();
        private string _placeSlug = string.Empty;
        private BillionSearch _billionSearch = new BillionSearch();
        private BillionProductSearch _productSearch = new BillionProductSearch();
        private List<string> _placeIds = new List<string>();
        private string _placeDescTitle = string.Empty;

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListSearchPlace(int id, string slug, int? page)
        {
            var model = new SearchViewModel();
            var places = CacheService.GetOrSet("places_v8", () => _db.PlaceRepository.GetAll().ToList());
            var place = places.Where(p => p.Id == id).FirstOrDefault<Place>();
            int pageIndex = page == 0 || page == null ? 1 : page.Value;
            int pageSize = 20;
            string sort = Request.QueryString["sort"];
            string filterQuery = Request.QueryString["filter"];
            string filter = SearchHelper.GetCompleteSearchFilter(filterQuery);


            if (place != null)
            {
                string placename = GetPlaceDisplayName(place, Request.Url.Host);
                string url = UrlBuilder.PlaceUrl(place.Id, placename.Trim().Replace(" ", "-").Replace("/", "-").ToLower(), Resources.Resources.Slug_Search_Start, sort, filterQuery);

                if (placename.Trim().Replace(" ", "-").Replace("/", "-").ToLower() != slug)
                {
                    return RedirectPermanent(url);
                }

                model.PlaceName = placename;

                model.Url = string.Format("http://{0}{1}", Request.Url.Host, url);

                _placeIds.Add(place.Id.ToString());
                GetPlaceIds(place.Id, places);

                var breadcrumbs = new List<BreadCrumbItem>();
                breadcrumbs = GetBreadCrumbs(GetPlaceDisplayName(place, model.Host));
                model.BreadCrumbs = breadcrumbs;

                model.Header = string.Format("{0} - {1}", Resources.Resources.Header_SearchPlace, GetPlaceHeader(place.Id));
                model.PageTitle = Resources.Resources.Header_SearchPlace + ", " + GetPlaceHeader(place.Id);

            }
            else
            {
                return Redirect("/" + Resources.Resources.Url_FindHouse);
            }

            var countries = places.Where(c => c.PlaceTypeId == 1).OrderBy(c => c.Name);
            var listCountries = new List<PlaceItem>();

            foreach (var c in countries)
            {
                string c_placename = GetPlaceDisplayName(c, Request.Url.Host);
                listCountries.Add(new PlaceItem()
                {
                    Id = c.Id,
                    DisplayName = c_placename,
                    Url = UrlBuilder.PlaceUrl(c.Id, c_placename.Trim().Replace(" ", "-").Replace("/", "-").ToLower(), Resources.Resources.Slug_Search_Start)
                });
            }

            model.Countries = listCountries;
            model.PlaceId = id;



            int skip = (pageIndex - 1) * pageSize;

            string sortIndex = string.Empty;

            if (!string.IsNullOrEmpty(sort))
            {
                sortIndex = sort.Contains("_") ? sort.Replace("_", " ") : sort;
                model.SearchSort = sort;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                model.SearchFilter = filterQuery;    
            }

            var searchResult = _productSearch.Search(_placeIds, skip, sortIndex, filter);

            var result = searchResult.Results;

            if (result != null && result.Count > 0)
            {
                foreach (var p in result)
                {
                    _placeProducts.Add(SetProductData(p.Document));
                }
            }

            int total = searchResult.Count.HasValue ? int.Parse(searchResult.Count.Value.ToString()) : 0;
            int pageStart = ((pageIndex - 1) * pageSize) + 1;

            int spanStop = pageIndex * pageSize;
            int pageStop = spanStop > total ? total : spanStop;

            model.PageSize = pageSize;
            model.PageIndex = pageIndex;
            model.Products = _placeProducts;
            model.ProductCount = total;
            model.PageStart = pageStart;
            model.PageStop = pageStop;
            model.MapObjects = GetMapObjects(_placeProducts);

            model.PlaceDescription = model.Host.Contains("mowido.se") ? GetPlaceDescriptionSV(place, places) : string.Empty;
            model.PlaceDescTitle = !string.IsNullOrEmpty(model.PlaceDescription) ? _placeDescTitle : string.Empty;

            return View(model);
        }

        public ActionResult ListAllPlaces()
        {
            var model = new ListAllPlacesViewModel();
            var places = CacheService.GetOrSet("places_v9", () => _db.PlaceRepository.GetAll().ToList());

            model.Header = Resources.Resources.Header_FindHouse;
            model.PageTitle = Resources.Resources.Header_FindHouse + " - Mowdio";

            string host = Request.Url.Host;

            var countrylist = places.Where(c => c.PlaceTypeId == 1).OrderBy(p => p.Name).ToList();
            model.Countries = GetAlphbeticPlaceList(countrylist, host);

            var areaList = places.Where(c => c.PlaceTypeId == 2).OrderBy(p => p.Name).ToList();
            model.Areas = GetAlphbeticPlaceList(areaList, host);

            var cityList = places.Where(c => c.PlaceTypeId == 3).OrderBy(p => p.Name).ToList();
            model.Cities = GetAlphbeticPlaceList(cityList, host);

            var districtList = places.Where(c => c.PlaceTypeId == 4).OrderBy(p => p.Name).ToList();
            model.Districts = GetAlphbeticPlaceList(districtList, host);

            return View(model);
        }

        public ActionResult Search(string q = "")
        {
            if (string.IsNullOrWhiteSpace(q))
                q = "*";

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = _billionSearch.Search(q, Request.Url.Host).Results
            };
        }

        public ActionResult SortSearch(string q, string filter, string id, string slug, string startslug)
        {
            if (string.IsNullOrWhiteSpace(q))
                q = "InsertDate desc";


            string url = UrlBuilder.PlaceUrl(int.Parse(id), slug, startslug, q, filter);

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = url
            };
        }

        public ActionResult SearchMap(int id, string slug, int? page, string sort)
        {
            var model = new SearchViewModel();
            var places = CacheService.GetOrSet("places_v9", () => _db.PlaceRepository.GetAll().ToList());
            var place = places.Where(p => p.Id == id).FirstOrDefault<Place>();
            int pageIndex = page == 0 || page == null ? 1 : page.Value;
            int pageSize = 20;
            if (place != null)
            {
                string placename = GetPlaceDisplayName(place, Request.Url.Host);
                string url = UrlBuilder.PlaceUrl(place.Id, placename.Trim().Replace(" ", "-").Replace("/", "-").ToLower(), Resources.Resources.Slug_Search_Start);

                if (placename.Trim().Replace(" ", "-").Replace("/", "-").ToLower() != slug)
                {
                    return RedirectPermanent(url);
                }

                model.PlaceName = placename;

                model.Url = string.Format("http://{0}{1}", Request.Url.Host, url);

                _placeIds.Add(place.Id.ToString());
                GetPlaceIds(place.Id, places);

                var breadcrumbs = new List<BreadCrumbItem>();
                breadcrumbs = GetBreadCrumbs(GetPlaceDisplayName(place, model.Host));
                model.BreadCrumbs = breadcrumbs;

                model.Header = string.Format("{0} - {1}", Resources.Resources.Header_SearchPlace, GetPlaceHeader(place.Id));
                model.PageTitle = Resources.Resources.Header_SearchPlace + ", " + GetPlaceHeader(place.Id);

            }
            else
            {
                return Redirect("/" + Resources.Resources.Url_FindHouse);
            }

            var countries = places.Where(c => c.PlaceTypeId == 1).OrderBy(c => c.Name);
            var listCountries = new List<PlaceItem>();

            foreach (var c in countries)
            {
                string c_placename = GetPlaceDisplayName(c, Request.Url.Host);
                listCountries.Add(new PlaceItem()
                {
                    Id = c.Id,
                    DisplayName = c_placename,
                    Url = UrlBuilder.PlaceUrl(c.Id, c_placename.Trim().Replace(" ", "-").Replace("/", "-").ToLower(), Resources.Resources.Slug_Search_Start)
                });
            }

            model.Countries = listCountries;
            model.PlaceId = id;



            int skip = (pageIndex - 1) * pageSize;



            var searchResult = _productSearch.Search(_placeIds, skip, sort, string.Empty);

            var result = searchResult.Results;

            if (result != null && result.Count > 0)
            {
                foreach (var p in result)
                {
                    _placeProducts.Add(SetProductData(p.Document));
                }
            }

            int total = searchResult.Count.HasValue ? int.Parse(searchResult.Count.Value.ToString()) : 0;
            int pageStart = ((pageIndex - 1) * pageSize) + 1;

            int spanStop = pageIndex * pageSize;
            int pageStop = spanStop > total ? total : spanStop;

            model.PageSize = pageSize;
            model.PageIndex = pageIndex;
            model.Products = _placeProducts;
            model.ProductCount = total;
            model.PageStart = pageStart;
            model.PageStop = pageStop;
            model.MapObjects = GetMapObjects(_placeProducts);

            model.PlaceDescription = model.Host.Contains("mowido.se") ? GetPlaceDescriptionSV(place, places) : string.Empty;
            model.PlaceDescTitle = !string.IsNullOrEmpty(model.PlaceDescription) ? _placeDescTitle : string.Empty;
            return View(model);
        }

        private void GetPlaceIds(int id, List<Place> places)
        {
            var result = places.Where(p => p.ParentId == id).Select(p => p.Id.ToString()).ToList();

            _placeIds.AddRange(result);

            if (result == null)
            {
                return;
            }

            foreach (var p in result)
            {
                GetPlaceIds(int.Parse(p), places);
            }
        }

        private IEnumerable<Place> GetParentPlaces(int id, IEnumerable<Place> places)
        {
            var list = places.Where(p => p.ParentId == id);

            return list;
        }

        private List<PlaceItem> GetAlphbeticPlaceList(List<Place> places, string host)
        {
            var list = new List<PlaceItem>();

            foreach (var c in places)
            {
                string placeName = GetPlaceDisplayName(c, host);
                list.Add(new PlaceItem
                {
                    Id = c.Id,
                    DisplayName = placeName,
                    Url = UrlBuilder.PlaceUrl(c.Id, placeName.Trim().Replace(" ", "-").Replace("/", "-").ToLower(), Resources.Resources.Slug_Search_Start)
                });
            }

            return list;
        }

        private string GetMapObjects(List<SearchProductItem> products)
        {
            string result = "[";

            foreach (var p in products)
            {
                if (!string.IsNullOrEmpty(p.Address) && !string.IsNullOrEmpty(p.Lat) && !string.IsNullOrEmpty(p.Long))
                {
                    result += "{";
                    result += string.Format("'title': '{0}',", p.Address.Replace(",", "").Replace("'", "-"));
                    result += string.Format("'lat': '{0}',", p.Lat);
                    result += string.Format("'lng': '{0}',", p.Long);
                    result += string.Format("'description': '<div style=\"width:200px;\"><a style=\"text-decoration:none;\" href=\"{0}\"><h4>{1}</h4><img style=\"width:100px;float:left;margin-right:15px;\" src=\"{2}\" alt=\"{3}\" \\><p style=\"color:#ee8d26;font-weight:bold;\">{4}</p><p><a class=\"btn btn-blue\" href=\"{5}\">{6}</a></p></a></div>'",
                        p.Url,
                        (p.Address.Length > 18 ? p.Address.Substring(0, 15) + "..." : p.Address),
                        p.ThumbnailUrl,
                        (p.Address.Length > 18 ? p.Address.Substring(0, 15) + "..." : p.Address),
                        p.Price,
                        p.Url,
                        "Läs mer");
                    result += "},";
                }
            }

            result += "]";

            return result;
        }

        private string GetPlaceHeader(int placeid)
        {
            var place = _db.PlaceRepository.GetByID(placeid);
            string result = string.Empty;

            if (place != null)
            {
                if (string.IsNullOrEmpty(_placeSlug))
                {
                    _placeSlug = GetPlaceDisplayName(place, Request.Url.Host);
                }
                else
                {
                    _placeSlug = string.Format("{0}, {1}", _placeSlug, GetPlaceDisplayName(place, Request.Url.Host));
                }

                if (place.ParentId.HasValue)
                {
                    return GetPlaceHeader(place.ParentId.Value);
                }
            }

            result = _placeSlug;
            _placeSlug = string.Empty;
            return result;
        }

        private SearchProductItem SetProductData(SearchProduct product)
        {
            var item = new SearchProductItem();

            var place = _db.PlaceRepository.GetByID(int.Parse(product.PlaceId));

            item.Id = product.Id;
            item.Address = product.Address;
            item.PlaceHeader = GetPlaceHeader(int.Parse(product.PlaceId));
            item.Description = product.Description;
            item.ExternalLink = product.ExternalLink;
            item.LivingSpace = product.LivingSpace;
            item.NumberOfRooms = product.NumberOfRooms;
            item.Url = UrlBuilder.ProductUrl(product.Id, product.Address, Resources.Resources.Url_Product, place.Name);
            item.ThumbnailUrl = product.ThumbnailUrl;
            item.CustomerId = product.SalesResponsibleId;

            if (!string.IsNullOrEmpty(product.Lat) && !string.IsNullOrEmpty(product.Long))
            {
                item.Lat = product.Lat;
                item.Long = product.Long;
                item.ShowMap = true;
            }

            if (!string.IsNullOrEmpty(product.Price))
            {
                item.Price = GetProductPrice(product.Price);
            }

            return item;
        }

        private List<BreadCrumbItem> GetBreadCrumbs(string placeName)
        {
            var breadCrumbs = new List<BreadCrumbItem>();

            breadCrumbs.Add(new BreadCrumbItem
            {
                Name = Helper.FirstCharToUpper(Resources.Resources.Url_FindHouse),
                Url = "/" + Resources.Resources.Url_FindHouse
            });

            breadCrumbs.Add(new BreadCrumbItem { Name = placeName });
            return breadCrumbs;
        }

        private string GetPlaceDisplayName(Place p, string host)
        {
            return host.Contains("mowido.se") ? p.NameSV : p.Name;
        }

        private string GetPlaceDescriptionSV(Place place, IEnumerable<Place> places)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(place.DescriptionSV))
            {
                result = place.DescriptionSV;
                _placeDescTitle = place.NameSV;
            }
            else if (place.ParentId.HasValue)
            {
                var parent = places.Where(p => p.Id == place.ParentId.Value).FirstOrDefault();

                return GetPlaceDescriptionSV(parent, places);
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }

        private string GetProductPrice(string price)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(price))
            {
                var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
                ci.NumberFormat.NumberDecimalSeparator = ".";

                decimal priceDec = decimal.Parse(price, ci);

                result = priceDec.ToString("C0", new System.Globalization.CultureInfo("fr-FR"));
            }

            return result;
        }
    }
}