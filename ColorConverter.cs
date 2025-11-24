using System.Globalization;

namespace CetTodoApp
{
    // Bu sınıf tarihe bakar: Tarih eskiyse Kırmızı, değilse Siyah renk verir.
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime taskDate)
            {
                // Tarih bugünden küçükse KIRMIZI yap
                if (taskDate < DateTime.Now.Date)
                {
                    return Colors.Red;
                }
            }
            // Değilse SİYAH olsun
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}