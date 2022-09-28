// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterNodeStatusToBlockchainSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.DataSources;
    using Ecp.True.Bdd.Tests.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class RegisterNodeStatusToBlockchainSteps : EcpApiStepDefinitionBase
    {
        private readonly BlockchainDataSource blockchainDataSource = new BlockchainDataSource();

        public RegisterNodeStatusToBlockchainSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        [When(@"I have Movements in the system with this node connection as source or destination")]
        public void WhenIHaveMovementsInTheSystemWithThisNodeConnectionAsSourceOrDestination()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I Perform operational cutoff for them")]
        public void WhenIPerformOperationalCutoffForThem()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I update the order of node connection-connections in a segment")]
        public void WhenIUpdateTheOrderOfNodeConnectionConnectionsInASegment()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I Perform ownership calculation")]
        public void WhenIPerformOwnershipCalculation()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I verify if the status for the node has been updated to ""(.*)""")]
        public async Task ThenIVerifyIfTheStatusForTheNodeHasBeenUpdatedToAsync(string p0)
        {
            var nodeId = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeId).ConfigureAwait(false);
            int data = int.Parse(nodeId["NodeId"], CultureInfo.InvariantCulture);
            await Task.Delay(120000).ConfigureAwait(true);
            var parameters = new Dictionary<string, object>() { { "nodeId", data } };
            Console.WriteLine(p0);
            var result = await this.blockchainDataSource.GetDataAsync<NodeDetailsStruct>("NodeDetailsFactory", "Get", parameters).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext["NodeName"], result.Name);
        }

        [Then(@"I verify if the status for the node connection has been updated to ""(.*)"" in blockchain register")]
        public void ThenIVerifyIfTheStatusForTheNodeConnectionHasBeenUpdatedToInBlockchainRegister(string p1)
        {
            Console.WriteLine(p1);
            ScenarioContext.Current.Pending();
        }

        [Then(@"I verify the status of the node connection in the blockchain register is '(.*)'")]
        public void ThenIVerifyTheStatusOfTheNodeConnectionInTheBlockchainRegisterIs(string p2)
        {
            Console.WriteLine(p2);
            ScenarioContext.Current.Pending();
        }
    }
}
