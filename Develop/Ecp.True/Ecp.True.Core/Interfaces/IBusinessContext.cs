// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBusinessContext.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Core.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Business Context.
    /// </summary>
    public interface IBusinessContext
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        string UserId { get; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        string Image { get; }

        /// <summary>
        /// Gets the activity identifier.
        /// </summary>
        /// <value>
        /// The activity identifier.
        /// </value>
        Guid ActivityId { get; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Populates the specified user information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="email">The email.</param>
        /// <param name="roles">The roles.</param>
        void Populate(string userId, string email, IEnumerable<string> roles);

        /// <summary>
        /// Populates the image.
        /// </summary>
        /// <param name="image">The image.</param>
        void PopulateImage(string image);

        /// <summary>
        /// Sets the activity identifier.
        /// </summary>
        /// <param name="activityId">The activity identifier.</param>
        void SetActivityId(Guid activityId);
    }
}