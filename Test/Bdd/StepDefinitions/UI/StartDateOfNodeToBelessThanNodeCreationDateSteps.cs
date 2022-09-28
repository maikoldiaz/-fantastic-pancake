// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartDateOfNodeToBelessThanNodeCreationDateSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class StartDateOfNodeToBelessThanNodeCreationDateSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I get start date from node tag")]
        public async Task WhenIGetStartDateFromNodeTagAsync()
        {
            var startDate = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNodeStartDateFromNodeTag, args: new { nodeName = this.ScenarioContext["NodeName"].ToString() }).ConfigureAwait(false);
            var startDateList = startDate.ToDictionaryList();
            this.ScenarioContext["NodeGroupStartdate"] = startDateList[0][ConstantValues.StartDate].ToString();
            this.ScenarioContext["SegmentGroupStartdate"] = startDateList[1][ConstantValues.StartDate].ToString();
            this.ScenarioContext["OperatorGroupStartdate"] = startDateList[2][ConstantValues.StartDate].ToString();
        }

        [Then(@"start date of node type group is ""(.*)"" months less than current date")]
        public void ThenStartDateOfNodeTypeGroupIsMonthsLessThanCurrentDate(string key, Table table)
        {
            if (table != null)
            {
                foreach (TableRow row in table.Rows)
                {
                    int months = row[key].ToInt();
                    var expectedDate = DateTime.Now.AddMonths(-months).ToShortDateString();
                    var actualDate = this.ScenarioContext["NodeGroupStartdate"].ToString().ToDateTime().ToShortDateString();
                    Assert.AreEqual(expectedDate, actualDate);
                }
            }
        }

        [StepDefinition(@"start date of node in segment group is ""(.*)"" months less than current date")]
        public void ThenStartDateOfNodeInSegmentGroupIsMonthsLessThanCurrentDate(string key, Table table)
        {
            if (table != null)
            {
                foreach (TableRow row in table.Rows)
                {
                    int months = row[key].ToInt();
                    var expectedDate = DateTime.Now.AddMonths(-months).ToShortDateString();
                    var actualDate = this.ScenarioContext["SegmentGroupStartdate"].ToString().ToDateTime().ToShortDateString();
                    Assert.AreEqual(expectedDate, actualDate);
                }
            }
        }

        [StepDefinition(@"start date of node in operator group is ""(.*)"" months less than current date")]
        public void ThenStartDateOfNodeInOperatorGroupIsMonthsLessThanCurrentDate(string key, Table table)
        {
            if (table != null)
            {
                foreach (TableRow row in table.Rows)
                {
                    int months = row[key].ToInt();
                    var expectedDate = DateTime.Now.AddMonths(-months).ToShortDateString();
                    var actualDate = this.ScenarioContext["OperatorGroupStartdate"].ToString().ToDateTime().ToShortDateString();
                    Assert.AreEqual(expectedDate, actualDate);
                }
            }
        }

        [StepDefinition(@"I set the date value in the xml to (.*) days less than current date")]
        public async Task GivenISetTheValueInTheXmlLessThanCurrentDateAsync(int noOfdays)
        {
            var xmlFieldValue = DateTime.Now.AddDays(-noOfdays).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            ////this.When("I update xml \"Inventory\" \"DATE\" with \"" + xmlFieldValue + "\"");
            await this.UpdateXmlForSinoperAsync("Inventory", "DATE", xmlFieldValue).ConfigureAwait(false);
        }

        [StepDefinition(@"I set inventory date as 2 days less than current date")]
        public void WhenISetInventoryDateAsDaysLessThanCurrentDate()
        {
            ////this.When("I have 1 inventory");
            this.SetValue(ConstantValues.TestData, "WithScenarioId");
            this.SetValue(ConstantValues.BATCHID, new Faker().Random.AlphaNumeric(10).ToString(CultureInfo.InvariantCulture));
            this.SapPoInventoryRegistration(1);
        }

        [StepDefinition(@"I set the date value for movement in the xml to 2 days less than current date")]
        public async Task GivenISetTheValueInTheXmlToDaysLessThanCurrentDateAsync()
        {
            ////this.Given("I want to adjust an \"Movements\"");
            await this.IWantToCancelOrAdjustAnAsync("Movements").ConfigureAwait(false);
        }

        [StepDefinition(@"I set operative date to 2 days less than current date")]
        public void WhenISetOperativeDateToDaysLessThanCurrentDate()
        {
            this.LogToReport("Covered in previous step");
        }

        [StepDefinition(@"it should not be registered in the system")]
        public async Task ThenItShouldNotBeRegisteredInTheSystemAsync()
        {
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
            var status = this.Get<ElementPage>().GetElement(nameof(Resources.FileStatus)).Text;
            Assert.IsTrue(status.EqualsIgnoreCase("Fallido"));
        }

        [StepDefinition(@"I create Node in the system")]
        public async Task WhenICreateNodeInTheSystemAsync()
        {
            ////this.And("I have segment category in the system");
            await this.SegementCategoryIntheSystemAsync().ConfigureAwait(false);
            ////this.When("I navigate to \"CreateNodes\" page");
            this.UiNavigation("CreateNodes");
            ////this.And("I click on \"CreateNode\" \"button\"");
            this.IClickOn("CreateNode", "button");
            ////this.When("I provide the value for \"CreateNode\" \"name\" \"textbox\"");
            this.IProvideTheValueForElement("CreateNode\" \"name", "textbox");
            ////this.And("I select any \"NodeTypeValue\" from \"CreateNode\" \"type\" \"dropdown\"");
            this.ISelectAnyElement("NodeTypeValue", "CreateNode\" \"type", "dropdown");
            ////this.And("I select any \"Operatorvalue\" from \"CreateNode\" \"operator\" \"dropdown\"");
            this.ISelectAnyElement("Operatorvalue", "CreateNode\" \"operator", "dropdown");
            ////this.And("I select SegmentValue from \"CreateNode\" \"segment\" \"dropdown\"");
            await this.ISelectSegmentValueFromAsync("CreateNode\" \"segment", "dropdown").ConfigureAwait(false);
            ////this.And("I enter \"1\" into \"Decimal\" \"Order\" \"textbox\"");
            this.IEnterValueInto("1", "Decimal\" \"Order", "textbox");
            ////this.And("I provide the value for \"CreateNode\" \"description\" \"textarea\"");
            this.IProvideTheValueForElement("CreateNode\" \"description", "textarea");
            ////this.When("I click on \"StorageLocations\" tab");
            this.IClickOnTab("StorageLocations");
            ////this.When("I click on \"NodeStorageLocationGrid\" \"create\" \"button\"");
            this.IClickOn("NodeStorageLocationGrid\" \"create", "button");
            ////this.Then("I should see \"Create\" \"NodeStorageLocation\" \"form\"");
            this.IShouldSee("Create\" \"NodeStorageLocation", "Form");
            ////this.When("I provide the value for \"NodeStorageLocation\" \"name\" \"textbox\"");
            this.IProvideTheValueForElement("NodeStorageLocation\" \"name", "textbox");
            ////this.And("I select any \"NodeTypeValue\" from \"NodeStorageLocation\" \"StorageLocationType\" \"dropdown\"");
            this.ISelectAnyElement("NodeTypeValue", "NodeStorageLocation\" \"StorageLocationType", "dropdown");
            ////this.And("I provide the value for \"NodeStorageLocation\" \"description\" \"textarea\"");
            this.IProvideTheValueForElement("NodeStorageLocation\" \"description", "textarea");
            ////this.And("I click on \"NodeStorageLocation\" \"submit\" \"button\"");
            this.IClickOn("NodeStorageLocation\" \"submit", "button");
            ////this.When("I click on \"nodeStorageLocation\" \"AddProducts\" \"button\" for 1 Product");
            this.IClickOnForProduct("nodeStorageLocation\" \"AddProducts", "button", 1);
            ////this.When("I enter new \"ProductName\" into NodeStorageLocation name textbox");
            this.IEnterNewIntoNodeStorageLocationNameTextbox("ProductName");

            ////this.And("I click on \"AddProduct\" \"submit\" \"button\"");
            this.IClickOn("AddProduct\" \"submit", "button");
            ////this.When("I click on \"submit\" \"button\"");
            this.IClickOn("AddProduct\" \"submit", "button");
            this.ScenarioContext["NodeName"] = this.GetValue(ConstantValues.CreatedNodeName);
        }

        [StepDefinition(@"it should be processed")]
        public async Task ThenItShouldBeProcessedAsync()
        {
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
            var status = this.Get<ElementPage>().GetElement(nameof(Resources.FileStatus)).Text;
            Assert.IsTrue(status == "Finalizado");
        }
    }
}