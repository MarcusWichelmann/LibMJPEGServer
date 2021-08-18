using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer.Sources
{
    public interface VideoSource
    {
        event EventHandler<FrameCapturedEventArgs> FrameCaptured;

        void StartCapture();
        void StopCapture();
    }
}
