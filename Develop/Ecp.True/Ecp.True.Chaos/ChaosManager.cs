// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChaosManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Chaos
{
    using System;
    using System.Globalization;
    using Ecp.True.Chaos.Interfaces;
    using Ecp.True.Core;
    using Ecp.True.Core.Attributes;

    /// <summary>
    /// The chaos manager.
    /// </summary>
    [IoCRegistration(IoCLifetime.Hierarchical)]
    public class ChaosManager : IChaosManager
    {
        /// <inheritdoc/>
        public bool HasChaos => !string.IsNullOrWhiteSpace(this.ChaosValue);

        /// <inheritdoc/>
        public string ChaosValue { get; private set; }

        /// <inheritdoc/>
        public void Initialize(string value)
        {
            this.ChaosValue = value;
        }

        /// <inheritdoc/>
        public string TryTriggerChaos(string caller)
        {
            ArgumentValidators.ThrowIfNullOrEmpty(caller, nameof(caller));
            if (this.IsApiChaos() && caller.EqualsIgnoreCase(ChaosType.Api.ToString()))
            {
                return string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessage, this.ChaosValue);
            }

            if (this.IsWebChaos() && caller.EqualsIgnoreCase(ChaosType.Web.ToString()))
            {
                return string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessage, this.ChaosValue);
            }

            if (this.HasChaos && caller.EqualsIgnoreCase(this.ChaosValue))
            {
                return string.Format(CultureInfo.InvariantCulture, Constants.ErrorMessage, this.ChaosValue);
            }

            return null;
        }

        private bool IsApiChaos()
        {
            return this.HasChaos && this.ChaosValue.EqualsIgnoreCase(ChaosType.Api.ToString());
        }

        private bool IsWebChaos()
        {
            return this.HasChaos && this.ChaosValue.EqualsIgnoreCase(ChaosType.Web.ToString());
        }
    }
}
