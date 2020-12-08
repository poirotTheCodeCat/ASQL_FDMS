using System;
using System.Collections.Generic;
using System.Text;

namespace dg_sm_jd_em_FDMS
{
    public class PacketHead
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
