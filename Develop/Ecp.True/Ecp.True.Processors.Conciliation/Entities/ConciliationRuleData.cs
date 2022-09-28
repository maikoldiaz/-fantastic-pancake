// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConciliationRuleData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Conciliation.Entities
{
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;

    /// <summary>
    /// The ownership rule data class.
    /// </summary>
    public class ConciliationRuleData : OrchestratorMetaData
    {
        /// <summary>
        /// Gets or sets the conciliation nodes resquest.
        /// </summary>
        /// <value>
        /// The ownership rule request.
        /// </value>
        public ConciliationNodesResquest ConciliationNodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the ValidateNodeStates.
        /// </summary>
        /// <value>
        /// The ValidateNodeStates resquest.
        /// </value>
        public bool ValidateConciliationNodeStates { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the StatusType.
        /// </summary>
        /// <value>
        /// The StatusType resquest.
        /// </value>
        public StatusType StatusType { get; set; }
    }
}
