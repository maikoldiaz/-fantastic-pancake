// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorDetail.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Query
{
    using System;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// the error details.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class ErrorDetail : Entity
    {
        /// <summary>
        /// Gets or sets the error identifier.
        /// </summary>
        /// <value>
        /// The error identifier.
        /// </value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the type of the system.
        /// </summary>
        /// <value>
        /// The type of the system.
        /// </value>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the name of the system type.
        /// </summary>
        /// <value>
        /// The name of the system type.
        /// </value>
        public string SystemTypeName { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        /// <value>
        /// The segment.
        /// </value>
        public string Segment { get; set; }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>
        /// The process.
        /// </value>
        public string Process { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public string Volume { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        /// <value>
        /// The measurement unit.
        /// </value>
        public string MeasurementUnit { get; set; }

        /// <summary>
        /// Gets or sets the initial date.
        /// </summary>
        /// <value>
        /// The initial date.
        /// </value>
        public DateTime? InitialDate { get; set; }

        /// <summary>
        /// Gets or sets the final date.
        /// </summary>
        /// <value>
        /// The final date.
        /// </value>
        public DateTime? FinalDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the source node.
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the destination node.
        /// </summary>
        /// <value>
        /// The destination node.
        /// </value>
        public string DestinationNode { get; set; }

        /// <summary>
        /// Gets or sets the source product.
        /// </summary>
        /// <value>
        /// The source product.
        /// </value>
        public string SourceProduct { get; set; }

        /// <summary>
        /// Gets or sets the destination product.
        /// </summary>
        /// <value>
        /// The destination product.
        /// </value>
        public string DestinationProduct { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is retry.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is retry; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetry { get; set; }

        /// <summary>
        /// Gets or sets the file registration transaction identifier.
        /// </summary>
        /// <value>
        /// The file registration transaction identifier.
        /// </value>
        public int? FileRegistrationTransactionId { get; set; }
    }
}
