// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipResponseProcessor.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Stubs.Api.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ecp.True.Stubs.Api.Models;
    using Ecp.True.Stubs.Api.Request;
    using Ecp.True.Stubs.Api.Response;
    using Microsoft.Extensions.Options;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class OwnershipResponseProcessor : IOwnershipResponseProcessor
    {
        /// <summary>
        /// The BLOB client.
        /// </summary>
        private CloudBlobClient blobClient;

        private const string containerName = "ownership";
        private readonly StubConfiguration configuration;

        public OwnershipResponseProcessor(IOptions<StubConfiguration> options)
        {
            this.configuration = options.Value;
            InitializeBlobClient();
        }

        public async Task SaveJsonToBlob(JObject ownershipResponse, string blobName)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(ownershipResponse.ToString())))
            {
                var container = this.blobClient.GetContainerReference(containerName);
                var blob = container.GetBlockBlobReference(blobName);
                await blob.UploadFromStreamAsync(ms);
            }
        }

        public async Task<JObject> GetResponseJson(JObject request, bool hasErrors, string blobName)
        {
            var stream = await GetCloudBlobStreamAsync(blobName).ConfigureAwait(false);
            var ownershipResponse = stream != null ? DeserializeStream<JObject>(stream) : BuildResponse(request, hasErrors);

            return ownershipResponse;
        }

        public Task<bool> DeleteResponseBlob(string blobName)
        {
            var container = this.blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            return blob.DeleteIfExistsAsync();
        }

        public JObject BuildResponse(JObject request, bool hasErrors)
        {
            var outer = request.Value<JObject>("volPayload");
            var requestObject = outer.Value<JObject>("volInput");

            var isActive = requestObject != null && requestObject["estrategiasActivas"] != null ? Convert.ToBoolean(requestObject["estrategiasActivas"]) : true;
            var ownershipRuleRequest = JsonConvert.DeserializeObject<OwnershipRuleRequest>(JsonConvert.SerializeObject(requestObject));
            var ownershipRuleResponse = this.BuildOwnershipResult(ownershipRuleRequest, hasErrors, isActive);

            var innerOutput = JObject.Parse(JsonConvert.SerializeObject(ownershipRuleResponse, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            outer.Add("volOutput", innerOutput);

            return request;
        }

        private void InitializeBlobClient()
        {
            var storageAccount = CloudStorageAccount.Parse(this.configuration.StorageConnection);
            this.blobClient = storageAccount.CreateCloudBlobClient();
        }

        private OwnershipRuleResponse BuildOwnershipResult(OwnershipRuleRequest ownershipRuleRequest, bool hasErrors, bool isActive)
        {
            var movementResults = new List<OwnershipResultMovement>();
            var inventoryResults = new List<OwnershipResultInventory>();
            var movementErrors = new List<OwnershipErrorMovement>();
            var inventoryErrors = new List<OwnershipErrorInventory>();
            var nodeOwnershipRules = new List<OwnershipRule>();
            var nodeProductOwnershipRules = new List<OwnershipRule>();
            var ownershipRuleConnections = new List<OwnershipRule>();
            var newMovements = new List<NewMovement>();
            var commercialMovementResults = new List<CommercialMovementResult>();
            var cancellationMovements = new List<string> { "ANULACIONENTRADA", "ANULACIONSALIDA" };

            if (isActive)
            {
                nodeOwnershipRules = new List<OwnershipRule>
                { new OwnershipRule
                    {
                        RuleId = 1,
                        Name = "Entradas"
                    },
                    new OwnershipRule
                    {
                        RuleId = 2,
                        Name = "Disponible"
                    }
                };

                nodeProductOwnershipRules = new List<OwnershipRule>
                { new OwnershipRule
                    {
                        RuleId = 1,
                        Name = "Salidas"
                    },
                    new OwnershipRule
                    {
                        RuleId = 2,
                        Name = "Inventário Final"
                    }
                };

                ownershipRuleConnections = new List<OwnershipRule>
                { new OwnershipRule
                    {
                        RuleId = 1,
                        Name = "Entradas"
                    },
                    new OwnershipRule
                    {
                        RuleId = 2,
                        Name = "Disponible"
                    },
                    new OwnershipRule
                    {
                        RuleId = 3,
                        Name = "Eventos"
                    }
                };
            }
            else
            {
                nodeOwnershipRules = new List<OwnershipRule>
                {
                    new OwnershipRule
                    {
                        RuleId = 5,
                        Name = "Inactive"
                    }
                };

                nodeProductOwnershipRules = new List<OwnershipRule>
                {
                    new OwnershipRule
                    {
                        RuleId = 5,
                        Name = "Inactive"
                    }
                };

                ownershipRuleConnections = new List<OwnershipRule>
                {
                    new OwnershipRule
                    {
                        RuleId = 5,
                        Name = "Inactive"
                    }
                };
            }

            if (ownershipRuleRequest.MovementsOperationalData != null)
            {
                var previousMovementIds = ownershipRuleRequest.PreviousMovementsOperationalData.Select(x => x.MovementId).ToList();
                var movements = ownershipRuleRequest.MovementsOperationalData.Where(x => !(previousMovementIds.Contains(x.MovementId) || cancellationMovements.Contains(x.MovementTypeId)));

                foreach (var x in movements)
                {
                    if (!movementResults.Any(y => y.MovementId == x.MovementId.ToString(CultureInfo.InvariantCulture)))
                    {
                        movementResults.Add(new OwnershipResultMovement
                        {
                            MovementId = x.MovementId.ToString(CultureInfo.InvariantCulture),
                            OwnershipVolume = x.NetVolume.GetValueOrDefault(),
                            OwnerId = 30,
                            OwnershipPercentage = 100.00M,
                            AppliedRule = "1",
                            RuleVersion = 1,
                            Ticket = x.Ticket.ToString(CultureInfo.InvariantCulture),
                        });

                        if (hasErrors)
                        {
                            movementErrors.Add(new OwnershipErrorMovement
                            {
                                MovementId = x.MovementId.ToString(CultureInfo.InvariantCulture),
                                ExecutionDate = x.OperationalDate,
                                SourceNodeId = Convert.ToString(x.SourceNodeId ?? x.DestinationNodeId.GetValueOrDefault(), CultureInfo.InvariantCulture),
                                ErrorDescription = "Error occurred",
                            });
                        }
                    }
                }
            }

            if (ownershipRuleRequest.InventoryOperationalData != null)
            {
                var previousInventoryIds = ownershipRuleRequest.PreviousInventoryOperationalData.Select(x => x.InventoryId).ToList();
                var inventories = ownershipRuleRequest.InventoryOperationalData.Where(x => !previousInventoryIds.Contains(x.InventoryId));

                foreach (var x in inventories)
                {
                    if (!inventoryResults.Any(y => y.InventoryId == x.InventoryId.ToString(CultureInfo.InvariantCulture)))
                    {
                        inventoryResults.Add(new OwnershipResultInventory
                        {
                            InventoryId = x.InventoryId.ToString(CultureInfo.InvariantCulture),
                            OwnershipVolume = x.NetVolume.GetValueOrDefault(),
                            OwnerId = 30,
                            OwnershipPercentage = 100.00M,
                            AppliedRule = "1",
                            RuleVersion = 1,
                            Ticket = x.Ticket.ToString(CultureInfo.InvariantCulture),
                        });

                        if (hasErrors)
                        {
                            inventoryErrors.Add(new OwnershipErrorInventory
                            {
                                InventoryId = x.InventoryId.ToString(CultureInfo.InvariantCulture),
                                ExecutionDate = x.OperationalDate,
                                NodeId = Convert.ToString(x.NodeId, CultureInfo.InvariantCulture),
                                ErrorDescription = "Error occurred",
                            });
                        }
                    }
                }
            }

            if (ownershipRuleRequest.Events != null)
            {
                foreach (var evnt in ownershipRuleRequest.Events.Where(x => x.IsAgreement))
                {
                    var movement = ownershipRuleRequest.MovementsOperationalData.FirstOrDefault(
                            x => x.SourceNodeId.HasValue &&
                            x.DestinationNodeId.HasValue &&
                            !newMovements.Any(y => y.MovementId == x.MovementId));

                    if (movement != null)
                    {
                        newMovements.Add(new NewMovement()
                        {
                            AgreementType = "COLABORACION",
                            CreditorOwnerId = evnt.OwnerId1,
                            DebtorOwnerId = evnt.OwnerId2,
                            OwnershipVolume = evnt.OwnershipValue ?? 0.00M,
                            AppliedRule = evnt.IsAgreement ? "6" : "7",
                            RuleVersion = "1",
                            EventId = evnt.EventIdentifier,
                            MovementId = movement.MovementId,
                            NodeId = movement.SourceNodeId.Value,
                            ProductId = movement.SourceProductId,
                        });
                    }
                }

                foreach (var evnt in ownershipRuleRequest.Events.Where(x => !x.IsAgreement))
                {
                    var movement = ownershipRuleRequest.MovementsOperationalData.FirstOrDefault(
                            x => x.SourceNodeId.HasValue &&
                            x.DestinationNodeId.HasValue);
                    if (movement != null)
                    {
                        newMovements.Add(new NewMovement()
                        {
                            AgreementType = "EVACUACION",
                            CreditorOwnerId = evnt.OwnerId1,
                            DebtorOwnerId = evnt.OwnerId2,
                            OwnershipVolume = evnt.OwnershipValue ?? 0.00M,
                            AppliedRule = evnt.IsAgreement ? "6" : "7",
                            RuleVersion = "1",
                            NodeId = movement.DestinationNodeId.Value,
                            ProductId = movement.DestinationProductId,
                        });
                    }
                }
            }

            if (ownershipRuleRequest.Contracts != null)
            {
                foreach (var contract in ownershipRuleRequest.Contracts)
                {
                    var movement = ownershipRuleRequest.MovementsOperationalData.FirstOrDefault(
                         x => x.SourceNodeId.HasValue &&
                         x.DestinationNodeId.HasValue &&
                         !commercialMovementResults.Any(y => y.MovementId == x.MovementId));

                    if (movement != null)
                    {
                        commercialMovementResults.Add(new CommercialMovementResult()
                        {
                            MovementId = movement.MovementId,
                            ContractId = contract.ContractId,
                            MovementType = "COMPRA",
                            OwnerId = contract.BuyerOwnerId,
                            Volume = contract.ContractValue ?? 0.00M,
                            AppliedRule = "5",
                            RuleVersion = "1",
                        });

                        commercialMovementResults.Add(new CommercialMovementResult()
                        {
                            MovementId = movement.MovementId,
                            ContractId = contract.ContractId,
                            MovementType = "VENTA",
                            OwnerId = contract.SellerOwnerId,
                            Volume = contract.ContractValue ?? 0.00M,
                            AppliedRule = "5",
                            RuleVersion = "1",
                        });
                    }
                }
            }

            return new OwnershipRuleResponse
            {
                InventoryResults = inventoryResults,
                MovementResults = movementResults,
                InventoryErrors = inventoryErrors,
                MovementErrors = movementErrors,
                NodeOwnershipRules = nodeOwnershipRules,
                NodeProductOwnershipRules = nodeProductOwnershipRules,
                OwnershipRuleConnections = ownershipRuleConnections,
                NewMovements = newMovements,
                CommercialMovementsResults = commercialMovementResults
            };
        }

        private async Task<Stream> GetCloudBlobStreamAsync(string blobName)
        {
            var container = this.blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            var blobExists = await blob.ExistsAsync();
            if (!blobExists)
            {
                return null;
            }

            return await blob.OpenReadAsync();
        }

        private T DeserializeStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();
            using (var textStream = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(textStream))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
