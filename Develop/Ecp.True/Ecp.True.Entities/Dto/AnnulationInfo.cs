// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnulationInfo.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Dto
{
    /// <summary>
    /// The annulation information dto.
    /// </summary>
    public class AnnulationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnulationInfo" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="annulation">The annulation.</param>
        /// <param name="type">The type.</param>
        public AnnulationInfo(string source, string annulation, string type)
        {
            this.Source = source;
            this.Annulation = annulation;
            this.Type = type;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; }

        /// <summary>
        /// Gets the annulation.
        /// </summary>
        /// <value>
        /// The annulation.
        /// </value>
        public string Annulation { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; }
    }
}
