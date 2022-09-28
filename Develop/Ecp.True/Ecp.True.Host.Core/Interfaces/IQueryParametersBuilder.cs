// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryParametersBuilder.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Interfaces
{
    /// <summary>
    /// The query parameter builder.
    /// </summary>
    public interface IQueryParametersBuilder
    {
        /// <summary>
        /// Withes the Or parameter.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Withes the or parameter as <see cref="IQueryParametersBuilder" />.
        /// </returns>
        IQueryParametersBuilder AndODataFilterParameter<T>(string key, string operation, T value);

        /// <summary>
        /// Builds the OData Filter query.
        /// </summary>
        /// <returns>
        /// Builds the o data query as <see cref="string" />.
        /// </returns>
        string BuildODataFilterQuery();

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>
        /// The parameters.
        /// </returns>
        string BuildQuery();

        /// <summary>
        /// Ors the specified value.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Ors the o data filter parameter as <see cref="IQueryParametersBuilder" />.
        /// </returns>
        IQueryParametersBuilder OrODataFilterParameter<T>(string key, string operation, T value);

        /// <summary>
        /// Withes the And parameter.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Withes the and parameter as <see cref="IQueryParametersBuilder" />.
        /// </returns>
        IQueryParametersBuilder WithODataFilterParameter<T>(string key, string operation, T value);

        /// <summary>
        /// Withes the parameter.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The builder.
        /// </returns>
        IQueryParametersBuilder WithQueryParameter<T>(string key, T value);

        /// <summary>
        /// Appends the query parameter.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The regenerated query string.</returns>
        string AppendQueryParameter(string queryString, string key, string value);
    }
}