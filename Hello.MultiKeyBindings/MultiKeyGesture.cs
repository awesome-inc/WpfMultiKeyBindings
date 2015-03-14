using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Hello.MultiKeyBindings
{
    public class MultiKeyGesture : KeyGesture
    {
        private readonly ModifierKeys _modifiers;
        private readonly TimeSpan _maxDelayBetweenKeys;
        int _keyIndex;
        private readonly Stopwatch _stopWatch = Stopwatch.StartNew();
        private readonly IList<Key> _keyCollection;

        public MultiKeyGesture(ICollection<Key> keyCollection, ModifierKeys modifiers, 
            TimeSpan? maxDelayBetweenKeys = null) 
            : base(Key.F1)
        {
            if (keyCollection == null) throw new ArgumentNullException("keyCollection");
            if (keyCollection.Count < 2) throw new ArgumentException(@"Should specify more than one key for MultiKeyGesture","keyCollection");
            _keyCollection = keyCollection.ToList();
            _modifiers = modifiers;
            _maxDelayBetweenKeys = maxDelayBetweenKeys ?? TimeSpan.FromSeconds(1);
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            var keyEventArgs = inputEventArgs as KeyEventArgs;
            if (keyEventArgs == null) return false;

            var key = keyEventArgs.Key;

            if (_keyIndex == 0) 
                _stopWatch.Restart();

            if (_keyIndex == 0 && keyEventArgs.KeyboardDevice.Modifiers != _modifiers)
                return false;

            if (_stopWatch.Elapsed > _maxDelayBetweenKeys)
            {
                _keyIndex = 0;
                return false;
            }

            if (_keyIndex >= _keyCollection.Count || _keyCollection[_keyIndex] != key)
            {
                _keyIndex = 0;
                return false;
            }

            if (_keyIndex != _keyCollection.Count - 1)
            {
                _keyIndex++;
                _stopWatch.Restart();
                return false;
            }

            _keyIndex = 0;
            return true;
        }
    }
}