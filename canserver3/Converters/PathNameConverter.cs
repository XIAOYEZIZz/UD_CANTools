using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace canserver3.Converters
{
    public class PathNameConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count >= 2 && values[0] is string path && values[1] is string name)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return "* - " + name;
                }
                else
                {
                    return path;
                }
            }
            return string.Empty;
        }
    }
}
