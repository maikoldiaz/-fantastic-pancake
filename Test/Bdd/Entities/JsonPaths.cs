// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonPaths.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Entities
{
    using System;
    using System.Collections.Generic;

    public static class JsonPaths
    {
        public static Dictionary<string, string> Mappings { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "StorageLocation", "nodeStorageLocations[0]." },
            { "NewStorageLocation", "nodeStorageLocations[1]." },
            { "ProductLocation", "nodeStorageLocations[0].products[0]." },
            { "Product", "nodeStorageLocations[0].products[0]." },
            { "ProductLocations", "nodeStorageLocations[0]." },
            { "Products", "nodeStorageLocations[0]." },
            { "HomologationGroup",  "homologationGroups[0]." },
            { "HomologationObject",  "homologationGroups[0].homologationObjects[0]." },
            { "HomologationObjects",  "homologationGroups[0]." },
            { "HomologationDataMapping",  "homologationGroups[0].homologationDataMapping[0]." },
            { "HomologationDataMapping_Source",  "homologationGroups[0].homologationDataMapping[0]." },
            { "HomologationDataMapping_Destination",  "homologationGroups[0].homologationDataMapping[1]." },
            { "HomologationDataMapping_NodeId",  "homologationGroups[0].homologationDataMapping[2]." },
            { "HomologationDataMapping_TankId1",  "homologationGroups[0].homologationDataMapping[3]." },
            { "HomologationDataMapping_TankId2",  "homologationGroups[0].homologationDataMapping[4]." },
            { "HomologationDataMapping_SourceProductId",  "homologationGroups[1].homologationDataMapping[1]." },
            { "HomologationDataMapping_DestinationProductId",  "homologationGroups[1].homologationDataMapping[2]." },
            { "HomologationDataMapping_Source1",  "homologationGroups[0].homologationDataMapping[3]." },
            { "Product_1-Owner_1",  "products[0].owners[0]." },
            { "Product_1-Owner_2",  "products[0].owners[1]." },
            { "Product_2-Owner_1",  "products[1].owners[0]." },
            { "Product_2-Owner_2",  "products[1].owners[1]." },
            { "Product_3-Owner_1",  "products[2].owners[0]." },
            { "Product_3-Owner_2",  "products[2].owners[1]." },
            { "Product_4-Owner_1",  "products[3].owners[0]." },
            { "Product_4-Owner_2",  "products[3].owners[1]." },
            { "ProductId",  "products[0]." },
            { "OwnerId",  "products[0].owners[0]." },
            { "Attribute",  "products[0].attributes[0]." },
            { "OwnershipPercentage",  "products[0].owners[0]." },
            { "Owners",  "products[0]." },
            { "Connection_Product_1",  "products[0]." },
            { "Connection_Product_2",  "products[1]." },
            { "HomologationDataMapping_Node1",  "homologationGroups[0].homologationDataMapping[0]." },
            { "HomologationDataMapping_Node2",  "homologationGroups[0].homologationDataMapping[1]." },
            { "HomologationDataMapping_Node3",  "homologationGroups[0].homologationDataMapping[2]." },
            { "HomologationDataMapping_Node4",  "homologationGroups[0].homologationDataMapping[3]." },
            { "HomologationDataMapping_Node5",  "homologationGroups[0].homologationDataMapping[4]." },
            { "HomologationDataMapping_Node6",  "homologationGroups[0].homologationDataMapping[5]." },
            { "HomologationDataMapping_MovementTypeId",  "homologationGroups[0].homologationDataMapping[0]." },
            { "HomologationDataMapping_ProductTypeId",  "homologationGroups[0].homologationDataMapping[0]." },
            { "HomologationDataMapping_SourceProductTypeId",  "homologationGroups[0].homologationDataMapping[1]." },
            { "HomologationDataMapping_DestinationProductTypeId",  "homologationGroups[0].homologationDataMapping[2]." },
            { "HomologationDataMapping_Owner",  "homologationGroups[0].homologationDataMapping[0]." },
            { "HomologationObj", "homologationGroups[0]" },
            { "HomologationMap", "homologationGroups[0].homologationMappings" },
            { "Period", "period." },
            { "MovementOwner", "owners[0]." },
            { "MovementSource", "movementSource." },
            { "MovementDestination", "movementDestination." },
            { "Attributes", "attributes[0]." },
            { "BackupMovement", "backupMovement." },
            { "ProductsType", "products[0]." },
            { "FicoRules", "volPayload.volInput." },
            { "OwnershipRules", "ids[0]." },
            { "ConnectionUpdate", string.Empty },
            { "NodeUpdate", string.Empty },
            { "Ticket", "ticket." },
            { "OfficialInformation", "officialInformation." },
            { "BatchId", "products[0]." },
            { "Product_1", "products[0]." },
            { "Product_2", "products[1]." },
            { "SAPMovementAttribute", "attributes[0]." },
            { "SAPMovementOwners", "owners[0]." },
            { "Movement", string.Empty },
        };

        public static Dictionary<string, string> PathMaps { get; } = new Dictionary<string, string>()
        {
            { "HomologationObject", "HomologationObject homologationObjectTypeId" },
            { "HomologationDataMapping", "HomologationMapping sourceValue" },
            { "Group HomologationGroupId", "HomologationGroup homologationGroupId" },
            { "Object HomologationGroupId", "HomologationObject homologationGroupId" },
            { "DataMapping HomologationGroupId", "HomologationDataMapping homologationGroupId" },
            { "Object HomologationObjectId", "HomologationObject homologationObjectId" },
            { "DataMapping HomologationDataMappingId", "HomologationDataMapping homologationDataMappingId" },
            { "IsRequiredMapping", "HomologationObject isRequiredMapping" },
        };
    }
}