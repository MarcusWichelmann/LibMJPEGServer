using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibMJPEGServer.QualityManagement;
using LibMJPEGServer.Sources;
using LibThreadedSockets;

namespace LibMJPEGServer
{
    public class SimpleServer
    {
        private const string ServerUrl = "/live";

        private readonly ThreadedServer _server;
        private readonly MJPEGStreamer _streamer;

        public string ServerAddress => _server.GetServerAddress() + ServerUrl;

        public event EventHandler<StreamErrorEventArgs> StreamError;

        public SimpleServer(int port, VideoSource source, QualityDefinition qualityDefinition)
        {
            _server = new ThreadedServer(port);
            _server.ClientDataReceived += OnClientDataReceived;
            _server.SocketError += OnSocketError;
            _streamer = new MJPEGStreamer(source, qualityDefinition);
        }

        public void Start() => _server.Start();
        public void Stop() => _server.Stop();

        private void OnClientDataReceived(object sender, ClientDataReceivedEventArgs e)
        {
            string requestHeader = Encoding.UTF8.GetString(e.Data);
            if(!requestHeader.Contains($"GET {ServerUrl}"))
            {
                // TODO: 404 Error Message
                _server.DisconnectClient(e.ClientConnection);
                return;
            }

            _streamer.AddClient(e.ClientConnection);
        }

        private void OnSocketError(object sender, SocketErrorEventArgs e)
        {
            StreamError?.Invoke(this, new StreamErrorEventArgs($"Socket error: {e.Message}", e));
        }
    }
}