// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologatedDataInventorySteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;
    using Ecp.True.Bdd.Tests.Utils;

    using Flurl;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    [Binding]
    public class HomologatedDataInventorySteps : EcpApiStepDefinitionBase
    {
        private const int MillisecondsDelayForRequestProcessing = 60000;

        public HomologatedDataInventorySteps(FeatureContext featureContext)
          : base(featureContext)
        {
        }

        [Given(@"I want to register an ""(.*)"" in the system")]
        public async Task GivenIWantToRegisterAnInTheSystemAsync(string entity)
        {
            await this.IWantToRegisterAnInTheSystemAsync(entity).ConfigureAwait(false);
        }

        [Given(@"I want to cancel an ""(.*)""")]
        [Given(@"I want to adjust an ""(.*)""")]
        public async Task GivenIWantToCancelAnAsync(string entity)
        {
            await this.IWantToCancelOrAdjustAnAsync(entity).ConfigureAwait(false);
        }

        [When(@"I don't receive ""(.*)"" in XML")]
        public async Task WhenIDonTReceiveInXMLAsync(string field)
        {
            await this.IDonTReceiveInXMLAsync(field).ConfigureAwait(false);
        }

        [When(@"I receive invalid ""(.*)"" in ""(.*)"" XML")]
        public async Task InvalidStartEndDateAsync(string field, string entity)
        {
            await this.WhenIDonTReceiveInXMLAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
            var xmlFieldValue = DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + entity, field, xmlFieldValue);
        }

        [When(@"it meets ""(.*)"" input validations")]
        public async Task WhenItMeetsAllInputValidationsAsync(string value)
        {
            await this.ItMeetsAllInputValidationsAsync(value).ConfigureAwait(false);
        }

        [When(@"the ""(.*)"" field is equal to ""(.*)""")]
        [When(@"the ""(.*)"" field is equal to ""(.*)"" with unique batch identifier")]
        public void WhenTheFieldIsEqualTo(string field1, string fieldValue)
        {
            Assert.IsNotNull(field1);
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType[fieldValue];
        }

        [StepDefinition(@"it should be registered")]
        [Then(@"it should record the operations in the system")]
        [Then(@"record a new Inventory with positive product volume")]
        [Then(@"updated should be successful for second product")]
        public async Task ThenItShouldBeRegisteredAsync()
        {
            await this.ShouldBeRegisteredAsync().ConfigureAwait(false);
        }

        [StepDefinition(@"it must be stored in a Pendingtransactions repository with validation ""(.*)""")]
        [Then(@"second inventory product must be stored in a Pendingtransactions repository with validation ""(.*)""")]
        public async Task ThenItMustBeStoredInAPendingTransactionsRepositoryWithValidationAsync(string field)
        {
            await Task.Delay(MillisecondsDelayForRequestProcessing).ConfigureAwait(true);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.PendingTransactions], args: new { messageCode = this.ScenarioContext[ConstantValues.MessageId].ToString() }).ConfigureAwait(false);
            this.SetValue(Keys.Error, lastCreatedRow[ConstantValues.ErrorJson]);
            Assert.IsTrue(this.GetValue(Keys.Error).ContainsIgnoreCase(field));
        }

        [Then(@"it must be stored in a FileRegistrationError repository with validation ""(.*)""")]
        public async Task ThenItMustBeStoredInAFileRegistrationErrorRepositoryWithValidationAsync(string expectedMessage)
        {
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[ConstantValues.FileRegistrationError]).ConfigureAwait(false);
            var actualErrorMessage = lastCreatedRow["ErrorMessage"];
            this.VerifyThat(() => Assert.AreEqual(expectedMessage, actualErrorMessage));
        }

        [When(@"I receive the data​ with ""(.*)"" that exceeds (.*) characters")]
        public async Task WhenIReceiveTheDataWithThatExceedsCharactersAsync(string field1, int field2)
        {
            await this.WhenIDonTReceiveInXMLAsync(field1 + "_Characters").ConfigureAwait(false);
            Assert.IsNotNull(field2);
        }

        [When(@"I receive the data​ with ""(.*)"" containing spaces")]
        public async Task WhenIReceiveTheDataWithContainingSpacesAsync(string field)
        {
            await this.WhenIDonTReceiveInXMLAsync(field + "_Spaces").ConfigureAwait(false);
        }

        [When(@"I receive the data​ with ""(.*)"" containing other than letters")]
        public async Task WhenIReceiveTheDataWithContainingOtherThanLettersAsync(string field)
        {
            await this.WhenIDonTReceiveInXMLAsync(field + "_Numbers").ConfigureAwait(false);
        }

        [When(@"I receive the data​ with ""(.*)"" greater than or equal to current date")]
        public async Task WhenIReceiveTheDataWithGreaterThanOrEqualToCurrentDateAsync(string field)
        {
            var xmlFieldValue = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var xmlField = "DATE";
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue);
            await this.WhenIDonTReceiveInXMLAsync(field + "_DateValidation").ConfigureAwait(false);
        }

        [When(@"I receive the data​ with ""(.*)"" different from the current date month")]
        public async Task WhenIReceiveTheDataWithDifferentFromTheCurrentDateMonthAsync(string field)
        {
            await this.WhenIDonTReceiveInXMLAsync(field + "_MonthValidation").ConfigureAwait(false);
        }

        [When(@"the ""(.*)"" does not exist")]
        public async Task WhenTheDoesNotExistAsync(string field)
        {
            await this.WhenIDonTReceiveInXMLAsync(field + "_NodeValidation").ConfigureAwait(false);
        }

        [When(@"""(.*)"" does not belongs to one of the ""(.*)"" of the ""(.*)""")]
        public async Task WhenDoesNotBelongsToOneOfTheOfTheAsync(string field1, string field2, string field3)
        {
            await this.WhenIDonTReceiveInXMLAsync(field1 + "_Validation").ConfigureAwait(false);
            Assert.IsNotNull(field2);
            Assert.IsNotNull(field3);
        }

        [When(@"there are owner records with different ""(.*)""")]
        public async Task WhenThereAreOwnerRecordsWithDifferentAsync(string field)
        {
            await this.WhenIDonTReceiveInXMLAsync(field + "_Validation").ConfigureAwait(false);
        }

        [Then(@"record a new ""(.*)"" with negative values for the ""(.*)""")]
        public async Task ThenRecordANewWithNegativeValuesForTheAsync(string entity, string field)
        {
            if (entity.EqualsIgnoreCase("Inventory"))
            {
                var lastCreatedInventoryRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[this.GetValue(Keys.EntityType)]).ConfigureAwait(false);
                var fileRegistrationTransactionId = lastCreatedInventoryRow[ApiContent.Ids[this.GetValue(Keys.EntityType)]];
                var inventoryProducts = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["InventoryProduct"], args: new { fileRegistrationTransactionId }).ConfigureAwait(false);
                Assert.IsTrue(inventoryProducts.ToDictionaryList().Count.Equals(this.ScenarioContext[ConstantValues.EventType].ToString().EqualsIgnoreCase("Delete") ? 1 : 2));
                foreach (var inventoryProductsRow in inventoryProducts.ToDictionaryList())
                {
                    if (inventoryProductsRow[field].ToString().Contains("-"))
                    {
                        this.VerifyThat(() => Assert.IsTrue(inventoryProductsRow[field].ToString().Contains("-")));
                        break;
                    }
                }
            }
            else
            {
                var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[entity + "_ProductVolume"]).ConfigureAwait(false);
                this.VerifyThat(() => Assert.IsTrue(lastCreatedRow[field].Contains("-")));
            }
        }

        [When(@"there are owner records with ""(.*)"" for all Owners as ""(.*)"" and sum of ""(.*)"" is not equal to ""(.*)""")]
        public async Task WhenThereAreOwnerRecordsWithForAllOwnersAsAndSumOfIsNotAsync(string field1, string field2, string field3, string field4)
        {
            await this.WhenIDonTReceiveInXMLAsync(field2 + "_" + field1 + "_Validation").ConfigureAwait(false);
            Assert.IsNotNull(field3);
            Assert.IsNotNull(field4);
        }

        [When(@"the ""(.*)"" already exist")]
        public async Task WhenTheAlreadyExistAsync(string field)
        {
            await this.WhenIDonTReceiveInXMLAsync(field + "_AlreadyExist").ConfigureAwait(false);
            var xmlFieldValue = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? this.ScenarioContext["MOVEMENTID"] : this.ScenarioContext["DATE"];
            var xmlField = this.GetValue(Keys.EntityType).EqualsIgnoreCase("Movements") ? ConstantValues.MovementId : "DATE";
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + field, xmlField, xmlFieldValue.ToString());
            await this.PerformHomologationAsync(field + "_AlreadyExist").ConfigureAwait(false);
        }

        [When(@"the '(.*)' does not exist and ""(.*)"" field is equal to ""(.*)""")]
        public async Task WhenTheDoesNotExistAndFieldIsEqualToAsync(string field1, string field2, string field2Value)
        {
            Assert.IsNotNull(field1);
            Assert.IsNotNull(field2);
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType[field2Value];
            this.UpdateXmlDefaultValue(this.GetValue(Keys.EntityType));
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), this.GetValue(Keys.EntityType), field2Value);
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }

        [When(@"I dont receive ""(.*)"" and '(.*)' is equal to '(.*)' or '(.*)'")]
        public async Task WhenIDontReceiveAndIsEqualToOrAsync(string field1, string field2, string field2Value1, string field2Value2 = null)
        {
            await this.WhenIDonTReceiveInXMLAsync(field1 + "_" + field2 + "_" + field2Value1 + "_" + field2Value2 + "_Validation").ConfigureAwait(false);
        }

        [When(@"I dont receive '(.*)' and '(.*)' is equal to '(.*)'")]
        public async Task WhenIDontReceiveAndIsEqualToAsync(string field1, string field2, string field2Value)
        {
            await this.WhenIDonTReceiveInXMLAsync(field1 + "_" + field2 + "_" + field2Value + "_Validation").ConfigureAwait(false);
        }

        [Then(@"record a new ""(.*)"" with negative values for the '(.*)' and '(.*)'")]
        public async Task ThenRecordANewWithNegativeValuesForTheAndAsync(string entity, string field1, string field2)
        {
            BlobExtensions.UpdateXmlData(this.ScenarioContext[ConstantValues.EventType] + "_" + entity + "_ProductVolume", ConstantValues.MovementId, this.ScenarioContext["MOVEMENTID"].ToString());
            this.UpdateXmlDefaultValue(this.ScenarioContext[ConstantValues.EventType] + "_" + entity + "_ProductVolume");
            await this.PerformHomologationAsync(this.ScenarioContext[ConstantValues.EventType] + "_" + entity + "_ProductVolume").ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[entity + "_ProductVolume"]).ConfigureAwait(false);
            this.VerifyThat(() => Assert.IsTrue(lastCreatedRow[field1].Contains("-")));
            this.VerifyThat(() => Assert.IsTrue(lastCreatedRow[field2].Contains("-")));
        }

        [Given(@"connection between ""(.*)"" and ""(.*)"" does not exist")]
        public async Task GivenConnectionBetweenAndDoesNotExistAsync(string field1, string field2)
        {
            await this.CreateHomologationAsync("No").ConfigureAwait(false);
            await this.WhenIDonTReceiveInXMLAsync(field1 + "_" + field2 + "_NotExist").ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" as ""(.*)""")]
        public async Task WhenIProvideAsAsync(string field, string fieldValue)
        {
            Assert.IsNotNull(fieldValue);
            this.UpdateXmlDefaultValue(this.GetValue(Keys.EntityType));
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), field, fieldValue);
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), "LocationId", "AYACUCHO:1500");
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }

        [When(@"I provide different ""(.*)"" with same product")]
        public async Task WhenIProvideDifferentWithSameProductAsync(string field)
        {
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), ApiContent.Conversion[field], "AYACUCHO:2000");
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
            var lastCreatedRow = await this.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryByTankName, args: new { tankName = this.ScenarioContext["TankName2"] }).ConfigureAwait(false);
            Assert.AreEqual(this.ScenarioContext["TankName2"], lastCreatedRow["TankName"]);
        }

        [When(@"I provide different ""(.*)"" with same ""(.*)""")]
        public async Task WhenIProvideDifferentWithSameAsync(string field1, string field2)
        {
            Assert.IsNotNull(field2);
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), ApiContent.Conversion[field1], "CAST");
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }

        [When(@"I register with same data")]
        public async Task WhenIRegisterWithSameDataAsync()
        {
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }

        [Given(@"I provide ""(.*)"" field is equal to ""(.*)""")]
        [Given(@"I provide ""(.*)"" field is equal to ""(.*)"" with unique batch identifier")]
        public async Task WhenIProvideFieldIsEqualToAsync(string field, string fieldValue)
        {
            Assert.IsNotNull(field);
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType[fieldValue];
            if (this.GetValue(ConstantValues.InventoryWithMultipleProducts) == "Yes")
            {
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + ConstantValues.InventoryWithMultipleProducts, this.GetValue(Keys.EntityType), fieldValue);
            }
            else
            {
                BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), this.GetValue(Keys.EntityType), fieldValue);
            }

            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }

        [Given(@"I want to adjust an '(.*)' with ""(.*)"" as ""(.*)""")]
        [Given(@"I want to cancel an '(.*)' with ""(.*)"" as ""(.*)""")]
        public async Task GivenIWantToAdjustAnWithAsAsync(string entity, string field, string fieldValue)
        {
            await this.GivenIWantToRegisterAnInTheSystemAsync(entity).ConfigureAwait(false);
            this.UpdateXmlDefaultValue(entity);
            this.SetValue(ConstantValues.TankName, "1500");
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), "LocationId", "AYACUCHO:1500");
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), field, fieldValue);
            await this.PerformHomologationAsync(entity).ConfigureAwait(false);
        }

        [Given(@"I provide ""(.*)"" field is equal to '(.*)' and add new ""(.*)""")]
        public async Task GivenIProvideFieldIsEqualToAndAddNewAsync(string field, string fieldValue, string field1)
        {
            Assert.IsNotNull(field);
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType[fieldValue];
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), this.GetValue(Keys.EntityType), fieldValue);
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), ApiContent.Conversion[field1], "CAST");
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }

        [When(@"I provide ""(.*)"" that exceeds (.*) characters for inventory")]
        public async Task WhenIProvideThatExceedsCharactersForInventoryAsync(string field, int limit)
        {
            Assert.IsNotNull(field);
            await this.CreateHomologationAsync("Yes", "No").ConfigureAwait(false);
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), "LocationType", "Tanque");
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), "LocationId", "AYACUCHO:" + new Faker().Random.AlphaNumeric(limit + 1));
            await this.WhenItMeetsAllInputValidationsAsync("all").ConfigureAwait(false);
        }

        [Given(@"I have ""(.*)"" in the system through SINOPER")]
        public async Task GivenIHaveInTheSystemThroughSINOPERAsync(string entity)
        {
            ////this.Given($"I want to register an \"{entity}\" in the system");
            await this.IWantToRegisterAnInTheSystemAsync(entity).ConfigureAwait(false);
            ////this.When("it meets \"all\" input validations");
            await this.ItMeetsAllInputValidationsAsync("all").ConfigureAwait(false);
            ////this.When("the \"EventType\" field is equal to \"Insert\"");
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType["Insert"];
            ////this.When("it should be registered");
            await this.ShouldBeRegisteredAsync().ConfigureAwait(false);
        }

        [When(@"it meets ""(.*)"" input validations with multiple products")]
        public async Task WhenItMeetsInputValidationsWithMultipleProductsAsync(string value)
        {
            Assert.IsNotNull(value);
            await this.WhenIDonTReceiveInXMLAsync("Inventory_MultipleProduct").ConfigureAwait(false);
        }

        public async Task CreateHomologationAsync(string nodeConnection, string tankHomologation = "Yes")
        {
            try
            {
                await this.ReadAllSqlAsync(SqlQueries.DeleteDataInHomologationObjectAndDataMapping, args: new { sourceSytem=2, destinationSystem=1 }).ConfigureAwait(false);
                var homologationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationId]).ConfigureAwait(false);
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

            var nodeConnectionRequest = ApiContent.Creates[ConstantValues.ManageConnection];
            nodeConnectionRequest = await this.CreateSourceAndDestinationNodesAsync(nodeConnectionRequest).ConfigureAwait(false);
            nodeConnectionRequest = await this.CreateOwnersAsync(nodeConnectionRequest).ConfigureAwait(false);
            await this.SetResultAsync(() => nodeConnection.EqualsIgnoreCase("Yes") ? this.PostAsync<dynamic>(this.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(nodeConnectionRequest)) : null).ConfigureAwait(false);
            this.ScenarioContext[ConstantValues.SourceNode] = nodeConnectionRequest.JsonGetValue(ConstantValues.SourceNode);
            this.ScenarioContext[ConstantValues.DestinationNode] = nodeConnectionRequest.JsonGetValue(ConstantValues.DestinationNode);
            this.ScenarioContext["Count"] = 0;
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_3", this.GetValue("NodeId"));
            await this.CreateNodesStepsAsync().ConfigureAwait(false);
            this.SetValue("NodeId_4", this.GetValue("NodeId"));
            await this.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            this.SetValue("MovementTypeId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            this.SetValue("ProductTypeId", this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            this.SetValue(ConstantValues.NameOfSourceProductTypeId, this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            this.SetValue(ConstantValues.NameOfDestinationProductTypeId, this.ScenarioContext["CategoryElement"]);
            await this.CreateCatergoryElementAsync("7").ConfigureAwait(false);
            this.SetValue("Owner", this.ScenarioContext["CategoryElement"]);
            ////var nodeStorageLocationRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.NodeStorageLocationByNodeId], args: new { nodeId = this.ScenarioContext[ConstantValues.SourceNode] }).ConfigureAwait(false);
            ////this.ScenarioContext["NodeStorageLocationId"] = nodeStorageLocationRow["NodeStorageLocationId"];
            ////var storageLocationProductRow = await this.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["StorageLocationProductByNodeStorageLocationId"], args: new { nodeStorageLocationId = this.ScenarioContext["NodeStorageLocationId"] }).ConfigureAwait(false);
            ////this.ScenarioContext[ConstantValues.StorageLocationProductId] = storageLocationProductRow[ConstantValues.StorageLocationProductId];
            ////Homologation between 2 and 1
            var setupHomologationRequestForNodes = tankHomologation.EqualsIgnoreCase("Yes") ? ApiContent.Creates[ConstantValues.HomologationForNodesWithTank] : ApiContent.Creates[ConstantValues.HomologationForNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(this.ScenarioContext[ConstantValues.SourceNode].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(this.ScenarioContext[ConstantValues.DestinationNode].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(this.GetValue("NodeId_3"), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(this.GetValue("NodeId_4"), CultureInfo.InvariantCulture));
            await this.CreateNodeAsync().ConfigureAwait(false);
            this.ScenarioContext["TankName1"] = await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false);
            setupHomologationRequestForNodes = tankHomologation.EqualsIgnoreCase("Yes") ? setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node5 destinationValue", this.ScenarioContext["TankName1"].ToString()) : setupHomologationRequestForNodes;
            await this.CreateNodeAsync().ConfigureAwait(false);
            this.ScenarioContext["TankName2"] = await this.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false);
            setupHomologationRequestForNodes = tankHomologation.EqualsIgnoreCase("Yes") ? setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node6 destinationValue", this.ScenarioContext["TankName2"].ToString()) : setupHomologationRequestForNodes;
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for Products
            var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
            setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 2);
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 2);
            if (this.GetValue(ConstantValues.MeasurementUnit) == ConstantValues.Empty)
            {
                setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("HomologationDataMapping destinationValue", 0);
            }

            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for MovementTypeId
            var setupHomologationRequestForMovementType = ApiContent.Creates[ConstantValues.HomologationForMovementTypeId];
            setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("HomologationDataMapping_MovementTypeId destinationValue", int.Parse(this.GetValue("MovementTypeId"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForMovementType)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for ProductTypeId
            var setupHomologationRequestForProductType = ApiContent.Creates[ConstantValues.HomologationForProductTypeId];
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_ProductTypeId destinationValue", int.Parse(this.GetValue("ProductTypeId"), CultureInfo.InvariantCulture));
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_SourceProductTypeId destinationValue", int.Parse(this.GetValue("SourceProductTypeId"), CultureInfo.InvariantCulture));
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_DestinationProductTypeId destinationValue", int.Parse(this.GetValue("DestinationProductTypeId"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProductType)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for OwnerId
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", int.Parse(this.GetValue("Owner"), CultureInfo.InvariantCulture));
            await this.SetResultAsync(async () => await this.PostAsync<dynamic>(this.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Given(@"I have an ""(.*)"" and ""(.*)"" in the system")]
        public async Task GivenIHaveAnAndInTheSystemAsync(string entity1, string entity2)
        {
            this.SetValue(ConstantValues.CreationOfBothMovementAndInventory, "Yes");
            await this.GivenIWantToRegisterAnInTheSystemAsync(entity1).ConfigureAwait(false);
            this.UpdateXmlDefaultValue(entity1);
            await this.PerformHomologationAsync(entity1).ConfigureAwait(false);
            this.SetValue(Keys.EntityType, entity2);
            this.UpdateXmlDefaultValue(entity2);
            await this.PerformHomologationAsync(entity2).ConfigureAwait(false);
            await this.ThenItShouldBeRegisteredAsync().ConfigureAwait(false);
        }

        [Given(@"I provide ""(.*)"" field is equal to '(.*)' and add new batch identifier")]
        public async Task GivenIProvideFieldIsEqualToAndAddNewBatchIdentifierAsync(string field, string fieldValue)
        {
            Assert.IsNotNull(field);
            this.ScenarioContext[ConstantValues.EventType] = ApiContent.HomologateLogType[fieldValue];
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), this.GetValue(Keys.EntityType), fieldValue);
            var xmlField = ConstantValues.BATCHID + "_1";
            var xmlFieldValue = new Faker().Random.AlphaNumeric(25).ToString(CultureInfo.InvariantCulture);
            this.SetValue(ConstantValues.BATCHID + "_1", xmlFieldValue);
            BlobExtensions.UpdateXmlData(this.GetValue(Keys.EntityType) + "\\" + this.GetValue(Keys.EntityType), xmlField, xmlFieldValue);
            await this.PerformHomologationAsync(this.GetValue(Keys.EntityType)).ConfigureAwait(false);
        }
    }
}