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

        private readonly DelegateCommand _disconnectCommand;
        public ICommand DisconnectCommand => _disconnectCommand;

        #endregion

        #region Constructor

        public Shimmer3dViewModel()
        {
            _streamAndConnectCommand = new DelegateCommand(ConnectAndStream);
            _disconnectCommand = new DelegateCommand(Disconnect);
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
            OutputText += "\nTrying to connect on " + this.comPort;

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
                lastShimmerModel = Shimmer3DModel.GetModelFromArray(data.ToArray());
                //dataQueue.Enqueue(s);
                //if (count % 10 == 0) { Shimmer3DModel.PrintModel(s); }
                //count++;
                Shimmer3DModel.PrintModel(lastShimmerModel);
                //RotateCube(s);

            }
        }

        #endregion
    }
}
