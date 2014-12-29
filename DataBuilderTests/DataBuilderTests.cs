using Capgemini.DataBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            AssertEquals(source, result);
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

        /// <summary>
        /// Tests building strings with different encodings.
        /// </summary>
        [TestMethod]
        public void BuildStrings()
        {
            string testString = "Foo€";
            IDictionary<Encoding, byte[]> expectedResults = new Dictionary<Encoding, byte[]>() 
            { 
                {ASCIIEncoding.ASCII, new byte[]{0x46, 0x6F, 0x6F, 0x3F}},
                {UnicodeEncoding.UTF7, new byte[]{0x46, 0x6F, 0x6F, 0x2B, 0x49, 0x4B, 0x77, 0x2D}},
                {UnicodeEncoding.UTF8, new byte[]{0x46, 0x6F, 0x6F, 0xE2, 0x82, 0xAC}},
                // Little endian UTF-16
                {UnicodeEncoding.Unicode, new byte[]{0x46, 0x00, 0x6F, 0x00, 0x6F, 0x00, 0xAC, 0x20}},
                // Big endian UTF-16
                {UnicodeEncoding.BigEndianUnicode, new byte[]{0x00, 0x46, 0x00, 0x6F, 0x00, 0x6F, 0x20, 0xAC}},
                // Little endian
                {UnicodeEncoding.UTF32, new byte[]{0x46, 0x00, 0x00, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x6F, 0x00, 0x00, 0x00, 0xAC, 0x20, 0x00, 0x00}},
            };

            foreach (Encoding encoding in expectedResults.Keys)
            {
                byte[] result = new DataBuilder().SetEncoding(encoding).Append(testString).Build();
                AssertEquals(expectedResults[encoding], result);
            }
        }

        /// <summary>
        /// Tests building signed little endian integer data sets.
        /// </summary>
        [TestMethod]
        public void BuildSignedLittleEndianIntegers()
        {
            byte[] result = new DataBuilder()
                .SetByteOrder(DataBuilder.ByteOrder.LittleEndian)
                .Append((sbyte)-1)
                .Append((short)-2)
                .Append(-3)
                .Append(-4L)
                .Build();

            AssertEquals(
                new byte[] { 0xFF, 0xFE, 0xFF, 0xFD, 0xFF, 0xFF, 0xFF, 0xFC, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF },
                result);
        }

        /// <summary>
        /// Tests building unsigned little endian integer data sets.
        /// </summary>
        [TestMethod]
        public void BuildUnsignedLittleEndianIntegers()
        {
            byte[] result = new DataBuilder()
                .SetByteOrder(DataBuilder.ByteOrder.LittleEndian)
                .Append((byte)1)
                .Append((ushort)2)
                .Append((uint)3)
                .Append((ulong)4)
                .Build();

            AssertEquals(
                new byte[] { 0x01, 0x02, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                result);
        }

        /// <summary>
        /// Tests building signed little endian integer data sets.
        /// </summary>
        [TestMethod]
        public void BuildSignedBigEndianIntegers()
        {
            byte[] result = new DataBuilder()
                .SetByteOrder(DataBuilder.ByteOrder.BigEndian)
                .Append((sbyte)-1)
                .Append((short)-2)
                .Append(-3)
                .Append(-4L)
                .Build();

            AssertEquals(
                new byte[] { 0xFF, 0xFF, 0xFE, 0xFF, 0xFF, 0xFF, 0xFD, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFC },
                result);
        }

        /// <summary>
        /// Tests building unsigned big endian integers.
        /// </summary>
        [TestMethod]
        public void BuildUnsignedBigEndianIntegers()
        {
            byte[] result = new DataBuilder()
                .SetByteOrder(DataBuilder.ByteOrder.BigEndian)
                .Append((byte)1)
                .Append((ushort)2)
                .Append((uint)3)
                .Append((ulong)4)
                .Build();

            AssertEquals(
                new byte[] { 0x01, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04 },
                result);
        }

        private void AssertEquals(byte[] expected, byte[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length, "Array lengths are not equal.");

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "Elements at index " + i + " are not equal.");
            }
        }
    }
}
