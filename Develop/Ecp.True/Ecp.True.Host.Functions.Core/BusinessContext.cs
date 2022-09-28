// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessContext.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core.Interfaces;

    /// <summary>
    /// The Business Context.
    /// </summary>
    public class BusinessContext : IBusinessContext
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; } = "System";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public IEnumerable<string> Roles { get; set; }

        /// <inheritdoc/>
        public string Email => null;

        /// <inheritdoc/>
        public Guid ActivityId { get; private set; }

        /// <inheritdoc/>
        public void Populate(string userId, string email, IEnumerable<string> roles)
        {
            // Functions have no user context
        }

        /// <inheritdoc/>
        public void PopulateImage(string image)
        {
            // Functions have no user context
        }

        /// <inheritdoc/>
        public void SetActivityId(Guid activityId)
        {
            this.ActivityId = activityId;
        }
    }
}
