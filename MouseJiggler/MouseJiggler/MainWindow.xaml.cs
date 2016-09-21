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
using System.Timers;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace MouseJiggler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int delay_between_jiggle = -1;
        Timer myTimer = new Timer();
        private bool is_running = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InputMinutesBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            int parsedValue;
            if (int.TryParse(InputMinutesBox.Text, out parsedValue) && parsedValue > 0)
            {
                delay_between_jiggle = parsedValue;
            }
            else
            {
                delay_between_jiggle = -1;
            }
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            if (delay_between_jiggle != -1)
            {
                myTimer.Interval = delay_between_jiggle * 60000;
                //change current button display to stop
                if ((string)MainButton.Content == "Start")
                {
                    //restart timer
                    try
                    {
                        MainButton.Content = "Stop";
                        myTimer.AutoReset = true;
                        myTimer.Elapsed += Jiggle;
                        myTimer.Start();
                    } catch (ObjectDisposedException exception) {
                        Console.WriteLine(exception.ToString());
                    }
                    
                }
                else
                {
                    //stop timer
                    MainButton.Content = "Start";
                    myTimer.Stop();
                    //myTimer.Dispose();
                }
            }
        }

        private static void Jiggle(Object source, ElapsedEventArgs e)
        {
            IntPtr handle = IntPtr.Zero;
            POINT orig_pos = GetCursorPosition();

            var sim = new WindowsInput.InputSimulator();
            try
            {
                sim.Mouse.MoveMouseTo(System.Windows.SystemParameters.PrimaryScreenWidth, System.Windows.SystemParameters.PrimaryScreenHeight);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            SetCursorPos(orig_pos.x, orig_pos.y);
            Console.WriteLine("Jiggled!");
        }

        [DllImport("User32.Dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        public static POINT GetCursorPosition()
        {
            POINT aPoint;
            GetCursorPos(out aPoint);
            return aPoint;
        }
        
    }   
}
