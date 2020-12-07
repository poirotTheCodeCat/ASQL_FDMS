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
        
        
        
    }
}
