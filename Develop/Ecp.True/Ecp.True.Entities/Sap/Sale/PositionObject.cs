// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionObject.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Sap
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The PositionObject class.
    /// </summary>
    /// <seealso cref="Ecp.True.Entities.Core.Entity" />
    public class PositionObject
    {
        /// <summary>
        /// Gets or sets the Positions.
        /// </summary>
        /// <value>
        /// The Positions.
        /// </value>
        [Required]
        [JsonProperty("POSITION")]
        public IEnumerable<Position> Positions { get; set; }
    }
}
