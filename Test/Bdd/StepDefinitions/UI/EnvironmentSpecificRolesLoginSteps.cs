// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentSpecificRolesLoginSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    public class EnvironmentSpecificRolesLoginSteps : WebStepDefinitionBase
    {
        [Given(@"I have any environment specific role defined")]
        public void GivenIHaveAnyEnvironmentSpecificRoleDefined()
        {
            this.LogToReport("Configured at application level");
        }

        [Given(@"I have other environment specific role defined")]
        public void GivenIHaveOtherEnvironmentSpecificRoleDefined()
        {
            this.LogToReport("Configured at application level");
        }

        [When(@"I try to login with that defined role")]
        public void WhenITryToLoginWithThatDefinedRole()
        {
            ////this.Given("I am logged in as \"nonprodrole\"");
            this.LoggedInAsUser("nonprodrole");
        }

        [When(@"I try to login with other environment specific role")]
        public void WhenITryToLoginWithOtherEnvironmentSpecificRole()
        {
            ////this.Given("I am logged in as \"prodrole\"");
            this.LoggedInAsUser("nonprodrole");
        }

        [Then(@"the login should be successful")]
        public void ThenTheLoginShouldBeSuccessful()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.NavigationBar));
        }

        [Then(@"the login should not be successful")]
        public void ThenTheLoginShouldNotBeSuccessful()
        {
            this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.ErrorPage));
            this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ErrorPage));
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.NavigationBar));
        }
    }
}
