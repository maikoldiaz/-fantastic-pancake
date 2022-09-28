// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core
{
    /// <summary>
    /// The Info service.
    /// </summary>
    public class GraphInfo
    {
        /// <summary>
        /// Gets or sets the graph api path.
        /// </summary>
        /// <value>
        /// The graph api path.
        /// </value>
        public string GraphApiPath { get; set; }

        /// <summary>
        /// Gets or sets the graph api scope.
        /// </summary>
        /// <value>
        /// The graph api scope.
        /// </value>
        public string GraphScope { get; set; }
    }
}
