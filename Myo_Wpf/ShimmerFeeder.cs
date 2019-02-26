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
        int count = 0; // number of items processed

        // queue to hold shimmer data
        public Queue<ShimmerModel> dataQueue = new Queue<ShimmerModel>(100);

        private readonly string comPort; // com port to be used for connection to shimmer

        // COM PORT may need to be changed depending on Shimmer device and System
        public ShimmerFeeder(string comPort)
        {
            this.comPort = comPort;
            this.dataQueue = new Queue<ShimmerModel>();
        }

        public void Start()
        {
            Console.WriteLine(Environment.OSVersion.Platform);
            ShimmerController sc = new ShimmerController(this);

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

        // this method is called for each model as it is received
        public void UpdateFeed(List<double> data)
        {
            if (data != null)
            {
                Shimmer3DModel s = Shimmer3DModel.GetModelFromArray(data.ToArray());
                dataQueue.Enqueue(s);
                if (count % 10 == 0) { Shimmer3DModel.PrintModel(s); }
                count++;
            }
        }
    }
}
