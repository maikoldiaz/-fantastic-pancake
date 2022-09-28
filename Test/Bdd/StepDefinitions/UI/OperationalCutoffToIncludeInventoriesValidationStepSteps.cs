// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutoffToIncludeInventoriesValidationStepSteps.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    [Binding]
    public class OperationalCutoffToIncludeInventoriesValidationStepSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I create new node ""(.*)"" and ""(.*)""")]
        public async Task WhenICreateNewNodeAndAsync(string inventory, string movement)
        {
            string fileName = "TestData_" + (inventory is null ? throw new System.ArgumentNullException(nameof(inventory)) : inventory.Replace(" ", string.Empty)) + movement;
            ////this.When($"I update the excel with \"{fileName}\" data");
            this.WhenIUpdateTheExcelFileWithDaywiseData(fileName);
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
            ////this.When($"I select \"{fileName}\" file from directory");
            await this.ISelectFileFromDirectoryAsync(fileName).ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [When(@"I update the Segment date different from initial date of next cutoff")]
        public async Task WhenIUpdateTheSegmentDateDifferentFromInitialDateOfNextCutoffAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagSegmentDate, args: new { date = 1, nodeId = this.GetValue("NodeId_1"), elementId = this.GetValue("SegmentId") }).ConfigureAwait(false);
        }

        [StepDefinition(@"validate (that "".*"" "".*"") wizard is ""(.*)""")]
        public void ThenValidateThatWizardIs(ElementLocator elementLocator, string state)
        {
            var attributeValue = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("class");
            Assert.IsTrue(attributeValue.ContainsIgnoreCase(state));
        }

        [StepDefinition(@"validate the message ""(.*)"" in ""(.*)""")]
        public void ThenValidateTheMessageIn(string message, string element)
        {
            Assert.IsNotNull(message);
            if (element.EqualsIgnoreCase("Header"))
            {
                var expectedMessage = "Verificar inventarios para el corte del " + this.GetValue("EndDate") + " al " + this.GetValue("EndDate") + " del segmento " + this.GetValue("CategorySegment");
                var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.Header)).Text;
                Assert.IsTrue(expectedMessage.EqualsIgnoreCase(actualMessage));
            }
            else if (element.EqualsIgnoreCase("Initial Inventory"))
            {
                var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.StepDescInitialInventory)).Text;
                Assert.IsTrue(message.EqualsIgnoreCase(actualMessage));
            }
            else if (element.EqualsIgnoreCase("Possible New Nodes"))
            {
                var actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.StepDescPossibleNewNodes)).Text;
                Assert.IsTrue(message.EqualsIgnoreCase(actualMessage));
            }
        }

        [StepDefinition(@"validate ""(.*)"" Icon displayed in ""(.*)""")]
        public void ThenValidateIconDisplayedIn(string expectedStatus, string step)
        {
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var actualStatus = this.Get<ElementPage>().GetElement(nameof(Resources.StepStatusInitialInventory)).GetAttribute("class");
                Assert.IsTrue(actualStatus.ContainsIgnoreCase(UIContent.Conversion[expectedStatus]));
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var actualStatus = this.Get<ElementPage>().GetElement(nameof(Resources.StepStatusPossibleNewNodes)).GetAttribute("class");
                Assert.IsTrue(actualStatus.ContainsIgnoreCase(UIContent.Conversion[expectedStatus]));
            }
        }

        [Then(@"validate warning message ""(.*)""")]
        public void ThenValidateWarningMessage(string expectedMessage)
        {
            var actualStatus = this.Get<ElementPage>().GetElement(nameof(Resources.NotificationErrorMessage)).Text;
            Assert.IsTrue(actualStatus.EqualsIgnoreCase(expectedMessage));
        }

        [Then(@"validate ""(.*)"" without owner in ""(.*)""")]
        [Then(@"validate ""(.*)"" without initial inventory in ""(.*)""")]
        public void ThenValidateWithoutInitialInventoryIn(string nodes, string step)
        {
            Assert.IsNotNull(nodes);
            int count = 0;
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountInitialInventory));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.StepNodeNameColumnInitialInventory), i).Text;
                    for (var j = 3; j <= 4; j++)
                    {
                        if (nodeName.EqualsIgnoreCase(this.GetValue("NodeName_" + j)))
                        {
                            Assert.IsTrue(nodeName.EqualsIgnoreCase(this.GetValue("NodeName_" + j)));
                            count++;
                            break;
                        }
                    }
                }
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountPossibleNewNodes));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.StepNodeNameColumnPossibleNewNodes), i).Text;
                    for (var j = 3; j <= 4; j++)
                    {
                        if (nodeName.EqualsIgnoreCase(this.GetValue("NodeName_" + j)))
                        {
                            Assert.IsTrue(nodeName.EqualsIgnoreCase(this.GetValue("NodeName_" + j)));
                            count++;
                            break;
                        }
                    }
                }
            }

            Assert.IsTrue(count.Equals(2));
        }

        [Then(@"validate ""(.*)"" corresponds to the ""(.*)"" displayed in ""(.*)""")]
        public void ThenValidateCorrespondsToTheDisplayedIn(string date, string nodes, string step)
        {
            Assert.IsNotNull(nodes);
            Assert.IsNotNull(date);
            int count = 0;
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountInitialInventory));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeDate = this.Get<ElementPage>().GetElement(nameof(Resources.StepDateColumnInitialInventory), i).Text;
                    for (var j = 3; j <= 4; j++)
                    {
                        if (nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDate")))
                        {
                            Assert.IsTrue(nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDate")));
                            count++;
                        }
                    }
                }
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountPossibleNewNodes));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeDate = this.Get<ElementPage>().GetElement(nameof(Resources.StepDateColumnPossibleNewNodes), i).Text;
                    for (var j = 3; j <= 4; j++)
                    {
                        if (nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDate")))
                        {
                            Assert.IsTrue(nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDate")));
                            count++;
                        }
                    }
                }
            }

            Assert.IsTrue(count.Equals(4));
        }

        [Then(@"validate ""(.*)"" corresponds to the ""(.*)"" group displayed in ""(.*)""")]
        public void ThenValidateCorrespondsToTheGroupDisplayedIn(string date, string nodes, string step)
        {
            Assert.IsNotNull(nodes);
            Assert.IsNotNull(date);
            int count = 0;
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountInitialInventory));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeDate = this.Get<ElementPage>().GetElement(nameof(Resources.StepDateColumnInitialInventory), i).Text;
                    for (var j = 3; j <= 4; j++)
                    {
                        if (nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDateNodeGroup")))
                        {
                            Assert.IsTrue(nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDateNodeGroup")));
                            count++;
                        }
                    }
                }
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountPossibleNewNodes));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeDate = this.Get<ElementPage>().GetElement(nameof(Resources.StepDateColumnPossibleNewNodes), i).Text;
                    for (var j = 3; j <= 4; j++)
                    {
                        if (nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDateNodeGroup")))
                        {
                            Assert.IsTrue(nodeDate.EqualsIgnoreCase(this.GetValue("InitialInventoryDateNodeGroup")));
                            count++;
                        }
                    }
                }
            }

            Assert.IsTrue(count.Equals(4));
        }

        [Then(@"validate ""(.*)"" corresponds to '(.*)' in ""(.*)""")]
        public void ThenValidateCorrespondsToIn(string totalNodes, string nodeType, string step)
        {
            Assert.IsNotNull(nodeType);
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountInitialInventory));
                Assert.IsTrue(UIContent.Conversion[totalNodes].EqualsIgnoreCase(numberOfRows.Count.ToString(CultureInfo.InvariantCulture)));
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountPossibleNewNodes));
                Assert.IsTrue(UIContent.Conversion[totalNodes].EqualsIgnoreCase(numberOfRows.Count.ToString(CultureInfo.InvariantCulture)));
            }
        }

        [Then(@"validate ""(.*)"" group without initial inventory in ""(.*)""")]
        [Then(@"validate ""(.*)"" group without owner in ""(.*)""")]
        public void ThenValidateGroupWithoutOwnerIn(string group, string step)
        {
            int count = 0;
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountInitialInventory));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.StepNodeNameColumnInitialInventory), i).Text;
                    for (var j = 1; j <= 2; j++)
                    {
                        if (nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)))
                        {
                            Assert.IsTrue(nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)));
                            count++;
                            break;
                        }
                    }
                }
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountPossibleNewNodes));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.StepNodeNameColumnPossibleNewNodes), i).Text;
                    for (var j = 1; j <= 2; j++)
                    {
                        if (nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)))
                        {
                            Assert.IsTrue(nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)));
                            count++;
                            break;
                        }
                    }
                }
            }

            Assert.IsTrue(count.Equals(2));
        }

        [Then(@"validate ""(.*)"" group not in ""(.*)""")]
        public void ThenValidateGroupNotIn(string group, string step)
        {
            int count = 0;
            if (step.EqualsIgnoreCase("Initial Inventory"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountInitialInventory));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.StepNodeNameColumnInitialInventory), i).Text;
                    for (var j = 1; j <= 2; j++)
                    {
                        if (!nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)))
                        {
                            Assert.IsFalse(nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)));
                            count++;
                        }
                    }
                }
            }
            else if (step.EqualsIgnoreCase("Possible New Nodes"))
            {
                var numberOfRows = this.Get<ElementPage>().GetElements(nameof(Resources.StepRowCountPossibleNewNodes));
                for (var i = 1; i <= numberOfRows.Count; i++)
                {
                    var nodeName = this.Get<ElementPage>().GetElement(nameof(Resources.StepNodeNameColumnPossibleNewNodes), i).Text;
                    for (var j = 1; j <= 2; j++)
                    {
                        if (!nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)))
                        {
                            Assert.IsFalse(nodeName.EqualsIgnoreCase(this.GetValue(group + "_Group_NodeName_" + j)));
                            count++;
                        }
                    }
                }
            }

            Assert.IsTrue(count.Equals(4));
        }
    }
}