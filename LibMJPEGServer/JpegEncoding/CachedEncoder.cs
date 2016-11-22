using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibMJPEGServer.QualityManagement;
using LibMJPEGServer.Sources;

namespace LibMJPEGServer.JpegEncoding
{
    public class CachedEncoder
    {
        public VideoSource Source { get; }
        public QualityDefinition QualityDefinition { get; }

        private static ImageCodecInfo _jpegEncoder;

        private readonly Dictionary<int, Task<MemoryStream>> _encodings = new Dictionary<int, Task<MemoryStream>>();

        private Image _currentFrame = null;

        public event EventHandler FrameUpdated;

        public CachedEncoder(VideoSource source, QualityDefinition qualityDefinition)
        {
            Source = source;
            QualityDefinition = qualityDefinition;

            Source.FrameCaptured += OnFrameCaptured;

            _jpegEncoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
        }

        public int GetQuality(int lastQuality = -1, int transmissionTime = -1)
        {
            return lastQuality <= 0 || transmissionTime <= 0 ? QualityDefinition.GetDefaultQuality() : QualityDefinition.GetQualityForFps(lastQuality, transmissionTime);
        }

        public async Task<MemoryStream> GetCurrentFrame(int quality)
        {
            if(_currentFrame == null)
                throw new InvalidOperationException("No frame captured yet.");

            lock(_encodings)
            {
                if(_encodings.ContainsKey(quality))
                {
                    Task<MemoryStream> encodingTask = _encodings[quality];

                    if(!encodingTask.IsCompleted)
                        encodingTask.Wait();

                    return encodingTask.Result;
                }
            }

            Task<MemoryStream> newEncoding = Task.Run(() => EncodeFrame(quality));

            lock(_encodings)
                _encodings[quality] = newEncoding;

            return await newEncoding;
        }

        private MemoryStream EncodeFrame(int quality)
        {
            MemoryStream frameStream = new MemoryStream();

            using(EncoderParameters encoderParameters = new EncoderParameters(1))
            using(encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality))
            {
                lock(_currentFrame)
                    _currentFrame.Save(frameStream, _jpegEncoder, encoderParameters);
            }

            return frameStream;
        }

        private void OnFrameCaptured(object sender, FrameCapturedEventArgs e)
        {
            if(_currentFrame == null)
            {
                _currentFrame = e.Frame;
            }
            else
            {
                lock(_currentFrame)
                {
                    _currentFrame?.Dispose();
                    _currentFrame = e.Frame;
                }
            }

            lock(_encodings)
                _encodings.Clear();

            FrameUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}