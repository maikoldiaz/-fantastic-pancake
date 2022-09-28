// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayBase.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc.Tests.Types
{
    using System.Collections.Generic;

    using Ecp.True.Ioc.Tests.Types.Core;

    /// <summary>
    /// The array base.
    /// </summary>
    /// <seealso cref="Ecp.True.Ioc.Tests.Types.Core.IArrayBase" />
    public class ArrayBase : IArrayBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBase" /> class.
        /// </summary>
        /// <param name="components">The components.</param>
        public ArrayBase(IEnumerable<IBase> components)
        {
            this.Components = components;
        }

        /// <summary>
        /// Gets the components.
        /// </summary>
        /// <value>
        /// The components.
        /// </value>
        public IEnumerable<IBase> Components { get; }
    }
}