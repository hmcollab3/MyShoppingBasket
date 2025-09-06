using PdfSharp.Fonts;

namespace Kantar.ShoppingBasket.Application.Providers
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            if (faceName == "Arial#")
            {
                var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Fonts", "ARIAL.TTF");

                return File.ReadAllBytes(fontPath);
            }
            throw new InvalidOperationException($"Font face '{faceName}' not found.");
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
                return new FontResolverInfo("Arial#");

            return null;
        }
    }
}
