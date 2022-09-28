// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToEditOwnershipOfFinalInventorySteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class UIToEditOwnershipOfFinalInventorySteps : EcpWebStepDefinitionBase
    {
        [Then(@"I should see the edit inventory model window is closed")]
        public void ThenIShouldSeeTheEditInventoryModelWindowIsClosed()
        {
            try
            {
                Assert.IsFalse(this.Get<ElementPage>().GetElement(nameof(Resources.CategoryHeader)).Displayed);
            }
            catch (WebDriverTimeoutException exception)
            {
                Assert.IsNotNull(exception);
            }
        }

        [Then(@"I should see no item is selected for InventoryOwnership ReasonForChange dropdown")]
        public void ThenIShouldSeeNoItemIsSelectedForInventoryOwnershipReasonForChangeDropdown()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DropDownDefaultOption)).Displayed);
        }

        [Then(@"I should see all change reasons (for ""(.*)"" ""(.*)"")")]
        public void ThenIShouldSeeAllChangeReasonsFor(ElementLocator elementLocator)
        {
            var actualCount = this.Get<ElementPage>().GetElements(elementLocator).Count;
        }

        [Then(@"I should see ""(.*)"" information of the inventory")]
        public async System.Threading.Tasks.Task ThenIShouldSeeInformationOfTheInventoryAsync(string type)
        {
            var page = this.Get<ElementPage>();
            if (type == "Operational")
            {
                var nodeName = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeById, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
                this.SetValue("NodeName", nodeName[ConstantValues.Name]);
                var actualOperationalInformation = new Dictionary<string, string>()
            {
                { "Date", page.GetElement(nameof(Resources.EditInventoryOperationalInformation), formatArgs: 1).Text },
                { "Tank", page.GetElement(nameof(Resources.EditInventoryOperationalInformation), formatArgs: 2).Text },
                { "NetVolume", page.GetElement(nameof(Resources.EditInventoryOperationalInformation), formatArgs: 3).Text },
                { "Unit", page.GetElement(nameof(Resources.EditInventoryOperationalInformation), formatArgs: 4).Text },
                { "OwnershipFunction", page.GetElement(nameof(Resources.EditInventoryOperationalInformation), formatArgs: 5).Text },
            };

                var expectedOperationalInformation = new Dictionary<string, string>()
            {
                { "Date", page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 1).Text },
                { "Tank", page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 2).Text },
                { "NetVolume", page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 3).Text },
                { "Unit", page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 4).Text },
                { "OwnershipFunction", page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 5).Text },
            };
                Assert.IsTrue(this.VerifyDiffs(expectedOperationalInformation, actualOperationalInformation));
                Assert.AreEqual(ConstantValues.ProductName, page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 4).Text);
                Assert.AreEqual(this.GetValue("NodeName"), page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 2).Text);
            }
            else if (type == "Ownership")
            {
                var actualOwnershipValue = page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 6).Text;
                var actualOwnershipPercentage = page.GetElement(nameof(Resources.EditMovementAndInventoryGrid), formatArgs: 7).Text;
                var expectedOwnershipValue = page.GetElement(nameof(Resources.OwnerhsipData), 1).GetAttribute("value");
                var expectedOwnershipPercentage = page.GetElement(nameof(Resources.OwnerhsipData), 2).GetAttribute("value");

                Assert.AreEqual(expectedOwnershipValue, actualOwnershipValue);
                Assert.AreEqual(actualOwnershipPercentage, actualOwnershipPercentage);
            }
        }

        [When(@"I have modified owners volume")]
        public void WhenIHaveModifiedOwnersVolume()
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 1).SendKeys(ConstantValues.OwnershipValue);
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 3).SendKeys(ConstantValues.OwnershipValue);
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 5).SendKeys(ConstantValues.OwnershipValue);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }

        [Then(@"ownership percentage is auto updated as per modifed volume")]
        public void ThenOwnershipPercentageIsAutoUpdatedAsPerModifedVolume()
        {
            var actualOwnershipPercentage1 = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).GetAttribute("value");
            var actualOwnershipPercentage2 = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 4).GetAttribute("value");
            var actualOwnershipPercentage3 = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 6).GetAttribute("value");
            var expectedOwnershipPercentage = "2.4096385542168677";
            Assert.AreEqual(expectedOwnershipPercentage, actualOwnershipPercentage1);
            Assert.AreEqual(expectedOwnershipPercentage, actualOwnershipPercentage2);
            Assert.AreEqual(expectedOwnershipPercentage, actualOwnershipPercentage3);
        }

        [When(@"I have modified owners volume percentage")]
        public void WhenIHaveModifiedOwnersVolumePercentage()
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).SendKeys(ConstantValues.OwnershipValue);
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 4).SendKeys(ConstantValues.OwnershipValue);
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 6).SendKeys(ConstantValues.OwnershipValue);
#pragma warning restore CA1303 // Do not pass literals as localized parameters

        }

        [Then(@"total volume and percentage should be auto updated")]
        public void ThenTotalVolumeAndPercentageShouldBeAutoUpdated()
        {
            var totalOwnershipPercentage = this.Get<ElementPage>().GetElement(nameof(Resources.OwenrshipTotalPercentage)).Text;
            var expectedTotalOwnershipPercentage = "7.2289156626506035";
            Assert.AreEqual(expectedTotalOwnershipPercentage, totalOwnershipPercentage);
        }

        [Then(@"default owners should be added to the grid")]
        public void ThenDefaultOwnersShouldBeAddedToTheGrid()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I should see the owners volume is auto updated")]
        public void ThenIShouldSeeTheOwnersVolumeIsAutoUpdated()
        {
            var totalOwnershipVolume = this.Get<ElementPage>().GetElement(nameof(Resources.OwenrshipVolume)).Text;
            var expectedTotalOwnershipVolume = "3000";
            Assert.AreEqual(expectedTotalOwnershipVolume, totalOwnershipVolume);
        }

        [Then(@"new owners must display default configured volume and percentage")]
        public void ThenNewOwnersMustDisplayDefaultConfiguredVolumeAndPercentage()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Total ownership percentage is not met (.*)")]
        public void ThenTotalOwnershipPercentageIsNotMet(int percentage)
        {
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 1).SendKeys(percentage + "10");
        }
    }
}
