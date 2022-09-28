// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecFlowEx.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Properties;
    using Ecp.True.Bdd.Tests.StepDefinitions.Api;

    using Flurl;

    using global::Bdd.Core.Api.Executors;
    using global::Bdd.Core.Entities;
    using global::Bdd.Core.StepDefinitions;
    using global::Bdd.Core.Utils;
    using global::Bdd.Core.Web.Executors;
    using global::Bdd.Core.Web.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NLog;

    using NUnit.Framework;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public static class SpecFlowEx
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const int MillisecondsDelay = 30000;

        private static readonly NameValueCollection Settings = ConfigurationManager.GetSection(ConstantValues.FicoConnect) as NameValueCollection;

        private static IWebElement input;

        public static async Task TokenForUserAsync(this StepDefinitionBase step, ApiExecutor apiExecutor, Credentials userCredentials)
        {
            step.ThrowIfNull(nameof(step));
            apiExecutor.ThrowIfNull(nameof(apiExecutor));
            var authInfo = await apiExecutor.AddAuthInfo(userCredentials).ConfigureAwait(false);
            step.ScenarioContext["UserToken"] = authInfo;
        }

        public static async Task TokenForServiceAsync(this StepDefinitionBase step, ApiExecutor apiExecutor, string service)
        {
            step.ThrowIfNull(nameof(step));
            apiExecutor.ThrowIfNull(nameof(apiExecutor));
            var authInfo = await apiExecutor.AddAuthInfo(service).ConfigureAwait(false);
            step.ScenarioContext[service] = authInfo;
        }

        public static async Task UIAuthenticationForUserAsync(this StepDefinitionBase step, string role, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await ecpApiStep.GivenIAmAuthenticatedForUserAsync(role).ConfigureAwait(false);
        }

        public static void UiNavigation(this StepDefinitionBase step, string link)
        {
            step.ThrowIfNull(nameof(step));
            step.ScenarioContext[Entities.Keys.EntityType] = link;
            var page = step.Get<ElementPage>();
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.WaitUntilElementToBeClickable(nameof(Resources.NavigationBar));
            page.Click(nameof(Resources.NavigationBar));

            try
            {
                page.WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[link]);
                page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[link]);
                page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[link]);
                page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            }
            catch (WebDriverTimeoutException)
            {
                page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[link]);
                page.WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Navigation[link]);
                page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Navigation[link]);
                page.ScrollIntoView(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[link]);
                page.WaitUntilElementToBeClickable(nameof(Resources.PageIdentifier), 5, formatArgs: UIContent.Conversion[link]);
                page.Click(nameof(Resources.PageIdentifier), formatArgs: UIContent.Conversion[link]);
                page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 60);
            }

            page.Click(nameof(Resources.NavigationBar));
        }

        public static async Task IHaveHomologationDataInTheSystemAsync(this StepDefinitionBase step, string systemType, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            systemType.ThrowIfNull(nameof(systemType));
            step.ThrowIfNull(nameof(step));
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            if (systemType.EqualsIgnoreCase("Event"))
            {
                step.ScenarioContext["Count"] = "0";
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["NodeId_1"] = step.ScenarioContext["NodeId"].ToString();
                var nodeDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                step.ScenarioContext["NodeName_1"] = nodeDetails[ConstantValues.Name];
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["NodeId_2"] = step.ScenarioContext["NodeId"].ToString();
                nodeDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_2"].ToString() }).ConfigureAwait(false);
                step.ScenarioContext["NodeName_2"] = nodeDetails[ConstantValues.Name];
                await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_1"].ToString(), step.ScenarioContext["NodeId_2"].ToString(), 2, "0.07", "0.06").ConfigureAwait(false);
                await ecpApiStep.CreateOwnershipRulesForNodeConnectionsAsync(step.ScenarioContext["NodeConnectionId"].ToString()).ConfigureAwait(false);

                // Delete Homologation between 5 to 1
                try
                {
                    var homologationRow = await step.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForEvent]).ConfigureAwait(false);
                    step.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                    var homologationGroup = await step.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                    foreach (var homologationGroupRow in homologationGroup)
                    {
                        var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                        step.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                        await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = step.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                    }

                    await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                }
                catch (NullReferenceException ex)
                {
                    Logger.Info("Homologation for Event does not exists");
                    Assert.IsNotNull(ex);
                }
                catch (ArgumentNullException ex)
                {
                    Logger.Info("Homologation for Event does not exists");
                    Assert.IsNotNull(ex);
                }

                // Create Homologation between 5 to 1 For Nodes
                var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForEventNodes];
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(step.ScenarioContext["NodeId_1"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(step.ScenarioContext["NodeId_2"].ToString(), CultureInfo.InvariantCulture));
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 5 to 1 For Product
                var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
                setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 5);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 5 to 1 For Owner
                var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
                setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 5);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 5 to 1 For Unit
                var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
                setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 5);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 5 to 1 For EventType
                var setupHomologationRequestForEventType = ApiContent.Creates[ConstantValues.HomologationForEventType];
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForEventType)).ConfigureAwait(false)).ConfigureAwait(false);

                // Updating Ownership calculated Segment with Event Nodes
                if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.OwnershipCalculatedForSegemnt)))
                {
                    var nodeId = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetAllNodeTagsForGivenNodeId, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                    var elementIdForEvent = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = step.ScenarioContext["Segment"].ToString() }).ConfigureAwait(false);
                    await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementIdInNodeTag, args: new { elementId = elementIdForEvent["ElementId"], startDate = DateTime.UtcNow.AddDays(-5).ToShortDateString(), endDate = DateTime.UtcNow.ToShortDateString(), nodeTagId = nodeId["NodeTagId"] }).ConfigureAwait(false);
                    await ecpApiStep.CreateOwnershipRulesAsync(step.ScenarioContext["NodeId_1"].ToString()).ConfigureAwait(false);
                }
            }
            else if (systemType.EqualsIgnoreCase("SalesAndPurchase"))
            {
                Assert.IsNotNull(systemType);
                step.ScenarioContext["Count"] = "0";
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["NodeId_1"] = step.ScenarioContext["NodeId"].ToString();
                step.ScenarioContext["CategorySegment"] = step.ScenarioContext["SegmentName"].ToString();
                step.ScenarioContext[ConstantValues.Segment] = step.ScenarioContext["CategorySegment"].ToString();
                await ecpApiStep.CreateOwnershipRulesAsync(step.ScenarioContext["NodeId"].ToString()).ConfigureAwait(false);
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["NodeId_2"] = step.ScenarioContext["NodeId"].ToString();
                await ecpApiStep.CreateOwnershipRulesAsync(step.ScenarioContext["NodeId"].ToString()).ConfigureAwait(false);
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["NodeId_3"] = step.ScenarioContext["NodeId"].ToString();
                await ecpApiStep.CreateOwnershipRulesAsync(step.ScenarioContext["NodeId"].ToString()).ConfigureAwait(false);
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["NodeId_4"] = step.ScenarioContext["NodeId"].ToString();
                await ecpApiStep.CreateOwnershipRulesAsync(step.ScenarioContext["NodeId"].ToString()).ConfigureAwait(false);

                // nodeconnection
                await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_2"].ToString(), step.ScenarioContext["NodeId_1"].ToString(), 2, "0.07", "0.06").ConfigureAwait(false);
                await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_1"].ToString(), step.ScenarioContext["NodeId_4"].ToString(), 2, "0.05", "0.04").ConfigureAwait(false);
                await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_3"].ToString(), step.ScenarioContext["NodeId_1"].ToString(), 1, "0.02", "0.06", "10000002372").ConfigureAwait(false);

                // Delete Homologation between 4 to 1
                try
                {
                    var homologationRow = await step.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForContracts]).ConfigureAwait(false);
                    step.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                    var homologationGroup = await step.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                    foreach (var homologationGroupRow in homologationGroup)
                    {
                        var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                        step.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                        await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = step.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                    }

                    await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                }
                catch (NullReferenceException ex)
                {
                    Logger.Info("Homologation for SalesAndPurchase does not exists");
                    Assert.IsNotNull(ex);
                }
                catch (ArgumentNullException ex)
                {
                    Logger.Info("Homologation for SalesAndPurchase does not exists");
                    Assert.IsNotNull(ex);
                }

                // Create Homologation between 4 to 1 For Nodes
                var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForContractNodes];
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(step.ScenarioContext["NodeId_1"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(step.ScenarioContext["NodeId_2"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(step.ScenarioContext["NodeId_3"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(step.ScenarioContext["NodeId_4"].ToString(), CultureInfo.InvariantCulture));
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 4 to 1 For Product
                var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForContractProduct];
                setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 4);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 4 to 1 For Owners
                var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
                setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 4);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 4 to 1 For Unit
                var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForContractUnit];
                setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 4);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 4 to 1 For MovementTypeId
                var setupHomologationRequestForEventType = ApiContent.Creates[ConstantValues.HomologationForContractMovementTypeId];
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForEventType)).ConfigureAwait(false)).ConfigureAwait(false);
            }
            else if (systemType.EqualsIgnoreCase("SAPPO"))
            {
                step.ScenarioContext["Count"] = "0";

                // Create Nodes
                if (step.GetValueInternal("TransferPointNodes").EqualsIgnoreCase("True"))
                {
                    // Create Transfer Point Nodes
                    await ecpApiStep.CreateNodesForTransferPointTestDataAsync().ConfigureAwait(false);
                }
                else
                {
                    // Create Nodes
                    await ecpApiStep.CreateNodesForTestDataAsync().ConfigureAwait(false);
                }

                // Create system and tag with nodes
                ////await ecpApiStep.CreateCatergoryElementAsync("8", step.ScenarioContext["SegmentName"].ToString() + "_Sistema").ConfigureAwait(false);
                ////step.ScenarioContext["SystemElementId"] = step.ScenarioContext["CategoryElement"].ToString();
                ////await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString(), elementId = step.ScenarioContext["SystemElementId"), date = 4 }).ConfigureAwait(false);
                ////await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_2"].ToString(), elementId = step.ScenarioContext["SystemElementId"), date = 4 }).ConfigureAwait(false);
                ////await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_3"].ToString(), elementId = step.ScenarioContext["SystemElementId"), date = 4 }).ConfigureAwait(false);
                ////await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_4"].ToString(), elementId = step.ScenarioContext["SystemElementId"), date = 4 }).ConfigureAwait(false);

                // create elements
                await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

                // Create Connections between created nodes
                await ecpApiStep.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

                // Delete Homologation between 7 to 1
                try
                {
                    var numberOfHomologations = await step.ReadAllSqlAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForSAP]).ConfigureAwait(false);
                    var homologationRow = numberOfHomologations.ToDictionaryList();
                    for (int i = 0; i < homologationRow.Count; i++)
                    {
                        step.ScenarioContext[ConstantValues.HomologationId] = homologationRow[i][ConstantValues.HomologationId];
                        var homologationGroup = await step.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                        foreach (var homologationGroupRow in homologationGroup)
                        {
                            var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                            step.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                            await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = step.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                        }

                        await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                    }
                }
                catch (NullReferenceException ex)
                {
                    Logger.Info("Homologation for SAP does not exists");
                    Assert.IsNotNull(ex);
                }
                catch (ArgumentNullException ex)
                {
                    Logger.Info("Homologation for SAP does not exists");
                    Assert.IsNotNull(ex);
                }

                // Create Homologation between 7 to 1
                var setupHomologationRequestForNodes = ApiContent.Creates[ConstantValues.HomologationForNodesWithTank];
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(step.ScenarioContext["NodeId_1"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(step.ScenarioContext["NodeId_2"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(step.ScenarioContext["NodeId_3"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(step.ScenarioContext["NodeId_4"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("sourceSystemId", 7);
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["TankName1"] = step.ScenarioContext["NodeId"].ToString();
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node5 destinationValue", step.ScenarioContext["TankName1"].ToString());
                await ecpApiStep.CreateNodesWithOwnershipAsync().ConfigureAwait(false);
                step.ScenarioContext["TankName2"] = step.ScenarioContext["NodeId"].ToString();
                setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node6 destinationValue", step.ScenarioContext["TankName2"].ToString());
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for Products
                var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
                setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 7);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for Unit
                var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
                setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 7);
                var homologationDataMappingForAttributeUnit = ApiContent.Creates[ConstantValues.HomologationDataMapping];
                homologationDataMappingForAttributeUnit = homologationDataMappingForAttributeUnit.JsonChangePropertyValue("sourceValue", "Automation_ValueAttributeUnit");
                homologationDataMappingForAttributeUnit = homologationDataMappingForAttributeUnit.JsonChangePropertyValue("destinationValue", step.GetValueInternal("ValueAttributeUnitId"));
                setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonAddObject("HomologationObj homologationDataMapping", homologationDataMappingForAttributeUnit);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for MovementTypeId
                var setupHomologationRequestForMovementType = ApiContent.Creates[ConstantValues.HomologationForMovementTypeId];
                setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("sourceSystemId", 7);
                setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("HomologationDataMapping_MovementTypeId destinationValue", int.Parse(step.ScenarioContext["MovementTypeId"].ToString(), CultureInfo.InvariantCulture));
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForMovementType)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for ProductTypeId
                var setupHomologationRequestForProductType = ApiContent.Creates[ConstantValues.HomologationForProductTypeId];
                setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_ProductTypeId destinationValue", int.Parse(step.ScenarioContext["ProductTypeId"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_SourceProductTypeId destinationValue", int.Parse(step.ScenarioContext["SourceProductTypeId"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_DestinationProductTypeId destinationValue", int.Parse(step.ScenarioContext["DestinationProductTypeId"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("sourceSystemId", 7);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProductType)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for OwnerId
                var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
                setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", int.Parse(step.ScenarioContext["Owner"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 7);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for Segment
                var setupHomologationRequestForSegment = ApiContent.Creates[ConstantValues.HomologationForSegment];
                setupHomologationRequestForSegment = setupHomologationRequestForSegment.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", step.ScenarioContext["SegmentId"].ToString());
                setupHomologationRequestForSegment = setupHomologationRequestForSegment.JsonChangePropertyValue("sourceSystemId", 7);
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForSegment)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for Operator
                var setupHomologationRequestForOperator = ApiContent.Creates[ConstantValues.HomologationForOperator];
                setupHomologationRequestForOperator = setupHomologationRequestForOperator.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", step.ScenarioContext["OperatorId"].ToString());
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOperator)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for AttributeId
                var setupHomologationRequestForAttributeId = ApiContent.Creates[ConstantValues.HomologationForAttributeId];
                setupHomologationRequestForAttributeId = setupHomologationRequestForAttributeId.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", step.ScenarioContext["AttributeId"].ToString());
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForAttributeId)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for ValueAttributeUnit
                ////var setupHomologationRequestForValueAttributeUnit = ApiContent.Creates[ConstantValues.HomologationForValueAttributeUnit];
                ////setupHomologationRequestForValueAttributeUnit = setupHomologationRequestForValueAttributeUnit.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", step.ScenarioContext["ValueAttributeUnitId"].ToString());
                ////await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForValueAttributeUnit)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for StorageLocationId
                var nodeStorageLocationDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeStorageLocationByNodeId, args: new { nodeId = step.ScenarioContext["NodeId_2"].ToString() }).ConfigureAwait(false);
                step.ScenarioContext["SourceStorageLocationId"] = nodeStorageLocationDetails[ConstantValues.NodeStorageLocationIdColumn];
                nodeStorageLocationDetails = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetNodeStorageLocationByNodeId, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                step.ScenarioContext["DestinationStorageLocationId"] = nodeStorageLocationDetails[ConstantValues.NodeStorageLocationIdColumn];
                var setupHomologationRequestForStorageLocation = ApiContent.Creates[ConstantValues.HomologationForStorageLocation];
                setupHomologationRequestForStorageLocation = setupHomologationRequestForStorageLocation.JsonChangePropertyValue("HomologationDataMapping_Source destinationValue", int.Parse(step.ScenarioContext["SourceStorageLocationId"].ToString(), CultureInfo.InvariantCulture));
                setupHomologationRequestForStorageLocation = setupHomologationRequestForStorageLocation.JsonChangePropertyValue("HomologationDataMapping_Destination destinationValue", int.Parse(step.ScenarioContext["DestinationStorageLocationId"].ToString(), CultureInfo.InvariantCulture));
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForStorageLocation)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for System
                var setupHomologationRequestForSystem = ApiContent.Creates[ConstantValues.HomologationForSystem];
                setupHomologationRequestForSystem = setupHomologationRequestForSystem.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", step.ScenarioContext["SystemElementId"].ToString());
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForSystem)).ConfigureAwait(false)).ConfigureAwait(false);

                // Create Homologation between 7 to 1 for SourceSystem
                var setupHomologationRequestForSourceSystem = ApiContent.Creates[ConstantValues.HomologationForSourceSystem];
                await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForSourceSystem)).ConfigureAwait(false)).ConfigureAwait(false);
            }
            else if (systemType.EqualsIgnoreCase("Excel"))
            {
                step.ScenarioContext["Count"] = "0";

                if (step.GetValueInternal("TransferPointNodes").EqualsIgnoreCase("True"))
                {
                    // Create Transfer Point Nodes
                    await ecpApiStep.CreateNodesForTransferPointTestDataAsync().ConfigureAwait(false);
                }
                else
                {
                    // Create Nodes
                    await ecpApiStep.CreateNodesForTestDataAsync().ConfigureAwait(false);
                }

                // Create system for Homologation
                ////await ecpApiStep.CreateCatergoryElementAsync("8", step.ScenarioContext["SegmentName"].ToString() + "_Sistema").ConfigureAwait(false);
                ////step.ScenarioContext["SystemElementId"] = step.ScenarioContext["CategoryElement"].ToString();

                // create elements
                await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

                // Create Connections between created nodes
                await ecpApiStep.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

                // Homologation between 3 to 1
                await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);
            }
        }

        public static void ISelectFileFromFileUploadDropdown(this StepDefinitionBase step, string action)
        {
            step.ThrowIfNull(nameof(step));
            step.ScenarioContext["ActionType"] = action;
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            step.Get<ElementPage>().Click(nameof(Resources.UploadDropDown));
            step.Get<ElementPage>().Click(nameof(Resources.UploadType), UIContent.Conversion[step.ScenarioContext["ActionType"].ToString()], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), UIContent.Conversion[action]).Count);
        }

        public static void IClickOnUploadButton(this StepDefinitionBase step, string locator)
        {
            step.ThrowIfNull(nameof(step));
            input = step.Get<ElementPage>().GetElement(nameof(Resources.Browse));
            input.ClientClick();
            Assert.IsNotNull(locator);
        }

        public static async Task ISelectFileFromDirectoryAsync(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            fileName.ThrowIfNull(nameof(fileName));
            var path = fileName.Contains("Invalid") ? FilePaths.FilePath + fileName + ".txt" : FilePaths.FilePath + fileName + ".xlsx";
            input.SendKeys(path.GetFullPath());
            await Task.Delay(3000).ConfigureAwait(true);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            await Task.Delay(3000).ConfigureAwait(true);
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
        }

        public static void WhenISelectInContractsFromMovementTypeDropdown(this StepDefinitionBase step, string type)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 15);
            step.Get<ElementPage>().Click(nameof(Resources.FileUploadTypeDropDown));
            step.Get<ElementPage>().Click(nameof(Resources.UploadType), UIContent.Conversion[type], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), UIContent.Conversion[type]).Count);
        }

        public static void WhenISelectFileFromGrid(this StepDefinitionBase step, string operation)
        {
            operation.ThrowIfNull(nameof(operation));
            step.ThrowIfNull(nameof(step));
            string path;
            switch (operation)
            {
                case "InvalidFormat":
                    path = operation + ".txt";
                    break;
                case "WithoutRecords":
                    path = operation + ".xlsx";
                    break;
                case "WithoutSourceColumn":
                    path = operation + ".xlsx";
                    break;
                case "InsertContractExcel":
                    path = operation + ".xlsx";
                    break;
                case "UpdateContractExcel":
                    path = operation + ".xlsx";
                    break;
                case "DeleteContractExcel":
                    path = operation + ".xlsx";
                    break;
                default:
                    path = "ValidExcel.xlsx";
                    break;
            }

            var finalPath = FilePaths.ContractsFilePath + path;
            input.SendKeys(finalPath.GetFullPath());
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
            Assert.IsTrue(true);
        }

        public static async Task IShouldSeeTheInTheSystemAsync(this StepDefinitionBase step, string entity, string type)
        {
            step.ThrowIfNull(nameof(step));
            await Task.Delay(10000).ConfigureAwait(true);
            if (entity.EqualsIgnoreCase("event"))
            {
                var lastRow = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastEvent).ConfigureAwait(false);
                if (type == "Inserted")
                {
                    Assert.AreEqual(step.ScenarioContext["NodeId_1"].ToString(), lastRow["SourceNodeId"]);
                    Assert.AreEqual(step.ScenarioContext["NodeId_2"].ToString(), lastRow["DestinationNodeId"]);
                }
                else if (type == "Updated")
                {
                    Assert.AreEqual(ConstantValues.EventValue, lastRow["Volume"]);
                }
                else if (type == "Deleted")
                {
                    Assert.AreEqual(ConstantValues.InactiveEvent, lastRow["IsDeleted"]);
                }
            }
            else if (entity.EqualsIgnoreCase("contract"))
            {
                var lastPurchaseRow = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastPurchaseContract).ConfigureAwait(false);
                var lastSaleRow = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastSaleContract).ConfigureAwait(false);
                if (type == "Inserted")
                {
                    Assert.AreEqual(step.ScenarioContext["NodeId_2"], lastPurchaseRow["SourceNodeId"]);
                    Assert.AreEqual(step.ScenarioContext["NodeId_1"], lastPurchaseRow["DestinationNodeId"]);
                    Assert.AreEqual(step.ScenarioContext["NodeId_1"], lastSaleRow["SourceNodeId"]);
                    Assert.AreEqual(step.ScenarioContext["NodeId_4"], lastSaleRow["DestinationNodeId"]);
                }
                else if (type == "Updated")
                {
                    Assert.AreEqual(ConstantValues.ContractPurchaseValue, lastPurchaseRow["Volume"]);
                    Assert.AreEqual(ConstantValues.ContractSaleValue, lastSaleRow["Volume"]);
                }
                else if (type == "Deleted")
                {
                    Assert.AreEqual(ConstantValues.InactiveContract, lastPurchaseRow["IsDeleted"]);
                    Assert.AreEqual(ConstantValues.InactiveContract, lastSaleRow["IsDeleted"]);
                }
            }
        }

        public static async Task VerifyThatDeltaCalculationShouldBeSuccessfulAsync(this StepDefinitionBase step)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var ticketRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetDeltaTicketStatus).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.TicketStatus] = ticketRow["Status"];
            for (int i = 1; i <= 4; i++)
            {
                step.ScenarioContext[ConstantValues.TicketId] = ticketRow[ConstantValues.TicketId].ToString();
                step.Get<ElementPage>().SetValue(nameof(Resources.DeltaColumnHeader), formatArgs: "ticketId", value: step.ScenarioContext["TicketId"].ToString());
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.DeltaColumnHeader), formatArgs: "ticketId");
                step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["DeltaStatus"]).Text;
                if (status.EqualsIgnoreCase("Finalizado") || status.EqualsIgnoreCase("Fallido"))
                {
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }
            }
        }

        public static async Task VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync(this StepDefinitionBase step)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var ticketRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetOfficialDeltaTicketStatus).ConfigureAwait(false);
            step.SetValueInternal("Official Delta TicketId", ticketRow[ConstantValues.TicketId].ToString());
            for (int i = 1; i <= 4; i++)
            {
                step.ScenarioContext[ConstantValues.TicketId] = ticketRow[ConstantValues.TicketId].ToString();
                step.Get<ElementPage>().SetValue(nameof(Resources.DeltaColumnHeader), formatArgs: "ticketId", value: step.ScenarioContext["TicketId"].ToString());
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.DeltaColumnHeader), formatArgs: "ticketId");
                step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["OfficialDeltaStatus"]).Text;
                if (status.EqualsIgnoreCase("Deltas") || status.EqualsIgnoreCase("Fallido"))
                {
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }
            }
        }

        public static async Task VerifyThatLogisticOfficialBalanceFileGenerationShouldBeSuccessfulAsync(this StepDefinitionBase step)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var ticketRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetTicketBySegmentName, args: new { segment = step.GetValueInternal("SegmentName"), ticketType = 6 }).ConfigureAwait(false);
            step.SetValueInternal("OfficialLogisticTicketId", ticketRow[ConstantValues.TicketId].ToString());
            for (int i = 1; i <= 4; i++)
            {
                step.Get<ElementPage>().SetValue(nameof(Resources.DeltaColumnHeader), formatArgs: "segment", value: step.GetValueInternal("SegmentName"));
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.DeltaColumnHeader), formatArgs: "segment");
                step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.DeltaColumnGrid), UIContent.GridPosition["LogisticsOfficialStatus"]).Text;
                step.SetValueInternal("LogisticsOfficialStatus", status);
                if (status.EqualsIgnoreCase("Finalizado") || status.EqualsIgnoreCase("Fallido"))
                {
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }
            }
        }

        public static async Task WaitForFileUploadingToCompleteAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);
            var lastCreatedRow = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetUploadId).ConfigureAwait(false);
            for (int i = 1; i <= 8; i++)
            {
                step.ScenarioContext[ConstantValues.UploadId] = lastCreatedRow[ConstantValues.UploadId];
                step.ScenarioContext[ConstantValues.MessageId] = lastCreatedRow[ConstantValues.UploadId];
                step.Get<ElementPage>().SetValue(nameof(Resources.UploadIdFilter), step.ScenarioContext["UploadId"].ToString());
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.UploadIdFilter));
                step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.FileStatus)).Text;
                step.ScenarioContext["ExcelUploadStatus"] = status;
                if (status.EqualsIgnoreCase("Fallido") || status.EqualsIgnoreCase("Finalizado"))
                {
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }
            }

            step.Get<ElementPage>().ClearText(nameof(Resources.UploadIdFilter));
            step.Get<ElementPage>().SendEnterKey(nameof(Resources.UploadIdFilter));
        }

        public static async Task WaitForOfficialDeltaCalculationAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 5);

            for (int i = 1; i <= 8; i++)
            {
                step.Get<ElementPage>().SetValue(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable), step.ScenarioContext["CategorySegment"].ToString());
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.FileStatus)).Text;
                step.ScenarioContext["OfficialDeltaCalculationStatus"] = status;
                if (status.EqualsIgnoreCase("Fallido") || status.EqualsIgnoreCase("Deltas"))
                {
                    await Task.Delay(15000).ConfigureAwait(true);
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }
            }

            step.Get<ElementPage>().ClearText(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable));
            step.Get<ElementPage>().SendEnterKey(nameof(Resources.SegmentFilterOnOfficialDeltaCalculationTable));
        }

        public static void IShouldSeeUploadNewFileInterface(this StepDefinitionBase step)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            Assert.IsTrue(step.Get<ElementPage>().GetElement(nameof(Resources.ElementByText), formatArgs: ConstantValues.ContentOnUploadFile).Displayed);
        }

        public static void ISelectFromFileTypeDropdown(this StepDefinitionBase step, string fileType)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            step.Get<ElementPage>().Click(nameof(Resources.UploadFileTypeDropdownOfFileUploadGrid));
            step.Get<ElementPage>().Click(nameof(Resources.UploadType), UIContent.Conversion[fileType], step.Get<ElementPage>().GetElements(nameof(Resources.UploadTypeName), UIContent.Conversion[fileType]).Count);
        }

        public static async Task ISelectFileFromExplorerAsync(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            string path;
            switch (fileName)
            {
                case "InvalidFormat":
                    path = fileName + ".txt";
                    break;
                case "WithoutRecords":
                    path = fileName + ".xlsx";
                    break;
                case "WithoutSourceColumn":
                    path = fileName + ".xlsx";
                    break;
                case "ValidExcel":
                    path = fileName + ".xlsx";
                    break;
                case "UpdateExcel":
                    path = fileName + ".xlsx";
                    break;
                case "PeriodOverlap":
                    path = fileName + ".xlsx";
                    break;
                case "InsertEventExcel":
                    path = fileName + ".xlsx";
                    break;
                case "UpdateEventExcel":
                    path = fileName + ".xlsx";
                    break;
                case "DeleteEventExcel":
                    path = fileName + ".xlsx";
                    break;
                case "ValidFicoEvents":
                    path = fileName + ".xlsx";
                    break;

                default:
                    path = "ValidExcel.xlsx";
                    break;
            }

            var finalPath = FilePaths.EventsFilePath + path;
            input.SendKeys(finalPath.GetFullPath());
            await Task.Delay(3000).ConfigureAwait(true);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
            await Task.Delay(3000).ConfigureAwait(true);
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
        }

        public static async Task IVerifyIfIHaveRegisteredTheContractsAsync(this StepDefinitionBase step)
        {
            await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
            step.ThrowIfNull(nameof(step));
            var contractRow = await step.ReadSqlAsStringDictionaryAsync(ApiContent.LastCreated[ConstantValues.Contract], args: new { documentNumber = step.ScenarioContext[ConstantValues.Contract].ToString() }).ConfigureAwait(false);
            Assert.IsNotNull(contractRow);
        }

        public static void SetValueInternal<T>(this StepDefinitionBase step, string key, T value)
        {
            step.ThrowIfNull(nameof(step));
            step.ScenarioContext.Set(value, key);
        }

        public static string GetValueInternal(this StepDefinitionBase step, string key)
        {
            if (step.ScenarioContext.ContainsKey(key))
            {
                return step.ScenarioContext.Get<string>(key);
            }

            return null;
        }

        public static async Task TestDataForOwnershipCalculationAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

            // Create Nodes
            await ecpApiStep.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);

            //// Generation of System Reports
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_2"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_3"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_4"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);

            // Creating annulation movement
            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements)))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = step.GetValueInternal("MovementTypeId"), AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId"), isActive = 1 }).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport)))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = step.GetValueInternal("MovementTypeId"), AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId"), isActive = 1 }).ConfigureAwait(false);
                for (int i = 42; i <= 44; i++)
                {
                    await step.ReadAllSqlAsync(input: SqlQueries.DeleteAnnulationBySourceMovementTypeId, args: new { sourceMovementTypeId = i }).ConfigureAwait(false);
                    await ecpApiStep.CreateCatergoryElementAsync("9").ConfigureAwait(false);
                    step.SetValueInternal("AnnulationId", step.ScenarioContext["CategoryElement"]);
                    await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = i, AnnulationMovementTypeId = step.GetValueInternal("AnnulationId"), isActive = 1 }).ConfigureAwait(false);
                }
            }

            // Updating Node Tags
            if (string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.FICO))
                && (string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation))
                && string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements))
                && string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport))))
            {
                var elementRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = step.ScenarioContext["CategorySegment"].ToString() }).ConfigureAwait(false);
                var elementId = elementRow["ElementId"];
                var otherElementId = await step.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
                if (!string.IsNullOrEmpty(step.GetValueInternal("NodeStatus")))
                {
                    await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 10, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal("InitialCutOff")))
                {
                    await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 5, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal("OfficialMovementsInventories")))
                {
                    await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 63, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                }
                else
                {
                    await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
                }

                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId1, args: new { nodeTagId = nodeTagIds[0] }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId2, args: new { nodeTagId = nodeTagIds[9] }).ConfigureAwait(false);
            }

            if (!string.IsNullOrEmpty(step.GetValueInternal("OwnershipOperation")))
            {
                ////step.When("I update the excel with \"TestData_Ownership\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("TestData_Ownership");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("NodeStatus")))
            {
                ////step.When("I update the excel with \"TestData_NodeStatus\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("TestData_NodeStatus");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("InitialCutOff")))
            {
                ////step.When("I update the excel with \"TestData_InitialCutOff\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("TestData_InitialCutOff");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation))
                || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements)))
            {
                ////step.When("I update the excel with \"TestData_Consolidation\" data");
                step.WhenIUpdateTheExcelFileWithConsolidationData("TestData_Consolidation");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("OfficialMovementsInventories")))
            {
                ////step.When("I update the excel with \"TestData_InitialCutOff\" data");
                step.WhenIUpdateTheExcelFileWithTwoOwners("TestData_OfficialMovementsInventories");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestDataForConsolidatedWithOfficial)))
            {
                ////step.When("I update the excel with \"TestData_InitialCutOff\" data");
                step.WhenIUpdateTheExcelFileWithOfficialDeltaConsolidationData("TestData_OfficialwithMultipleOwners");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.FICO)))
            {
                ////step.When("I update the excel with \"TestData_FICO\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("TestData_FICO");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport)))
            {
                step.WhenIUpdateTheExcelFileForOfficialDeltaPerNodeReport("TestData_BalanceOfficialPerNode");
            }
            else
            {
                ////step.When("I update the excel with \"TestDataCutOff_Daywise\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("TestDataCutOff_Daywise");
            }

            ////step.When("I navigate to \"FileUpload\" page");
            step.UiNavigation("FileUpload");
            ////step.When("I click on \"FileUpload\" \"button\"");
            step.IClickOn("FileUpload", "button");
            ////step.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            step.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////step.When("I select \"Insert\" from FileUpload dropdown");
            step.ISelectFileFromFileUploadDropdown("Insert");
            ////step.When("I click on \"Browse\" to upload");
            step.IClickOnUploadButton("Browse");
            if (!string.IsNullOrEmpty(step.GetValueInternal("OwnershipOperation")))
            {
                ////step.When("I select \"TestData_Ownership\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_Ownership").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("NodeStatus")))
            {
                ////step.When("I select \"TestData_NodeStatus\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_NodeStatus").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("InitialCutOff")))
            {
                ////step.When("I select \"TestData_InitialCutOff\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_InitialCutOff").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation))
                || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements)))
            {
                ////step.When("I select \"TestData_Consolidation\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_Consolidation").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("OfficialMovementsInventories")))
            {
                await step.ISelectFileFromDirectoryAsync("TestData_OfficialMovementsInventories").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestDataForConsolidatedWithOfficial)))
            {
                await step.ISelectFileFromDirectoryAsync("TestData_OfficialwithMultipleOwners").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.FICO)))
            {
                await step.ISelectFileFromDirectoryAsync("TestData_FICO").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport)))
            {
                await step.ISelectFileFromDirectoryAsync("TestData_BalanceOfficialPerNode").ConfigureAwait(false);
            }
            else
            {
                ////step.When("I select \"TestDataCutOff_Daywise\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestDataCutOff_Daywise").ConfigureAwait(false);
            }

            ////step.When("I click on \"FileUpload\" \"Save\" \"button\"");
            step.IClickOn("uploadFile\" \"Submit", "button");
            ////step.When("I wait till file upload to complete");
            await step.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public static async Task TestDataForOwnershipCalculationForOfficialDeltaAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

            // Create Nodes
            await ecpApiStep.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);

            //// Generation of System Reports
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_2"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_3"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_4"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);

            // Updating Node Tags
            var elementRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = step.ScenarioContext["CategorySegment"].ToString() }).ConfigureAwait(false);
            var elementId = elementRow["ElementId"];
            var otherElementId = await step.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
            if (!string.IsNullOrEmpty(step.GetValueInternal("NodeStatus")))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 10, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("InitialCutOff")))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 5, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation)))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 63, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("OfficialMovementsInventories")))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 63, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            }
            else
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            }

            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId1, args: new { nodeTagId = nodeTagIds[0] }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId2, args: new { nodeTagId = nodeTagIds[9] }).ConfigureAwait(false);

            ////step.When("I update the excel with \"TestDataCutOff_Daywise\" data");
            ////    step.WhenIUpdateTheExcelFileWithDaywiseData("TestDataCutOff_OfficialDelta");

            // Updating the current date as Start date for the Node on which inventory is made
            ////await step.ReadAllSqlAsync(SqlQueries.UpdateTheNodeTagForOfficialDelta, args: new { CurrentDateAsStartDate = DateTime.UtcNow.ToString("dd-MMM-yy") + " 00:00:00.000", NodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            await step.ReadAllSqlAsync(SqlQueries.UpdateTheNodeTagForOfficialDelta, args: new { NodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);

            step.WhenIUpdateTheExcelFileWithOfficialDeltaData("TestDataCutOff_OfficialDelta");

            ////step.When("I navigate to \"FileUpload\" page");
            step.UiNavigation("FileUpload");
            ////step.When("I click on \"FileUpload\" \"button\"");
            step.IClickOn("FileUpload", "button");
            ////step.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            step.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////step.When("I select \"Insert\" from FileUpload dropdown");
            step.ISelectFileFromFileUploadDropdown("Insert");
            ////step.When("I click on \"Browse\" to upload");
            step.IClickOnUploadButton("Browse");

            await step.ISelectFileFromDirectoryAsync("TestDataCutOff_OfficialDelta").ConfigureAwait(false);

            ////step.When("I click on \"FileUpload\" \"Save\" \"button\"");
            step.IClickOn("uploadFile\" \"Submit", "button");
            ////step.When("I wait till file upload to complete");
            await step.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public static async Task TestDataForOwnershipCalculationAnalyticalModelAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

            // Create Nodes
            await ecpApiStep.CreateNodesForAnalyticalModelAsync().ConfigureAwait(false);

            // create elements
            await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionsForTestDataAnalyticalModelAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);

            // Updating Node Tags
            var elementRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = step.ScenarioContext["CategorySegment"].ToString() }).ConfigureAwait(false);
            var elementId = elementRow["ElementId"];
            var otherElementId = await step.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId1, args: new { nodeTagId = nodeTagIds[0] }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId2, args: new { nodeTagId = nodeTagIds[9] }).ConfigureAwait(false);

            ////step.When("I update the excel with \"TestDataCutOff_Daywise\" data");
            step.WhenIUpdateTheExcelFileWithDaywiseData("TestData_AnalyticalModel");

            ////step.When("I navigate to \"FileUpload\" page");
            step.UiNavigation("FileUpload");
            ////step.When("I click on \"FileUpload\" \"button\"");
            step.IClickOn("FileUpload", "button");
            ////step.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            step.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////step.When("I select \"Insert\" from FileUpload dropdown");
            step.ISelectFileFromFileUploadDropdown("Insert");
            ////step.When("I click on \"Browse\" to upload");
            step.IClickOnUploadButton("Browse");
            ////step.When("I select \"TestDataCutOff_Daywise\" file from directory");
            await step.ISelectFileFromDirectoryAsync("TestData_AnalyticalModel").ConfigureAwait(false);

            ////step.When("I click on \"FileUpload\" \"Save\" \"button\"");
            step.IClickOn("uploadFile\" \"Submit", "button");
            ////step.When("I wait till file upload to complete");
            await step.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public static async Task IHaveOwnershipCalculationDataGeneratedInTheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            if (!string.IsNullOrEmpty(step.GetValueInternal("NodeStatus")))
            {
                ////step.Given("I have \"ownershipnodes\" created for node status");
                await ecpApiStep.IHaveCreatedForNodeStatusAsync("ownershipnodes").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal("OfficialMovementsInventories")))
            {
                await ecpApiStep.IHaveOwnershipNodesForOfficialMovementsAndInventoriesCreatedAsync("ownershipnodes").ConfigureAwait(false);
            }
            else
            {
                ////step.Given("I have \"ownershipnodes\" created");
                await step.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(step.GetValueInternal("OfficialMovementsInventories")))
            {
                //// CutOff and Ownership Ticket Generation
                await step.CutoffAndOwnershipTicketGenerationAsync().ConfigureAwait(false);
            }
        }

        public static async Task CutoffAndOwnershipTicketGenerationAsync(this StepDefinitionBase step)
        {
            if (string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.NoSONSegment)))
            {
                ////step.When("I navigate to \"Operational Cutoff\" page");
                step.UiNavigation("Operational Cutoff");
                ////step.When("I click on \"NewCut\" \"button\"");
                step.IClickOn("NewCut", "button");
                ////step.When("I choose CategoryElement from \"InitTicket\" \"Segment\" \"combobox\"");
                step.ISelectCategoryElementFrom("InitTicket\" \"Segment", "combobox");
                if (!string.IsNullOrEmpty(step.GetValueInternal("OwnershipOperation")))
                {
                    ////step.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Cutoff\" DatePicker");
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Cutoff").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal("NodeStatus")))
                {
                    ////step.When("I select the FinalDate lessthan \"10\" days from CurrentDate on \"Cutoff\" DatePicker");
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(10, "Cutoff").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForAnotherPeriodConsolidation)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(32, "AnotherPeriod_Consildation_Cutoff").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(30, "Consildation_Cutoff").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(25, "Consildation_BeforeOperativeDelta").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements))
                    || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable))
                    || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions))
                    || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "CutOffForOfficialBalanceFile").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestDataForConsolidatedWithOfficial)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "OfficialData_With_Consolidation").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "OfficialData_Report_Cutoff").ConfigureAwait(false);
                }
                else
                {
                    ////step.When("I select the FinalDate lessthan \"4\" days from CurrentDate on \"Cutoff\" DatePicker");
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(4, "Cutoff").ConfigureAwait(false);
                }

                ////step.When("I click on \"InitTicket\" \"next\" \"button\"");
                step.IClickOn("InitTicket\" \"Submit", "button");
                ////step.Then("validate that \"checkInventories\" \"Next\" \"button\" as enabled");
                step.ValidateThatAsEnabled("validateInitialInventory\" \"submit", "button");
                ////step.When("I click on \"checkInventories\" \"Next\" \"button\"");
                step.IClickOn("validateInitialInventory\" \"submit", "button");
                ////step.When("I select all pending records from grid");
                step.ISelectAllPendingRepositroriesFromGrid();
                step.ProvidedRequiredDetailsForPendingTransactionsGrid();
                ////step.Then("validate that \"ErrorsGrid\" \"Next\" \"button\" as enabled");
                step.ValidateThatAsEnabled("ErrorsGrid\" \"Submit", "button");
                ////step.When("I click on \"ErrorsGrid\" \"Next\" \"button\"");
                step.IClickOn("ErrorsGrid\" \"Submit", "button");
                ////step.When("I click on \"officialPointsGrid\" \"Next\" \"button\"");
                step.IClickOn("officialPointsGrid\" \"Submit", "button");
                ////step.When("I select all unbalances in the grid");
                step.ISelectAllPendingRepositroriesFromGrid();
                step.ProvidedRequiredDetailsForUnbalancesGrid();
                ////step.When("I click on \"ConsistencyCheck\" \"Next\" \"button\"");
                step.IClickOn("unbalancesGrid\" \"submit", "button");
                ////step.When("I click on \"Confirm\" \"Cutoff\" \"Submit\" \"button\"");
                step.IClickOn("ConfirmCutoff\" \"Submit", "button");
                ////step.When("I wait till cutoff ticket processing to complete");
                await step.WaitForTicketToCompleteProcessingAsync().ConfigureAwait(false);
                ////step.When("I navigate to \"ownershipcalculation\" page");
                step.UiNavigation("ownershipcalculation");
                ////step.When("I click on \"NewBalance\" \"button\"");
                step.IClickOn("NewBalance", "button");
                ////step.When("I select segment from \"OwnershipCal\" \"Segment\" \"dropdown\"");
                step.ISelectSegmentFrom("OwnershipCal\" \"Segment", "dropdown");
                if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(30, "Consildation_Ownership").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(25, "Consildation_Ownership_BeforeOperativeDelta").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements))
                         || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable))
                         || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions))
                         || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "OwnershipForOfficialBalanceFile").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestDataForConsolidatedWithOfficial)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "OwnershipForOfficialData_With_Consolidation").ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport)))
                {
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "OfficialData_Report_Ownership").ConfigureAwait(false);
                }
                else
                {
                    ////step.When("I select the FinalDate lessthan \"1\" days from CurrentDate on \"Ownership\" DatePicker");
                    await step.ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(1, "Ownership").ConfigureAwait(false);
                }
                ////step.When("I click on \"ownershipCalCriteria\" \"Next\" \"button\"");
                step.IClickOn("ownershipCalculationCriteria\" \"submit", "button");
                ////step.When("I verify all validations passed");
                step.IVerifyValidationsPassed();
                ////step.When("I click on \"ownershipCalValidations\" \"Next\" \"button\"");
                step.IClickOn("ownershipCalulationValidations\" \"submit", "button");
                ////step.When("I click on \"OwnershipCalConfirmation\" \"Execute\" \"button\"");
                step.IClickOn("ownershipCalculationConfirmation\" \"submit", "button");
                await Task.Delay(10000).ConfigureAwait(true);
                ////step.Then("verify the ownership is calculated successfully");
                await step.VerifyTheOwnershipIsCalculatedSuccessfullyAsync().ConfigureAwait(false);
                ////step.When("I wait till ownership ticket geneation to complete");
                await step.WaitForOwnershipTicketProcessingToCompleteAsync().ConfigureAwait(false);
            }
        }

        public static async Task IHaveOwnershipCalculationDataGeneratedInTheSystemForOfficialDeltaAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));

            await step.IHaveOwnershipNodesCreatedForOfficialDeltaAsync("ownershipnodes").ConfigureAwait(false);
        }

        public static async Task IHaveOwnershipNodesCreatedAsync(this StepDefinitionBase step, string ownershipNodes, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            ownershipNodes.ThrowIfNull(nameof(ownershipNodes));
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }
            ////step.Given("I want create TestData for " + ownershipNodes);
            await step.TestDataForOwnershipCalculationAsync().ConfigureAwait(false);
            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.FICO)))
            {
                // Homologation between 5 to 1
                await ecpApiStep.IHaveEventHomologationInTheSystemAsync().ConfigureAwait(false);
            }

            if (string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.FICO))
                && (string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidation))
                && string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForConsolidationExclusionMovements))
                && string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialDeltaPerNodeReport))))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_2"].ToString() }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_3"].ToString() }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_4"].ToString() }).ConfigureAwait(false);
            }
        }

        public static async Task IHaveOwnershipNodesCreatedForOfficialDeltaAsync(this StepDefinitionBase step, string ownershipNodes)
        {
            ownershipNodes.ThrowIfNull(nameof(ownershipNodes));
            step.ThrowIfNull(nameof(step));
            ////step.Given("I want create TestData for " + ownershipNodes);
            await step.TestDataForOwnershipCalculationForOfficialDeltaAsync().ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_2"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_3"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_4"].ToString() }).ConfigureAwait(false);
        }

        public static async Task IHaveOwnershipNodesForOfficialMovementsAndInventoriesCreatedAsync(this StepDefinitionBase step, string ownershipNodes)
        {
            ownershipNodes.ThrowIfNull(nameof(ownershipNodes));
            step.ThrowIfNull(nameof(step));
            ////step.Given("I want create TestData for " + ownershipNodes);
            await step.TestDataForOwnershipCalculationAsync().ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 63, nodeId = step.ScenarioContext["NodeId_2"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 63, nodeId = step.ScenarioContext["NodeId_3"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 63, nodeId = step.ScenarioContext["NodeId_4"].ToString() }).ConfigureAwait(false);
        }

        public static async Task IHaveOwnershipNodesCreatedAnalyticalModelAsync(this StepDefinitionBase step, string ownershipNodes)
        {
            ownershipNodes.ThrowIfNull(nameof(ownershipNodes));
            step.ThrowIfNull(nameof(step));
            ////step.Given("I want create TestData for " + ownershipNodes);
            await step.TestDataForOwnershipCalculationAnalyticalModelAsync().ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_2"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_3"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_4"].ToString() }).ConfigureAwait(false);
        }

        public static async Task ISelectTheFinalDateLessthanDaysFromCurrentDateOnDatePickerAsync(this StepDefinitionBase step, int days, string type)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            await Task.Delay(5000).ConfigureAwait(true);
            if (type == "Cutoff")
            {
                if (page.GetElement(nameof(Resources.StartDatePickerinSegment)).GetAttribute("Disabled") == null)
                {
                    step.ScenarioContext["StartDate"] = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    ////var dayOfStartDate = step.ScenarioContext["StartDate"].ToString().Split('/')[1];
                    page.WaitUntilElementToBeClickable(nameof(Resources.StartDatePickerinSegment));
                    page.Click(nameof(Resources.StartDatePickerinSegment));
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext["StartDate"].ToString());
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Control + 'a');
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Backspace);
                    ////page.GetElement(nameof(Resources.SelectDateOnCalendar), formatArgs: dayOfStartDate).Click();
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext["StartDate"].ToString());
                }

                step.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                ////var dayOfFinalDate = step.ScenarioContext["FinalDate"].ToString().Split('/')[1];
                page.WaitUntilElementToBeClickable(nameof(Resources.FinalDatePickerinSegment));
                page.Click(nameof(Resources.FinalDatePickerinSegment));
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(step.ScenarioContext["FinalDate"].ToString());
                ////page.GetElement(nameof(Resources.SelectDateOnCalendar), formatArgs: dayOfFinalDate).Click();
            }
            else if (type == "Consildation_Cutoff" || type == "Consildation_BeforeOperativeDelta" || type == "AnotherPeriod_Consildation_Cutoff" || type == "CutOffForOfficialBalanceFile" || type == "OfficialData_With_Consolidation" || type.EqualsIgnoreCase("OfficialData_Report_Cutoff"))
            {
                if (page.GetElement(nameof(Resources.StartDatePickerinSegment)).GetAttribute("Disabled") == null)
                {
                    if (type == "AnotherPeriod_Consildation_Cutoff")
                    {
                        step.ScenarioContext["StartDate"] = step.ScenarioContext["AnotherPeriod_Consolidation"];
                    }
                    else if (type == "CutOffForOfficialBalanceFile")
                    {
                        step.ScenarioContext["StartDate"] = step.ScenarioContext["LogisticOfficialBalanceCutoffAndOwnershipDate"];
                    }
                    else if (type == "OfficialData_With_Consolidation")
                    {
                        step.ScenarioContext["StartDate"] = "07/01/2020";
                    }
                    else
                    {
                        step.ScenarioContext["StartDate"] = step.ScenarioContext["PreviousMonthStartDate"];
                    }

                    page.WaitUntilElementToBeClickable(nameof(Resources.StartDatePickerinSegment));
                    page.Click(nameof(Resources.StartDatePickerinSegment));
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext["StartDate"].ToString());
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Control + 'a');
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Backspace);
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext["StartDate"].ToString());
                }

                if (type == "Consildation_BeforeOperativeDelta")
                {
                    step.ScenarioContext["FinalDate"] = step.ScenarioContext["BeforeOperativeDelta_CutoffEndDate"];
                }
                else if (type == "CutOffForOfficialBalanceFile")
                {
                    step.ScenarioContext["FinalDate"] = step.ScenarioContext["LogisticOfficialBalanceCutoffAndOwnershipDate"];
                }
                else if (type == "OfficialData_With_Consolidation")
                {
                    step.ScenarioContext["FinalDate"] = "07/01/2020";
                }
                else
                {
                    step.ScenarioContext["FinalDate"] = step.ScenarioContext["PreviousMonthEndDate"];
                }

                page.WaitUntilElementToBeClickable(nameof(Resources.FinalDatePickerinSegment));
                page.Click(nameof(Resources.FinalDatePickerinSegment));
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(step.ScenarioContext["FinalDate"].ToString());
            }
            else if (type == "Consildation_AfterOperativeDelta")
            {
                step.ScenarioContext["FinalDate"] = step.ScenarioContext["PreviousMonthEndDate"];
                page.WaitUntilElementToBeClickable(nameof(Resources.FinalDatePickerinSegment));
                page.Click(nameof(Resources.FinalDatePickerinSegment));
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(step.ScenarioContext["FinalDate"].ToString());
            }
            else if (type == "Consildation_Ownership" || type == "Consildation_Ownership_BeforeOperativeDelta" || type == "Consildation_Ownership_AfterOperativeDelta" || type == "OwnershipForOfficialBalanceFile" || type == "OwnershipForOfficialData_With_Consolidation" || type == "OfficialData_Report_Ownership")
            {
                if (type == "Consildation_Ownership" || type == "Consildation_Ownership_AfterOperativeDelta" || type == "OfficialData_Report_Ownership")
                {
                    step.ScenarioContext["FinalDate"] = step.ScenarioContext["PreviousMonthEndDate"];
                }
                else if (type == "Consildation_Ownership_BeforeOperativeDelta")
                {
                    step.ScenarioContext["FinalDate"] = step.ScenarioContext["BeforeOperativeDelta_CutoffEndDate"];
                }
                else if (type == "OwnershipForOfficialBalanceFile")
                {
                    step.ScenarioContext["FinalDate"] = step.ScenarioContext["LogisticOfficialBalanceCutoffAndOwnershipDate"];
                }
                else if (type == "OwnershipForOfficialData_With_Consolidation")
                {
                    step.ScenarioContext["FinalDate"] = "07/01/2020";
                }

                var monthOfFinalDate = step.ScenarioContext["FinalDate"].ToString().Split('/')[0];
                var dayOfFinalDate = step.ScenarioContext["FinalDate"].ToString().Split('/')[1].TrimStart('0');
                page.WaitUntilElementToBeClickable(nameof(Resources.OwnershipFinalDate));
                page.Click(nameof(Resources.OwnershipFinalDate));
                page.WaitUntilElementToBeClickable(nameof(Resources.MonthDropdownOnDatePicker));
                page.Click(nameof(Resources.MonthDropdownOnDatePicker));
                page.WaitUntilElementToBeClickable(nameof(Resources.NameOfMonthOnDatePicker), formatArgs: UIContent.Conversion[monthOfFinalDate]);
                page.Click(nameof(Resources.NameOfMonthOnDatePicker), formatArgs: UIContent.Conversion[monthOfFinalDate]);
                page.Click(nameof(Resources.UploadType), dayOfFinalDate, page.GetElements(nameof(Resources.UploadTypeName), dayOfFinalDate).Count);
            }
            else if (type == "Ownership")
            {
                step.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (days == 1)
                {
                    step.Get<ElementPage>().GetElement(nameof(Resources.OwnershipFinalDate)).SendKeys(OpenQA.Selenium.Keys.Enter);
                }
                else
                {
                    var dayOfFinalDate = step.ScenarioContext["FinalDate"].ToString().Split('/')[1].TrimStart('0');
                    page.WaitUntilElementToBeClickable(nameof(Resources.OwnershipFinalDate));
                    page.Click(nameof(Resources.OwnershipFinalDate));
                    page.Click(nameof(Resources.UploadType), dayOfFinalDate, page.GetElements(nameof(Resources.UploadTypeName), dayOfFinalDate).Count);
                }
            }
            else if (type == "FirstCutoff" || type == "SecondCutoff")
            {
                if (page.GetElement(nameof(Resources.StartDatePickerinSegment)).GetAttribute("Disabled") == null)
                {
                    page.WaitUntilElementToBeClickable(nameof(Resources.StartDatePickerinSegment));
                    page.Click(nameof(Resources.StartDatePickerinSegment));
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext[type].ToString());
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Control + 'a');
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Backspace);
                    page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext[type].ToString());
                }

                page.WaitUntilElementToBeClickable(nameof(Resources.FinalDatePickerinSegment));
                page.Click(nameof(Resources.FinalDatePickerinSegment));
                if (type == "SecondCutoff")
                {
                    page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Enter);
                }
                else
                {
                    page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(step.ScenarioContext[type].ToString());
                }
            }
        }

        public static void IChangeTheElementsCountPerPageTo(this StepDefinitionBase step, int recordsCount)
        {
            var ele = new SelectElement(step.Get<ElementPage>().GetElement(nameof(Resources.PaginationDropdown)));
            ele.SelectByText(recordsCount.ToString(CultureInfo.InvariantCulture));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
        }

        public static void ISelectAllPendingRepositroriesFromGrid(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            if (page.CheckIfElementIsPresent(nameof(Resources.PaginationCount)))
            {
                var totalPendingRecords = int.Parse(page.GetElementText(nameof(Resources.PaginationCount)).Split(' ')[4], CultureInfo.InvariantCulture);
                step.ScenarioContext[ConstantValues.PendingTransactions] = totalPendingRecords;
                if (totalPendingRecords > 100)
                {
                    ////step.When("I change the elements count per page to " + totalPendingRecords);
                    var i = totalPendingRecords / 100;
                    step.IChangeTheElementsCountPerPageTo(100);
                    page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    while (i > 0)
                    {
                        page.WaitUntilElementToBeClickable(nameof(Resources.SelectAllCheckBox));
                        page.Click(nameof(Resources.SelectAllCheckBox));
                        step.IClickOn("ErrorsGrid\" \"AddNote", "button");
                        ////step.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                        step.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                        ////step.When("I click on \"AddComment\" \"Submit\" \"button\"");
                        step.IClickOn("AddComment\" \"Submit", "button");
                        i--;
                    }

                    totalPendingRecords %= 100;
                    step.ScenarioContext[ConstantValues.PendingTransactions] = totalPendingRecords;
                }

                if (totalPendingRecords != 0)
                {
                    if (totalPendingRecords > 10 && totalPendingRecords <= 50)
                    {
                        ////step.When("I change the elements count per page to 50");
                        step.IChangeTheElementsCountPerPageTo(50);
                    }
                    else if (totalPendingRecords > 50 && totalPendingRecords <= 100)
                    {
                        ////step.When("I change the elements count per page to 100");
                        step.IChangeTheElementsCountPerPageTo(100);
                    }

                    page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                    page.WaitUntilElementToBeClickable(nameof(Resources.SelectAllCheckBox));
                    page.Click(nameof(Resources.SelectAllCheckBox));
                }
            }
            else
            {
                step.ScenarioContext[ConstantValues.PendingTransactions] = 0;
            }
        }

        public static async Task ISelectANodeToRaiseAnApprovalAsync(this StepDefinitionBase step, string ticketId, string approvalType)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            page.ClearText(nameof(Resources.TicketIdSearchBox));

            //// Serach the nodes with the TicketId
            page.EnterText(nameof(Resources.TicketIdSearchBox), ticketId);
            page.SendEnterKey(nameof(Resources.TicketIdSearchBox));
            await Task.Delay(1500).ConfigureAwait(false);

            //// Get the nodes results
            page.WaitUntilElementIsVisible(nameof(Resources.NodeNameBasedOnItsState));
            var recordsCurrent = page.GetElements(nameof(Resources.NodeNameBasedOnItsState));
            page.ClearText(nameof(Resources.TicketIdSearchBox));
            page.ClearText(nameof(Resources.OwnershipNodeSearchInGrid));

            //// If type is automatic, search using the name of first node
            if (approvalType.EqualsIgnoreCase("automatic"))
            {
                page.EnterText(nameof(Resources.OwnershipNodeSearchInGrid), recordsCurrent[0].Text);
            }
            //// If type is manual, search using the name of second node
            else
            {
                page.EnterText(nameof(Resources.OwnershipNodeSearchInGrid), recordsCurrent[1].Text);
            }

            page.SendEnterKey(nameof(Resources.OwnershipNodeSearchInGrid));
            await Task.Delay(1500).ConfigureAwait(false);
            page.WaitUntilElementIsVisible(nameof(Resources.EditOwnershipButton));

            //// Navigate to the Node details page by clicking on the Edit button of the resultant node on the page
            page.Click(nameof(Resources.EditOwnershipButton));
            page.WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));

            //// Get the value of 'OwnershipNodeId' from the url and set it in the Scenario context for further reference
            string urlValue = step.DriverContext.Driver.Url;
            step.ScenarioContext["CurrentOwnershipNodeId"] = urlValue.Substring(urlValue.Length - 6).Split('_')[0];

            //// Set the name of current node in the Scenario context for further reference
            step.ScenarioContext["NodeNameForRejection"] = page.GetElementText(nameof(Resources.NodeNameOnNodeDetailsPage));
        }

        public static async Task WaitForTicketToCompleteProcessingAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var lastCreatedRow = await step.ReadSqlAsStringDictionaryAsync(SqlQueries.GetLastTicket).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.TicketStatus] = lastCreatedRow["Status"];
            for (int i = 1; i <= 8; i++)
            {
                step.ScenarioContext[ConstantValues.TicketId] = lastCreatedRow[ConstantValues.TicketId];
                step.Get<ElementPage>().SetValue(nameof(Resources.TicketIdFilter), step.ScenarioContext["TicketId"].ToString());
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.TicketIdFilter));
                step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.FileStatus)).Text;
                if (status.EqualsIgnoreCase("Fallido") || status.EqualsIgnoreCase("Finalizado"))
                {
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }
            }

            step.ScenarioContext.Set<string>(step.ScenarioContext["TicketId"].ToString(), "TicketId");
            step.Get<ElementPage>().ClearText(nameof(Resources.TicketIdFilter));
            step.Get<ElementPage>().SendEnterKey(nameof(Resources.TicketIdFilter));
        }

        public static void IVerifyAllValidationsPassed(this StepDefinitionBase step, int totalValidations)
        {
            var successIcons = step.Get<ElementPage>().GetElements(nameof(Resources.SuccessValidationIcon));
            Assert.AreEqual(successIcons.Count, totalValidations);
        }

        public static async Task VerifyTheOwnershipIsCalculatedSuccessfullyAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            var ticketRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetLastTicketAndElement).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.TicketId] = ticketRow[ConstantValues.TicketId].ToString();
            Assert.AreEqual(ticketRow[ConstantValues.Name].ToString(), step.ScenarioContext["Segment"].ToString());
        }

        public static async Task WaitForOwnershipTicketProcessingToCompleteAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader), 10);
            var ticketRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetLastTicketAndElement).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.TicketStatus] = ticketRow["Status"];
            for (int i = 1; i <= 30; i++)
            {
                step.ScenarioContext[ConstantValues.TicketId] = ticketRow[ConstantValues.TicketId].ToString();
                step.Get<ElementPage>().SetValue(nameof(Resources.TicketIdFilter), step.ScenarioContext["TicketId"].ToString());
                step.Get<ElementPage>().SendEnterKey(nameof(Resources.TicketIdFilter));
                step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
                var status = step.Get<ElementPage>().GetElement(nameof(Resources.FileStatus)).Text;
                step.ScenarioContext[ConstantValues.TicketStatus] = status;
                if (status.EqualsIgnoreCase("Propiedad") || status.EqualsIgnoreCase("Fallido"))
                {
                    break;
                }
                else
                {
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
                }

                step.ScenarioContext[ConstantValues.OwnershipTicketStatus] = status;
            }
        }

        public static async Task IHaveInventoryOrMovementDataToProcessInSystemAsync(this StepDefinitionBase step, string entity)
        {
            step.ThrowIfNull(nameof(step));
            var content = ApiContent.Creates[entity];
            step.ScenarioContext[Entities.Keys.EntityType] = entity;
            step.ScenarioContext[step.ScenarioContext[Entities.Keys.EntityType].ToString()] = content;
            ////step.Given(string.Format(CultureInfo.InvariantCulture, "I have \"{0}\" homologation data in the system", "SAPPO"));
            await step.IHaveHomologationDataInTheSystemAsync(systemType: "SAPPO").ConfigureAwait(false);
        }

        public static void ISelectOwnerOnTheCreateFileInterface(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.OwnerOnCreateLogisticsInterface), formatArgs: UIContent.GridPosition[ConstantValues.Name]);
            step.Get<ElementPage>().GetElement(nameof(Resources.OwnerOnCreateLogisticsInterface), formatArgs: UIContent.GridPosition[ConstantValues.Name]).Click();
        }

        public static async Task ReceiveTheDataWithThatExceedsCharactersAsync(this StepDefinitionBase step, string field1, int field2, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await ecpApiStep.IDonTReceiveInXMLAsync(field1 + "_Characters").ConfigureAwait(false);
            Assert.IsNotNull(field2);
        }

        public static async Task UpdateXmlForSinoperAsync(this StepDefinitionBase step, string field, string xmlField, string fieldValue, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            Assert.IsNotNull(fieldValue);
            ecpApiStep.UpdateXmlDefaultValue(step.ScenarioContext[Entities.Keys.EntityType].ToString());
            BlobExtensions.UpdateXmlData(step.ScenarioContext[Entities.Keys.EntityType].ToString() + "\\" + field, xmlField, fieldValue);
            await ecpApiStep.PerformHomologationAsync(step.ScenarioContext[Entities.Keys.EntityType].ToString()).ConfigureAwait(false);
        }

        public static void SapPoInventoryRegistration(this StepDefinitionBase step, int inventoriesCount = 1, int lengthOfField = 1, string attribute = null, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            ecpApiStep.CommonMethodForInventoryRegistration(inventoriesCount, lengthOfField, attribute);
        }

        public static async Task IWantToRegisterAnInTheSystemAsync(this StepDefinitionBase step, string entity)
        {
            if (entity.EqualsIgnoreCase("Inventory_MultipleProduct"))
            {
                step.ScenarioContext[Entities.Keys.EntityType] = entity.Split('_')[0];
                step.ScenarioContext["MultipleProduct"] = "Yes";
            }
            else
            {
                step.ScenarioContext[Entities.Keys.EntityType] = entity;
            }

            await step.CreateHomologationAsync("Yes").ConfigureAwait(false);
        }

        public static async Task IWantToCancelOrAdjustAnAsync(this StepDefinitionBase step, string entity, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await step.IWantToRegisterAnInTheSystemAsync(entity).ConfigureAwait(false);
            ecpApiStep.UpdateXmlDefaultValue(entity);
            await ecpApiStep.PerformHomologationAsync(entity).ConfigureAwait(false);
        }

        public static async Task TestDataForCutOffAsync(this StepDefinitionBase step, string days, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            step.ScenarioContext["Count"] = "0";

            // Create Nodes
            await ecpApiStep.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);
            var elementRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = step.ScenarioContext["CategorySegment"].ToString() }).ConfigureAwait(false);
            var elementId = elementRow["ElementId"];
            var otherElementId = await step.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            ////await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId1, args: new { nodeTagId = nodeTagIds[0] }).ConfigureAwait(false);
            ////await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId2, args: new { nodeTagId = nodeTagIds[9] }).ConfigureAwait(false);

            if (days == "SingleDay")
            {
                ////step.When("I update the excel with \"SingleDay\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("SingleDay");
            }
            else if (days == "Testdata_20215")
            {
                ////step.When("I update the excel with \"Testdata_20215\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("Testdata_20215");
            }
            else if (days == "Testdata_20215_0Volume")
            {
                ////step.When("I update the excel with \"Testdata_20215_0Volume\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("Testdata_20215_0Volume");
            }
            else
            {
                ////step.When("I update the excel with \"TestDataCutOff_Daywise\" data");
                step.WhenIUpdateTheExcelFileWithDaywiseData("TestDataCutOff_Daywise");
            }

            ////step.When("I am logged in as \"admin\"");
            step.LoggedInAsUser("admin");
            ////step.When("I navigate to \"FileUpload\" page");
            step.UiNavigation("FileUpload");
            ////step.When("I click on \"FileUpload\" \"button\"");
            step.IClickOn("FileUpload", "button");
            ////step.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            step.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////step.When("I select \"Insert\" from FileUpload dropdown");
            step.ISelectFileFromFileUploadDropdown("Insert");
            ////step.When("I click on \"Browse\" to upload");
            step.IClickOnUploadButton("Browse");

            if (days == "SingleDay")
            {
                ////step.When("I select \"SingleDay\" file from directory");
                await step.ISelectFileFromDirectoryAsync("SingleDay").ConfigureAwait(false);
            }
            else if (days == "Testdata_20215")
            {
                ////step.When("I select \"Testdata_20215\" file from directory");
                await step.ISelectFileFromDirectoryAsync("Testdata_20215").ConfigureAwait(false);
            }
            else if (days == "Testdata_20215_0Volume")
            {
                ////step.When("I select \"Testdata_20215_0Volume\" file from directory");
                await step.ISelectFileFromDirectoryAsync("Testdata_20215_0Volume").ConfigureAwait(false);
            }
            else
            {
                ////step.When("I select \"TestDataCutOff_Daywise\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestDataCutOff_Daywise").ConfigureAwait(false);
            }

            ////step.When("I click on \"FileUpload\" \"Save\" \"button\"");
            step.IClickOn("uploadFile\" \"Submit", "button");
            ////step.When("I wait till file upload to complete");
            await step.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public static async Task CreateHomologationAsync(this StepDefinitionBase step, string nodeConnection, string tankHomologation = "Yes", EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            try
            {
                await step.ReadAllSqlAsync(SqlQueries.DeleteDataInHomologationObjectAndDataMapping, args: new { sourceSytem = 2, destinationSystem = 1 }).ConfigureAwait(false);
                var homologationRow = await step.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationId]).ConfigureAwait(false);
                step.ScenarioContext[ConstantValues.HomologationId] = homologationRow[ConstantValues.HomologationId];
                var homologationGroup = await step.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                foreach (var homologationGroupRow in homologationGroup)
                {
                    var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                    step.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                    await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = step.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                }

                await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }

            var nodeConnectionRequest = ApiContent.Creates[ConstantValues.ManageConnection];
            nodeConnectionRequest = await ecpApiStep.CreateSourceAndDestinationNodesAsync(nodeConnectionRequest).ConfigureAwait(false);
            nodeConnectionRequest = await ecpApiStep.CreateOwnersAsync(nodeConnectionRequest).ConfigureAwait(false);
            await ecpApiStep.SetResultAsync(() => nodeConnection.EqualsIgnoreCase("Yes") ? ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegment(ApiContent.Routes[ConstantValues.ManageConnection]), JObject.Parse(nodeConnectionRequest)) : null).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.SourceNode] = nodeConnectionRequest.JsonGetValue(ConstantValues.SourceNode);
            step.ScenarioContext[ConstantValues.DestinationNode] = nodeConnectionRequest.JsonGetValue(ConstantValues.DestinationNode);
            step.ScenarioContext["Count"] = 0;
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext["NodeId_3"] = step.ScenarioContext["NodeId"].ToString();
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext["NodeId_4"] = step.ScenarioContext["NodeId"].ToString();
            await ecpApiStep.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            step.ScenarioContext["MovementTypeId"] = step.ScenarioContext["CategoryElement"];
            await ecpApiStep.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            step.ScenarioContext["ProductTypeId"] = step.ScenarioContext["CategoryElement"];
            await ecpApiStep.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NameOfSourceProductTypeId] = step.ScenarioContext["CategoryElement"];
            await ecpApiStep.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NameOfDestinationProductTypeId] = step.ScenarioContext["CategoryElement"];
            await ecpApiStep.CreateCatergoryElementAsync("7").ConfigureAwait(false);
            step.ScenarioContext["Owner"] = step.ScenarioContext["CategoryElement"];
            ////var nodeStorageLocationRow = await step.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow[ConstantValues.NodeStorageLocationByNodeId], args: new { nodeId = step.ScenarioContext[ConstantValues.SourceNode] }).ConfigureAwait(false);
            ////step.ScenarioContext["NodeStorageLocationId"] = nodeStorageLocationRow["NodeStorageLocationId"];
            ////var storageLocationProductRow = await step.ReadSqlAsStringDictionaryAsync(input: ApiContent.GetRow["StorageLocationProductByNodeStorageLocationId"], args: new { nodeStorageLocationId = step.ScenarioContext["NodeStorageLocationId"] }).ConfigureAwait(false);
            ////step.ScenarioContext[ConstantValues.StorageLocationProductId] = storageLocationProductRow[ConstantValues.StorageLocationProductId];
            ////Homologation between 2 and 1
            var setupHomologationRequestForNodes = tankHomologation.EqualsIgnoreCase("Yes") ? ApiContent.Creates[ConstantValues.HomologationForNodesWithTank] : ApiContent.Creates[ConstantValues.HomologationForNodes];
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(step.ScenarioContext[ConstantValues.SourceNode].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(step.ScenarioContext[ConstantValues.DestinationNode].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(step.ScenarioContext["NodeId_3"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForNodes = setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(step.ScenarioContext["NodeId_4"].ToString(), CultureInfo.InvariantCulture));
            await ecpApiStep.CreateNodeAsync().ConfigureAwait(false);
            step.ScenarioContext["TankName1"] = await ecpApiStep.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false);
            setupHomologationRequestForNodes = tankHomologation.EqualsIgnoreCase("Yes") ? setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node5 destinationValue", step.ScenarioContext["TankName1"].ToString()) : setupHomologationRequestForNodes;
            await ecpApiStep.CreateNodeAsync().ConfigureAwait(false);
            step.ScenarioContext["TankName2"] = await ecpApiStep.GetLastCreatedRowIdAsync(ConstantValues.Nodes).ConfigureAwait(false);
            setupHomologationRequestForNodes = tankHomologation.EqualsIgnoreCase("Yes") ? setupHomologationRequestForNodes.JsonChangePropertyValue("HomologationDataMapping_Node6 destinationValue", step.ScenarioContext["TankName2"].ToString()) : setupHomologationRequestForNodes;
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForNodes)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for Products
            var setupHomologationRequestForProducts = ApiContent.Creates[ConstantValues.HomologationForProduct];
            setupHomologationRequestForProducts = setupHomologationRequestForProducts.JsonChangePropertyValue("sourceSystemId", 2);
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProducts)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for Unit
            var setupHomologationRequestForUnit = ApiContent.Creates[ConstantValues.HomologationForUnit];
            setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("sourceSystemId", 2);
            if (ecpApiStep.GetValueInternal(ConstantValues.MeasurementUnit) == ConstantValues.Empty)
            {
                setupHomologationRequestForUnit = setupHomologationRequestForUnit.JsonChangePropertyValue("HomologationDataMapping destinationValue", 0);
            }

            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForUnit)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for MovementTypeId
            var setupHomologationRequestForMovementType = ApiContent.Creates[ConstantValues.HomologationForMovementTypeId];
            setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForMovementType = setupHomologationRequestForMovementType.JsonChangePropertyValue("HomologationDataMapping_MovementTypeId destinationValue", int.Parse(step.ScenarioContext["MovementTypeId"].ToString(), CultureInfo.InvariantCulture));
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForMovementType)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for ProductTypeId
            var setupHomologationRequestForProductType = ApiContent.Creates[ConstantValues.HomologationForProductTypeId];
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_ProductTypeId destinationValue", int.Parse(step.ScenarioContext["ProductTypeId"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_SourceProductTypeId destinationValue", int.Parse(step.ScenarioContext["SourceProductTypeId"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequestForProductType = setupHomologationRequestForProductType.JsonChangePropertyValue("HomologationDataMapping_DestinationProductTypeId destinationValue", int.Parse(step.ScenarioContext["DestinationProductTypeId"].ToString(), CultureInfo.InvariantCulture));
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForProductType)).ConfigureAwait(false)).ConfigureAwait(false);
            ////Create Homologation between 2 to 1 for OwnerId
            var setupHomologationRequestForOwner = ApiContent.Creates[ConstantValues.HomologationForOwner];
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("sourceSystemId", 2);
            setupHomologationRequestForOwner = setupHomologationRequestForOwner.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", int.Parse(step.ScenarioContext["Owner"].ToString(), CultureInfo.InvariantCulture));
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequestForOwner)).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static async Task IWantToRegisterAnExcelHomologationInTheSystemAsync(this StepDefinitionBase step, string entity, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            Assert.IsNotNull(entity);
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            step.ScenarioContext["Count"] = "0";

            // Create Nodes
            await ecpApiStep.CreateNodesForTestDataAsync().ConfigureAwait(false);

            // create elements
            await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionsForTestDataAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);
            step.IUpdateTheExcelFile("ValidExcel");
        }

        public static async Task IHaveFicoConnectionSetupIntoTheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var fetchtokenContent = "{ }";
            fetchtokenContent = fetchtokenContent.JsonAddField(ConstantValues.Clientid, Settings.GetValue("Clientid"));
            fetchtokenContent = fetchtokenContent.JsonAddField(ConstantValues.Secret, Settings.GetValue("Secret"));
            await ecpApiStep.SetResultAsync(async () => await FicoRuleshelper.PosttoFetchTokenAsync(Settings.GetValue("GrantFicoTokenEndpoint"), fetchtokenContent).ConfigureAwait(false)).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Tokenid] = step.ScenarioContext[Entities.Keys.Result];
        }

        public static async Task NodeOperativeOwnershipExAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await ecpApiStep.NodeOperativeOwnershipAsync().ConfigureAwait(false);
        }

        public static async Task NodeWithSegmentCategoryAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.given("i am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            ////create nodes
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext["NodeId_1"] = step.ScenarioContext["NodeId"].ToString();
            var nodename = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetLastNodeName, args: new { nodeid = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            step.ScenarioContext["NodeName"] = nodename["Name"].ToString();
            step.ScenarioContext["CategorySegment"] = step.ScenarioContext["SegmentName"].ToString();
        }

        public static async Task DataSetupForInactiveNetworkConfigurationAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId1] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            step.ScenarioContext[ConstantValues.CategorySegment] = step.ScenarioContext["SegmentName"].ToString();
            step.ScenarioContext["OperatorName"] = step.ScenarioContext["OperatorName"].ToString();
            var name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId1].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node1Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId2] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId2].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node2Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateInActiveNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId3] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId3].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node3Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateInActiveNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId4] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId4].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node4Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateCatergoryElementAsync("8").ConfigureAwait(false);
            var categoryElementRow = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = step.ScenarioContext["CategoryElement"] }).ConfigureAwait(false);
            step.ScenarioContext["SegmentName"] = categoryElementRow[ConstantValues.Name];
        }

        public static async Task UserCalculatedOwnershipForSegmentwithContractsAndTicketGeneratedAsync(this StepDefinitionBase step, string contractsCondition, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            if (contractsCondition == "with")
            {
                ////step.Given("I have \"ownershipnodes\" created");
                await step.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
                step.ScenarioContext[ConstantValues.OwnershipCalculatedForSegemnt] = true;
                ////step.Given("I have \"Contract\" information in the system");
                await ecpApiStep.IHaveInformationInTheSystemAsync("Contract").ConfigureAwait(false);
                ////step.Given("Ownership is Calculated for Segment and Ticket is Generated");
                await ecpApiStep.CalculatedOwnershipForSegmentAndTicketGeneratedAsync().ConfigureAwait(false);
            }
            else
            {
                ////step.Given("I have \"ownershipnodes\" created");
                await step.IHaveOwnershipNodesCreatedAsync("ownershipnodes").ConfigureAwait(false);
                ////step.Given("Ownership is Calculated for Segment and Ticket is Generated");
                await ecpApiStep.CalculatedOwnershipForSegmentAndTicketGeneratedAsync().ConfigureAwait(false);
            }
        }

        public static async Task DataSetupForNetworkConfigurationAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId1] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            step.ScenarioContext[ConstantValues.CategorySegment] = step.ScenarioContext["SegmentName"].ToString();
            step.ScenarioContext["OperatorName"] = step.ScenarioContext["OperatorName"].ToString();
            var name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId1].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node1Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId2] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId2].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node2Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId3] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId3].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node3Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateInActiveNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.NodeId4] = step.ScenarioContext[ConstantValues.NodeId].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext[ConstantValues.NodeId4].ToString() }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.Node4Name] = name[ConstantValues.Name];
            await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext[ConstantValues.NodeId2].ToString(), step.ScenarioContext[ConstantValues.NodeId1].ToString(), 2, "0.07", "0.06").ConfigureAwait(false);
            await ecpApiStep.CreateInActiveNodeConnectionAsync(step.ScenarioContext[ConstantValues.NodeId1].ToString(), step.ScenarioContext[ConstantValues.NodeId3].ToString(), 2, ConstantValues.InactiveConnection, "0.05", "0.04").ConfigureAwait(false);
            await ecpApiStep.CreateInActiveNodeConnectionAsync(step.ScenarioContext[ConstantValues.NodeId3].ToString(), step.ScenarioContext[ConstantValues.NodeId2].ToString(), 1, ConstantValues.WithoutTransferpoint, "0.02", "0.06", "10000002372").ConfigureAwait(false);
            await ecpApiStep.CreateCatergoryElementAsync("8").ConfigureAwait(false);
            var categoryElementRow = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = step.ScenarioContext["CategoryElement"] }).ConfigureAwait(false);
            step.ScenarioContext["SegmentName"] = categoryElementRow[ConstantValues.Name];
        }

        public static async Task ResponseShouldBeSuccessfulAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            await Task.Delay(10000).ConfigureAwait(true);
            Assert.IsNotNull(step.ScenarioContext[Entities.Keys.Results]);
        }

        public static async Task ShouldBeRegisteredAsync(this StepDefinitionBase step)
        {
            step.ScenarioContext[ConstantValues.EventType] = step.ScenarioContext[ConstantValues.EventType].ToString() == null ? "Insert" : step.ScenarioContext[ConstantValues.EventType];
            var lastCreatedRow = await step.ReadSqlAsStringDictionaryAsync(input: ApiContent.LastCreated[step.ScenarioContext[Entities.Keys.EntityType].ToString()], args: new { eventType = step.ScenarioContext[ConstantValues.EventType] }).ConfigureAwait(false);
            step.ScenarioContext[ConstantValues.LogType] = lastCreatedRow["EventType"];
            var id = step.ScenarioContext[Entities.Keys.EntityType].ToString().EqualsIgnoreCase("Inventory") ? "InventoryId" : "MovementId";
            var idValue = step.ScenarioContext[Entities.Keys.EntityType].ToString().EqualsIgnoreCase("Inventory") ? step.ScenarioContext["DATE"] : step.ScenarioContext["MOVEMENTID"];
            Assert.AreEqual(idValue.ToString().Replace("-", string.Empty), lastCreatedRow[id]);
            Assert.AreEqual(step.ScenarioContext[ConstantValues.EventType].ToString().ToUpperInvariant(), step.ScenarioContext[ConstantValues.LogType].ToString().ToUpperInvariant());
        }

        public static void IShouldSeeInterface(this StepDefinitionBase step, string title)
        {
            step.ScenarioContext[Entities.Keys.Title] = title;
            Assert.IsTrue(UIContent.Conversion[title].EqualsIgnoreCase(step.Get<ElementPage>().GetElement(nameof(Resources.CategoryHeader)).Text));
            step.ScenarioContext[Entities.Keys.RandomFieldValue] = string.Concat($"Automation_", new Faker().Random.AlphaNumeric(5));
        }

        public static async Task ShouldBeAbleToSelectNodesBelongToThatSegmentAsync(this StepDefinitionBase step)
        {
            IDictionary<string, string> nodeDetails = null;
            if (string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.Node)))
            {
                nodeDetails = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetSAPDetailsOfANode, args: new { segment = step.ScenarioContext[ConstantValues.Segment].ToString() }).ConfigureAwait(false);
                step.ScenarioContext[ConstantValues.NodeWithSendToSAPTrue] = nodeDetails[ConstantValues.SendToSAP];
                step.ScenarioContext[ConstantValues.Node] = nodeDetails[ConstantValues.Name];
            }

            ////step.When("I click on Node textbox on criteria step");
            step.Get<ElementPage>().Click(nameof(Resources.NodeFieldOnCreateLogisticsInterface));
            var page = step.Get<ElementPage>();
            var nodeSelection = page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface));
            nodeSelection.SendKeys(step.ScenarioContext[ConstantValues.Node].ToString());
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            page.SelectItemAtIndex(nodeSelection, 1);
            Assert.AreEqual(step.ScenarioContext[ConstantValues.Node].ToString(), page.GetElement(nameof(Resources.NodeInputOnCreateLogisticsInterface)).GetAttribute(ConstantValues.Value));
        }

        public static async Task DataSetupForOperationalTransformationAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

            // Create Nodes
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext["NodeId_1"] = step.ScenarioContext["NodeId"].ToString();
            step.ScenarioContext["CategorySegment"] = step.ScenarioContext["SegmentName"].ToString();
            var name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            step.ScenarioContext["Origin_NodeName"] = name[ConstantValues.Name];
            step.ScenarioContext["Destination_DestinationName"] = name[ConstantValues.Name];
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext["NodeId_2"] = step.ScenarioContext["NodeId"].ToString();
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            step.ScenarioContext["NodeId_3"] = step.ScenarioContext["NodeId"].ToString();
            await ecpApiStep.CreateNodesStepsAsync().ConfigureAwait(false);
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_3"].ToString() }).ConfigureAwait(false);
            step.ScenarioContext["Origin_DestinationNodeName"] = name[ConstantValues.Name];
            step.ScenarioContext["NodeId_4"] = step.ScenarioContext["NodeId"].ToString();
            name = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetNode, args: new { nodeId = step.ScenarioContext["NodeId_4"].ToString() }).ConfigureAwait(false);
            step.ScenarioContext["Destination_NodeName"] = name[ConstantValues.Name];

            // Create Categorty Elements
            await ecpApiStep.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            step.ScenarioContext["MovementTypeId"] = step.ScenarioContext["CategoryElement"].ToString();
            await ecpApiStep.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            step.ScenarioContext["ProductTypeId"] = step.ScenarioContext["CategoryElement"].ToString();
            await ecpApiStep.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            step.ScenarioContext["SourceProductTypeId"] = step.ScenarioContext["CategoryElement"].ToString();
            await ecpApiStep.CreateCatergoryElementAsync("11").ConfigureAwait(false);
            step.ScenarioContext["DestinationProductTypeId"] = step.ScenarioContext["CategoryElement"].ToString();
            await ecpApiStep.CreateCatergoryElementAsync("7").ConfigureAwait(false);
            step.ScenarioContext["Owner"] = step.ScenarioContext["CategoryElement"].ToString();

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_2"].ToString(), step.ScenarioContext["NodeId_1"].ToString(), 2, "0.07", "0.06").ConfigureAwait(false);
            await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_1"].ToString(), step.ScenarioContext["NodeId_4"].ToString(), 2, "0.05", "0.04").ConfigureAwait(false);
            await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext["NodeId_3"].ToString(), step.ScenarioContext["NodeId_1"].ToString(), 1, "0.02", "0.06", "10000002372").ConfigureAwait(false);

            // Delete Homologation between 3 to 1
            try
            {
                var numberOfHomologations = await step.ReadAllSqlAsync(input: ApiContent.GetRow[ConstantValues.HomologationIdForXL]).ConfigureAwait(false);
                var homologationRow = numberOfHomologations.ToDictionaryList();
                for (int i = 0; i < homologationRow.Count; i++)
                {
                    step.ScenarioContext[ConstantValues.HomologationId] = homologationRow[i][ConstantValues.HomologationId];
                    var homologationGroup = await step.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow[ConstantValues.HomologationGroupId], args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                    foreach (var homologationGroupRow in homologationGroup)
                    {
                        var homologationJson = homologationGroupRow[ConstantValues.HomologationGroupId];
                        step.ScenarioContext[ConstantValues.HomologationGroupId] = homologationJson;
                        await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationObjectAndDataMapping, args: new { homologationGroupId = step.ScenarioContext[ConstantValues.HomologationGroupId] }).ConfigureAwait(false);
                    }

                    await step.ReadAllSqlAsync(input: SqlQueries.DeleteHomologationAndGroup, args: new { homologationId = step.ScenarioContext[ConstantValues.HomologationId] }).ConfigureAwait(false);
                }
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }

            // Create Homologation between 3 to 1
            var setupHomologationRequest = ApiContent.Creates[ConstantValues.SetupHomologationForCutoff];
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_Node1 destinationValue", int.Parse(step.ScenarioContext["NodeId_1"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_Node2 destinationValue", int.Parse(step.ScenarioContext["NodeId_2"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_Node3 destinationValue", int.Parse(step.ScenarioContext["NodeId_3"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_Node4 destinationValue", int.Parse(step.ScenarioContext["NodeId_4"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_MovementTypeId destinationValue", int.Parse(step.ScenarioContext["MovementTypeId"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_ProductTypeId destinationValue", int.Parse(step.ScenarioContext["ProductTypeId"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_SourceProductTypeId destinationValue", int.Parse(step.ScenarioContext["SourceProductTypeId"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_DestinationProductTypeId destinationValue", int.Parse(step.ScenarioContext["DestinationProductTypeId"].ToString(), CultureInfo.InvariantCulture));
            setupHomologationRequest = setupHomologationRequest.JsonChangePropertyValue("HomologationDataMapping_Owner destinationValue", int.Parse(step.ScenarioContext["Owner"].ToString(), CultureInfo.InvariantCulture));
            await ecpApiStep.SetResultAsync(async () => await ecpApiStep.PostAsync<dynamic>(ecpApiStep.Endpoint.AppendPathSegments(ApiContent.Routes[ConstantValues.Homologation]), JObject.Parse(setupHomologationRequest)).ConfigureAwait(false)).ConfigureAwait(false);

            var elementRow = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetElementId, args: new { categoryName = step.ScenarioContext["CategorySegment"].ToString() }).ConfigureAwait(false);
            var elementId = elementRow["ElementId"];
            var otherElementId = await step.ReadAllSqlAsDictionaryAsync(SqlQueries.GetOtherElementIds, args: new { elementId, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            List<dynamic> nodeTagIds = otherElementId.SelectMany(d => d.Values).ToList();
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateNodeTagDate, args: new { date = 4, nodeId = step.ScenarioContext["NodeId_1"].ToString() }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId1, args: new { nodeTagId = nodeTagIds[0] }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.UpdateElementId2, args: new { nodeTagId = nodeTagIds[9] }).ConfigureAwait(false);

            ////step.When("I update the excel with \"OperationalTransformation\" new data");
            step.IUpdateTheExcelWithNewData("OperationalTransformation");
        }

        public static async Task SystemCategoryIntheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await ecpApiStep.CreateCatergoryElementAsync("8").ConfigureAwait(false);
            var categoryElementRow = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = step.ScenarioContext["CategoryElement"] }).ConfigureAwait(false);
            step.ScenarioContext["CategorySegment"] = categoryElementRow[ConstantValues.Name];
        }

        public static async Task ActiveMovementTypeAndItsAnnulationTypeAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await ecpApiStep.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            step.SetValueInternal("MovementTypeId", step.ScenarioContext["CategoryElement"]);
            step.SetValueInternal("MovementTypeName", step.ScenarioContext["CategoryElementName"]);
            await ecpApiStep.CreateCatergoryElementAsync("9").ConfigureAwait(false);
            step.SetValueInternal("AnnulationMovementTypeId", step.ScenarioContext["CategoryElement"]);
            step.SetValueInternal("AnnulationMovementName", step.ScenarioContext["CategoryElementName"]);
        }

        public static async Task SegementCategoryIntheSystemAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);
            await ecpApiStep.CreateCatergoryElementAsync("2").ConfigureAwait(false);
            var categoryElementRow = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetCategoryElementNameByCategoryElementId, args: new { elementId = step.ScenarioContext["CategoryElement"] }).ConfigureAwait(false);
            step.ScenarioContext["CategorySegment"] = categoryElementRow[ConstantValues.Name];
        }

        public static async Task CalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedExAsync(this StepDefinitionBase step, string condition, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await ecpApiStep.GivenIHaveCalculatedOwnershipForSegmentwithEventsofSameSegmentAndTicketGeneratedAsync(condition).ConfigureAwait(false);
        }

        public static async Task GraphicalNodeConnectionAsync(this StepDefinitionBase step, string state, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            if (state == ConstantValues.Active)
            {
                await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext[ConstantValues.NodeId2].ToString(), step.ScenarioContext[ConstantValues.NodeId1].ToString(), 2, "0.07", "0.06").ConfigureAwait(false);
            }
            else
            {
                await ecpApiStep.CreateNodeConnectionAsync(step.ScenarioContext[ConstantValues.NodeId4].ToString(), step.ScenarioContext[ConstantValues.NodeId3].ToString(), 2, "0.07", "0.06").ConfigureAwait(false);
            }
        }

        public static async Task CalculatedOwnershipForSegmentAndTicketGeneratedAsync(this StepDefinitionBase step, string eventsCondition, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await ecpApiStep.IHaveCalculatedOwnershipForSegmentAndTicketGeneratedAsync(eventsCondition).ConfigureAwait(false);
        }

        public static void IClickOnTab(this StepDefinitionBase step, string field)
        {
            step.ThrowIfNull(nameof(step));
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            step.Get<ElementPage>().Click(nameof(Resources.SelectBoxOptionByValue), formatArgs: UIContent.Conversion[field]);
        }

        public static void SelectValueFromDropDown(this StepDefinitionBase step, string value, string field)
        {
            step.ThrowIfNull(nameof(step));
            step.ScenarioContext[Entities.Keys.SelectedValue] = value;
            var dropDownIndex = int.Parse(step.ScenarioContext["DropDownIndex"].ToString(), CultureInfo.InvariantCulture);
            var dropDownBox = step.Get<ElementPage>().GetElements(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            dropDownBox[dropDownIndex - 1].Click();
            //// step.Get<ElementPage>().Click(nameof(Resources.SelectBox), formatArgs: UIContent.Conversion[field]);
            step.Get<ElementPage>().WaitUntilElementToBeClickable(nameof(Resources.SelectBoxMenu), 5, formatArgs: UIContent.Conversion[field]);
            var option = step.Get<ElementPage>().GetElement(nameof(Resources.SelectBoxOption), formatArgs: value);
            Actions action = new Actions(step.DriverContext.Driver);
            action.MoveToElement(option).Perform();
            option.Click();
        }

        public static void IVerifyValidationsPassed(this StepDefinitionBase step)
        {
            Assert.IsTrue(step.Get<ElementPage>().GetElement(nameof(Resources.OwnerShipNextButton)).Enabled);
        }

        public static async Task ISelectTheStartDateLessthanDaysFromCurrentDateOnDatePickerAsync(this StepDefinitionBase step, int days, string type)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            await Task.Delay(5000).ConfigureAwait(true);
            if (type == "Cutoff")
            {
                if (string.IsNullOrEmpty(step.GetValueInternal("InitialInventoryDateNodeGroup")))
                {
                    step.ScenarioContext["InitialInventoryDateNodeGroup"] = DateTime.Now.AddDays(-(days + 1)).ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToString();
                }

                step.ScenarioContext["StartDate"] = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                step.ScenarioContext["InitialDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString();
                step.ScenarioContext["FromDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToString();
                page.WaitUntilElementToBeClickable(nameof(Resources.StartDatePickerinSegment));
                page.Click(nameof(Resources.StartDatePickerinSegment));
                page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext["StartDate"].ToString());
                page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Control + 'a');
                page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Backspace);
                page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(step.ScenarioContext["StartDate"].ToString());
                page.GetElement(nameof(Resources.StartDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Tab);
            }
        }

        public static async Task ISelectTheEndDateLessthanDaysFromCurrentDateOnDatePickerAsync(this StepDefinitionBase step, int days, string type)
        {
            step.ThrowIfNull(nameof(step));
            var page = step.Get<ElementPage>();
            await Task.Delay(5000).ConfigureAwait(true);
            if (type == "Cutoff")
            {
                if (string.IsNullOrEmpty(step.GetValueInternal("InitialInventoryDate")))
                {
                    step.ScenarioContext["InitialInventoryDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToString();
                }

                step.ScenarioContext["EndDate"] = DateTime.Now.AddDays(-days).ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToString();
                step.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                page.WaitUntilElementToBeClickable(nameof(Resources.FinalDatePickerinSegment));
                page.Click(nameof(Resources.FinalDatePickerinSegment));
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(step.ScenarioContext["FinalDate"].ToString());
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Control + 'a');
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Backspace);
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(step.ScenarioContext["FinalDate"].ToString());
                page.GetElement(nameof(Resources.FinalDatePickerinSegment)).SendKeys(OpenQA.Selenium.Keys.Tab);
            }

            if (type == "Ownership")
            {
                step.ScenarioContext["FinalDate"] = DateTime.Now.AddDays(-days).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                var dayOfFinalDate = step.ScenarioContext["FinalDate"].ToString().Split('/')[1].TrimStart('0');
                page.WaitUntilElementToBeClickable(nameof(Resources.OwnershipFinalDate));
                page.Click(nameof(Resources.OwnershipFinalDate));
                page.Click(nameof(Resources.UploadType), dayOfFinalDate, page.GetElements(nameof(Resources.UploadTypeName), dayOfFinalDate).Count);
            }
        }

        public static void ISeeMessageFor(this StepDefinitionBase step, string message, string messageHeader)
        {
            Assert.AreEqual(step.Get<ElementPage>().GetElement(nameof(Resources.GetValidationMessage), messageHeader).Text, message);
        }

        public static async Task ShouldBeRegisteredInTheSystemAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));
            ////step.When("I wait till file upload to complete");
            await step.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
            await Task.Delay(MillisecondsDelay).ConfigureAwait(true);
            var inventoryRow = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetInventoryBySegmentId, args: new { segmentId = step.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            step.ScenarioContext["UniqueInventoryId"] = inventoryRow["InventoryId"];
            var movementRow = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementBySegmentId, args: new { segmentId = step.ScenarioContext["SegmentId"] }).ConfigureAwait(false);
            step.ScenarioContext["UniqueMovementId"] = movementRow["MovementId"];
            var movemenRowSource = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementSource, args: new { movementTransactionId = movementRow["MovementTransactionId"] }).ConfigureAwait(false);
            var movementRowDestination = await step.ReadSqlAsStringDictionaryAsync(input: SqlQueries.GetMovementDestination, args: new { movementTransactionId = movementRow["MovementTransactionId"] }).ConfigureAwait(false);

            if (step.ScenarioContext["ActionType"].ToString() == "Insert")
            {
                Assert.AreEqual(step.ScenarioContext["NodeId_1"].ToString(), inventoryRow["NodeId"]);
                Assert.AreEqual(step.ScenarioContext["ActionType"].ToString(), inventoryRow["EventType"]);
                Assert.AreEqual(step.ScenarioContext["ActionType"].ToString(), movementRow["EventType"]);
                Assert.AreEqual(step.ScenarioContext["NodeId_2"].ToString(), movemenRowSource["SourceNodeId"]);
                Assert.AreEqual(step.ScenarioContext["NodeId_1"].ToString(), movementRowDestination["DestinationNodeId"]);
            }
            else if (step.ScenarioContext["ActionType"].ToString() == "Update")
            {
                var inventoryProductRow = await step.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetInventoryProduct, args: new { InventoryPrimaryKeyId = inventoryRow["InventoryTransactionId"] }).ConfigureAwait(false);
                Assert.AreEqual(ConstantValues.UpdatedProductCount, inventoryProductRow.ToDictionaryList().Count);
                Assert.AreEqual(step.ScenarioContext["ActionType"].ToString(), movementRow["EventType"]);
            }
            else if (step.ScenarioContext["ActionType"].ToString() == "Delete")
            {
                Assert.AreEqual(step.ScenarioContext["ActionType"].ToString(), movementRow["EventType"]);
                var inventoryProductRow = await step.ReadAllSqlAsDictionaryAsync(input: SqlQueries.GetInventoryProduct, args: new { InventoryPrimaryKeyId = inventoryRow["InventoryTransactionId"] }).ConfigureAwait(false);
                Assert.AreEqual(ConstantValues.DeletedProductCount, inventoryProductRow.ToDictionaryList().Count);
            }
        }

        public static void IShouldSeeTheMessageRequired(this StepDefinitionBase step, string expectedMessage)
        {
            var errorMessage = step.Get<ElementPage>().GetElements(nameof(Resources.GetErrorMessageFromModel));
            foreach (var actualMessage in errorMessage)
            {
                Assert.AreEqual(expectedMessage, actualMessage.Text);
            }
        }

        public static void IShouldSeeMessage(this StepDefinitionBase step, string message)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            step.Get<ElementPage>().WaitUntilElementIsVisible(nameof(Resources.NoRecordsFoundMessage));
            Assert.IsTrue(step.Get<ElementPage>().GetElement(nameof(Resources.NoRecordsFoundMessage)).Text.ContainsIgnoreCase(message));
        }

        public static void IShouldSeeTheOperationalCutoffPage(this StepDefinitionBase step, string page)
        {
            step.Get<ElementPage>().WaitUntilInvisibilityOfElementLocated(nameof(Resources.Loader));
            Assert.AreEqual(UIContent.Conversion[page], step.Get<ElementPage>().GetElement(nameof(Resources.OperationalCutoffPage)).Text);
        }

        public static async Task IInvokeTheFICOServiceToFetchTheStrategiesWithInputParameterTipoLlamadaAndEstadoAsync(this StepDefinitionBase step, string firstValue, string secondvalue, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            var content = ApiContent.Creates[ConstantValues.FicoConnection];
            content = content.JsonChangePropertyValue("FicoRules tipoLlamada", firstValue);
            content = content.JsonChangePropertyValue("FicoRules estrategiasActivas", secondvalue);
            await ecpApiStep.SetResultAsync(async () => await FicoRuleshelper.PostWithJwtAsync(Settings.GetValue("FetchFicoStrategies"), content, step.ScenarioContext[ConstantValues.Tokenid].ToString()).ConfigureAwait(false)).ConfigureAwait(false);
            step.ScenarioContext["Result"] = step.ScenarioContext[Entities.Keys.Result];
        }

        public static void IShouldSeeBreadcrumb(this StepDefinitionBase step, string text)
        {
            step.ThrowIfNull(nameof(step));
            string brdcrm = step.DriverContext.Driver.FindElements(By.ClassName("ep-bcrumbs__lst-lnk"))[1].Text;
            Assert.AreEqual(brdcrm, text);
        }

        public static async Task ItMeetsAllInputValidationsAsync(this StepDefinitionBase step, string value)
        {
            if (value == "all")
            {
                await step.IDontReceiveInXMLAsync(step.ScenarioContext[Entities.Keys.EntityType].ToString()).ConfigureAwait(false);
            }
            else
            {
                BlobExtensions.UpdateXmlData(step.ScenarioContext[Entities.Keys.EntityType].ToString() + "\\" + step.ScenarioContext[Entities.Keys.EntityType].ToString(), step.ScenarioContext[Entities.Keys.EntityType].ToString() == "Inventory" ? "Inventory UOM" : "Movement UOM", string.Empty);
                await step.IDontReceiveInXMLAsync(step.ScenarioContext[Entities.Keys.EntityType].ToString()).ConfigureAwait(false);
            }
        }

        public static async Task IDontReceiveInXMLAsync(this StepDefinitionBase step, string field, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            if (!field.EqualsIgnoreCase("MovementId"))
            {
                ecpApiStep.UpdateXmlDefaultValue(field);
            }

            await ecpApiStep.PerformHomologationAsync(field).ConfigureAwait(false);
        }

        public static void ComboboxShouldBeDisabled(this StepDefinitionBase step, string field, string grid, string expectedValue)
        {
            string isDisabled = step.Get<ElementPage>().GetElement(nameof(Resources.ComboboxDisabled), grid.ToCamelCase(), field.ToCamelCase()).GetAttribute("disabled");
            if (isDisabled == null)
            {
                Assert.AreEqual("enabled", expectedValue);
            }
            else
            {
                Assert.AreEqual("disabled", expectedValue);
            }
        }

        public static async Task IRegisterInventoriesOrMovementsInSystemThroughSappoAsync(this StepDefinitionBase step, string entity, string invalidToken = null, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            if (string.IsNullOrEmpty(invalidToken))
            {
                await ecpApiStep.GivenIAmAuthenticatedForServiceAsync("sappoapi").ConfigureAwait(false);
            }

            var contentJson = step.ScenarioContext[step.ScenarioContext[Entities.Keys.EntityType].ToString()];
            await ecpApiStep.SetResultsAsync(async () => await ecpApiStep.SapPostAsync<dynamic>(ecpApiStep.SapEndpoint.AppendPathSegment(ApiContent.Routes[entity]), JArray.Parse(contentJson.ToString())).ConfigureAwait(false)).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(step.GetValueInternal(Entities.Keys.Results)))
            {
                step.ScenarioContext[ConstantValues.MessageId] = step.ScenarioContext[Entities.Keys.Results];
            }
        }

        public static async Task ValidateThatResponseAreSuccessfullyLoadingIntoTheTableAtLevelForWhichIdIsAsync(this StepDefinitionBase step, string level, string tokenid)
        {
            var jsonresult = JObject.Parse(step.ScenarioContext["Result"].ToString());
            var keyLayer = jsonresult["volPayload"]["volOutput"].SelectToken(tokenid);
            if (level == ConstantValues.Node)
            {
                var tabledata = await step.ReadAllSqlAsDictionaryAsync(input: SqlQueries.NodeOwnershipRules).ConfigureAwait(false);
                var tableDataListinDb = tabledata.ToDictionaryList();

                for (int i = 0; i < keyLayer.Count(); i++)
                {
                    for (int j = 0; j < keyLayer[i].Count(); j++)
                    {
                        var data = keyLayer[i];
                        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                        Assert.AreEqual(values.ToList()[j].ToString(), tableDataListinDb[i].ToList()[j].ToString());
                    }
                }
            }
            else if (level == ConstantValues.ProductRules)
            {
                var tabledata = await step.ReadAllSqlAsDictionaryAsync(input: SqlQueries.ProductOwnershipRules).ConfigureAwait(false);
                var tableDataListinDb = tabledata.ToDictionaryList();

                for (int i = 0; i < keyLayer.Count(); i++)
                {
                    for (int j = 0; j < keyLayer[i].Count(); j++)
                    {
                        var data = keyLayer[i];
                        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                        Assert.AreEqual(values.ToList()[j].ToString(), tableDataListinDb[i].ToList()[j].ToString());
                    }
                }
            }
            else if (level == ConstantValues.ConnectionRules)
            {
                var tabledata = await step.ReadAllSqlAsDictionaryAsync(input: SqlQueries.ConnectionOwnershipRules).ConfigureAwait(false);
                var tableDataListinDb = tabledata.ToDictionaryList();

                for (int i = 0; i < keyLayer.Count(); i++)
                {
                    for (int j = 0; j < keyLayer[i].Count(); j++)
                    {
                        var data = keyLayer[i];
                        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                        Assert.AreEqual(values.ToList()[j].ToString(), tableDataListinDb[i].ToList()[j].ToString());
                    }
                }
            }
        }

        public static void ProvidedRequiredDetailsForPendingTransactionsGrid(this StepDefinitionBase step)
        {
            try
            {
                if (step.ScenarioContext[ConstantValues.PendingTransactions].ToInt() != 0)
                {
                    ////step.When("I click on \"ErrorsGrid\" \"AddNote\" \"button\"");
                    step.IClickOn("ErrorsGrid\" \"AddNote", "button");
                    ////step.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                    step.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                    ////step.When("I click on \"AddComment\" \"Submit\" \"button\"");
                    step.IClickOn("AddComment\" \"Submit", "button");
                }
            }
            catch (KeyNotFoundException)
            {
                Logger.Info("PendingTransactions key not present so no need to execute this method");
            }
        }

        public static void ProvidedRequiredDetailsForUnbalancesGrid(this StepDefinitionBase step)
        {
            try
            {
                if (step.ScenarioContext[ConstantValues.PendingTransactions].ToInt() != 0)
                {
                    ////step.When("I click on \"consistencyCheck\" \"AddNote\" \"button\"");
                    step.IClickOn("consistencyCheck\" \"AddNote", "button");
                    ////step.When("I enter valid value into \"AddComment\" \"Comment\" \"textbox\"");
                    step.IEnterValidValueInto("AddComment\" \"Comment", "textbox");
                    ////step.When("I click on \"AddComment\" \"submit\" \"button\"");
                    step.IClickOn("AddComment\" \"submit", "button");
                }
            }
            catch (KeyNotFoundException)
            {
                Logger.Info("PendingTransactions key not present so no need to execute this method");
            }
        }

        public static async Task TheTRUESystemIsProcessingTheOfficialMovementsAndInventoriesConsolidationAsync(this StepDefinitionBase step)
        {
            step.SetValueInternal(ConstantValues.TestDataForConsolidatedWithOfficial, "Yes");
            ////And I have ownership calculation data generated in the system
            await step.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);

            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = step.GetValueInternal("MovementTypeId"), AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId"), isActive = 1 }).ConfigureAwait(false);

            //// Approving all nodes related segment
            await step.ReadAllSqlAsync(SqlQueries.UpdateOwnershipNodeStatusBasedOnSegmentName, args: new { ownershipStatusId = 9, segment = step.GetValueInternal("SegmentName") }).ConfigureAwait(false);

            step.SetValueInternal("DeltaCategorySegment", step.GetValueInternal("SegmentName"));
            ////When I navigate to "Calculation of deltas by official adjust" page
            step.UiNavigation("Calculation of deltas by official adjustment");
            ////And I click on "newDeltasCalculation" "button"
            step.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            step.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");

            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForAnotherPeriodConsolidation)))
            {
                await step.GenerateAnotherPeriodOfficialDeltaTicketAsync().ConfigureAwait(false);
            }

            ////And I select "2020" year from drop down
            step.ISelectYearFromDropDown(step.GetValueInternal("YearForPeriod"));
            ////And I select "Jun" period from drop down
            step.ISelectAPeriodFromDropDown(step.GetValueInternal("PreviousMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "process" "button"
            step.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            step.IClickOn("validateOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            step.IClickOn("officialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await step.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        public static async Task TheTRUESystemIsProcessingTheOperativeMovementsAndInventoriesConsolidationAsync(this StepDefinitionBase step)
        {
            step.SetValueInternal(ConstantValues.TestdataForConsolidation, "Yes");
            ////And I have ownership calculation data generated in the system
            await step.IHaveOwnershipCalculationDataGeneratedInTheSystemAsync().ConfigureAwait(false);

            //// Failing process when ownership ticket status is failed
            Assert.IsFalse(step.GetValueInternal(ConstantValues.OwnershipTicketStatus).EqualsIgnoreCase("Fallido"));

            Assert.IsEmpty(await step.ReadAllSqlAsync(SqlQueries.ConsolidatedInventoryProductInformationWithSegmentName, args: new { segmentName = step.GetValueInternal("SegmentName") }).ConfigureAwait(false));
            Assert.IsEmpty(await step.ReadAllSqlAsync(SqlQueries.ConsolidatedMovementInformationWithSegmentName, args: new { segmentName = step.GetValueInternal("SegmentName") }).ConfigureAwait(false));

            //// Approving all nodes related segment
            await step.ReadAllSqlAsync(SqlQueries.UpdateOwnershipNodeStatusBasedOnSegmentName, args: new { ownershipStatusId = 9, segment = step.GetValueInternal("SegmentName") }).ConfigureAwait(false);

            //// Generation of official delta ticket
            await step.TheTRUESystemIsProcessingTheOperativeConsolidationAsync().ConfigureAwait(false);
        }

        public static async Task TheTRUESystemIsProcessingTheOperativeConsolidationAsync(this StepDefinitionBase step)
        {
            var officialDeltaDetails = await step.ReadAllSqlAsync(SqlQueries.GetProcessingOfficialDeltaTickets, args: new { segmentName = step.GetValueInternal("SegmentName") }).ConfigureAwait(false);
            var numberOfOfficialDeltaProcessingTickets = officialDeltaDetails.ToDictionaryList();
            if (numberOfOfficialDeltaProcessingTickets.Count > 0)
            {
                for (int i = 0; i < numberOfOfficialDeltaProcessingTickets.Count; i++)
                {
                    await step.ReadAllSqlAsync(SqlQueries.UpdateOfficialDeltaTicketsToFailed, args: new { ticketId = numberOfOfficialDeltaProcessingTickets[i]["TicketId"] }).ConfigureAwait(false);
                }
            }

            step.SetValueInternal("DeltaCategorySegment", step.GetValueInternal("SegmentName"));
            ////When I navigate to "Calculation of deltas by official adjust" page
            step.UiNavigation("Calculation of deltas by official adjustment");
            ////And I click on "newDeltasCalculation" "button"
            step.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            step.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");

            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForAnotherPeriodConsolidation)))
            {
                await step.GenerateAnotherPeriodOfficialDeltaTicketAsync().ConfigureAwait(false);
            }

            ////And I select "2020" year from drop down
            step.ISelectYearFromDropDown(step.GetValueInternal("YearForPeriod"));
            ////And I select "Jun" period from drop down
            step.ISelectAPeriodFromDropDown(step.GetValueInternal("PreviousMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "process" "button"
            step.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            step.IClickOn("validateOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            step.IClickOn("officialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await step.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        public static async Task GenerateAnotherPeriodOfficialDeltaTicketAsync(this StepDefinitionBase step)
        {
            step.ISelectYearFromDropDown(step.GetValueInternal("YearForAnotherPeriod"));
            ////And I select "Jun" period from drop down
            step.ISelectAPeriodFromDropDown(step.GetValueInternal("AnotherPeriodMonthName").ToPascalCase());
            ////And I click on "initOfficialDeltaTicket" "process" "button"
            step.IClickOn("initOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "validateOfficialDeltaTicket" "submit" "button"
            step.IClickOn("validateOfficialDeltaTicket\" \"submit", "button");
            ////And I click on "confirmOfficialDeltaTicket" "submit" "button"
            step.IClickOn("officialDeltaTicket\" \"submit", "button");
            //// wait till ticket processing complete
            await step.VerifyThatOfficialDeltaCalculationShouldBeSuccessfulAsync().ConfigureAwait(false);
            step.ScenarioContext["AnotherPeriodOfficialDeltaTicket"] = step.GetValueInternal("Official Delta TicketId");

            var officialDeltaDetails = await step.ReadAllSqlAsync(SqlQueries.GetProcessingOfficialDeltaTickets, args: new { segmentName = step.GetValueInternal("SegmentName") }).ConfigureAwait(false);
            var numberOfOfficialDeltaProcessingTickets = officialDeltaDetails.ToDictionaryList();
            if (numberOfOfficialDeltaProcessingTickets.Count > 0)
            {
                for (int i = 0; i < numberOfOfficialDeltaProcessingTickets.Count; i++)
                {
                    await step.ReadAllSqlAsync(SqlQueries.UpdateOfficialDeltaTicketsToFailed, args: new { ticketId = numberOfOfficialDeltaProcessingTickets[i]["TicketId"] }).ConfigureAwait(false);
                }
            }

            ////And I click on "newDeltasCalculation" "button"
            step.IClickOn("newDeltasCalculation", "button");
            ////And I select delta segment from "initOfficialDeltaTicket" "segment" "dropdown"
            step.ISelectOfficialDeltaSegmentFrom("initOfficialDeltaTicket\" \"segment", "dropdown");
        }

        public static async Task TestDataForOfficialBalanceLogisticsFileAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            step.ScenarioContext["Count"] = "0";
            ////step.Given("I am authenticated as \"admin\"");
            await ecpApiStep.GivenIAmAuthenticatedForUserAsync("admin").ConfigureAwait(false);

            // Create Nodes
            await ecpApiStep.CreateNodesForOfficialBalanceFileAsync().ConfigureAwait(false);

            // create elements
            await ecpApiStep.CreateCategoryElementsForTestDataAsync().ConfigureAwait(false);

            // Create Connections between created nodes
            await ecpApiStep.CreateNodeConnectionsForOfficialBalanceFileAsync().ConfigureAwait(false);

            // Homologation between 3 to 1
            await ecpApiStep.CreateHomologationForExcelAsync().ConfigureAwait(false);

            //// Generation of System Reports
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_1"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_2"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_3"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(ApiContent.InsertRow["TagSystemElementWithNode"], args: new { nodeId = step.ScenarioContext["NodeId_4"].ToString(), elementId = step.ScenarioContext["SystemElementId"], date = 4 }).ConfigureAwait(false);

            // Deleting Annulation for Movement type 42,43 and 44
            await step.ReadAllSqlAsync(SqlQueries.DeleteAnnulationForSystemGeneratedMovementTypes).ConfigureAwait(false);

            // Creating annulation movement
            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithoutCancellationMovements))
                || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithoutAnnulation)))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = step.GetValueInternal("MovementTypeId"), AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId"), isActive = 0 }).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements))
                || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions))
                || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = 44, AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeIdForUnIdentifiedLosses"), isActive = 1 }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = 43, AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeIdForTolerance"), isActive = 1 }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForDeltaCalculation, args: new { MovementTypeId = 42, AnnulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeIdForProductTransfer"), isActive = 1 }).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = 44, annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeIdForUnIdentifiedLosses"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = 43, annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeIdForTolerance"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
                await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = 42, annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeIdForProductTransfer"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);

                //// Creating annualtion movement for all inserted movement type movements
                await step.CreationOfAnnulationForAllTransformationScenariosAsync().ConfigureAwait(false);
            }

            // updating excel file
            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithoutCancellationMovements)))
            {
                step.WhenIUpdateTheExcelFileWithOfficialData("TestData_OfficialBalanceFileWithoutAnnulation");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions)))
            {
                step.WhenIUpdateTheExcelFileWithOfficialData("TestData_OfficialBalanceFileWithAnnulationCalculations");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements)))
            {
                step.WhenIUpdateTheExcelFileWithOfficialData("TestData_OfficialBalanceFileWithAnnulation");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                step.WhenIUpdateTheExcelFileWithOfficialData("TestData_OfficialBalanceFileWithAnnulationAsPerTransformation");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithoutAnnulation)))
            {
                step.WhenIUpdateTheExcelFileWithOfficialData("ErrorScenarioTestData_OfficialBalanceFileWithoutAnnulation");
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
            {
                step.WhenIUpdateTheExcelFileWithOfficialData("ErrorScenarioTestData_OfficialBalanceFileWithAnnulation");
            }

            ////step.When("I navigate to \"FileUpload\" page");
            step.UiNavigation("FileUpload");
            ////step.When("I click on \"FileUpload\" \"button\"");
            step.IClickOn("FileUpload", "button");
            ////step.When("I select segment from \"FileUpload\" \"Segment\" \"dropdown\"");
            step.ISelectSegmentFrom("FileUpload\" \"Segment", "dropdown");
            ////step.When("I select \"Insert\" from FileUpload dropdown");
            step.ISelectFileFromFileUploadDropdown("Insert");
            ////step.When("I click on \"Browse\" to upload");
            step.IClickOnUploadButton("Browse");

            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithoutCancellationMovements)))
            {
                ////step.When("I select \"TestData_OfficialBalanceFileWithoutAnnulation\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_OfficialBalanceFileWithoutAnnulation").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovementsCalcualtions)))
            {
                await step.ISelectFileFromDirectoryAsync("TestData_OfficialBalanceFileWithAnnulationCalculations").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithCancellationMovements)))
            {
                ////step.When("I select \"TestData_OfficialBalanceFileWithAnnulation\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_OfficialBalanceFileWithAnnulation").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                ////step.When("I select \"TestData_OfficialBalanceFileWithAnnulationAsPerTransformation\" file from directory");
                await step.ISelectFileFromDirectoryAsync("TestData_OfficialBalanceFileWithAnnulationAsPerTransformation").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithoutAnnulation)))
            {
                ////step.When("I select \"TestData_OfficialBalanceFileWithAnnulationAsPerTransformation\" file from directory");
                await step.ISelectFileFromDirectoryAsync("ErrorScenarioTestData_OfficialBalanceFileWithoutAnnulation").ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
            {
                ////step.When("I select \"TestData_OfficialBalanceFileWithAnnulationAsPerTransformation\" file from directory");
                await step.ISelectFileFromDirectoryAsync("ErrorScenarioTestData_OfficialBalanceFileWithAnnulation").ConfigureAwait(false);
            }

            ////step.When("I click on \"FileUpload\" \"Save\" \"button\"");
            step.IClickOn("uploadFile\" \"Submit", "button");
            ////step.When("I wait till file upload to complete");
            await step.WaitForFileUploadingToCompleteAsync().ConfigureAwait(false);
        }

        public static async Task CreationOfAnnulationForAllTransformationScenariosAsync(this StepDefinitionBase step)
        {
            //// Source Node - Origen
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId1"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId1"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId2"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId2"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId3"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId3"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId4"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId4"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId5"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId5"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId6"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId6"), sourceNodeId = 1, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId7"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId7"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId8"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId8"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId9"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId9"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId10"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId10"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId11"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId11"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId12"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId12"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId13"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId13"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId14"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId14"), sourceNodeId = 1, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);

            //// Source Node - Destino
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId15"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId15"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId16"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId16"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId17"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId17"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId18"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId18"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId19"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId19"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId20"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId20"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId21"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId21"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId22"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId22"), sourceNodeId = 2, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId23"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId23"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId24"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId24"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId25"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId25"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId26"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId26"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId27"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId27"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId28"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId28"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId29"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId29"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId30"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId30"), sourceNodeId = 2, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);

            //// Source Node - Ninguno
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId31"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId31"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId32"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId32"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId33"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId33"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId34"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId34"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId35"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId35"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId36"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId36"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId37"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId37"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId38"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId38"), sourceNodeId = 3, destinationNodeId = 1, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId39"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId39"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId40"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId40"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId41"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId41"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId42"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId42"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId43"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId43"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId44"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId44"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId45"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId45"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId46"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId46"), sourceNodeId = 3, destinationNodeId = 2, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);

            //// Destination Node - Ninguno
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId47"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId47"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId48"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId48"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId49"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId49"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId50"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId50"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId51"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId51"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId52"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId52"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId53"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId53"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId54"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId54"), sourceNodeId = 1, destinationNodeId = 3, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId55"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId55"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 1, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId56"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId56"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 1, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId57"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId57"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 1, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId58"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId58"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 3, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId59"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId59"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 3, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId60"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId60"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 2, destinationProductId = 1, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId61"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId61"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 2, destinationProductId = 2, isActive = 1 }).ConfigureAwait(false);
            await step.ReadSqlAsDictionaryAsync(SqlQueries.InsertAnnulationMovementForOfficialBalanceFile, args: new { movementTypeId = step.GetValueInternal("MovementTypeId62"), annulationMovementTypeId = step.GetValueInternal("AnnulationMovementTypeId62"), sourceNodeId = 2, destinationNodeId = 3, sourceProductId = 2, destinationProductId = 3, isActive = 1 }).ConfigureAwait(false);
        }

        public static async Task OfficialDeltaTicketGenerationAsync(this StepDefinitionBase step, EcpApiStepDefinitionBase ecpApiStep = null)
        {
            step.ThrowIfNull(nameof(step));
            if (ecpApiStep == null)
            {
                ecpApiStep = step.Resolve<EcpApiStepDefinitionBase>();
            }

            await ecpApiStep.TheTRUESystemIsProcessingTheOperativeMovementsAsync().ConfigureAwait(false);
        }

        public static void ThrowIfNull(this object arg, string argName, [CallerMemberName] string member = "")
        {
            if (arg == null)
            {
                throw new ArgumentNullException($"{member}: {argName}");
            }
        }
    }
}