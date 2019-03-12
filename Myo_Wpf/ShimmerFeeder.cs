using ShimmerAPI;
using ShimmerInterfaceTest;
using ShimmerRT;
using ShimmerRT.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myo_Wpf
{
    class ShimmerFeeder : IFeedable
    {
        #region == Fields and Properties ==
        private string comPort;
        private ShimmerController sc;

        // data structure used to shared data between this Shimmer 
        // and ShimmerJointOrientation
        public Queue<Shimmer3DModel> Queue { get; set; }

        // True if and only if this Shimmer has paired successfully, at which point it will 
        // provide data and a connection with it will be maintained when possible.
        public bool IsPaired
        {
            get { return sc != null && sc.ShimmerDevice.IsConnected(); }
        }
        #endregion

        // COM PORT may need to be changed depending on Shimmer device and System
        public ShimmerFeeder(string comPort)
        {
            this.comPort = comPort;
            Queue = new Queue<Shimmer3DModel>();
        }

        public void Start()
        {
            Console.WriteLine(Environment.OSVersion.Platform);
            sc = new ShimmerController(this);

            sc.Connect(comPort);
            Console.WriteLine("Connecting...");
            do // TODO : fix this, can cause hang if cannot connect to Shimmer
            {
                System.Threading.Thread.Sleep(100);
            } while (!sc.ShimmerDevice.IsConnected());

            Console.WriteLine("Connected");
            Console.WriteLine("Starting stream...");

            sc.ShimmerDevice.Set3DOrientation(true); // set 3D orientation
            sc.ShimmerDevice.WriteBaudRate(230400); // set baud rates
            Console.WriteLine("Baud Rate: " + sc.ShimmerDevice.GetBaudRate());

            Console.WriteLine("Sampling Rate: " + sc.ShimmerDevice.GetSamplingRate());

            sc.StartStream();

            Console.ReadKey();
            Console.WriteLine("Stopping stream...");
            sc.StopStream();
            Console.ReadKey();
        }

        #region == Get data ==
        // this method is called for each row of data received from the Shimmer
        public void UpdateFeed(List<double> data)
        {
            Shimmer3DModel s;
            if (data.Count > 0)
            {
                // put this data as a model on the shared Queue
                s = Shimmer3DModel.GetModelFromArray(data.ToArray());
                Queue.Enqueue(s);
            }
        }
        #endregion
    }
}
