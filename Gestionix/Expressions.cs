using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gestionix
{
    public static class Expressions
    {
        public static readonly Regex IsDate = new Regex(@"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", RegexOptions.Compiled);
        public static readonly Regex IsDateTime = new Regex(@".", RegexOptions.Compiled);
        public static readonly Regex IsLegalInput = new Regex(@"^[a-zA-Z0-9'´áéíóúÁÉÍÓÚ.@!¡¿?\[\]|ñÑ$:\-_,= \{\}\(\)\s\n]{1,250}$", RegexOptions.Compiled);
        public static readonly Regex IsLegalTextArea = new Regex(@"^[a-zA-Z0-9'´áéíóúÁÉÍÓÚ.@!¡¿?\[\]|ñÑ$:\-_,= \{\}\(\)\s\n\t&;/]{1,4000}$", RegexOptions.Compiled);
        public static readonly Regex IsGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
        public static readonly Regex IsEmail = new Regex(@"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?", RegexOptions.Compiled);
        public static readonly Regex IsPostalCode = new Regex(@"^[0-9]{5}$", RegexOptions.Compiled);
        public static readonly Regex IsPhone = new Regex(@"^[\d- \(\)]{7,20}$");
        public static readonly Regex IsNumber = new Regex(@"\d");
        public static readonly Regex IsDecimal = new Regex(@"^[-]{0,1}(\d*.)?\d+$", RegexOptions.Compiled);
        public static readonly Regex IsCurrency = new Regex(@"^\${0,1}\d*(\.{0,1}\d{0,2})$", RegexOptions.Compiled);
    }
}
