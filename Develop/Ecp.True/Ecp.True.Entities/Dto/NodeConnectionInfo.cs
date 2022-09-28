// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeConnectionInfo.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// Node cost connection info dto.
    /// </summary>
    public class NodeConnectionInfo
    {
        /// <summary>
        /// Gets or sets the node connection.
        /// </summary>
        public NodeConnection NodeConnection { get; set; }

        /// <summary>
        /// Gets or sets the creation status.
        /// </summary>
        public EntityInfoCreationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the offchain connection.
        /// </summary>
        public OffchainNodeConnection OffchainConnection { get; set; }

        /// <summary>
        /// Gets or sets the Errors.
        /// </summary>
        public ErrorResponse Errors { get; set; }
    }
}
