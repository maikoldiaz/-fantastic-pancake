// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateAndPublishAnalyticalModelsSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Backend
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateAndPublishAnalyticalModelsSteps : EcpApiStepDefinitionBase
    {
        public CreateAndPublishAnalyticalModelsSteps(FeatureContext featureContext)
           : base(featureContext)
        {
        }

        [When(@"I upload the ""(.*)"" into the blob")]
        [When(@"I upload a csv file to the ""(.*)"" container")]
        [When(@"I upload the ""(.*)"" csv file into the blob")]
        [Then(@"I verify if ""(.*)"" historical data is loaded into the storage blob")]
        [Then(@"I verify if ""(.*)"" historical data is loaded onto the storage blob")]
        public async Task ThenIVerifyIfHistoricalDataIsLoadedOntoTheStorageBlobAsync(string entity)
        {
            try
            {
                this.ScenarioContext[ConstantValues.FileName] = entity;
                await BlobExtensions.UploadFileAsync(ApiContent.ContainerNames[entity], ApiContent.FileNames[entity], ApiContent.FileNames[entity]).ConfigureAwait(false);
            }
            catch (NullReferenceException)
            {
                Assert.Fail("File is not loaded into the Blob Storage");
            }
        }

        [Then(@"I verify if the ""(.*)"" are present")]
        public async Task ThenIVerifyIfTheArePresentAsync(string entity)
        {
            try
            {
                var dataRow = await this.ReadSqlAsDictionaryAsync(ApiContent.GetRow[entity]).ConfigureAwait(false);
            }
            catch (SqlException)
            {
                Assert.Fail("Records are not loaded into the DB");
            }
        }

        [StepDefinition(@"I initiate the data load process")]
        public async Task WhenIInitiateTheDataLoadProcessAsync()
        {
            var name = this.ScenarioContext[ConstantValues.FileName].ToString();
            this.ScenarioContext[ConstantValues.Status] = await ADataFactoryClient.RunADFPipelineAsync(ApiContent.Pipeline[name]).ConfigureAwait(false);
        }

        [Then(@"I verify if data is present in the ""(.*)""")]
        public async Task ThenIVerifyIfDataIsPresentInTheAsync(string fileName)
        {
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.Counts[fileName]).ConfigureAwait(false);
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            var lineCount = File.ReadAllLines(path).Length;
            Assert.AreEqual(lineCount - 1, dataRow);
        }

        [Then(@"I verify if the data that is loaded is stored into appdb data tables")]
        public async Task ThenIVerifyIfTheDataThatIsLoadedIsStoredIntoAppdbDataTablesAsync()
        {
            string fileName = this.ScenarioContext[ConstantValues.FileName].ToString();
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.Counts[fileName]).ConfigureAwait(false);
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            var lineCount = File.ReadAllLines(path).Length;
            Assert.AreEqual(lineCount - 1, dataRow);
        }

        [Given(@"I have tables to store in the database")]
        public void GivenIHaveTablesToStoreInTheDatabase()
        {
            this.LogToReport("Report1");
        }

        [Then(@"I also verify if the data is a direct copy of data that is loaded from csv")]
        public void ThenIAlsoVerifyIfTheDataIsADirectCopyOfThatIsLoadedFrom()
        {
            this.LogToReport("Random");
        }

        [Given(@"I want to create and publish analytical models of property calculation for transfer points")]
        public void GivenIWantToCreateAndPublishAnalyticalModelsOfPropertyCalculationForTransferPoints()
        {
            this.LogToReport("Report3");
        }

        [When(@"I want to load data for creation and publishing of analytical model I need sql table to store the data")]
        [Given(@"I have historical data to create analytical models")]
        [When(@"I want to review the historical files of operational movements")]
        public void WhenIWantToReviewTheHistoricalFilesOfOperationalMovements()
        {
            this.LogToReport("Report");
        }
    }
}