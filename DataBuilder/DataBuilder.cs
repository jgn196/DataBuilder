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
    public class DataBuilder
    {
        private IList<byte[]> recipeElements = new List<byte[]>();
        private Encoding encoding = ASCIIEncoding.ASCII;

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
        /// Adds an arbitary chunk of binary data to the build "recipe".
        /// </summary>
        /// <param name="value">The data that should be copied into the recipe.</param>
        /// <returns>This DataBuilder for chaining calls.</returns>
        public DataBuilder Append(byte[] value)
        {
            Condition.Ensures(value).IsNotNull();

            recipeElements.Add((byte[]) value.Clone());

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
        public DataBuilder Append(string value)
        {
            Condition.Ensures(value).IsNotNull();

            recipeElements.Add(encoding.GetBytes(value));
            
            return this;
        }

        /// <summary>
        /// Builds the data according to the "recipe" that has been built up by calling methods on this object.
        /// </summary>
        /// <returns>The new data.</returns>
        public byte[] Build()
        {
            IEnumerable<byte> result = new byte[0];

            foreach (byte[] element in recipeElements)
            {
                result = result.Concat(element);
            }

            return result.ToArray();
        }
    }
}
