// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorizeOperationProcessor.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ecp.True.Core;
    using Ecp.True.Entities.Core;
    using NSwag;
    using NSwag.Generation.Processors;
    using NSwag.Generation.Processors.Contexts;

    /// <summary>
    /// The Authorize Operation Processor.
    /// </summary>
    /// <seealso cref="IOperationProcessor" />
    public class AuthorizeOperationProcessor : IOperationProcessor
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        private static string Name => "Roles";

        /// <inheritdoc/>
        /// <summary>Processes the specified method information.</summary>
        /// <param name="context"></param>
        /// <returns>true if the operation should be added to the Swagger specification.</returns>
        public bool Process(OperationProcessorContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            context.OperationDescription.Operation.Security ??= new List<OpenApiSecurityRequirement>();

            var scopes = this.GetScopes(context.OperationDescription, context.MethodInfo);
            if (context.OperationDescription.Operation.Security.Any(a => a.ContainsKey(Name)))
            {
                return true;
            }

            context.OperationDescription.Operation.Security.Add(new OpenApiSecurityRequirement
            {
                { Name, scopes },
            });

            return true;
        }

        /// <summary>Gets the security scopes for an operation.</summary>
        /// <param name="operationDescription">The operation description.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>The scopes.</returns>
        protected virtual IEnumerable<string> GetScopes(OpenApiOperationDescription operationDescription, MethodInfo methodInfo)
        {
            ArgumentValidators.ThrowIfNull(methodInfo, nameof(methodInfo));

            var attributes = methodInfo.GetCustomAttributes().Concat(methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes());

            if (attributes.Any(a => a.GetType().Name == "TrueAuthorizeAttribute"))
            {
                return attributes
                        .Where(a => a.GetType().Name == "TrueAuthorizeAttribute")
                        .Select(a => (dynamic)a)
                        .Where(a => a.AllowedRoles != null)
                        .SelectMany(a => ((IEnumerable<Role>)a.AllowedRoles).Select(r => r.ToString()))
                        .Distinct();
            }

            if (attributes.Any(a => a.GetType().Name == "AuthorizeAttribute"))
            {
                return attributes
                        .Where(a => a.GetType().Name == "AuthorizeAttribute")
                        .Select(a => (dynamic)a)
                        .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                        .Select(a => (string)a.Roles);
            }

            return Enumerable.Empty<string>();
        }
    }
}
