// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// The request culture provider.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Localization.IRequestCultureProvider" />
    public class CultureProvider : IRequestCultureProvider
    {
        /// <summary>
        /// The english.
        /// </summary>
        private readonly string english;

        /// <summary>
        /// The spanish.
        /// </summary>
        private readonly string spanish;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureProvider"/> class.
        /// </summary>
        /// <param name="english">The english.</param>
        /// <param name="spanish">The spanish.</param>
        public CultureProvider(string english, string spanish)
        {
            this.english = english;
            this.spanish = spanish;
        }

        /// <inheritdoc/>
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            ArgumentValidators.ThrowIfNull(httpContext, nameof(httpContext));

            var headers = httpContext.Request.Headers;
            if (headers.TryGetValue("culture", out StringValues values) && values.Any(v => v.EqualsIgnoreCase(this.english)))
            {
                return Task.FromResult(new ProviderCultureResult(this.english));
            }

            return Task.FromResult(new ProviderCultureResult(this.spanish));
        }
    }
}
