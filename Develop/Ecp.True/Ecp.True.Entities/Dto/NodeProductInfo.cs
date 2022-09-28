// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeProductInfo.cs" company="Microsoft">
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
    using Ecp.True.Entities.Admin;

    /// <summary>The node product info.</summary>
    public class NodeProductInfo
    {
        /// <summary>
        ///     Gets or sets the destination nodes.
        /// </summary>
        /// <value>
        ///     The node storage locations.
        /// </value>
        public IEnumerable<Node> DestinationNodes { get; set; }

        /// <summary>
        ///     Gets or sets the source products.
        /// </summary>
        /// <value>
        ///     The source products.
        /// </value>
        public IEnumerable<StorageLocationProduct> SourceProducts { get; set; }

        /// <summary>
        ///     Gets or sets the destination products.
        /// </summary>
        /// <value>
        ///     The destination products.
        /// </value>
        public IEnumerable<StorageLocationProduct> DestinationProducts { get; set; }
    }
}