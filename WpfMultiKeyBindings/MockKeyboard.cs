using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NSubstitute;

namespace WpfMultiKeyBindings
{
    public class MockKeyboard : KeyboardDevice
    {
        private static readonly PresentationSource Source = Substitute.For<PresentationSource>();
        private readonly Random _random = new Random();

        public readonly Dictionary<Key, KeyStates> Keys = new Dictionary<Key, KeyStates>();

        public MockKeyboard() : base(InputManager.Current)
        {
        }

        public void SetModifiers(ModifierKeys modifiers)
        {
            Keys[Key.LeftAlt] = Keys[Key.RightAlt] = 
                modifiers.HasFlag(ModifierKeys.Alt) ? KeyStates.Down : KeyStates.None;

            Keys[Key.LeftCtrl] = Keys[Key.RightCtrl] =
                modifiers.HasFlag(ModifierKeys.Control) ? KeyStates.Down : KeyStates.None;

            Keys[Key.LeftShift] = Keys[Key.RightShift] =
                modifiers.HasFlag(ModifierKeys.Shift) ? KeyStates.Down : KeyStates.None;
        }

        public KeyEventArgs ArgsFor(Key key, RoutedEvent keyEvent = null)
        {
            return new KeyEventArgs(this, Source, 0, key) { RoutedEvent = keyEvent ?? Keyboard.KeyDownEvent };
        }

        public KeyEventArgs RandomKeyArgs()
        {
            return ArgsFor((Key)_random.Next((int)Key.A, (int)Key.Z));
        }

        protected override KeyStates GetKeyStatesFromSystem(Key key)
        {
            KeyStates keyState;
            Keys.TryGetValue(key, out keyState);
            return keyState;
        }
    }
}