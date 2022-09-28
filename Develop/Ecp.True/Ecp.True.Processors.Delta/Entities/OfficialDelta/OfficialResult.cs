// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OfficialResult.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Delta.Entities.OfficialDelta
{
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Processors.Delta.Interfaces;

    /// <summary>
    /// The blockchain entity.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Delta.IOfficialResult" />
    public class OfficialResult : IOfficialResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficialResult"/> class.
        /// </summary>
        protected OfficialResult()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ResultMovement"/> is sign.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sign; otherwise, <c>false</c>.
        /// </value>
        public bool Sign { get; set; }

        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>
        /// The delta.
        /// </value>
        public decimal OfficialDelta { get; set; }

        /// <summary>
        /// Gets or sets the origen.
        /// </summary>
        /// <value>
        /// The origen.
        /// </value>
        public OriginType Origin { get; set; }

        /// <summary>
        /// Gets or sets the net standard volume.
        /// </summary>
        /// <value>
        /// The net standard volume.
        /// </value>
        public decimal NetStandardVolume { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        public string OwnerId { get; set; }
    }
}
