// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationsAddedInOwnershipCalculationProcessSteps.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.StepDefinitions;
    using global::Bdd.Core.Web.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class ValidationsAddedInOwnershipCalculationProcessSteps : WebStepDefinitionBase
    {
        [StepDefinition(@"I have ownership strategy for node")]
        public async Task GivenIHaveOwnershipStrategyForNodeAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have ownership strategy for node products")]
        public async Task GivenIHaveOwnershipStrategyForNodeProductsAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeProductsWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have ownership strategy for node connections")]
        public async Task GivenIHaveOwnershipStrategyForNodeConnectionsAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeConnectionsWithOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I see ""(.*)"" message for ""(.*)""")]
        public void ThenISeeMessageFor(string message, string messageHeader)
        {
            this.ISeeMessageFor(message, messageHeader);
        }

        [StepDefinition(@"I verify all ""(.*)"" validations passed")]
        public void ThenIVerifyAllValidationsPassed(int totalValidations)
        {
            this.IVerifyAllValidationsPassed(totalValidations);
        }

        [StepDefinition(@"I have ownership strategy removed for node")]
        public async Task GivenIHaveOwnershipStrategyRemovedForNodeAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.DeleteNodeOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have ownership strategy removed for product")]
        public async Task GivenIHaveOwnershipStrategyRemovedForProductAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.DeleteNodeProductsOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have ownership strategy removed for connections")]
        public async Task GivenIHaveOwnershipStrategyRemovedForConnectionsAsync()
        {
            await this.ReadSqlAsDictionaryAsync(SqlQueries.DeleteNodeConnectionsOwnershipStrategy, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have ownership strategy for node connections with priority")]
        public async Task GivenIHaveOwnershipStrategyForNodeConnectionsWithPriorityAsync()
        {
            await this.GivenIHaveOwnershipStrategyForNodeConnectionsAsync().ConfigureAwait(false);
        }

        [StepDefinition(@"I have active ownership strategy for node")]
        public async Task GivenIHaveActiveOwnershipStrategyForNodeAsync()
        {
            await this.GivenIHaveOwnershipStrategyForNodeAsync().ConfigureAwait(false);
        }

        [StepDefinition(@"I see ""(.*)"" error message for ""(.*)""")]
        public async Task ThenISeeErrorMessageForAsync(string expectedmessage, string messageHeader)
        {
            if ((expectedmessage != null) && expectedmessage.Contains("node name"))
            {
                int validationCount = 0;
                string actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.GetValidationMessage), messageHeader).Text;
                var nodeNames = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetNodeNamesOfSegment, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
                var nodeNamesList = nodeNames.ToDictionaryList();
                var messages = expectedmessage.Split(new string[] { "[node name]" }, StringSplitOptions.None);
                for (int i = 0; i < nodeNamesList.Count; i++)
                {
                    if (string.Equals(actualMessage, messages[0] + nodeNamesList[i][ConstantValues.Name].ToString() + messages[1], StringComparison.OrdinalIgnoreCase))
                    {
                        validationCount += 1;
                    }
                }

                Assert.IsTrue(validationCount > 0);
            }

            if ((expectedmessage != null) && expectedmessage.Contains("source node - destination node"))
            {
                int validationCount = 0;
                string actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.GetValidationMessage), messageHeader).Text;
                var nodeConnections = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetAllNodeConnectionsOfSegment, args: new { segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
                var nodeConnectionsList = nodeConnections.ToDictionaryList();
                var messages = expectedmessage.Split(new string[] { "[source node - destination node]" }, StringSplitOptions.None);
                for (int i = 0; i < nodeConnectionsList.Count; i++)
                {
                    var sourceId = nodeConnectionsList[i][ConstantValues.SourceNodeId];
                    var destinationId = nodeConnectionsList[i][ConstantValues.DestinationNodeId];
                    var sourceName = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNodeNameFromNodeId, args: new { nodeId = sourceId.ToString() }).ConfigureAwait(false);
                    var destinationName = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetNodeNameFromNodeId, args: new { nodeId = destinationId.ToString() }).ConfigureAwait(false);
                    if (string.Equals(actualMessage, messages[0] + sourceName[ConstantValues.Name].ToString() + " - " + destinationName[ConstantValues.Name].ToString() + messages[1], StringComparison.OrdinalIgnoreCase))
                    {
                        validationCount += 1;
                    }
                }
            }

            if ((expectedmessage != null) && expectedmessage.Contains("inactiveOwnership strategy name"))
            {
                if (expectedmessage.Contains("Nodo") && !expectedmessage.Contains("Nodo - producto"))
                {
                    await this.GivenIHaveOwnershipStrategyForNodeAsync().ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.DeleteInactiveNodeOwnershipStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"] }).ConfigureAwait(false);
                }
                else if (expectedmessage.Contains("Conexión - Producto"))
                {
                    await this.GivenIHaveOwnershipStrategyForNodeConnectionsAsync().ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.DeleteInactiveConnectionOwnershipStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"] }).ConfigureAwait(false);
                }
                else if (expectedmessage.Contains("Nodo - producto"))
                {
                    await this.GivenIHaveOwnershipStrategyForNodeProductsAsync().ConfigureAwait(false);
                    await this.ReadSqlAsDictionaryAsync(SqlQueries.DeleteInactiveProductOwnershipStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"] }).ConfigureAwait(false);
                }

                int validationCount = 0;
                string actualMessage = this.Get<ElementPage>().GetElement(nameof(Resources.GetValidationMessage), messageHeader).Text;
                var messages = expectedmessage.Split(new string[] { "[inactiveOwnership strategy name]" }, StringSplitOptions.None);
                if (string.Equals(actualMessage, messages[0] + this.ScenarioContext["ownershipStratergy"].ToString() + messages[1], StringComparison.OrdinalIgnoreCase))
                {
                        validationCount += 1;
                }

                Assert.IsTrue(validationCount > 0);
            }
        }

        [StepDefinition(@"I have node with inactive ownership strategy for from FICO")]
        public async Task GivenIHaveNodeWithInactiveOwnershipStrategyForFromFICOAsync()
        {
            ////this.Given("I have Fico Connection setup into the system");
            await this.IHaveFicoConnectionSetupIntoTheSystemAsync().ConfigureAwait(false);
            ////this.When("I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: \"BUSCA_ESTRATEGIA\" and estado: \"false\"");
            await this.IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync("BUSCA_ESTRATEGIA", "false").ConfigureAwait(false);
            var jsonresult = JObject.Parse(this.ScenarioContext["Result"].ToString());
            var keyLayer = jsonresult["volPayload"]["volOutput"].SelectToken("estrategiaPropiedadNodo");
            for (int j = 0; j < keyLayer[0].Count(); j++)
            {
                var data = keyLayer[0];
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                this.ScenarioContext["ownershipId"] = values.ToList()[0].ToInt();
                this.ScenarioContext["ownershipStratergy"] = values.ToList()[1].ToString();
            }

            await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertInactiveNodeOwnershipStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"], ruleName = this.ScenarioContext["ownershipStratergy"].ToString() }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateInactiveNodeStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"], segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have inactive ownership strategy for product connection from FICO")]
        public async Task GivenIHaveInactiveOwnershipStrategyForProductConnectionFromFICOAsync()
        {
            ////this.Given("I have Fico Connection setup into the system");
            await this.IHaveFicoConnectionSetupIntoTheSystemAsync().ConfigureAwait(false);
            ////this.When("I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: \"BUSCA_ESTRATEGIA\" and estado: \"false\"");
            await this.IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync("BUSCA_ESTRATEGIA", "false").ConfigureAwait(false);
            var jsonresult = JObject.Parse(this.ScenarioContext["Result"].ToString());
            var keyLayer = jsonresult["volPayload"]["volOutput"].SelectToken("estrategiaConexiones");
            for (int j = 0; j < keyLayer[0].Count(); j++)
            {
                var data = keyLayer[0];
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                this.ScenarioContext["ownershipId"] = values.ToList()[0].ToInt();
                this.ScenarioContext["ownershipStratergy"] = values.ToList()[1].ToString();
            }

            await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertInactiveConnectioneOwnershipStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"], ruleName = this.ScenarioContext["ownershipStratergy"].ToString() }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateInactiveConnectionStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"], segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }

        [StepDefinition(@"I have inactive ownership strategy for node product from FICO")]
        public async Task GivenIHaveInactiveOwnershipStrategyForNodeProductFromFICOAsync()
        {
            ////this.Given("I have Fico Connection setup into the system");
            await this.IHaveFicoConnectionSetupIntoTheSystemAsync().ConfigureAwait(false);
            ////this.When("I invoke the FICO service to fetch the strategies with input parameter tipoLlamada: \"BUSCA_ESTRATEGIA\" and estado: \"false\"");
            await this.IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync("BUSCA_ESTRATEGIA", "false").ConfigureAwait(false);
            var jsonresult = JObject.Parse(this.ScenarioContext["Result"].ToString());
            var keyLayer = jsonresult["volPayload"]["volOutput"].SelectToken("estrategiaPropiedadNodoProducto");
            for (int j = 0; j < keyLayer[0].Count(); j++)
            {
                var data = keyLayer[0];
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                this.ScenarioContext["ownershipId"] = values.ToList()[0].ToInt();
                this.ScenarioContext["ownershipStratergy"] = values.ToList()[1].ToString();
            }

            await this.ReadSqlAsDictionaryAsync(SqlQueries.InsertInactiveProductOwnershipStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"], ruleName = this.ScenarioContext["ownershipStratergy"].ToString() }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateInactiveProductStrategy, args: new { ruleId = this.ScenarioContext["ownershipId"], segment = this.ScenarioContext["SegmentName"].ToString() }).ConfigureAwait(false);
        }
    }
}