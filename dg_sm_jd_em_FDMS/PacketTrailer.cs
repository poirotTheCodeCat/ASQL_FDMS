using System;
using System.Collections.Generic;
using System.Text;

namespace dg_sm_jd_em_FDMS
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
