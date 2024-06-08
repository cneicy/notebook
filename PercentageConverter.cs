using System.Globalization;
using System.Windows.Data;

namespace notebook
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var percentage = System.Convert.ToDouble(parameter);
            var num = System.Convert.ToDouble(value);
            return num * percentage;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}