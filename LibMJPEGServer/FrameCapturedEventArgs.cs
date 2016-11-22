using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer
{
    public class FrameCapturedEventArgs
    {
        public Image Frame { get; }

        public FrameCapturedEventArgs(Image frame)
        {
            Frame = frame;
        }
    }
}
