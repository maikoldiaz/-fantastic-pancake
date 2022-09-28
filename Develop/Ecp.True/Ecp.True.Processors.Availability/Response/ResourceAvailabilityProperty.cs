// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceAvailabilityProperty.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Availability.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// ResourceAvailabilityProperty.
    /// </summary>
    public class ResourceAvailabilityProperty
    {
        /// <summary>
        /// Gets or sets the state of the availability.
        /// </summary>
        /// <value>
        /// The state of the availability.
        /// </value>
        [JsonProperty("availabilityState")]
        public string AvailabilityState { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        [JsonProperty("summary")]
        public string Summary { get; set; }
    }
}