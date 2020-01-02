using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using System.IO;

namespace EphemeraScale {

    public partial class MainWindow : Window {
        DispatcherTimer ephTimer = new DispatcherTimer();
        Label currentLabel;
        const double secondsPerDay = 24 * 60 * 60;

        public MainWindow() {
            InitializeComponent();
            ephTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
            ephTimer.Tick += new EventHandler(ephTimer_Tick);
            ephTimer.Start();
        } 

        private void ephTimer_Tick(object sender, EventArgs e) { 
            int ephHours = (DateTime.Today.AddDays(1) - DateTime.Now).Hours;
            int ephMinu = (DateTime.Today.AddDays(1) - DateTime.Now).Minutes;
            int ephSecs = (DateTime.Today.AddDays(1) - DateTime.Now).Seconds;

            //EphClock is the label beneath the timeline
            //Eph timeline max is set to 24 to represent the hours

            ephTimeLine.Maximum = secondsPerDay;


            //ephClock should never be used again, avoid it.
            ephClock.Content = (ephHours < 10 ? "0" : "") + ephHours.ToString() + ":"
                             + (ephMinu < 10 ? "0" : "") + ephMinu.ToString() + ":"
                             + (ephSecs < 10 ? "0" : "") + ephSecs.ToString();

            ephTimeLine.Value = ephTimeLine.Maximum - (((ephHours * 60 + ephMinu) * 60) + ephSecs);
          
 
        }

        private void onLabelClicked(object sender, MouseButtonEventArgs e) { // Mouse clicked label
            Label label = sender as Label;
            if (currentLabel != label) {
                currentLabel = label;
                onLabelDrag(sender, e);
            }
        }

        private void onLabelDrag(object sender, System.Windows.Input.MouseEventArgs e) {
            if (currentLabel == null) return;
            Point mouse = e.GetPosition(null);
            currentLabel.SetValue(Canvas.LeftProperty, mouse.X - currentLabel.ActualWidth / 2);
            currentLabel.SetValue(Canvas.TopProperty, mouse.Y - currentLabel.ActualHeight / 2);
        } 

        private void onLabelDrop(object sender, System.Windows.Input.MouseButtonEventArgs e) { // Mouse Release
            currentLabel = null;
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        } 

        private void createLabel(object sender, System.Windows.RoutedEventArgs e) { // Create Event Button
            Label eventCreateText = new Label();
            eventCreateText.Foreground = new SolidColorBrush(Colors.White);
            eventCreateText.Content = labelNameBox.Text;
            eventCreateText.FontSize = 16;
            eventCreateText.FontFamily = new FontFamily("Segoe UI");
            eventCreateText.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            eventCreateText.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            ephCanvas.Children.Add(eventCreateText);
            eventCreateText.MouseDown += onLabelClicked;
            eventCreateText.MouseUp += onLabelDrop;
            eventCreateText.SetValue(Canvas.LeftProperty, ephCanvas.ActualWidth / 2);
            eventCreateText.SetValue(Canvas.TopProperty, ephCanvas.ActualHeight / 2);

        } 
    } 
} 
