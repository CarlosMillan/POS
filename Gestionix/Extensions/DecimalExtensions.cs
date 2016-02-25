using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Gestionix
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Convert Tdecimal object to currency number string.
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatCurrency(this decimal number)
        {
            return number.FormatCurrency(2);
        }

        /// <summary>
        /// Convert T object to currency number.
        /// Add the $ and add Comma each 3 digits.        
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="number">precision</param>
        [DebuggerStepThrough]
        public static string FormatCurrency(this decimal number, int scale)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:C", scale, "}"), number);
        }

        /// <summary>
        /// Convert T object to formatted number.
        /// <para>Remove .00 and add Comma each 3 digits.</para>
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatNumber(this decimal number)
        {
            return number.FormatNumber(2);
        }

        [DebuggerStepThrough]
        public static string FormatNumber(this decimal number, int scale)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:n", scale, "}"), number).Replace(".00", "");
        }

        /// <summary>
        /// Convert T object to formatted number.
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatDecimal(this decimal number)
        {
            return number.FormatDecimal(2);
        }

        [DebuggerStepThrough]
        public static string FormatDecimal(this decimal number, int scale)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:n", scale, "}"), number);
        }

        /// <summary>
        /// Convert T object to currency number.
        /// Add the $ and add Comma each 3 digits.
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatPercentage(this decimal number)
        {
            return number.FormatPercentage(2);
        }

        /// <summary>
        /// Convert T object to currency number.
        /// Add the $ and add Comma each 3 digits.        
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="number">scale</param>
        [DebuggerStepThrough]
        public static string FormatPercentage(this decimal number, int scale)
        {
            return string.Format(GestionixPOSCulture.GestionixCurrentNumberFormat, String.Concat("{0:P", scale, "}"), number / 100);
        }

        /// <summary>
        /// Convert a decimal value to normal string but concidering culture
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string DecimalToString(this decimal number)
        {
            return number.ToString(GestionixPOSCulture.GestionixCurrentNumberFormat);
        }
    }
}
