using System;
using System.Collections.Generic;
using System.Text;

namespace dg_sm_jd_em_FDMS
{
    class PacketBody
    {
        private string teldata;

        public string Teldata
        {
            get { return teldata; }
            set { teldata = value; }
        }

        public PacketBody()
        {

        }

        public PacketBody(string telemetryData)
        {
            teldata = telemetryData;
        }
    }
}
