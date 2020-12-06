using Microsoft.VisualStudio.TestTools.UnitTesting;
using dg_sm_jd_em_FDMS;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string dbConStr = "Data Source=SASHAMALESEFDB0;Initial Catalog=GroundTerminal;Integrated Security=True;";
            string search = "CFJAX";
            List<Telemetry> telSearch = SqlDataAccess.getRecords(search, dbConStr);

            Assert.AreEqual(telSearch.Count, 0);
        }
    }
}
