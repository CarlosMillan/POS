using System.Globalization;

namespace Gestionix
{
    public static class GestionixPOSCulture
    {
        public static CultureInfo GestionixCurrentCulture { get; private set; }
        public static NumberFormatInfo GestionixCurrentNumberFormat { get; private set; }

        #region Public Methods
        public static void SetPOSCulture(SupportedCultures culture)
        {
            GestionixCurrentCulture = new CultureInfo(culture.GetStringValue());
            NumberFormatInfo FNumber = GestionixCurrentCulture.NumberFormat;
            FNumber.CurrencyDecimalSeparator = ".";
            FNumber.NumberDecimalSeparator = ".";
            FNumber.CurrencyGroupSeparator = ",";
            FNumber.NumberGroupSeparator = ",";
            FNumber.PercentDecimalSeparator = ".";
            FNumber.PercentGroupSeparator = ",";
            GestionixCurrentNumberFormat = FNumber;
            GestionixPOSConfig.Currency = new RegionInfo(GestionixCurrentCulture.Name).ISOCurrencySymbol;
            SettingsPerCulture(GestionixCurrentCulture.Name);
        }
        #endregion

        #region Helpers
        private static void SettingsPerCulture(string name)
        {
            if (name == SupportedCultures.MEXICO.GetStringValue())
                MexicoSettings();
        }

        private static void MexicoSettings()
        {
            string[] Days = { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            string[] Months = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "" };
            GestionixCurrentCulture.DateTimeFormat.DayNames = Days;
            GestionixCurrentCulture.DateTimeFormat.MonthNames = Months;
            GestionixCurrentCulture.DateTimeFormat.MonthGenitiveNames = Months;
        }
        #endregion
    }
}
