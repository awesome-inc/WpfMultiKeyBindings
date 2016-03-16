using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfSample
{
    // Adapted from Caliburn.Micro sample
    // https://github.com/Caliburn-Micro/Caliburn.Micro/tree/master/samples/Caliburn.Micro.KeyBinding/Caliburn.Micro.KeyBinding/Input
    public partial class MainWindow
    {
        private readonly DispatcherTimer _timer;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly TimeSpan _clearDelay = TimeSpan.FromSeconds(2);

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Tick += (s, e) => ClearStatus();
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Start();
        }

        private void ClearStatus()
        {
            if (_stopwatch.Elapsed < _clearDelay) return;
            KeyBindingStatus.Text = string.Empty;
            PreviewKeyDownStatus.Text = string.Empty;
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer.Stop();
            _stopwatch.Stop();
            base.OnClosed(e);
        }


        private void commandTest_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            KeyBindingStatus.Text = "You pressed " + e.Parameter;
            _stopwatch.Restart();
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            PreviewKeyDownStatus.Text = "You pressed " + e.Key;
        }
    }
}
