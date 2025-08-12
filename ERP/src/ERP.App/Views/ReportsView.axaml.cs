using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ERP.App.Services;
using ERP.App.ViewModels;

namespace ERP.App.Views;

public partial class ReportsView : UserControl
{
    public ReportsView()
    {
        InitializeComponent();
        this.FindControl<Button>("ExportSalesExcel").Click += async (_, __) => await ExportSalesExcelAsync();
        this.FindControl<Button>("ExportSalesPdf").Click += async (_, __) => await ExportSalesPdfAsync();
        this.FindControl<Button>("ExportInventoryExcel").Click += async (_, __) => await ExportInventoryExcelAsync();
        this.FindControl<Button>("ExportInventoryPdf").Click += async (_, __) => await ExportInventoryPdfAsync();
    }

    private async Task ExportSalesExcelAsync()
    {
        if (DataContext is ReportsViewModel vm && VisualRoot is Window window)
        {
            var service = new ExportService();
            var bytes = service.ExportSalesToExcel(vm.Sales);
            await SaveBytesAsync(window, bytes, "sales.xlsx");
        }
    }

    private async Task ExportSalesPdfAsync()
    {
        if (DataContext is ReportsViewModel vm && VisualRoot is Window window)
        {
            var service = new ExportService();
            var bytes = service.ExportSalesToPdf(vm.Sales);
            await SaveBytesAsync(window, bytes, "sales.pdf");
        }
    }

    private async Task ExportInventoryExcelAsync()
    {
        if (DataContext is ReportsViewModel vm && VisualRoot is Window window)
        {
            var service = new ExportService();
            var bytes = service.ExportInventoryToExcel(vm.Inventory);
            await SaveBytesAsync(window, bytes, "inventory.xlsx");
        }
    }

    private async Task ExportInventoryPdfAsync()
    {
        if (DataContext is ReportsViewModel vm && VisualRoot is Window window)
        {
            var service = new ExportService();
            var bytes = service.ExportInventoryToPdf(vm.Inventory);
            await SaveBytesAsync(window, bytes, "inventory.pdf");
        }
    }

    private static async Task SaveBytesAsync(Window window, byte[] bytes, string suggestedName)
    {
        var sfd = new SaveFileDialog
        {
            InitialFileName = suggestedName
        };
        var path = await sfd.ShowAsync(window);
        if (!string.IsNullOrWhiteSpace(path))
        {
            await File.WriteAllBytesAsync(path, bytes);
        }
    }
}