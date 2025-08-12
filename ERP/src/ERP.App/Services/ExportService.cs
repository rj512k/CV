using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using ERP.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ERP.App.Services;

public interface IExportService
{
    byte[] ExportInventoryToExcel(IEnumerable<Product> products);
    byte[] ExportSalesToExcel(IEnumerable<Sale> sales);
    byte[] ExportInventoryToPdf(IEnumerable<Product> products);
    byte[] ExportSalesToPdf(IEnumerable<Sale> sales);
}

public class ExportService : IExportService
{
    public byte[] ExportInventoryToExcel(IEnumerable<Product> products)
    {
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Inventory");
        ws.Cell(1, 1).Value = "Id";
        ws.Cell(1, 2).Value = "Name";
        ws.Cell(1, 3).Value = "SKU";
        ws.Cell(1, 4).Value = "Price";
        ws.Cell(1, 5).Value = "Stock";
        var row = 2;
        foreach (var p in products)
        {
            ws.Cell(row, 1).Value = p.Id;
            ws.Cell(row, 2).Value = p.Name;
            ws.Cell(row, 3).Value = p.Sku;
            ws.Cell(row, 4).Value = p.Price;
            ws.Cell(row, 5).Value = p.StockQuantity;
            row++;
        }
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    public byte[] ExportSalesToExcel(IEnumerable<Sale> sales)
    {
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Sales");
        ws.Cell(1, 1).Value = "Id";
        ws.Cell(1, 2).Value = "Date";
        ws.Cell(1, 3).Value = "Customer";
        ws.Cell(1, 4).Value = "Total";
        var row = 2;
        foreach (var s in sales)
        {
            ws.Cell(row, 1).Value = s.Id;
            ws.Cell(row, 2).Value = s.DateUtc;
            ws.Cell(row, 3).Value = s.Customer?.Name;
            ws.Cell(row, 4).Value = s.TotalAmount;
            row++;
        }
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    public byte[] ExportInventoryToPdf(IEnumerable<Product> products)
    {
        var bytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Header().Text("Inventory").FontSize(18).Bold();
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });
                    table.Header(header =>
                    {
                        header.Cell().Text("Id").SemiBold();
                        header.Cell().Text("Name").SemiBold();
                        header.Cell().Text("SKU").SemiBold();
                        header.Cell().Text("Price").SemiBold();
                        header.Cell().Text("Stock").SemiBold();
                    });
                    foreach (var p in products)
                    {
                        table.Cell().Text(p.Id.ToString());
                        table.Cell().Text(p.Name);
                        table.Cell().Text(p.Sku);
                        table.Cell().Text(p.Price.ToString("0.00"));
                        table.Cell().Text(p.StockQuantity.ToString());
                    }
                });
            });
        }).GeneratePdf();
        return bytes;
    }

    public byte[] ExportSalesToPdf(IEnumerable<Sale> sales)
    {
        var bytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Header().Text("Sales").FontSize(18).Bold();
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });
                    table.Header(header =>
                    {
                        header.Cell().Text("Id").SemiBold();
                        header.Cell().Text("Date").SemiBold();
                        header.Cell().Text("Customer").SemiBold();
                        header.Cell().Text("Total").SemiBold();
                    });
                    foreach (var s in sales)
                    {
                        table.Cell().Text(s.Id.ToString());
                        table.Cell().Text(s.DateUtc.ToString("u"));
                        table.Cell().Text(s.Customer?.Name ?? "");
                        table.Cell().Text(s.TotalAmount.ToString("0.00"));
                    }
                });
            });
        }).GeneratePdf();
        return bytes;
    }
}