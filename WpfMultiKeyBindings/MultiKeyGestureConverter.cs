using System;
using System.ComponentModel;
using System.Globalization;

namespace WpfMultiKeyBindings
{
    public class MultiKeyGestureConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof (string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = (string)value;
            return MultiKeyGesture.Parse(str);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null ||destinationType != typeof(string))
                throw new ArgumentException(@"Can only convert to string", nameof(destinationType));
            
            var multiKeyGesture = value as MultiKeyGesture;
            if (multiKeyGesture == null)
                throw new ArgumentException(@"Can only convert from MultiKeyGesture", nameof(value));
            return multiKeyGesture.ToString();
        }
    }
}