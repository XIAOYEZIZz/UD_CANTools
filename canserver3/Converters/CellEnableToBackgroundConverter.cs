using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace canserver3.Converters
{
    public class CellEnableToBackgroundConverter : IValueConverter
    {
        public IBrush TrueBrush { get; set; } = Brushes.Transparent;
        public IBrush FalseBrush { get; set; } = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // 浅灰色

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? TrueBrush : FalseBrush;
            }
            return TrueBrush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
