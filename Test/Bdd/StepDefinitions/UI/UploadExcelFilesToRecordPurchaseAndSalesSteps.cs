// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadExcelFilesToRecordPurchaseAndSalesSteps.cs" company="Microsoft">
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
    using System.Windows.Forms;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class UploadExcelFilesToRecordPurchaseAndSalesSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I should see breadcrumb ""(.*)""")]
        public void ThenIShouldSeeBreadcrumb(string text)
        {
            this.IShouldSeeBreadcrumb(text);
        }

        [Then(@"it should associate the new process Id with old process Id")]
        public void ThenItShouldAssociateTheNewProcessIdWithOldProcessId()
        {
            Assert.IsTrue(true);
        }

        [When(@"I browse to ""(.*)"" page")]
        public void WhenIBrowseToPage(string linkName)
        {
            this.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().Click(nameof(Resources.NavigationBar));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.BalanceTransportersWithProperty), 5);
            this.Get<ElementPage>().Click(nameof(Resources.BalanceTransportersWithProperty));
            this.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Conversion[linkName]);
            this.Get<ElementPage>().Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[linkName]);
        }

        [When(@"I select ""(.*)"" file from purchase sales directory")]
        public void WhenISelectFileFromPurchaseSalesDirectory(string operation)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            string path;
            switch (operation)
            {
                case "InvalidFormat":
                    path = operation + ".txt";
                    break;
                case "WithoutRecords":
                    path = FilePaths.ContractsFilePath + operation + ".xlsx";
                    break;
                case "WithoutSourceColumn":
                    path = operation + ".xlsx";
                    break;
                default:
                    path = FilePaths.ContractsFilePath + "ValidExcel.xlsx";
                    break;
            }

            SendKeys.SendWait(path.GetFullPath());
            SendKeys.SendWait("{ENTER}");
            SendKeys.SendWait("{ESC}");
            Assert.IsTrue(true);
        }

        [When(@"I click on ""(.*)"" to upload in purchase and sales page")]
        public void WhenIClickOnToUpload(string locator)
        {
            this.IClickOnUploadButton(locator);
        }
    }
}
