// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Feature.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities
{
    using System.Collections.Generic;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The module.
    /// </summary>
    public class Feature : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        public Feature()
        {
            this.FeatureRoles = new List<FeatureRole>();
        }

        /// <summary>
        /// Gets or sets the feature Identifier.
        /// </summary>
        /// <value>
        /// The feature identifier.
        /// </value>
        public int FeatureId { get; set; }

        /// <summary>
        /// Gets or sets the scenario Identifier.
        /// </summary>
        /// <value>
        /// The scenario identifier.
        /// </value>
        public int ScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the name of the feature.
        /// </summary>
        /// <value>
        /// The name of the feature.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the feature.
        /// </summary>
        /// <value>
        /// The description of the feature.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the feature sequence number.
        /// </summary>
        /// <value>
        /// The feature sequence number.
        /// </value>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets the scenario.
        /// </summary>
        /// <value>
        /// The scenario.
        /// </value>
        public virtual Scenario Scenario { get; set; }

        /// <summary>
        /// Gets the feature roles.
        /// </summary>
        /// <value>
        /// The feature roles.
        /// </value>
        public virtual ICollection<FeatureRole> FeatureRoles { get; }
    }
}
