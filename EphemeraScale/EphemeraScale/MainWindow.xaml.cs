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


/*TODO LIST
 * 1. Make all events color coordinated. 
 * *Color white: Event hasn't started.
 * *Color red: Event has passed
 * **Active events stay white but appear under the :Active Event: area.
 * 2. Allow save settings.
 * 3. At the end of every 24 hour day, prompt to enter a description of your day.
 * 4. async with an SQL database for information output.
 * 5. Make the program run in the background, minimizing places it in the notification area.
 * 
 * note: In the event there is no text, do not create an event**[bug]
 * 
 * MORE INFORMATION
 * The data that Ephemera should take at the end of every day is as follows.
 * Each event has a user input description.
 * This description is only 80 characters long.
 * Upon entering this description the user is prompted to rate their productivity level.
 * This information is all output to a database.
 * 
 * WHAT ABOUT THE FUTURE?
 * Once all the bugs are knocked out I plan on developing a mobile application which will collect its own set of data.
 * 
 */

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
        } // End of MainWindow

        private void ephTimer_Tick(object sender, EventArgs e) { // Clock/Countdown
            int ephHours = (DateTime.Today.AddDays(1) - DateTime.Now).Hours;
            int ephMinu = (DateTime.Today.AddDays(1) - DateTime.Now).Minutes;
            int ephSecs = (DateTime.Today.AddDays(1) - DateTime.Now).Seconds;

            //EphClock is the label beneath the timeline
            //EphTimeLine is... the timeline.
            //Eph timeline max is set to 24 to represent the hours

            ephTimeLine.Maximum = secondsPerDay;


            //ephClock should never be used again, avoid it.
            ephClock.Content = (ephHours < 10 ? "0" : "") + ephHours.ToString() + ":"
                             + (ephMinu < 10 ? "0" : "") + ephMinu.ToString() + ":"
                             + (ephSecs < 10 ? "0" : "") + ephSecs.ToString();

            ephTimeLine.Value = ephTimeLine.Maximum - (((ephHours * 60 + ephMinu) * 60) + ephSecs);
          
 
        } // End of EphTimer_Tick

        private void onLabelClicked(object sender, MouseButtonEventArgs e) { // Mouse clicked label
            Label label = sender as Label;
            // Set label as current if we haven't done that already
            if (currentLabel != label) {
                currentLabel = label;
                onLabelDrag(sender, e);
            }
        } // End of OnLabelClicked

        private void onLabelDrag(object sender, System.Windows.Input.MouseEventArgs e) { // Mouse down
            // If we're not dragging anything we don't need to do anything
            if (currentLabel == null) return;
            // Get mouse position
            Point mouse = e.GetPosition(null);
            // Set X and Y to the mouse's to make the label stick to the mouse
            currentLabel.SetValue(Canvas.LeftProperty, mouse.X - currentLabel.ActualWidth / 2);
            currentLabel.SetValue(Canvas.TopProperty, mouse.Y - currentLabel.ActualHeight / 2);
        } // End of OnLabelDrag

        private void onLabelDrop(object sender, System.Windows.Input.MouseButtonEventArgs e) { // Mouse Release
            // Set current label to null so we don't drag anything anymore
            currentLabel = null;
        } // OnLabelDrop

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        } // End of CloseCommandHandler

        /* WHAT IS: This creates a label via the Create Event button. 
         * PURPOSE: Creates new draggable labels which can be used as events.
         * COMMENTS: Don't fucking touch this you losers.
         */
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

        } // End of createLabel
    } // End of MainWindow
} // End of NameSpace
