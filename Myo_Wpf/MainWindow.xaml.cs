using ShimmerInterfaceTest;
using ShimmerRT;
using ShimmerRT.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Myo_Wpf
{
    public partial class MainWindow : Window, IFeedable
    {
        //port for the shimmer
        public string comPort;

        //ref to shimmer controller
        private ShimmerController sc;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Connecting/Disconnecting Logic
        public void ConnectAndStream()
        {
            comPort = txtbxComPort.Text;
            txtOutput.Text = "Connecting...";
            txtOutput.Text = "COM PORT: " + comPort;
            Debug.WriteLine("COM PORT: " + comPort);
            sc = new ShimmerController(this);
            txtOutput.Text += "\nTrying to connect on " + this.comPort;
            sc.Connect(comPort);

            do
            {
                System.Threading.Thread.Sleep(100);
            } while (!sc.ShimmerDevice.IsConnected());
            
            txtOutput.Text += "\nConnected";

            txtOutput.Text += "\nStarting stream...";

            sc.ShimmerDevice.Set3DOrientation(true);

            sc.StartStream();
        }

        public void Disconnect()
        {
            //print("Stopping stream...");
            txtOutput.Text += "\nStopping stream";
            sc.StopStream();
            sc.ShimmerDevice.Disconnect();
            sc = null;
            //do
            //{
            //    System.Threading.Thread.Sleep(100);
            //} while (sc.ShimmerDevice.IsConnected());
            txtOutput.Text += "\nStream Stopped";
            txtOutput.Text += "\nDisconnected";
        }

        public int randomInt()
        {
            Random r = new Random();
            int rInt = r.Next(0, 361);

            return rInt;
        }

        private void RotX_Clicked(object sender, RoutedEventArgs e)
        {
            Quaternion q = new Quaternion(new Vector3D(1, 0, 0), randomInt());
            // Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), randomInt()));
            Cube.Transform = new RotateTransform3D(new QuaternionRotation3D(q));
        }

        private void RotY_Clicked(object sender, RoutedEventArgs e)
        {
            Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), randomInt()));
        }

        private void RotZ_Clicked(object sender, RoutedEventArgs e)
        {
            Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), randomInt()));
        }

        private void RotateCube(Shimmer3DModel s)
        {
            // Quaternion Calibrated Values
            //var x = s.Quaternion_0_CAL;
            //var y = s.Quaternion_1_CAL;
            //var z = s.Quaternion_2_CAL;
            //var w = s.Quaternion_3_CAL;

            // Gyroscope Calibrated Values
            var x = s.Gyroscope_X_CAL;
            var y = s.Gyroscope_X_CAL;
            var z = s.Gyroscope_X_CAL;

            //Shimmer3DModel.PrintModel(s);

            
            //Quaternion q = new Quaternion(new Vector3D(x, y, z), w);
            //Cube.Transform = new RotateTransform3D(new QuaternionRotation3D(q));

            Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), x));
            Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), y));
            Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), z));
            
        }

        public void UpdateFeed(List<double> data)
        {
            if (data != null)
            {
                Shimmer3DModel s = Shimmer3DModel.GetModelFromArray(data.ToArray());
                //dataQueue.Enqueue(s);
                //if (count % 10 == 0) { Shimmer3DModel.PrintModel(s); }
                //count++;

                RotateCube(s);

            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectAndStream();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }
    }
    #region temp connect 
    //public void connectToShimmer(String port, int baudRate, Parity parity, int databits, StopBits stopBits)
    //{
    //    //https://www.youtube.com/watch?v=BGb5LIyMmvM
    //    //https://docs.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?redirectedfrom=MSDN&view=netframework-4.7.2
    //    comPort = new SerialPort(port, baudRate, parity, databits, stopBits);

    //    try
    //    {
    //        //comPort.DataReceived += new SerialDataReceivedEventHandler();
    //    }
    //    catch (Exception ex){ MessageBox.Show(ex.ToString() + " In ConnectToShimmer"); }

    //}

    //private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    //{

    //}
    #endregion
}
