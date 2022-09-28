// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SapOther.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap.Purchases
{
    using Newtonsoft.Json;

    /// <summary>
    /// The SAP PO Other class.
    /// </summary>
    public class SapOther
    {
        /// <summary>
        /// Gets or sets almacen.
        /// </summary>
        [JsonProperty("ALMACEN")]
        public string Almacen { get; set; }

        /// <summary>
        /// Gets or sets center.
        /// </summary>
        [JsonProperty("CENTER")]
        public string Center { get; set; }
    }
}
