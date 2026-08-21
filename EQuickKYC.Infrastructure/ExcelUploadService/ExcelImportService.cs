using CsvHelper;
using EQuickKYC.Application.DTOs.Bank;
using EQuickKYC.Application.ExcelUpload;
using EQuickKYC.Domain.Entities;
using EQuickKYC.Infrastructure.Data;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace EQuickKYC.Infrastructure.ExcelUploadService
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly EQuickKYCDbContext _dbContext;
        public ExcelImportService(EQuickKYCDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BankUploadResponseDto> ImportSalesAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default)
        {
            System.Text.Encoding.RegisterProvider(
                System.Text.CodePagesEncodingProvider.Instance);

            var connection = (SqlConnection)_dbContext.Database.GetDbConnection();

            await connection.OpenAsync(cancellationToken);

            if (extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = ExcelReaderFactory.CreateReader(fileStream);

                // Process each worksheet
                do
                {
                    await ImportSheetAsync(reader, connection, cancellationToken);

                } while (reader.NextResult());
            }
            extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);

            var data = await ImportCsvAsync(fileStream, connection, cancellationToken);
            return data;

        }

        private async Task ImportSheetAsync(IExcelDataReader reader, SqlConnection connection, CancellationToken cancellationToken)
        {
            // Skip header
            if (!reader.Read()) return;

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.SalesRecords",

                BatchSize = 10_000,

                BulkCopyTimeout = 0,

                EnableStreaming = true
            };

            bulkCopy.NotifyAfter = 10_000;

            bulkCopy.SqlRowsCopied += (sender, e) =>
            {
                Console.WriteLine($"Rows copied: {e.RowsCopied:N0}");
            };

            bulkCopy.ColumnMappings.Add(0, "Region");

            bulkCopy.ColumnMappings.Add(1, "Country");

            bulkCopy.ColumnMappings.Add(2, "ItemType");

            bulkCopy.ColumnMappings.Add(3, "SalesChannel");

            bulkCopy.ColumnMappings.Add(4, "OrderPriority");

            bulkCopy.ColumnMappings.Add(5, "OrderDate");

            bulkCopy.ColumnMappings.Add(6, "OrderId");

            bulkCopy.ColumnMappings.Add(7, "ShipDate");

            bulkCopy.ColumnMappings.Add(8, "UnitsSold");

            bulkCopy.ColumnMappings.Add(9, "UnitPrice");

            bulkCopy.ColumnMappings.Add(10, "UnitCost");

            bulkCopy.ColumnMappings.Add(11, "TotalRevenue");

            bulkCopy.ColumnMappings.Add(12, "TotalCost");

            bulkCopy.ColumnMappings.Add(13, "TotalProfit");

            await bulkCopy.WriteToServerAsync(reader, cancellationToken);
        }

        private async Task<BankUploadResponseDto> ImportCsvAsync(Stream fileStream, SqlConnection connection, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            using var streamReader = new StreamReader(fileStream);

            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var records = new List<SalesDataDemo>();

            int failedRows = 0;

            await csv.ReadAsync();
            csv.ReadHeader();

            while (await csv.ReadAsync())
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    records.Add(new SalesDataDemo
                    {
                        Region = csv.GetField("Region") ?? "",
                        Country = csv.GetField("Country") ?? "",
                        ItemType = csv.GetField("Item Type") ?? "",
                        SalesChannel = csv.GetField("Sales Channel") ?? "",
                        OrderPriority = csv.GetField("Order Priority") ?? "",

                        OrderDate = csv.GetField<DateTime>("Order Date"),
                        OrderId = csv.GetField<long>("Order ID"),
                        ShipDate = csv.GetField<DateTime>("Ship Date"),

                        UnitsSold = csv.GetField<int>("Units Sold"),

                        UnitPrice = csv.GetField<decimal>("Unit Price"),
                        UnitCost = csv.GetField<decimal>("Unit Cost"),
                        TotalRevenue = csv.GetField<decimal>("Total Revenue"),
                        TotalCost = csv.GetField<decimal>("Total Cost"),
                        TotalProfit = csv.GetField<decimal>("Total Profit")
                    });
                }
                catch (Exception ex)
                {
                    failedRows++;

                    Console.WriteLine(
                        $"Failed CSV row: {csv.Parser.Row} | {ex.Message}");
                }
            }

            int totalRows = records.Count + failedRows;

            Console.WriteLine($"Total rows: {totalRows:N0}");
            Console.WriteLine($"Valid rows: {records.Count:N0}");
            Console.WriteLine($"Failed rows: {failedRows:N0}");

            var table = new DataTable();

            table.Columns.Add("Region", typeof(string));
            table.Columns.Add("Country", typeof(string));
            table.Columns.Add("ItemType", typeof(string));
            table.Columns.Add("SalesChannel", typeof(string));
            table.Columns.Add("OrderPriority", typeof(string));
            table.Columns.Add("OrderDate", typeof(DateTime));
            table.Columns.Add("OrderId", typeof(long));
            table.Columns.Add("ShipDate", typeof(DateTime));
            table.Columns.Add("UnitsSold", typeof(int));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("UnitCost", typeof(decimal));
            table.Columns.Add("TotalRevenue", typeof(decimal));
            table.Columns.Add("TotalCost", typeof(decimal));
            table.Columns.Add("TotalProfit", typeof(decimal));

            foreach (var record in records)
            {
                table.Rows.Add(
                    record.Region,
                    record.Country,
                    record.ItemType,
                    record.SalesChannel,
                    record.OrderPriority,
                    record.OrderDate,
                    record.OrderId,
                    record.ShipDate,
                    record.UnitsSold,
                    record.UnitPrice,
                    record.UnitCost,
                    record.TotalRevenue,
                    record.TotalCost,
                    record.TotalProfit);
            }

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "dbo.SalesRecords",
                BatchSize = 10_000,
                BulkCopyTimeout = 0
            };

            bulkCopy.NotifyAfter = 10_000;

            bulkCopy.SqlRowsCopied += (sender, e) =>
            {
                Console.WriteLine(
                    $"Saved: {e.RowsCopied:N0} / {totalRows:N0}");
            };

            bulkCopy.ColumnMappings.Add("Region", "Region");
            bulkCopy.ColumnMappings.Add("Country", "Country");
            bulkCopy.ColumnMappings.Add("ItemType", "ItemType");
            bulkCopy.ColumnMappings.Add("SalesChannel", "SalesChannel");
            bulkCopy.ColumnMappings.Add("OrderPriority", "OrderPriority");
            bulkCopy.ColumnMappings.Add("OrderDate", "OrderDate");
            bulkCopy.ColumnMappings.Add("OrderId", "OrderId");
            bulkCopy.ColumnMappings.Add("ShipDate", "ShipDate");
            bulkCopy.ColumnMappings.Add("UnitsSold", "UnitsSold");
            bulkCopy.ColumnMappings.Add("UnitPrice", "UnitPrice");
            bulkCopy.ColumnMappings.Add("UnitCost", "UnitCost");
            bulkCopy.ColumnMappings.Add("TotalRevenue", "TotalRevenue");
            bulkCopy.ColumnMappings.Add("TotalCost", "TotalCost");
            bulkCopy.ColumnMappings.Add("TotalProfit", "TotalProfit");

            try
            {
                await bulkCopy.WriteToServerAsync(table, cancellationToken);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                Console.WriteLine($"Bulk insert failed: {ex.Message}");
                Console.WriteLine($"Time taken: {stopwatch.Elapsed}");

                throw;
            }

            stopwatch.Stop();

            return new BankUploadResponseDto
            {
                TotalRows = totalRows,
                SavedRows = records.Count,
                FailedRows = failedRows,
                TimeTaken = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff")
            };

            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Total rows   : {totalRows:N0}");
            Console.WriteLine($"Saved rows   : {records.Count:N0}");
            Console.WriteLine($"Failed rows  : {failedRows:N0}");
            Console.WriteLine($"Time taken   : {stopwatch.Elapsed}");
            Console.WriteLine("----------------------------------");
        }


        //private async Task ImportCsvAsync(Stream fileStream, SqlConnection connection, CancellationToken cancellationToken)
        //{
        //    using var streamReader = new StreamReader(fileStream);

        //    var totalRows = 0;

        //    while (await streamReader.ReadLineAsync() != null)
        //    {
        //        totalRows++;
        //    }

        //    // Subtract header
        //    totalRows--;

        //    Console.WriteLine($"Total rows: {totalRows:N0}");

        //    using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        //    var records = new List<SalesDataDemo>();

        //    await csv.ReadAsync();
        //    csv.ReadHeader();
        //    //Stopwatch sw = Stopwatch.StartNew();
        //    while (await csv.ReadAsync())
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();

        //        records.Add(new SalesDataDemo
        //        {
        //            Region = csv.GetField("Region") ?? "",
        //            Country = csv.GetField("Country") ?? "",
        //            ItemType = csv.GetField("Item Type") ?? "",
        //            SalesChannel = csv.GetField("Sales Channel") ?? "",
        //            OrderPriority = csv.GetField("Order Priority") ?? "",

        //            OrderDate = csv.GetField<DateTime>("Order Date"),
        //            OrderId = csv.GetField<long>("Order ID"),
        //            ShipDate = csv.GetField<DateTime>("Ship Date"),

        //            UnitsSold = csv.GetField<int>("Units Sold"),

        //            UnitPrice = csv.GetField<decimal>("Unit Price"),
        //            UnitCost = csv.GetField<decimal>("Unit Cost"),
        //            TotalRevenue = csv.GetField<decimal>("Total Revenue"),
        //            TotalCost = csv.GetField<decimal>("Total Cost"),
        //            TotalProfit = csv.GetField<decimal>("Total Profit")
        //        });
        //    }

        //    var table = new DataTable();

        //    table.Columns.Add("Region", typeof(string));
        //    table.Columns.Add("Country", typeof(string));
        //    table.Columns.Add("ItemType", typeof(string));
        //    table.Columns.Add("SalesChannel", typeof(string));
        //    table.Columns.Add("OrderPriority", typeof(string));
        //    table.Columns.Add("OrderDate", typeof(DateTime));
        //    table.Columns.Add("OrderId", typeof(long));
        //    table.Columns.Add("ShipDate", typeof(DateTime));
        //    table.Columns.Add("UnitsSold", typeof(int));
        //    table.Columns.Add("UnitPrice", typeof(decimal));
        //    table.Columns.Add("UnitCost", typeof(decimal));
        //    table.Columns.Add("TotalRevenue", typeof(decimal));
        //    table.Columns.Add("TotalCost", typeof(decimal));
        //    table.Columns.Add("TotalProfit", typeof(decimal));

        //    foreach (var record in records)
        //    {
        //        table.Rows.Add(
        //            record.Region,
        //            record.Country,
        //            record.ItemType,
        //            record.SalesChannel,
        //            record.OrderPriority,
        //            record.OrderDate,
        //            record.OrderId,
        //            record.ShipDate,
        //            record.UnitsSold,
        //            record.UnitPrice,
        //            record.UnitCost,
        //            record.TotalRevenue,
        //            record.TotalCost,
        //            record.TotalProfit);
        //    }

        //    using var bulkCopy = new SqlBulkCopy(connection)
        //    {
        //        DestinationTableName = "dbo.SalesRecords",
        //        BatchSize = 10_000,
        //        BulkCopyTimeout = 0
        //    };

        //    bulkCopy.NotifyAfter = 10_000;

        //    bulkCopy.SqlRowsCopied += (sender, e) =>
        //    {
        //        Console.WriteLine($"Rows copied: {e.RowsCopied:N0}");
        //    };

        //    // Explicit mappings
        //    bulkCopy.ColumnMappings.Add("Region", "Region");
        //    bulkCopy.ColumnMappings.Add("Country", "Country");
        //    bulkCopy.ColumnMappings.Add("ItemType", "ItemType");
        //    bulkCopy.ColumnMappings.Add("SalesChannel", "SalesChannel");
        //    bulkCopy.ColumnMappings.Add("OrderPriority", "OrderPriority");
        //    bulkCopy.ColumnMappings.Add("OrderDate", "OrderDate");
        //    bulkCopy.ColumnMappings.Add("OrderId", "OrderId");
        //    bulkCopy.ColumnMappings.Add("ShipDate", "ShipDate");
        //    bulkCopy.ColumnMappings.Add("UnitsSold", "UnitsSold");
        //    bulkCopy.ColumnMappings.Add("UnitPrice", "UnitPrice");
        //    bulkCopy.ColumnMappings.Add("UnitCost", "UnitCost");
        //    bulkCopy.ColumnMappings.Add("TotalRevenue", "TotalRevenue");
        //    bulkCopy.ColumnMappings.Add("TotalCost", "TotalCost");
        //    bulkCopy.ColumnMappings.Add("TotalProfit", "TotalProfit");

        //    await bulkCopy.WriteToServerAsync(table, cancellationToken);
        //}
    }
}

