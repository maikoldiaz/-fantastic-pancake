// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ODataOperationProcessor.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.Core;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Http;
    using NSwag;
    using NSwag.Generation.Processors;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The ODATA operation processor.
    /// </summary>
    /// <seealso cref="NSwag.Generation.Processors.IOperationProcessor" />
    public class ODataOperationProcessor : IOperationProcessor
    {
        /// <summary>
        /// The summary.
        /// </summary>
        private readonly string summary = "Gets all the {0}, the method supports ODATA";

        /// <inheritdoc/>
        public bool Process(OperationProcessorContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            // if method name starts with word Query
            if (context.MethodInfo.Name.StartsWith("Query", StringComparison.OrdinalIgnoreCase))
            {
                var description = context.OperationDescription;
                var attribute = context.MethodInfo.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(ODataRouteAttribute));

                description.Path = $"/odata/v1/{attribute.ConstructorArguments[0].Value}";
                var operation = description.Operation;
                var entityName = GetEntityName(operation.OperationId);
                operation.Summary = string.Format(CultureInfo.InvariantCulture, this.summary, entityName);

                if (operation.Responses.TryGetValue(StatusCodes.Status200OK.ToString(CultureInfo.InvariantCulture), out OpenApiResponse response))
                {
                    response.Description = "The ODATA query response";
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
            methodName = methodName.Replace("Query", string.Empty, StringComparison.OrdinalIgnoreCase);

            return methodName.ToSentence().ToLowerCase();
        }
    }
}
