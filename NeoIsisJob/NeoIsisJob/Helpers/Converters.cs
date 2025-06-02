using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using System.Globalization;

namespace NeoIsisJob.Helpers
{
    /// <summary>
    /// Converts boolean values to their negated equivalents.
    /// </summary>
    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }

    /// <summary>
    /// Converts string values to boolean values (null or empty = false, otherwise true).
    /// </summary>
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts string values to Visibility (null or empty = Collapsed, otherwise Visible).
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts double values to formatted string representations.
    /// </summary>
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double doubleValue)
            {
                return doubleValue.ToString("F1");
            }
            return "0.0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (double.TryParse(value as string, out double result))
            {
                return result;
            }
            return 0.0;
        }
    }

    /// <summary>
    /// Converts boolean values to status icons.
    /// </summary>
    public class BoolToStatusIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "✅" : "⚠️";
            }
            return "⚠️";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts boolean values to status colors.
    /// </summary>
    public class BoolToStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Colors.Green : Colors.Orange;
            }
            return Colors.Orange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts boolean values to status text.
    /// </summary>
    public class BoolToStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "On Track" : "Needs Attention";
            }
            return "Needs Attention";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts boolean values to check/warning symbols.
    /// </summary>
    public class BoolToCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "✓" : "⚠";
            }
            return "⚠";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts DateTime values to short date strings.
    /// </summary>
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("MM/dd");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (DateTime.TryParse(value as string, out DateTime result))
            {
                return result;
            }
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Converts DateTime values to time strings.
    /// </summary>
    public class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("HH:mm");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (DateTime.TryParse(value as string, out DateTime result))
            {
                return result;
            }
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Converts count values to Visibility (0 = Collapsed, otherwise Visible).
    /// </summary>
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 