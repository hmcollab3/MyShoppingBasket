using Kantar.ShoppingBasket.Application.Providers.Interfaces;
using Kantar.ShoppingBasket.Domain.Model;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace Kantar.ShoppingBasket.Application.Providers
{
    public class PDFFileProvider : IFileProvider
    {
        public byte[]? GenerateFile(DetailedReceipt receipt)
        {
            if (receipt == null || receipt.Receipt == null)
                return null;

            var document = new PdfDocument();
            document.Info.Title = $"Receipt #{receipt.Receipt.Id}";

            var page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            var gfx = XGraphics.FromPdfPage(page);

            GlobalFontSettings.FontResolver = new CustomFontResolver();

            var fontTitle = new XFont("Arial", 18, XFontStyleEx.Bold);
            var fontHeader = new XFont("Arial", 12, XFontStyleEx.Bold);
            var fontRow = new XFont("Arial", 12, XFontStyleEx.Regular);

            double yPoint = 40;

            gfx.DrawString($"Receipt #{receipt.Receipt.Id}", fontTitle, XBrushes.Black, new XRect(0, yPoint, page.Width, 30), XStringFormats.TopCenter);
            yPoint += 40;

            gfx.DrawString($"Date: {receipt.Receipt.Date:yyyy-MM-dd}", fontRow, XBrushes.Black, new XRect(40, yPoint, page.Width, 20), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Total: {receipt.Receipt.TotalCost} {receipt.Receipt.Currency}", fontRow, XBrushes.Black, new XRect(40, yPoint, page.Width, 20), XStringFormats.TopLeft);
            yPoint += 30;

            gfx.DrawString("Name", fontHeader, XBrushes.Black, new XRect(40, yPoint, 100, 20), XStringFormats.TopLeft);
            gfx.DrawString("Qty", fontHeader, XBrushes.Black, new XRect(150, yPoint, 40, 20), XStringFormats.TopLeft);
            gfx.DrawString("Price", fontHeader, XBrushes.Black, new XRect(200, yPoint, 60, 20), XStringFormats.TopLeft);
            gfx.DrawString("Discount", fontHeader, XBrushes.Black, new XRect(270, yPoint, 60, 20), XStringFormats.TopLeft);
            gfx.DrawString("Total", fontHeader, XBrushes.Black, new XRect(340, yPoint, 60, 20), XStringFormats.TopLeft);
            yPoint += 20;

            foreach (var item in receipt.ReceiptItems)
            {
                gfx.DrawString(item.Name, fontRow, XBrushes.Black, new XRect(40, yPoint, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString(item.Quantity.ToString(), fontRow, XBrushes.Black, new XRect(150, yPoint, 40, 20), XStringFormats.TopLeft);
                gfx.DrawString($"{item.Price} {item.Currency}", fontRow, XBrushes.Black, new XRect(200, yPoint, 60, 20), XStringFormats.TopLeft);
                gfx.DrawString(item.Discount.ToString("F2"), fontRow, XBrushes.Black, new XRect(270, yPoint, 60, 20), XStringFormats.TopLeft);
                gfx.DrawString(item.ItemTotalCost.ToString("F2"), fontRow, XBrushes.Black, new XRect(340, yPoint, 60, 20), XStringFormats.TopLeft);
                yPoint += 20;
            }

            yPoint += 30;
            gfx.DrawString("Thank you for your purchase!", fontHeader, XBrushes.Black, new XRect(0, yPoint, page.Width, 20), XStringFormats.TopCenter);

            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}