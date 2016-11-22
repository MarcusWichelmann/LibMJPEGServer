using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibMJPEGServer.Sources;
using LibThreadedSockets;
using LibMJPEGServer.JpegEncoding;

namespace LibMJPEGServer
{
    public class ClientStream
    {
        private const string Boundary = "--boundary";
        private const int TransmissionUpdateCount = 50;

        public ClientConnection TargetClient { get; }
        public CachedEncoder Encoder { get; }

        private bool _streamActive = true;
        private ManualResetEvent _newFrameAvailableEvent = new ManualResetEvent(false);

        private Stopwatch _stopwatch = new Stopwatch();
        private List<int> _lastTransmissionTimes = new List<int>();

        private int _lastQuality = -1;

        public ClientStream(ClientConnection targetClient, CachedEncoder encoder)
        {
            TargetClient = targetClient;
            Encoder = encoder;

            Encoder.FrameUpdated += OnFrameUpdated;
        }

        public async Task Run()
        {
            string header = "HTTP/1.1 200 OK\r\n" +
                            $"Content-Type: multipart/x-mixed-replace; boundary={Boundary}\r\n" +
                            "Server: LibMJPEGStreamer\r\n";

            try
            {
                TargetClient.Send(Encoding.UTF8.GetBytes(header));

                while(_streamActive)
                {
                    _newFrameAvailableEvent.WaitOne();
                    _newFrameAvailableEvent.Reset();

                    if(!_streamActive)
                        break;

                    await SendFrame();
                }
            }
            catch
            {
                // Shit happens...
                // Client probably disconnected.
            }
        }

        public void Stop()
        {
            _streamActive = false;
            _newFrameAvailableEvent.Set();

            Encoder.FrameUpdated -= OnFrameUpdated;
        }

        private async Task SendFrame()
        {
            if(_lastQuality <= 0)
                _lastQuality = Encoder.GetQuality();

            MemoryStream frameStream = await Encoder.GetCurrentFrame(_lastQuality);

            string header = $"\r\n{Boundary}\r\n" +
                            "Content-Type: image/jpeg\r\n" +
                            $"Content-Length: {frameStream.Length}\r\n\r\n";

            _stopwatch.Restart();
            {
                TargetClient.Send(Encoding.UTF8.GetBytes(header));
                TargetClient.Send(frameStream);
                TargetClient.Send(Encoding.UTF8.GetBytes("\r\n"));
            }
            _stopwatch.Stop();

            int nextTransmissionTime = (int)_stopwatch.ElapsedMilliseconds;

            if(_lastTransmissionTimes.Count >= TransmissionUpdateCount)
            {
                _lastQuality = Encoder.GetQuality(_lastQuality, (int)_lastTransmissionTimes.Average());
                _lastTransmissionTimes.Clear();
            }

            // 1000 FPS ist good enough...
            if(nextTransmissionTime <= 0)
                nextTransmissionTime = 1;

            _lastTransmissionTimes.Add(nextTransmissionTime);
        }

        private void OnFrameUpdated(object sender, EventArgs e)
        {
            _newFrameAvailableEvent.Set();
        }
    }
}