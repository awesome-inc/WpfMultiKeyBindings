using System;
using System.ComponentModel;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace WpfMultiKeyBindings
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    internal class MultiKeyBinding_Should
    {
        [Test]
        public void Use_gesture_converter()
        {
            typeof (MultiKeyBinding).GetProperty(nameof(MultiKeyBinding.Gesture)).Should()
                .BeDecoratedWith<TypeConverterAttribute>()
                .Which.ConverterTypeName.Should().Be(typeof(MultiKeyGestureConverter).AssemblyQualifiedName);
        }

        [Test]
        public void Protect_setter_to_multikeygestures()
        {
            var sut = new MultiKeyBinding();

            InputGesture notAMultiKeyGesture = new KeyGesture(Key.A, ModifierKeys.Control);
            sut.Invoking(x => x.Gesture = notAMultiKeyGesture).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Suport_ToString()
        {
            var gesture = new MultiKeyGesture(new[] {Key.A, Key.B}, ModifierKeys.Control);
            var sut = new MultiKeyBinding {Gesture = gesture};
            sut.ToString().Should().Be(gesture.ToString());
        }
    }
}