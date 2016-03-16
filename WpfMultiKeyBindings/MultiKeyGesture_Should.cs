using System;
using System.Threading;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace WpfMultiKeyBindings
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    internal class MultiKeyGesture_Should
    {
        [Test, Apartment(ApartmentState.STA)]
        public void Support_multi_keys()
        {
            var delay = TimeSpan.FromMilliseconds(10);

            var keyboard = new MockKeyboardDevice();
            var key1 = keyboard.RandomKeyArgs();
            var key2 = keyboard.RandomKeyArgs();
            var keyOther = keyboard.ArgsFor(Key.None);

            foreach (var modifier in PossibleShortcutGesture.ModifierCombos)
            {
                var sut = new MultiKeyGesture(new[] { key1.Key, key2.Key }, modifier, delay);

                // modifier hold over the whole sequence
                keyboard.SetModifiers(modifier);
                sut.Matches(null, key1).Should().BeFalse("should not match on first key");
                sut.Matches(null, key2).Should().BeTrue("matched on last key within max delay");

                // delay between keys execeeded
                keyboard.SetModifiers(modifier);
                sut.Matches(null, key1).Should().BeFalse("no match on first key");
                Thread.Sleep(delay);
                sut.Matches(null, key2).Should().BeFalse("max delay exceeded");

                // modifier only pressed on the 1st key
                keyboard.SetModifiers(modifier);
                sut.Matches(null, key1).Should().BeFalse("should not match on first key");
                keyboard.SetModifiers(ModifierKeys.None);
                sut.Matches(null, key2).Should().BeTrue("matched on last key within max delay");

                // sequence broken
                keyboard.SetModifiers(modifier);
                sut.Matches(null, key1).Should().BeFalse("should not match on first key");
                sut.Matches(null, keyOther).Should().BeFalse("no match on second within max delay");
                sut.Matches(null, key2).Should().BeFalse("not the specified key sequence");

                // sequence restarted
                keyboard.SetModifiers(modifier);
                sut.Matches(null, key1).Should().BeFalse("should not match on first key");
                sut.Matches(null, keyOther).Should().BeFalse("no match on second within max delay");
                sut.Matches(null, key1).Should().BeFalse("should not match on first key");
                sut.Matches(null, key2).Should().BeTrue("Sequence restarted");

                // sequence not started with modifier
                keyboard.SetModifiers(ModifierKeys.None);
                sut.Matches(null, key1).Should().BeFalse("should not match on first key");
                sut.Matches(null, key2).Should().BeFalse("no modifier");
            }
        }
    }
}