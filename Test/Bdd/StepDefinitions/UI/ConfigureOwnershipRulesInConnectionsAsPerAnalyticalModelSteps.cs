// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureOwnershipRulesInConnectionsAsPerAnalyticalModelSteps.cs" company="Microsoft">
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
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ocaramba.Types;

    using TechTalk.SpecFlow;

    [Binding]
    public class ConfigureOwnershipRulesInConnectionsAsPerAnalyticalModelSteps : EcpWebStepDefinitionBase
    {
        [Then(@"verify the list of models listed in ""(.*)"" dropdown are as per master data")]
        public async System.Threading.Tasks.Task ThenVerifyTheListOfModelsListedInDropdownAreAsPerMasterDataAsync(string field)
        {
            var algorithms = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAlgorithms).ConfigureAwait(false);
            var expectedAlgorithmsList = algorithms.ToDictionaryList();
            var dropDownBox = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[0].Click();
            var actualAlgorithms = this.Get<ElementPage>().GetElements(nameof(Resources.SelectBoxMenu), formatArgs: UIContent.Conversion[field]);
            string[] stringSeparators = new string[] { "\r\n" };
            string[] actualAlgorithmsList = actualAlgorithms[0].Text.Split(stringSeparators, StringSplitOptions.None);
            await Task.Delay(3000).ConfigureAwait(false);
            Assert.AreEqual(expectedAlgorithmsList.Count, actualAlgorithmsList.Length);
            //// expectedAlgorithmsList.Equals(actualAlgorithmsList);
            foreach (var algorithm in expectedAlgorithmsList)
            {
                bool flag = false;
                for (int i = 0; i < actualAlgorithmsList.Length; i++)
                {
                    if (algorithm[ConstantValues.ModelName].Equals(actualAlgorithmsList[i]))
                    {
                        flag = true;
                        break;
                    }
                }

                Assert.IsTrue(flag);
            }
        }

        [Then(@"saved ""(.*)"" value should be disassociated from the connection")]
        public void ThenSavedValueShouldBeDisassociatedFromTheConnection(string field)
        {
            Assert.IsNotNull(field);
            Assert.IsTrue(string.IsNullOrEmpty(this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromConnectionsMainGrid), formatArgs: UIContent.GridPosition[field]).Text));
        }

        [Then(@"verify (that "".*"" "".*"" "".*"") should be ""(.*)""")]
        public void ThenVerifyThatShouldBe(ElementLocator elementLocator, string expectedValue)
        {
            string isChecked = this.Get<ElementPage>().GetElement(elementLocator).GetAttribute("checked");
            if (isChecked == null)
            {
                Assert.AreEqual("unchecked", expectedValue);
            }
            else
            {
                Assert.AreEqual("checked", expectedValue);
            }
        }

        [Then(@"I should not see current ownership function in the grid")]
        public void ThenIShouldNotSeeCurrentOwnershipFunctionInTheGrid()
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.PropertyFunctionColumn));
        }

        [Then(@"I should not be able to set or edit new ownership function in the grid")]
        public void ThenIShouldNotBeAbleToSetOrEditNewOwnershipFunctionInTheGrid()
        {
            ////this.When("I click on \"connectionProducts\" \"editRules\" \"link\" of a combination not having value");
            this.IClickOn("connectionProducts\" \"editRules", "link");
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.NewPropertyFunctionLabel));
        }

        [Then(@"the changes should be updated in ""(.*)"" column")]
        public void ThenTheChangesShouldBeUpdatedInColumn(string field)
        {
            Assert.IsNotNull(field);
            Assert.AreEqual(this.GetValue(Entities.Keys.SelectedValue), this.Get<ElementPage>().GetElement(nameof(Resources.GetValueFromConnectionsMainGrid), formatArgs: UIContent.GridPosition[field]).Text);
        }
    }
}