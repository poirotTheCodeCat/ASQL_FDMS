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
    }
}
