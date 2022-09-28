// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangesinAnalyticalModelSteps.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class ChangesinAnalyticalModelSteps : EcpApiStepDefinitionBase
    {
        public ChangesinAnalyticalModelSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [Then(@"Modified Records from ""(.*)"" to ""(.*)"" date should NOT be loaded into ""(.*)""")]
        public async Task ThenModifiedRecordsFromToDateShouldNOTBeLoadedInAsync(string fromDate, string toDate, string fileName)
        {
            Assert.AreEqual("Successful", this.ScenarioContext[ConstantValues.Status]);
            var dataRow = await this.ReadSqlScalarAsync<int>(ApiContent.CountsWithDifferentOperationalDate[fileName], args: new { startDate = fromDate, endDate = toDate }).ConfigureAwait(false);
            Assert.IsTrue(dataRow == 0);
        }

        [Then(@"Modified Records other than ""(.*)"" to ""(.*)"" date should be loaded into ""(.*)""")]
        public async Task ThenModifiedRecordsOtherThanToDateShouldBeLoadedInAsync(string fromDate, string toDate, string fileName)
        {
            // Actual values stored in the Database
            var dbData = await this.ReadAllSqlAsDictionaryAsync(ApiContent.CountWithoutHistoricalInformations[fileName], args: new { startDate = fromDate, endDate = toDate }).ConfigureAwait(false);
            var dbDataList = dbData.ToDictionaryList();

            // Expected values reading from csv file
            var path = (@"TestData\Input\" + ApiContent.FileNames[fileName] + ".csv").GetFullPath();
            var dictionaries = new List<Dictionary<string, string>>();
            var lines = File.ReadLines(path).ToList();
            var keys = lines.FirstOrDefault().Split(',').ToList();
            lines.RemoveAt(0);
            lines.Take(5).ToList().ForEach(x =>
            {
                dictionaries.Add(new Dictionary<string, string>());
                var values = x.Split(',').ToList();
                var dict = dictionaries.LastOrDefault();
                int index = 0;
                values.ForEach(y =>
                {
                    dict.Add(keys[index++], y);
                });
            });

            // Comparission of actual and expected values
            for (int i = 0; i < dictionaries.Count; i++)
            {
                Assert.AreEqual(dbDataList[i][keys[0]], dictionaries[i][keys[0]].ToDateTime());
                for (int j = 1; j < keys.Count - 1; j++)
                {
                    Assert.AreEqual(dbDataList[i][keys[j]].ToString(), dictionaries[i][keys[j]]);
                }

                Assert.AreEqual(dbDataList[i][keys[keys.Count - 1]], dictionaries[i][keys[keys.Count - 1]].ToDecimal());
            }
        }
    }
}
