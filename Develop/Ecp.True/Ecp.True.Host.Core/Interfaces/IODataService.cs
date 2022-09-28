// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IODataService.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.OData
{
    using Microsoft.AspNet.OData.Query;
    using Microsoft.AspNetCore.Http;
    using Microsoft.OData.Edm;

    /// <summary>
    /// The OData Service class.
    /// </summary>
    public interface IODataService
    {
        /// <summary>
        /// Gets the o data query options.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TParent">The type of the parent.</typeparam>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="model">The model.</param>
        /// <param name="keyValue">The key value.</param>
        /// <returns>OData query options.</returns>
        ODataQueryOptions<TResult> GetODataQueryOptions<TResult, TParent, TChild>(HttpRequest request, IEdmModel model, int keyValue);

        /// <summary>
        /// Gets the o data query options.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="model">The model.</param>
        /// <param name="keyValue">The key value.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>OData query options.</returns>
        ODataQueryOptions<TResult> GetODataQueryOptions<TResult, TChild>(HttpRequest request, IEdmModel model, int keyValue, string parentName);
    }
}
