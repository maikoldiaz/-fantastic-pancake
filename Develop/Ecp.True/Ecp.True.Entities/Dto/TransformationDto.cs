// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformationDto.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;

    /// <summary>The transformation.</summary>
    public class TransformationDto
    {
        /// <summary>Gets or sets the transformation identifier.</summary>
        /// <value>The transformation identifier.</value>
        public int TransformationId { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>
        /// The row version.
        /// </value>
        public string RowVersion { get; set; }

        /// <summary>Gets or sets the origin.</summary>
        /// <value>The origin.</value>
        public Origin Origin { get; set; }

        /// <summary>Gets or sets the destination.</summary>
        /// <value>The destination.</value>
        public Destination Destination { get; set; }

        /// <summary>Gets or sets the message type identifier.</summary>
        /// <value>The message type identifier.</value>
        [EnumDataType(typeof(MessageType))]
        public MessageType MessageTypeId { get; set; }
    }
}