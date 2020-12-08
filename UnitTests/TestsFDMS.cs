using Microsoft.VisualStudio.TestTools.UnitTesting;
using dg_sm_jd_em_FDMS;
using System.Collections.Generic;
using System;

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
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void InsertRecord()
        {
            DateTime currTime = System.DateTime.Now;
            string dbConStr = "Data Source=SASHAMALESEFDB0;Initial Catalog=GroundTerminal;Integrated Security=True;";
            Telemetry t = new Telemetry("TAIL", 0, 0, 0, 0, 0, 0, 0, currTime);

            List<Telemetry> ts = SqlDataAccess.getRecords("TAIL", dbConStr);

        }

        [TestMethod]
        public void SearchQueryTelemetryExists()
        {
            string dbConStr = "Data Source=SASHAMALESEFDB0;Initial Catalog=GroundTerminal;Integrated Security=True;";
            string tailNum = "TAIL";
            List<Telemetry> ts = SqlDataAccess.getRecords(tailNum, dbConStr);
            if(ts.Count == 0 )
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void SearchQueryTelemetryDoesNotExist()
        {
            string dbConStr = "Data Source=SASHAMALESEFDB0;Initial Catalog=GroundTerminal;Integrated Security=True;";
            string tailNum = "RAND";
            List<Telemetry> ts = SqlDataAccess.getRecords(tailNum, dbConStr);
            if (ts.Count > 0)
            {
                Assert.Fail();
            }

        }
    }
}
