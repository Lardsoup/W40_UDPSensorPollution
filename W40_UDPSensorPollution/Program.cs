using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace W40_UDPSensorPollution
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 9000;
            //string ipAddressloc = "127.0.0.1";
            IPAddress ip = IPAddress.Any;

            UdpClient UdpReciever = new UdpClient(port);
            IPEndPoint RemoteEndPoint = new IPEndPoint(ip, port);

            List<double> CO = new List<double>();
            List<double> NOx = new List<double>();

            int x = 0; //int til at stoppe den

            while (true)
            {
                try
                {
                    Byte[] recievebBytes = UdpReciever.Receive(ref RemoteEndPoint);

                    string recieveData = Encoding.ASCII.GetString(recievebBytes);
                    string[] data = recieveData.Split(char.Parse("\n"));

                    if (data[0].ToUpper() == "STOP" || x >= 5)
                    {
                        break;
                    }

                    foreach (string d in data)
                    {
                        string[] temp = d.Split(' ');
                        if (temp[0].Contains("CO"))
                        {
                            CO.Add(double.Parse(temp[1]));
                        }
                        else if (temp[0].Contains("NOx"))
                        {
                            NOx.Add(double.Parse(temp[1]));
                        }
                        Console.WriteLine(d);
                    }
                    x++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            Console.WriteLine("CO sum: " + CO.Sum() + "\n" + "CO Avg: " + CO.Average());
            Console.WriteLine("NOx sum: " + NOx.Sum() + "\n" + "NOx Avg: " + NOx.Average());
        }
    }
}
