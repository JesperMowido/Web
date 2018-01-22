using Microsoft.Azure.Search;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;
using BC.Search.SearchModel;

namespace BC.Search
{
    public class BillionSearch
    {
        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;

        /// <summary>
        ///  Test search index
        /// </summary>
        //private const string SearchServiceName = "mowido";
        //private const string SearchServiceApiKey = "D188997865133E3DBC4CE0924441E8BA";


        /// <summary>
        ///  Production search index
        /// </summary>
        private const string SearchServiceName = "mowido-prod";
        private const string SearchServiceApiKey = "AC53D4031AB2E8A71EAC4DDB10067FDA";
        private const string SearchServiceNameProd = "mowido-prod";
        private const string SearchServiceApiKeyProd = "AC53D4031AB2E8A71EAC4DDB10067FDA";

        public static string errorMessage;

        static BillionSearch()
        {
            try
            {
                //PROD
                string searchServiceName = SearchServiceNameProd;
                string apiKey = SearchServiceApiKeyProd;

                //TEST
                //string searchServiceName = SearchServiceName;
                //string apiKey = SearchServiceApiKey;

                // Create an HTTP reference to the catalog index
                _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
                _indexClient = _searchClient.Indexes.GetClient("mowido-index");
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        

        public DocumentSearchResult Search(string searchText, string host)
        {
            // Execute search based on query string
            List<string> fields = new List<string>();

            try
            {
                if (host.Contains("mowido.se"))
                {
                    fields.Add("NameSV");
                    SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All, SearchFields = fields };
                    return _indexClient.Documents.Search(searchText + "*", sp);
                }
                else
                {
                    fields.Add("Name");
                    SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All };
                    return _indexClient.Documents.Search(searchText + "*", sp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }
    }

    public class BillionProductSearch
    {
        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;
        private const string SearchServiceName = "mowido";
        private const string SearchServiceApiKey = "D188997865133E3DBC4CE0924441E8BA";
        private const string SearchServiceNameProd = "mowido-prod";
        private const string SearchServiceApiKeyProd = "AC53D4031AB2E8A71EAC4DDB10067FDA";

        public static string errorMessage;
        static BillionProductSearch()
        {
            try
            {
                //PROD
                string searchServiceName = SearchServiceNameProd;
                string apiKey = SearchServiceApiKeyProd;

                //TEST
                //string searchServiceName = SearchServiceName;
                //string apiKey = SearchServiceApiKeyProd;

                // Create an HTTP reference to the catalog index
                _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
                _indexClient = _searchClient.Indexes.GetClient("mowido-product-index");
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        //public DocumentSearchResult<SearchProduct> Search(List<string> placeIds, int skip)
        //{
        //    return Search(placeIds, skip, null, null);
        //}

        //public DocumentSearchResult<SearchProduct> Search(List<string> placeIds, int skip, string sort)
        //{
        //    return Search(placeIds, skip, sort, string.Empty);
        //}

        public DocumentSearchResult<SearchProduct> Search(List<string> placeIds, int skip, string sort, string filter)
        {
            // Execute search based on query string
            // ADD FILER: Filter = "LivingSpace lt 150"
            List<string> fields = new List<string>();

            if (string.IsNullOrEmpty(sort))
            {
                sort = "InsertDate desc";
            }

            try
            {
                fields.Add("FKPlaceID");
                SearchParameters sp = new SearchParameters()
                {
                    SearchFields = fields,
                    Top = 20,
                    Skip = skip,
                    IncludeTotalResultCount = true,
                    OrderBy = new[] { sort }
                };

                if (!string.IsNullOrEmpty(filter))
                {
                    sp.Filter = filter;
                }

                return _indexClient.Documents.Search<SearchProduct>(string.Join(", ", placeIds), sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }
    }
}
