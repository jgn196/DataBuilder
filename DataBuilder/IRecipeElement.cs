using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capgemini.DataBuilder
{
    /// <summary>
    /// The interface that all build "recipe" elements must implement.
    /// </summary>
    internal interface IRecipeElement
    {
        /// <summary>
        /// Builds this "recipe" element.
        /// </summary>
        /// <returns>The result of building the element.</returns>
        byte[] Build();
    }
}
