using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Diagnostics;

namespace Gestionix
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static string RemoveAccents(this string input)
        {
            string PreResult = new string(input
                                          .Normalize(NormalizationForm.FormD)
                                          .ToCharArray()
                                          .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                                          .ToArray());

            MatchCollection Matches = Regex.Matches(input, "[ñÑ]");
            
            foreach (Match M in Matches)
            {
                PreResult = PreResult.Remove(M.Index, 1);
                PreResult = PreResult.Insert(M.Index, M.Value);
            }

            return PreResult;
        }

        [DebuggerStepThrough]
        public static decimal DecimalFormatToDecimal(this string input)
        {
            decimal DecimalRepresentacion;
            Decimal.TryParse(input, NumberStyles.Number, GestionixPOSCulture.GestionixCurrentNumberFormat, out DecimalRepresentacion);
            return DecimalRepresentacion;
        }

        [DebuggerStepThrough]
        public static decimal CurrencyFormatToDecimal(this string input)
        {
            decimal DecimalRepresentacion;
            Decimal.TryParse(input, NumberStyles.Currency, GestionixPOSCulture.GestionixCurrentNumberFormat, out DecimalRepresentacion);
            return DecimalRepresentacion;
        }

        [DebuggerStepThrough]
        public static decimal PercentageFormatToDecimal(this string input)
        {
            decimal DecimalRepresentacion;
            string StringWithoutPercentgeSymbol = String.Empty;
            int PercentageSymbolIndex = input.IndexOf(GestionixPOSCulture.GestionixCurrentNumberFormat.PercentSymbol);            

            if(PercentageSymbolIndex > -1)
                StringWithoutPercentgeSymbol = input.Remove(PercentageSymbolIndex, 1);

            Decimal.TryParse(StringWithoutPercentgeSymbol, NumberStyles.Number, GestionixPOSCulture.GestionixCurrentNumberFormat, out DecimalRepresentacion);
            return DecimalRepresentacion;
        }
    }
}
