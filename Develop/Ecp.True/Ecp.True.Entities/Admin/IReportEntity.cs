// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportEntity.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The ExecutionStatus.
    /// </summary>
    public interface IReportEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the Execution identifier.
        /// </summary>
        /// <value>
        /// The Execution identifier.
        /// </value>
        public Guid ExecutionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public bool Status { get; set; }
    }
}
