// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAccessInfo.cs" company="Microsoft">
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
    /// <summary>
    ///     The fileAccessInfo.
    /// </summary>
    public class FileAccessInfo
    {
        /// <summary>
        /// Gets or sets the name of the BLOB.
        /// </summary>
        /// <value>
        /// The name of the BLOB.
        /// </value>
        public string BlobName { get; set; }

        /// <summary>
        ///     Gets or sets the container name.
        /// </summary>
        /// <value>
        ///     The container name.
        /// </value>
        public string ContainerName { get; set; }

        /// <summary>
        ///     Gets or sets the account name.
        /// </summary>
        /// <value>
        ///     The account name.
        /// </value>
        public string AccountName { get; set; }

        /// <summary>
        ///     Gets or sets the sas token.
        /// </summary>
        /// <value>
        ///     The sas token.
        /// </value>
        public string SasToken { get; set; }

        /// <summary>
        ///     Gets or sets the sas token.
        /// </summary>
        /// <value>
        ///     The sas token.
        /// </value>
        public string BlobPath { get; set; }
    }
}
