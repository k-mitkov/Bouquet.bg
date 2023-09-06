using System;
using System.Globalization;
using Bouquet.Mobile.Resources.Resx;
using Xamarin.Forms;

namespace Bouquet.Mobile.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return "";
            }

            var price = double.Parse(value.ToString());

            return Math.Round(price, 2) + AppResources.strLv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
