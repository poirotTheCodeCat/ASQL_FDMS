using System;
using System.Collections.Generic;
using System.Text;

namespace FDMS_Aircraft_Transmission_System
{
    class PacketHead
    {
        private string tailNum;
        private int packetNum;

        public PacketHead()
        {

        }

        public PacketHead(string tail, int packetNumber)
        {
            tailNum = tail;
            packetNum = packetNumber;
        }

        public string TailNum
        {
            get { return tailNum; }
            set { tailNum = value; }
        }

        public int PacketNum
        {
            get { return packetNum; }
            set { packetNum = value; }
        }
    }
}
