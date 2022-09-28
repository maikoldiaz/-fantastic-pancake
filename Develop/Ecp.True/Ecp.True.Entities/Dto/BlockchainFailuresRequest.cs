// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainFailuresRequest.cs" company="Microsoft">
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
    /// The blockchain failures request.
    /// </summary>
    public class BlockchainFailuresRequest
    {
        /// <summary>
        ///     Gets or sets a value indicating whether gets or sets the isCritical.
        /// </summary>
        /// <value>
        ///     is critical flag.
        /// </value>
        public bool IsCritical { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether gets or sets the takeRecords.
        /// </summary>
        /// <value>
        ///     The number of records that get.
        /// </value>
        public int? TakeRecords { get; set; }
    }
}