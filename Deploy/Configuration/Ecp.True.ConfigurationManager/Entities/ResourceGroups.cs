// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceGroups.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Entities
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Resource Group.
    /// </summary>
    public class ResourceGroups : ICopyable
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Subscription.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <inheritdoc/>
        public void CopyFrom(JToken existing)
        {
            if (existing == null)
            {
                return;
            }

            this.Name = existing.GetStringValueOrDefault(nameof(this.Name), this.Name);
            this.SubscriptionId = existing.GetStringValueOrDefault(nameof(this.SubscriptionId), this.SubscriptionId);
        }
    }
}
