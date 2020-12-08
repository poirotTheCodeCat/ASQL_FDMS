using System;
using System.Collections.Generic;
using System.Text;

namespace FDMS_Aircraft_Transmission_System
{
    public class PacketBody
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
