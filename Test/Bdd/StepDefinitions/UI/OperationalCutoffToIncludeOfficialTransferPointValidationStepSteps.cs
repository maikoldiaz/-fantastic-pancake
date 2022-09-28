// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationalCutoffToIncludeOfficialTransferPointValidationStepSteps.cs" company="Microsoft">
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

    using AventStack.ExtentReports.Utils;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using NUnit.Framework;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class OperationalCutoffToIncludeOfficialTransferPointValidationStepSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"validate transfer point movements last updated record without GlobalMovementId are displayed in ""(.*)"" grid")]
        [StepDefinition(@"validate transfer point movements without GlobalMovementId are displayed in ""(.*)"" grid")]
        public void ThenValidateTransferPointMovementsWithoutGlobalMovementIdAreDisplayedInGrid(string gridName)
        {
            var page = this.Get<ElementPage>();
            var gridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: gridName.ToCamelCase());
            int count = 0;
            for (int i = 1; i <= gridRow.Count; i++)
            {
                var arguments = new object[] { gridName.ToCamelCase(), i, 2 };
                var movementId = page.GetElement(nameof(Resources.GetEachRowByEachColumnInGrid), formatArgs: arguments).Text;
                for (int j = 1; j <= 2; j++)
                {
                    if (movementId == this.ScenarioContext["Movement" + j].ToString())
                    {
                        Assert.IsTrue(movementId.EqualsIgnoreCase(this.ScenarioContext["Movement" + j].ToString()));
                        count++;
                        break;
                    }
                }
            }

            Assert.AreEqual(gridRow.Count, count);
        }

        [Then(@"validate transfer point movement with GlobalMovementId for any of event is not displayed in ""(.*)"" grid")]
        public void ThenValidateTransferPointMovementWithGlobalMovementIdForAnyOfEventIsNotDisplayedInGrid(string gridName)
        {
            var page = this.Get<ElementPage>();
            var gridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: gridName.ToCamelCase());
            Assert.AreEqual(1, gridRow.Count);
            var arguments = new object[] { gridName.ToCamelCase(), 1, 2 };
            var movementId = page.GetElement(nameof(Resources.GetEachRowByEachColumnInGrid), formatArgs: arguments).Text;
            Assert.IsFalse(movementId.EqualsIgnoreCase(this.ScenarioContext["Movement2"].ToString()));
        }

        [Then(@"validate that error icon is enabled for movement with error reported by SAP PO in ""(.*)"" grid")]
        [Then(@"Validate transfer point movement with error reported by SAP PO are displayed in ""(.*)"" grid")]
        public void ThenValidateTransferPointMovementWithErrorReportedBySAPPOAreDisplayedInGrid(string gridName)
        {
            var page = this.Get<ElementPage>();
            var gridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: gridName.ToCamelCase());
            for (int i = 1; i <= gridRow.Count; i++)
            {
                var arguments = new object[] { gridName.ToCamelCase(), i, 2 };
                var movementId = page.GetElement(nameof(Resources.GetEachRowByEachColumnInGrid), formatArgs: arguments).Text;
                if (movementId.EqualsIgnoreCase(this.ScenarioContext["Movement1"].ToString()))
                {
                    Assert.IsTrue(movementId.EqualsIgnoreCase(this.ScenarioContext["Movement1"].ToString()));
                    arguments = new object[] { gridName.ToCamelCase(), i, gridName.ToCamelCase() };
                    var errorIcon = page.GetElement(nameof(Resources.GetErrorIconOfEachRow), formatArgs: arguments);
                    Assert.IsTrue(errorIcon.Enabled);
                }
            }
        }

        [Then(@"I click on error icon movement with error reported by SAP PO in ""(.*)"" grid")]
        public void ThenIClickOnErrorIconMovementWithErrorReportedBySAPPOInGrid(string gridName)
        {
            var page = this.Get<ElementPage>();
            var gridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: gridName.ToCamelCase());
            for (int i = 1; i <= gridRow.Count; i++)
            {
                var arguments = new object[] { gridName.ToCamelCase(), i, 2 };
                var movementId = page.GetElement(nameof(Resources.GetEachRowByEachColumnInGrid), formatArgs: arguments).Text;
                if (movementId.EqualsIgnoreCase(this.ScenarioContext["Movement1"].ToString()))
                {
                    Assert.IsTrue(movementId.EqualsIgnoreCase(this.ScenarioContext["Movement1"].ToString()));
                    arguments = new object[] { gridName.ToCamelCase(), i, gridName.ToCamelCase() };
                    var errorIcon = page.GetElement(nameof(Resources.GetErrorIconOfEachRow), formatArgs: arguments);
                    errorIcon.Click();
                }
            }
        }

        [Then(@"validate that error icon is disabled for transfer point movements without error reported by SAP PO and without GlobalMovementId in ""(.*)"" grid")]
        public void ThenValidateThatErrorIconIsDisabledForTransferPointMovementsWithoutErrorReportedBySAPPOAndWithoutGlobalMovementIdInGrid(string gridName)
        {
            var page = this.Get<ElementPage>();
            var gridRow = page.GetElements(nameof(Resources.GridRow), formatArgs: gridName.ToCamelCase());
            for (int i = 1; i <= gridRow.Count; i++)
            {
                var arguments = new object[] { gridName.ToCamelCase(), i, 2 };
                var movementId = page.GetElement(nameof(Resources.GetEachRowByEachColumnInGrid), formatArgs: arguments).Text;
                if (movementId.EqualsIgnoreCase(this.ScenarioContext["Movement2"].ToString()))
                {
                    Assert.IsTrue(movementId.EqualsIgnoreCase(this.ScenarioContext["Movement2"].ToString()));
                    arguments = new object[] { gridName.ToCamelCase(), i, gridName.ToCamelCase() };
                    var errorIcon = page.GetElement(nameof(Resources.GetErrorIconOfEachRow), formatArgs: arguments);
                    Assert.IsFalse(errorIcon.GetAttribute("disabled").IsNullOrEmpty());
                }
            }
        }

        [Then(@"validate movement are marked as official and notes added")]
        public async Task ThenValidateMovementAreMarkedAsOfficialAndNotesAddedAsync()
        {
            for (int i = 1; i <= 2; i++)
            {
                var movementRow = await this.ReadSqlAsStringDictionaryAsync(input: UIContent.GetRow["GetMovementByMovementId"], args: new { movementId = this.ScenarioContext["Movement" + 1].ToString() }).ConfigureAwait(false);
                var movementTransId = movementRow["MovementTransactionId"];
                var isOfficial = movementRow["IsOfficial"].ToString();
                Assert.IsTrue(isOfficial.EqualsIgnoreCase("True"));
                var sapTrackingRow = await this.ReadSqlAsStringDictionaryAsync(input: UIContent.GetRow["GetSapTrackingByMovementTransactionId"], args: new { movementTransactionId = movementTransId }).ConfigureAwait(false);
                var comment = sapTrackingRow["Comment"];
                Assert.IsTrue(comment.EqualsIgnoreCase(ConstantValues.ValidValue));
            }
        }

        [Then(@"validate message ""(.*)"" (in "".*"" "".*"")")]
        public void ThenValidateMessageIn(string expectedMessage, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var actualMessage = page.GetElement(elementLocator).Text;
            Assert.IsTrue(expectedMessage.EqualsIgnoreCase(actualMessage));
        }

        [Then(@"validate the selected ""(.*)"" (in "".*"" "".*"")")]
        public void ThenValidateTheSelectedIn(string field, ElementLocator elementLocator)
        {
            var page = this.Get<ElementPage>();
            var actualValue = page.GetElement(elementLocator).Text;
            if (field.EqualsIgnoreCase("MovementId"))
            {
                Assert.IsTrue(this.ScenarioContext["Movement1"].ToString().EqualsIgnoreCase(actualValue));
            }
            else if (field.EqualsIgnoreCase("MovementType"))
            {
                Assert.IsTrue(this.GetValue("MovementTypeName").EqualsIgnoreCase(actualValue));
            }
            else if (field.EqualsIgnoreCase("OperationalDate"))
            {
                var expectedDate = DateTime.Now.AddDays(-3).ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToString();
                Assert.IsTrue(expectedDate.EqualsIgnoreCase(actualValue));
            }
            else if (field.EqualsIgnoreCase("ErrorMessage"))
            {
                var expectedMessage = "Test";
                Assert.IsTrue(expectedMessage.EqualsIgnoreCase(actualValue));
            }
        }

        [Then(@"validate (that "".*"" "".*"") is selected with current date minus one")]
        public void ThenValidateThatIsSelectedWithCurrentDateMinusOne(ElementLocator elementLocator)
        {
            var actualValue = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value");
            var expectedDate = DateTime.Now.AddDays(-1).ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToString();
            Assert.IsTrue(expectedDate.EqualsIgnoreCase(actualValue));
        }

        [Then(@"validate (that "".*"" "".*"") is empty")]
        public void ThenValidateThatIsEmpty(ElementLocator elementLocator)
        {
            var actualValue = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("value");
            Assert.IsTrue(actualValue.IsNullOrEmpty());
        }

        [Then(@"validate ""(.*)"" is displayed (in "".*"" "".*"")")]
        public void ThenValidateIsDisplayedIn(string expectedMessage, ElementLocator elementLocator)
        {
            var actualMessae = this.Get<ElementPage>().GetElement(elementLocator).Text;
            Assert.IsTrue(expectedMessage.EqualsIgnoreCase(actualMessae));
        }

        [Then(@"validate that ""(.*)"" is not displayed")]
        public void ThenValidateThatIsNotDisplayed(string elementName)
        {
            if (elementName.EqualsIgnoreCase("OfficialPointInfoMessage"))
            {
                Assert.IsFalse(this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.OfficialPointInfoMessage)));
            }
        }
    }
}
