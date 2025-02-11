using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ExcelMerge.GUI.ValueConverters
{
    public class FilePathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filePath = value as string;
            if (!string.IsNullOrEmpty(filePath))
            {
                return Path.GetFileName(filePath);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
