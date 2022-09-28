// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovementBootstrapper.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Balance.Bootstrap
{
    using Ecp.True.Host.Functions.Core.Bootstrap;
    using Ecp.True.Processors.Balance.Operation;
    using Ecp.True.Processors.Balance.Operation.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The balance calculation registration entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.Functions.Core.Bootstrap.BootstrapperBase" />
    public class MovementBootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovementBootstrapper"/> class.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public MovementBootstrapper(IServiceCollection serviceCollection)
            : base(serviceCollection)
        {
        }

        /// <summary>
        /// Bootstraps this instance.
        /// </summary>
        public void Bootstrap()
        {
            this.RegisterTransient<IInterfaceMovementGenerator, InterfaceMovementGenerator>();
            this.RegisterTransient<IBalanceToleranceMovementGenerator, BalanceToleranceMovementGenerator>();
            this.RegisterTransient<IUnidentifiedLossMovementGenerator, UnidentifiedLossMovementGenerator>();
        }
    }
}
