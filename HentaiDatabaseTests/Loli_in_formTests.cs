using Microsoft.VisualStudio.TestTools.UnitTesting;
using BentaiDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BentaiDataBase.Tests
{
    [TestClass()]
    public class PopulateDataBaseTest
    {
        [TestMethod()]
        public void PopulateDataBase_LoadNewPic_PicExists()
        {
            PopulateDataBase populateDataBase = new PopulateDataBase(0);
        }

        [TestMethod()]
        public void EmptyPanelTest()
        {
            Assert.Fail();
        }
    }
}