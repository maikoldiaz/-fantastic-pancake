// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleAvailabilitySettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The module availability Settings.
    /// </summary>
    public class ModuleAvailabilitySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAvailabilitySettings"/> class.
        /// </summary>
        public ModuleAvailabilitySettings()
        {
            this.Resources = new List<string>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>
        /// The resources.
        /// </value>
        public ICollection<string> Resources { get; private set; }
    }
}
