// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegrationData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    using System;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The integration data.
    /// </summary>
    public class IntegrationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationData"/> class.
        /// </summary>
        public IntegrationData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationData"/> class.
        /// </summary>
        /// <param name="type">The system type.</param>
        public IntegrationData(SystemType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public SystemType Type { get; set; }

        /// <summary>
        /// Gets or sets the type of the integration.
        /// </summary>
        /// <value>
        /// The type of the integration.
        /// </value>
        public IntegrationType IntegrationType { get; set; }

        /// <summary>
        /// Gets or sets the previous upload identifier.
        /// It is a previous GUID, if this integration haven't a previous upload, the value must be null.
        /// </summary>
        /// <value>
        /// The previous upload identifier.
        /// </value>
        public string PreviousUploadId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the BLOB.
        /// </summary>
        /// <value>
        /// The name of the BLOB.
        /// </value>
        public string BlobName { get; set; }

        /// <summary>
        /// Gets or sets the BLOB path.
        /// </summary>
        /// <value>
        /// The BLOB path.
        /// </value>
        public string BlobPath { get; set; }
    }
}
