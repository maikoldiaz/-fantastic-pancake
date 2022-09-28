// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityExistsResult.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Result
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Analytics;
    using Ecp.True.Entities.Core;
    using Microsoft.AspNetCore.Mvc;
    using EntityContants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The entity exists action result.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.IActionResult" />
    public class EntityExistsResult : IActionResult
    {
        /// <summary>
        /// The error messages.
        /// </summary>
        private static readonly Dictionary<string, string> ErrorMessages = new Dictionary<string, string>
        {
            { nameof(Node), EntityContants.NodeNameMustBeUnique },
            { nameof(NodeStorageLocation), EntityContants.StorageNameMustBeUnique },
            { nameof(Category), EntityContants.CategoryNameAlreadyExists },
            { nameof(CategoryElement), EntityContants.CategoryElementNameAlreadyExists },
            { nameof(OperativeNodeRelationshipWithOwnership), EntityContants.TransferAssociationDuplicate },
        };

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        private readonly string key;

        /// <summary>
        /// The exists.
        /// </summary>
        private readonly bool exists;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExistsResult" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        public EntityExistsResult(string key, IEntity entity)
        {
            this.key = key;
            this.exists = entity != null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExistsResult"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="exists">if set to <c>true</c> [exists].</param>
        public EntityExistsResult(string key, bool exists)
        {
            this.key = key;
            this.exists = exists;
        }

        /// <summary>
        /// Executes the result operation of the action method asynchronously. This method is called by MVC to process
        /// the result of an action method.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes
        /// information about the action that was executed and request information.</param>
        /// <returns>
        /// A task that represents the asynchronous execute operation.
        /// </returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));

            if (this.exists)
            {
                var errorMessage = resourceProvider.GetResource(this.BuildErrorMessage());
                var apiResponse = new ErrorResponse(errorMessage);

                var notFoundResult = new OkObjectResult(apiResponse);
                return notFoundResult.ExecuteResultAsync(context);
            }

            var result = new NoContentResult();
            return result.ExecuteResultAsync(context);
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <returns>Returns the error message.</returns>
        private string BuildErrorMessage()
        {
            return ErrorMessages.ContainsKey(this.key) ? ErrorMessages[this.key] : EntityContants.EntityNameMustBeUnique;
        }
    }
}
