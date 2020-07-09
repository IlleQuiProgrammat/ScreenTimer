using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ScreenTimer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int timeElapsed = 0;
        int dueTime = 60;
        int currHours = 0;
        int currMinutes = 1;
        Windows.UI.Xaml.DispatcherTimer timer;

        public MainPage()
        {
            timer = new Windows.UI.Xaml.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += timerCallback;
            this.InitializeComponent();
            updateUI();
        }
       
        private void timerCallback(object sender, object e)
        {
            timeElapsed += timer.Interval.Seconds;

            if (timeElapsed >= dueTime)
            {
                timer.Stop();
            }
            updateUI();
        }

        private void updateUI()
        {
            ProgressTimer.Value = Math.Min(100, 100.0f * (System.Math.Min(timeElapsed, dueTime) / (float)dueTime));
            int timeRemaining = dueTime - timeElapsed;
            if (timeRemaining <= 60)
            {
                TimerResultText.Text = $"{timeRemaining}s";
            } else
            {
                int hours = (timeRemaining) / 3600;
                int minutes = (int)Math.Ceiling((timeRemaining - hours * 3600) / 60d);
                if (minutes == 60)
                {
                    TimerResultText.Text = $"{hours+1}h 0m";
                } else
                {
                    TimerResultText.Text = $"{hours}h {minutes}m";
                }
            }
        }

        void On_PauseContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                PauseContinueButtonIcon.Symbol = Symbol.Pause;
                timer.Start();
                updateUI();
            }
            else
            {
                PauseContinueButtonIcon.Symbol = Symbol.Play;
                timer.Stop();
            }
        }

        private void On_RestartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timeElapsed = 0;
            updateUI();
        }

        private  async void On_ConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            HoursInput.Value = currHours;
            MinutesInput.Value = currMinutes;
        }

        private void On_FlyoutInputOK_Click(object sender, RoutedEventArgs e)
        {
            currHours = (int)HoursInput.Value;
            currMinutes = (int)MinutesInput.Value;
            dueTime = currHours * 3600 + currMinutes * 60;
            updateUI();
        }

        private void On_FlyoutInputCancel_Click(object sender, RoutedEventArgs e)
        {
            HoursInput.Value = currHours;
            MinutesInput.Value = currMinutes;
        }
    }
}
