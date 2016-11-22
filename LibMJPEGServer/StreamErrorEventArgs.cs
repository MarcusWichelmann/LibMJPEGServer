using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibThreadedSockets;

namespace LibMJPEGServer
{
    public class StreamErrorEventArgs
    {
        public string Message { get; }
        public SocketErrorEventArgs SocketError { get; }

        public StreamErrorEventArgs(string message, SocketErrorEventArgs socketError = null)
        {
            Message = message;
            SocketError = socketError;
        }
    }
}