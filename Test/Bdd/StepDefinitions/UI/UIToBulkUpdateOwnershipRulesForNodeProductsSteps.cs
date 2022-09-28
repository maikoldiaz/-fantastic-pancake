// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToBulkUpdateOwnershipRulesForNodeProductsSteps.cs" company="Microsoft">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class UIToBulkUpdateOwnershipRulesForNodeProductsSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"the record count in Grid shown per page should be (.*)")]
        public void ThenTheRecordsCountInGridShownPerPageShouldBe(int recordCount)
        {
            var paginationCount = this.Get<ElementPage>().GetElement(nameof(Resources.PaginationCount)).Text;
            var count = paginationCount.Split(' ');

            Assert.AreEqual(recordCount.ToString(CultureInfo.InvariantCulture), count[2]);
        }

        [When(@"I select all records from grid")]
        public void WhenISelectAllRecordsFromGrid()
        {
            var page = this.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.WaitUntilElementToBeClickable(nameof(Resources.SelectAllCheckBox));
            page.Click(nameof(Resources.SelectAllCheckBox));
        }

        [Then(@"I see confirmation dialog with ""(.*)"" records and ownership strategy information")]
        public void WhenISeeConfirmationDialogWithRecordsAndOwnershipStrategyInformation(int number)
        {
            if (this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Contains(','))
            {
                Assert.AreEqual(number, int.Parse(this.Get<ElementPage>().GetElementText(nameof(Resources.NumberOfNodesOnTheConfirmationPopUp)), CultureInfo.InvariantCulture));
            }
        }

        [StepDefinition(@"I see button name ""(.*)"" in confirmation window")]
        public void ThenISeeButtonNameAs(string buttonName)
        {
            if (this.GetValue(ConstantValues.OldOwnershipStrategyInformation).Contains(','))
            {
                var actualButton = this.Get<ElementPage>().GetElementText(nameof(Resources.ButtonsOnCreateLogisticInterface), formatArgs: UIContent.ReportLogisticGrid[ConstantValues.Node]);
                Assert.IsTrue(buttonName.EqualsIgnoreCase(actualButton));
            }
        }

        [StepDefinition(@"I see message as ""(.*)"" (in "".*"" "".*"")")]
        public void ThenISeeMessageIn(string message, ElementLocator elementLocator)
        {
           var actualMeassage = this.Get<ElementPage>().GetElement(elementLocator).Text;
           Assert.AreEqual(message, actualMeassage);
        }

        [Then(@"I see strategies separated by comma on the interface")]
        public void ThenISeeStrategiesSeparatedByCommaOnTheInterface()
        {
            Assert.AreEqual(this.GetValue(ConstantValues.OldOwnershipStrategyInformation), this.Get<ElementPage>().GetElementText(nameof(Resources.OldOwnershipStrategiesInformation)));
        }

        [Then(@"I select ownership strategy (from "".*"" "".*"" "".*"")")]
        public async Task ThenISelectOwnershipStrategyFromAsync(ElementLocator elementLocator)
        {
            try
            {
                var ownershipStrategyInformation = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeProductOwnershipStrategyInformation).ConfigureAwait(false);
                this.SetValue(ConstantValues.OwnershipStrategyInformation, ownershipStrategyInformation[ConstantValues.OwnershipStrategyInformation]);
                this.Get<ElementPage>().Click(elementLocator);
                this.Get<ElementPage>().Click(nameof(Resources.UploadType), this.GetValue(ConstantValues.OwnershipStrategyInformation), this.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), this.GetValue(ConstantValues.OwnershipStrategyInformation)).Count);
            }
            catch (NullReferenceException)
            {
                Logger.Info("There are no ownership rules in the System");
                Assert.Fail("There are no ownership rules in the System");
            }
        }

        [Then(@"I see ""(.*)"" records in the grid with new ownership strategy")]
        public void ThenISeeRecordsInTheGridWithNewOwnershipStrategy(int recordCount)
        {
            this.WhenIGetOwnershipStrategiesForRecorsInTheGrid(recordCount);
            Assert.AreEqual(this.GetValue(ConstantValues.OwnershipStrategyInformation), this.GetValue(ConstantValues.OldOwnershipStrategyInformation));
        }

        [When(@"I get ownership strategies for ""(.*)"" records in the grid")]
        public void WhenIGetOwnershipStrategiesForRecorsInTheGrid(int numberOfRecords)
        {
            string oldOwnershipStrategy = string.Empty;
            for (int i = 1; i <= numberOfRecords; i++)
            {
                var arguments = new object[] { i, 6 };
                var actualOwnershipStrategyPerNode = this.Get<ElementPage>().GetElement(nameof(Resources.GridColumnDetails), formatArgs: arguments).Text;

                if (!string.IsNullOrEmpty(actualOwnershipStrategyPerNode) && !oldOwnershipStrategy.Contains(actualOwnershipStrategyPerNode))
                {
                    oldOwnershipStrategy += actualOwnershipStrategyPerNode + ", ";
                }
            }

            this.SetValue(ConstantValues.OldOwnershipStrategyInformation, oldOwnershipStrategy.TrimEnd(',', ' '));
        }

        [StepDefinition(@"I input value (in "".*"" "".*"" "".*"")")]
        public void WhenIEnterValueIn(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(ConstantValues.EcoPetrol);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [StepDefinition(@"I get last updated deatils from ownershipRuleRefreshHistory table")]
        public async Task WhenIGetDeatilsFromOwnershipRuleRefreshHistoryTableAsync()
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastUpdatedRuleRefreshHistory).ConfigureAwait(false);
            this.ScenarioContext["lastRefreshHistorybeforeInvokingFico"] = int.Parse(lastCreatedRow[ConstantValues.OwnershipRuleRefreshHistoryId], CultureInfo.InvariantCulture) + 1;
        }

        [StepDefinition(@"strategy cache should be updated by invoking the FICO service")]
        public async Task ThenTheStrategyCacheShouldBeUpdatedByInvokingTheFICOServiceAsync()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.RuleSynchronizerstate), formatArgs: ConstantValues.Information);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastUpdatedRuleRefreshHistory).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext["lastRefreshHistorybeforeInvokingFico"], Convert.ToInt32(lastCreatedRow[ConstantValues.OwnershipRuleRefreshHistoryId], CultureInfo.InvariantCulture));
        }

        [StepDefinition(@"I see record is updated with new strategy")]
        public void ThenISeeRecordInTheGridAreUpdatedWithNewOwnershipStrategy()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            var updatedownershipstrategy = this.Get<ElementPage>().GetElements(nameof(Resources.NodeOwnershipstrategyonPage)).ElementAt(0).Text;
            this.VerifyThat(() => Assert.AreEqual(this.GetValue("NewOwnershipStrategy"), updatedownershipstrategy));
        }

        [StepDefinition(@"I should not see old ownership strategy in ""(.*)"" options")]
        public void ThenIShouldNotSeeOldOwnershipStrategyIn(string field)
        {
            var value = this.ScenarioContext["currentOwnersipStrategyinInterface"];
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.EnterStrategyinInterface), formatArgs: value));
        }

        [StepDefinition(@"wait for the 10 sec for the Fico Invocation to complete successfully")]
        public void ThenWaitForTheSecForTheFicoInvocationToCompleteSuccessfully()
        {
            Thread.Sleep(10000);
        }

        [StepDefinition(@"system must use ""(.*)"" collection of the cached service response that invokes FICO")]
        public async Task ThenSystemMustUseCollectionOfTheCachedServiceResponseThatInvokesFICOAsync(string collection)
        {
            ////this.Given("I have Fico Connection setup into the system");
            await this.IHaveFicoConnectionSetupIntoTheSystemAsync().ConfigureAwait(false);
            ////this.When("I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: \"BUSCA_ESTRATEGIA\" and estado: \"true\"");
            await this.IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync("BUSCA_ESTRATEGIA", "true").ConfigureAwait(false);
            ////this.Then("Validate that response are successfully loading into the table at \"Node\" Level for which id is " + collection);
            await this.ValidateThatResponseAreSuccessfullyLoadingIntoTheTableAtLevelForWhichIdIsAsync("Node", collection).ConfigureAwait(false);
        }

        [StepDefinition(@"all the strategies should be displayed in destination list dropdown")]
        public void ThenAllTheStrategiesShouldBeDisplayedInDestinationListDropdown()
        {
            var value = this.Get<ElementPage>().GetElementText(nameof(Resources.OldOwnershipStrategiesInformation));
            var values = value.Split(',');
            foreach (string oldValue in values)
            {
            var dropDownIndex = int.Parse(this.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), oldValue);
            dropDownBox[dropDownIndex - 1].Click();
            Assert.IsTrue(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.EnterStrategyinInterface), formatArgs: value));
            }
        }

        [StepDefinition(@"I should see ""(.*)"" error message")]
        public void ThenIShouldSeeErrorMessage(string expectedMessage)
        {
            var actualErrorMessage = this.Get<ElementPage>().GetElement(nameof(Resources.FailedErrorMessage)).Text;
            Assert.AreEqual(expectedMessage, actualErrorMessage);
        }
    }
}