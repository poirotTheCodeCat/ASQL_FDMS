using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace dg_sm_jd_em_FDMS
{
    static class TelProcess
    {
        /*
         * Function: Telemetry process(byte[] packetBytes)
         * Description: process a packet by extracting telemetry data and inserting calling a database function to store 
         * it in the database
         */
        public static Telemetry process(byte[] packetBytes)
        {
            try
            {
                String recMessage = System.Text.Encoding.ASCII.GetString(packetBytes, 0, packetBytes.Length);
                Packet packet = JsonConvert.DeserializeObject<Packet>(recMessage.Trim());
                return process(packet);
            }
            catch
            {
                return null;
            }
        }

        /*
         * Function: Telemetry process(Packet packet)
         * Description: process a packet by extracting telemetry data and inserting calling a database function to store 
         * it in the database
         */
        public static Telemetry process(Packet packet)
        {
            // process the body of the packet to retrieve the values
            // body teldata in form - date/time, x,y,z, weight, alt, pitch, bank

            string[] aircraftData = packet.Body.Teldata.Split(",");
            try
            {
                // process the packet to create telemetry object
                Telemetry tel = new Telemetry(packet.Head.TailNum, Double.Parse(aircraftData[1]), Double.Parse(aircraftData[2]), Double.Parse(aircraftData[3]), Double.Parse(aircraftData[4]),
                    Double.Parse(aircraftData[5]), Double.Parse(aircraftData[6]), Double.Parse(aircraftData[7]), Convert.ToDateTime(aircraftData[0]));

                // check if the checksum is correct
                if(packet.Trailer.Checksum != Packet.calculateCheckSum(tel))
                {
                    return null;
                }
                else
                {
                    // call database method to insert telemetry data


                    return tel;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
