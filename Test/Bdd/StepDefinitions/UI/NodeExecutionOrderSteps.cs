// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeExecutionOrderSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class NodeExecutionOrderSteps : EcpWebStepDefinitionBase
    {
        [When(@"I provide the ""(.*)"" (for "".*"" "".*"")")]
        public void WhenIProvideTheFor(string value, ElementLocator elementLocator)
        {
            value = ConstantValues.OrderNumber;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(value);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }

        [Then(@"it should be registered in the system with with execution order number")]
        public async System.Threading.Tasks.Task ThenItShouldBeRegisteredInTheSystemWithWithExecutionOrderNumberAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastNode).ConfigureAwait(false);
            Assert.AreEqual(lastRow[ConstantValues.Order], ConstantValues.OrderNumber);
        }

        [Then(@"I should return to edit mode without save information")]
        public void ThenIShouldReturnToEditModeWithoutSaveInformation()
        {
            Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.SubmitButton)).Enabled);
        }

        [When(@"I click (on "".*"" "".*"") without order number")]
        public void WhenIClickOnWithoutOrderNumber(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().Click(elementLocator);
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(OpenQA.Selenium.Keys.Tab);
        }

        [Then(@"it should be updated in the system with with execution order number")]
        public async System.Threading.Tasks.Task ThenItShouldBeUpdatedInTheSystemWithWithExecutionOrderNumberAsync()
        {
            var lastRow = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastUpdatedNode).ConfigureAwait(false);
            Assert.AreEqual(lastRow[ConstantValues.Order], ConstantValues.OrderNumber);
        }
    }
}
