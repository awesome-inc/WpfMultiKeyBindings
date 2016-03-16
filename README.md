# WpfMultiKeyBindings

A WPF implementation for MultiKeyBindings like in Visual Studio.

![Screenshot of sample application](WpfSample.png)

## What is it?

When input speed is important users ask for keyboard shortcuts. As your feature list grows they will ask for more and more. 
Soon, you will run out of keys to use with the default [KeyGesture](https://msdn.microsoft.com/en-us/library/system.windows.input.keygesture%28v=vs.110%29.aspx). 
As you also don't want to collide with the default Windows control shortcuts, the way to go is to simply support longer gestures with unbound key sequences. 
Visual Studio extensions like [ReSharper](https://www.jetbrains.com/resharper/) do this and this is exactly what you get here.

## Usage

It's simple. Replace your existing `KeyBinding` with `MultiKeyBinding`. 
Then, have fun coming up with funny key combinations. 
Just have a look at the [sample application](Hello.MultiKeyBindings/MainWindow.xaml). 
The maximum delay between consecutive keys defaults to one second. Feel free to adjust.

