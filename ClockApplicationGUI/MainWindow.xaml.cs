using ClockApplicationBO.Models;
using ClockApplicationRepo;
using ClockApplicationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClockApplicationGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CountdownService countdownService;
        private AlarmService alarmService;
        private AlarmRepo alarmRepo;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCountdownService();
            InitializeAlarmService();
            alarmRepo = new AlarmRepo();
            var alrmList = alarmRepo.GetAll();
            alarmDataGrid.ItemsSource = alrmList;
            dpAlarmDate.SelectedDate = DateTime.Today;
            alrmList.ForEach(alarm =>
            {
                if (alarm.Enabled)
                {
                    alarmService.SetAlarm(alarm.AlarmTime);
                }
            });            
        }

        private void InitializeAlarmService()
        {
            alarmService = new AlarmService();
            alarmService.AlarmTriggered += AlarmService_AlarmTriggered;
        }

        private void InitializeCountdownService()
        {
            countdownService = new CountdownService(0, 0, 0);
            countdownService.TimeChanged += CountdownService_TimeChanged;
            countdownService.CountdownFinished += CountdownService_CountdownFinished;
        }

        private void btnSetAlarm_Click(object sender, RoutedEventArgs e)
        {
            int hours = int.Parse(txtAlarmHours.Text);
            int minutes = int.Parse(txtAlarmMinutes.Text);
            var alarmName = txtAlarmName.Text;
            DateTime alarmTime = dpAlarmDate.SelectedDate.Value.Date.AddHours(hours).AddMinutes(minutes);
            Alarm alarm = new Alarm
            {
                AlarmName = alarmName,
                AlarmTime = alarmTime,
                Enabled = true
            };
            alarmRepo.Create(alarm);
            
            alarmDataGrid.ItemsSource = alarmRepo.GetAll();
            alarmService.SetAlarm(alarmTime);
        }

        private void enableDisableButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            Alarm alarm = clickedButton.DataContext as Alarm;
            alarm.Enabled = !alarm.Enabled;
            alarmRepo.Update(alarm);
            var alrmList = alarmRepo.GetAll();
            alarmDataGrid.ItemsSource = alrmList;
            alarmService = new AlarmService();            
            alrmList.ForEach(alarm =>
            {
                if (alarm.Enabled)
                {
                    alarmService.SetAlarm(alarm.AlarmTime);
                }
            });
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            Alarm alarm = clickedButton.DataContext as Alarm;
            alarmRepo.Remove(alarm);
            var alrmList = alarmRepo.GetAll();
            alarmDataGrid.ItemsSource = alrmList;
            alarmService = new AlarmService();            
            alrmList.ForEach(alarm =>
            {
                if (alarm.Enabled)
                {
                    alarmService.SetAlarm(alarm.AlarmTime);
                }
            });
        }        

        private void AlarmService_AlarmTriggered(object sender, EventArgs e)
        {
            MessageBox.Show("Alarm triggered!");
        }

        private void CountdownService_TimeChanged(object sender, string time)
        {
            string[] parts = time.Split(':');
            Dispatcher.Invoke(() =>
            {
                txtHours.Text = parts[0];
                txtMinutes.Text = parts[1];
                txtSeconds.Text = parts[2];
            });            
        }

        private void CountdownService_CountdownFinished(object sender, EventArgs e)
        {
            MessageBox.Show("Countdown finished!");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int hours = int.Parse(txtHours.Text);
            int minutes = int.Parse(txtMinutes.Text);
            int seconds = int.Parse(txtSeconds.Text);

            if (countdownService != null)
            {
                countdownService.Stop();
            }

            countdownService = new CountdownService(hours, minutes, seconds);
            countdownService.TimeChanged += CountdownService_TimeChanged;
            countdownService.CountdownFinished += CountdownService_CountdownFinished;
            countdownService.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            countdownService.Stop();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            countdownService.Reset();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Get the caret index before appending the new text
            int caretIndexBefore = textBox.CaretIndex;

            // Append the new text
            var newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            // Get the caret index after appending the new text
            int caretIndexAfter = caretIndexBefore + e.Text.Length;

            if (newText.Length > 2)
            {
                e.Handled = true;
                return;
            }

            int value;
            if (!int.TryParse(newText, out value))
            {
                e.Handled = true; // Non-numeric characters are not allowed
                return;
            }

            // Determine the range based on the TextBox name
            switch (textBox.Name)
            {
                case "txtHours":
                    if (value < 0 || value > 23)
                        e.Handled = true; // Hours should be between 0 and 24
                    break;
                case "txtMinutes":
                case "txtSeconds":
                    if (value < 0 || value > 59)
                        e.Handled = true; // Minutes and seconds should be between 0 and 60
                    break;
            }
        }

    }
}
