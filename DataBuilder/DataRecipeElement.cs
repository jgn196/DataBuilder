using Capgemini.CommonObjectUtils;
using CuttingEdge.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.DataBuilder
{
    /// <summary>
    /// A build "recipe" element that holds a fixed set of bytes.
    /// </summary>
    internal class DataRecipeElement : IRecipeElement
    {
        private byte[] data;

        /// <summary>
        /// Creates a DataRecipeElement.
        /// </summary>
        /// <param name="data">The data to keep a copy of.</param>
        public DataRecipeElement(byte[] data)
        {
            Condition.Ensures(data).IsNotNull();

            this.data = data.Clone() as byte[];
        }

        /// <inheritdoc/>
        public byte[] Build()
        {
            return data;
        }

        /// <summary>
        /// Overrides the method to give the contents of the data.
        /// </summary>
        /// <returns>The String representation.</returns>
        public override string ToString()
        {
            return new ToStringBuilder(this)
                .AppendMany("data", data)
                .ToString();
        }

        /// <summary>
        /// Overrides the method to check the data is equal.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            DataRecipeElement other = obj as DataRecipeElement;

            if (other == null)
            {
                return false;
            }
            else
            {
                return new EqualsBuilder()
                    .AppendMany(data, other.data)
                    .IsEquals;
            }
        }

        /// <summary>
        /// Overrides the method to use the data in the hash code calculation.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(data)
                .GetHashCode();
        }
    }
}
