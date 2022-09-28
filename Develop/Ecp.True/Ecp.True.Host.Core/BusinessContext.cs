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

namespace Ecp.True.Host.Core
{
    using System;
    using System.Collections.Generic;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Core.Interfaces;

    /// <summary>
    /// The Business Context.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class BusinessContext : IBusinessContext
    {
        /// <inheritdoc/>
        public string UserId { get; private set; }

        /// <inheritdoc/>
        public string Email { get; private set; }

        /// <inheritdoc/>
        public string Image { get; private set; }

        /// <inheritdoc/>
        public Guid ActivityId { get; private set; }

        /// <inheritdoc/>
        public IEnumerable<string> Roles { get; private set; }

        /// <inheritdoc/>
        public void Populate(string userId, string email, IEnumerable<string> roles)
        {
            this.UserId = userId;
            this.Email = email;
            this.Roles = roles;
        }

        /// <inheritdoc/>
        public void PopulateImage(string image)
        {
            this.Image = image;
        }

        /// <inheritdoc/>
        public void SetActivityId(Guid activityId)
        {
            this.ActivityId = activityId;
        }
    }
}