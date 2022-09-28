// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeltaNodeReopenRequest.cs" company="Microsoft">
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
    using System.Collections.Generic;

    /// <summary>
    /// Delta Node Reopen Request.
    /// </summary>
    public class DeltaNodeReopenRequest
    {
        /// <summary>
        /// Gets or sets the delta node identifier.
        /// </summary>
        /// <value>
        /// The delta node identifier.
        /// </value>
        public IEnumerable<int> DeltaNodeId { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public string Comment { get; set; }
    }
}