// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockEventRequest.cs" company="Microsoft">
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

    /// <summary>
    /// The block event request.
    /// </summary>
    public class BlockEventRequest
    {
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        [Range(1, 100, ErrorMessage = Entities.Constants.InvalidBlockchainPageSize)]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the last head.
        /// </summary>
        /// <value>
        /// The last head.
        /// </value>
        public ulong? LastHead { get; set; }
    }
}
