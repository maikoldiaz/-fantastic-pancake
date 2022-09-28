// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToCreateAMovementWithOwnershipInANodeSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class UIToCreateAMovementWithOwnershipInANodeSteps : EcpWebStepDefinitionBase
    {
        [When(@"I selected node from either source node or destination node based on input variable")]
        public async Task WhenISelectedNodeFromEitherSourceNodeOrDestinationNodeBasedOnInputVariableAsync()
        {
            await this.ISelectedNodeFromEitherSourceNodeOrDestinationNodeBasedOnInputVariableAsync().ConfigureAwait(false);
        }

        [When(@"I selected ""(.*)"" (from "".*"" "".*"")")]
        public void WhenISelectedFrom(string inputValue, ElementLocator elementLocator)
        {
            this.ISelectedValueFrom(inputValue, elementLocator);
        }

        [When(@"I enter volume (into "".*"" "".*"" "".*"")")]
        public void WhenIEnterVolumeInto(ElementLocator elementLocator)
        {
            this.IEnterVolumeInto(elementLocator);
        }

        [When(@"I select node (from "".*"" "".*"") on create movement interface")]
        public async Task WhenISelectedValueOnCreateMovementFormAsync(ElementLocator elementLocator)
        {
            await this.ISelectedValueOnCreateMovementFormAsync(elementLocator).ConfigureAwait(false);
        }

        [When(@"I selected node (from "".*"" "".*"") on create movement interface")]
        public async Task WhenISelectedValueOnCreateMovementInterfaceAsync(ElementLocator elementLocator)
        {
            await this.ISelectedValueOnCreateMovementInterfaceAsync(elementLocator).ConfigureAwait(false);
        }

        [When(@"I selected product from either source product or destination product")]
        public void WhenISelectedProductFromEitherSourceProductOrDestinationProduct()
        {
            ////this.When("I selected \"product\" from \"CreateMovement\" \"sourceProduct\" \"combobox\"");
            this.ISelectedValueFrom("product", "CreateMovement\" \"sourceProduct", "combobox");
        }

        [When(@"all input validations met on create movement interface")]
        public async Task WhenAllInputValidationsMetOnCreateMovementInterfaceAsync()
        {
            ////this.When("I enter volume into \"decimal\" \"netVolume\" \"textbox\"");
            this.IEnterVolumeInto("decimal\" \"netVolume", "textbox");
            ////this.When("I selected \"Bbl\" from \"CreateMovement\" \"unit\" \"combobox\"");
            this.ISelectedValueFrom("Bbl", "CreateMovement\" \"unit", "combobox");
            ////this.When("I selected \"Tolerance​\" from \"CreateMovement\" \"variable\" \"combobox\"");
            this.ISelectedValueFrom("Tolerance", "CreateMovement\" \"variable", "combobox");
            ////this.When("I select node from \"CreateMovement\" \"SourceNode\" \"combobox\" on create movement interface");
            await this.ISelectedValueOnCreateMovementFormAsync("CreateMovement\" \"SourceNode", "combobox").ConfigureAwait(false);
            ////this.When("I selected \"product\" from \"CreateMovement\" \"sourceProduct\" \"combobox\"");
            this.ISelectedValueFrom("product", "CreateMovement\" \"sourceProduct", "combobox");
            ////this.When("I selected \"Tolerance​\" from \"CreateMovement\" \"movementType\" \"combobox\"");
            this.ISelectedValueFrom("Tolerance", "CreateMovement\" \"movementType", "combobox");
            this.Get<ElementPage>().ScrollIntoView(nameof(Resources.CommentsTextBoxOnCreateMovement));
            ////this.When("I selected \"reasonForChange​\" from \"CreateMovement\" \"reasonForChange\" \"combobox\"");
            this.ISelectedValueFrom("reasonForChange", "CreateMovement\" \"reasonForChange", "combobox");
            ////this.When("I enter morethan 20 characters into \"comments\" \"textbox\"");
            this.IEnterMorethanCharactersInto(20, "comments", "textbox");
        }

        [When(@"input validations met on create movement interface except total ownership percentage")]
        public async Task WhenInputValidationsMetOnCreateMovementInterfaceExceptAsync()
        {
            await this.WhenAllInputValidationsMetOnCreateMovementInterfaceAsync().ConfigureAwait(false);
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).Clear();
            this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).SendKeys("2");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }

        [When(@"node is not blocked by other users")]
        public async Task WhenNodeIsNotBlockedByOtherUsersAsync()
        {
            var nodeStatus = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOwnershipNodeStatus, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            Assert.AreNotEqual(ConstantValues.Bloqueado, nodeStatus[ConstantValues.Name]);
        }

        [Then(@"date should be end date of ownership period")]
        public void ThenDateShouldBeEndDateOfOwnershipPeriod()
        {
            var page = this.Get<ElementPage>();
            var expectedDate = page.GetElement(nameof(Resources.EditOwnershipLabelsInformation), formatArgs: 3).Text.Split('l')[1];
            var actualDateOnCreateMovement = page.GetElement(nameof(Resources.DatePickerOnCreateMovement)).GetAttribute("value");
            Assert.AreEqual(expectedDate, actualDateOnCreateMovement.Split('/')[0] + "-" + UIContent.Conversion[actualDateOnCreateMovement.Split('/')[1]].Remove(3) + "-" + actualDateOnCreateMovement.Split('/')[2].Substring(2));
        }

        [Then(@"""(.*)"" should be loaded with their corresponding values")]
        public async Task ThenVariableShouldBeLoadedWithTheirCorrespondingValuesAsync(string field)
        {
            switch (field)
            {
                case "variable":
                    var expectedVariablesList = new List<string>()
                    { "Interfase", "Tolerancia", "Pérdida No Identificada", "Entrada", "Salida", "Pérdida Identificada" };
                    ////this.When("I click on \"CreateMovement\" \"variable\" \"combobox\"");
                    this.IClickOn("CreateMovement\" \"variable", "combobox");
                    var actualvariables = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedVariablesList.Count, actualvariables.Count);
                    for (int i = 0; i < actualvariables.Count; i++)
                    {
                        Assert.IsTrue(expectedVariablesList[i] == actualvariables[i]);
                    }

                    break;
                case "unit":
                    var expectedUnit = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllUnits).ConfigureAwait(false);
                    var expectedUnitList = expectedUnit.ToDictionaryList();
                    ////this.When("I click on \"CreateMovement\" \"unit\" \"combobox\"");
                    this.IClickOn("CreateMovement\" \"unit", "combobox");
                    var actualUnits = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedUnitList.Count, actualUnits.Count);
                    for (int i = 0; i < actualUnits.Count; i++)
                    {
                        Assert.IsTrue(expectedUnitList[i][ConstantValues.Name].Equals(actualUnits[i]));
                    }

                    break;
                case "movement type":
                    var expectedMovementType = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllMovementTypes).ConfigureAwait(false);
                    var expectedMovementTypeList = expectedMovementType.ToDictionaryList();
                    ////this.When("I click on \"CreateMovement\" \"movementType\" \"combobox\"");
                    this.IClickOn("CreateMovement\" \"movementType", "combobox");
                    var actualMovementTypes = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedMovementTypeList.Count, actualMovementTypes.Count);
                    for (int i = 0; i < actualMovementTypes.Count; i++)
                    {
                        Assert.IsTrue(expectedMovementTypeList[i][ConstantValues.Name].Equals(actualMovementTypes[i]));
                    }

                    break;
                case "reason for change":
                    var expectedReasonForChange = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllReasonForChanges).ConfigureAwait(false);
                    var expectedReasonForChangeList = expectedReasonForChange.ToDictionaryList();
                    this.Get<ElementPage>().Click(nameof(Resources.DestinationProductsOnCreateMovementInterface));
                    ////this.When("I click on \"CreateMovement\" \"reasonForChange\" \"combobox\"");
                    this.IClickOn("CreateMovement\" \"reasonForChange", "combobox");
                    var actualReasonForChanges = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                    await Task.Delay(3000).ConfigureAwait(false);
                    Assert.AreEqual(expectedReasonForChangeList.Count, actualReasonForChanges.Count);
                    for (int i = 0; i < actualReasonForChanges.Count; i++)
                    {
                        Assert.IsTrue(expectedReasonForChangeList[i][ConstantValues.Name].Equals(actualReasonForChanges[i]));
                    }

                    break;
                default:
                    break;
            }
        }

        [Then(@"""(.*)"" list must contain the nodes of the input connections to the current node")]
        public async Task ThenSourceNodeListMustContainTheNodesOfTheInputConnectionsToTheCurrentNodeAsync(string field)
        {
            var expectedSourceNode = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllInputConnectionsToNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            var expectedSourceNodeList = expectedSourceNode.ToDictionaryList();
            ////this.When("I click on \"CreateMovement\" \"sourceNode\" \"combobox\"");
            this.IClickOn("CreateMovement\" \"sourceNode", "combobox");
            var actualSourceNode = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.AreEqual(expectedSourceNodeList.Count, actualSourceNode.Count);
            for (int i = 0; i < actualSourceNode.Count; i++)
            {
                Assert.IsTrue(expectedSourceNodeList[i][ConstantValues.Name].Equals(actualSourceNode[i]));
            }
        }

        [Then(@"destination node should be selected as current node")]
        public async Task ThenDestinationNodeShouldBeSelectedAsCurrentNodeAndDisabledAsync()
        {
            var node = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.SetValue("NodeName", node[ConstantValues.Name]);
            Assert.AreEqual(this.GetValue("NodeName"), this.Get<ElementPage>().GetElement(nameof(Resources.DestinationNodeOnCreateMovementInterface)).Text);
        }

        [Then(@"source node should be selected as current node")]
        public async Task ThenSourceNodeShouldBeSelectedAsCurrentNodeAsync()
        {
            var node = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.SetValue("NodeName", node[ConstantValues.Name]);
            Assert.AreEqual(this.GetValue("NodeName"), this.Get<ElementPage>().GetElement(nameof(Resources.SourceNodeOnCreateMovementInterface)).Text);
        }

        [Then(@"""(.*)"" combobox on ""(.*)"" interface should be ""(.*)""")]
        public void ThenComboboxShouldBeDisabled(string field, string grid, string expectedValue)
        {
            this.ComboboxShouldBeDisabled(field, grid, expectedValue);
        }

        [Then(@"""(.*)"" list must contain the nodes of the output connections of the current node")]
        public async Task ThenDestinationNodeListMustContainTheNodesOfTheOutputConnectionsOfTheCurrentNodeAsync(string field)
        {
            var expectedDestinationNode = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetAllOutputConnectionsToNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            var expectedDestinationNodeList = expectedDestinationNode.ToDictionaryList();
            ////this.When("I click on \"CreateMovement\" \"destinationNode\" \"combobox\"");
            this.IClickOn("CreateMovement\" \"destinationNode", "combobox");
            var actualDestinationNode = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.AreEqual(expectedDestinationNodeList.Count, actualDestinationNode.Count);
            for (int i = 0; i < actualDestinationNode.Count; i++)
            {
                Assert.IsTrue(expectedDestinationNodeList[i][ConstantValues.Name].Equals(actualDestinationNode[i]));
            }
        }

        [Then(@"destination node list should appear disabled without elements")]
        public void ThenDestinationNodeListShouldAppearDisabledWithoutElements()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DestinationNodeOnCreateMovementInterface)).Text == "Seleccionar");
            ////this.Then("\"destination node\" combobox on \"create movement\" interface should be \"disabled\"");
            this.ComboboxShouldBeDisabled("destination node", "create movement", "disabled");
        }

        [Then(@"""(.*)"" list must contain the current node​")]
        public async Task ThenSourceNodeListMustContainTheCurrentNodeAsync(string field)
        {
            if (field == "source node")
            {
                ////this.When("I click on \"CreateMovement\" \"sourceNode\" \"combobox\"");
                this.IClickOn("CreateMovement\" \"sourceNode", "combobox");
                var actualSourceNode = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                await Task.Delay(3000).ConfigureAwait(false);
                Assert.IsTrue(this.GetValue("NodeName") == actualSourceNode[0]);
            }
            else if (field == "destination node")
            {
                ////this.When("I click on \"CreateMovement\" \"destinationNode\" \"combobox\"");
                this.IClickOn("CreateMovement\" \"destinationNode", "combobox");
                var actualDestinationNode = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
                await Task.Delay(3000).ConfigureAwait(false);
                Assert.IsTrue(this.GetValue("NodeName") == actualDestinationNode[0]);
            }
        }

        [Then(@"""(.*)"" list should contain the products that belong to the selected source node")]
        public async Task ThenSourceProductsListShouldContainTheProductsThatBelongToTheSelectedSourceNodeAsync(string field)
        {
            IEnumerable<IDictionary<string, object>> sourceProducts = null;
            if (this.GetValue(ConstantValues.Variable) == "IdentifiedLoss")
            {
                sourceProducts = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetProductsForGivenNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            }
            else
            {
                sourceProducts = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetProductsForGivenNode, args: new { nodeId = this.GetValue(ConstantValues.SourceNode) }).ConfigureAwait(false);
            }

            var expectedSourceProductsList = sourceProducts.ToDictionaryList();
            ////this.When("I click on \"CreateMovement\" \"sourceProduct\" \"combobox\"");
            this.IClickOn("CreateMovement\" \"sourceProduct", "combobox");
            var actualSourceProducts = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.AreEqual(expectedSourceProductsList.Count, actualSourceProducts.Count);
            for (int i = 0; i < actualSourceProducts.Count; i++)
            {
                Assert.IsTrue(expectedSourceProductsList[i][ConstantValues.Name].Equals(actualSourceProducts[i]));
            }
        }

        [Then(@"""(.*)"" list should contain the products that belong to the selected destination node")]
        public async Task ThenDestinationProductsListShouldContainTheProductsThatBelongToTheSelectedDestinationNodeAsync(string field)
        {
            var destinationProducts = await this.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetProductsForGivenNode, args: new { nodeId = this.GetValue(ConstantValues.DestinationNode) }).ConfigureAwait(false);
            var expectedDestinationProductsList = destinationProducts.ToDictionaryList();
            ////this.When("I click on \"CreateMovement\" \"destinationProduct\" \"combobox\"");
            this.IClickOn("CreateMovement\" \"destinationProduct", "combobox");
            var actualDestinationProducts = this.Get<ElementPage>().GetListElementsText(nameof(Resources.SelectBoxMenuOnCreateMovement), formatArgs: UIContent.Conversion[field]);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.AreEqual(expectedDestinationProductsList.Count, actualDestinationProducts.Count);
            for (int i = 0; i < actualDestinationProducts.Count; i++)
            {
                Assert.IsTrue(expectedDestinationProductsList[i][ConstantValues.Name].Equals(actualDestinationProducts[i]));
            }
        }

        [Then(@"destination products list should be disabled without elements")]
        public void ThenDestinationProductsListShouldBeDisabledWithoutElements()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.DestinationProductsOnCreateMovementInterface)).Text == "Seleccionar");
            ////this.Then("\"destination node\" combobox on \"create movement\" interface should be \"disabled\"");
            this.ComboboxShouldBeDisabled("destination node", "create movement", "disabled");
        }

        [Then(@"owners configured at the Connection-Product level should be displayed")]
        public void ThenOwnersConfiguredAtTheConnectionProductLevelShouldBeDisplayed()
        {
            this.OwnersConfiguredAtTheConnectionProductLevelShouldBeDisplayed();
        }

        [Then(@"owner's volume is auto updated")]
        public void ThenOwnerSVolumeIsAutoUpdated()
        {
            if (this.GetValue(ConstantValues.Variable) != "Input")
            {
                var ownershipVolume = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 1).GetAttribute("value");
                var expectedOwnershipVolume = "2,000";
                Assert.AreEqual(expectedOwnershipVolume, ownershipVolume);
            }
        }

        [Then(@"owner's volume is auto updated on node product level")]
        public void ThenOwnerSVolumeIsAutoUpdatedOnNodeProductLevel()
        {
            var ownershipVolume = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 1).GetAttribute("value");
            var expectedOwnershipVolume = "1200";
            Assert.AreEqual(expectedOwnershipVolume, ownershipVolume);
        }

        [Then(@"ownership percentage is auto updated as per modifed volume on create movement interface")]
        public void ThenOwnershipPercentageIsAutoUpdatedAsPerModifedVolumeOnCreateMovementInterface()
        {
            if (this.GetValue(ConstantValues.Variable) != "Input")
            {
                var actualOwnershipPercentage = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).GetAttribute("value");
                var expectedOwnershipPercentage = "200";
                Assert.AreEqual(expectedOwnershipPercentage, actualOwnershipPercentage);
            }
        }

        [Then(@"ownership percentage is auto updated as per modifed volume on node product level")]
        public void ThenOwnershipPercentageIsAutoUpdatedAsPerModifedVolumeOnNodeProductLevel()
        {
            var actualOwnershipPercentage = this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).GetAttribute("value");
            var expectedOwnershipPercentage = "120";
            Assert.AreEqual(expectedOwnershipPercentage, actualOwnershipPercentage);
        }

        [When(@"I have modified owners volume on create movement interface")]
        public void WhenIHaveModifiedOwnersVolumeOnCreateMovementInterface()
        {
            if (this.GetValue(ConstantValues.Variable) != "Input")
            {
                try
                {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 1).Clear();
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 1).SendKeys("120");
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 5).Clear();
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 5).SendKeys("230");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    Logger.Info("All owners are not available");
                }
                finally
                {
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 3).Clear();
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 3).SendKeys("880");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
            }
        }

        [When(@"I have modified owners volume percentage on create movement interface")]
        public void WhenIHaveModifiedOwnersVolumePercentageOnCreateMovementInterface()
        {
            if (this.GetValue(ConstantValues.Variable) != "Input")
            {
                try
                {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).Clear();
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).SendKeys("120");
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 6).Clear();
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 6).SendKeys("230");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    Logger.Info("All owners are not available");
                }
                finally
                {
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 4).Clear();
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 4).SendKeys("880");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
            }
        }

        [Then(@"total volume and percentage should be auto updated on create movement interface")]
        public void ThenTotalVolumeAndPercentageShouldBeAutoUpdatedOnCreateMovementInterface()
        {
            if (this.GetValue(ConstantValues.Variable) != "Input")
            {
                var totalOwnershipVolume = this.Get<ElementPage>().GetElement(nameof(Resources.TotalVolumeOnCreateMovement)).Text;
                var expectedTotalOwnershipVolume = "1000";
                Assert.AreEqual(expectedTotalOwnershipVolume, totalOwnershipVolume);
                var totalOwnershipVolumePercentage = this.Get<ElementPage>().GetElement(nameof(Resources.TotalVolumePercentageOnCreateMovement)).GetAttribute("value");
                var expectedTotalOwnershipVolumePercentage = "1000";
                Assert.AreEqual(expectedTotalOwnershipVolumePercentage, totalOwnershipVolumePercentage);
            }
        }

        [Then(@"owners configured at the Node-Product level should be displayed")]
        public void ThenOwnersConfiguredAtTheNodeProductLevelShouldBeDisplayed()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.OwnerhsipData), 2).GetAttribute("value") == ConstantValues.EquionOwnershipPercentage);
        }

        [Then(@"create movement interface is closed")]
        public void ThenCreateMovementInterfaceIsClosed()
        {
            Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.DestinationProductsOnCreateMovementInterface)));
        }

        [When(@"I click on close button")]
        public void WhenIClickOnCloseButton()
        {
            this.Get<ElementPage>().Click(nameof(Resources.CloseButtonOnCreateMovementInterface));
        }

        [Then(@"balance summary with ownership is updated with newly created movement")]
        [Then(@"I should see temporarily stored information for that Movement")]
        public void ThenBalanceSummaryWithOwnershipIsUpdatedWithNewlyCreatedMovement()
        {
            ////this.When("I selected \"Tolerance​\" from \"editOwnershipNode\" \"variable\" \"combobox\"");
            this.ISelectedValueFrom("Tolerance", "editOwnershipNode\" \"variable", "combobox");
            ////this.When("I selected \"REFICAR​\" from \"editOwnershipNode\" \"owner\" \"combobox\"");
            this.ISelectedValueFrom("REFICAR", "editOwnershipNode\" \"owner", "combobox");
            var page = this.Get<ElementPage>();
            Assert.AreEqual(UIContent.Conversion["Tolerance"], page.GetElement(nameof(Resources.NewMovementInformation), formatArgs: 1).Text);
            var expectedDate = page.GetElement(nameof(Resources.EditOwnershipLabelsInformation), formatArgs: 3).Text.Split('l')[1];
            Assert.AreEqual(expectedDate, page.GetElement(nameof(Resources.NewMovementInformation), formatArgs: 2).Text);
            Assert.AreEqual(this.GetValue("NodeName"), page.GetElement(nameof(Resources.NewMovementInformation), formatArgs: 3).Text);
            Assert.AreEqual(ConstantValues.ProductName, page.GetElement(nameof(Resources.NewMovementInformation), formatArgs: 5).Text);
            Assert.AreEqual(ConstantValues.NewMovementVolume, page.GetElement(nameof(Resources.NewMovementInformation), formatArgs: 7).Text.Split('.')[0]);
            Assert.AreEqual(ConstantValues.MeasurementUnit, page.GetElement(nameof(Resources.NewMovementInformation), formatArgs: 8).Text);
        }

        [Then(@"status should be updated to ""(.*)""")]
        public async Task ThenStatusShouldBeUpdatedToAsync(string status)
        {
            var nodeStatus = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOwnershipNodeStatus, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            Assert.AreEqual(UIContent.Conversion[status], nodeStatus[ConstantValues.Name]);
        }
    }
}
