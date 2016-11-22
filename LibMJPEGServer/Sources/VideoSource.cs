using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer.Sources
{
    public abstract class VideoSource
    {
        public abstract event EventHandler<FrameCapturedEventArgs> FrameCaptured;

        public abstract void StartCapture();
        public abstract void StopCapture();
    }
}
