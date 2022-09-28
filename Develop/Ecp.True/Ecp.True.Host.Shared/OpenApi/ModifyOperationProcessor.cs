// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyOperationProcessor.cs" company="Microsoft">
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
    /// The operation processor.
    /// </summary>
    /// <seealso cref="NSwag.Generation.Processors.IOperationProcessor" />
    public class ModifyOperationProcessor : IOperationProcessor
    {
        /// <summary>
        /// The create summary.
        /// </summary>
        private readonly string createSummary = "Creates a new {0}";

        /// <summary>
        /// The update summary.
        /// </summary>
        private readonly string updateSummary = "Updates an existing {0}";

        /// <summary>
        /// The delete summary.
        /// </summary>
        private readonly string deleteSummary = "Deletes an existing {0}";

        /// <summary>
        /// The created successfully.
        /// </summary>
        private readonly string status200Message = "The {0} was {1} successfully.";

        /// <summary>
        /// The bad request.
        /// </summary>
        private readonly string status400Message = "The {0} has missing/invalid values.";

        /// <summary>
        /// The error response.
        /// </summary>
        private readonly string status500Message = "Unknown error while {1} {0}";

        /// <inheritdoc/>
        public bool Process(OperationProcessorContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            if (IsCreate(context))
            {
                this.Document(context.OperationDescription.Operation, "C");
                return true;
            }

            if (IsUpdate(context))
            {
                this.Document(context.OperationDescription.Operation, "U");
                return true;
            }

            if (IsDelete(context))
            {
                this.Document(context.OperationDescription.Operation, "D");
                return true;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified context is create.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is create; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsCreate(OperationProcessorContext context)
        {
            return context.MethodInfo.Name.StartsWith("Create", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified context is update.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is update; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsUpdate(OperationProcessorContext context)
        {
            return context.MethodInfo.Name.StartsWith("Update", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified context is delete.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the specified context is delete; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsDelete(OperationProcessorContext context)
        {
            return context.MethodInfo.Name.StartsWith("Delete", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <param name="operationId">The operation identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns>
        /// The entity name.
        /// </returns>
        private static string GetEntityName(string operationId, string status)
        {
            var methodName = operationId.Split('_')[1];
            methodName = methodName.Replace(GetKeyToReplace(status), string.Empty, StringComparison.OrdinalIgnoreCase);

            return methodName.ToSentence().ToLowerCase();
        }

        /// <summary>
        /// Gets the key to replace.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The key to replace.</returns>
        private static string GetKeyToReplace(string status)
        {
            if (status == "C")
            {
                return "Create";
            }

            if (status == "U")
            {
                return "Update";
            }

            return "Delete";
        }

        /// <summary>
        /// Gets the description message.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="is200">if set to <c>true</c> [is200].</param>
        /// <returns>
        /// The description message.
        /// </returns>
        private static string GetDescriptionMessage(string status, bool is200)
        {
            if (status == "C")
            {
                return is200 ? "created" : "creating";
            }

            if (status == "U")
            {
                return is200 ? "updated" : "updating";
            }

            return is200 ? "deleted" : "deleting";
        }

        /// <summary>
        /// Gets the summary message.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>
        /// The summary message.
        /// </returns>
        private string GetSummaryMessage(string status)
        {
            if (status == "C")
            {
                return this.createSummary;
            }

            if (status == "U")
            {
                return this.updateSummary;
            }

            return this.deleteSummary;
        }

        /// <summary>
        /// Documents the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="status">The status.</param>
        private void Document(OpenApiOperation operation, string status)
        {
            var entityName = GetEntityName(operation.OperationId, status);

            OpenApiResponse response;
            if (operation.ActualResponses.TryGetValue(StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture), out response))
            {
                response.Description = string.Format(CultureInfo.InvariantCulture, this.status200Message, entityName, GetDescriptionMessage(status, true));
            }

            if (operation.ActualResponses.TryGetValue(StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture), out response))
            {
                response.Description = string.Format(CultureInfo.InvariantCulture, this.status400Message, entityName);
            }

            if (operation.ActualResponses.TryGetValue(StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture), out response))
            {
                response.Description = string.Format(CultureInfo.InvariantCulture, this.status500Message, entityName, GetDescriptionMessage(status, false));
            }

            operation.Summary = string.Format(CultureInfo.InvariantCulture, this.GetSummaryMessage(status), entityName);
        }
    }
}
