// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisServiceRefreshInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Proxies.Azure.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// The AnalysisServiceRefreshInfo.
    /// </summary>
    public class AnalysisServiceRefreshInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisServiceRefreshInfo"/> class.
        /// </summary>
        public AnalysisServiceRefreshInfo()
        {
            this.Objects = new List<AnalysisServiceObject>();
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the maximum parallelism.
        /// </summary>
        /// <value>
        /// The maximum parallelism.
        /// </value>
        public int MaxParallelism { get; set; }

        /// <summary>
        /// Gets the analysis service objects.
        /// </summary>
        /// <value>
        /// The analysis service objects.
        /// </value>
        public ICollection<AnalysisServiceObject> Objects { get; private set; }
    }
}
