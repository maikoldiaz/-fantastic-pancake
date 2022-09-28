// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebExtensions.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using ChaosConstants = Ecp.True.Chaos.Constants;

    /// <summary>
    /// The web extensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class WebExtensions
    {
        /// <summary>
        /// Builds the error result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The action result.</returns>
        public static IActionResult BuildErrorResult(this HttpContext context, TrueWebException exception)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(exception, nameof(exception));

            if (exception.StatusCode.HasValue && string.IsNullOrWhiteSpace(exception.Message))
            {
                return context.BuildErrorResult((int)exception.StatusCode.Value);
            }

            if (exception.StatusCode.HasValue && !string.IsNullOrWhiteSpace(exception.Message))
            {
                return context.BuildErrorResult(exception.Message, (int)exception.StatusCode.Value);
            }

            return context.BuildErrorResult(exception.Message);
        }

        /// <summary>
        /// Builds the error result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="errorList">The errorList.</param>
        /// <returns>The action result.</returns>
        public static IActionResult BuildErrorResult(this HttpContext context, IEnumerable<ErrorResponse> errorList)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(errorList, nameof(errorList));

            return errorList.First().ErrorCodes.First().Code.EqualsIgnoreCase(True.Core.Constants.PositionsErrorCode)
                ? new ObjectResult(errorList)
                {
                    StatusCode = 413,
                }
                : new ObjectResult(errorList)
                {
                    StatusCode = 400,
                };
        }

        /// <summary>
        /// Builds the result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The error.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public static IActionResult BuildErrorResult(this HttpContext context, string error)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            if (string.IsNullOrWhiteSpace(error))
            {
                if (context.IsGet())
                {
                    return new NotFoundResult();
                }

                return new BadRequestResult();
            }

            var response = new ErrorResponse(error);
            if (context.IsGet())
            {
                return new NotFoundObjectResult(response);
            }

            if (error.Contains(SapConstants.MoreThanMaxLimitPositionsCode, StringComparison.Ordinal))
            {
                return new ObjectResult(response)
                {
                    StatusCode = 413,
                };
            }

            return new BadRequestObjectResult(response);
        }

        /// <summary>
        /// Builds the error result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The error.</param>
        /// <param name="code">The code.</param>
        /// <returns>The action result.</returns>
        public static IActionResult BuildErrorResult(this HttpContext context, string error, int code)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var response = new ErrorResponse(error);
            return new ObjectResult(response)
            {
                StatusCode = code,
            };
        }

        /// <summary>
        /// Builds the error result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="code">The code.</param>
        /// <returns>The action result.</returns>
        public static IActionResult BuildErrorResult(this HttpContext context, int code)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            return new StatusCodeResult(code);
        }

        /// <summary>
        /// Builds the result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public static IActionResult BuildErrorResult(this HttpContext context, IList<ErrorInfo> errors)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(errors, nameof(errors));

            var response = new ErrorResponse(errors);
            if (context.IsGet())
            {
                return new NotFoundObjectResult(response);
            }

            return new BadRequestObjectResult(response);
        }

        /// <summary>
        /// Builds the conflict error.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The error code.</param>
        /// <returns>The result.</returns>
        public static IActionResult BuildConflictError(this HttpContext context, string error)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            var response = new ErrorResponse(error);
            return new ConflictObjectResult(response);
        }

        /// <summary>
        /// Builds the chaos error.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The error code.</param>
        /// <returns>The result.</returns>
        public static IActionResult BuildChaosError(this HttpContext context, string error)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            var response = new ErrorResponse(error);
            return new BadRequestObjectResult(response);
        }

        /// <summary>
        /// Throws if error response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="throwOnNoSuccessStatus">if set to <c>true</c> [throw on not success status].</param>
        /// <returns>The Task.</returns>
        public static async Task ThrowIfErrorAsync(this HttpResponseMessage response, bool throwOnNoSuccessStatus)
        {
            ArgumentValidators.ThrowIfNull(response, nameof(response));

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException();
            }

            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseMessage = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorMessage = GetErrorMessage(responseMessage);
                var code = Convert.ToString((int)Enum.Parse(typeof(HttpStatusCode), response.StatusCode.ToString()), CultureInfo.InvariantCulture);
                throw new TrueWebException(errorMessage, code);
            }

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new TrueWebException(HttpStatusCode.Conflict);
            }

            if (!response.IsSuccessStatusCode && throwOnNoSuccessStatus)
            {
                throw new WebException();
            }
        }

        /// <summary>
        /// Determines whether this instance is post.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is post; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPost(this HttpContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return context.Request.Method == "POST";
        }

        /// <summary>
        /// Determines whether this instance is put.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is post; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPut(this HttpContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return context.Request.Method == "PUT";
        }

        /// <summary>
        /// Determines whether this instance is get.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is post; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGet(this HttpContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return context.Request.Method == "GET";
        }

        /// <summary>
        /// Determines whether this instance is delete.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is delete; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDelete(this HttpContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            return context.Request.Method == "DELETE";
        }

        /// <summary>
        /// Gets the chaos header value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The chaos value.</returns>
        public static string GetChaosHeaderValue(this HttpContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            if (context.Request.Headers.ContainsKey(ChaosConstants.ChaosHeaderName))
            {
                return context.Request.Headers[ChaosConstants.ChaosHeaderName];
            }

            return null;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        /// <returns>The response.</returns>
        private static string GetErrorMessage(string responseMessage)
        {
            try
            {
                return JsonConvert.DeserializeObject<ErrorResponse>(responseMessage)?.ErrorCodes.First(x => x != null)?.Message;
            }
            catch
            {
                return JsonConvert.DeserializeObject<ErrorInfo>(responseMessage).Message;
            }
        }
    }
}
