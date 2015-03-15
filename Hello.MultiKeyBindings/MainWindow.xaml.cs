using System.Windows;
using System.Windows.Input;

namespace Hello.MultiKeyBindings
{
    // Adapted from Caliburn.Micro sample
    // https://github.com/Caliburn-Micro/Caliburn.Micro/tree/master/samples/Caliburn.Micro.KeyBinding/Caliburn.Micro.KeyBinding/Input
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void commandTest_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("You pressed " + e.Parameter);
        }
    }
}
