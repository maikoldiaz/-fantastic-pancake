// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingRedirectHelper.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Functions.Core.Setup
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;
    using Ecp.True.Host.Functions.Core.Entities;

    /// <summary>
    /// The assembly binding redirect helper class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class BindingRedirectHelper
    {
        /// <summary>
        /// Reads the "BindingRedirects" field from the app settings and applies the redirection on the
        /// specified assemblies.
        /// </summary>
        public static void ConfigureBindingRedirects()
        {
            var redirects = GetBindingRedirects();
            redirects.ForEach(RedirectAssembly);
        }

        /// <summary>
        /// Get binding redirects.
        /// </summary>
        /// <returns>List of binding redirects.</returns>
        private static List<BindingRedirect> GetBindingRedirects()
        {
            var bindingRedirects = new List<BindingRedirect>();
            bindingRedirects.Add(new BindingRedirect { PublicKeyToken = "b03f5f7f11d50a3a", RedirectToVersion = "4.0.0.0", ShortName = "System.ComponentModel.Annotations" });
            bindingRedirects.Add(new BindingRedirect { PublicKeyToken = "30ad4fe6b2a6aeed", RedirectToVersion = "12.0.0.0", ShortName = "Newtonsoft.Json" });
            bindingRedirects.Add(new BindingRedirect { PublicKeyToken = "31bf3856ad364e35", RedirectToVersion = "5.2.0.0", ShortName = "Microsoft.IdentityModel.Clients.ActiveDirectory" });
            bindingRedirects.Add(new BindingRedirect { PublicKeyToken = "94bc3704cddfc263", RedirectToVersion = "3.2.0.0", ShortName = "System.Interactive.Async" });
            return bindingRedirects;
        }

        /// <summary>
        /// Redirect assembly
        /// Credit: https://github.com/Azure/azure-functions-host/issues/992#issuecomment-298624059
        /// Credit: https://codopia.wordpress.com/2017/07/21/how-to-fix-the-assembly-binding-redirect-problem-in-azure-functions/.
        /// </summary>
        /// <param name="bindingRedirect">The binding redirect.</param>
        private static void RedirectAssembly(BindingRedirect bindingRedirect)
        {
            ResolveEventHandler handler = null;
            handler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != bindingRedirect.ShortName)
                {
                    return null;
                }

                var targetPublicKeyToken = new AssemblyName("x, PublicKeyToken=" + bindingRedirect.PublicKeyToken).GetPublicKeyToken();
                requestedAssembly.SetPublicKeyToken(targetPublicKeyToken);
                requestedAssembly.Version = new Version(bindingRedirect.RedirectToVersion);
                requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;
                AppDomain.CurrentDomain.AssemblyResolve -= handler;
                return Assembly.Load(requestedAssembly);
            };
            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }
    }
}
