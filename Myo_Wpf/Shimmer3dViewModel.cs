using ShimmerAPI;
using ShimmerInterfaceTest;
using ShimmerRT.models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Myo_Wpf
{
    public class Shimmer3dViewModel : BaseViewModel, IFeedable
    {

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
            get { return comPort; }
            set { comPort = value; }
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

        #region ShimmerController Connect/Disconnect

        //Connecting/Disconnecting Logic
        public void ConnectAndStream()
        {
            //comPort = txtbxComPort.Text;
            OutputText += "\nConnecting...";
            OutputText += "\nCOM PORT: " + comPort;
            Debug.WriteLine("COM PORT: " + comPort);
            sc = new ShimmerController(this);
            OutputText += "\nTrying to connect on " + this.comPort;
            sc.Connect(comPort);

            do
            {
                System.Threading.Thread.Sleep(100);
            } while (!sc.ShimmerDevice.IsConnected());

            OutputText += "\nConnected";

            OutputText += "\nStarting stream...";

            sc.ShimmerDevice.Set3DOrientation(true);

            sc.StartStream();
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
