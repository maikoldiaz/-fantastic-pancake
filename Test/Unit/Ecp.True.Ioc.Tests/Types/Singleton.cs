// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Singleton.cs" company="Microsoft">
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
    using Ecp.True.Core.Attributes;
    using Ecp.True.Ioc.Tests.Types.Core;

    /// <summary>
    /// The singleton type.
    /// </summary>
    /// <seealso cref="Ecp.True.Ioc.Tests.Types.Core.TestBase" />
    /// <seealso cref="Ecp.True.Ioc.Tests.Types.Core.ISingleton" />
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class Singleton : TestBase, ISingleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Singleton" /> class.
        /// </summary>
        public Singleton()
        {
            this.State = Randomizer.GetNext();
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State { get; }
    }
}