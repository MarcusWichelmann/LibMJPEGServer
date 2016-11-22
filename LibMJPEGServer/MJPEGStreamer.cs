using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibMJPEGServer.JpegEncoding;
using LibMJPEGServer.QualityManagement;
using LibMJPEGServer.Sources;
using LibThreadedSockets;

namespace LibMJPEGServer
{
    public class MJPEGStreamer
    {
        public VideoSource Source { get; }
        public QualityDefinition QualityDefinition { get; }

        public CachedEncoder Encoder { get; }

        public List<ClientStream> CurrentStreams { get; } = new List<ClientStream>();

        public event EventHandler<StreamStartedEventArgs> StreamStarted;
        public event EventHandler<StreamStoppedEventArgs> StreamStopped;

        public MJPEGStreamer(VideoSource source, QualityDefinition qualityDefinition)
        {
            Encoder = new CachedEncoder(Source = source, QualityDefinition = qualityDefinition);
        }

        public ClientStream AddClient(ClientConnection client)
        {
            ClientStream stream = new ClientStream(client, Encoder);

            Task.Run(() => {
                stream.Run().Wait();
                stream.Stop();

                CurrentStreams.Remove(stream);
                StreamStopped?.Invoke(this, new StreamStoppedEventArgs(stream));

                if(!CurrentStreams.Any())
                    Source.StopCapture();
            });

            if(!CurrentStreams.Any())
                Source.StartCapture();

            CurrentStreams.Add(stream);
            StreamStarted?.Invoke(this, new StreamStartedEventArgs(stream));

            return stream;
        }
    }
}