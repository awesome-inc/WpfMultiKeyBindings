# WpfMultiKeyBindings

A WPF implementation for MultiKeyBindings like in Visual Studio.

[![Build status](https://ci.appveyor.com/api/projects/status/x1h1u6p7pbal097j?svg=true)](https://ci.appveyor.com/project/awesome-inc-build/wpfmultikeybindings)

[![NuGet](https://badge.fury.io/nu/FontAwesome.Sharp.svg)](https://www.nuget.org/packages/WpfMultiKeyBindings/) 
[![NuGet](https://img.shields.io/nuget/dt/WpfMultiKeyBindings.svg?style=flat-square)](https://www.nuget.org/packages/WpfMultiKeyBindings/)

[![Coverage Status](https://coveralls.io/repos/github/awesome-inc/FluentAssertions.Autofac/badge.svg)](https://coveralls.io/github/awesome-inc/FluentAssertions.Autofac)
[![Coverage Status](https://coveralls.io/repos/github/awesome-inc/WpfMultiKeyBindings/badge.svg)](https://coveralls.io/github/awesome-inc/WpfMultiKeyBindings)

Here is a screenshot from the sample application

![Screenshot of sample application](WpfSample.png)

## What is it?

When input speed is important users ask for keyboard shortcuts. As your feature list grows they will ask for more and more. 
Soon, you will run out of keys to use with the default [KeyGesture](https://msdn.microsoft.com/en-us/library/system.windows.input.keygesture%28v=vs.110%29.aspx). 
As you also don't want to collide with the default Windows control shortcuts, the way to go is to simply support longer gestures with unbound key sequences. 
Visual Studio extensions like [ReSharper](https://www.jetbrains.com/resharper/) do this and this is exactly what you get here.

## Usage

It's simple. Replace your existing `KeyBinding` with `MultiKeyBinding`. 
Then, have fun coming up with funny key combinations.
 
Just have a look at the [sample application](WpfSample/MainWindow.xaml). 
The maximum delay between consecutive keys defaults to one second. Feel free to adjust.

## Testing

To support automated testing there is the `MockKeyboardDevice` which you can use to mock keyboard states and to generate `KeyEventArgs`.