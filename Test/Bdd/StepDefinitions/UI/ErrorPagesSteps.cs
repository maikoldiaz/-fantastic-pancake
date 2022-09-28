// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorPagesSteps.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    public class ErrorPagesSteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see an Page Not Found error")]
        public void ThenIShouldSeeAnPageNotFoundError()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error404Page));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.Error404Page));
        }

        [Then(@"I should be able to See the support link")]
        public void ThenIShouldBeAbleToSeeTheSupportLink()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.SupportLink));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.SupportLink));
        }

        [Then(@"I should see Internal Error Page")]
        public void ThenIShouldSeeInternalErrorPage()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.Error500Page));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.Error500Page));
        }

        [Then(@"I should be able to click refresh")]
        public void ThenIShouldBeAbleToClickRefresh()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.RefreshPageLink));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.RefreshPageLink));
        }
    }
}
