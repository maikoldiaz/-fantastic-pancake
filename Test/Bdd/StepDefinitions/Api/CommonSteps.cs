// <copyright file="CommonSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Ecp.True.Bdd.Tests.StepDefinitions.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Entities;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Microsoft.CSharp.RuntimeBinder;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using OfficeOpenXml;

    using TechTalk.SpecFlow;

    [Binding]
    public class CommonSteps : EcpApiStepDefinitionBase
    {
        public CommonSteps(FeatureContext featureContext)
            : base(featureContext)
        {
        }

        // Using the Step-Argument Transformation from Transformations class in the last parameter here
        [Given(@"I am (authenticated as "".*"")")]
        public async Task GivenIAmLoggedIntoAsUserAsync(Credentials userCredentials)
        {
            this.SetValue(nameof(this.UserDetails), userCredentials);
            await this.TokenForUserAsync(this.ApiExecutor, userCredentials).ConfigureAwait(false);
        }

        [Given(@"I am authenticated for ""(.*)"" service")]
        public async Task GivenIAmLoggedIntoAsUserAsync(string service)
        {
            await this.TokenForServiceAsync(this.ApiExecutor, service).ConfigureAwait(false);
        }

        [When(@"I Get all records")]
        [When(@"I Query all records")]
        public async Task WhenIGetAllRecordsAsync()
        {
            this.Endpoint = this.Endpoint.Replace("api", "odata");
            await this.SetResultsAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegment(this.GetValue<string>(Entities.Keys.Route))).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response status should be successful")]
        public void ThenTheResponseStatusShouldBe()
        {
            this.VerifyThat(() => Assert.IsTrue(this.GetValue<bool>("Status")));
        }

        [Then(@"the response should succeed with message ""(.*)""")]
        public async Task ThenTheResponseShouldSucceedWithMessageAsync(string expectedMessage)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.VerifyThat(() => Assert.AreEqual(this.GetValue<dynamic>(Entities.Keys.InitialRowCount), expectedMessage.ContainsIgnoreCase("homologación  eliminado") ? dbResults.Count() + 1 : dbResults.Count()));
            var result = this.GetValue<dynamic>(Entities.Keys.Result);
#pragma warning disable IDE0019 // Use pattern matching
            var response = result as HttpResponseMessage;
#pragma warning restore IDE0019 // Use pattern matching
            var jsonObject = response != null ? JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false)) : result;
            this.VerifyThat(() => Assert.AreEqual(expectedMessage, jsonObject["message"].ToString()));
        }

        [Then(@"the response should fail with message ""(.*)""")]
        public async Task ThenTheResponseShouldFailWithMessageAsync(string expectedMessage)
        {
            try
            {
                var jsonResult = JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false));
                var actualMessage = (jsonResult["message"] == null) ? jsonResult["errorCodes"][0]["message"].ToString() : jsonResult["message"].ToString();
                this.VerifyThat(() => Assert.AreEqual(expectedMessage, actualMessage));
                if (!expectedMessage.EqualsIgnoreCase("Algoritmo no definido para el punto de transferencia") && this.ScenarioContext.ContainsKey(Entities.Keys.InitialRowCount))
                {
                    var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
                    this.VerifyThat(() => Assert.AreEqual(this.GetValue<dynamic>(Entities.Keys.InitialRowCount), dbResults.Count()));
                }
            }
            catch (RuntimeBinderException)
            {
                var apiResults = this.GetValue<dynamic>(Entities.Keys.Results);
                var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
                if (apiResults.Count != 0)
                {
                    this.VerifyThat(() => Assert.AreNotEqual(dbResults.Count(), 0));
                }
                else
                {
                    JObject.Parse(await this.GetValue<dynamic>(Entities.Keys.Error).ReadAsStringAsync().ConfigureAwait(false));
                }
            }
            catch (JsonReaderException)
            {
                this.VerifyThat(() => Assert.AreEqual(expectedMessage, this.GetValue<HttpContent>(Entities.Keys.Error).ReadAsStringAsync().Result));
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.VerifyThat(() => Assert.Fail(expectedMessage, ex.ToString()));
            }
        }

        [Given(@"I want to create a ""(.*)"" in the system")]
        public void GivenIWantToCreateAInTheSystem(string entity)
        {
            this.SetValue(Entities.Keys.Route, ApiContent.Routes[entity]);
            this.SetValue(Entities.Keys.EntityType, entity);
        }

        [When(@"I don't provide ""(.*)""")]
        public async Task WhenIDonTProvideAsync(string field)
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, field.EqualsIgnoreCase("description") || field.EqualsIgnoreCase("Connection Products") || field.EqualsIgnoreCase("UncertaintyPercentage") || field.EqualsIgnoreCase("ControlLimit") || field.EqualsIgnoreCase("Owners") ? dbResults.Count() + 1 : dbResults.Count());
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content;
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
            if (!field.EqualsIgnoreCase("name"))
            {
                content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? content : content.JsonChangeValue();
            }

            content = entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.NodeContentCreationAsync(content).ConfigureAwait(false) : content;
            content = content.JsonChangePropertyValue(field);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide the required fields")]
        public async Task WhenIProvideTheRequiredFieldsForTheAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            if (entity != ConstantValues.OperationalCutoff && entity != ConstantValues.FileRegistration)
            {
                var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
                this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
                content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content.JsonChangeValue();
                content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
                content = entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.NodeContentCreationAsync(content).ConfigureAwait(false) : content;
            }

            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide inactive Node details while creating node connection")]
        public async Task WhenIProvideInactiveNodeDetailsWhileCreatingNodeConnectionAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            if (entity != ConstantValues.OperationalCutoff && entity != ConstantValues.FileRegistration)
            {
                var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
                this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
                content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateInactiveSourceAndDestinationNodesAsync(content).ConfigureAwait(false) : content.JsonChangeValue();
                content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? await this.CreateOwnersAsync(content).ConfigureAwait(false) : content;
            }

            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" value as ""(.*)"" and without ""(.*)"" for connection")]
        public async Task WhenIProvideValueAsAndWithoutForConnectionAsync(string field1, string field1Value, string field2)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count() + 1);
            content = await this.CreateSourceAndDestinationNodesAsync(content).ConfigureAwait(false);
            content = await this.CreateOwnersAsync(content).ConfigureAwait(false);
            content = content.JsonChangePropertyValue(field1, field1Value);
            content = content.JsonChangePropertyValue(field2);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" that exceeds (.*) characters")]
        public async Task WhenIProvideThatExceedsCharactersAsync(string field, int limit)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            this.SetValue(Entities.Keys.RandomFieldValue, new Faker().Random.AlphaNumeric(limit + 1));
            content = content.JsonChangePropertyValue(field, this.GetValue(Entities.Keys.RandomFieldValue));
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" that contains any alphabet")]
        [When(@"I provide ""(.*)"" that contains special characters other than expected")]
        public async Task WhenIProvideThatContainsSpecialCharactersOtherThanAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            this.SetValue(Entities.Keys.RandomFieldValue, string.Concat($"Automation#", new Faker().Random.AlphaNumeric(5)));
            content = content.JsonChangePropertyValue(field, this.GetValue(Entities.Keys.RandomFieldValue));
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I don't provide any values")]
        public async Task WhenIDonTProvideAnyValuesAsync()
        {
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            var entity = this.GetValue(Entities.Keys.EntityType);
            this.LogToReport(JToken.Parse("{}"));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse("{}")).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"the Node data has been modified")]
        [When(@"I update a record with required fields")]
        public async Task WhenIUpdateWithRequiredFieldsAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            updateContent = await this.UpdateRequestJsonIdsAsync(updateContent, entity).ConfigureAwait(false);
            var lastCreatedRowDetails = await this.ReadAllSqlAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            var lastCreated = lastCreatedRowDetails.ToDictionaryList();
            updateContent = updateContent.JsonChangePropertyValue(ConstantValues.NodeRowVersion, lastCreated[0]["RowVersion"].ToString());
            updateContent = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? updateContent.JsonChangeValue("description", "_Mod_") : updateContent.JsonChangeValue("name", "_Mod_");
            dynamic data = JObject.Parse(updateContent);
            string result = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? data["description"] : data["name"];
            this.SetValue(Entities.Keys.RandomFieldValue, result);
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.UpdateContentForNodeAsync(updateContent).ConfigureAwait(false) : updateContent;
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record without ""(.*)""")]
        public async Task WhenIUpdateWithoutAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            updateContent = await this.UpdateRequestJsonIdsAsync(updateContent, entity).ConfigureAwait(false);
            updateContent = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? updateContent.JsonChangeValue("description", "_Mod_") : updateContent.JsonChangeValue();
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.UpdateContentForNodeAsync(updateContent).ConfigureAwait(false) : updateContent;
            updateContent = updateContent.JsonChangePropertyValue(field);
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record with ""(.*)"" that exceeds (.*) characters")]
        public async Task WhenIUpdateWithThatExceedsCharactersAsync(string field, int limit)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            content = entity.EqualsIgnoreCase(ConstantValues.ManageConnection) ? content.JsonChangeValue("description", "_Mod_") : content.JsonChangeValue("name", "_Mod_");
            this.SetValue(Entities.Keys.RandomFieldValue, new Faker().Random.AlphaNumeric(limit + 1));
            content = content.JsonChangePropertyValue(field, this.GetValue(Entities.Keys.RandomFieldValue));
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get record with invalid Id")]
        public async Task WhenIGetByInvalidIdAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], "-123")).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I Get record with valid Id")]
        public async Task WhenIGetWithValidIdAsync()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string idValue = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            this.ScenarioContext[ApiContent.Ids[entity]] = idValue;
            await this.SetResultAsync(async () => await this.GetAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity], idValue)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I provide an existing ""(.*)""")]
        public async Task WhenIProvideAnExistingAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Creates[entity];
            content = content.JsonChangeValue();
            content = entity.EqualsIgnoreCase(ConstantValues.Node) ? await this.NodeContentCreationAsync(content).ConfigureAwait(false) : content;
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[field.ToPascalCase()];
            content = content.JsonChangePropertyValue(field, fieldValue);
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Then(@"the response should return all valid records")]
        public async Task ThenTheResponseShouldReturnAllValidRecordsAsync()
        {
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Results);
            var jObjectResult = JObject.Parse(apiResults.ToString());
            var content = jObjectResult.value;
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            Assert.AreEqual(dbResults.Count(), content.Count);
        }

        [Then(@"the response should return requested record details")]
        public void ThenTheResponseShouldReturnRequestedDetails()
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var apiResults = this.GetValue<dynamic>(Entities.Keys.Result);
            if (entity.EqualsIgnoreCase(ConstantValues.ManageConnection))
            {
                this.VerifyThat(() => Assert.AreEqual(this.ScenarioContext[ConstantValues.SourceNode], apiResults[ConstantValues.SourceNode.ToCamelCase()].ToString()));
                this.VerifyThat(() => Assert.AreEqual(this.ScenarioContext[ConstantValues.DestinationNode], apiResults[ConstantValues.DestinationNode.ToCamelCase()].ToString()));
            }
            else
            {
                Assert.AreEqual(this.ScenarioContext[ApiContent.Ids[entity]], apiResults[ApiContent.Ids[entity]].ToString());
            }
        }

        [Then(@"the response should have the message ""(.*)""")]
        public void ThenTheResponseShouldHaveTheMessage(string expectedMessage)
        {
            var jsonResult = JObject.Parse(this.GetValue<dynamic>(Entities.Keys.Result));
            this.VerifyThat(() => Assert.AreEqual(expectedMessage, jsonResult["errorCodes"][0]["message"].ToString()));
        }

        [When(@"I update a record with existing ""(.*)""")]
        public async Task WhenIUpdateWithExistingAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var updateContent = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            string fieldValue = lastCreatedRow[field.ToPascalCase()];
            var createContent = ApiContent.Creates[entity];
            createContent = createContent.JsonChangeValue();
            createContent = entity.EqualsIgnoreCase(ConstantValues.Node) || entity.EqualsIgnoreCase(ConstantValues.Nodes) ? await this.NodeContentCreationAsync(createContent).ConfigureAwait(false) : createContent;
            this.SetValue(Entities.Keys.Result, await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[entity]), JObject.Parse(createContent)).ConfigureAwait(false));
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
            lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[entity]).ConfigureAwait(false);
            this.ScenarioContext["EntityId"] = lastCreatedRow[ApiContent.Ids[entity].ToPascalCase()];
            updateContent = updateContent.JsonChangePropertyValue(field, fieldValue);
            updateContent = updateContent.JsonChangePropertyValue(ApiContent.Ids[entity], this.ScenarioContext["EntityId"].ToString());
            updateContent = entity.EqualsIgnoreCase(ConstantValues.Node) || entity.EqualsIgnoreCase(ConstantValues.Nodes) ? await this.UpdateContentForNodeAsync(updateContent).ConfigureAwait(false) : updateContent;
            this.LogToReport(JToken.Parse(updateContent));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(updateContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [When(@"I update a record with ""(.*)"" that contains special characters other than expected")]
        public async Task WhenIUpdateARecordWithThatContainsSpecialCharactersOtherThanExpectedAsync(string field)
        {
            var entity = this.GetValue(Entities.Keys.EntityType);
            var content = ApiContent.Updates[entity].Replace("^^placeholder^^", string.Empty);
            this.SetValue(Entities.Keys.RandomFieldValue, string.Concat($"Automation#", new Faker().Random.AlphaNumeric(5)));
            content = content.JsonChangePropertyValue(field, this.GetValue(Entities.Keys.RandomFieldValue));
            this.LogToReport(JToken.Parse(content));
            await this.SetResultAsync(async () => await this.PutAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[entity]), JObject.Parse(content)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Given(@"I don't have any ""(.*)"" in the system")]
        public async Task GivenIDonTHaveAnyInTheSystemAsync(string entity)
        {
            this.SetValue(Entities.Keys.EntityType, entity);
            this.SetValue(Entities.Keys.Route, ApiContent.Routes[entity]);
            var dbResults = await this.Output.ReadAllAsync<dynamic>().ConfigureAwait(false);
            this.SetValue(Entities.Keys.InitialRowCount, dbResults.Count());
        }

        [StepDefinition(@"I have segment category in the system")]
        public async Task IHaveSegementCategoryIntheSystemAsync()
        {
            await this.SegementCategoryIntheSystemAsync().ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" active ""(.*)"" segments available")]
        public async Task GivenIHaveActiveSegmentsAvailableAsync(int requiredSegmentsCount, string segmentType)
        {
            for (int currentIndex = 0; currentIndex < requiredSegmentsCount; currentIndex++)
            {
                await this.IHaveSegementCategoryIntheSystemAsync().ConfigureAwait(false);

                if (segmentType.Equals("son", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        this.ScenarioContext["SonSegmentsAdded"] = this.ScenarioContext["SonSegmentsAdded"] + ":" + this.ScenarioContext["CategoryElementName"].ToString();
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        this.ScenarioContext["SonSegmentsAdded"] = this.ScenarioContext["CategoryElementName"].ToString();
                    }

                    await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateChainSegmentAsASonSegment, args: new { name = this.ScenarioContext["CategoryElementName"].ToString() }).ConfigureAwait(false);
                }
                else
                {
                    try
                    {
                        this.ScenarioContext["ChainSegmentsAdded"] = this.ScenarioContext["ChainSegmentsAdded"] + ":" + this.ScenarioContext["CategoryElementName"].ToString();
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        this.ScenarioContext["ChainSegmentsAdded"] = this.ScenarioContext["CategoryElementName"].ToString();
                    }
                }
            }
        }

        [Given(@"I have System category in the system")]
        public async Task IHaveSystemCategoryIntheSystemAsync()
        {
            await this.SystemCategoryIntheSystemAsync().ConfigureAwait(false);
        }

        [Given(@"I have Node with Segment Category")]
        public async Task IHaveNodeinthesystemAsync()
        {
            await this.NodeWithSegmentCategoryAsync().ConfigureAwait(false);
        }

        [Given(@"I want to register a ""(.*)"" in the system")]
        public async Task GivenIWantToRegisterAnInTheSystemAsync(string entity)
        {
            await this.IWantToRegisterAnExcelHomologationInTheSystemAsync(entity, this).ConfigureAwait(false);
        }

        [Given(@"I want create TestData for Operational Cutoff ""(.*)""")]
        public async Task TestDataSetupForOperationCutOffAsync(string days)
        {
            await this.TestDataForOperationCutOffAsync(days).ConfigureAwait(false);
        }

        [Given(@"I want create TestData for ownershipnodes")]
        public async Task CreateTestDataForOwnershipAsync()
        {
            await this.TestDataForOwnershipCalculationAsync(this).ConfigureAwait(false);
        }

        [Given(@"I want create TestData for Operational Transformation")]
        public async Task TestDataSetupForOperationalTransformationAsync()
        {
            await this.DataSetupForOperationalTransformationAsync().ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" Transformation data in the system")]
        public async Task GivenIHaveTransformationDataInTheSystemAsync(string type)
        {
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            this.ScenarioContext["Count"] = "0";
            this.ScenarioContext[ConstantValues.Segment] = string.Empty;
            if (type.EqualsIgnoreCase("Inventory"))
            {
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Origin_NodeId", this.GetValue("NodeId"));
                var nodeRowOrigin = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Origin_NodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Origin_NodeName"] = nodeRowOrigin[ConstantValues.Name];
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Destination_NodeId", this.GetValue("NodeId"));
                var nodeRowDestination = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Destination_NodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Destination_NodeName"] = nodeRowDestination[ConstantValues.Name];
            }
            else if (type.EqualsIgnoreCase("Movement"))
            {
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Origin_SourceNodeId", this.GetValue("NodeId"));
                var nodeRowOrigin = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Origin_NodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Origin_NodeName"] = nodeRowOrigin[ConstantValues.Name];
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Destination_SourceNodeId", this.GetValue("NodeId"));
                var nodeRowDestination = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Destination_SourceNodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Destination_NodeName"] = nodeRowDestination[ConstantValues.Name];
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Origin_DestinationNodeId", this.GetValue("NodeId"));
                var nodetRowOriginDestination = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Origin_NodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Origin_DestinationNodeName"] = nodetRowOriginDestination[ConstantValues.Name];
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Destination_DestinationNodeId", this.GetValue("NodeId"));
                var nodetRowDestinationDestination = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Origin_NodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Destination_DestinationName"] = nodetRowDestinationDestination[ConstantValues.Name];

                // Create Connections between created nodes
                await this.CreateNodeConnectionAsync(this.GetValue("Origin_SourceNodeId"), this.GetValue("Destination_SourceNodeId"), 2, "0.07", "0.06").ConfigureAwait(false);
                await this.CreateNodeConnectionAsync(this.GetValue("Origin_DestinationNodeId"), this.GetValue("Destination_DestinationNodeId"), 2, "0.05", "0.04").ConfigureAwait(false);
            }
            else if (type.EqualsIgnoreCase("EditInventory"))
            {
                await this.CreateNodesStepsAsync().ConfigureAwait(false);
                this.SetValue("Destination_NodeId", this.GetValue("NodeId"));
                var nodeRowDestination = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNode, args: new { nodeId = this.GetValue("Destination_NodeId") }).ConfigureAwait(false);
                this.ScenarioContext["Destination_NodeName"] = nodeRowDestination[ConstantValues.Name];
            }
        }

        [Given(@"I have ""(.*)"" Transformation in the system")]
        public async Task GivenIHaveTransformationInTheSystemAsync(string type)
        {
            if (type.EqualsIgnoreCase("Movement"))
            {
                await this.GivenIHaveTransformationDataInTheSystemAsync(type).ConfigureAwait(false);
                ////this.When("I navigate to \"TransformSettings\" page");
                this.UiNavigation("TransformSettings");
                ////this.When("I click on \"Transformation\" \"button\"");
                this.IClickOn("Transformation", "button");
                ////this.Then("I should see \"Transformation\" \"Movements\" \"Create\" \"Form\"");
                this.IShouldSee("Transformation\" \"Movements\" \"Create", "Form");
                ////this.When("I provide value for \"Origin\" \"NodeOrigin\" \"Textbox\" on \"Movement\" Interface");
                this.IProvideValueForOnInterface("Origin\" \"NodeOrigin", "Textbox", "Movement");
                ////this.When("I select any \"NodeDestination\" from \"Origin\" \"DestinationNode\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("NodeDestination", "Origin\" \"DestinationNode", "dropdown", "Movement");
                ////this.When("I select any \"SourceProduct\" from \"Origin\" \"SourceProduct\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("SourceProduct", "Origin\" \"SourceProduct", "dropdown", "Movement");
                ////this.When("I select any \"DestinationProduct\" from \"Origin\" \"DestinationProduct\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("DestinationProduct", "Origin\" \"DestinationProduct", "dropdown", "Movement");
                ////this.When("I select any \"Unit\" from \"Origin\" \"MeasurementUnit\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("Unit", "Origin\" \"MeasurementUnit", "dropdown", "Movement");
                ////this.When("I provide value for \"Destination\" \"NodeOrigin\" \"Textbox\" on \"Movement\" Interface");
                this.IProvideValueForOnInterface("Destination\" \"NodeOrigin", "Textbox", "Movement");
                ////this.When("I select any \"NodeDestination\" from \"Destination\" \"DestinationNode\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("NodeDestination", "Destination\" \"DestinationNode", "dropdown", "Movement");
                ////this.When("I select any \"SourceProduct\" from \"Destination\" \"SourceProduct\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("SourceProduct", "Destination\" \"SourceProduct", "dropdown", "Movement");
                ////this.When("I select any \"DestinationProduct\" from \"Destination\" \"DestinationProduct\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("DestinationProduct", "Destination\" \"DestinationProduct", "dropdown", "Movement");
                ////this.When("I select any \"Unit\" from \"Destination\" \"MeasurementUnit\" \"dropdown\" on \"Movement\" Interface");
                this.ISelectAnyFromOnInterface("Unit", "Destination\" \"MeasurementUnit", "dropdown", "Movement");
                ////this.When("I click on \"Element\" \"Submit\" \"button\"");
                this.IClickOn("Element\" \"Submit", "button");
            }
            else if (type.EqualsIgnoreCase("Inventory"))
            {
                await this.GivenIHaveTransformationDataInTheSystemAsync(type).ConfigureAwait(false);
                ////this.When("I navigate to \"TransformSettings\" page");
                this.UiNavigation("TransformSettings");
                ////this.When("I click on \"Inventories\" tab");
                this.IClickOnTab("Inventories");
                ////this.When("I click on \"Transformation\" \"button\"");
                this.IClickOn("Transformation", "button");
                ////this.Then("I should see \"Transformation\" \"Inventories\" \"Create\" \"Form\"");
                this.IShouldSee("Transformation\" \"Inventories\" \"Create", "Form");
                ////this.When("I provide value for \"Origin\" \"Node\" \"Textbox\" on \"Inventory\" Interface");
                this.IProvideValueForOnInterface("Origin\" \"Node", "Textbox", "Inventory");
                ////this.When("I select any \"SourceProduct\" from \"Origin\" \"SourceProduct\" \"dropdown\" on \"Inventory\" Interface");
                this.ISelectAnyFromOnInterface("SourceProduct", "Origin\" \"SourceProduct", "dropdown", "Inventory");
                ////this.When("I select any \"Unit\" from \"Origin\" \"MeasurementUnit\" \"dropdown\" on \"Inventory\" Interface");
                this.ISelectAnyFromOnInterface("Unit", "Origin\" \"MeasurementUnit", "dropdown", "Inventory");
                ////this.When("I provide value for \"Destination\" \"Node\" \"Textbox\" on \"Inventory\" Interface");
                this.IProvideValueForOnInterface("Destination\" \"Node", "Textbox", "Inventory");
                ////this.When("I select any \"SourceProduct\" from \"Destination\" \"SourceProduct\" \"dropdown\" on \"Inventory\" Interface");
                this.ISelectAnyFromOnInterface("SourceProduct", "Destination\" \"SourceProduct", "dropdown", "Inventory");
                ////this.When("I select any \"Unit\" from \"Destination\" \"MeasurementUnit\" \"dropdown\" on \"Inventory\" Interface");
                this.ISelectAnyFromOnInterface("Unit", "Destination\" \"MeasurementUnit", "dropdown", "Inventory");
                ////this.When("I click on \"Element\" \"Submit\" \"button\"");
                this.IClickOn("Element\" \"Submit", "button");
            }
        }

        [Given(@"I have ""(.*)"" homologation data in the system")]
        public async Task GivenIHaveHomologationDataInTheSystemAsync(string type)
        {
            await this.IHaveHomologationDataInTheSystemAsync(systemType: type, this).ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" information in the system")]
        public async Task GivenIHaveInformationInTheSystemAsync(string type)
        {
            await this.IHaveInformationInTheSystemAsync(type).ConfigureAwait(false);
        }

        [Given(@"Ownership is Calculated for Segment and Ticket is Generated and ""(.*)"" Events are processed")]
        public async Task GivenIHaveCalculatedOwnershipForSegmentAndTicketGeneratedAsync(string eventsCondition)
        {
            await this.IHaveCalculatedOwnershipForSegmentAndTicketGeneratedAsync(eventsCondition).ConfigureAwait(false);
        }

        [Given(@"Ownership is Calculated for Segment and Ticket is Generated")]
        public async Task OwnershipForSegmentAndTicketGeneratedAsync()
        {
            await this.CalculatedOwnershipForSegmentAndTicketGeneratedAsync().ConfigureAwait(false);
        }

        [Given(@"I create Nodes in the system")]
        public async Task GivenICreateNodesInTheSystemAsync()
        {
            this.ScenarioContext["Count"] = "0";
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
        }

        [Given(@"I have valid Movements and Inventory in the system")]
        public async Task GivenIHaveValidMovementsAndInventoryInTheSystemAsync()
        {
            await this.GivenIWantToRegisterAnInTheSystemAsync("Homologation").ConfigureAwait(true);
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"ValidExcel\" file from directory");
            await this.ISelectFileFromDirectoryAsync("ValidExcel").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.Then("it should be registered in the system");
            await this.ShouldBeRegisteredInTheSystemAsync().ConfigureAwait(false);
        }

        [Given(@"I have valid official Inventory with same identifier in the system")]
        [Given(@"I have valid official Movements with same identifier in the system")]
        public async Task GivenIHaveValidOfficialMovementsAndInventoryInTheSystemAsync()
        {
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "Excel", this).ConfigureAwait(true);
            ////this.When("I update the \"TestData_Official\" excel");
            this.IUpdateTheExcelForOfficialMovements("TestData_Official");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"TestData_Official\" file from directory");
            await this.ISelectFileFromDirectoryAsync("TestData_Official").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I have pending transactions in the system")]
        public async Task GivenIHavePendingTransactionsInTheSystemAsync()
        {
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await this.CreateCatergoryElementAsync("2").ConfigureAwait(false);
            var categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext["CategoryElement"] }).ConfigureAwait(false);
            this.SetValue("CategorySegment", categoryElementRow[ConstantValues.Name]);
            ////this.When("I update the excel with \"ValidExcel\"");
            this.IUpdateTheExcelFile("ValidExcel");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"ValidExcel\" file from directory");
            await this.ISelectFileFromDirectoryAsync("ValidExcel").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
        }

        [Given(@"I have ownership calculation data generated in the system")]
        public async Task GivenIHaveOwnershipCalculationDataGeneratedInTheSystemAsync()
        {
            await this.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);
        }

        [Given(@"I have ownership calculation data generated in the system with fico endpoint")]
        public async Task GivenIHaveOwnershipCalculationDataGeneratedInTheSystemWithFicoEndpointAsync()
        {
            this.SetValue(ConstantValues.FICO, "Yes");
            await this.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);
        }

        [Given(@"I have segments nodes movements and inventories created")]
        public async Task GivenIHaveSegmentsNodesMovementsAndInventoriesCreatedAsync()
        {
            await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
        }

        [Given(@"I have ownership calculation data generated in the system for official delta")]
        public async Task GivenIHaveOwnershipCalculationDataGeneratedInTheSystemForOfficialDeltaAsync()
        {
            await this.IHaveOwnershipCalculationDataGeneratedInTheSystemForOfficialDeltaAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Generates the data including segment, nodes and products (one product for each node)
        /// Then uploads the excel 'TestData_AnalyticalModel'.
        /// Performs operational cutoff.
        /// </summary>
        /// <returns>returns nothing.</returns>
        [Given(@"I have ownership calculation data generated in the system conditionally")]
        public async Task GivenIHaveOwnershipCalculationDataGeneratedInTheSystemConditionallyAsync()
        {
            ////this.SetValue("OwnershipForAnalytical", "Yes");
            ////await this.GivenIHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(this.GetValue("NodeStatus")))
            {
                ////this.Given("I have \"ownershipnodes\" created for node status");
                await this.GivenIHaveCreatedForNodeStatusAsync("ownershipnodes").ConfigureAwait(false);
            }
            else
            {
                ////this.Given("I have \"ownershipnodes\" created");
                await this.IHaveOwnershipNodesCreatedAnalyticalModelAsync("ownershipnodes").ConfigureAwait(false);
            }

            ////this.When("I navigate to \"Operational Cutoff\" page");
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            if (!string.IsNullOrEmpty(this.GetValue("OwnershipOperation")))
            {
                ////this.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Cutoff\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Cutoff").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(this.GetValue("NodeStatus")))
            {
                ////this.When("I select the FinalDate lessthan \"10\" days from CurrentDate on \"Cutoff\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(10, "Cutoff").ConfigureAwait(false);
            }
            else
            {
                ////this.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
                await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
            }

            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"Submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"Submit", "button");

            this.CompleteVerifyMessagingProcess();
            this.CompleteTransferPointsProcess();
            this.CompleteCheckConsistencyProcess();

            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
            ////this.When("I navigate to \"ownershipcalculation\" page");
            this.UiNavigation("ownershipcalculation");
            ////this.When("I click on \"NewBalance\" \"button\"");
            this.IClickOn("NewBalance", "button");
            ////this.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
            ////this.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Ownership\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Ownership").ConfigureAwait(false);
            ////this.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
            ////this.When("I verify all validations passed");
            this.IVerifyValidationsPassed();
            ////this.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
            this.IClickOn("ownershipCalulationValidations\" \"submit", "button");
            ////this.When("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
            this.IClickOn("ownershipCalculationConfirmation\" \"submit", "button");
            await Task.Delay(10000).ConfigureAwait(true);
            ////this.Then("verify the ownership is calculated successfully");
            await this.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
            ////this.When("I wait till ownership ticket geneation to complete");
            await this.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I have operational cutoff data generated in the system")]
        public async Task GivenIHaveOperationalCutoffDataGeneratedInTheSystemAsync()
        {
            ////this.Given("I have \"ownershipnodes\" created");
            await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
            ////this.When("I navigate to \"Operational Cutoff\" page");
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            ////this.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"Submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all pending records from grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"Submit\" \"button\"");
            this.IClickOn("AddComment\" \"Submit", "button");
            ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
            this.IClickOn("consistencyCheck\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"submit\" \"button\"");
            this.IClickOn("AddComment\" \"submit", "button");
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
        }

        [Given(@"I have data with logistic center information")]
        public async Task CreateLogisticCenterDetailsTestDataAsync()
        {
            this.ScenarioContext["Count"] = "0";
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

            // Create Nodes
            await this.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await this.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await this.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await this.CreateHomologationForExcelAsync().ConfigureAwait(false);

            // Homologation between 1 to 5
            await this.CreateHomologationForTrueToSivAsync().ConfigureAwait(false);

            ////this.When("I update the excel with \"TestData_logistic\" data");
            this.WhenIUpdateTheExcelFileWithDaywiseData("TestData_logistic");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"TestData_logistic\" file from directory");
            await this.ISelectFileFromDirectoryAsync("TestData_logistic").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I want to create testdata for Inactive Configuration Validation")]
        public async Task TestDataSetupForInactiveNetworkConfigurationAsync()
        {
            await this.DataSetupForInactiveNetworkConfigurationAsync().ConfigureAwait(false);
        }

        [Given(@"Creating a Connection between 2 ""(.*)"" Nodes")]
        public async Task CreateNodeConnectionsForGraphicalValidationsAsync(string state)
        {
            await this.GraphicalNodeConnectionAsync(state, this).ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" category element in the system")]
        public async Task GivenIHaveCategoryElementInTheSystemAsync(string category)
        {
            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            var categoryRow = await this.ReadSqlAsStringDictionaryAsync(ApiContent.GetRow["CategoryByName"], args: new { categoryName = category }).ConfigureAwait(false);
            await this.CreateCatergoryElementAsync(categoryRow["CategoryId"]).ConfigureAwait(false);
            this.ScenarioContext[category + "Id"] = await this.GetLastCreatedRowIdAsync("Category Element").ConfigureAwait(false);
            var categoryElementRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = this.ScenarioContext[category + "Id"] }).ConfigureAwait(false);
            this.ScenarioContext[category + "Name"] = categoryElementRow[ConstantValues.Name];
        }

        [Given(@"I want to create testdata for network configuration")]
        public async Task TestDataSetupForNetworkConfigurationAsync()
        {
            await this.DataSetupForNetworkConfigurationAsync().ConfigureAwait(false);
        }

        [Given(@"I want to create data for Operative Nodes with Ownership")]
        public async Task OperativeNodeOwnershipAsync()
        {
            await this.NodeOperativeOwnershipAsync().ConfigureAwait(false);
        }

        [Given(@"Ownership is Calculated for Segment and Ticket is Generated and ""(.*)"" Contracts are processed")]
        public async Task GivenIHaveCalculatedOwnershipForSegmentwithContractsAndTicketGeneratedAsync(string contractsCondition)
        {
            await this.UserCalculatedOwnershipForSegmentwithContractsAndTicketGeneratedAsync(contractsCondition).ConfigureAwait(false);
        }

        [Given(@"Ownership is Calculated for Segment and Ticket is Generated and ""(.*)"" Events of same sagments are processed")]
        public async Task IHaveCalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedAsync(string eventsCondition)
        {
            await this.GivenIHaveCalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedAsync(eventsCondition).ConfigureAwait(false);
        }

        [StepDefinition(@"I update xml ""(.*)"" ""(.*)"" with ""(.*)""")]
        public async Task IUpdateXmlWithAsync(string field, string xmlField, string fieldValue)
        {
            await this.UpdateXmlWithAsync(field, xmlField, fieldValue).ConfigureAwait(false);
        }

        [StepDefinition(@"I create test data for contracts to generate movements")]
        public async Task ContractswithsamesegmentAsync()
        {
            await this.SameSegmentContractsAsync().ConfigureAwait(false);
        }

        [StepDefinition(@"I create test data for events to generate movements")]
        public async Task EventswithsamesegmentAsync()
        {
            var uploadFileName = @"TestData\Input\Planning, Programming and Collaboration agreements\EventsforMovements.xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // For Events
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            // Updating Date Range of Events
            worksheet.Cells[2, 9].Value = DateTime.UtcNow.AddDays(-2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[2, 10].Value = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[3, 9].Value = DateTime.UtcNow.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[3, 10].Value = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            package.Save();
            package.Dispose();

            ////this.Given("I am authenticated as \"admin\"");
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            var nodeDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            this.ScenarioContext.Add("NodeName_1", nodeDetails[ConstantValues.Name]);
            nodeDetails = await this.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            this.ScenarioContext.Add("NodeName_2", nodeDetails[ConstantValues.Name]);
            await this.CreateNodeConnectionAsync(this.GetValue("NodeId_1"), this.GetValue("NodeId_2"), 2, "0.07", "0.06").ConfigureAwait(false);
            await this.CreateOwnershipRulesForNodeConnectionsAsync(this.GetValue("NodeConnectionId")).ConfigureAwait(false);

            // Delete Homologation between 5 to 1
            try
            {
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForEvent]).ConfigureAwait(false);
                this.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    this.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = this.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await this.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = this.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }

            // Create Homologation between 5 to 1 For Nodes
            var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForEventNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(this.GetValue("NodeId_1"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(this.GetValue("NodeId_2"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Product
            var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
            setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Owner
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 5);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

            // Create Homologation between 5 to 1 For EventType
            var setupHomologationRequestForEventType = ApiContent.Creates[ConstantValues.HomologationForEventType];
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForEventType)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" created for node status")]
        public async Task GivenIHaveCreatedForNodeStatusAsync(string ownershipNodes)
        {
            await this.IHaveCreatedForNodeStatusAsync(ownershipNodes).ConfigureAwait(false);
        }

        [Given(@"I have ownership for node status")]
        public async Task GivenIHaveOwnershipForNodeStatusAsync()
        {
            this.SetValue("NodeStatus", "Yes");
            ////this.Given("I have ownership calculation data generated in the system");
            await this.GivenIHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);
        }

        [Given(@"I have nodes of the segment for the selected period already have an operational cutoff executed")]
        public async Task IHaveNodesOfTheSegmentForTheSelectedPeriodAlreadyHaveAnOperationalCutoffExecutedAsync()
        {
            this.SetValue("InitialCutOff", "Yes");
            ////this.Given("I have \"ownershipnodes\" created");
            await this.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
            this.Given(@"I update the segment to SON");
            ////this.When("I navigate to \"Operational Cutoff\" page");
            this.UiNavigation("Operational Cutoff");
            ////this.When("I click on \"NewCut\" \"button\"");
            this.When(@"I refresh the current page");
            this.IClickOn("NewCut", "button");
            ////this.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
            this.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
            ////this.When("I select the StartDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheStartDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
            ////this.When("I select the EndDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
            await this.ISelectTheEndDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
            ////this.When("I click on \"InitTicket\" \"next\" \"button\"");
            this.IClickOn("InitTicket\" \"Submit", "button");
            ////this.When("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("validateInitialInventory\" \"Submit", "button");
            ////this.When("I click on \"checkInventories\" \"Next\" \"button\"");
            this.IClickOn("validateInitialInventory\" \"Submit", "button");
            if (!this.Get<ElementPage>().GetElement(nameof(Resources.ErrorGridNextButton)).Enabled)
            {
                this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
                ////this.When("I select all pending records from grid");
                this.ISelectAllPendingRepositroriesFromGrid();
                ////this.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
                this.IClickOn("ErrorsGrid\" \"AddNote", "button");
                ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                ////this.When("I click on \"AddComment\" \"Submit\" \"button\"");
                this.IClickOn("AddComment\" \"Submit", "button");
                ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
                this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            }

            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"officialPointsGrid\" \"Next\" \"button\"");
            this.IClickOn("officialPointsGrid\" \"Submit", "button");
            this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            ////this.When("I select all unbalances in the grid");
            this.ISelectAllPendingRepositroriesFromGrid();
            ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
            this.IClickOn("consistencyCheck\" \"AddNote", "button");
            ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
            this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
            ////this.When("I click on \"AddComment\" \"submit\" \"button\"");
            this.IClickOn("AddComment\" \"submit", "button");
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
            ////this.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
            this.IClickOn("ConfirmCutoff\" \"Submit", "button");
            ////this.When("I wait till cutoff ticket processing to complete");
            await this.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" group '(.*)' before first day of cutoff and '(.*)' on the first day of cutoff")]
        public async Task IHaveGroupBeforeFirstDayOfCutoffAndOnTheFirstDayOfCutoffAsync(string group, string inventory, string movement)
        {
            if (string.IsNullOrEmpty(this.GetValue("Count")))
            {
                this.ScenarioContext["Count"] = "0";
                ////this.Given("I am authenticated as \"admin\"");
                await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

                // create elements
                await this.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);
            }

            // Create Nodes
            await this.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // Create Connection
            await this.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);
            this.SetValue(group + "_Group_NodeId_1", this.GetValue("NodeId_3"));
            this.SetValue(group + "_Group_NodeId_2", this.GetValue("NodeId_4"));
            this.SetValue(group + "_Group_NodeName_1", this.GetValue("NodeName_3"));
            this.SetValue(group + "_Group_NodeName_2", this.GetValue("NodeName_4"));

            // Homologation between 3 to 1
            await this.CreateHomologationForExcelAsync().ConfigureAwait(false);

            // Updating Node Tags
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_3") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_4") }).ConfigureAwait(false);

            string fileName = "TestData_" + (inventory is null ? throw new System.ArgumentNullException(nameof(inventory)) : inventory.Replace(" ", string.Empty)) + (movement is null ? throw new System.ArgumentNullException(nameof(movement)) : movement.Replace(" ", string.Empty));
            if (!fileName.EqualsIgnoreCase("TestData_WithoutInventoryNoMovements"))
            {
                ////this.When($"I update the excel with \"{fileName}\" data");
                this.WhenIUpdateTheExcelFileWithDaywiseData(fileName);
                ////this.When("I navigate to \"FileUpload\" page");
                this.UiNavigation("FileUpload");
                ////this.When("I click on \"FileUpload\" \"button\"");
                this.IClickOn("FileUpload", "button");
                ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
                this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
                ////this.When("I select \"Insert\" from FileUpload dropdown");
                this.ISelectFileFromFileUploadDropdown("Insert");
                ////this.When("I click on \"Browse\" to upload");
                this.IClickOnUploadButton("Browse");
                ////this.When($"I select \"{fileName}\" file from directory");
                await this.ISelectFileFromDirectoryAsync(fileName).ConfigureAwait(false);
                ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
                this.IClickOn("uploadFile\" \"Submit", "button");
                ////this.When("I wait till file upload to complete");
                await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
            }
        }

        [Given(@"I want create TestData for transfer point")]
        public async Task CreateTestDataFoTransferPointAsync()
        {
            this.ScenarioContext["Count"] = "0";
            this.Given("I am authenticated as \"admin\"");

            // Create Nodes
            await this.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await this.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await this.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await this.CreateHomologationForExcelAsync().ConfigureAwait(false);

            // Updating Node Tags
            var elementRow = await this.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = this.GetValue("CategorySegment") }).ConfigureAwait(false);
            var elementId = elementRow["ElementId"];
            var otherElementId = await this.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 0, nodeId = this.GetValue("NodeId_1") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_2") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_3") }).ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = this.GetValue("NodeId_4") }).ConfigureAwait(false);
        }

        [Given(@"I have transfer point movements without update or delete events and without having GlobalMovementId")]
        public async Task GivenIHaveTransferPointMovementsWithoutUpdateOrDeleteEventsAndWithoutHavingGlobalMovementIdAsync()
        {
            await this.CreateTestDataFoTransferPointAsync().ConfigureAwait(false);
            this.WhenIUpdateTheExcelFileWithDaywiseData("TestData_TransferPointMovements");
            this.UiNavigation("FileUpload");
            await this.UploadExcelFileAsync("Insert").ConfigureAwait(false);
        }

        [Given(@"I have transfer point movements with update events and without having GlobalMovementId")]
        public async Task GivenIHaveTransferPointMovementsWithUpdateEventsAndWithoutHavingGlobalMovementIdAsync()
        {
            await this.GivenIHaveTransferPointMovementsWithoutUpdateOrDeleteEventsAndWithoutHavingGlobalMovementIdAsync().ConfigureAwait(false);
            await this.UploadExcelFileAsync("Update").ConfigureAwait(false);
        }

        [Given(@"I have transfer point movements with delete events and without having GlobalMovementId")]
        public async Task GivenIHaveTransferPointMovementsWithDeleteEventsAndWithoutHavingGlobalMovementIdAsync()
        {
            await this.GivenIHaveTransferPointMovementsWithoutUpdateOrDeleteEventsAndWithoutHavingGlobalMovementIdAsync().ConfigureAwait(false);
            await this.UploadExcelFileAsync("Delete").ConfigureAwait(false);
        }

        [Given(@"I have transfer point movements with update events and insert event having GlobalMovementId")]
        public async Task GivenIHaveTransferPointMovementsWithUpdateEventsAndInsertEventHavingGlobalMovementIdAsync()
        {
            await this.GivenIHaveTransferPointMovementsWithoutUpdateOrDeleteEventsAndWithoutHavingGlobalMovementIdAsync().ConfigureAwait(false);
            for (int i = 1; i <= 2; i++)
            {
                await this.ReadSqlAsDictionaryAsync(UIContent.UpdateRow["UpdateGlobalMovementId"], args: new { movementId = this.ScenarioContext["Movement" + i].ToString() }).ConfigureAwait(false);
            }

            await this.UploadExcelFileAsync("Update").ConfigureAwait(false);
        }

        [Given(@"I have transfer point movements with update events and update event having GlobalMovementId")]
        public async Task GivenIHaveTransferPointMovementsWithUpdateEventsAndUpdateEventHavingGlobalMovementIdAsync()
        {
            await this.GivenIHaveTransferPointMovementsWithoutUpdateOrDeleteEventsAndWithoutHavingGlobalMovementIdAsync().ConfigureAwait(false);
            await this.UploadExcelFileAsync("Update").ConfigureAwait(false);
            for (int i = 1; i <= 2; i++)
            {
                await this.ReadSqlAsDictionaryAsync(UIContent.UpdateRow["UpdateGlobalMovementId"], args: new { movementId = this.ScenarioContext["Movement" + i].ToString() }).ConfigureAwait(false);
            }
        }

        [StepDefinition(@"I have transfer point movement with error reported by SAP PO")]
        public async Task GivenIHaveTransferPointMovementWithErrorReportedBySAPPOAsync()
        {
            var movementRow = await this.ReadSqlAsStringDictionaryAsync(input: UIContent.GetRow["GetMovementByMovementId"], args: new { movementId = this.ScenarioContext["Movement1"].ToString() }).ConfigureAwait(false);
            var movementTransId = movementRow["MovementTransactionId"];
            await this.ReadSqlAsDictionaryAsync(UIContent.UpdateRow["UpdateErrorInSapTracking"], args: new { movementTransactionId = movementTransId }).ConfigureAwait(false);
        }

        [Given(@"I have transfer point movements with multiple events and any event having GlobalMovementId")]
        public async Task GivenIHaveTransferPointMovementsWithMultipleEventsAndAnyEventHavingGlobalMovementIdAsync()
        {
            await this.GivenIHaveTransferPointMovementsWithoutUpdateOrDeleteEventsAndWithoutHavingGlobalMovementIdAsync().ConfigureAwait(false);
            await this.UploadExcelFileAsync("Update").ConfigureAwait(false);
            await this.ReadSqlAsDictionaryAsync(UIContent.UpdateRow["UpdateGlobalMovementId"], args: new { movementId = this.ScenarioContext["Movement2"].ToString() }).ConfigureAwait(false);
            await this.UploadExcelFileAsync("Update").ConfigureAwait(false);
            await this.UploadExcelFileAsync("Update").ConfigureAwait(false);
        }

        [When(@"I have official movements and inventories in the system")]
        public async Task WhenIHaveOfficialMovementsAndInventoriesInTheSystemAsync()
        {
            this.SetValue("OfficialMovementsInventories", "Yes");
            await this.GivenIHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);
        }

        public async Task UploadExcelFileAsync(string eventType)
        {
            this.IClickOn("FileUpload", "button");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            this.ISelectFileFromFileUploadDropdown(eventType);
            this.IClickOnUploadButton("Browse");
            await this.ISelectFileFromDirectoryAsync("TestData_TransferPointMovements").ConfigureAwait(false);
            this.IClickOn("uploadFile\" \"Submit", "button");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public void CompleteVerifyMessagingProcess()
        {
            while (!this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: "Sin registros"))
            {
                this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
                ////this.When("I select all pending records from grid");
                this.ISelectAllPendingRepositroriesFromGrid();
                ////this.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
                this.IClickOn("ErrorsGrid\" \"AddNote", "button");
                ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                ////this.When("I click on \"AddComment\" \"Submit\" \"button\"");
                this.IClickOn("AddComment\" \"Submit", "button");
            }

            ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("ErrorsGrid\" \"Submit", "button");
        }

        public void CompleteTransferPointsProcess()
        {
            while (!this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: "Sin registros"))
            {
                this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
                ////this.When("I select all pending records from grid");
                this.ISelectAllPendingRepositroriesFromGrid();
                ////this.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
                this.IClickOn("OfficialPointsGrid\" \"AddNote", "button");
                ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                ////this.When("I click on \"AddComment\" \"Submit\" \"button\"");
                this.IClickOn("AddComment\" \"Submit", "button");
            }

            ////this.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("officialPointsGrid\" \"Submit", "button");
            ////this.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
            this.IClickOn("officialPointsGrid\" \"Submit", "button");
        }

        public void CompleteCheckConsistencyProcess()
        {
            while (!this.Get<ElementPage>().CheckIfElementIsPresent(nameof(Resources.ElementByText), formatArgs: "Sin registros"))
            {
                this.ScenarioContext[ConstantValues.PendingTransactions] = 0;
                ////this.When("I select all unbalances in the grid");
                this.ISelectAllPendingRepositroriesFromGrid();
                ////this.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
                this.IClickOn("consistencyCheck\" \"AddNote", "button");
                ////this.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                this.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                ////this.When("I click on \"AddComment\" \"submit\" \"button\"");
                this.IClickOn("AddComment\" \"submit", "button");
            }

            ////this.Then("validate that \"ConsistencyCheck\" \"Next\" \"button\" as enabled");
            this.ValidateThatAsEnabled("unbalancesGrid\" \"submit", "button");
            ////this.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
            this.IClickOn("unbalancesGrid\" \"submit", "button");
        }

        [Given(@"I have valid official Movements and Inventories with same identifier in the system")]
        public async Task GivenIHaveValidOfficialMovementsAndInventoriesWithSameIdentifierInTheSystemAsync()
        {
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "Excel", this).ConfigureAwait(true);
            ////this.When("I update the \"TestData_Official\" excel");
            ////this.IUpdateTheExcelForOfficialMovementsAndInventories("TestData_OfficialMovements");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"TestData_Official\" file from directory");
            await this.ISelectFileFromDirectoryAsync("TestData_OfficialMovements").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I have logistic center with official movements information")]
        public async Task GivenIHaveLogisticCenterWithOfficialMovementsInformationAsync()
        {
            this.SetValue(ConstantValues.TestdataForOfficialBalanceFileWithoutCancellationMovements, "Yes");
            this.SetValue(ConstantValues.NoSONSegment, "Yes");

            //// Data setup for Official balance File Generation
            await this.TestDataSetupForOfficialBalanceFileGenerationAsync().ConfigureAwait(false);
        }

        [Given(@"I have logistic center with official movements information that have annulation")]
        public async Task GivenIHaveLogisticCenterWithOfficialMovementsInformationThatHaveAnnulationAsync()
        {
            this.SetValue(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions, "Yes");

            //// Data setup for Official balance File Generation
            await this.TestDataSetupForOfficialBalanceFileGenerationAsync().ConfigureAwait(false);
        }

        [Given(@"I have logistic center with official movements information that have annulation as per tautology")]
        public async Task GivenIHaveLogisticCenterWithOfficialMovementsInformationThatHaveAnnulationAsPerTautologyAsync()
        {
            this.SetValue(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements, "Yes");

            //// Data setup for Official balance File Generation
            await this.TestDataSetupForOfficialBalanceFileGenerationAsync().ConfigureAwait(false);
        }

        [Given(@"I have logistic center with official movements information that have annulation as per relationship table")]
        public async Task GivenIHaveLogisticCenterWithOfficialMovementsInformationThatHaveAnnulationAsPerRelationshipTableAsync()
        {
            this.SetValue(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable, "Yes");

            //// Data setup for Official balance File Generation
            await this.TestDataSetupForOfficialBalanceFileGenerationAsync().ConfigureAwait(false);
        }

        [Given(@"I have official movements information with same product, logistic centre and storage location without annulation")]
        public async Task GivenIHaveOfficialMovementsInformationWithSameProductLogisticCentreAndStorageLocationWithoutAnnulationAsync()
        {
            this.SetValue(ConstantValues.TestdataForInvalidScenarioWithoutAnnulation, "Yes");
            this.SetValue(ConstantValues.NoSONSegment, "Yes");

            //// Data setup for Official balance File Generation
            await this.TestDataSetupForOfficialBalanceFileGenerationAsync().ConfigureAwait(false);
        }

        [Given(@"I have official movements information with same product, logistic centre and storage location with annulation")]
        public async Task GivenIHaveOfficialMovementsInformationWithSameProductLogisticCentreAndStorageLocationWithAnnulationAsync()
        {
            this.SetValue(ConstantValues.TestdataForInvalidScenarioWithAnnulation, "Yes");

            //// Data setup for Official balance File Generation
            await this.TestDataSetupForOfficialBalanceFileGenerationAsync().ConfigureAwait(false);
        }

        public async Task TestDataSetupForOfficialBalanceFileGenerationAsync()
        {
            this.SetValue(ConstantValues.TestdataForLogisticOfficialBalance, "Yes");

            //// Test data setup for Excel upload
            await this.TestDataForOfficialBalanceLogisticsFileAsync().ConfigureAwait(false);

            // Homologation between 1 to 6
            await this.CreateHomologationForTrueToSivAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(this.GetValue(ConstantValues.NoSONSegment)))
            {
                //// CutOff and Ownership Ticket Generation
                await this.CutoffAndOwnershipTicketGenerationAsync().ConfigureAwait(false);

                //// Approving all nodes related segment
                await this.ReadAllSqlAsync(SqlQueries.UpdateOwnershipNodeStatusBasedOnSegmentName, args: new { ownershipStatusId = 9, segment = this.GetValue("SegmentName") }).ConfigureAwait(false);
            }

            //// Generation official delta Ticket
            await this.OfficialDeltaTicketGenerationAsync().ConfigureAwait(false);

            //// Approving all Official delta nodes
            await this.ReadAllSqlAsync(SqlQueries.UpdateOfficialNodeApprovalStatus, args: new { status = 9, officialDeltaTicket = this.GetValue("Official Delta TicketId") }).ConfigureAwait(false);
        }

        [Given(@"I have an official movements in the system with movements originated by manual inventory or an inventory delta")]
        [Given(@"I have an official movements in the system with movements originated by an inventory delta")]
        [Given(@"I have an official movements in the system with movements originated by manual inventory deltas")]
        public async Task GivenIHaveAnOfficialMovementsInTheSystemWithMovementsOriginatedByManualInventoryDeltasAsync()
        {
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "Excel", this).ConfigureAwait(true);
            ////this.WhenIUpdateTheExcelForOfficialMovementsAndInventories("TestData-OfficialManualInvOficial");
            this.IUpdateTheExcelForOfficialMovementsAndInventories("TestData-OfficialManualInvOficial");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"TestData_Official\" file from directory");
            await this.ISelectFileFromDirectoryAsync("TestData-OfficialManualInvOficial").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I have an official movements in the system with movements originated by manual delta movements or movement deltas")]
        public async Task GivenIHaveAnOfficialMovementsInTheSystemWithMovementsOriginatedByManualDeltaMovementsOrMovementDeltasAsync()
        {
            await this.IHaveHomologationDataInTheSystemAsync(systemType: "Excel", this).ConfigureAwait(true);
            ////this.WhenIUpdateTheExcelForOfficialMovementsAndInventories("TestData-OfficialManualInvOficial");
            this.IUpdateTheExcelForOfficialMovementsAndInventories("TestData-OfficialManualMovOficial");
            ////this.When("I navigate to \"FileUpload\" page");
            this.UiNavigation("FileUpload");
            ////this.When("I click on \"FileUpload\" \"button\"");
            this.IClickOn("FileUpload", "button");
            ////this.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            this.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////this.When("I select \"Insert\" from FileUpload dropdown");
            this.ISelectFileFromFileUploadDropdown("Insert");
            ////this.When("I click on \"Browse\" to upload");
            this.IClickOnUploadButton("Browse");
            ////this.When("I select \"TestData_Official\" file from directory");
            await this.ISelectFileFromDirectoryAsync("TestData-OfficialManualMovOficial").ConfigureAwait(false);
            ////this.When("I click on \"FileUpload\" \"Save\" \"button\"");
            this.IClickOn("uploadFile\" \"Submit", "button");
            ////this.When("I wait till file upload to complete");
            await this.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        [Given(@"I have category element in the system")]
        public async Task GivenIHaveElementInTheSystemAsync()
        {
            await this.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            var createContent = ApiContent.Creates["Transport Category"];
            createContent = createContent.JsonChangeValue();
            this.ScenarioContext["CategoryName"] = createContent.JsonGetValue("name");
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes["Transport Category"]), JObject.Parse(createContent)).ConfigureAwait(false)).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated["Transport Category"]).ConfigureAwait(false);
            this.ScenarioContext[ApiContent.Ids["Transport Category"].ToPascalCase()] = lastCreatedRow[ApiContent.Ids["Transport Category"].ToPascalCase()];
            createContent = ApiContent.Creates["Category Element"];
            createContent = createContent.JsonChangeValue();
            this.ScenarioContext["CategoryElementName"] = createContent.JsonGetValue("name");
            this.SetValue(Entities.Keys.RandomFieldValue, createContent.JsonGetValue("name"));
            createContent = createContent.JsonChangePropertyValue("categoryId", this.ScenarioContext[ApiContent.Ids["Transport Category"].ToPascalCase()].ToString());
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes["Category Element"]), JObject.Parse(createContent)).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}