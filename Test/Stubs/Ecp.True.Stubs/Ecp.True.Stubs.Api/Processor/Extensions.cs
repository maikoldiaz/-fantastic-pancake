using System;
using System.Security.Cryptography;
using System.Text;
using Ecp.True.Proxies.OwnershipRules.OfficialDeltaDataRequest;

namespace Ecp.True.Stubs.Api.Processor
{
    public static class Extensions
    {
        public static string GetMovementUniqueKey(this OfficialDeltaConsolidatedMovement officialDeltaConsolidatedMovement)
        {
            return GetSha256Hash(
                string.Join(
                ',',
                officialDeltaConsolidatedMovement.SourceNodeId,
                officialDeltaConsolidatedMovement.SourceProductId,
                officialDeltaConsolidatedMovement.DestinationNodeId,
                officialDeltaConsolidatedMovement.DestinationProductId));
        }

        public static string GetMovementUniqueKey(this OfficialDeltaPendingOfficialMovement officialDeltaPendingOfficialMovement)
        {
            return GetSha256Hash(
                string.Join(
                ',',
                officialDeltaPendingOfficialMovement.SourceNodeId,
                officialDeltaPendingOfficialMovement.SourceProductId,
                officialDeltaPendingOfficialMovement.DestinationNodeId,
                officialDeltaPendingOfficialMovement.DestinationProductId));
        }

        public static string GetInventoryUniqueKey(this OfficialDeltaConsolidatedInventoryProduct officialDeltaConsolidatedInventoryProduct)
        {
            return GetSha256Hash(
                string.Join(
                ',',
                officialDeltaConsolidatedInventoryProduct.NodeId,
                officialDeltaConsolidatedInventoryProduct.ProductId,
                officialDeltaConsolidatedInventoryProduct.InventoryDate.Date.ToString()));
        }

        public static string GetInventoryUniqueKey(this OfficialDeltaPendingOfficialInventory officialDeltaPendingOfficialInventory)
        {
            return GetSha256Hash(
                string.Join(
                ',',
                officialDeltaPendingOfficialInventory.NodeId,
                officialDeltaPendingOfficialInventory.ProductID,
                officialDeltaPendingOfficialInventory.InventoryDate.Date.ToString()));
        }

        public static string GetSha256Hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
