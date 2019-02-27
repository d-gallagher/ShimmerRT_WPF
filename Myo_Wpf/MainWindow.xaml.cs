using ShimmerRT.models;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Myo_Wpf
{
    public partial class MainWindow : Window
    {
        #region Static

        private static Random rand = new Random(361);
        public int randomInt()
        {
            return rand.Next();
        }

        #endregion

        private Shimmer3dViewModel viewModel;

        //private ObservableCollection<Shimmer3DModel> _models = new ObservableCollection<Shimmer3DModel>();

        public MainWindow()
        {
            InitializeComponent();

            // instantiate and assign the view model
            DataContext = viewModel = new Shimmer3dViewModel(this);

            //// Trying to create a custom binding on cube Rotation to viewModel
            //Cube.Transform = new RotateTransform3D();
            //var bind = new Binding("CubeRotationCustom");
            //bind.Source = viewModel.CubeRotation;
            //BindingOperations.SetBinding(Cube, RotateTransform3D.RotationProperty, bind);
        }

        // TODO: unused in this class - will be moved to viewModel
        private void RotateCube(Shimmer3DModel s)
        {
            // Quaternion Calibrated Values
            var x = s.Quaternion_0_CAL;
            var y = s.Quaternion_1_CAL;
            var z = s.Quaternion_2_CAL;
            var w = s.Quaternion_3_CAL;

            //// Gyroscope Calibrated Values
            //var x = s.Gyroscope_X_CAL;
            //var y = s.Gyroscope_X_CAL;
            //var z = s.Gyroscope_X_CAL;

            //Shimmer3DModel.PrintModel(s);


            //Quaternion q = new Quaternion(new Vector3D(x, y, z), w);
            //Cube.Transform = new RotateTransform3D(new QuaternionRotation3D(q));

            //Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), x));
            //Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), y));
            //Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), z));

        }

        public void UpdateView()
        {
        }

        #region EventHandlers

        //private void BtnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    // TODO: implement using Command or MessagingCenter
        //    //ConnectAndStream();
        //}

        //private void BtnStop_Click(object sender, RoutedEventArgs e)
        //{
        //    // TODO: implement using Command or MessagingCenter
        //    //Disconnect();
        //}

        //private void RotX_Clicked(object sender, RoutedEventArgs e)
        //{
        //    Quaternion q = new System.Windows.Media.Media3D.Quaternion(new Vector3D(1, 0, 0), randomInt());
        //    // Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), randomInt()));
        //    Cube.Transform = new RotateTransform3D(new QuaternionRotation3D(q));
        //}

        //private void RotY_Clicked(object sender, RoutedEventArgs e)
        //{
        //    Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), randomInt()));
        //}

        //private void RotZ_Clicked(object sender, RoutedEventArgs e)
        //{
        //    Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), randomInt()));
        //}

        #endregion

        #region Alternative SerialPort connection code - retaining for future reference
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
}
