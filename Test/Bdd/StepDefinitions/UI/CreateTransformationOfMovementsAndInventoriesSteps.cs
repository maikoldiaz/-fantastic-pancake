// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateTransformationOfMovementsAndInventoriesSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class CreateTransformationOfMovementsAndInventoriesSteps : EcpWebStepDefinitionBase
    {
        [When(@"I find previous records available for transformation configurations")]
        public void WhenIFindPreviousRecordsAvailableForTransformationConfigurations()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I do not find previous records available for transformation configurations")]
        public void WhenIDoNotFindPreviousRecordsAvailableForTransformationConfigurations()
        {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            this.Get<ElementPage>().GetElement(nameof(Resources.MovementNodeOrginTextBox)).SendKeys(ConstantValues.SourceNodeProductId);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            this.Get<ElementPage>().GetElement(nameof(Resources.MovementNodeOrginTextBox)).SendKeys(OpenQA.Selenium.Keys.Enter);
        }

        [When(@"I do not enter values for Mandatory fields for ""(.*)""")]
        public void WhenIDoNotEnterValuesForMandatoryFieldsFor(string type)
        {
            Assert.IsNotNull(type);
        }

        [When(@"I provide value (for "".*"" "".*"") on ""(.*)"" Interface")]
        public void WhenIProvideValueForOnInterface(ElementLocator elementLocator, string field)
        {
            this.IProvideValueForOnInterface(elementLocator, field);
        }

        [When(@"I select any ""(.*)"" (from "".*"" "".*"") on ""(.*)"" Interface")]
        public void WhenISelectAnyFromOnInterface(string value, ElementLocator elementLocator, string field)
        {
            this.ISelectAnyFromOnInterface(value, elementLocator, field);
        }

        [Then(@"it should ""(.*)"" the ""(.*)"" transforamtion data in the system")]
        public async System.Threading.Tasks.Task ThenItShouldTheTransforamtionDataInTheSystemAsync(string action, string field)
        {
            IDictionary<string, string> lastRow = null;
            if (action.EqualsIgnoreCase("Register"))
            {
                lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastTransformation).ConfigureAwait(false);
            }
            else if (action.EqualsIgnoreCase("Update"))
            {
                lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastUpdatedTransformation).ConfigureAwait(false);
            }
            else if (field.EqualsIgnoreCase("Inventory"))
            {
                Assert.AreEqual(this.ScenarioContext["Origin_NodeId"], lastRow["OriginSourceNodeId"]);
                Assert.AreEqual(this.ScenarioContext["Destination_NodeId"], lastRow["DestinationSourceNodeId"]);
                Assert.AreEqual(ConstantValues.OriginSourceProductId, lastRow["OriginSourceProductId"]);
                Assert.AreEqual(ConstantValues.DestinationSourceProductId, lastRow["DestinationSourceProductId"]);
            }
            else if (field.EqualsIgnoreCase("Movement"))
            {
                Assert.AreEqual(this.ScenarioContext["Origin_SourceNodeId"], lastRow["OriginSourceNodeId"]);
                Assert.AreEqual(this.ScenarioContext["Destination_SourceNodeId"], lastRow["OriginDestinationNodeId"]);
                Assert.AreEqual(this.ScenarioContext["Origin_DestinationNodeId"], lastRow["DestinationSourceNodeId"]);
                Assert.AreEqual(this.ScenarioContext["Destination_DestinationNodeId"], lastRow["DestinationDestinationNodeId"]);
                Assert.AreEqual(ConstantValues.OriginSourceProductId, lastRow["OriginSourceProductId"]);
                Assert.AreEqual(ConstantValues.DestinationSourceProductId, lastRow["DestinationSourceProductId"]);
                Assert.AreEqual(ConstantValues.OriginSourceProductId, lastRow["OriginDestinationProductId"]);
                Assert.AreEqual(ConstantValues.DestinationSourceProductId, lastRow["DestinationDestinationProductId"]);
            }

            Assert.AreEqual(ConstantValues.MeasurementUnitId, lastRow["OriginMeasurementId"]);
            Assert.AreEqual(ConstantValues.MeasurementUnitId, lastRow["DestinationMeasurementId"]);
        }

        [When(@"I provide existing value (for "".*"" "".*"") on ""(.*)"" Interface")]
        public async System.Threading.Tasks.Task WhenIProvideExistingValueForOnInterfaceAsync(ElementLocator elementLocator, string field)
        {
            _ = elementLocator is null ? throw new System.ArgumentNullException(nameof(elementLocator)) : elementLocator;

            if (elementLocator.Value.Contains("origin") && (field.EqualsIgnoreCase("Inventory") || field.EqualsIgnoreCase("Movement")))
            {
                var lastCreatedTransformation = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastTransformation).ConfigureAwait(false);
                var originNodeId = lastCreatedTransformation["OriginSourceNodeId"];
                var nodeRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeById, args: new { nodeId = originNodeId }).ConfigureAwait(false);
                var originNodeName = nodeRow["Name"];
                this.Get<ElementPage>().EnterText(nameof(Resources.SourceNodeNameTextBox), originNodeName, 1);
                this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                this.Get<ElementPage>().GetElement(nameof(Resources.SourceNodeNameTextBox), 1).SendKeys(OpenQA.Selenium.Keys.Enter);
            }
        }

        [When(@"I select existing ""(.*)"" (from "".*"" "".*"") on ""(.*)"" Interface")]
        public async System.Threading.Tasks.Task WhenISelectExistingFromOnInterfaceAsync(string value, ElementLocator elementLocator, string field)
        {
            _ = elementLocator is null ? throw new System.ArgumentNullException(nameof(elementLocator)) : elementLocator;
            _ = value is null ? throw new System.ArgumentNullException(nameof(elementLocator)) : value;

            if (elementLocator.Value.Contains("origin") && value.Contains("Product"))
            {
                var lastCreatedTransformation = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastTransformation).ConfigureAwait(false);
                var originSourceProductId = lastCreatedTransformation["OriginSourceProductId"];
                var productRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetProductById, args: new { productId = originSourceProductId }).ConfigureAwait(false);
                var originProductName = productRow["Name"];
                this.Get<ElementPage>().Click(elementLocator);
                this.Get<ElementPage>().Click(nameof(Resources.UploadType), originProductName, this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOptionByValue), originProductName).Count);
            }
            else if (elementLocator.Value.Contains("origin"))
            {
                var lastCreatedTransformation = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastTransformation).ConfigureAwait(false);
                var originDestinationNodeId = lastCreatedTransformation["OriginDestinationNodeId"];
                var nodeRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetNodeById, args: new { nodeId = originDestinationNodeId }).ConfigureAwait(false);
                var destinationinNodeName = nodeRow["Name"];
                this.Get<ElementPage>().Click(elementLocator);
                this.Get<ElementPage>().Click(nameof(Resources.UploadType), destinationinNodeName, this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOptionByValue), destinationinNodeName).Count);
            }

            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.IsNotNull(field);
        }

        [Then(@"I should see the error message ""(.*)"" displayed")]
        [Then(@"I should see the duplicate message ""(.*)""")]
        public void ThenIShouldSeeTheDuplicateMessage(string message)
        {
            Assert.AreEqual(message, this.Get<ElementPage>().GetElement(nameof(Resources.DuplicatetransformationMessage)).Text);
        }

        [When(@"I click on ""(.*)"" (from "".*"" "".*"") on ""(.*)"" Interface")]
        public void WhenIClickOnFromOnInterface(string field, ElementLocator elementLocator, string type)
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.UploadType), ConstantValues.ProductName, this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxOptionByValue), ConstantValues.ProductName).Count).Displayed);
            Assert.IsNotNull(field);
            Assert.IsNotNull(elementLocator);
            Assert.IsNotNull(type);
        }
    }
}