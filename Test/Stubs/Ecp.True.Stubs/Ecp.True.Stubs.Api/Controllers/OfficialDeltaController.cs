
namespace Ecp.True.Stubs.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;
    using Ecp.True.Proxies.OwnershipRules.Response;
    using Ecp.True.Stubs.Api.Processor;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    [ApiController]
    public class OfficialDeltaController
    {
        [HttpPost]
        [Route("DecisionExecutor/rest/service/processWithDecisionFlow/officialdelta")]
        public IActionResult GetResultResponse([FromHeader] string chaos, [FromBody] JObject request)
        {
            if (string.Equals(chaos, "OfficialDeltaServerError", StringComparison.OrdinalIgnoreCase))
            {
                return new StatusCodeResult(500);
            }

            var outer = request.Value<JObject>("payload");
            var requestObject = outer.Value<JObject>("payloadInput");

            var officialDeltaRequest = JsonConvert.DeserializeObject<OfficialDeltaRequest>(JsonConvert.SerializeObject(requestObject));

            var innerOutput = JObject.Parse(JsonConvert.SerializeObject(string.Equals(chaos, "OfficialDeltaError", StringComparison.OrdinalIgnoreCase)
                    ? BuildErrorResponse(officialDeltaRequest) : BuildSuccessResponse(officialDeltaRequest), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            outer.Add("payloadOutput", innerOutput);

            return new JsonResult(request);
        }

        private static OfficialDeltaResponse BuildSuccessResponse(OfficialDeltaRequest officialDeltaRequest)
        {
            var (resultMovement, errormovement) = BuildResultMovements(officialDeltaRequest);
            return new OfficialDeltaResponse
            {
                ErrorMovements = errormovement,
                ErrorInventories = new List<OfficialDeltaErrorInventory>(),
                ResultMovements = resultMovement,
                ResultInventories = BuildResultInventories(officialDeltaRequest)
            };
        }

        private static OfficialDeltaResponse BuildErrorResponse(OfficialDeltaRequest officialDeltaRequest)
        {
            var errorDescription = "Error deliberado debido al encabezado del caos";
            return new OfficialDeltaResponse
            {
                ErrorMovements = officialDeltaRequest.PendingOfficialMovements.GroupBy(y => y.MovementTransactionId).Select(x => new OfficialDeltaErrorMovement { Description = errorDescription, MovementTransactionId = x.Key, Origin = Entities.Enumeration.OriginType.OFICIAL })
                                        .Union(officialDeltaRequest.ConsolidationMovements.GroupBy(y => y.ConsolidatedMovementId).Select(y => new OfficialDeltaErrorMovement { Description = errorDescription, MovementTransactionId = y.Key, Origin = Entities.Enumeration.OriginType.CONSOLIDADO })),
                ErrorInventories = officialDeltaRequest.PendingOfficialInventories.GroupBy(y => y.InventoryProductID).Select(x => new OfficialDeltaErrorInventory { Description = errorDescription, InventoryTransactionId = x.Key, Origin = Entities.Enumeration.OriginType.OFICIAL })
                                        .Union(officialDeltaRequest.ConsolidationInventories.GroupBy(y => y.ConsolidatedInventoryProductId).Select(y => new OfficialDeltaErrorInventory { Description = errorDescription, InventoryTransactionId = y.Key, Origin = Entities.Enumeration.OriginType.CONSOLIDADO })),
                ResultMovements = new List<OfficialDeltaResultMovement>(),
                ResultInventories = new List<OfficialDeltaResultInventory>()
            };
        }

        private static (IEnumerable<OfficialDeltaResultMovement>, IEnumerable<OfficialDeltaErrorMovement>) BuildResultMovements(OfficialDeltaRequest officialDeltaRequest)
        {
            var resultMovements = new List<OfficialDeltaResultMovement>();
            var errorMovement = new List<OfficialDeltaErrorMovement>();
            var movementKeysForConsolidated = new List<string>();
            foreach (var consolidatedMovementGroup in officialDeltaRequest.ConsolidationMovements.GroupBy(x => x.ConsolidatedMovementId))
            {
                var totalConsolidatedVolume = consolidatedMovementGroup.Sum(x => x.OwnershipVolume);
                var consolidatedMovement = consolidatedMovementGroup.First();
                movementKeysForConsolidated.Add(consolidatedMovement.GetMovementUniqueKey());
                var officialMovements = officialDeltaRequest.PendingOfficialMovements.Where(y =>
                                                    y.SourceNodeId == consolidatedMovement.SourceNodeId
                                                    && y.SourceProductId == consolidatedMovement.SourceProductId
                                                    && y.DestinationNodeId == consolidatedMovement.DestinationNodeId
                                                    && y.DestinationProductId == consolidatedMovement.DestinationProductId);

                var totalOfficialVolume = officialMovements.Sum(x => x.OwnerShipValue);

                if (totalOfficialVolume == 0.0M)
                {
                    if (!officialDeltaRequest.CancellationTypes.Any(x => x.SourceMovementTypeId == consolidatedMovement.MovementTypeId))
                    {
                        errorMovement.Add(new OfficialDeltaErrorMovement { Description = $"Tipo de cancelación no presente para movemnt tipo { consolidatedMovement.MovementTypeId}", MovementTransactionId = consolidatedMovement.ConsolidatedMovementId, Origin = Entities.Enumeration.OriginType.CONSOLIDADO });
                    }
                    else
                    {
                        foreach (var item in consolidatedMovementGroup.GroupBy(x => x.OwnerId))
                        {
                            resultMovements.Add(new OfficialDeltaResultMovement
                            {
                                DeltaOfficial = item.Sum(x => x.OwnershipVolume),
                                MovementOwnerId = item.Key,
                                MovementTransactionId = consolidatedMovementGroup.Key,
                                NetStandardVolume = totalConsolidatedVolume,
                                Origin = Entities.Enumeration.OriginType.CONSOLIDADO,
                                Sign = "NEGATIVO"
                            });
                        }
                    }
                }
                else if(totalOfficialVolume < totalConsolidatedVolume)
                {
                    var officialMovement = officialMovements.First();
                    if (!officialDeltaRequest.CancellationTypes.Any(x => x.SourceMovementTypeId == officialMovement.MovementTypeId))
                    {
                        errorMovement.Add(new OfficialDeltaErrorMovement { Description = $"Tipo de cancelación no presente para movemnt tipo {officialMovement.MovementTypeId}", MovementTransactionId = officialMovement.MovementTransactionId, Origin = Entities.Enumeration.OriginType.OFICIAL });
                    }
                    else
                    {
                        var numberofowners = consolidatedMovementGroup.Select(x => x.OwnerId).Distinct().Count();
                        var volumeDiff = totalConsolidatedVolume - totalOfficialVolume;
                        foreach (var item in consolidatedMovementGroup.GroupBy(x => x.OwnerId))
                        {
                            resultMovements.Add(new OfficialDeltaResultMovement
                            {
                                DeltaOfficial = volumeDiff / numberofowners,
                                MovementOwnerId = item.Key,
                                MovementTransactionId = officialMovement.MovementTransactionId,
                                NetStandardVolume = volumeDiff,
                                Origin = Entities.Enumeration.OriginType.OFICIAL,
                                Sign = "NEGATIVO"
                            });
                        }
                    }
                }
                else
                {
                    var numberofowners = consolidatedMovementGroup.Select(x => x.OwnerId).Distinct().Count();
                    var volumeDiff = totalOfficialVolume - totalConsolidatedVolume;
                    foreach (var item in consolidatedMovementGroup.GroupBy(x => x.OwnerId))
                    {
                        resultMovements.Add(new OfficialDeltaResultMovement
                        {
                            DeltaOfficial = volumeDiff / numberofowners,
                            MovementOwnerId = item.Key,
                            MovementTransactionId = officialMovements.First().MovementTransactionId,
                            NetStandardVolume = volumeDiff,
                            Origin = Entities.Enumeration.OriginType.OFICIAL,
                            Sign = "POSITIVO"
                        });
                    }
                }
            }

            var officialMovementsWithoutConsolidated = officialDeltaRequest.PendingOfficialMovements.Where(x => !movementKeysForConsolidated.Any(y => y == x.GetMovementUniqueKey()));

            foreach (var officialMovement in officialMovementsWithoutConsolidated)
            {
                var numberofowners = officialMovementsWithoutConsolidated.Where(x => x.MovementTransactionId == officialMovement.MovementTransactionId).Select(x => x.OwnerId).Distinct().Count();
                var volumeDiff = officialMovementsWithoutConsolidated.Where(x => x.MovementTransactionId == officialMovement.MovementTransactionId).Sum(x => x.OwnerShipValue);

                resultMovements.Add(new OfficialDeltaResultMovement
                {
                    DeltaOfficial = volumeDiff / numberofowners,
                    MovementOwnerId = officialMovement.MovementOwnerId,
                    MovementTransactionId = officialMovement.MovementTransactionId,
                    NetStandardVolume = volumeDiff,
                    Origin = Entities.Enumeration.OriginType.OFICIAL,
                    Sign = "POSITIVO"
                });
            }

            return (resultMovements, errorMovement);
        }

        private static IEnumerable<OfficialDeltaResultInventory> BuildResultInventories(OfficialDeltaRequest officialDeltaRequest)
        {
            var resultInventories = new List<OfficialDeltaResultInventory>();
            var inventoryKeysForConsolidated = new List<string>();
            foreach (var consolidatedInventoryGroup in officialDeltaRequest.ConsolidationInventories.GroupBy(x => x.ConsolidatedInventoryProductId))
            {
                var totalConsolidatedVolume = consolidatedInventoryGroup.Sum(x => x.OwnershipVolume);
                var consolidatedInventory = consolidatedInventoryGroup.First();
                inventoryKeysForConsolidated.Add(consolidatedInventory.GetInventoryUniqueKey());
                var officialInventories = officialDeltaRequest.PendingOfficialInventories.Where(y =>
                                                    y.NodeId == consolidatedInventory.NodeId
                                                    && y.ProductID == consolidatedInventory.ProductId
                                                    && y.InventoryDate.Date == consolidatedInventory.InventoryDate.Date);

                var totalOfficialVolume = officialInventories.Sum(x => x.OwnerShipValue);

                if (totalOfficialVolume == 0.0M)
                {
                    foreach (var item in consolidatedInventoryGroup.GroupBy(x => x.OwnerId))
                    {
                        resultInventories.Add(new OfficialDeltaResultInventory
                        {
                            DeltaOfficial = item.Sum(x => x.OwnershipVolume),
                            InventoryProductOwnerId = item.Key,
                            InventoryTransactionId = consolidatedInventoryGroup.Key,
                            NetStandardVolume = totalConsolidatedVolume,
                            Origin = Entities.Enumeration.OriginType.CONSOLIDADO,
                            Sign = "NEGATIVO"
                        });
                    }
                }
                else if (totalOfficialVolume < totalConsolidatedVolume)
                {
                    var numberofowners = consolidatedInventoryGroup.Select(x => x.OwnerId).Distinct().Count();
                    var volumeDiff = totalConsolidatedVolume - totalOfficialVolume;
                    foreach (var item in consolidatedInventoryGroup.GroupBy(x => x.OwnerId))
                    {
                        resultInventories.Add(new OfficialDeltaResultInventory
                        {
                            DeltaOfficial = volumeDiff / numberofowners,
                            InventoryProductOwnerId = item.Key,
                            InventoryTransactionId = officialInventories.First().InventoryProductID,
                            NetStandardVolume = volumeDiff,
                            Origin = Entities.Enumeration.OriginType.OFICIAL,
                            Sign = "NEGATIVO"
                        });
                    }
                }
                else
                {
                    var numberofowners = consolidatedInventoryGroup.Select(x => x.OwnerId).Distinct().Count();
                    var volumeDiff = totalOfficialVolume - totalConsolidatedVolume;
                    foreach (var item in consolidatedInventoryGroup.GroupBy(x => x.OwnerId))
                    {
                        resultInventories.Add(new OfficialDeltaResultInventory
                        {
                            DeltaOfficial = volumeDiff / numberofowners,
                            InventoryProductOwnerId = item.Key,
                            InventoryTransactionId = officialInventories.First().InventoryProductID,
                            NetStandardVolume = volumeDiff,
                            Origin = Entities.Enumeration.OriginType.OFICIAL,
                            Sign = "POSITIVO"
                        });
                    }
                }
            }

            var officialInventoriesWithoutConsolidated = officialDeltaRequest.PendingOfficialInventories.Where(x => !inventoryKeysForConsolidated.Any(y => y == x.GetInventoryUniqueKey()));

            foreach (var officialInventories in officialInventoriesWithoutConsolidated)
            {
                var numberofowners = officialInventoriesWithoutConsolidated.Where(x => x.InventoryProductID == officialInventories.InventoryProductID).Select(x => x.OwnerId).Distinct().Count();
                var volumeDiff = officialInventoriesWithoutConsolidated.Where(x => x.InventoryProductID == officialInventories.InventoryProductID).Sum(x => x.OwnerShipValue);
                resultInventories.Add(new OfficialDeltaResultInventory
                {
                    DeltaOfficial = volumeDiff / numberofowners,
                    InventoryProductOwnerId = officialInventories.OwnerId,
                    InventoryTransactionId = officialInventories.InventoryProductID,
                    NetStandardVolume = volumeDiff,
                    Origin = Entities.Enumeration.OriginType.OFICIAL,
                    Sign = "POSITIVO"
                });
            }

            return resultInventories;
        }
    }
}
