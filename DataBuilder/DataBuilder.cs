using Capgemini.CommonObjectUtils;
using CuttingEdge.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capgemini.DataBuilder
{
    /// <summary>
    /// A utility class for making writing small binary data samples easy.
    /// 
    /// Clients instantiate an instance of the builder, call various methods in the Fluent style to configure 
    /// how data will be generated and then call the Build method to build the data.
    /// The order in which methods are called is the order in which their arguments will be added to the 
    /// finished built data.
    /// </summary>
    public sealed class DataBuilder : IEquatable<DataBuilder>
    {
        private IList<IRecipeElement> recipeElements = new List<IRecipeElement>();
        private Encoding encoding = ASCIIEncoding.ASCII;
        private ByteOrder endianness = ByteOrder.LittleEndian;

        /// <summary>
        /// Sets the encoding to use when appending text strings.
        /// Any text strings that have already been appended are unaffected.
        /// </summary>
        /// <param name="encoding">The new text encoding.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder SetEncoding(Encoding encoding)
        {
            Condition.Ensures(encoding).IsNotNull();

            this.encoding = encoding;

            return this;
        }

        /// <summary>
        /// Sets the byte order (endianness) to use when appending integers.
        /// Any integers that have already been appeneded are unaffected.
        /// </summary>
        /// <param name="endianness">The new byte order.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder SetByteOrder(ByteOrder endianness)
        {
            this.endianness = endianness;

            return this;
        }

        /// <summary>
        /// Adds an arbitary chunk of binary data to the build "recipe".
        /// </summary>
        /// <param name="value">The data that should be copied into the recipe.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendBytes(byte[] value)
        {
            Condition.Ensures(value).IsNotNull();

            recipeElements.Add(new DataRecipeElement(value));

            return this;
        }

        /// <summary>
        /// Adds a string to the build "recipe".
        /// The string will not be null terminated.
        /// 
        /// If the SetEncoding method has not been called, encoding defaults to ASCII.
        /// </summary>
        /// <param name="value">The string to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendText(string value)
        {
            Condition.Ensures(value).IsNotNull();

            recipeElements.Add(new DataRecipeElement(encoding.GetBytes(value)));

            return this;
        }

        /// <summary>
        /// Adds a signed byte to the build "recipe".
        /// </summary>
        /// <param name="value">The byte to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendSByte(sbyte value)
        {
            recipeElements.Add(new DataRecipeElement(new byte[] { (byte)value }));

            return this;
        }

        /// <summary>
        /// Adds a byte to the build "recipe".
        /// </summary>
        /// <param name="value">The byte to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendByte(byte value)
        {
            recipeElements.Add(new DataRecipeElement(new byte[] { value }));

            return this;
        }

        /// <summary>
        /// Adds a 16-bit signed integer to the build "recipe".
        /// 
        /// If the SetByteOrder method has not been called, little endian is used by default.
        /// </summary>
        /// <param name="value">The integer to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendInt16(short value)
        {
            recipeElements.Add(new DataRecipeElement(CorrectByteOrder(BitConverter.GetBytes(value))));

            return this;
        }

        /// <summary>
        /// Adds an unsigned 16-bit integer to the build "recipe".
        /// 
        /// If the SetByteOrder method has not been called, little endian is used by default.
        /// </summary>
        /// <param name="value">The integer to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendUInt16(ushort value)
        {
            recipeElements.Add(new DataRecipeElement(CorrectByteOrder(BitConverter.GetBytes(value))));

            return this;
        }

        /// <summary>
        /// Adds a 32-bit signed integer to the build "recipe".
        /// 
        /// If the SetByteOrder method has not been called, little endian is used by default.
        /// </summary>
        /// <param name="value">The integer to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendInt32(int value)
        {
            recipeElements.Add(new DataRecipeElement(CorrectByteOrder(BitConverter.GetBytes(value))));

            return this;
        }

        /// <summary>
        /// Adds an unsigned 32-bit integer to the build "recipe".
        /// 
        /// If the SetByteOrder method has not been called, little endian is used by default.
        /// </summary>
        /// <param name="value">The integer to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendUInt32(uint value)
        {
            recipeElements.Add(new DataRecipeElement(CorrectByteOrder(BitConverter.GetBytes(value))));

            return this;
        }

        /// <summary>
        /// Adds a 64-bit signed integer to the build "recipe".
        /// 
        /// If the SetByteOrder method has not been called, little endian is used by default.
        /// </summary>
        /// <param name="value">The integer to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendInt64(long value)
        {
            recipeElements.Add(new DataRecipeElement(CorrectByteOrder(BitConverter.GetBytes(value))));
            return this;
        }

        /// <summary>
        /// Adds an unsigned 64-bit integer to the build "recipe".
        /// 
        /// If the SetByteOrder method has not been called, little endian is used by default.
        /// </summary>
        /// <param name="value">The integer to add.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder AppendUInt64(ulong value)
        {
            recipeElements.Add(new DataRecipeElement(CorrectByteOrder(BitConverter.GetBytes(value))));

            return this;
        }

        /// <summary>
        /// Adds the result of a secondary DataBuilder as a repeating pattern.
        /// 
        /// Passing a DataBuilder itself as the repeating pattern is illegal.
        /// </summary>
        /// <param name="count">The number of times the pattern must repeat.</param>
        /// <param name="pattern">The DataBuilder to get the pattern from.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder Repeat(uint count, DataBuilder pattern)
        {
            Condition.Ensures(pattern).IsNotNull().Evaluate(pattern != this);

            recipeElements.Add(new RepeatRecipeElement(count, pattern));

            return this;
        }

        /// <summary>
        /// Builds the data according to the "recipe" that has been built up by calling methods on this object.
        /// </summary>
        /// <returns>The new data.</returns>
        public byte[] Build()
        {
            IEnumerable<byte> result = new byte[0];

            foreach (IRecipeElement element in recipeElements)
            {
                result = result.Concat(element.Build());
            }

            return result.ToArray();
        }

        /// <summary>
        /// Overrides the Equals method to compare all "recipe" elements.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the objects are equal.</returns>
        public override bool Equals(object obj)
        {
            DataBuilder other = obj as DataBuilder;

            if (other == null)
            {
                return false;
            }
            else
            {
                return Equals(other);
            }
        }

        /// <summary>
        /// Compares another DataBuilder object.
        /// </summary>
        /// <param name="other">The DataBuilder to compare to.</param>
        /// <returns>True if the builders are equal.</returns>
        public bool Equals(DataBuilder other)
        {
            if (recipeElements.Count != other.recipeElements.Count)
            {
                return false;
            }

            for (int i = 0; i < recipeElements.Count; i++)
            {
                if (!recipeElements[i].Equals(other.recipeElements[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Overrides the GetHashCode method to take all the "recipe" elements into account.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            HashCodeBuilder builder = new HashCodeBuilder();

            foreach (IRecipeElement element in recipeElements)
            {
                builder.Append(element);
            }

            return builder.GetHashCode();
        }

        /// <summary>
        /// Overrides the ToString method to return the class name and a list of the builder's "recipe" elements.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new ToStringBuilder(this)
                .AppendMany("recipeElements", recipeElements)
                .ToString();
        }

        private byte[] CorrectByteOrder(byte[] value)
        {
            ByteOrder systemByteOrder = BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

            if (endianness != systemByteOrder)
            {
                Array.Reverse(value);
            }

            return value;
        }
    }
}
