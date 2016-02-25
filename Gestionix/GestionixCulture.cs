using System.Globalization;

namespace Gestionix
{
    public static class GestionixPOSCulture
    {
        public static CultureInfo GestionixCurrentCulture { get; private set; }
        public static NumberFormatInfo GestionixCurrentNumberFormat { get; private set; }

        public static void SetPOSCulture(SupportedCultures culture)
        {
            GestionixCurrentCulture = new CultureInfo(culture.GetStringValue());

            if(culture == SupportedCultures.MEXICO)
                SetMexicoCulture();
        }

        private static void SetMexicoCulture()
        {            
            NumberFormatInfo FNumber = GestionixCurrentCulture.NumberFormat;
            FNumber.CurrencyDecimalSeparator = ".";
            FNumber.NumberDecimalSeparator = ".";
            FNumber.CurrencyGroupSeparator = ",";
            FNumber.NumberGroupSeparator = ",";
            FNumber.PercentDecimalSeparator = ".";
            FNumber.PercentGroupSeparator = ",";
            FNumber.CurrencySymbol = "$";
            FNumber.PercentSymbol = "%";
            FNumber.PositiveSign = "+";            
            GestionixCurrentNumberFormat = GestionixCurrentCulture.NumberFormat;
        }
    }
}
