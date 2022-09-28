// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorViewModel.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Models
{
    /// <summary>
    /// The error view model.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        /// <value>
        /// Gets the total result count for Web view (JSX) rendering.
        /// </value>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show request id.
        /// </summary>
        /// <value>
        /// Gets the total result count for Web view (JSX) rendering.
        /// </value>
        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}