using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace ExcelMerge.GUI.ValueConverters
{
    public class FileExistenceMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var fileName = values[0] as string;
            var otherList = values[1] as IEnumerable;

            if (fileName != null && otherList != null)
            {
                var fileNameOnly = Path.GetFileName(fileName);
                foreach (var item in otherList)
                {
                    if (Path.GetFileName(item.ToString()) == fileNameOnly)
                    {
                        return Brushes.Black; // 文件存在，使用默认颜色
                    }
                }
                return Brushes.Blue; // 文件不存在，使用蓝色
            }

            return Brushes.Black; // 默认颜色
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
