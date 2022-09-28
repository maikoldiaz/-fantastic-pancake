// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockRangeRequest.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// The block transaction request.
    /// </summary>
    public class BlockRangeRequest
    {
        /// <summary>
        /// Gets or sets the head block.
        /// </summary>
        /// <value>
        /// The head block.
        /// </value>
        [Required(ErrorMessage = Constants.BlockNumberRequired)]
        [JsonConverter(typeof(BlockNumberConverter))]
        public ulong HeadBlock { get; set; }

        /// <summary>
        /// Gets or sets the tail block.
        /// </summary>
        /// <value>
        /// The tail block.
        /// </value>
        [Required(ErrorMessage = Constants.BlockNumberRequired)]
        [JsonConverter(typeof(BlockNumberConverter))]
        public ulong TailBlock { get; set; }

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>
        /// The event.
        /// </value>
        public int Event { get; set; }
    }
}
