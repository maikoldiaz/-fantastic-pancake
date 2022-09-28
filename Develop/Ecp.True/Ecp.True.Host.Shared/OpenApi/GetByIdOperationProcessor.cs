// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetByIdOperationProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Shared.OpenApi
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Http;
    using NSwag;
    using NSwag.Generation.Processors;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The exists operation processor.
    /// </summary>
    /// <seealso cref="NSwag.Generation.Processors.IOperationProcessor" />
    public class GetByIdOperationProcessor : IOperationProcessor
    {
        /// <summary>
        /// The summary.
        /// </summary>
        private readonly string summary = "Gets the {0} by {1}";

        /// <summary>
        /// The status200 message.
        /// </summary>
        private readonly string status200Message = "The {0} is found.";

        /// <summary>
        /// The status404 message.
        /// </summary>
        private readonly string status404Message = "The {0} is not found.";

        /// <summary>
        /// The status500 message.
        /// </summary>
        private readonly string status500Message = "Unknown error while getting {0}.";

        /// <inheritdoc/>
        public bool Process(OperationProcessorContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            // if method name starts with word Get and has word By
            if (context.MethodInfo.Name.StartsWith("Get", StringComparison.OrdinalIgnoreCase)
                && context.MethodInfo.Name.Contains("By", StringComparison.OrdinalIgnoreCase))
            {
                var operation = context.OperationDescription.Operation;
                var entityName = GetEntityName(operation.OperationId);

                operation.Summary = this.BuildSummary(entityName, context.MethodInfo);

                OpenApiResponse response;
                if (operation.Responses.TryGetValue(StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture), out response))
                {
                    response.Description = string.Format(CultureInfo.InvariantCulture, this.status200Message, entityName);
                }

                if (operation.Responses.TryGetValue(StatusCodes.Status404NotFound.ToString(CultureInfo.InvariantCulture), out response))
                {
                    response.Description = string.Format(CultureInfo.InvariantCulture, this.status404Message, entityName);
                }

                if (operation.Responses.TryGetValue(StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture), out response))
                {
                    response.Description = string.Format(CultureInfo.InvariantCulture, this.status500Message, entityName);
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <param name="operationId">The operation identifier.</param>
        /// <returns>
        /// The entity name.
        /// </returns>
        private static string GetEntityName(string operationId)
        {
            var methodName = operationId.Split('_')[1];
            methodName = methodName
                            .Replace("Get", string.Empty, StringComparison.OrdinalIgnoreCase)
                            .Replace("By", "-", StringComparison.OrdinalIgnoreCase);

            var entityName = methodName.Substring(0, methodName.IndexOf('-', StringComparison.OrdinalIgnoreCase));

            return entityName.ToSentence().ToLowerCase();
        }

        /// <summary>
        /// Builds the summary.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="mi">The method info.</param>
        /// <returns>The summary.</returns>
        private string BuildSummary(string entityName, MethodInfo mi)
        {
            ArgumentValidators.ThrowIfNull(mi, nameof(mi));

            var parameters = mi.GetParameters();
            var sb = new StringBuilder();
            for (var i = 0; i < parameters.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(i == parameters.Length - 1 ? " and " : ", ");
                }

                sb.Append(parameters[i].Name.ToSentence().ToLowerCase());
            }

            return string.Format(CultureInfo.InvariantCulture, this.summary, entityName, sb);
        }
    }
}
