using BC.Intranet.Models;
using Core.Accounts;
using Core.Entities;
using Core.Infrastructure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BC.Intranet.Controllers
{
    [Authorize(Roles = BCRoles.Admin)]
    public class ProductController : BaseController
    {
        internal BillionCompanyDbContext _context = new BillionCompanyDbContext();
        // GET: Product
        public ActionResult Index()
        {
            var model = new ProductListViewModel();
            model.PageTitle = "List of products - Mowido";

            model.Products = _context.ProductRepository.GetAll().OrderByDescending(i => i.InsertDate).ToList();
            return View(model);
        }

        public ActionResult CreateProduct()
        {
            var model = new ProductViewModel();
            model.PageTitle = "Add product - Mowido";

            var areas = _context.PlaceRepository.GetAll().Where(a => a.PlaceTypeId == 2).OrderBy(a => a.Name).ToList();
            var cities = _context.PlaceRepository.GetAll().Where(a => a.PlaceTypeId == 3).OrderBy(a => a.Name).ToList();
            var salesPersons = _context.UserRepository.GetAll().OrderBy(u => u.UserName).ToList();
            var projectTypes = _context.ProductTypeRepository.GetAll().ToList();

            model.PlaceAreas = new SelectList(areas, "Id", "Name");
            model.PlaceCities = new SelectList(cities, "Id", "Name");
            model.SalesPersons = new SelectList(salesPersons, "Id", "UserName");
            model.ProductTypes = new SelectList(projectTypes, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product();
                newProduct.ProductTypeId = model.ProductTypeId;
                newProduct.SalesResponsibleId = model.SalesResponsibleId;
                newProduct.PlaceId = model.PlaceId;
                newProduct.Description = model.Description;
                newProduct.ExternalLink = model.ExternalLink;
                newProduct.Address = model.Address;
                newProduct.Lat = model.Lat;
                newProduct.Long = model.Long;
                newProduct.IsApproved = model.IsApproved;
                newProduct.InsertDate = DateTime.Now;

                _context.ProductRepository.Insert(newProduct);

                Console.Write(newProduct.Id);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult CreateProductType()
        {
            var model = new ProductTypeViewModel();
            model.PageTitle = "Add product type - Mowido";

            return View(model);
        }

        public ActionResult UploadProductImages(int productId, int listBlobs)
        {
            var model = new ProductImageViewModel();
            model.PageTitle = "Product Image upload - Mowido";
            model.ProductId = productId;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            string containerName = string.Format("product{0}", productId);

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            var isNew = container.CreateIfNotExistsAsync();
            if (isNew.ToString() == "true")
            {
                SetPublicContainerPermissions(container);
            }

            model.Blobs = new List<string>();

            if (listBlobs == 1)
            {
                foreach (IListBlobItem item in container.ListBlobs(useFlatBlobListing: true))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        model.Blobs.Add(blob.Uri.ToString());
                    }
                    //else if (item.GetType() == typeof(CloudPageBlob))
                    //{
                    //    CloudPageBlob blob = (CloudPageBlob)item;
                    //    model.Blobs.Add(blob.Uri.ToString());
                    //}
                    //else if (item.GetType() == typeof(CloudBlobDirectory))
                    //{
                    //    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    //    model.Blobs.Add(dir.Uri.ToString());
                    //}
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UploadProductImages(IEnumerable<HttpPostedFileBase> files, int productId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            string containerName = string.Format("product{0}", productId);

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            SetPublicContainerPermissions(container);

            string blobName = string.Empty;
            int index = 0;

            foreach (var file in files)
            {

                string serverMapPath = "D:\\Images\\" + file.FileName;
                FileInfo fi = new FileInfo(serverMapPath);

                file.SaveAs(serverMapPath);
                file.InputStream.Dispose();

                index++;
                blobName = string.Format("{0}_{1}", containerName, index.ToString());
                CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
                
                using (var fileStream = System.IO.File.OpenRead(serverMapPath))
                {
                    blob.UploadFromStream(fileStream);
                }

                fi.Delete();
            }

            return RedirectToAction("UploadProductImages", new { productId = productId, listBlobs = 1 });
        }

        public static void SetPublicContainerPermissions(CloudBlobContainer container)
        {
            BlobContainerPermissions permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);
        }
    }
}