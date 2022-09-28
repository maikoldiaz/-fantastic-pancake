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

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Globalization;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

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
            this.Get<ElementPage>().WaitUntilPageLoad();
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().Click(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.Administration), 5);
            this.Get<ElementPage>().Click(nameof(Resources.Administration));
        }

        [When(@"I click on conveyor balance with property menu")]
        public void WhenIClickOnConveyorBalanceWithPropertyMenu()
        {
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.BalanceTransportersWithProperty), 5);
            this.Get<ElementPage>().Click(nameof(Resources.BalanceTransportersWithProperty));
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

        [StepDefinition(@"I hit the ""(.*)"" URL directly")]
        public void WhenIHitTheURLDirectly(string page)
        {
            var url = Smart.Format(CultureInfo.InvariantCulture, BaseConfiguration.GetUrlValue);
            url = Url.Combine(url, UIContent.DirectPageUrl[page]);
            this.DriverContext.NavigateToAndMeasureTime(new Uri(url), true);
        }

        [Then(@"I should see an unauthorized page")]
        public void ThenIShouldSeeAnUnauthorizedPage()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorPage));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ErrorPage));
        }
    }
}
