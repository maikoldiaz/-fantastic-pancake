// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FeedAndTrainDataToAnalyticalModelSteps.cs" company="Microsoft">
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
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class FeedAndTrainDataToAnalyticalModelSteps : EcpApiStepDefinitionBase
    {
        public FeedAndTrainDataToAnalyticalModelSteps(FeatureContext featureContext)
           : base(featureContext)
        {
        }

        [Given(@"I need to deliver the data to feed and train the analytical model that calculates the property based on historical and current data")]
        public async System.Threading.Tasks.Task GivenINeedToDeliverTheDataToFeedAndTrainTheAnalyticalModelThatCalculatesThePropertyBasedOnHistoricalAndCurrentDataAsync()
        {
            var movementRecord = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestMovementRecord).ConfigureAwait(false);
            this.SetValue(ConstantValues.NetStandardVolume, movementRecord[ConstantValues.NetStandardVolume]);
        }

        [When(@"I obtain the current data from the TRUE system of the operational movements for a given date")]
        public async System.Threading.Tasks.Task WhenIObtainTheCurrentDataFromTheTRUESystemOfTheOperationalMovementsForAGivenDateAsync()
        {
            var expectedData = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestExpectedMovementsPeriodicRecord, args: new { netStandardVolume = this.GetValue(ConstantValues.NetStandardVolume) }).ConfigureAwait(false);
            var actualData = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLatestOperativeMovementsPeriodicRecord, args: new { netStandardVolume = this.GetValue(ConstantValues.NetStandardVolume) }).ConfigureAwait(false);

            Assert.IsTrue(this.VerifyDiffs(expectedData, actualData));
        }

        [Then(@"the generation of operational movements from the TRUE system must be validated and checked in the operational movements table")]
        public void ThenTheGenerationOfOperationalMovementsFromTheTRUESystemMustBeValidatedAndCheckedInTheOperationalMovementsTable()
        {
            this.LogToReport("Covered in previous step");
        }
    }
}
