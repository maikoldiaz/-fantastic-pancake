// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scenario.cs" company="Microsoft">
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
    /// The scenario.
    /// </summary>
    public class Scenario : Entity
    {
        /// <summary>
        /// Gets or sets the scenario identifier.
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
        /// Gets or sets the feature sequence number.
        /// </summary>
        /// <value>
        /// The feature sequence number.
        /// </value>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The features.
        /// </value>
        public IEnumerable<Feature> Features { get; set; }
    }
}
