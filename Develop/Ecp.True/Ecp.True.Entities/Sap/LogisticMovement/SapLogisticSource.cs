// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapLogisticSource.cs" company="Microsoft">
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
    /// The Logistic Movement Request.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class SapLogisticSource
    {
        /// <summary>
        /// Gets or Sets the SourcePlant.
        /// </summary>
        [JsonProperty("SOURCEPLANT")]
        public string SourcePlant { get; set; }

        /// <summary>
        /// Gets or Sets the SourceStorageLocationId.
        /// </summary>
        [JsonProperty("SOURCESTORAGELOCATIONID")]
        public string SourceStorageLocationId { get; set; }

        /// <summary>
        /// Gets or Sets the SourceProductId.
        /// </summary>
        [JsonProperty("SOURCEPRODUCTID")]
        public string SourceProductId { get; set; }
    }
}
