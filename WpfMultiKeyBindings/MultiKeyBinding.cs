using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfMultiKeyBindings
{
    public class MultiKeyBinding : InputBinding
    {
        [TypeConverter(typeof (MultiKeyGestureConverter))]
        public override InputGesture Gesture
        {
            get { return base.Gesture; }
            set
            {
                if (!(value is MultiKeyGesture))
                    throw new ArgumentException(@"Not a MultiKeyGesture", nameof(value));
                base.Gesture = value;
            }
        }

        public override string ToString()
        {
            return Gesture?.ToString() ?? base.ToString();
        }
    }
}