// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationAndAuthorizationSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Security
{
    using System;
    using System.Globalization;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;

    using Flurl;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Ocaramba;

    using SmartFormat;

    using TechTalk.SpecFlow;

    [Binding]
    public class AuthenticationAndAuthorizationSteps : EcpWebStepDefinitionBase
    {
        [When(@"I click on Administration tab")]
        public void WhenIClickOnAdministrationTab()
        {
            var elementPage = this.Get<ElementPage>();
            elementPage.WaitUntilPageLoad();
            elementPage.WaitUntilElementToBeClickable(nameof(Resources.NavigationBar));
            elementPage.Click(nameof(Resources.NavigationBar));
            elementPage.WaitUntilElementToBeClickable(nameof(Resources.Administration), 5);
            elementPage.Click(nameof(Resources.Administration));
        }

        [When(@"I click on conveyor balance with property menu")]
        public void WhenIClickOnConveyorBalanceWithPropertyMenu()
        {
            var elementPage = this.Get<ElementPage>();
            elementPage.WaitUntilElementToBeClickable(nameof(Resources.BalanceTransportersWithProperty), 5);
            elementPage.Click(nameof(Resources.BalanceTransportersWithProperty));
        }

        [Then(@"I should see ""(.*)"" tab")]
        public void ThenIShouldSeeTab(string link)
        {
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.Module), formatArgs: UIContent.Conversion[link]);
        }

        [Then(@"I should not see ""(.*)"" tab")]
        public void ThenIShouldNotSeeTab(string link)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Module), formatArgs: UIContent.Conversion[link]);
        }

        [When(@"I hit the ""(.*)"" URL directly")]
        public void WhenIHitTheUrlDirectly(string page)
        {
            var url = Smart.Format(CultureInfo.InvariantCulture, BaseConfiguration.GetUrlValue);
            url = Url.Combine(url, UIContent.DirectPageUrl[page]);
            this.Get<UrlPage>().NavigateToUrl(url);
        }

        [Then(@"I should see an unauthorized page")]
        public void ThenIShouldSeeAnUnauthorizedPage()
        {
            var elementPage = this.Get<ElementPage>();
            elementPage.WaitUntilElementIsVisible(nameof(Resources.ErrorPage));
            elementPage.CheckIfElementIsPresent(nameof(Resources.ErrorPage));
        }
    }
}
