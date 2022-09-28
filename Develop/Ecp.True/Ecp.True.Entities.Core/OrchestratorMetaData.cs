// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestratorMetaData.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Core
{
    /// <summary>
    /// The OrchestratorMetaData.
    /// </summary>
    public class OrchestratorMetaData
    {
        /// <summary>
        /// Gets or sets the chaos value.
        /// </summary>
        /// <value>
        /// The chaos value.
        /// </value>
        public string ChaosValue { get; set; }

        /// <summary>
        /// Gets or sets the caller.
        /// </summary>
        /// <value>
        /// The caller.
        /// </value>
        public string Caller { get; set; }

        /// <summary>
        /// Gets or sets the orchestrator.
        /// </summary>
        /// <value>
        /// The caller.
        /// </value>
        public string Orchestrator { get; set; }

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <value>
        /// The caller.
        /// </value>
        public string Activity { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        public string ReplyTo { get; set; }
    }
}
