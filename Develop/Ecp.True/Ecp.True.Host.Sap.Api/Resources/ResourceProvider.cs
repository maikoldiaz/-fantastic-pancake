// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceProvider.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Resources
{
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Core.Interfaces;
    using Ecp.True.Host.Sap.Api.Setup;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// The resource provider.
    /// </summary>
    /// <seealso cref="Ecp.True.Core.Interfaces.IResourceProvider" />
    [IoCRegistration(IoCLifetime.ContainerControlled)]
    public class ResourceProvider : IResourceProvider
    {
        /// <summary>
        /// The localizer.
        /// </summary>
        private readonly IStringLocalizer localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProvider"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public ResourceProvider(IStringLocalizerFactory factory)
        {
            ArgumentValidators.ThrowIfNull(factory, nameof(factory));
            this.localizer = SetupOptions.BuildSharedLocalizer(factory);
        }

        /// <inheritdoc/>
        public string GetResource(string key)
        {
            return this.localizer[key];
        }
    }
}
