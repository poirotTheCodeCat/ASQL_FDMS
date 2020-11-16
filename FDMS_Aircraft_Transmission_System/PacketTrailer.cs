using System;
using System.Collections.Generic;
using System.Text;

namespace FDMS_Aircraft_Transmission_System
{
    class PacketTrailer
    {
        private int checksum;

        public PacketTrailer()
        {

        }

        public PacketTrailer(int check_sum)
        {
            checksum = check_sum;
        }

        public int Checksum
        {
            get { return checksum; }
            set { checksum = value; }
        }
    }
}
