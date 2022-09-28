// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileRegistrationError.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The register file error logs.
    /// </summary>
    /// <seealso cref="Entity" />
    public class FileRegistrationError : Entity
    {
        /// <summary>
        /// Gets or sets the temporary identifier.
        /// </summary>
        /// <value>
        /// The temporary identifier.
        /// </value>
        public int TempId { get; set; }

        /// <summary>
        /// Gets or sets the error log identifier.
        /// </summary>
        /// <value>
        /// The error log identifier.
        /// </value>
        public int FileRegistrationErrorId { get; set; }

        /// <summary>
        /// Gets or sets the register file identifier.
        /// </summary>
        /// <value>
        /// The register file identifier.
        /// </value>
        public int FileRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        [ColumnIgnore]
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the register file.
        /// </summary>
        /// <value>
        /// The register file.
        /// </value>
        [ColumnIgnore]
        public virtual FileRegistration FileRegistration { get; set; }
    }
}
