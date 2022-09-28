// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheSettings.cs" company="Microsoft">
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
    public class CacheSettings : SettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheSettings"/> class.
        /// </summary>
        public CacheSettings()
        {
            this.RedisConnectionString = "#RedisCacheConnectionString#";
            this.Expiration = 60;
            this.Sliding = true;
        }

        /// <summary>
        /// Gets or sets the redis connection string.
        /// </summary>
        /// <value>
        /// The redis connection string.
        /// </value>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public int Expiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CacheSettings"/> is sliding.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sliding; otherwise, <c>false</c>.
        /// </value>
        public bool Sliding { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonIgnore]
        public override string Key => "Cache.Settings";

        /// <inheritdoc/>
        protected override void DoCopyFrom(CopyInput input)
        {
            ArgumentValidators.ThrowIfNull(input, nameof(input));
            this.RedisConnectionString = input.GetStringValueOrDefault(nameof(this.RedisConnectionString), this.RedisConnectionString, this.Key);
            this.Expiration = input.GetIntValueOrDefault(nameof(this.Expiration), this.Expiration, this.Key);
            this.Sliding = input.GetBoolValueOrDefault(nameof(this.Sliding), this.Sliding, this.Key);
        }
    }
}
