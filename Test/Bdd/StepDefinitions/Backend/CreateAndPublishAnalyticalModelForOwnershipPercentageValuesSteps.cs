// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateAndPublishAnalyticalModelForOwnershipPercentageValuesSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Bogus;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;
    using global::Bdd.Core.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TechTalk.SpecFlow;

    [Binding]
    public class CreateAndPublishAnalyticalModelForOwnershipPercentageValuesSteps : EcpApiStepDefinitionBase
    {
        public CreateAndPublishAnalyticalModelForOwnershipPercentageValuesSteps(FeatureContext featureContext)
           : base(featureContext)
        {
        }

        [When(@"I have modified data in ""(.*)"" csv")]
        public async Task WhenIHaveModifiedDataInCsvForOwnershipPercentageValueAsync(string fileName)
        {
            var trueDate = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTrueDate).ConfigureAwait(false);
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            List<string> lines = new List<string>();

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                    {
                        string[] split = line.Split(',');
                        if (!split[2].EqualsIgnoreCase(ConstantValues.OwnershipPercentage))
                        {
                            split[0] = trueDate[ConstantValues.TRUEDATE];
                            split[2] = new Faker().Random.Double(10.01, 99.99).ToString(CultureInfo.InvariantCulture);
                            line = string.Join(",", split);
                        }

                        lines.Add(line);
                    }
                }

                File.WriteAllLines(path, lines);
            }
        }

        [Given(@"I want to create and publish analytical models for ownership percentage values")]
        [When(@"I want to review the historical files of ownership percentage values information")]
        public void GivenIWantToCreateAndPublishAnalyticalModelsForOwnershipPercentageValues()
        {
            // Method intentionally left empty.
        }

        [When(@"I upload the invalid data ""(.*)"" csv file into the blob")]
        public async Task WhenIUploadTheInvalidDataCsvFileIntoTheBlobAsync(string fileName)
        {
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            List<string> lines = new List<string>();

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                    {
                        string[] split = line.Split(',');
                        if (!split[0].EqualsIgnoreCase(ConstantValues.OperationalDate))
                        {
                            split[0] = "Invalid Text";
                            line = string.Join(",", split);
                        }

                        lines.Add(line);
                    }
                }

                File.WriteAllLines(path, lines);
            }

            try
            {
                await BlobExtensions.UploadFileAsync(ApiContent.ContainerNames[fileName], ApiContent.FileNames[fileName], ApiContent.FileNames[fileName]).ConfigureAwait(false);
            }
            catch (NullReferenceException)
            {
                Assert.Fail("File is not loaded into the Blob Storage");
            }
        }

        [Then(@"source system should be ""(.*)""")]
        public async Task ThenSourceSystemShouldBeAsync(string sourceSystem)
        {
            string fileName = this.ScenarioContext[ConstantValues.FileName].ToString();
            var dataRow = await this.ReadSqlScalarAsync<int>(SqlQueries.GetOwnershipPercentageValuesCountWhereSourceSystemIsNotCSV, args: new { sourceSystem }).ConfigureAwait(false);
            Assert.IsFalse(dataRow != 0, "There are records in OwnershipPercentageValues Table where source system is not equal to CSV");
        }

        [When(@"I have information about old records in ""(.*)"" table")]
        public async Task WhenIHaveInformationAboutOldRecordsInTableAsync(string fileName)
        {
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.Counts[fileName]).ConfigureAwait(false);
            this.ScenarioContext[ConstantValues.OldOwnershipPercentageValuesRecords] = dataRow;
        }

        [Then(@"old records are not deleted from ""(.*)"" table")]
        [Then(@"new records are not loaded when ADF pipeline is failed at copy level into ""(.*)""")]
        public async Task ThenOldRecordsAreNotDeletedFromTableAsync(string fileName)
        {
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.Counts[fileName]).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext[ConstantValues.OldOwnershipPercentageValuesRecords], dataRow);
        }

        [Given(@"I have historical data to create analytical models for ""(.*)""")]
        public async Task GivenIHaveHistoricalDataToCreateAnalyticalModelsForOwnershipPercentageValuesAsync(string fileName)
        {
            var trueDate = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetTrueDate).ConfigureAwait(false);
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            List<string> lines = new List<string>();

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                    {
                        string[] split = line.Split(',');
                        if (!split[0].EqualsIgnoreCase(ConstantValues.OperationalDate))
                        {
                            split[0] = trueDate[ConstantValues.TRUEDATE];
                            line = string.Join(",", split);
                        }

                        lines.Add(line);
                    }
                }

                File.WriteAllLines(path, lines);
            }
        }

        [Then(@"I verified in ""(.*)"" table whether data is properly loaded from csv")]
        public async Task ThenIVerifiedInTableWhetherDataIsProperlyLoadedFromCsvAsync(string fileName)
        {
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.Counts[fileName]).ConfigureAwait(false);
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            var lineCount = File.ReadAllLines(path).Length;
            Assert.AreEqual(lineCount - 1, dataRow);
        }

        [Then(@"I verified in ""(.*)"" table whether data is properly updated")]
        [Then(@"loaddate should be current date or loaded date in the ""(.*)"" table")]
        public async Task ThenIVerifiedInOwnershipPercentageValuesTableWhetherDataIsProperlyUpdatedAsync(string fileName)
        {
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.Counts[ConstantValues.ADFUpdatedOwnershipPercentageValuesRecords]).ConfigureAwait(false);
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            var lineCount = File.ReadAllLines(path).Length;
            Assert.AreEqual(lineCount - 1, dataRow);
        }
    }
}