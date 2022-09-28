// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginPage.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Executors
{
    using System;
    using System.Globalization;
    using global::Bdd.Core.Entities;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Ocaramba;
    using Ocaramba.Extensions;
    using Ocaramba.Types;

    using SmartFormat;

    public class LoginPage : ProjectPageBase
    {
        private const string OpeningPageStatus = "Opening page {0}";

        private readonly ElementLocator userIdLocator;
        private readonly ElementLocator passwordLocator;
        private readonly ElementLocator submitLocator;
        private readonly ElementLocator logoutLocator;
        private readonly ElementLocator nextLocator;
        ////private readonly ElementLocator signInWithPassword;

        public LoginPage(DriverContext driverContext)
            : base(driverContext)
        {
            this.userIdLocator = new ElementLocator(Locator.Name, "loginfmt");
            this.passwordLocator = new ElementLocator(Locator.Id, "passwordInput");
            this.nextLocator = new ElementLocator(Locator.XPath, "//input[@type='submit']");
            this.submitLocator = new ElementLocator(Locator.XPath, "//*[@id='submitButton']");
            ////this.signInWithPassword = new ElementLocator(Locator.XPath, "//a[@class='actionLink']");
            this.logoutLocator = new ElementLocator(Locator.Id, "logout");
        }

        public string Title => this.Driver.Title;

        public ProjectPageBase Login(Credentials user)
        {
            this.OpenLogin().SetCreds(user?.User, user?.Password); ////.Submit();
            this.Get<ElementPage>().ClientClick(this.submitLocator, true);
            this.Get<ElementPage>().ClientClick(this.nextLocator, true);
            return this;
        }

        public void Logout()
        {
            this.Get<ElementPage>().ClientClick(this.Driver.GetElement(this.logoutLocator).ToWrappedElement());
        }

        public void LoginAgainWith(string userId, string password)
        {
            this.SetCreds(userId, password).Submit();
        }

#pragma warning disable CA1801 // Remove unused parameter
#pragma warning disable IDE0060 // Remove unused parameter
        public LoginPage SetCreds(string userId, string password)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1801 // Remove unused parameter
        {
            var user = this.Driver.GetElement(this.userIdLocator, BaseConfiguration.MediumTimeout);
            user?.Clear();
            user?.SendKeys(userId);

            this.Get<ElementPage>().ClientClick(this.nextLocator, true);
            ////var signInPasswordLnk = this.Driver.GetElement(this.signInWithPassword, BaseConfiguration.MediumTimeout);
            ////signInPasswordLnk.Click();
            try
            {
                var pwd = this.Driver.GetElement(this.passwordLocator, BaseConfiguration.MediumTimeout);
                pwd?.Clear();
                Console.WriteLine(password);
                pwd?.SendKeys(password);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Do nothing
                // Password field will not be present when running from local box due to single-sign-on
            }

            return this;
        }

        private LoginPage OpenLogin()
        {
            var url = Smart.Format(CultureInfo.InvariantCulture, BaseConfiguration.GetUrlValue);
            this.DriverContext.NavigateToAndMeasureTime(new Uri(url), true);
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Logger.Info(CultureInfo.CurrentCulture, OpeningPageStatus, url);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            return this;
        }

        private void Submit()
        {
            this.Get<ElementPage>().ClientClick(this.submitLocator, true);
        }
    }
}