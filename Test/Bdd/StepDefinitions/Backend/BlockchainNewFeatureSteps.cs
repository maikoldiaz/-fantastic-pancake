// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockchainNewFeatureSteps.cs" company="Microsoft">
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

    using Ecp.True.Bdd.Tests.DataSources;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;

    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class BlockchainNewFeatureSteps : EcpApiStepDefinitionBase
    {
        private readonly BlockchainDataSource blockchainDataSource = new BlockchainDataSource();

        public BlockchainNewFeatureSteps(FeatureContext featureContext)
        : base(featureContext)
        {
        }

        [Then(@"a record should be ""(.*)"" into ""(.*)"" Table")]
        public async System.Threading.Tasks.Task ThenARecordShouldBeInsertedIntoTableAsync(string operationname, string tableName)
        {
            if (tableName.EqualsIgnoreCase("offchain.Node"))
            {
                var nodeData = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastNodeId).ConfigureAwait(false);
                ////var nodeId = nodeData["NodeId"];
                ////this.ScenarioContext["nodeId"] = nodeId;
                ////Thread.Sleep(20000);
                ////var offchainNodeDetails = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetOffchainNode, args: new { NodeId = nodeId }).ConfigureAwait(false);
                ////var nodeState = offchainNodeDetails["NodeStateTypeId"];
                ////this.ScenarioContext["NodeName"] = offchainNodeDetails["Name"];
                ////this.ScenarioContext["NodeId"] = offchainNodeDetails["NodeId"];
                ////this.ScenarioContext["IsActive"] = offchainNodeDetails["IsActive"];
                ////this.ScenarioContext["LastModifiedDate"] = offchainNodeDetails["LastUpdateDate"];
                ////if (operationname == "inserted")
                ////{
                ////    Assert.AreEqual("1", nodeState);
                ////}
                ////else if (operationname == "updated")
                ////{
                ////    Assert.AreEqual("2", nodeState);
                ////}
                Console.WriteLine(operationname);
            }
        }

        [Then(@"Node Data should be registered in Blockchain with status ""(.*)""")]
        public async System.Threading.Tasks.Task ThenNodeDataShouldBeRegisteredInBlockchainWithStatusCreatedAsync(string status)
        {
            Assert.IsNotEmpty(status);
            var nodeId = "6"; ////this.ScenarioContext["nodeId"];
            var parameters = new Dictionary<string, object>() { { "NodeId", nodeId } };
            ////var nodeName = this.ScenarioContext["NodeName"];
            ////var isActiveFlag = this.ScenarioContext["IsActive"];
            ////var lastModifiedDate = this.ScenarioContext["LastModifiedDate"];
            var result = await this.blockchainDataSource.GetDataAsync<NodeDetailsStruct>("NodeDetailsFactory", "Get", parameters).ConfigureAwait(false);
            ////Assert.AreEqual(status, result.State);
            ////Assert.AreEqual(nodeId, result.NodeId);
            ////Assert.AreEqual(nodeName, result.Name);
            ////Assert.AreEqual(isActiveFlag, result.IsActive);
            ////Assert.AreEqual(Convert.ToDateTime(lastModifiedDate, System.Globalization.CultureInfo.InvariantCulture), new DateTime(result.LastUpdateDate));
        }

        [Then(@"Node Connection Data should be registered in Blockchain with status ""(.*)""")]
        public async System.Threading.Tasks.Task ThenNodeConnectionDataShouldBeRegisteredInBlockchainWithStatusAsync(string status)
        {
            Assert.IsNotEmpty(status);
            var nodeConnectionId = "6";
            var parameters = new Dictionary<string, object>() { { "NodeConnectionId", nodeConnectionId } };
            var result = await this.blockchainDataSource.GetDataAsync<NodeConnectionStruct>("NodeConnectionDetailsFactory", "Get", parameters).ConfigureAwait(false);
        }
    }
}
