using System;
using System.Configuration;
using System.Threading;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace DataIndexer
{
    class Program
    {
        private const string MowidoProductIndex = "mowido-product-index";
        private const string MowidoIndex = "mowido-index";
        private const string MowidoDataSource = "mowido-datasource";
        private const string MowidoIndexer = "mowido-indexer";

        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;
        private static ISearchIndexClient _indexProductClient;

        // This Sample shows how to delete, create, upload documents and query an index
        static void Main(string[] args)
        {
            //string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            //string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

            string searchServiceName = ConfigurationManager.AppSettings["ProdSearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["ProdSearchServiceApiKey"];

            // Create an HTTP reference to the catalog index
            _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            _indexClient = _searchClient.Indexes.GetClient(MowidoIndex);

            Console.WriteLine("{0}", "Deleting index, data source, and indexer...\n");
            if (DeleteIndexingResources())
            {
                Console.WriteLine("{0}", "Creating index...\n");
                CreateIndex();
                Console.WriteLine("{0}", "Sync documents from Azure SQL...\n");
                SyncDataFromAzureSQL();
            }
            Console.WriteLine("{0}", "Complete.  Press any key to end application...\n");
            Console.ReadKey();

            _indexProductClient = _searchClient.Indexes.GetClient(MowidoProductIndex);

            Console.WriteLine("{0}", "Deleting index, data source, and indexer...\n");
            if (DeleteProductIndexingResources())
            {
                Console.WriteLine("{0}", "Creating index...\n");
                CreateProductIndex();
                Console.WriteLine("{0}", "Sync documents from Azure SQL...\n");
                SyncProductDataFromAzureSQL();
            }
            Console.WriteLine("{0}", "Complete.  Press any key to end application...\n");
            Console.ReadKey();
        }

        private static bool DeleteIndexingResources()
        {
            // Delete the index, data source, and indexer.
            try
            {
                _searchClient.Indexes.Delete(MowidoIndex);
                _searchClient.DataSources.Delete(MowidoDataSource);
                _searchClient.Indexers.Delete(MowidoIndexer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting indexing resources: {0}\r\n", ex.Message);
                Console.WriteLine("Did you remember to add your SearchServiceName and SearchServiceApiKey to the app.config?\r\n");
                return false;
            }

            return true;
        }

        private static bool DeleteProductIndexingResources()
        {
            // Delete the index, data source, and indexer.
            try
            {
                _searchClient.Indexes.Delete(MowidoProductIndex);
                _searchClient.DataSources.Delete(MowidoDataSource);
                _searchClient.Indexers.Delete(MowidoIndexer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting indexing resources: {0}\r\n", ex.Message);
                Console.WriteLine("Did you remember to add your SearchServiceName and SearchServiceApiKey to the app.config?\r\n");
                return false;
            }

            return true;
        }

        private static void CreateIndex()
        {
            // Create the Azure Search index based on the included schema
            try
            {
                var definition = new Index()
                {
                    Name = MowidoIndex,
                    Fields = new[] 
                    { 
                        new Field("ID",             DataType.String)         { IsKey = true,  IsSearchable = false, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true},
                        new Field("FKParentID",     DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("FKPlaceTypeID",  DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Name",           DataType.String)         { IsKey = false, IsSearchable = true,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("NameSV",         DataType.String)         { IsKey = false, IsSearchable = true, IsFilterable = true,  IsSortable = true,  IsFacetable = true,  IsRetrievable = true},
                        new Field("Lat",            DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Long",           DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true}
                    }
                };

                _searchClient.Indexes.Create(definition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating index: {0}\r\n", ex.Message);
            }

        }

        private static void CreateProductIndex()
        {
            // Create the Azure Search index based on the included schema
            try
            {
                var definition = new Index()
                {
                    Name = MowidoProductIndex,
                    Fields = new[]
                    {
                        new Field("ID",                 DataType.String)         { IsKey = true,  IsSearchable = false, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true},
                        new Field("FKProductTypeID",    DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("SalesResponsibleID", DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("FKPlaceID",          DataType.String)          { IsKey = false, IsSearchable = true,  IsFilterable = true,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Description",        DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("LivingSpace",        DataType.Int32)          { IsKey = false, IsSearchable = false, IsFilterable = true,  IsSortable = true,  IsFacetable = false,  IsRetrievable = true},
                        new Field("NumberOfRooms",      DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("Price",              DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("MonthlyCharge",      DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("SquareMeterPrice",   DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("YearBuilt",          DataType.DateTimeOffset) { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("ExternalLink",       DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("InsertDate",         DataType.DateTimeOffset) { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("Address",            DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Lat",                DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Long",               DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("PlotSize",           DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("ThumbnailUrl",       DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("PriceFilter",        DataType.Double)         { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true}
                    }
                };

                _searchClient.Indexes.Create(definition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating index: {0}\r\n", ex.Message);
            }

        }

        private static void CreateProductFilterSortIndex()
        {
            // Create the Azure Search index based on the included schema
            try
            {
                var definition = new Index()
                {
                    Name = MowidoProductIndex,
                    Fields = new[]
                    {
                        new Field("ID",                 DataType.String)         { IsKey = true,  IsSearchable = false, IsFilterable = false, IsSortable = false, IsFacetable = false, IsRetrievable = true},
                        new Field("FKProductTypeID",    DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("SalesResponsibleID", DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("FKPlaceID",          DataType.String)         { IsKey = false, IsSearchable = true,  IsFilterable = true,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Description",        DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("LivingSpace",        DataType.Int32)          { IsKey = false, IsSearchable = false, IsFilterable = true,  IsSortable = true,  IsFacetable = false,  IsRetrievable = true},
                        new Field("NumberOfRooms",      DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("Price",              DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("MonthlyCharge",      DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("SquareMeterPrice",   DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("YearBuilt",          DataType.DateTimeOffset) { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("ExternalLink",       DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("InsertDate",         DataType.DateTimeOffset) { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = true,  IsFacetable = false, IsRetrievable = true},
                        new Field("Address",            DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Lat",                DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("Long",               DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("PlotSize",           DataType.Int32)          { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("ThumbnailUrl",       DataType.String)         { IsKey = false, IsSearchable = false,  IsFilterable = false,  IsSortable = false,  IsFacetable = false, IsRetrievable = true},
                        new Field("PriceFilter",        DataType.Double)         { IsKey = false, IsSearchable = false,  IsFilterable = true,  IsSortable = true,  IsFacetable = false, IsRetrievable = true}
                    }
                };

                _searchClient.Indexes.Create(definition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating index: {0}\r\n", ex.Message);
            }

        }

        private static void SyncDataFromAzureSQL()
        {
            // This will use the Azure Search Indexer to synchronize data from Azure SQL to Azure Search
            Console.WriteLine("{0}", "Creating Data Source...\n");
            var dataSource =
                DataSource.AzureSql(
                    name: MowidoDataSource,
                    sqlConnectionString: "Server=tcp:mowido.database.windows.net,1433;Initial Catalog=MowidoDB;Persist Security Info=False;User ID=mowido;Password=Billions123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;",
                    tableOrViewName: "Places",
                    description: "Mowido Dataset");

            try
            {
                _searchClient.DataSources.Create(dataSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating data source: {0}", ex.Message);
                return;
            }

            Console.WriteLine("{0}", "Creating Indexer and syncing data...\n");

            var indexer =
                new Indexer()
                {
                    Name = MowidoIndexer,
                    Description = "Mowido start search data indexer",
                    DataSourceName = dataSource.Name,
                    TargetIndexName = MowidoIndex
                };

            try
            {
                _searchClient.Indexers.Create(indexer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating and running indexer: {0}", ex.Message);
                return;
            }

            bool running = true;
            Console.WriteLine("{0}", "Synchronization running...\n");
            while (running)
            {
                IndexerExecutionInfo status = null;

                try
                {
                    status = _searchClient.Indexers.GetStatus(indexer.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error polling for indexer status: {0}", ex.Message);
                    return;
                }

                IndexerExecutionResult lastResult = status.LastResult;
                if (lastResult != null)
                {
                    switch (lastResult.Status)
                    {
                        case IndexerExecutionStatus.InProgress:
                            Console.WriteLine("{0}", "Synchronization running...\n");
                            Thread.Sleep(1000);
                            break;

                        case IndexerExecutionStatus.Success:
                            running = false;
                            Console.WriteLine("Synchronized {0} rows...\n", lastResult.ItemCount);
                            break;

                        default:
                            running = false;
                            Console.WriteLine("Synchronization failed: {0}\n", lastResult.ErrorMessage);
                            break;
                    }
                }
            }
        }

        private static void SyncProductDataFromAzureSQL()
        {
            // This will use the Azure Search Indexer to synchronize data from Azure SQL to Azure Search
            Console.WriteLine("{0}", "Creating Product Data Source...\n");
            var dataSource =
                DataSource.AzureSql(
                    name: MowidoDataSource,
                    sqlConnectionString: "Server=tcp:mowido.database.windows.net,1433;Initial Catalog=MowidoDB;Persist Security Info=False;User ID=mowido;Password=Billions123;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;",
                    tableOrViewName: "Products",
                    description: "Mowido Dataset");

            try
            {
                _searchClient.DataSources.Create(dataSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating data source: {0}", ex.Message);
                return;
            }

            Console.WriteLine("{0}", "Creating Indexer and syncing data...\n");

            var indexer =
                new Indexer()
                {
                    Name = MowidoIndexer,
                    Description = "Mowido start search product data indexer",
                    DataSourceName = dataSource.Name,
                    TargetIndexName = MowidoProductIndex
                };

            try
            {
                _searchClient.Indexers.Create(indexer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating and running indexer: {0}", ex.Message);
                return;
            }

            bool running = true;
            Console.WriteLine("{0}", "Synchronization running...\n");
            while (running)
            {
                IndexerExecutionInfo status = null;

                try
                {
                    status = _searchClient.Indexers.GetStatus(indexer.Name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error polling for indexer status: {0}", ex.Message);
                    return;
                }

                IndexerExecutionResult lastResult = status.LastResult;
                if (lastResult != null)
                {
                    switch (lastResult.Status)
                    {
                        case IndexerExecutionStatus.InProgress:
                            Console.WriteLine("{0}", "Synchronization running...\n");
                            Thread.Sleep(1000);
                            break;

                        case IndexerExecutionStatus.Success:
                            running = false;
                            Console.WriteLine("Synchronized {0} rows...\n", lastResult.ItemCount);
                            break;

                        default:
                            running = false;
                            Console.WriteLine("Synchronization failed: {0}\n", lastResult.ErrorMessage);
                            break;
                    }
                }
            }
        }
    }
}
