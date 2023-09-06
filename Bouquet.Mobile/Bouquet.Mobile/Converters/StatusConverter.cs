using Bouquet.Mobile.Resources.Resx;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Bouquet.Mobile.Converters
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "";
            }

            var status = (int)value;

            switch (value) { 
                case 0:
                    return AppResources.strUnknown;
                case 1:
                    return AppResources.strNew;
                case 2:
                    return AppResources.strNew;
                case 3:
                    return AppResources.strAccepted;
                case 4:
                    return AppResources.strCompleted;
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