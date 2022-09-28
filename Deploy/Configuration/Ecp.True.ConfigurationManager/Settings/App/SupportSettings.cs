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
namespace Ecp.True.ConfigurationManager.Console.Settings
{
    using Ecp.True.ConfigurationManager.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// The Support Settings.
    /// </summary>
    /// <seealso cref="Ecp.True.ConfigurationManager.Console.Settings.SettingsBase" />
    public class SupportSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupportSettings"/> class.
        /// </summary>
        public SupportSettings()
        {
            this.AttentionLinePhoneNumber = "2345000";
            this.AttentionLinePhoneNumberExtention = "4-4-1";
            this.AttentionLineEmail = "servicedesk@ecopetrol.com.co";
            this.ChatbotServiceLink = "https://www.ecopetrol.com.co/";
            this.AutoServicePortalLink = "https://www.ecopetrol.com.co/";
        }

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

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Support.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.AttentionLinePhoneNumber = input.GetStringValueOrDefault(nameof(this.AttentionLinePhoneNumber), this.AttentionLinePhoneNumber, this.Key);
            this.AttentionLinePhoneNumberExtention = input.GetStringValueOrDefault(nameof(this.AttentionLinePhoneNumberExtention), this.AttentionLinePhoneNumberExtention, this.Key);
            this.AttentionLineEmail = input.GetStringValueOrDefault(nameof(this.AttentionLineEmail), this.AttentionLineEmail, this.Key);
            this.ChatbotServiceLink = input.GetStringValueOrDefault(nameof(this.ChatbotServiceLink), this.ChatbotServiceLink, this.Key);
            this.AutoServicePortalLink = input.GetStringValueOrDefault(nameof(this.AutoServicePortalLink), this.AutoServicePortalLink, this.Key);
        }
    }
}
