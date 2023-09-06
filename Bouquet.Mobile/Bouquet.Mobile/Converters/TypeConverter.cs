using Bouquet.Mobile.Resources.Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bouquet.Mobile.Converters
{
    public class TypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            var status = (int)value;

            switch (value)
            {
                case 0:
                    return AppResources.strYes;
                case 1:
                    return AppResources.strNo;
                default:
                    return AppResources.strUnknown;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}