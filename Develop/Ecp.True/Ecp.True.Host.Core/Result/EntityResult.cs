// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityResult.cs" company="Microsoft">
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
    using System.Threading.Tasks;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The entity exists action result.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.IActionResult" />
    public class EntityResult : IActionResult
    {
        /// <summary>
        /// The message key.
        /// </summary>
        private readonly string messageKey;

        /// <summary>
        /// The not found.
        /// </summary>
        private readonly bool notFound;

        /// <summary>
        /// The value.
        /// </summary>
        private readonly object value;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityResult"/> class.
        /// </summary>
        public EntityResult()
        {
            this.IsEmpty = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityResult"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public EntityResult(object value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityResult" /> class.
        /// </summary>
        /// <param name="messageKey">The message key.</param>
        public EntityResult(string messageKey)
        {
            this.messageKey = messageKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityResult" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="notFoundMessage">The not found message.</param>
        public EntityResult(object value, string notFoundMessage)
            : this(notFoundMessage)
        {
            this.value = value;
            this.notFound = true;
        }

        /// <summary>
        /// Gets the messagekey.
        /// </summary>
        /// <value>
        /// The messagekey.
        /// </value>
        public string Messagekey
        {
            get { return this.messageKey; }
        }

        /// <summary>
        /// Gets a value indicating whether [not found].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [not found]; otherwise, <c>false</c>.
        /// </value>
        public bool NotFound
        {
            get { return this.notFound; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty { get; }

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
            if (this.IsEmpty)
            {
                var result = new OkResult();
                return result.ExecuteResultAsync(context);
            }

            return this.value != null ? this.BuildValueResultAsync(context) : this.BuildMessageOnlyResultAsync(context);
        }

        /// <summary>
        /// Builds the value result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        private Task BuildValueResultAsync(ActionContext context)
        {
            // All cases where we have value like below:-
            // NON-ODATA Get APIs like master controller
            // When GetById API returns response.
            var result = new OkObjectResult(this.value);
            return result.ExecuteResultAsync(context);
        }

        /// <summary>
        /// Builds the message only result.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The task.</returns>
        private Task BuildMessageOnlyResultAsync(ActionContext context)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));

            var resourceProvider = (IResourceProvider)context.HttpContext.RequestServices.GetService(typeof(IResourceProvider));
            var message = resourceProvider.GetResource(this.messageKey);

            if (this.notFound)
            {
                // When GetById APIs returns no response
                var result = new NotFoundObjectResult(ApiResponse.Create(message));
                return result.ExecuteResultAsync(context);
            }
            else
            {
                // Create/Update success case
                var result = new OkObjectResult(ApiResponse.Create(message));
                return result.ExecuteResultAsync(context);
            }
        }
    }
}
