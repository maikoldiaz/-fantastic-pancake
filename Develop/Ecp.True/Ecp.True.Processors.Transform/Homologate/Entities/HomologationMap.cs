// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationMap.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Homologate.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The homologation mapping.
    /// </summary>
    public class HomologationMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationMap"/> class.
        /// </summary>
        public HomologationMap()
        {
            this.ConfigurationMaps = new List<ConfigurationMap>();
        }

        /// <summary>
        /// Gets or sets  to ConfigurationMaps.
        /// </summary>
        public IEnumerable<ConfigurationMap> ConfigurationMaps { get; set; }

        /// <summary>
        /// The getHomologationValue.
        /// </summary>
        /// <param name="original">Name of the object.</param>
        /// <param name="categoryName">Name of the Category.</param>
        /// <returns>
        /// Return the homologated value.
        /// </returns>
        public string GetHomologationValue(string original, string categoryName)
        {
            var value = this.ConfigurationMaps.Where(x => x.Name == original && x.CategoryName == categoryName).ToList();
            return value.Any() ? value.FirstOrDefault().Value : string.Empty;
        }
    }
}
