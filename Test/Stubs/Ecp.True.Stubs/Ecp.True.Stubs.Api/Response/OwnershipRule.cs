// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipRule.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// The class Ownership Rule.
    /// </summary>
    public class OwnershipRule
    {
        /// <summary>
        /// Gets or sets the rule identifier.
        /// </summary>
        /// <value>
        /// The rule identifier.
        /// </value>
        [JsonProperty("IdEstrategia")]
        public int RuleId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("Estrategia")]
        public string Name { get; set; }
    }
}
