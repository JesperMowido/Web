using BC.Web.Models;
using Common.Helpers;
using Core.Entities;
using Core.Infrastructure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Web.Controllers
{
    [HandleError]
    public class ProductController : BaseController
    {
        private BillionCompanyDbContext _db = BillionCompanyDbContext.Create();
        private string _topImage = string.Empty;
        private string _placeHeader = string.Empty;
        // GET: Product

        [OutputCache(Duration = 3600, VaryByHeader = "Host", VaryByCustom = "id")]
        public ActionResult Index(int id, string placeslug, string slug)
        {
            var product = _db.ProductRepository.GetByID(id);
            var place = _db.PlaceRepository.GetByID(product.PlaceId);

            string url = UrlBuilder.ProductUrl(id, product.Address.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("!", "-").ToLower(), 
                Resources.Resources.Url_Product, place.Name.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("!", "-").ToLower());

            if (place.Name.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("!", "-").ToLower() != placeslug || 
                product.Address.Trim().Replace(" ", "-").Replace("/", "-").Replace(".", "-").Replace("å", "a").Replace("ä", "a").Replace("ö", "o").Replace("!", "-").ToLower() != slug)
            {
                return RedirectPermanent(url);
            }

            var model = new ProductViewModel();
            var user = _db.UserRepository.GetByID(product.SalesResponsibleId);

            model.Header = product.Address;
            model.PlaceHeader = GetPlaceHeader(product.PlaceId);
            model.Description = product.Description;
            model.ExternalLink = product.ExternalLink;
            model.ExternalPicLink = string.IsNullOrEmpty(product.ExternalPicLink) ? product.ExternalLink : product.ExternalPicLink;
            model.Images = GetBlobImages(id);
            model.TopImage = _topImage;
            model.LivingSpace = product.LivingSpace;
            model.NumberOfRooms = product.NumberOfRooms;
            model.SalesResponsibleName = GetResponsibleSales(user);
            model.Url = url;
            model.CustomerId = product.SalesResponsibleId;
            model.BrokerDescription = user.BrokerDescription;

            if (!string.IsNullOrEmpty(product.Lat) && !string.IsNullOrEmpty(product.Long))
            {
                model.Lat = product.Lat;
                model.Long = product.Long;
                model.ShowMap = true;
            }

            if (product.YearBuilt.HasValue)
            {
                model.YearBuilt = product.YearBuilt.Value.Year;
            }

            if(product.Price.HasValue)
            {
                model.Price = product.Price.Value.ToString("C0", new System.Globalization.CultureInfo("fr-FR"));
            }

            if (product.MonthlyCharge.HasValue)
            {
                model.MonthlyCharge = product.MonthlyCharge.Value.ToString("C0", new System.Globalization.CultureInfo("fr-FR"));
            }

            if (product.SquareMeterPrice.HasValue)
            {
                model.SquareMeterPrice = product.SquareMeterPrice.Value.ToString("C0", new System.Globalization.CultureInfo("fr-FR"));
            }

            var breadcrumbs = new List<BreadCrumbItem>();
            breadcrumbs = GetBreadCrumbs(place.Id, GetPlaceDisplayName(place, model.Host), product.Address);
            model.BreadCrumbs = breadcrumbs;

            model.PageTitle = string.Format("{0}, {1}", product.Address, GetPlaceDisplayName(place, model.Host));

            return View(model);
        }

        private List<string> GetBlobImages(int productId)
        {
            var blobs = new List<string>();
            string containerName = string.Format("product{0}", productId);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            if (container.Exists())
            {
                int index = 0;
                foreach (IListBlobItem item in container.ListBlobs(useFlatBlobListing: true))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;

                        if (index == 0)
                        {
                            _topImage = blob.Uri.ToString();
                        }
                       
                        blobs.Add(blob.Uri.ToString());

                        index++;
                    }
                }
            }

            return blobs;
        }

        private string GetPlaceHeader(int productId)
        {
            var product = _db.PlaceRepository.GetByID(productId);

            if (product != null)
            {
                if (string.IsNullOrEmpty(_placeHeader))
                {
                    _placeHeader = product.Name;
                }
                else
                {
                    _placeHeader = string.Format("{0}, {1}", _placeHeader, product.Name);
                }

                if (product.ParentId.HasValue)
                {
                    return GetPlaceHeader(product.ParentId.Value);
                }
            }

            return _placeHeader;
        }

        private string GetResponsibleSales(User user)
        {
            string fullname = string.Empty;

            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.BrokerName))
                {
                    fullname = user.BrokerName;
                }
                else
                {
                    fullname = string.Format("{0} {1}", user.FirstName, user.LastName);
                }
                
            }

            return fullname;
        }

        private List<BreadCrumbItem> GetBreadCrumbs(int placeid, string placeName, string address)
        {
            var breadCrumbs = new List<BreadCrumbItem>();

            breadCrumbs.Add(new BreadCrumbItem
            {
                Name = Helper.FirstCharToUpper(Resources.Resources.Url_FindHouse),
                Url = "/" + Resources.Resources.Url_FindHouse
            });

            if (!string.IsNullOrEmpty(placeName))
            {
                breadCrumbs.Add(new BreadCrumbItem
                {
                    Name = placeName,
                    Url = UrlBuilder.PlaceUrl(placeid, placeName, Resources.Resources.Url_FindHouse)
                });
            }

            breadCrumbs.Add(new BreadCrumbItem { Name = address });
            return breadCrumbs;
        }

        private string GetPlaceDisplayName(Place p, string host)
        {
            return host.Contains("mowido.se") ? p.NameSV : p.Name;
        }
    }
}