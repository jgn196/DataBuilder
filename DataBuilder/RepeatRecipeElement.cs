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
    /// A build "recipe" element that repeats another build "recipe" a fixed number of times.
    /// </summary>
    internal class RepeatRecipeElement : IRecipeElement
    {
        private DataBuilder pattern;
        private uint count;

        /// <summary>
        /// Creates a RepeatRecipeElement.
        /// </summary>
        /// <param name="count">The number of times the pattern should be repeated.</param>
        /// <param name="pattern">The pattern to repeat.</param>
        public RepeatRecipeElement(uint count, DataBuilder pattern)
        {
            Condition.Ensures(pattern).IsNotNull();

            this.pattern = pattern;
            this.count = count;
        }

        /// <inheritdoc/>
        public byte[] Build()
        {
            IEnumerable<byte> result = new byte[0];

            for (int i = 0; i < count; i++)
            {
                result = result.Concat(pattern.Build());
            }

            return result.ToArray();
        }

        /// <summary>
        /// Overrides to show the count and internal pattern.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return new ToStringBuilder(this)
                .Append("count", count)
                .Append("pattern", pattern)
                .ToString();
        }

        /// <summary>
        /// Overrides the method to compare counts and repeat patterns.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            RepeatRecipeElement other = obj as RepeatRecipeElement;

            if (other == null)
            {
                return false;
            }
            else
            {
                return new EqualsBuilder()
                    .Append(count, other.count)
                    .Append(pattern, other.pattern)
                    .IsEquals;
            }
        }

        /// <summary>
        /// Overrides the method to include the count and repeat pattern in the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(count)
                .Append(pattern)
                .GetHashCode();
        }
    }
}
