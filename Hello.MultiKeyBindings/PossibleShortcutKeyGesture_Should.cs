using System;
using System.Linq;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace Hello.MultiKeyBindings
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    class PossibleShortcutKeyGesture_Should
    {
        [Test, RequiresSTA]
        public void Match_any_interesting_combo()
        {
            var delay = TimeSpan.FromMilliseconds(10);
            const int keys = 3;
            var sut = new PossibleShortcutGesture(keys, delay);

            var keyboard = new MockKeyboard();
            keyboard.SetModifiers(ModifierKeys.Control);

            var keyK = keyboard.ArgsFor(Key.K);
            sut.Matches(null, keyK).Should().BeTrue();
            sut.Matches(null, keyK).Should().BeTrue();
            sut.Matches(null, keyK).Should().BeTrue();
            sut.Matches(null, keyK).Should().BeFalse("max keys exceeded");
        }

        [Test, RequiresSTA]
        public void Match_function_keys()
        {
            var sut = new PossibleShortcutGesture();
            var keyboard = new MockKeyboard();

            var k = keyboard.ArgsFor(Key.None);

            foreach (var key in PossibleShortcutGesture.FunctionKeys)
            {
                keyboard.Keys[key] = KeyStates.Toggled;
                sut.Matches(null, k).Should().BeTrue();
                keyboard.Keys[key] = KeyStates.None;
            }
        }

        [Test, RequiresSTA]
        public void Match_special_keys()
        {
            var sut = new PossibleShortcutGesture();
            var keyboard = new MockKeyboard();

            var k = keyboard.ArgsFor(Key.None);

            foreach (var key in PossibleShortcutGesture.SpecialKeys)
            {
                keyboard.Keys[key] = KeyStates.Toggled;
                sut.Matches(null, k).Should().BeTrue();
                keyboard.Keys[key] = KeyStates.None;
            }
        }
    }
}