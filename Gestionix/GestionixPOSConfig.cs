namespace Gestionix
{
    public static class GestionixPOSConfig
    {
        #region Fields
        public static readonly decimal ErrorRange = 0.02M;
        #endregion

        #region Properties
        public static string ConfiguredPrinter { get; set; }
        public static int TicketSize { get; set; }
        public static bool IsBusinessNameVisible { get; set; }
        public static bool IsClientNameVisible { get; set; }
        public static bool IsTaxAddressVisible { get; set; }
        public static string Environment { get; set; }
        public static bool HasLogo { get; set; }
        public static string POSVerionId { get; set; }
        public static System.Collections.Specialized.StringCollection RecentClients { get; set; }
        public static string Currency { get; set; }
        public static bool EFInitialized { get; set; }
        #endregion
    }
}
