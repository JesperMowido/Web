using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using BC.Web.Models;
using System.IO;
using System.Threading.Tasks;

namespace BC.Web.Controllers
{
    [Authorize]
    public class BlobsController : BaseController
    {
        // GET: Blobs
        public ActionResult CreateBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("test-blob-container");

                var isNew = container.CreateIfNotExistsAsync();
            if(isNew.ToString() ==  "true")
                container.SetPermissionsAsync(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            ViewBag.BlobContainerName = container.Name;

            var model = new BaseViewModel();

            return View(model);
        }

        public EmptyResult UploadBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("test-blob-container");

            CloudBlockBlob blob = container.GetBlockBlobReference("first-upload");

            using (var fileStream = System.IO.File.OpenRead("D://Movey//house2.jpg"))
            {
                blob.UploadFromStream(fileStream);
            }

            return new EmptyResult();
        }

        public ActionResult ListBlobs()
        {
            var model = new ListBlobViewModel();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("test-blob-container");

            model.Blobs = new List<string>();

            string imageFullPath = string.Empty;

            foreach (IListBlobItem item in container.ListBlobs(useFlatBlobListing:true))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    model.Blobs.Add(blob.Name);
                    imageFullPath = blob.Uri.ToString();
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    model.Blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    model.Blobs.Add(dir.Uri.ToString());
                }
            }

            model.FirstImage = imageFullPath;

            return View(model);

        }

        public EmptyResult DownloadBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("test-blob-container");

            CloudBlockBlob blob = container.GetBlockBlobReference("first-upload");

            using (var fileStream = System.IO.File.OpenWrite("C://BlobDownload"))
            {
                blob.DownloadToStream(fileStream);
            }

            return new EmptyResult();
        }
    }

    public class ListBlobViewModel : BaseViewModel
    {
        public List<string> Blobs { get; set; }
        public string FirstImage { get; set; }
    }
}