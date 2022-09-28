// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuAndNavigationSteps.cs" company="Microsoft">
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
    using System.Linq;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.UI;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class MenuAndNavigationSteps : EcpWebStepDefinitionBase
    {
        [When(@"I click on ""(.*)"" toggler")]
        public void WhenIClickOnToggler(string type)
        {
            Assert.IsNotNull(type);
            this.Get<ElementPage>().Click(nameof(Resources.Toggler));
        }

        [When(@"I click on ""(.*)"" link")]
        public void WhenIClickOnLink(string value)
        {
            var page = this.Get<ElementPage>();
            if (value == ConstantValues.Administration)
            {
                page.Click(nameof(Resources.UploadType), UIContent.Conversion[value], 1);
            }
            else
            {
                page.Click(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[value]);
            }
        }

        [Then(@"I should see the below ""(.*)"" Options")]
        public void ThenIShouldSeeTheBelowOptions(string field, Table table)
        {
            foreach (var row in table?.Rows.Select((value) => (Default: value[field], Unused: string.Empty)))
            {
                if (row.Default == ConstantValues.Administration && field == ConstantValues.SubMenu)
                {
                    this.Get<ElementPage>().ScrollIntoView(nameof(Resources.PageIdentifier), row.Default);
                    Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.UploadType), UIContent.Conversion[row.Default], 1).Displayed);
                }
                else
                {
                    Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[row.Default]).Displayed);
                }
            }
        }

        [Then(@"I should see the ""(.*)"" option")]
        public void ThenIShouldSeeTheOption(string value)
        {
            if (value == "Load movements and inventories")
            {
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.UploadType), UIContent.Conversion[value], 2).Displayed);
            }
            else
            {
                Assert.IsTrue(this.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOptionByValue), UIContent.Conversion[value]).Displayed);
            }
        }
    }
}
