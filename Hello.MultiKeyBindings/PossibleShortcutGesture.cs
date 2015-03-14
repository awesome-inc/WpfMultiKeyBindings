using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Hello.MultiKeyBindings
{
    public class PossibleShortcutGesture : KeyGesture
    {
        private readonly int _maxKeys;
        private readonly TimeSpan _maxDelayBetweenKeys;
        private int _keyIndex;
        private readonly Stopwatch _stopWatch = Stopwatch.StartNew();

        public PossibleShortcutGesture(int maxKeys = 3, TimeSpan? maxDelayBetweenKeys = null)
            : base(Key.F1)
        {
            _maxKeys = maxKeys;
            _maxDelayBetweenKeys = maxDelayBetweenKeys ?? TimeSpan.FromSeconds(1);
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            var keyEventArgs = inputEventArgs as KeyEventArgs;
            if (keyEventArgs == null) return false;

            var keyboard = keyEventArgs.KeyboardDevice;

            if (_keyIndex == 0) 
                _stopWatch.Restart();

            if (IsFunctionKey(keyboard) || IsSpecialKey(keyboard))
            {
                _keyIndex = 0;
                return true;
            }

            if (_keyIndex == 0 && !IsInterestingModifierPressed(keyboard))
                return false;

            if (_stopWatch.Elapsed > _maxDelayBetweenKeys)
            {
                _keyIndex = 0;
                return false;
            }

            if (_keyIndex < _maxKeys)
            {
                _keyIndex++;
                _stopWatch.Restart();
                return true;
            }

            _keyIndex = 0;
            return false;
        }

        public static readonly Key[] ModifierKeys = {Key.LeftCtrl, Key.LeftShift, Key.LeftAlt,
           Key.RightCtrl, Key.RightShift, Key.RightAlt};
        public static readonly Key[] FunctionKeys = Enumerable.Range((byte)Key.F1, Key.F24 - Key.F1)
                .Select(b => (Key)b).ToArray();
        public static readonly Key[] SpecialKeys = 
        {
            Key.Play, Key.Pause, Key.MediaPlayPause, 
            Key.MediaNextTrack, Key.MediaPreviousTrack
        };


        private static bool IsInterestingModifierPressed(KeyboardDevice keyboard)
        {
            return ModifierKeys.Any(keyboard.IsKeyDown);
        }

        private static bool IsSpecialKey(KeyboardDevice keyboard)
        {
            return SpecialKeys.Any(keyboard.IsKeyToggled);
        }

        private static bool IsFunctionKey(KeyboardDevice keyboard)
        {
            return FunctionKeys.Any(keyboard.IsKeyToggled);
        }
    }
}