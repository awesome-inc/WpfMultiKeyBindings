using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace WpfMultiKeyBindings
{
    [TypeConverter(typeof(MultiKeyGestureConverter))]
    public class MultiKeyGesture : KeyGesture
    {
        // adapted from https://athaur.wordpress.com/2010/05/14/keygesture-with-multiple-keys/
        private readonly TimeSpan _maxDelayBetweenKeys;
        private int _keyIndex;
        private readonly Stopwatch _stopWatch = Stopwatch.StartNew();
        public readonly Key[] Keys;

        public MultiKeyGesture(ICollection<Key> keyCollection, ModifierKeys modifiers, 
            TimeSpan? maxDelayBetweenKeys = null) 
            : base(Key.F1, modifiers)
        {
            if (keyCollection == null) throw new ArgumentNullException(nameof(keyCollection));
            if (keyCollection.Count < 2) throw new ArgumentException(@"Should specify more than one key for MultiKeyGesture",nameof(keyCollection));
            Keys = keyCollection.ToArray();
            _maxDelayBetweenKeys = maxDelayBetweenKeys ?? TimeSpan.FromSeconds(1);
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            var keyEventArgs = inputEventArgs as KeyEventArgs;
            if (keyEventArgs == null) return false;

            var key = keyEventArgs.Key;

            if (_keyIndex == 0) 
                _stopWatch.Restart();

            if (_keyIndex == 0 && keyEventArgs.KeyboardDevice.Modifiers != Modifiers)
                return false;

            if (_stopWatch.Elapsed > _maxDelayBetweenKeys)
            {
                _keyIndex = 0;
                return false;
            }

            if (_keyIndex >= Keys.Length || Keys[_keyIndex] != key)
            {
                _keyIndex = 0;
                return false;
            }

            if (_keyIndex != Keys.Length - 1)
            {
                _keyIndex++;
                _stopWatch.Restart();
                return false;
            }

            _keyIndex = 0;
            return true;
        }

        // adapted from .NETs KeyGestureConverter, 
        // cf.: http://reflector.webtropy.com/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/wpf/src/Core/CSharp/System/Windows/Input/Command/KeyGestureConverter@cs/1305600/KeyGestureConverter@cs

        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(CultureInfo culture)
        {
            var safeCulture = culture ?? CultureInfo.InvariantCulture;
            var modifiersToken = ModifierKeysConverter.ConvertTo(null, safeCulture, Modifiers, typeof(string));

            var keys = Keys.Select(key => (string)KeyConverter.ConvertTo(null, safeCulture, key, typeof(string)));
            var keysToken = string.Join(KeysSeparator.ToString(safeCulture), keys);

            return string.Concat(modifiersToken, ModifiersDelimiter, keysToken);
        }


        public static MultiKeyGesture Parse(string value, CultureInfo culture = null)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var str = value.Trim();

            // break apart keys and modifiers
            var index = str.LastIndexOf(ModifiersDelimiter);
            var modifiersToken = str.Substring(0, index);
            var keysToken = str.Substring(index + 1);

            var safeCulture = culture ?? CultureInfo.InvariantCulture;
            var modifiers = (ModifierKeys)ModifierKeysConverter.ConvertFrom(null, safeCulture, modifiersToken);

            var keys = keysToken.Split(KeysSeparator).Select(keyToken =>
                (Key)KeyConverter.ConvertFrom(null, safeCulture, keyToken)).ToArray();

            return new MultiKeyGesture(keys, modifiers);
        }

        private const char ModifiersDelimiter = '+';
        private const char KeysSeparator = ',';

        private static readonly KeyConverter KeyConverter = new KeyConverter();
        private static readonly ModifierKeysConverter ModifierKeysConverter = new ModifierKeysConverter();
    }
}