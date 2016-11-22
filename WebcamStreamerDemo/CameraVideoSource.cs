using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using LibMJPEGServer;
using LibMJPEGServer.Sources;

namespace WebcamStreamerDemo
{
    internal class CameraVideoSource : VideoSource
    {
        private Capture _capture;

        private Mat _currentFrame = new Mat();

        public override event EventHandler<FrameCapturedEventArgs> FrameCaptured;

        public CameraVideoSource()
        {
            _capture = new Capture(0);
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);
            _capture.ImageGrabbed += OnImageGrabbed;
        }

        public override void StartCapture() => _capture.Start();
        public override void StopCapture() => _capture.Stop();

        private void OnImageGrabbed(object sender, EventArgs e)
        {
            _capture.Retrieve(_currentFrame);

            FrameCaptured?.Invoke(this, new FrameCapturedEventArgs(_currentFrame.Bitmap));
        }
    }
}