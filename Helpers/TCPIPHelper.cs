using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SuperSimpleTcp;

namespace GPMI_Laser_Marking.Helpers
{
    public class TCPIPHelper
    {
        SimpleTcpClient tcpClient;
        public TCPIPHelper(string IP)
        {
            tcpClient = new SimpleTcpClient(IP);
        }
        
    }
}
