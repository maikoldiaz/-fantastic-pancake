// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalTransformationProcessSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using Ecp.True.Bdd.Tests.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class OperationalTransformationProcessSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I want to process the operational transformation of movements and inventories")]
        public async Task GivenIWantToProcessTheOperationalTransformationOfMovementsAndInventoriesAsync()
        {
            ////this.Given("I want create TestData for Operational Transformation");
            await this.DataSetupForOperationalTransformationAsync().ConfigureAwait(false);
        }

        [When(@"I have an ""(.*)"" record which has equivalent transformation has been setup")]
        public void WhenIHaveAnRecordWhichHasEquivalentTransformationHasBeenSetup(string entity)
        {
            Assert.IsNotNull(entity);
            ////this.When("I navigate to \"TransformSettings\" page");
            this.UiNavigation("TransformSettings");
            ////this.When("I click on \"Transformation\" \"button\"");
            this.IClickOn("Transformation", "button");
            ////this.Then("I should see \"Transformation\" \"Movements\" \"Create\" \"Form\"");
            this.IShouldSee("Transformation\" \"Movements\" \"Create", "Form");
            ////this.When("I provide value for \"Origin\" \"Node\" \"Textbox\" on \"Movement\" Interface");
            this.IProvideValueForOnInterface("Origin\" \"Node", "Textbox", "Movement");
            ////this.And("I select any \"NodeDestination\" from \"Origin\" \"DestinationNode\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("NodeDestination", "Origin\" \"DestinationNode", "dropdown", "Movement");
            ////this.And("I select any \"SourceProduct\" from \"Origin\" \"SourceProduct\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("SourceProduct", "Origin\" \"SourceProduct", "dropdown", "Movement");
            ////this.And("I select any \"DestinationProduct\" from \"Origin\" \"DestinationProduct\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("DestinationProduct", "Origin\" \"DestinationProduct", "dropdown", "Movement");
            ////this.And("I select any \"Unit\" from \"Origin\" \"MeasurementUnit\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("Unit", "Origin\" \"MeasurementUnit", "dropdown", "Movement");
            ////this.When("I provide value for \"Destination\" \"NodeOrigin\" \"Textbox\" on \"Movement\" Interface");
            this.IProvideValueForOnInterface("Destination\" \"NodeOrigin", "Textbox", "Movement");
            this.ISelectAnyFromOnInterface("NodeDestination", "Destination\" \"DestinationNode", "dropdown", "Movement");
            ////this.When("I select any \"SourceProduct\" from \"Destination\" \"SourceProduct\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("SourceProduct", "Destination\" \"SourceProduct", "dropdown", "Movement");
            ////this.When("I select any \"DestinationProduct\" from \"Destination\" \"DestinationProduct\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("DestinationProduct", "Destination\" \"DestinationProduct", "dropdown", "Movement");
            ////this.When("I select any \"Unit\" from \"Destination\" \"MeasurementUnit\" \"dropdown\" on \"Movement\" Interface");
            this.ISelectAnyFromOnInterface("Unit", "Destination\" \"MeasurementUnit", "dropdown", "Movement");
            ////this.And("I click on \"Element\" \"Submit\" \"button\"");
            this.IClickOn("Element\" \"Submit", "button");
            ////this.And("I click on \"Inventories\" tab");
            this.IClickOnTab("Inventories");
            ////this.When("I click on \"Transformation\" \"button\"");
            this.IClickOn("Transformation", "button");
            ////this.Then("I should see \"Transformation\" \"Inventories\" \"Create\" \"Form\"");
            this.IShouldSee("Transformation\" \"Movements\" \"Create", "Form");
            ////this.When("I provide value for \"Origin\" \"Node\" \"Textbox\" on \"Inventory\" Interface");
            this.IProvideValueForOnInterface("Origin\" \"Node", "Textbox", "Inventory");
            ////this.When("I select any \"SourceProduct\" from \"Origin\" \"SourceProduct\" \"dropdown\" on \"Inventory\" Interface");
            this.ISelectAnyFromOnInterface("SourceProduct", "Origin\" \"SourceProduct", "dropdown", "Inventory");
            ////this.When("I select any \"Unit\" from \"Origin\" \"MeasurementUnit\" \"dropdown\" on \"Inventory\" Interface");
            this.ISelectAnyFromOnInterface("Unit", "Origin\" \"MeasurementUnit", "dropdown", "Inventory");
            ////this.When("I provide value for \"Destination\" \"Node\" \"Textbox\" on \"Inventory\" Interface");
            this.IProvideValueForOnInterface("Destination\" \"Node", "Textbox", "Inventory");
            ////this.When("I select any \"SourceProduct\" from \"Destination\" \"SourceProduct\" \"dropdown\" on \"Inventory\" Interface");
            this.ISelectAnyFromOnInterface("SourceProduct", "Destination\" \"SourceProduct", "dropdown", "Inventory");
            ////this.When("I select any \"Unit\" from \"Destination\" \"MeasurementUnit\" \"dropdown\" on \"Inventory\" Interface");
            this.ISelectAnyFromOnInterface("Unit", "Destination\" \"MeasurementUnit", "dropdown", "Inventory");
            ////this.When("I click on \"Element\" \"Submit\" \"button\"");
            this.IClickOn("Element\" \"Submit", "button");
        }

        [Then(@"the system must do the transformation")]
        public async System.Threading.Tasks.Task ThenTheSystemMustDoTheTransformationAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastTransformation).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("NodeId_1"), lastRow["OriginSourceNodeId"]);
            Assert.AreEqual(this.GetValue("NodeId_4"), lastRow["DestinationSourceNodeId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow["OriginSourceProductId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow["DestinationSourceProductId"]);
            var lastButOneRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastButOneTransformation).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("NodeId_1"), lastButOneRow["OriginSourceNodeId"]);
            Assert.AreEqual(this.GetValue("NodeId_4"), lastButOneRow["OriginDestinationNodeId"]);
            Assert.AreEqual(this.GetValue("NodeId_3"), lastButOneRow["DestinationSourceNodeId"]);
            Assert.AreEqual(this.GetValue("NodeId_1"), lastButOneRow["DestinationDestinationNodeId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastButOneRow["OriginSourceProductId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastButOneRow["DestinationSourceProductId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastButOneRow["OriginDestinationProductId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastButOneRow["DestinationDestinationProductId"]);
        }

        [Then(@"the system should perform the following subprocesses")]
        public async Task ThenTheSystemShouldPerformTheFollowingSubprocessesAsync()
        {
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"OperationalTransformation\" file from directory");
            await this.ISelectFileFromDirectoryAsync("OperationalTransformation").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            await Task.Delay(60000).ConfigureAwait(true);
        }

        [Then(@"it should save the record with changed values")]
        public async System.Threading.Tasks.Task ThenItShouldSaveTheRecordWithChangedValuesAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryTransformationRecord, args: new { inventoryId = this.ScenarioContext["InventoryId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("NodeId_4"), lastRow[ConstantValues.NodeId]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow[ConstantValues.ProductId]);
            lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementTransformationRecord, args: new { movementId = this.ScenarioContext["MovementId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("NodeId_3"), lastRow["SourceNodeId"]);
            Assert.AreEqual(this.GetValue("NodeId_1"), lastRow["DestinationNodeId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow["SourceProductId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow["DestinationProductId"]);
        }

        [When(@"I have an ""(.*)"" record which has no equivalent transformation has been setup")]
        public async Task WhenIHaveAnRecordWhichHasNoEquivalentTransformationHasBeenSetupAsync(string entity)
        {
            Assert.IsNotNull(entity);
            ////this.When("I am logged in as \"admin\"");
            this.LoggedInAsUser("admin");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"OperationalTransformation\" file from directory");
            await this.ISelectFileFromDirectoryAsync("OperationalTransformation").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            await Task.Delay(60000).ConfigureAwait(true);
        }

        [Then(@"the system must save the record without changing its contents")]
        public async Task ThenTheSystemMustSaveTheRecordWithoutChangingItsContentsAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetInventoryTransformationRecord, args: new { inventoryId = this.ScenarioContext["InventoryId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("NodeId_1"), lastRow[ConstantValues.NodeId]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow[ConstantValues.ProductId]);
            lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetMovementTransformationRecord, args: new { movementId = this.ScenarioContext["MovementId"].ToString() }).ConfigureAwait(false);
            Assert.AreEqual(this.GetValue("NodeId_1"), lastRow["SourceNodeId"]);
            Assert.AreEqual(this.GetValue("NodeId_4"), lastRow["DestinationNodeId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow["SourceProductId"]);
            Assert.AreEqual(ConstantValues.TransformationProductId, lastRow["DestinationProductId"]);
        }
    }
}
