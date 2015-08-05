using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Porter.Pages.Main.Customization
{
    public class BoolNegator : IValueConverter
    {
        public Object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return !((bool)value);
            return null;
        }

        public Object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return !((bool)value);
            return null;
        }
    }
}
