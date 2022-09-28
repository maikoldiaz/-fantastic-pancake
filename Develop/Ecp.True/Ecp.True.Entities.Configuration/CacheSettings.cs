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

namespace Ecp.True.Entities.Configuration
{
    /// <summary>
    /// The Support Settings configuration.
    /// </summary>
    public class CacheSettings
    {
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
    }
}
