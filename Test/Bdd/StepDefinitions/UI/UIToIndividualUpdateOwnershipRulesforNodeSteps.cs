// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToIndividualUpdateOwnershipRulesforNodeSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using TechTalk.SpecFlow;

    [Binding]
    public class UIToIndividualUpdateOwnershipRulesforNodeSteps : EcpWebStepDefinitionBase
    {
        [When(@"I click (on "".*"" "".*"" "".*"") present at end of record")]
        public void WhenIClickOnPresentAtEndOfRecord(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.GetElements(elementLocator).ElementAt(0).Click();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        [When(@"I see ""(.*)"" header in page")]
        public void WhenISeeHeaderinpage(string header)
        {
            var page = this.Get<ElementPage>();
            this.ScenarioContext[ConstantValues.CurrentOwnershipstrategyonPage] = page.GetElements(nameof(Resources.NodeOwnershipstrategyonPage)).ElementAt(0).Text;
            page.WaitUntilElementIsVisible(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion[header]);
            page.CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: UIContent.Conversion[header]);
        }

        [When(@"I should see current ownership strategy on the interface")]
        public void WhenIShouldSeeCurrentOwnershipStrategyOnTheInterface()
        {
            this.ScenarioContext["currentOwnersipStrategyinInterface"] = this.Get<ElementPage>().GetElement(nameof(Resources.CurrentOwnershipStrategyinInterface)).Text;
            this.VerifyThat(() => Assert.AreEqual(this.ScenarioContext["currentOwnershipstrategyonPage"], this.ScenarioContext["currentOwnersipStrategyinInterface"]));
        }

        [Then(@"I should not see old ownership strategy in ""(.*)""")]
        public void ThenIShouldNotSeeOldOwnershipStrategyIn(string field)
        {
            var value = this.ScenarioContext["currentOwnersipStrategyinInterface"];
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.EnterStrategyinInterface), formatArgs: value));
        }

        [When(@"I select New ownership strategy in ""(.*)""")]
        public async Task WhenISelectNewOwnershipStrategyInAsync(string field)
        {
            var rows = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.NodeOwnershipRuleStrategies).ConfigureAwait(false);
            var rowList = rows.ToDictionaryList();
            foreach (var data in rowList)
            {
                if (!data[ConstantValues.RuleName].Equals(this.ScenarioContext["currentOwnersipStrategyinInterface"]))
                {
                    this.SetValue("NewOwnershipStrategy", data[ConstantValues.RuleName]);
                    break;
                }
            }

            var value = this.GetValue("NewOwnershipStrategy");
            this.SetValue(Entities.Keys.SelectedValue, value);
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            //// this.Get<ElementPage>().Click(nameof(Resources.SelectBox), formatArgs: value);
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.EnterStrategyinInterface), 5, formatArgs: value);
            var option = this.Get<ElementPage>().GetElement(nameof(Resources.EnterStrategyinInterface), formatArgs: value);
            Actions action = new Actions(this.DriverContext.Driver);
            action.MoveToElement(option).Perform();
            option.Click();
        }

        [Then(@"I see record in the grid are updated with New ownership strategy")]
        public void ThenISeeRecordInTheGridAreUpdatedWithNewOwnershipStrategy()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var updatedownershipstrategy = this.Get<ElementPage>().GetElements(nameof(Resources.NodeOwnershipstrategyonPage)).ElementAt(0).Text;
            Assert.AreEqual(this.GetValue("NewOwnershipStrategy"), updatedownershipstrategy);
        }

        [Then(@"I should see the title as ""(.*)""")]
        public void ThenIShouldSeeTheTitleAs(string expectedTitle)
        {
            var actualTitle = this.Get<ElementPage>().GetElement(nameof(Resources.RuleSynchronizerDetails), formatArgs: ConstantValues.RuleSynchronizerTitle).Text;
            Assert.AreEqual(expectedTitle, actualTitle);
        }

        [Then(@"I should see an informational message as ""(.*)""")]
        public void ThenIShouldSeeAnInformationalMessageAs(string expectedMessage)
        {
            var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.RuleSynchronizerDetails), formatArgs: ConstantValues.RuleSynchronizerMessage).Text;

            try
            {
                Assert.AreEqual(expectedMessage, actualMessage);
            }
            catch (WebDriverTimeoutException message)
            {
                Assert.Fail(expectedMessage, message.ToString());
            }
        }

        [StepDefinition(@"Icon should be changed to the ""(.*)"" mode")]
        public void ThenIconShouldBeChangedToTheMode(string state)
        {
            try
            {
                IWebElement element = this.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.RuleSynchronizerstate), formatArgs: state);
                Assert.IsTrue(element.Displayed);
            }
            catch (NoSuchElementException e)
            {
                Assert.IsNotNull(e);
            }
        }

        [Then(@"I should see an informational message as ""(.*)"" while the process is running")]
        public void ThenIShouldSeeAnInformationalMessageAsWhileTheProcessIsRunning(string expactedMessage)
        {
            var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.RuleSynchronizerDetails), formatArgs: ConstantValues.RuleSynchronizerMessage).Text;
            Assert.AreEqual(expactedMessage, actualMessage);
        }

        [When(@"I get the detials from OwnershipRuleRefreshHistory Table")]
        public async Task WhenIGetTheDetialsFromOwnershipRuleRefreshHistoryTableAsync()
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastUpdatedRuleRefreshHistory).ConfigureAwait(false);
            this.ScenarioContext["lastRefreshHistorybeforeInvokingFico"] = int.Parse(lastCreatedRow[ConstantValues.OwnershipRuleRefreshHistoryId], CultureInfo.InvariantCulture) + 1;
        }

        [Then(@"the strategy cache should be updated by invoking the FICO service when the process is completed")]
        public async Task ThenTheStrategyCacheShouldBeUpdatedByInvokingTheFICOServiceWhenTheProcessIsCompletedAsync()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.RuleSynchronizerstate), formatArgs: ConstantValues.Information);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastUpdatedRuleRefreshHistory).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext["lastRefreshHistorybeforeInvokingFico"], int.Parse(lastCreatedRow[ConstantValues.OwnershipRuleRefreshHistoryId], CultureInfo.InvariantCulture));
        }

        [Then(@"wait for the ""(.*)"" sec after the Fico Invocation completed with success")]
        public void ThenWaitForTheSecAfterTheFicoInvocationCompletedWithSuccess(int waitTime)
        {
            this.LogToReport("Intentionally waiting for 10 sec as per the requirement");
            TimeSpan ts = TimeSpan.FromSeconds(waitTime);
            Thread.Sleep(ts);
        }

        [Then(@"I should (see "".*"" "".*"" "".*"") button as enabled")]
        public void ThenIShouldSeeButtonAsEnabled(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsTrue(page.GetElements(elementLocator).ElementAt(0).Enabled);
        }

        [StepDefinition(@"I update the OwnershipRuleRefreshHistory Table and set the status of last record as ""(.*)""")]
        public async Task WhenIUpdateTheOwnershipRuleRefreshHistoryTableAndSetstatusofTheLastRecordAsAsync(int state)
        {
            await this.ReadAllSqlAsync(input: SqlQueries.UpdateLastCreatedRefreshHistoryStatus, args: new { status = state }).ConfigureAwait(false);
        }

        [Then(@"the system must use the ""(.*)"" collection of the cached service response to display the list of ownership strategies per node")]
        public async Task ThenTheSystemMustUseTheCollectionOfTheCachedServiceResponseToDisplayTheListOfOwnershipStrategiesPerNodeAsync(string collection)
        {
            ////this.Given("I have Fico Connection setup into the system");
            await this.IHaveFicoConnectionSetupIntoTheSystemAsync().ConfigureAwait(false);
            ////this.When("I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: \"BUSCA_ESTRATEGIA\" and estado: \"true\"");
            await this.IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync("BUSCA_ESTRATEGIA", "true").ConfigureAwait(false);
            ////this.Then(" Validate that response are successfully loading into the table at \"Node\" Level for which id is " + collecion);
            await this.ValidateThatResponseAreSuccessfullyLoadingIntoTheTableAtLevelForWhichIdIsAsync("Node", collection).ConfigureAwait(false);
        }

        [Then(@"I should (see "".*"" "".*"" "".*"") button as disabled")]
        public void ThenIShouldSeeButtonAsDisabled(ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsFalse(page.GetElement(elementLocator).Enabled);
        }

        [When(@"I open the new tab and navigate to the screen")]
        public void WhenIOpenTheNewTabAndNavigateToTheScreen()
        {
            var currentDriver = this.DriverContext.Driver;
            var currentURL = currentDriver.Url;
            var jscript = currentDriver as IJavaScriptExecutor;
            jscript.ExecuteScript("window.open()");
            var tabs = currentDriver.WindowHandles;
            currentDriver.SwitchTo().Window(tabs[1]);
            this.DriverContext.NavigateToAndMeasureTime(new Uri(currentURL), true);
        }
    }
}