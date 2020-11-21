using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


namespace FDMS_Aircraft_Transmission_System
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverName = "127.0.0.1";
            Int32 port = 15000;
            // int numOfPackets = 1;

            try
            {
                // start a tcp client connection with the aircraft Ground Terminal
                TcpClient client = new TcpClient();
                client.Connect(serverName, port);
                NetworkStream stream = client.GetStream();

                //String telFile = args[0];   // the file should be specified as a command line argument 
                String telFile = "C:\\Users\\Daniel\\Desktop\\FDMS\\FDMS_Aircraft_Transmission_System\\C-FGAX.txt";
                String tail = getTail(telFile); // get the tail number from the specified file

                String[] lines = null;

                try
                {
                    lines = System.IO.File.ReadAllLines(telFile);  // try to read from the file
                }
                catch
                {
                    // close the connection to the ground terminal
                    client.Close();
                    stream.Close();
                    throw new Exception("Could not find the specified file");
                }

                int packetNum = 1; // initialize packet number

                foreach(String line in lines)   // read all lines recorded from file
                {
                    Telemetry extractedTel = processLine(line, tail);   // extract telemetry from line
                    Packet packet = generatePacket(extractedTel, packetNum);

                    sendPacket(packet, stream); // call function to send line to the ground terminal

                    packetNum++;    // increment packetNum
                }

                //numOfPackets = sendPackets("C:\\tmp\\C-FGAX.txt", stream, numOfPackets);
                //numOfPackets = sendPackets("C:\\tmp\\C-GEFC.txt", stream, numOfPackets);
                //numOfPackets = sendPackets("C:\\tmp\\C-QWWT.txt", stream, numOfPackets);

                // close the connection to the ground terminal
                client.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*
         * Function: getTail(String fileName)
         * Description: retrieves the tail string from the end of the filename
         */
        private static String getTail(String fileName)
        {
            // regex to match the aircraft tail from the filename
            Regex aircraftTailRegex = new Regex("(C-.*)[^.txt]");
            var aircraftMatch = aircraftTailRegex.Match(fileName);
            // contains the aircraft tail 
            string aircraftTail = aircraftMatch.Groups[0].ToString();

            return aircraftTail;
        }


        /*
         * Function: processLine(String line, String tail)
         * Description: extracts telemetry data from a line read from the telemetry file
         */
        private static Telemetry processLine(String line, String tail)
        {
            
            string[] aircraftData = line.Split(',');
            try
            {
                Telemetry tData = new Telemetry(tail, Double.Parse(aircraftData[1]), Double.Parse(aircraftData[2]), Double.Parse(aircraftData[3]), Double.Parse(aircraftData[4]),
                    Double.Parse(aircraftData[5]), Double.Parse(aircraftData[6]), Double.Parse(aircraftData[7]), Convert.ToDateTime(aircraftData[0].Trim().Replace('_', '-')));

                return tData;
            }
            catch
            {
                return null;
            }
        }


        /*
         * Function: generatePacket(Telemetry tel, int packetNum)
         * Description: generate packet from the telemetry data
         */
        private static Packet generatePacket(Telemetry tel, int packetNum)
        {
            // generate packet head
            PacketHead head = new PacketHead(tel.TailNum, packetNum);

            // generate packet body
            // bild telemetry data string for body
            String telData = $"{tel.TimeStamp},{tel.Accel_x},{tel.Accel_y},{tel.Accel_z},{tel.Weight},{tel.Altitude},{tel.Pitch},{tel.Bank}";
            PacketBody body = new PacketBody(telData);

            // generate packet tail
            int checkSum = Packet.calculateCheckSum(tel);
            PacketTrailer trailer = new PacketTrailer(checkSum);

            // generate packet
            Packet packet = new Packet(head, body, trailer);

            return packet;
        }

        private static void sendPacket(Packet packet, NetworkStream stream)
        {
            String JSONPacket = JsonConvert.SerializeObject(packet);

            JSONPacket = JSONPacket + '\0';

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(JSONPacket);

            bool successfulPacket = false;

            byte[] readBytes = new byte[256];

            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();

            stream.Read(readBytes, 0, readBytes.Length);

            //stream.Read(readBytes, 0, bytes.Length);

            //while (!successfulPacket)
            //{
            //    byte[] readBytes = new byte[1024];

            //    stream.Write(bytes, 0, bytes.Length);
            //    stream.Flush();

            //    // waits for the response packet

            //    stream.Read(readBytes, 0, bytes.Length);

            //    // convert byte data back into packet
            //    String recPacketStr = System.Text.Encoding.ASCII.GetString(bytes);
            //    Packet recPacket = JsonConvert.DeserializeObject<Packet>(recPacketStr);


            //    // if the packet sequence and checksum is identical between the sent and recieved packets, successfulPacket is set to true
            //    // to move onto sending the next packet
            //    if (recPacket.Head.PacketNum == packet.Head.PacketNum && recPacket.Trailer.Checksum == packet.Trailer.Checksum)
            //    {
            //        successfulPacket = true;
            //    }
            //}
        }
    }
}
