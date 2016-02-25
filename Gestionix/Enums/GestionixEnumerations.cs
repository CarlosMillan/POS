namespace Gestionix
{
    /// <summary>
    /// Allows to put attribute to value enum.
    /// </summary>
    public class StringValueAttribute : System.Attribute
    {

        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
        get { return _value; }
        }
    }

    public enum SupportedCultures
    {
        [StringValue("es-MX")]
        MEXICO
    }
}
