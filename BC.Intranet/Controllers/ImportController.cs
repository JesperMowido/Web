using BC.Intranet.Models;
using Common.Extensions;
using Common.Helpers;
using Common.Import;
using Core.Entities;
using Core.Infrastructure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace BC.Intranet.Controllers
{
    public class ImportController : BaseController
    {
        BillionCompanyDbContext _db = BillionCompanyDbContext.Create();
        // GET: Import
        public string Index()
        {

            var products = _db.ProductRepository.GetAll().Where(p => !string.IsNullOrEmpty(p.ThumbnailUrl) && p.Id < 300 && p.Id > 75);

            foreach (var p in products)
            {
                UpdateContainerAccess(p.Id);
            }

            return "Success";
        }

        public ActionResult SkandiaBroker()
        {
            int userId = 24;

            var model = new SkandiaImportViewModel();
            var skandiaProducts = _db.SkandiaProductRepository.GetAll().Where(s => s.Id > 4830).ToList();

            var products = new List<ProductItem>();

            foreach (var s_product in skandiaProducts)
            {
                string placeName = ImportHelper.GetPartOfString(s_product.Place, '-', 1);
                var p = new ProductItem();
                p.Price = ImportHelper.GetDecimalFromString(s_product.Price);
                p.SalesResponsibleId = userId;
                p.PlaceId = SetCreatePlaceId(placeName, 2);
                p.DisplayPlace = placeName;
                p.LivingSpace = ImportHelper.GetFirstIntFromString(s_product.LivingSpace);
                p.NumberOfRooms = ImportHelper.GetFirstIntFromString(s_product.NumberOfRooms);
                p.ExternalLink = s_product.ExternalLink;

                string firstPartAddress = ImportHelper.GetPartOfString(s_product.Address, '|', 0);
                p.Address = ImportHelper.GetPartOfString(firstPartAddress, ',', 0);
                p.Description = s_product.Description;
                p.RefNr = s_product.RefNo.Contains("-") && s_product.RefNo.Length < 12 ? s_product.RefNo : string.Empty;
                p.ImageUrl1 = s_product.ImageUrl1;
                p.ImageUrl2 = s_product.ImageUrl2;
                p.ImageUrl3 = s_product.ImageUrl3;
                p.ImageUrl4 = s_product.ImageUrl4;
                p.ImageUrl5 = s_product.ImageUrl5;
                p.ProductTypeId = 1;

                if (!string.IsNullOrEmpty(placeName))
                {
                    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBML9XUO8FpiHQShjmlL7QnIw7UkWheOQE", Uri.EscapeDataString(placeName));
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(requestUri);

                    var result = xdoc.SelectNodes("/GeocodeResponse/result/geometry/location");

                    foreach (XmlNode objXmlNode in result)
                    {
                        p.Lat = objXmlNode.ChildNodes.Item(0).InnerText;

                        p.Long = objXmlNode.ChildNodes.Item(1).InnerText;
                    }

                }

                SaveProduct(p);

                products.Add(p);
            }

            model.Products = products;
            model.NumberOfProducts = products.Count;

            return View(model);
        }

        public ActionResult WretmanBroker()
        {
            int userId = 10;

            var model = new WretmanImportViewModel();
            var wretmanProducts = _db.WretmanProductRepository.GetAll().Where(w => w.Id > 286).ToList();

            var products = new List<ProductItem>();

            foreach (var s_product in wretmanProducts)
            {
                string placeName = string.Empty;

                if (s_product.Place.Contains(","))
                {
                    placeName = ImportHelper.GetPartOfString(s_product.Place, ',', 0); ;
                }
                else
                {
                    placeName = s_product.Place;
                }
                
                var p = new ProductItem();
                p.Price = ImportHelper.GetDecimalFromString(s_product.Price);
                p.SalesResponsibleId = userId;
                p.PlaceId = SetCreateWretmanPlaceId(placeName, 10);
                p.DisplayPlace = placeName;
                p.LivingSpace = ImportHelper.GetLivingSpace(s_product.Spaces);
                p.NumberOfRooms = ImportHelper.GetFirstIntFromString(s_product.Spaces);
                p.ExternalLink = s_product.ExternalLink;

                p.Address = ImportHelper.GetPartOfString(s_product.Address, '|', 0);
                p.Description = ImportHelper.GetPartOfString(s_product.Description, '—', 1);

                p.RefNr = ImportHelper.GetPartOfString(s_product.Description, '—', 0).Trim();
                p.ImageUrl1 = s_product.ImageUrl1.Contains("picture-100") ? s_product.ImageUrl1.Replace("picture-100", "picture-1024") : s_product.ImageUrl1;
                p.ImageUrl2 = s_product.ImageUrl2.Contains("picture-100") ? s_product.ImageUrl2.Replace("picture-100", "picture-1024") : s_product.ImageUrl2;
                p.ImageUrl3 = s_product.ImageUrl3.Contains("picture-100") ? s_product.ImageUrl3.Replace("picture-100", "picture-1024") : s_product.ImageUrl3;
                p.ImageUrl4 = s_product.ImageUrl4.Contains("picture-100") ? s_product.ImageUrl4.Replace("picture-100", "picture-1024") : s_product.ImageUrl4;
                p.ImageUrl5 = s_product.ImageUrl5.Contains("picture-100") ? s_product.ImageUrl5.Replace("picture-100", "picture-1024") : s_product.ImageUrl5;
                p.ProductTypeId = 1;

                if (!string.IsNullOrEmpty(placeName))
                {
                    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBML9XUO8FpiHQShjmlL7QnIw7UkWheOQE", Uri.EscapeDataString(placeName));
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(requestUri);

                    var result = xdoc.SelectNodes("/GeocodeResponse/result/geometry/location");

                    foreach (XmlNode objXmlNode in result)
                    {
                        p.Lat = objXmlNode.ChildNodes.Item(0).InnerText;

                        p.Long = objXmlNode.ChildNodes.Item(1).InnerText;
                    }

                }

                SaveProduct(p);

                products.Add(p);
            }

            model.Products = products;
            model.NumberOfProducts = products.Count;

            return View(model);
        }

        public ActionResult PortugalBroker()
        {
            int userId = 7;
            int parentPlaceId = 16;

            var model = new PortugalImportViewModel();
            var portugalProducts = _db.PortugalMaklarnaProductRepository.GetAll().Where(p => p.Id > 946).ToList();

            var products = new List<ProductItem>();

            foreach (var s_product in portugalProducts)
            {
                string placeName = string.Empty;

                if (!string.IsNullOrEmpty(s_product.City))
                {
                    if (s_product.City.Contains("("))
                    {
                        placeName = ImportHelper.GetPartOfString(s_product.City, '(', 0).Trim();
                    }
                    else
                    {
                        placeName = s_product.City.Trim();
                    }
                }
                else
                {
                    placeName = s_product.Area.Trim();
                }

                var p = new ProductItem();
                p.Price = ImportHelper.GetDecimalFromString(s_product.Price);
                p.SalesResponsibleId = userId;
                p.PlaceId = SetCreateWretmanPlaceId(placeName, parentPlaceId);
                p.DisplayPlace = placeName;
                p.LivingSpace = ImportHelper.GetFirstIntFromString(s_product.LivingSpace);
                p.NumberOfRooms = ImportHelper.GetFirstIntFromString(s_product.NumberOfRooms);
                p.ExternalLink = s_product.ExternalLink;
                p.YearBuilt = !string.IsNullOrEmpty(s_product.YearBuilt) ? new DateTime(int.Parse(s_product.YearBuilt), 1, 1) : (DateTime?)null;

                p.Address = s_product.Address.Replace("+", "").Replace(":", "").Replace(".", "-").Replace("&", "");
                p.Description = s_product.Description;

                p.RefNr = s_product.RefNo;
                p.ImageUrl1 = s_product.ImageUrl1;
                p.ImageUrl2 = s_product.ImageUrl2;
                p.ImageUrl3 = s_product.ImageUrl3;
                p.ImageUrl4 = s_product.ImageUrl4;
                p.ImageUrl5 = s_product.ImageUrl5;
                p.ProductTypeId = 1;

                if (!string.IsNullOrEmpty(placeName))
                {
                    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBML9XUO8FpiHQShjmlL7QnIw7UkWheOQE", Uri.EscapeDataString(placeName));
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(requestUri);

                    var result = xdoc.SelectNodes("/GeocodeResponse/result/geometry/location");

                    foreach (XmlNode objXmlNode in result)
                    {
                        p.Lat = objXmlNode.ChildNodes.Item(0).InnerText;

                        p.Long = objXmlNode.ChildNodes.Item(1).InnerText;
                    }

                }

                SaveProduct(p);

                products.Add(p);
            }

            model.Products = products;
            model.NumberOfProducts = products.Count;

            return View(model);
        }

        public ActionResult NordicFranceBroker()
        {
            int userId = 15;
            int parentPlaceId = 6;

            var model = new NordicFranceViewModel();
            var franceProducts = _db.NordicFranceProductRepository.GetAll().Where(n => n.Id > 3616).ToList();

            var products = new List<ProductItem>();

            foreach (var s_product in franceProducts)
            {
                if (!string.IsNullOrEmpty(s_product.Address) && !string.IsNullOrEmpty(s_product.Address2))
                {
                    string placeName = string.Empty;

                    if (!string.IsNullOrEmpty(s_product.City))
                    {
                        placeName = ImportHelper.GetFirstString(s_product.City).Trim();
                    }
                    else
                    {
                        placeName = ImportHelper.GetFirstString(s_product.Area).Trim();
                    }

                    var p = new ProductItem();
                    p.Price = ImportHelper.GetDecimalFromString(s_product.Price) / 10;
                    p.SalesResponsibleId = userId;
                    p.PlaceId = SetCreateWretmanPlaceId(placeName, parentPlaceId);
                    p.DisplayPlace = placeName;
                    //p.LivingSpace = ImportHelper.GetFirstIntFromString(s_product.LivingSpace);
                    //p.NumberOfRooms = ImportHelper.GetFirstIntFromString(s_product.NumberOfRooms);
                    p.ExternalLink = s_product.ExternalLink;

                    p.Address = !string.IsNullOrEmpty(s_product.Address) ? s_product.Address.Replace("+", "").Replace(":", "").Replace(".", "-").Replace("&", "") : ImportHelper.GetPartOfString(s_product.Address2, '-', 1).Replace("+", "").Replace(":", "").Replace(".", "-").Replace("&", "");
                    p.Description = s_product.Desc1 + s_product.Desc2;

                    p.ImageUrl1 = s_product.ImageUrl1;
                    p.ImageUrl2 = s_product.ImageUrl2;
                    p.ImageUrl3 = s_product.ImageUrl3;
                    p.ImageUrl4 = s_product.ImageUrl4;
                    p.ImageUrl5 = s_product.ImageUrl5;
                    p.ProductTypeId = 1;

                    if (!string.IsNullOrEmpty(placeName))
                    {
                        string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBML9XUO8FpiHQShjmlL7QnIw7UkWheOQE", Uri.EscapeDataString(placeName));
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(requestUri);

                        var result = xdoc.SelectNodes("/GeocodeResponse/result/geometry/location");

                        foreach (XmlNode objXmlNode in result)
                        {
                            p.Lat = objXmlNode.ChildNodes.Item(0).InnerText;

                            p.Long = objXmlNode.ChildNodes.Item(1).InnerText;
                        }

                    }

                    SaveProduct(p);

                    products.Add(p);
                }
            }

            model.Products = products;
            model.NumberOfProducts = products.Count;

            return View(model);
        }

        public ActionResult CCHomesBroker()
        {
            int userId = 6;

            var model = new CCHomesViewModel();
            var ccProducts = _db.CcHomesProductRepository.GetAll().Where(x => x.Id > 1688).ToList();

            var products = new List<ProductItem>();

            foreach (var s_product in ccProducts)
            {
                var place = SetCCHomesPlaces(s_product.Place);

                if (place != null)
                {
                    var p = new ProductItem();
                    if (s_product.Price.Contains("SEK"))
                    {
                        p.Price = ImportHelper.GetDecimalFromString(s_product.Price) / 10;
                    }
                    else
                    {
                        p.Price = ImportHelper.GetDecimalFromString(s_product.Price);
                    }

                    p.SalesResponsibleId = userId;
                    p.PlaceId = place.Id;
                    p.DisplayPlace = place.NameSV;
                    p.LivingSpace = ImportHelper.GetFirstIntFromString(s_product.LivingSpace);
                    p.NumberOfRooms = ImportHelper.GetFirstIntFromString(s_product.NumberOfRooms);
                    p.ExternalLink = s_product.ExternalLink;

                    p.Address = s_product.Address;
                    p.Description = s_product.Description;

                    p.ImageUrl1 = ImportHelper.GetUrlFromString(s_product.ImageUrl1);
                    p.ImageUrl2 = ImportHelper.GetUrlFromString(s_product.ImageUrl2);
                    p.ImageUrl3 = ImportHelper.GetUrlFromString(s_product.ImageUrl3);
                    p.ImageUrl4 = ImportHelper.GetUrlFromString(s_product.ImageUrl4);
                    p.ImageUrl5 = ImportHelper.GetUrlFromString(s_product.ImageUrl5);
                    p.ProductTypeId = 1;

                    if (!string.IsNullOrEmpty(place.Name))
                    {
                        string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBML9XUO8FpiHQShjmlL7QnIw7UkWheOQE", Uri.EscapeDataString(place.Name));
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(requestUri);

                        var result = xdoc.SelectNodes("/GeocodeResponse/result/geometry/location");

                        foreach (XmlNode objXmlNode in result)
                        {
                            p.Lat = objXmlNode.ChildNodes.Item(0).InnerText;

                            p.Long = objXmlNode.ChildNodes.Item(1).InnerText;
                        }

                    }

                    SaveProduct(p);

                    products.Add(p);
                }
            }

           

            model.Products = products;
            model.NumberOfProducts = products.Count;

            return View(model);
        }

        public ActionResult CCHomesUpdateImages()
        {
            int userId = 6;

            var model = new CCHomesViewModel();
            var ccProducts = _db.ProductRepository.GetAll().Where(x => x.SalesResponsibleId == userId && (string.IsNullOrEmpty(x.ThumbnailUrl))).ToList();

            var products = new List<ProductItem>();

            foreach (var s_product in ccProducts)
            {
                var p = new ProductItem();
                var iProduct = _db.CcHomesProductRepository.GetAll().Where(i => i.ExternalLink == s_product.ExternalLink).FirstOrDefault();

                if (iProduct != null)
                {
                    p.Id = s_product.Id;
                    string img1 = ImportHelper.GetUrlFromString(iProduct.ImageUrl1).Replace("'", "");
                    string img2 = ImportHelper.GetUrlFromString(iProduct.ImageUrl2).Replace("'", "");
                    string img3 = ImportHelper.GetUrlFromString(iProduct.ImageUrl3).Replace("'", "");
                    string img4 = ImportHelper.GetUrlFromString(iProduct.ImageUrl4).Replace("'", "");
                    string img5 = ImportHelper.GetUrlFromString(iProduct.ImageUrl5).Replace("'", "");

                    p.ImageUrl1 = img1.Contains("http") ? img1 : "http://www.cchomes.se/" + img1;
                    p.ImageUrl2 = img2.Contains("http") ? img2 : "http://www.cchomes.se/" + img2;
                    p.ImageUrl3 = img3.Contains("http") ? img3 : "http://www.cchomes.se/" + img3;
                    p.ImageUrl4 = img4.Contains("http") ? img4 : "http://www.cchomes.se/" + img4;
                    p.ImageUrl5 = img5.Contains("http") ? img5 : "http://www.cchomes.se/" + img5;

                    UpdateProductImages(p, s_product);

                    products.Add(p);
                }
            }



            model.Products = products;
            model.NumberOfProducts = products.Count;

            return View(model);
        }

        public string TestRegex()
        {
            var ccHome = _db.CcHomesProductRepository.GetByID(10);

            StringBuilder builder = new StringBuilder();
            string[] lines = ccHome.Place.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0)
                    builder.Append("<br/>\n");
                builder.Append(WebUtility.HtmlEncode(lines[i]));
            }

            string firstplace = ImportHelper.GetPartOfString(builder.ToString(), "<br/>", 0);
            string secondplace = ImportHelper.GetPartOfString(builder.ToString(), "<br/>", 1);

            return "First place: " + firstplace + "<br />Second place: " + secondplace;
        }

        private void SaveProduct(ProductItem product)
        {
            var item = new Product();
            item.ProductTypeId = product.ProductTypeId;
            item.SalesResponsibleId = product.SalesResponsibleId;
            item.PlaceId = product.PlaceId;
            item.Description = product.Description;
            item.LivingSpace = product.LivingSpace;
            item.NumberOfRooms = product.NumberOfRooms;
            item.Price = product.Price;
            item.ExternalLink = product.ExternalLink;
            item.InsertDate = DateTime.Now;
            item.Address = product.Address;
            item.Lat = product.Lat;
            item.Long = product.Long;
            item.IsApproved = true;
            item.RefNr = product.RefNr;
            item.YearBuilt = product.YearBuilt;

            _db.ProductRepository.Insert(item);
            _db.SaveChanges();

            item.ThumbnailUrl = UploadImages(item.Id, product);
            _db.ProductRepository.Update(item);
            _db.SaveChanges();
            
        }

        private void UpdateProductImages(ProductItem product, Product updateProduct)
        {
            if (updateProduct != null && product != null)
            {
                updateProduct.ThumbnailUrl = UploadImages(updateProduct.Id, product);
                _db.ProductRepository.Update(updateProduct);

                _db.SaveChanges();
            }
        }

        public string UploadImages(int productId, ProductItem product)
        {
            string thumbnailUrl = string.Empty;
            bool hasImage = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            string containerName = string.Format("product{0}", productId);

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            container.CreateIfNotExistsAsync();
            SetPublicContainerPermissions(container);

            string blobName = string.Empty;
            int index = 0;

            WebClient client = new WebClient();

            if (!string.IsNullOrEmpty(product.ImageUrl1))
            {
                index++;
                blobName = string.Format("{0}_{1}", containerName, index.ToString());
                CloudBlockBlob newBlob = container.GetBlockBlobReference(blobName);
                byte[] imageBytes = client.DownloadData(product.ImageUrl1);

                using (var stream = new MemoryStream(imageBytes, writable: false))
                {
                    newBlob.UploadFromStream(stream);
                }

                hasImage = true;
            }

            if (!string.IsNullOrEmpty(product.ImageUrl2))
            {
                index++;
                blobName = string.Format("{0}_{1}", containerName, index.ToString());
                CloudBlockBlob newBlob = container.GetBlockBlobReference(blobName);
                byte[] imageBytes = client.DownloadData(product.ImageUrl2);
                using (var stream = new MemoryStream(imageBytes, writable: false))
                {
                    newBlob.UploadFromStream(stream);
                }

                hasImage = true;
            }

            if (!string.IsNullOrEmpty(product.ImageUrl3))
            {
                try
                {
                    index++;
                    blobName = string.Format("{0}_{1}", containerName, index.ToString());
                    CloudBlockBlob newBlob = container.GetBlockBlobReference(blobName);
                    byte[] imageBytes = client.DownloadData(product.ImageUrl3);
                    using (var stream = new MemoryStream(imageBytes, writable: false))
                    {
                        newBlob.UploadFromStream(stream);
                    }

                    hasImage = true;
                }
                catch (Exception)
                {

                }
               
            }

            if (!string.IsNullOrEmpty(product.ImageUrl4))
            {
                index++;
                blobName = string.Format("{0}_{1}", containerName, index.ToString());
                CloudBlockBlob newBlob = container.GetBlockBlobReference(blobName);
                byte[] imageBytes = client.DownloadData(product.ImageUrl4);
                using (var stream = new MemoryStream(imageBytes, writable: false))
                {
                    newBlob.UploadFromStream(stream);
                }

                hasImage = true;
            }

            if (!string.IsNullOrEmpty(product.ImageUrl5))
            {
                index++;
                blobName = string.Format("{0}_{1}", containerName, index.ToString());
                CloudBlockBlob newBlob = container.GetBlockBlobReference(blobName);
                byte[] imageBytes = client.DownloadData(product.ImageUrl5);
                using (var stream = new MemoryStream(imageBytes, writable: false))
                {
                    newBlob.UploadFromStream(stream);
                }

                hasImage = true;
            }

            if (hasImage)
            {
                IListBlobItem item = container.ListBlobs(useFlatBlobListing: true).FirstOrDefault();

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    thumbnailUrl = blob.Uri.ToString();
                }
            }

            return thumbnailUrl;
        }

        private int SetCreatePlaceId(string name, int parentId)
        {
            int placeId = 0;

            if (!string.IsNullOrEmpty(name))
            {
                var places = _db.PlaceRepository.GetAll();

                Place place = new Place();
                bool placeExist = false;

                foreach (var p in places)
                {
                    if (p.Name.ToLower().Trim() == name.ToLower().Trim())
                    {
                        place = p;
                        placeExist = true;
                    }
                }

                if (!placeExist)
                {
                    place.Name = name;
                    place.NameSV = name;
                    place.ParentId = parentId;
                    place.PlaceTypeId = 3;

                    _db.PlaceRepository.Insert(place);
                    _db.SaveChanges();
                }

                placeId = place.Id;

            }
            else
            {
                placeId = 1;
            }


            return placeId;
        }

        private int SetCreateWretmanPlaceId(string name, int parentId)
        {
            int placeId = 0;

            if (!string.IsNullOrEmpty(name))
            {
                var places = _db.PlaceRepository.GetAll();

                Place place = new Place();
                bool placeExist = false;

                foreach (var p in places)
                {
                    if (p.Name.ToLower().Trim() == name.ToLower().Trim())
                    {
                        place = p;
                        placeExist = true;
                    }
                }

                if (!placeExist)
                {
                    place.Name = Helper.FirstCharToUpper(name.ToLower());
                    place.NameSV = Helper.FirstCharToUpper(name.ToLower());
                    place.ParentId = parentId;
                    place.PlaceTypeId = 3;

                    _db.PlaceRepository.Insert(place);
                    _db.SaveChanges();
                }

                placeId = place.Id;

            }
            else
            {
                placeId = parentId;
            }


            return placeId;
        }

        private Place SetCCHomesPlaces(string placeInput)
        {
            Place place = new Place();

            if (!string.IsNullOrEmpty(placeInput))
            {
                var places = _db.PlaceRepository.GetAll();

                StringBuilder builder = new StringBuilder();
                string[] lines = placeInput.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                        builder.Append("<br/>\n");
                    builder.Append(WebUtility.HtmlEncode(lines[i]));
                }

                string firstplace = string.Empty;
                string secondplace = string.Empty;

                if (builder.ToString().Contains("<br/>\n<br/>"))
                {
                    firstplace = ImportHelper.GetPartOfString(builder.ToString(), "<br/>\n<br/>", 0);
                    secondplace = ImportHelper.GetPartOfString(builder.ToString(), "<br/>\n<br/>", 1);
                }
                else
                {
                    firstplace = ImportHelper.GetPartOfString(builder.ToString(), "<br/>", 0);
                    secondplace = ImportHelper.GetPartOfString(builder.ToString(), "<br/>", 1);
                }
               
                int? sPlaceId = null;

                if (!string.IsNullOrEmpty(secondplace))
                {
                    Place sPlace = new Place();
                    bool placeExist = false;

                    foreach (var p in places)
                    {
                        if (p.Name.ToLower().Trim() == WebUtility.HtmlDecode(secondplace).ToLower().Trim())
                        {
                            sPlace = p;
                            placeExist = true;
                        }
                    }

                    if (!placeExist)
                    {
                        sPlace.Name = Helper.FirstCharToUpper(WebUtility.HtmlDecode(secondplace).ToLower().Trim());
                        sPlace.NameSV = Helper.FirstCharToUpper(WebUtility.HtmlDecode(secondplace).ToLower().Trim());
                        sPlace.PlaceTypeId = 2;

                        _db.PlaceRepository.Insert(sPlace);
                        _db.SaveChanges();
                    }

                    sPlaceId = sPlace.Id;
                }

                if (!string.IsNullOrEmpty(firstplace))
                {
                    
                    bool placeExist = false;

                    foreach (var p in places)
                    {
                        if (p.Name.ToLower().Trim() == WebUtility.HtmlDecode(firstplace).ToLower().Trim())
                        {
                            place = p;
                            placeExist = true;
                        }
                    }

                    if (!placeExist)
                    {
                        place.Name = Helper.FirstCharToUpper(WebUtility.HtmlDecode(firstplace).ToLower().Trim());
                        place.NameSV = Helper.FirstCharToUpper(WebUtility.HtmlDecode(firstplace).ToLower().Trim());
                        place.PlaceTypeId = 3;
                        place.ParentId = sPlaceId.HasValue ? sPlaceId.Value : (int?)null;

                        _db.PlaceRepository.Insert(place);
                        _db.SaveChanges();
                    }
                }

            }

            return place;
        }

        public void UpdateContainerAccess(int productId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mowido;AccountKey=9XToqEMRMPAcugE0E8gio2IO1vhm3kZxw6T8gNdIdKOBCQ9PiEHycrvi7uBob2Mk/u5WyNpUSnoESmsxdqFnaw==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            string containerName = string.Format("product{0}", productId);

            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            SetPublicContainerPermissions(container);
        }

        public static void SetPublicContainerPermissions(CloudBlobContainer container)
        {
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);
        }

        public string ReplaceSemiColumn()
        {
            var products = _db.ProductRepository.GetAll().Where(p => p.Address.Contains(":")).ToList();

            foreach (var p in products)
            {
                p.Address = p.Address.Replace(":", "");

                _db.ProductRepository.Update(p);
            }

            _db.SaveChanges();

            return "succeded:" + products.Count.ToString();
        }

        public string ReplaceAndSign()
        {
            var products = _db.ProductRepository.GetAll().Where(p => p.Address.Contains("&")).ToList();

            foreach (var p in products)
            {
                p.Address = p.Address.Replace("&", "");

                _db.ProductRepository.Update(p);
            }

            _db.SaveChanges();

            return "succeded:" + products.Count.ToString();
        }

        public string ReplaceDot()
        {
            var products = _db.ProductRepository.GetAll().Where(p => p.Address.Contains(".")).ToList();

            foreach (var p in products)
            {
                p.Address = p.Address.Replace(".", "");

                _db.ProductRepository.Update(p);
            }

            _db.SaveChanges();

            return "succeded:" + products.Count.ToString();
        }

        public string ReplacePlus()
        {
            var products = _db.ProductRepository.GetAll().Where(p => p.Address.Contains("+")).ToList();

            foreach (var p in products)
            {
                p.Address = p.Address.Replace("+", "");

                _db.ProductRepository.Update(p);
            }

            _db.SaveChanges();

            return "succeded:" + products.Count.ToString();
        }

        

        public string SetLongLatPlaces()
        {
            var places = _db.PlaceRepository.GetAll().Where(p => string.IsNullOrEmpty(p.Lat)).ToList();

            foreach (var p in places)
            {
                if (!string.IsNullOrEmpty(p.Name))
                {
                    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyBML9XUO8FpiHQShjmlL7QnIw7UkWheOQE", Uri.EscapeDataString(p.Name));
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(requestUri);

                    var result = xdoc.SelectNodes("/GeocodeResponse/result/geometry/location");

                    if (result != null)
                    {
                        foreach (XmlNode objXmlNode in result)
                        {
                            p.Lat = objXmlNode.ChildNodes.Item(0).InnerText;

                            p.Long = objXmlNode.ChildNodes.Item(1).InnerText;
                        }

                        _db.PlaceRepository.Update(p);
                    }
                }
            }

            _db.SaveChanges();

            return places.Count.ToString();
        }

        public string UpdateLongLatProducts()
        {
            var products = _db.ProductRepository.GetAll().Where(p => String.IsNullOrEmpty(p.Lat)).ToList();

            foreach (var p in products)
            {
                var place = _db.PlaceRepository.GetByID(p.PlaceId);

                p.Lat = place.Lat;
                p.Long = place.Long;

                _db.ProductRepository.Update(p);
            }

            _db.SaveChanges();

            return products.Count.ToString();
        }
    }
}