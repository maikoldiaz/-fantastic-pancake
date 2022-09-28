// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Factory.cs" company="Microsoft">
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
    using Ecp.True.Ioc.Tests.Types.Core;

    /// <summary>
    /// The base factory.
    /// </summary>
    /// <seealso cref="Ecp.True.Ioc.Tests.Types.Core.IFactory" />
    public class Factory : IFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        /// <param name="specialized">The specialized.</param>
        /// <param name="generic">The generic.</param>
        /// <param name="singleton">The singleton.</param>
        /// <param name="hierarichal">The request scope.</param>
        public Factory(IAnother specialized, IRepo<string> generic, ISingleton singleton, IHierarichal hierarichal)
        {
            this.Specialized = specialized;
            this.Generic = generic;
            this.Singleton = singleton;
            this.Hierarichal = hierarichal;
        }

        /// <summary>
        /// Gets the generic.
        /// </summary>
        /// <value>
        /// The generic.
        /// </value>
        public IRepo<string> Generic { get; }

        /// <summary>
        /// Gets the child scope.
        /// </summary>
        /// <value>
        /// The child scope.
        /// </value>
        public IHierarichal Hierarichal { get; }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <value>
        /// The singleton.
        /// </value>
        public ISingleton Singleton { get; }

        /// <summary>
        /// Gets the specialized.
        /// </summary>
        /// <value>
        /// The specialized.
        /// </value>
        public IAnother Specialized { get; }

        /// <inheritdoc />
        /// <summary>
        /// Tests this instance.
        /// </summary>
        public void Test()
        {
            // Method intentionally left empty.
        }
    }
}