using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using System.Data;

namespace RiskPruebaTecnica.Controllers
{
    public class ReportController : Controller
    {
        private readonly IVentaService _ventaService;

        public ReportController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        public async Task<IActionResult> GenerateReport(string format, string timeRange)
        {
            var (startDate, endDate) = GetDateRange(timeRange);
            var ventas = await _ventaService.GetVentasInRange(startDate, endDate);

            if (format.ToLower() == "excel")
            {
                return GenerateExcelReport(ventas, timeRange);
            }
            else
            {
                return GeneratePdfReport(ventas, timeRange);
            }
        }

        private (DateTime startDate, DateTime endDate) GetDateRange(string timeRange)
        {
            var now = DateTime.Now;
            var startDate = now;
            var endDate = now;

            switch (timeRange.ToLower())
            {
                case "daily":
                    startDate = now.Date;
                    endDate = now.Date.AddDays(1).AddTicks(-1);
                    break;
                case "weekly":
                    startDate = now.AddDays(-(int)now.DayOfWeek).Date;
                    endDate = startDate.AddDays(7).AddTicks(-1);
                    break;
                case "monthly":
                    startDate = new DateTime(now.Year, now.Month, 1);
                    endDate = startDate.AddMonths(1).AddTicks(-1);
                    break;
                case "yearly":
                    startDate = new DateTime(now.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddTicks(-1);
                    break;
            }

            return (startDate, endDate);
        }

        private IActionResult GenerateExcelReport(IEnumerable<VentaDto> ventas, string timeRange)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte de Ventas");

            // Headers
            worksheet.Cell(1, 1).Value = "Fecha";
            worksheet.Cell(1, 2).Value = "Cliente";
            worksheet.Cell(1, 3).Value = "Total";

            // Data
            var row = 2;
            foreach (var venta in ventas)
            {
                worksheet.Cell(row, 1).Value = venta.FechaVenta.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(row, 2).Value = venta.ClienteNombre;
                worksheet.Cell(row, 3).Value = venta.Total;
                row++;
            }

            // Format
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Reporte_Ventas_{timeRange}_{DateTime.Now:yyyyMMdd}.xlsx"
            );
        }

        private IActionResult GeneratePdfReport(IEnumerable<VentaDto> ventas, string timeRange)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            var title = new Paragraph($"Reporte de Ventas - {timeRange}", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);
            document.Add(new Paragraph("\n"));

            // Table
            var table = new PdfPTable(3);
            table.WidthPercentage = 100;

            // Headers
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            table.AddCell(new PdfPCell(new Phrase("Fecha", headerFont)));
            table.AddCell(new PdfPCell(new Phrase("Cliente", headerFont)));
            table.AddCell(new PdfPCell(new Phrase("Total", headerFont)));

            // Data
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var venta in ventas)
            {
                table.AddCell(new PdfPCell(new Phrase(venta.FechaVenta.ToString("dd/MM/yyyy HH:mm"), normalFont)));
                table.AddCell(new PdfPCell(new Phrase(venta.ClienteNombre, normalFont)));
                table.AddCell(new PdfPCell(new Phrase($"${venta.Total:N0}", normalFont)));
            }

            document.Add(table);
            document.Close();

            return File(
                memoryStream.ToArray(),
                "application/pdf",
                $"Reporte_Ventas_{timeRange}_{DateTime.Now:yyyyMMdd}.pdf"
            );
        }
    }
}
