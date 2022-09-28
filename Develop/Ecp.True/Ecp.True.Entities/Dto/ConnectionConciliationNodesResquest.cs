// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionConciliationNodesResquest.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// The connection conciliation nodes request.
    /// </summary>
    public class ConnectionConciliationNodesResquest
    {
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the conciliation Nodes.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public IEnumerable<ConciliationNodesResult> ConciliationNodes { get; set; }
    }
}