// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupportSettings.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The Support Settings configuration.
    /// </summary>
    public class SupportSettings
    {
        /// <summary>
        /// Gets or sets attention line phone number.
        /// </summary>
        /// <value>
        /// The attention line phone number.
        /// </value>
        public string AttentionLinePhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets attention line phone number extention.
        /// </summary>
        /// <value>
        /// The attention line phone number extention.
        /// </value>
        public string AttentionLinePhoneNumberExtention { get; set; }

        /// <summary>
        /// Gets or sets attention line email.
        /// </summary>
        /// <value>
        /// The attention line email.
        /// </value>
        public string AttentionLineEmail { get; set; }

        /// <summary>
        /// Gets or sets chatbot service link.
        /// </summary>
        /// <value>
        /// The chatbot service link.
        /// </value>
        public string ChatbotServiceLink { get; set; }

        /// <summary>
        /// Gets or sets auto service portal link.
        /// </summary>
        /// <value>
        /// The auto service portal link.
        /// </value>
        public string AutoServicePortalLink { get; set; }
    }
}
