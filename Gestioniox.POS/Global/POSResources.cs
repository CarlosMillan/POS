using System;
using System.Windows;
using System.Windows.Media;

namespace Gestionix.POS.GUI
{
    public static class POSResources
    {
        #region Properties
        public static ResourceDictionary Strings { get; set; }
        public static ResourceDictionary Resources { get; set; }
        public static ResourceDictionary SolidColors { get; set; }
        #endregion

        /// <summary>
        /// Returns a string value for key
        /// </summary>
        /// <returns></returns>
        public static string S(string key, params string[] keyvalues)
        {
            try
            {
                return S(Strings, key, keyvalues);
            }
            catch
            {
                return String.Concat("-", key, "-");
            }
        }

        public static string S(ResourceDictionary dictionary, string key, params string[] keyvalue)
        {
            string Value = string.Empty;

            try
            {
                if (dictionary != null)
                {
                    Value = dictionary[key].ToString();

                    if (keyvalue.Length > 0)
                    {
                        if (keyvalue.Length % 2 == 0)
                        {
                            for (int index = 0; index < keyvalue.Length; index++)
                                Value = Value.Replace(String.Concat("{", keyvalue[index++], "}"), keyvalue[index]);
                        }
                        else                            
                            throw new NoPairValuesException("Las claves deben de ser pares CLAVE-VALOR");
                    }
                }
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return String.Concat("-", key, "-");
            }
            catch (NoPairValuesException ex)
            {
                throw ex;
            }

            return Value;
        }

        /// <summary>
        /// Returns a SolidColorBrush for key
        /// </summary>
        public static SolidColorBrush C(string key)
        {
            return C(SolidColors, key);
        }

        public static SolidColorBrush C(ResourceDictionary dictionary, string key)
        {
            try
            {
                return (SolidColorBrush)dictionary[key];
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return new SolidColorBrush(Colors.Black);
            }
        }
    }

    public class NoPairValuesException : Exception
    {
        public NoPairValuesException(string message)
            : base(message)
        { }
    }
}
