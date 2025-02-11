using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ExcelMerge.GUI.ValueConverters
{
    public class FileMatchToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isMatched = (bool)value;
            return isMatched ? Brushes.Black : Brushes.Blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

