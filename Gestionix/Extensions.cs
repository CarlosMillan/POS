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
        public static string FormatCurrency<T>(this T number, int scale)
        {
            return string.Format(CultureInfo.CurrentCulture, String.Concat("{0:C", scale, "}"), number);
        }

        /// <summary>
        /// Convert T object to formatted number.
        /// <para>Remove .00 and add Comma each 3 digits.</para>
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatNumber<T>(this T number)
        {
            return number.FormatNumber(2);
        }

        [DebuggerStepThrough]
        public static string FormatNumber<T>(this T number, int scale)
        {
            return string.Format(CultureInfo.InvariantCulture, String.Concat("{0:n", scale ,"}"), number).Replace(".00", "");
        }

        /// <summary>
        /// Convert T object to formatted number.
        /// </summary>
        /// <param name="number">number</param>
        [DebuggerStepThrough]
        public static string FormatDecimal<T>(this T number)
        {
            return number.FormatDecimal(2);
        }

        [DebuggerStepThrough]
        public static string FormatDecimal<T>(this T number, int scale)
        {
            return string.Format(CultureInfo.InvariantCulture, String.Concat("{0:n", scale, "}"), number);
        }
    }
}
