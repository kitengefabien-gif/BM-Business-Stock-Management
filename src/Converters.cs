using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace BMBusinessStockManagement.Converters
{
    /// <summary>
    /// Convertit un booléen en booléen inversé (Not converter)
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return false;
        }
    }

    /// <summary>
    /// Convertit un string en booléen (true si non-vide)
    /// </summary>
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convertit un énumération en string lisible
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var enumValue = value.ToString();
            return enumValue switch
            {
                // Arrival Status
                "Arrive" => "Arrivé",
                "NonArrive" => "Non arrivé",
                
                // Payment Status
                "Paye" => "Payé",
                "NonPaye" => "Non payé",
                
                // Transaction Type
                "Revenu" => "Revenu",
                "Cout" => "Coût",
                
                // Payment Method
                "Especes" => "Espèces",
                "Virement" => "Virement",
                "MobileWallet" => "Mobile Money",
                
                _ => enumValue
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convertit un montant en devise formatée
    /// </summary>
    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                var currency = parameter?.ToString() ?? "ZMW";
                var symbol = currency switch
                {
                    "ZMW" => "ZK",
                    "CDF" => "FC",
                    _ => currency
                };
                return $"{symbol} {amount:N2}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convertit un statut en couleur
    /// </summary>
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Gray;

            var status = value.ToString();
            return status switch
            {
                "Arrive" => Colors.Green,
                "NonArrive" => Colors.Red,
                "Paye" => Colors.Green,
                "NonPaye" => Colors.Orange,
                "Completed" => Colors.Green,
                "Pending" => Colors.Orange,
                "Failed" => Colors.Red,
                _ => Colors.Gray
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Convertit une date en format lisible
    /// </summary>
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                var today = DateTime.UtcNow.Date;
                var date = dateTime.Date;

                if (date == today)
                    return $"Aujourd'hui à {dateTime:HH:mm}";
                else if (date == today.AddDays(-1))
                    return $"Hier à {dateTime:HH:mm}";
                else
                    return dateTime.ToString("dd MMM yyyy HH:mm", culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
