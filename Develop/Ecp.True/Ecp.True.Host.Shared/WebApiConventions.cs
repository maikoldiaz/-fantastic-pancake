// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConventions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Shared
{
    using System.Diagnostics.CodeAnalysis;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;

    /// <summary>
    /// The web api conventions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WebApiConventions
    {
        /// <summary>
        /// Defines the conventions for API controller action which begins with word Create.
        /// </summary>
        /// <param name="model">The model.</param>
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Create([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]object model)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Save.
        /// </summary>
        /// <param name="model">The model.</param>
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Save([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)] object model)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Update.
        /// </summary>
        /// <param name="model">The model.</param>
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Update([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]object model)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Delete.
        /// </summary>
        /// <param name="model">The model.</param>
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Delete([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]int model)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Delete.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="another">Another.</param>
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Delete(
            [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]int model,
            [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]int another)
        {
            ArgumentValidators.ThrowIfNull(model, nameof(model));
            ArgumentValidators.ThrowIfNull(another, nameof(another));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Exists.
        /// </summary>
        /// <param name="model">The model.</param>
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Exists([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]string model)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(model, nameof(model));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Get and Has ID parameters.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Get([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)]int id)
        {
            ArgumentValidators.ThrowIfNull(id, nameof(id));
        }

        /// <summary>
        /// Defines the conventions for API controller action which begins with word Get and Has ID parameters.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="other">Another.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Get(
            [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)]int id,
            [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]int other)
        {
            ArgumentValidators.ThrowIfNull(id, nameof(id));
            ArgumentValidators.ThrowIfNull(other, nameof(other));
        }
    }
}
