// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostConfigureCookieAuthenticationOptions.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.UI.Auth
{
    using Ecp.True.Core;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The post config option.
    /// </summary>
    public class PostConfigureCookieAuthenticationOptions : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        private readonly ITicketStore ticketStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostConfigureCookieAuthenticationOptions"/> class.
        /// </summary>
        /// <param name="ticketStore">The ticket store.</param>
        public PostConfigureCookieAuthenticationOptions(ITicketStore ticketStore)
        {
            this.ticketStore = ticketStore;
        }

        /// <summary>
        /// Invoked to configure a post config instance.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configured.</param>
        public void PostConfigure(string name, CookieAuthenticationOptions options)
        {
            ArgumentValidators.ThrowIfNull(options, nameof(options));
            options.SessionStore = this.ticketStore;
        }
    }
}
