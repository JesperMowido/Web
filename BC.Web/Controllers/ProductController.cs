using BC.Web.Models;
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
    [HandleError, Authorize]
    public class ProductController : Controller
    {
        private BillionCompanyDbContext _db = BillionCompanyDbContext.Create();
        private string _topImage = string.Empty;
        private string _placeHeader = string.Empty;
        // GET: Product

        [OutputCache(Duration = 3600, VaryByCustom = "id")]
        public ActionResult Index(int id)
        {
            var product = _db.ProductRepository.GetByID(id);
            var model = new ProductViewModel();
            model.PageTitle = string.Format("{0} - Mowido", product.Address);
            model.Header = product.Address;
            model.PlaceHeader = GetPlaceHeader(product.PlaceId);
            model.Description = product.Description;
            model.ExternalLink = product.ExternalLink;
            model.ExternalPicLink = string.IsNullOrEmpty(product.ExternalPicLink) ? product.ExternalLink : product.ExternalPicLink;
            model.Images = GetBlobImages(id);
            model.TopImage = _topImage;
            model.LivingSpace = product.LivingSpace;
            model.NumberOfRooms = product.NumberOfRooms;
            model.SalesResponsibleName = GetResponsibleSales(product.SalesResponsibleId);
           

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

            return View(model);
        }

        private List<string> GetBlobImages(int productId)
        {
            var blobs = new List<string>();
            string containerName = string.Format("product{0}", productId);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
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
                    else
                    {
                        blobs.Add(blob.Uri.ToString());
                    }

                    index++;
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

        private string GetResponsibleSales(int userId)
        {
            string fullname = string.Empty;

            var user = _db.UserRepository.GetByID(userId);

            if (user != null)
            {
                fullname = string.Format("{0} {1}", user.FirstName, user.LastName);
            }

            return fullname;
        }
    }
}