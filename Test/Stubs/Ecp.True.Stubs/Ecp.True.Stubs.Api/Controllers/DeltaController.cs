using System;
using System.Collections.Generic;
using System.Linq;

using Ecp.True.Stubs.Api.Request;
using Ecp.True.Stubs.Api.Response;

using Microsoft.AspNetCore.Mvc;

namespace Ecp.True.Stubs.Api.Controllers
{
    [ApiController]
    public class DeltaController
    {
        [HttpPost]
        [Route("DecisionExecutor/rest/service/processWithDecisionFlow/delta")]
        public ActionResult<DeltaResponse> GetResultResponse([FromHeader] string chaos, [FromBody] DeltaRequest deltaRequest)
        {
            if (string.Equals(chaos, "DeltaServerError", StringComparison.OrdinalIgnoreCase))
            {
                return new StatusCodeResult(500);
            }

            if (string.Equals(chaos, "DeltaError", StringComparison.OrdinalIgnoreCase))
            {
                var errorDescription = "Error deliberado debido al encabezado del caos";
                return new DeltaResponse
                {
                    ErrorMovements = deltaRequest.UpdatedMovements.Select(x => new DeltaErrorMovement { MovementId = x.MovementId, MovementTransactionId = x.MovementTransactionId, Description = errorDescription }),
                    ErrorInventories = deltaRequest.UpdatedInventories.Select(x => new DeltaErrorInventory { InventoryId = x.InventoryId, InventoryProductId = x.InventoryProductId, Description = errorDescription }),
                    ResultMovements = new List<DeltaResultMovement>(),
                    ResultInventories = new List<DeltaResultInventory>()
                };
            }

            var deltaResultMovements = BuildResultMovements(deltaRequest);
            var deltaResultInventories = BuildResultInventories(deltaRequest);
            return new DeltaResponse
            {
                ErrorMovements = new List<DeltaErrorMovement>(),
                ErrorInventories = new List<DeltaErrorInventory>(),
                ResultMovements = deltaResultMovements,
                ResultInventories = deltaResultInventories
            };
        }

        private static IEnumerable<DeltaResultMovement> BuildResultMovements(DeltaRequest deltaRequest)
        {
            var movementResults = new List<DeltaResultMovement>();
            foreach (var updatedMovement in deltaRequest.UpdatedMovements)
            {
                var isDelete = updatedMovement.EventType.Equals("Delete", StringComparison.OrdinalIgnoreCase);

                var originalMovement = deltaRequest.OriginalMovements.SingleOrDefault(x => x.MovementId.Equals(updatedMovement.MovementId, StringComparison.OrdinalIgnoreCase));
                if (originalMovement == null)
                {
                    movementResults.Add(new DeltaResultMovement
                    {
                        MovementId = updatedMovement.MovementId,
                        MovementTransactionId = updatedMovement.MovementTransactionId,
                        Delta = updatedMovement.Volume,
                        Sign = isDelete ? "NEGATIVO" : "POSITIVO"
                    });
                }
                else
                {
                    var delta = (isDelete ? 0 : updatedMovement.Volume) - originalMovement.Volume;
                    if (Math.Abs(delta) > 0)
                    {
                        movementResults.Add(new DeltaResultMovement
                        {
                            MovementId = updatedMovement.MovementId,
                            MovementTransactionId = updatedMovement.MovementTransactionId,
                            Delta = Math.Abs(delta),
                            Sign = delta >= 0 ? "POSITIVO" : "NEGATIVO"
                        });
                    }
                }
            }

            return movementResults;
        }

        private static IEnumerable<DeltaResultInventory> BuildResultInventories(DeltaRequest deltaRequest)
        {
            var inventoryResults = new List<DeltaResultInventory>();
            foreach (var updatedInventory in deltaRequest.UpdatedInventories)
            {
                var isDelete = updatedInventory.EventType.Equals("Delete", StringComparison.OrdinalIgnoreCase);

                var originalInventory = deltaRequest.OriginalInventories.SingleOrDefault(x => x.InventoryId.Equals(updatedInventory.InventoryId, StringComparison.OrdinalIgnoreCase));
                if (originalInventory == null)
                {
                    inventoryResults.Add(new DeltaResultInventory
                    {
                        InventoryId = updatedInventory.InventoryId,
                        InventoryProductId = updatedInventory.InventoryProductId,
                        Delta = updatedInventory.Volume,
                        Sign = isDelete ? "NEGATIVO" : "POSITIVO"
                    });
                }
                else
                {
                    var delta = (isDelete ? 0 : updatedInventory.Volume) - originalInventory.Volume;
                    if (Math.Abs(delta) > 0)
                    {
                        inventoryResults.Add(new DeltaResultInventory
                        {
                            InventoryId = updatedInventory.InventoryId,
                            InventoryProductId = updatedInventory.InventoryProductId,
                            Delta = Math.Abs(delta),
                            Sign = delta >= 0 ? "POSITIVO" : "NEGATIVO"
                        });
                    }
                }
            }

            return inventoryResults;
        }
    }
}
