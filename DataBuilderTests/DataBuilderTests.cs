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

        /// <summary>
        /// Tests that you can use the DataBuilder to copy a byte array.
        /// </summary>
        [TestMethod]
        public void CopyData()
        {
            byte[] source = new byte[] { 1, 2, 3 };

            byte[] result = new DataBuilder().Append(source).Build();

            Assert.AreNotSame(source, result);
            Assert.IsTrue(Enumerable.SequenceEqual(source, result));
        }

        /// <summary>
        /// Tests that you can use the DataBuilder to concatenate byte arrays together.
        /// </summary>
        [TestMethod]
        public void ConcatenateData()
        {
            byte[] source1 = new byte[] { 1, 2, 3 };
            byte[] source2 = new byte[] { 4, 5, 6 };

            byte[] result = new DataBuilder().Append(source1).Append(source2).Build();

            Assert.AreEqual(6, result.Length);
            Assert.IsTrue(Enumerable.Range(1, 6).SequenceEqual(result.Select(b => (int)b)));
        }
    }
}
