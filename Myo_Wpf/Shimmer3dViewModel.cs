using ShimmerAPI;
using ShimmerInterfaceTest;
using ShimmerRT;
using ShimmerRT.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Myo_Wpf
{
    public class Shimmer3dViewModel : BaseViewModel, IFeedable
    {
        private System.Windows.Media.Media3D.Quaternion q;
        public System.Windows.Media.Media3D.Quaternion Q
        {
            get { return q; }
            set
            {
                q = value;
                OnPropertyChanged();
            }
        }


        #region Fields and Properties

        //ref to shimmer controller
        private ShimmerController sc;

        // COM port to connect to
        private string comPort;
        public string ComPort
        {
            get { return comPort; }
            set { comPort = value; }
        }

        private string outputText;
        public string OutputText
        {
            get { return outputText; }
            set
            {
                outputText = value;
                OnPropertyChanged();
            }
        }

        private Shimmer3DModel lastShimmerModel;
        public Shimmer3DModel LastShimmerModel
        {
            get { return lastShimmerModel; }
            set
            {
                lastShimmerModel = value;
                OnPropertyChanged();
            }
        }

        private RotateTransform3D cubeRotation;
        public RotateTransform3D CubeRotation
        {
            get { return cubeRotation; }
            set
            {
                cubeRotation = value;
                OnPropertyChanged();
            }
        }

        private readonly MainWindow _parent;
        private readonly DelegateCommand _streamAndConnectCommand;
        public ICommand StreamAndConnectCommand => _streamAndConnectCommand;

        private readonly DelegateCommand _disconnectCommand;
        public ICommand DisconnectCommand => _disconnectCommand;


        private double xRot;
        public double XRot
        {
            get => xRot;
            set
            {
                xRot = value;
                OnPropertyChanged();
            }
        }

        private double yRot;
        public double YRot
        {
            get => yRot;
            set
            {
                yRot = value;
                OnPropertyChanged();
            }
        }

        private double zRot;
        public double ZRot
        {
            get => zRot;
            set
            {
                zRot = value;
                OnPropertyChanged();
            }
        }

        private Vector3D xAxis;
        public Vector3D XAxis
        {
            get => xAxis;
            set
            {
                xAxis = value;
                OnPropertyChanged();
            }
        }

        private Vector3D yAxis;
        public Vector3D YAxis
        {
            get => yAxis;
            set
            {
                yAxis = value;
                OnPropertyChanged();
            }
        }

        private Vector3D zAxis;
        public Vector3D ZAxis
        {
            get => zAxis;
            set
            {
                zAxis = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Shimmer3DModel> _models;
        public ObservableCollection<Shimmer3DModel> Models
        {
            get => _models;
            set
            {
                _models = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Constructor

        public Shimmer3dViewModel(MainWindow parent)
        {
            _parent = parent;
            _streamAndConnectCommand = new DelegateCommand(ConnectAndStream);
            _disconnectCommand = new DelegateCommand(Disconnect);

            XAxis = new Vector3D(0, 0, 1);
            YAxis = new Vector3D(1, 0, 0);
            ZAxis = new Vector3D(0, 1, 0);
        }

        #endregion

        #region ShimmerController Connect/Disconnect

        //Connecting/Disconnecting Logic
        private void ConnectAndStream(object cmdParam)
        {
            Debug.WriteLine("CONNECT AND STREAM");
            Debug.WriteLine("CONNECTING ON " + comPort);

            OutputText += "\nConnecting...";
            OutputText += "\nCOM PORT: " + comPort;
            OutputText += "\nTrying to connect on COM" + this.comPort;

            int portNum;
            string connStr = "COM";
            try
            {
                portNum = Convert.ToInt32(comPort);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                OutputText += "\nINVALID COM PORT";
                return;
            }

            sc = new ShimmerController(this);
            sc.Connect(connStr + portNum);


            //TODO: improve this code - potential hang
            do
            {
                System.Threading.Thread.Sleep(100);
            } while (!sc.ShimmerDevice.IsConnected());

            OutputText += "\nConnected";

            OutputText += "\nStarting stream...";

            sc.ShimmerDevice.Set3DOrientation(true);

            sc.StartStream();
        }

        private void Disconnect(object cmdParam)
        {
            Debug.WriteLine("DISCONNECT");
            OutputText += "\nStopping stream";
            sc.StopStream();
            sc.ShimmerDevice.Disconnect();
            sc = null;
            OutputText += "\nStream Stopped";
            OutputText += "\nDisconnected";
        }

        #endregion

        #region IFeedable implementation

        public void UpdateFeed(List<double> data)
        {
            if (data != null)
            {
                //lastShimmerModel = Shimmer3DModel.GetModelFromArray(data.ToArray());
                //Shimmer3DModel.PrintModel(lastShimmerModel);
                ////queue.Enqueue(s);

                //var x = lastShimmerModel.Quaternion_0_CAL;
                //var y = lastShimmerModel.Quaternion_1_CAL;
                //var z = lastShimmerModel.Quaternion_2_CAL;
                //var w = lastShimmerModel.Quaternion_3_CAL;

                //System.Windows.Media.Media3D.Quaternion q = new System.Windows.Media.Media3D.Quaternion(x, y, z, w);
                //cubeRotation = new RotateTransform3D(new QuaternionRotation3D(q));

                //Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), x));
                //Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), y));
                //Cube.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), z));



                //// Trying to freeze the rotation before applying it to CubeRotation Property
                //var rot = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), z));
                //rot.Freeze();
                //// Trying to create the rotation object on UI thread...?
                ///

                Dispatcher.CurrentDispatcher.Invoke(
                    () =>
                    {
                        lastShimmerModel = Shimmer3DModel.GetModelFromArray(data.ToArray());
                        Shimmer3DModel.PrintModel(lastShimmerModel);
                        //queue.Enqueue(s);

                        //_models.Add(lastShimmerModel);

                        //UpdateView(lastShimmerModel);



                        var x = lastShimmerModel.Gyroscope_X_CAL;
                        var y = lastShimmerModel.Gyroscope_Y_CAL;
                        var z = lastShimmerModel.Gyroscope_Z_CAL;
                        //var w = lastShimmerModel.Quaternion_3_CAL;

                        ////// This works in GUI
                        XRot = x;
                        YRot = y;
                        ZRot = z;

                        //CubeRotation = new RotateTransform3D(
                        //    new AxisAngleRotation3D(new Vector3D(0, 0, 1), z)
                        //    );
                    }
                    );

                //XAxis = new Vector3D(1, 0, 0);

            }
        }

        private void UpdateView(Shimmer3DModel lastShimmerModel)
        {
            var x = lastShimmerModel.Gyroscope_X_CAL;
            var y = lastShimmerModel.Quaternion_1_CAL;
            var z = lastShimmerModel.Quaternion_2_CAL;
            var w = lastShimmerModel.Quaternion_3_CAL;

            //// This works in GUI
            XRot = x;

            //// This does not work in GUI - dependency source error
            CubeRotation = new RotateTransform3D(
                new AxisAngleRotation3D(new Vector3D(0, 0, 1), z)
                );
        }

        #endregion
    }
}
