using System;
using System.Diagnostics;
using System.Globalization;

namespace Gestionix
{
    public static class Extensions
    {
        public static string RawDecimal(this string obj)
        {
            string ClearedText = obj.Replace("$", String.Empty).Replace(",", String.Empty).Trim().Replace(" ", String.Empty);

            if (!Expressions.IsDecimal.IsMatch(ClearedText))
                ClearedText = String.Empty;

            return ClearedText;
        }

        /// <summary>
        /// Convert T object to currency number.
        /// Add the $ and add Comma each 3 digits.
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatCurrency<T>(this T number)
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
        public static string FormatCurrency<T>(this T number, int precision)
        {
            return string.Format(CultureInfo.CurrentCulture, String.Concat("{0:", String.Concat("C" + precision), "}"), number);
        }
    }
}
