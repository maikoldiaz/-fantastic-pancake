// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLogisticOrder.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using Newtonsoft.Json;

    /// <summary>
    /// The Order Movement Request.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapLogisticOrder
    {
        /// <summary>
        /// Gets or Sets the BatchId.
        /// </summary>
        [JsonProperty("BATCHTID")]
        public string BatchId { get; set; }

        /// <summary>
        /// Gets or Sets the OrderMovement.
        /// </summary>
        [JsonProperty("ORDERMOVEMENT")]
        public string OrderMovement { get; set; }

        /// <summary>
        /// Gets or Sets the NumReg.
        /// </summary>
        [JsonProperty("NUMREG")]
        public string NumReg { get; set; }

        /// <summary>
        /// Gets or Sets the OrderNode.
        /// </summary>
        [JsonProperty("ORDERNODE")]
        public string OrderNode { get; set; }

        /// <summary>
        /// Gets or Sets the Segment.
        /// </summary>
        [JsonProperty("SEGMENT")]
        public string Segment { get; set; }
    }
}
