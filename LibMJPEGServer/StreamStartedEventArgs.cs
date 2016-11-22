using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer
{
    public class StreamStartedEventArgs
    {
        public ClientStream Stream { get; }

        public StreamStartedEventArgs(ClientStream stream)
        {
            Stream = stream;
        }
    }
}
