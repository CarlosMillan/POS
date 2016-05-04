using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Gestionix
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Convert T object to currency number.
        /// Add the $ and add Comma each 3 digits.        
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="number">precision</param>
        [DebuggerStepThrough]
        public static string FormatCurrency(this decimal number, int scale = 2)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:C", scale, "}"), number);
        }

        /// <summary>
        /// Convert number to formatted number.
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatNumber(this decimal number, int scale = 2)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:n", scale, "}"), number).Replace(".00", "");
        }

        [DebuggerStepThrough]
        public static string FormatNumberInt(this decimal number, int scale = 2)
        {
            return number.FormatNumber().Replace(".".PadRight(scale + 1, '0'), String.Empty);
        }

        /// <summary>
        /// Convert a decimal value to normal string but concidering culture
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string FormatDecimal(this decimal number, int scale = 2)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:n", scale, "}"), number);
        }

        [DebuggerStepThrough]
        public static string FormatPercentage(this decimal number, int scale = 2)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:P", scale, "}"), number / 100);
        }

        [DebuggerStepThrough]
        public static string FormatPercentageInt(this decimal number, int scale = 2)
        {
            return number.FormatPercentage(scale).Replace(".".PadRight(scale + 1, '0'), String.Empty);
        }
    }
}
