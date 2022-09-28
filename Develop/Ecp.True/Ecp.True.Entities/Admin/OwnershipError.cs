// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipError.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Ecp.True.Processors.Api.Tests")]

namespace Ecp.True.Entities.Admin
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    ///     The product.
    /// </summary>
    public class OwnershipError : Entity
    {
        /// <summary>
        /// Gets or sets the Operation identifier.
        /// </summary>
        /// <value>
        /// The Operation identifier.
        /// </value>
        public int OperationId { get; set; }

        /// <summary>
        /// Gets or sets the ownership node identifier.
        /// </summary>
        /// <value>
        /// The ownership node identifier.
        /// </value>
        public int OwnershipNodeId { get; set; }

        /// <summary>
        /// Gets or sets the classification.
        /// </summary>
        /// <value>
        /// The classification.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public string Operation { get; set; }

        /// <summary>
        /// Gets or sets the operation date.
        /// </summary>
        /// <value>
        /// The operation date.
        /// </value>
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Gets or sets the execution date.
        /// </summary>
        /// <value>
        /// The execution date.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets the net volume.
        /// </summary>
        /// <value>
        /// The net volume.
        /// </value>
        public decimal NetVolume { get; set; }

        /// <summary>
        /// Gets or sets the product origin.
        /// </summary>
        /// <value>
        /// The product origin.
        /// </value>
        public string ProductOrigin { get; set; }

        /// <summary>
        /// Gets or sets the product destination.
        /// </summary>
        /// <value>
        /// The product destination.
        /// </value>
        public string ProductDestination { get; set; }

        /// <summary>
        /// Gets or sets the node origin.
        /// </summary>
        /// <value>
        /// The node origin.
        /// </value>
        public string NodeOrigin { get; set; }

        /// <summary>
        /// Gets or sets the node destination.
        /// </summary>
        /// <value>
        /// The node destination.
        /// </value>
        public string NodeDestination { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}