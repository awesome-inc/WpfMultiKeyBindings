using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using NSubstitute;

namespace Hello.MultiKeyBindings
{
    class MockKeyboard : KeyboardDevice
    {
        private static readonly PresentationSource Source = Substitute.For<PresentationSource>();
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

        public KeyEventArgs ArgsFor(Key key)
        {
            return new KeyEventArgs(this, Source, 0, key) { RoutedEvent = Keyboard.KeyDownEvent };
        }

        protected override KeyStates GetKeyStatesFromSystem(Key key)
        {
            KeyStates keyState;
            Keys.TryGetValue(key, out keyState);
            return keyState;
        }
    }
}