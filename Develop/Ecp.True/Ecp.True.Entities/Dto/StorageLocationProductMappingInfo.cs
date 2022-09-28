// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductMappingInfo.cs" company="Microsoft">
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
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Enumeration;

    /// <summary>
    /// The storageLocation product info dto.
    /// </summary>
    public class StorageLocationProductMappingInfo
    {
        /// <summary>
        /// Gets or sets the storage location identifier.
        /// </summary>
        /// <value>
        /// The storage location identifier.
        /// </value>
        public string StorageLocationId { get; set; }

        /// <summary>
        /// Gets or sets the storage location name.
        /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the logistic center name.
        /// </summary>
        public string LogisticCenterName { get; set; }

        /// <summary>
        /// Gets or sets the creation status.
        /// </summary>
        public EntityInfoCreationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public ErrorResponse Errors { get; set; }
    }
}