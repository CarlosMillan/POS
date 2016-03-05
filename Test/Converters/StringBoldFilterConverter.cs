using System.Windows.Data;
using Gestionix;
using System;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Test.Converters
{
    public class StringBoldFilterConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TextBlock RepresentationValue = new TextBlock();

            try
            {
                if (values != null && values.Length == 2)
                {                    
                    string Base = values[0].ToString();
                    string Filter = values[1] == null ? String.Empty : values[1].ToString();

                    int Contains = System.Globalization.CultureInfo.InvariantCulture.CompareInfo.IndexOf(Base.RemoveAccents(),
                                                                                     Filter,
                                                                                     System.Globalization.CompareOptions.OrdinalIgnoreCase);

                    if (Contains > -1)
                    {
                        Run Before = new Run();
                        Run Middle = new Run();
                        Run After = new Run();

                       if (Contains > 0) //Crear antes                        
                            Before.Text = Base.Substring(0, Contains);

                        Middle.Text = Base.Substring(Contains, Filter.Length);

                        if ((Contains + Filter.Length) < Base.Length) //crear después
                            After.Text = Base.Substring((Contains + Filter.Length), Base.Length - (Contains + Filter.Length));

                        RepresentationValue.Inlines.Add(Before);
                        RepresentationValue.Inlines.Add(new Bold(Middle));
                        RepresentationValue.Inlines.Add(After);
                        return RepresentationValue;
                    }
                }
            }
            catch { }

            return RepresentationValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
