// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIToManageOperationalNodeWithOwnershipConfigurationSteps.cs" company="Microsoft">
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
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;
    using NUnit.Framework;
    using Ocaramba.Types;
    using TechTalk.SpecFlow;

    [Binding]
    public class UIToManageOperationalNodeWithOwnershipConfigurationSteps : EcpWebStepDefinitionBase
    {
        [Given(@"I have operational node with ownership data")]
        public async Task GivenIHaveOperationalNodeWithOwnershipDataAsync()
        {
            ////this.Given("I want to create data for Operative Nodes with Ownership");
            await this.NodeOperativeOwnershipExAsync().ConfigureAwait(false);
        }

        [Given(@"I have trained the analytical model with the historical data")]
        public async Task GivenIHaveTrainedTheAnalyticalModelWithTheHistoricalDataAsync()
        {
            var entity = "OperationalNodeWithOwnership";
            await BlobExtensions.UploadFileAsync(ApiContent.ContainerNames[entity], ApiContent.FileNames[entity], ApiContent.FileNames[entity]).ConfigureAwait(false);
            this.FeatureContext[ConstantValues.Status] = await ADataFactoryClient.RunADFPipelineAsync(ApiContent.Pipeline[entity]).ConfigureAwait(false);
        }

        [StepDefinition(@"I select any value (from "".*"" "".*"" "".*"")")]
        public void WhenISelectAnyValueFrom(ElementLocator elementLocator)
        {
            var id = elementLocator?.Value;
            this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(this.FeatureContext[id.Split('_')[2]].ToString());
        }

        [StepDefinition(@"I dont provide value (in ""(.*)"" ""(.*)"" ""(.*)"") in ui")]
        public void WhenIDontProvideInUi(ElementLocator elementLocator)
        {
            this.Get<ElementPage>().GetElements(elementLocator).ElementAt(1).SendKeys(string.Empty);
        }

        [Then(@"I should see all the uploaded data available in the UI")]
        public async Task ThenIShouldSeeAllTheUploadedDataAvailableInTheUIAsync()
        {
            var dataRow = await this.ReadSqlScalarAsync<int>(SqlQueries.GetOperativeMovementsWithOwnershipCount).ConfigureAwait(false);
            var elementCount = this.Get<ElementPage>().FindElementByXPath(Resources.PaginationCount).Text;
            var count = elementCount.Split(' ');
            Assert.AreEqual(dataRow, int.Parse(count[2], CultureInfo.InvariantCulture));
        }

        [Then(@"I should see created TransferRelation information in ""(.*)"" ""(.*)"" ""(.*)""")]
        public async Task ThenIShouldSeeCreatedTransferRelationInformationInGridAsync(ElementLocator elementLocator)
        {
            var sourceNode = this.FeatureContext["logisticSourceCenter"].ToString();
            this.Get<ElementPage>().GetElement(elementLocator).SendKeys(sourceNode);
            SendKeys.SendWait("{ENTER}");
            var elementCount = this.Get<ElementPage>().FindElementByXPath(Resources.PaginationCount).Text;
            var count = elementCount.Split(' ');
            var dataRow = await this.ReadSqlScalarAsync<int>(SqlQueries.GetOperativeNodeRelationshipWithOwnershipOnLogisticSourceCenter, args: new { sourceCenter = sourceNode }).ConfigureAwait(false);
            Assert.AreEqual(dataRow, int.Parse(count[2], CultureInfo.InvariantCulture));
        }
    }
}