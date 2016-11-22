using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer
{
    public class StreamStoppedEventArgs
    {
        public ClientStream Stream { get; }

        public StreamStoppedEventArgs(ClientStream stream)
        {
            Stream = stream;
        }
    }
}
