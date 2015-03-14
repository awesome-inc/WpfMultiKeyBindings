using System;
using System.Threading;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace Hello.MultiKeyBindings
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    class MultiKeyGesture_Should
    {
        [Test, RequiresSTA]
        public void Support_multi_keys()
        {
            var delay = TimeSpan.FromMilliseconds(10);
            var sut = new MultiKeyGesture(new[] {Key.K, Key.C}, ModifierKeys.Control, delay);

            var keyboard = new MockKeyboard();
            keyboard.SetModifiers(ModifierKeys.Control);

            var keyK = keyboard.ArgsFor(Key.K);
            var keyC = keyboard.ArgsFor(Key.C);
            var keyOther = keyboard.ArgsFor(Key.D);
            sut.Matches(null, keyK).Should().BeFalse("should not match on first key");
            sut.Matches(null, keyC).Should().BeTrue("matched on last key within max delay");

            sut.Matches(null, keyK).Should().BeFalse("no match on first key");
            Thread.Sleep(delay);
            sut.Matches(null, keyC).Should().BeFalse("max delay exceeded");

            sut.Matches(null, keyK).Should().BeFalse("should not match on first key");
            sut.Matches(null, keyOther).Should().BeFalse("no match on second within max delay");
            sut.Matches(null, keyC).Should().BeFalse("not the specified key sequence");
        }
    }
}