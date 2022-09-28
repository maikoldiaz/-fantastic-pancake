// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeCostCenterInfo.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;

    /// <summary>
    /// Node cost center info dto.
    /// </summary>
    public class NodeCostCenterInfo
    {
        /// <summary>
        /// The node cost center status.
        /// </summary>
        public enum CreationStatus
        {
            /// <summary>
            /// Node cost center created successfully.
            /// </summary>
            Created = 1,

            /// <summary>
            /// Node cost center already not created beacause of duplication.
            /// </summary>
            Duplicated = 2,
        }

        /// <summary>
        /// Gets or sets the node cost center.
        /// </summary>
        public NodeCostCenter NodeCostCenter { get; set; }

        /// <summary>
        /// Gets or sets the creation status.
        /// </summary>
        public CreationStatus Status { get; set; }
    }
}
