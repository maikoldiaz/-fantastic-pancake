// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebAppResolver.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using Ecp.True.Ioc.Interfaces;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The resolver for web apps.
    /// </summary>
    /// <seealso cref="Ecp.True.Ioc.Interfaces.IResolver" />
    public class WebAppResolver : IResolver
    {
        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebAppResolver"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public WebAppResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public T GetInstance<T>()
        {
            return (T)this.httpContextAccessor.HttpContext.RequestServices.GetService(typeof(T));
        }
    }
}
