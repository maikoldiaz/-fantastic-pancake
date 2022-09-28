// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionResolver.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core
{
    using System;
    using Ecp.True.Ioc.Interfaces;

    /// <summary>
    /// The resolver for function.
    /// </summary>
    /// <seealso cref="Ecp.True.Ioc.Interfaces.IResolver" />
    public class FunctionResolver : IResolver
    {
        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionResolver"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public FunctionResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public T GetInstance<T>()
        {
            return (T)this.serviceProvider.GetService(typeof(T));
        }
    }
}
