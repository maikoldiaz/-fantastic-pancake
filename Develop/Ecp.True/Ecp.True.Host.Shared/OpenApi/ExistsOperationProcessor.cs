// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExistsOperationProcessor.cs" company="Microsoft">
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
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Http;
    using NSwag;
    using NSwag.Generation.Processors;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The exists operation processor.
    /// </summary>
    /// <seealso cref="NSwag.Generation.Processors.IOperationProcessor" />
    public class ExistsOperationProcessor : IOperationProcessor
    {
        /// <summary>
        /// The summary.
        /// </summary>
        private readonly string summary = "Determines whether the {0} exists with same name";

        /// <summary>
        /// The status200 message.
        /// </summary>
        private readonly string status200Message = "The {0} name already exists.";

        /// <summary>
        /// The status204 message.
        /// </summary>
        private readonly string status204Message = "The {0} name is unique.";

        /// <summary>
        /// The status400 message.
        /// </summary>
        private readonly string status400Message = "The {0} name is invalid.";

        /// <summary>
        /// The status500 message.
        /// </summary>
        private readonly string status500Message = "Unknown error while validating {0} name.";

        /// <inheritdoc/>
        public bool Process(OperationProcessorContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            // if method name starts with word Query
            if (context.MethodInfo.Name.StartsWith("Exists", StringComparison.OrdinalIgnoreCase))
            {
                var operation = context.OperationDescription.Operation;
                var entityName = GetEntityName(operation.OperationId);
                operation.Summary = string.Format(CultureInfo.InvariantCulture, this.summary, entityName);

                OpenApiResponse response;
                if (operation.Responses.TryGetValue(StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture), out response))
                {
                    response.Description = string.Format(CultureInfo.InvariantCulture, this.status200Message, entityName);
                }

                if (operation.Responses.TryGetValue(StatusCodes.Status204NoContent.ToString(CultureInfo.InvariantCulture), out response))
                {
                    response.Description = string.Format(CultureInfo.InvariantCulture, this.status204Message, entityName);
                }

                if (operation.Responses.TryGetValue(StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture), out response))
                {
                    response.Description = string.Format(CultureInfo.InvariantCulture, this.status400Message, entityName);
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
            methodName = methodName.Replace("Exists", string.Empty, StringComparison.OrdinalIgnoreCase);

            return methodName.ToSentence().ToLowerCase();
        }
    }
}
