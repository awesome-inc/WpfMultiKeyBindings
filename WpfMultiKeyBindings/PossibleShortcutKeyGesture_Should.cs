using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace WpfMultiKeyBindings
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    internal class PossibleShortcutKeyGesture_Should
    {
        [Test, Apartment(ApartmentState.STA)]
        public void Match_any_interesting_combo()
        {
            var delay = TimeSpan.FromMilliseconds(10);
            const int maxNumKeys = 3;
            var sut = new PossibleShortcutGesture(maxNumKeys, delay);

            var keyboard = new MockKeyboardDevice();

            foreach (var modifier in PossibleShortcutGesture.ModifierCombos)
            {
                // modifier hold over the whole sequence
                keyboard.SetModifiers(modifier);
                for (var i = 0; i<maxNumKeys; i++)
                    sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeTrue();
                sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeFalse("max keys exceeded");

                // delay between keys execeeded
                keyboard.SetModifiers(modifier);
                sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeTrue();
                Thread.Sleep(delay);
                sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeFalse("delay exceeded");

                // modifier only pressed on the 1st key
                keyboard.SetModifiers(modifier);
                sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeTrue();
                keyboard.SetModifiers(ModifierKeys.None);
                for (var i = 1; i < maxNumKeys; i++)
                    sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeTrue();
                sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeFalse("max keys exceeded");

                // no modifier at beginning
                keyboard.SetModifiers(ModifierKeys.None);
                for (var i = 1; i <= maxNumKeys; i++)
                    sut.Matches(null, keyboard.RandomKeyArgs()).Should().BeFalse();
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Match_function_keys()
        {
            MatchKeys(PossibleShortcutGesture.FunctionKeys);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Match_special_keys()
        {
            MatchKeys(PossibleShortcutGesture.SpecialKeys);
        }

        private static void MatchKeys(IEnumerable<Key> keys)
        {
            var sut = new PossibleShortcutGesture();
            var keyboard = new MockKeyboardDevice();

            var k = keyboard.ArgsFor(Key.None);

            foreach (var key in keys)
            {
                keyboard.Keys[key] = KeyStates.Down;
                sut.Matches(null, k).Should().BeTrue();
                keyboard.Keys[key] = KeyStates.Toggled;
                sut.Matches(null, k).Should().BeTrue();
                keyboard.Keys[key] = KeyStates.None;

                sut.Matches(null, k).Should().BeFalse();
            }
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Support_Single_Keys_on_PreviewKeyDown()
        {
            var keyboard = new MockKeyboardDevice();

            var sut = new PossibleShortcutGesture();

            var e = keyboard.ArgsFor(Key.F11, Keyboard.PreviewKeyDownEvent);
            keyboard.Keys[Key.F11] = KeyStates.Down;

            sut.Matches(null, e).Should().BeTrue();
        }
    }
}