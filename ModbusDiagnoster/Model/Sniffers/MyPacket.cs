using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Sniffers
{
    public class MyPacket
    {
        public DateTime Time { get; set; }
        public IPAddress SourceIP { get; set; }
        public IPAddress DestinationIP { get; set; }
        public TcpPacket TCPpacket { get; set; }
        //ArpPacket ARPpacket { get; set; }
        public string Payload { get; set; }

        public MyPacket(DateTime time,IPAddress srcIp,IPAddress dstIp,TcpPacket packet)
        {
            Time = time;
            SourceIP = srcIp;
            DestinationIP = dstIp;
            TCPpacket = packet;

            foreach(byte fragment in TCPpacket.PayloadData)
            {
                Payload += fragment.ToString();
            }
            

        }
    }
}
