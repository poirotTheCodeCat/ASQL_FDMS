using Microsoft.VisualStudio.TestTools.UnitTesting;
using dg_sm_jd_em_FDMS;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class TestsFDMS
    {
        [TestMethod]
        public void AccessDatabaseTest()
        {
            string dbConStr = "Data Source=SASHAMALESEFDB0;Initial Catalog=GroundTerminal;Integrated Security=True;";
            string search = "CFJAX";
            try
            {
                List<Telemetry> telSearch = SqlDataAccess.getRecords(search, dbConStr);
                Assert.AreEqual(telSearch.Count, 0);
            }
            catch (System.Exception e)
            {
                Assert.Fail();
            }

        }
        
        
        
        [TestMethod]
        public void GetTailTest()
        {
            string telFile = "D:\\School Work\\Year 3\\Advance Software Quality\\ASQL_FDMS\\FDMS_Aircraft_Transmission_System\\C-FGAX.txt";

            // regex to match the aircraft tail from the filename
            Regex aircraftTailRegex = new Regex("(C-.*)[^.txt]");
            var aircraftMatch = aircraftTailRegex.Match(telFile);
            // contains the aircraft tail 
            string aircraftTail = aircraftMatch.Groups[0].ToString();

            Assert.IsTrue(aircraftTail == "C-FGAX", "TestGetTail has Failed");
        }
        
        
        
        [TestMethod]
        public void CheckChecksumTest()
        {
            Telemetry testTel = new Telemetry("C_FGAX", -0.319754, -0.716176, 1.797150, 2154.670410, 1643.844116, 0.022278, 0.033622, DateTime.Now);
            int testChecksum = Packet.calculateCheckSum(testTel);

            Assert.AreEqual(testChecksum, 547);
        }

        

        [TestMethod]
        public void testOpenFileSuccess()
        {
            string filePath = "C:\\tmp\\C-FGAX.txt";

            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        public void testOpenFileFailure()
        {
            string filePath = "C:\\tmp\\Non-Existent.txt";

            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }



        [TestMethod]
        public void CheckProcessLineTest()
        {
            string line = " 7_8_2018 19:34:3,-0.319754, -0.716176, 1.797150, 2154.670410, 1643.844116, 0.022278, 0.033622,";
            string tail = "C-FGAX";
            string testDate = " 7_8_2018 19:34:3";

            Telemetry successTData = new Telemetry("C-FGAX", -0.319754, -0.716176, 1.797150, 2154.670410, 1643.844116, 0.022278, 0.033622, Convert.ToDateTime(testDate.Trim().Replace('_', '-')));

            if (line.Contains('\0'))
            {
                Assert.Fail("Null Present");
            }
            string[] aircraftData = line.Split(',');
            try
            {
                Telemetry tData = new Telemetry(tail, Double.Parse(aircraftData[1]), Double.Parse(aircraftData[2]), Double.Parse(aircraftData[3]), Double.Parse(aircraftData[4]),
                    Double.Parse(aircraftData[5]), Double.Parse(aircraftData[6]), Double.Parse(aircraftData[7]), Convert.ToDateTime(aircraftData[0].Trim().Replace('_', '-')));

                Assert.AreEqual(tData.TailNum, successTData.TailNum);
                Assert.AreEqual(tData.Accel_x, successTData.Accel_x);
                Assert.AreEqual(tData.Accel_y, successTData.Accel_y);
                Assert.AreEqual(tData.Accel_z, successTData.Accel_z);
                Assert.AreEqual(tData.Altitude, successTData.Altitude);
                Assert.AreEqual(tData.Bank, successTData.Bank);
                Assert.AreEqual(tData.Pitch, successTData.Pitch);
                Assert.AreEqual(tData.TimeStamp, successTData.TimeStamp);
            }
            catch
            {
                Assert.Fail("Failed Try");
            }
        }
    }
}
