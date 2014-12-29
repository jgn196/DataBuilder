using Capgemini.DataBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBuilderTests
{
    /// <summary>
    /// Tests the DataBuilder class.
    /// </summary>
    [TestClass]
    public class DataBuilderTests
    {
        /// <summary>
        /// Tests building an empty (zero length) data set.
        /// </summary>
        [TestMethod]
        public void BuildEmptyData()
        {
            Assert.AreEqual(0, new DataBuilder().Build().Length);
        }
    }
}
