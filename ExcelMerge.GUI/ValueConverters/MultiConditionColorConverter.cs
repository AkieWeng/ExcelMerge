using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static ExcelMerge.GUI.ViewModels.FolderViewModel;

namespace ExcelMerge.GUI.ValueConverters
{
    public class MultiConditionColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 确保传入了足够的值
            if (values.Length < 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return Brushes.Black; // 默认颜色
            }

            bool isMatched = (bool)values[0];
            bool isSame = (bool)values[1];

            return !isMatched ? Brushes.Blue : !isSame ? Brushes.Red : Brushes.Black;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

