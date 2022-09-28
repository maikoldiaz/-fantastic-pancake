// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingRedirect.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Entities
{
    /// <summary>
    /// The binding redirect entity.
    /// </summary>
    public class BindingRedirect
    {
        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the public key token.
        /// </summary>
        /// <value>
        /// The public key token.
        /// </value>
        public string PublicKeyToken { get; set; }

        /// <summary>
        /// Gets or sets the redirect to version.
        /// </summary>
        /// <value>
        /// The redirect to version.
        /// </value>
        public string RedirectToVersion { get; set; }
    }
}
