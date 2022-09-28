// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Algorithm.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Ecp.True.Entities.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// The algorithms.
    /// </summary>
    public class Algorithm : AuditableEntity
    {
        /// <summary>Initializes a new instance of the <see cref="Algorithm"/> class.</summary>
        public Algorithm()
        {
            this.NodeConnections = new List<NodeConnection>();
        }

        /// <summary>
        /// Gets or sets the algorithm identifier.
        /// </summary>
        /// <value>
        /// The algorithm identifier.
        /// </value>
        public int AlgorithmId { get; set; }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        /// <value>
        /// The model name.
        /// </value>
        [Required(ErrorMessage = Entities.Constants.CategoryNameRequired)]
        [StringLength(150, ErrorMessage = Entities.Constants.NameMaxLength150)]
        [RegularExpression(Entities.Constants.AllowNumbersAndLettersWithSpecialCharactersWithSpaceRegex, ErrorMessage = Entities.Constants.AllowAlphanumericWithSpecialCharactersAndSpaceMessage)]
        public string ModelName { get; set; }

        /// <summary>Gets or sets the periods to forecast.</summary>
        /// <value>The periods to forecast.</value>
        public int? PeriodsToForecast { get; set; }

        /// <summary>Gets the node connections.</summary>
        /// <value>The node connections.</value>
        [JsonProperty]
        public ICollection<NodeConnection> NodeConnections { get; private set; }
    }
}
