// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageOwnersWithColorSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ManageOwnersWithColorSteps : EcpWebStepDefinitionBase
    {
        [Given(@"the owner has a color assigned")]
        public async Task WhenTheOwnerHasAColorAssignedAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateOwnerColor).ConfigureAwait(false);
        }

        [When(@"I verify it in ""(.*)"" page")]
        public async Task WhenIVerifyItInPageAsync(string page)
        {
            switch (page)
            {
                case "Creation of movements with ownership":
                    ////this.When("I click on \"OwnershipNodes\" \"EditOwnership\" \"link\"");
                    this.IClickOn("OwnershipNodes\" \"EditOwnership", "link");
                    ////this.When("I click on \"EditOwnershipNode\" \"NewMovement\" \"button\"");
                    this.IClickOn("EditOwnershipNode\" \"NewMovement", "button");
                    ////this.When("I enter volume into \"decimal\" \"netVolume\" \"textbox\"");
                    this.IEnterVolumeInto("decimal\" \"netVolume", "textbox");
                    ////this.When("I selected \"Tolerance\" from \"CreateMovement\" \"variable\" \"combobox\"");
                    this.ISelectedValueFrom("Tolerance", "CreateMovement\" \"variable", "combobox");
                    ////this.When("I selected node from either source node or destination node based on input variable");
                    await this.ISelectedNodeFromEitherSourceNodeOrDestinationNodeBasedOnInputVariableAsync().ConfigureAwait(false);
                    ////this.When("I selected \"product\" from \"CreateMovement\" \"sourceProduct\" \"combobox\"");
                    this.ISelectedValueFrom("product", "CreateMovement\" \"sourceProduct", "combobox");
                    ////this.Then("owners configured at the Connection-Product level should be displayed");
                    this.OwnersConfiguredAtTheConnectionProductLevelShouldBeDisplayed();
                    break;
                case "Edit ownership of a movement":
                    ////this.When("I select \"ECOPETROL\" from \"Propietario\" combo box in \"Edit Ownership Node\" grid");
                    await this.SelectValueFromComboBoxAsync("ECOPETROL", "Propietario", "Edit Ownership Node").ConfigureAwait(false);
                    ////this.When("I select \"Pérdida No Identificada\" from \"Variable\" combo box in \"Edit Ownership Node\" grid");
                    await this.SelectValueFromComboBoxAsync("Pérdida No Identificada", "Variable", "Edit Ownership Node").ConfigureAwait(false);
                    ////this.When("I click on \"ownershipNodeData\" \"edit\" \"link\"");
                    this.IClickOn("ownershipNodeData\" \"edit", "link");
                    break;
                case "Elimination of ownership of a movement":
                    ////this.When("I click on \"OwnershipNodeData\" \"Delete\" \"link\"");
                    this.IClickOn("OwnershipNodeData\" \"Delete", "link");
                    break;
                case "Ownership details of a movement":
                    ////this.When("I click on \"OwnershipNodeData\" \"Details\" \"link\"");
                    this.IClickOn("OwnershipNodeData\" \"Details", "link");
                    break;
                case "Edit ownership of a final inventory":
                    ////this.When("I select \"Inventario Final\" from \"Variable\" combo box in \"Edit Ownership Node\" grid");
                    await this.SelectValueFromComboBoxAsync("Inventario Final", "Variable", "Edit Ownership Node").ConfigureAwait(false);
                    ////this.When("I click on \"OwnershipNodeData\" \"Edit\" \"link\"");
                    this.IClickOn("OwnershipNodeData\" \"Edit", "link");
                    break;
                case "Ownership details of an inventory":
                    ////this.When("I click on \"OwnershipNodeData\" \"Details\" \"link\"");
                    this.IClickOn("OwnershipNodeData\" \"Details", "link");
                    break;
                case "Configure attributes nodes":
                    ////this.When("I navigate to \"ConfigureAttributesNodes\" page");
                    this.UiNavigation("ConfigureAttributesNodes");
                    ////this.When("I enter valid \"NodeName\" into \"nodeAttributes\" \"Name\" \"textbox\"");
                    this.IEnterValidInto("NodeName", "nodeAttributes\" \"Name", "textbox");
                    ////this.Then("I should see a \"NodeName\" belongs to \"nodeAttribute\" in the grid");
                    this.IShouldSeeABelongsToInTheGrid("NodeName", "nodeAttribute");
                    ////this.When("I click on \"nodeAttributes\" \"Edit\" \"link\"");
                    this.IClickOn("nodeAttributes\" \"Edit", "link");
                    ////this.When("I click on \"NodeProducts\" \"ownership\" \"edit\" \"link\" of a combination having or not having value");
                    this.IClickOn("NodeProducts\" \"ownership\" \"edit", "link");
                    ////this.When("I click on \"NodeProducts\" \"ownership\" \"pie\" \"submit\" \"button\"");
                    this.IClickOn("NodeProducts\" \"ownership\" \"pie\" \"submit", "button");
                    ////this.When("I click on \"NodeProducts\" \"owners\" \"target\" \"moveAll\" \"button\"");
                    this.IClickOn("NodeProducts\" \"owners\" \"target\" \"moveAll", "button");
                    ////this.When("I click on \"NodeProducts\" \"owners\" \"source\" \"30\" \"list\"");
                    this.IClickOn("NodeProducts\" \"owners\" \"source\" \"30", "list");
                    ////this.When("I click on \"NodeProducts\" \"owners\" \"source\" \"move\" \"button\"");
                    this.IClickOn("NodeProducts\" \"owners\" \"source\" \"move", "button");
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipPercentageTextBox), formatArgs: "percentage").SendKeys("100");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    ////this.When("I click on \"NodeProducts\" \"ownership\" \"submit\" \"button\"");
                    this.IClickOn("NodeProducts\" \"ownership\" \"submit", "button");
                    break;
                case "Configure connections attributes":
                    this.SetValue("SourceNodeName", this.GetValue("NodeName"));
                    ////this.When("I navigate to \"ConfigureAttributesConnections\" page");
                    this.UiNavigation("ConfigureAttributesConnections");
                    ////this.When("I enter valid \"SourceNodeName\" into \"connAttributes\" \"SourceNode\" \"Name\" \"textbox\"");
                    this.IEnterValidInto("SourceNodeName", "connAttributes\" \"SourceNode\" \"Name", "textbox");
                    ////this.Then("I should see a \"SourceNodeName\" belongs to \"node-connection\" in the grid");
                    this.IShouldSeeABelongsToInTheGrid("SourceNodeName", "node-connection");
                    ////this.When("I click on \"connAttributes\" \"Edit\" \"link\"");
                    this.IClickOn("connAttributes\" \"Edit", "link");
                    ////this.When("I click on \"connectionProducts\" \"ownership\" \"edit\" \"link\" of a combination having or not having value");
                    this.IClickOn("connectionProducts\" \"ownership\" \"edit", "link");
                    ////this.When("I click on \"connectionProducts\" \"owners\" \"source\" \"30\" \"list\"");
                    this.IClickOn("connectionProducts\" \"owners\" \"source\" \"30", "list");
                    ////this.When("I click on \"connectionProducts\" \"owners\" \"source\" \"move\" \"button\"");
                    this.IClickOn("connectionProducts\" \"owners\" \"source\" \"move", "button");
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    this.Get<ElementPage>().GetElement(nameof(Resources.OwnershipPercentageTextBox), formatArgs: "percentage").SendKeys("100");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                    ////this.When("I click on \"connectionProducts\" \"ownership\" \"submit\" \"button\"");
                    this.IClickOn("connectionProducts\" \"ownership\" \"submit", "button");
                    break;
                default:
                    break;
            }
        }

        [When(@"I see that owner must appear with its assigned color")]
        public void WhenISeeThatOwnerMustAppearWithItsAssignedColor()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsBar));
            Assert.IsTrue(elements[0].GetAttribute("class").Contains("#00903B"));
        }

        [When(@"I see that the owner must appear with its assigned color")]
        public void WhenISeeThatTheOwnerMustAppearWithItsAssignedColor()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsBar));
            Assert.IsTrue(elements[0].GetAttribute("style").Contains("rgb(0, 144, 59)"));
        }

        [When(@"I see that the owner must appear with the default color")]
        public void WhenISeeThatTheOwnerMustAppearWithTheDefaultColor()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsBar));
            Assert.IsTrue(elements[0].GetAttribute("style").Contains("rgb(224, 240, 250)"));
        }

        [Given(@"the owner does not have a color assigned")]
        public async Task WhenTheOwnerDoesNotHaveAColorAssignedAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateOwnerColorWithNull).ConfigureAwait(false);
        }

        [When(@"I see that owner must appear with the default color")]
        public void WhenISeeThatOwnerMustAppearWithTheDefaultColor()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsBar));
            Assert.IsTrue(elements[0].GetAttribute("class").Contains("#E0F0FA"));
        }

        [When(@"I see that owner must appear with its assigned color in pie chart")]
        public void WhenISeeThatOwnerMustAppearWithItsAssignedColorInPieChart()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsInPieChart));
            Assert.IsTrue(elements[0].GetAttribute("color").Contains("#00903B"));
        }

        [When(@"I see that owner must appear with the default color in pie chart")]
        public void WhenISeeThatOwnerMustAppearWithTheDefaultColorInPieChart()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsInPieChart));
            Assert.IsTrue(elements[0].GetAttribute("class").Contains("#E0F0FA"));
        }

        [StepDefinition(@"I see that owner must appear with its assigned color individually")]
        public void WhenISeeThatOwnerMustAppearWithItsAssignedColorIndividually()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsInPieChartContainer));
            Assert.IsTrue(elements[0].GetAttribute("style").Contains("rgb(0, 144, 59)"));
        }

        [StepDefinition(@"I see that owner must appear with the default color individually")]
        public void WhenISeeThatOwnerMustAppearWithTheDefaultColorIndividually()
        {
            var elements = this.Get<ElementPage>().GetElements(nameof(Resources.OwnersColorsInPieChartContainer));
            Assert.IsTrue(elements[0].GetAttribute("style").Contains("rgb(224, 240, 250)"));
        }
    }
}