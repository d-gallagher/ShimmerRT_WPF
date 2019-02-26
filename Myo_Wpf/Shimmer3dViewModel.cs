using ShimmerAPI;
using ShimmerInterfaceTest;
using ShimmerRT.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace Myo_Wpf
{
    public class Shimmer3dViewModel : BaseViewModel, IFeedable
    {
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

        private readonly DelegateCommand _streamAndConnectCommand;
        public ICommand StreamAndConnectCommand => _streamAndConnectCommand;


        #endregion

        #region Constructor

        public Shimmer3dViewModel()
        {
            _streamAndConnectCommand = new DelegateCommand(ConnectAndStream);
        }

        #endregion

        #region ShimmerController Connect/Disconnect

        //Connecting/Disconnecting Logic
        private void ConnectAndStream(object cmdParam)
        {
            //comPort = txtbxComPort.Text;
            OutputText += "\nConnecting...";
            OutputText += "\nCOM PORT: " + comPort;
            Debug.WriteLine("COM PORT: " + comPort);
            //sc = new ShimmerController(this);
            OutputText += "\nTrying to connect on " + this.comPort;

            //sc.Connect(comPort);

            //do
            //{
            //    System.Threading.Thread.Sleep(100);
            //} while (!sc.ShimmerDevice.IsConnected());

            OutputText += "\nConnected";

            OutputText += "\nStarting stream...";

            //sc.ShimmerDevice.Set3DOrientation(true);

            //sc.StartStream();
        }

        public void Disconnect()
        {
            //print("Stopping stream...");
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
                Shimmer3DModel s = Shimmer3DModel.GetModelFromArray(data.ToArray());
                //dataQueue.Enqueue(s);
                //if (count % 10 == 0) { Shimmer3DModel.PrintModel(s); }
                //count++;

                //RotateCube(s);

            }
        }

        #endregion
    }
}
