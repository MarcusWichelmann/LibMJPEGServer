using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibMJPEGServer;
using LibMJPEGServer.QualityManagement;
using LibMJPEGServer.Sources;

namespace WebcamStreamerDemo
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            CameraVideoSource videoSource = new CameraVideoSource();

            SimpleServer server = new SimpleServer(80, videoSource, new StaticQualityDefinition(60));
            server.Start();

            Console.WriteLine($"Server started: {server.ServerAddress}");

            Console.WriteLine("Press enter to stop!");
            Console.ReadLine();
        }
    }
}