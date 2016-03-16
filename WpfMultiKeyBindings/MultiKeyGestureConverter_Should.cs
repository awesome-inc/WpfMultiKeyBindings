using System;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace WpfMultiKeyBindings
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    internal class MultiKeyGestureConverter_Should
    {
        [Test]
        public void Convert_from_and_to_strings()
        {
            var sut = new MultiKeyGestureConverter();

            sut.CanConvertFrom(null, typeof (string)).Should().BeTrue();
            sut.CanConvertTo(null, typeof(string)).Should().BeTrue();
            sut.CanConvertFrom(null, typeof (int)).Should().BeFalse();
            sut.CanConvertTo(null, typeof(int)).Should().BeFalse();

            const string input = "Ctrl+A,B";
            var gesture = (MultiKeyGesture)sut.ConvertFrom(null, null, input);
            gesture.Keys.Should().BeEquivalentTo(new[] {Key.A, Key.B});
            gesture.Modifiers.Should().Be(ModifierKeys.Control);

            var str = (string) sut.ConvertTo(null, null, gesture, typeof(string));
            str.Should().Be(input);

            sut.ConvertFrom(null, null, 42).Should().BeNull();

            sut.Invoking(x => x.ConvertTo(null, null, 42, typeof(string))).ShouldThrow<ArgumentException>();
            sut.Invoking(x => x.ConvertTo(null, null, gesture, typeof (int))).ShouldThrow<ArgumentException>();
        }
    }
}