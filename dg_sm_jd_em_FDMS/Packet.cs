using System;
using System.Collections.Generic;
using System.Text;

namespace dg_sm_jd_em_FDMS
{
    public class Packet
    {
        private PacketHead head;
        private PacketBody body;
        private PacketTrailer trailer;

        // accessors and mutators 
        public PacketHead Head
        {
            get { return head; }
            set { head = value; }
        }
        public PacketBody Body
        {
            get { return body; }
            set { body = value; }
        }
        public PacketTrailer Trailer
        {
            get { return trailer; }
            set { trailer = value; }
        }

        public Packet()
        {

        }

        public Packet(PacketHead packetHeader, PacketBody packetBody, PacketTrailer packetTrailer)
        {
            head = packetHeader;
            body = packetBody;
            trailer = packetTrailer;
        }

        public static int calculateCheckSum(Telemetry tel)
        {
            double checksum = (tel.Altitude + tel.Pitch + tel.Bank) / 3;
            int checkReturn = (int)Math.Floor(checksum);

            return checkReturn;
        }
    }
}
