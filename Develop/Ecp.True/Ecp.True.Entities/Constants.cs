// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Entities
{
    /// <summary>
    /// The Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The true message.
        /// </summary>
        public const string TrueMessage = "TRUE";

        /// <summary>
        /// The allow numbers and letters with special characters with space regex.
        /// </summary>
        public const string AllowNumbersAndLettersWithSpecialCharactersWithSpaceRegex = "^[\"A-Za-z0-9\u00C0-\u00FF :_-]+$";

        /// <summary>
        /// The retry pending transaction.
        /// </summary>
        public const string RetryPendingTransaction = "Retrying pending transaction";

        /// <summary>
        /// The retry message error.
        /// </summary>
        public const string RetryMessageError = "RetryMessageError";

        /// <summary>
        /// The algorithm not found.
        /// </summary>
        public const string AlgorithmNotDefined = "Algoritmo no definido para el punto de transferencia";

        /// <summary>
        /// The category name required.
        /// </summary>
        public const string CategoryNameRequired = "CategoryNameRequired";

        /// <summary>
        /// The category element name required.
        /// </summary>
        public const string CategoryElementNameRequired = "CategoryElementNameRequired";

        /// <summary>
        /// The only alpha numeric allowed.
        /// </summary>
        public const string OnlyAlphaNumericAllowed = "OnlyAlphaNumericAllowed";

        /// <summary>
        /// The name maximum length of 150 characters.
        /// </summary>
        public const string NameMaxLength150 = "NameMaxLength150";

        /// <summary>
        /// The name maximum length of 25 characters.
        /// </summary>
        public const string InventoryBatchIdMax25Characters = "El identificador del batch puede contener máximo 25 caracteres";

        /// <summary>
        /// The name maximum length of 25 characters.
        /// </summary>
        public const string MovementBatchIdMax25Characters = "El identificador del batch puede contener máximo 25 caracteres";

        /// <summary>
        /// The category name already exists.
        /// </summary>
        public const string CategoryNameAlreadyExists = "CategoryNameAlreadyExists";

        /// <summary>
        /// The category name already exists.
        /// </summary>
        public const string CategoryColorAlreadyExists = "CategoryColorAlreadyExists";

        /// <summary>
        /// The category element name already exists.
        /// </summary>
        public const string CategoryElementNameAlreadyExists = "CategoryElementNameAlreadyExists";

        /// <summary>
        /// The description maximum length of 1000 characters.
        /// </summary>
        public const string DescriptionMaxLength1000 = "DescriptionMaxLength1000";

        /// <summary>
        /// The allow numbers and letters with special characters and space message.
        /// </summary>
        public const string AllowAlphanumericWithSpecialCharactersAndSpaceMessage = "AllowAlphanumericWithSpecialCharactersAndSpaceMessage";

        /// <summary>
        /// The allow numbers and letters with special characters and space message.
        /// </summary>
        public const string NodeNameSpecialCharactersMessage = "NodeNameSpecialCharactersMessage";

        /// <summary>
        /// The storage location name special characters message.
        /// </summary>
        public const string StorageLocationNameSpecialCharactersMessage = "StorageLocationNameSpecialCharactersMessage";

        /// <summary>
        /// The category status is required.
        /// </summary>
        public const string CategoryStatusIsRequired = "CategoryStatusIsRequired";

        /// <summary>
        /// The category status is required.
        /// </summary>
        public const string AnnulationStatusIsRequired = "AnnulationStatusIsRequired";

        /// <summary>
        /// The Node MovementType by CostCenter status is required.
        /// </summary>
        public const string NodeCostCenterStatusIsRequired = "NODECOSTCENTER_STATUS_REQUIREDVALIDATION";

        /// <summary>
        /// The Node cost center source node is required.
        /// </summary>
        public const string NodeCostCenterSourceNodeRequiredValidation = "NODECOSTCENTER_SOURCENODE_REQUIREDVALIDATION";

        /// <summary>
        /// The Node cost center movementType is required.
        /// </summary>
        public const string NodeCostCenterMovementTypeRequiredValidation = "NODECOSTCENTER_MOVEMENTTYPE_REQUIREDVALIDATION";

        /// <summary>
        /// The Node cost center costCenter is required.
        /// </summary>
        public const string NodeCostCenterCostCenterRequiredValidation = "NODECOSTCENTER_COSTCENTER_REQUIREDVALIDATION";

        /// <summary>
        /// The Node cost center already exists in active state.
        /// </summary>
        public const string NodeCostCenterAlreadyExistsActive = "NODECOSTCENTER_ALREADYEXISTSACTIVE";

        /// <summary>
        /// The Node cost center already exists in inactive state.
        /// </summary>
        public const string NodeCostCenterAlreadyExistsInactive = "NODECOSTCENTER_ALREADYEXISTSINACTIVE";

        /// <summary>
        /// The Node cost center costCenter is doesn't exist.
        /// </summary>
        public const string NodeCostCenterDoesNotExists = "NODECOSTCENTER_NOTEXISTS";

        /// <summary>
        /// The Node cost center costCenter is doesn't exist.
        /// </summary>
        public const string NodeCostCenterHasMovements = "NODECOSTCENTER_HASMOVEMENTS";

        /// <summary>
        /// The max150 characters.
        /// </summary>
        public const string Max150Characters = "Max150Characters";

        /// <summary>
        /// The max150 characters.
        /// </summary>
        public const string NodeMax150Characters = "NodeMax150Characters";

        /// <summary>
        /// The storage location name max150 characters.
        /// </summary>
        public const string StorageLocationNameMax150Characters = "StorageLocationNameMax150Characters";

        /// <summary>
        /// The max20 characters.
        /// </summary>
        public const string Max20Characters = "Max20Characters";

        /// <summary>
        /// The max1000 characters.
        /// </summary>
        public const string Max1000Characters = "Max1000Characters";

        /// <summary>
        /// The max1000 characters.
        /// </summary>
        public const string StorageLocationMax1000Characters = "StorageLocationMax1000Characters";

        /// <summary>
        /// The node name must be unique.
        /// </summary>
        public const string NodeNameMustBeUnique = "NodeNameMustBeUnique";

        /// <summary>
        /// The node name required.
        /// </summary>
        public const string NodeNameRequired = "NodeNameRequired";

        /// <summary>
        /// The node type required.
        /// </summary>
        public const string NodeTypeRequired = "NodeTypeRequired";

        /// <summary>
        /// The segment required.
        /// </summary>
        public const string SegmentRequired = "SegmentRequired";

        /// <summary>
        /// The operator required.
        /// </summary>
        public const string OperatorRequired = "OperatorRequired";

        /// <summary>
        /// The entity does not exist.
        /// </summary>
        public const string NodeTypeDoesNotExist = "NodeTypeDoesNotExist";

        /// <summary>
        /// The entity does not exist.
        /// </summary>
        public const string OperatorDoesNotExist = "OperatorDoesNotExist";

        /// <summary>
        /// The entity does not exist.
        /// </summary>
        public const string SegmentDoesNotExist = "SegmentDoesNotExist";

        /// <summary>
        /// The sap code required.
        /// </summary>
        public const string SapCodeRequired = "SapCodeRequired";

        /// <summary>
        /// The type required.
        /// </summary>
        public const string TypeRequired = "TypeRequired";

        /// <summary>
        /// The node should have atleast one store.
        /// </summary>
        public const string NodeShouldHaveAtleastOneStore = "NodeShouldHaveAtleastOneStore";

        /// <summary>
        /// The store should have atleast one product.
        /// </summary>
        public const string StoreShouldHaveAtLeastOneProduct = "StoreShouldHaveAtLeastOneProduct";

        /// <summary>
        /// The storage name must be unique.
        /// </summary>
        public const string StorageNameMustBeUnique = "StorageNameMustBeUnique";

        /// <summary>
        /// The entity name must be unique.
        /// </summary>
        public const string EntityNameMustBeUnique = "EntityNameMustBeUnique";

        /// <summary>
        /// The duplicate identity insert.
        /// </summary>
        public const string DuplicateEntityInsert = "DuplicateEntityInsert";

        /// <summary>
        /// The entity not found.
        /// </summary>
        public const string EntityNotFound = "EntityNotFound";

        /// <summary>
        /// The entity not exists.
        /// </summary>
        public const string EntityNotExists = "EntityNotExists";

        /// <summary>
        /// The category grouper is required.
        /// </summary>
        public const string CategoryGrouperIsRequired = "CategoryGrouperIsRequired";

        /// <summary>
        /// The duplicate identity insert.
        /// </summary>
        public const string NotNullConstraintsError = "NotNullConstraintsError";

        /// <summary>
        /// The category identifier required.
        /// </summary>
        public const string CategoryIdRequired = "CategoryIdRequired";

        /// <summary>
        /// The element status required.
        /// </summary>
        public const string ElementStatusRequired = "ElementStatusRequired";

        /// <summary>
        /// The node is active required.
        /// </summary>
        public const string NodeMustBeActive = "NodeMustBeActive";

        /// <summary>
        /// The node send to sap required.
        /// </summary>
        public const string NodeSendToSapRequired = "NodeSendToSapRequired";

        /// <summary>
        /// The node storage location must be active.
        /// </summary>
        public const string NodeStorageLocationMustBeActive = "NodeStorageLocationMustBeActive";

        /// <summary>
        /// The node storage location send to sap required.
        /// </summary>
        public const string NodeStorageLocationSendToSapRequired = "NodeStorageLocationSendToSapRequired";

        /// <summary>
        /// The product must be active.
        /// </summary>
        public const string ProductMustBeActive = "ProductMustBeActive";

        /// <summary>
        /// The product identifier required.
        /// </summary>
        public const string ProductIdRequired = "ProductIdRequired";

        /// <summary>
        /// The node storage location identifier required.
        /// </summary>
        public const string NodeStorageLocationIdRequired = "NodeStorageLocationIdRequired";

        /// <summary>
        /// The invalid node identifier.
        /// </summary>
        public const string InvalidNodeId = "InvalidNodeId";

        /// <summary>
        /// The invalid node storage locations.
        /// </summary>
        public const string InvalidNodeStorageLocations = "InvalidNodeStorageLocations";

        /// <summary>
        /// The invalid group type.
        /// </summary>
        public const string InvalidGroupType = "InvalidGroupType";

        /// <summary>
        /// The invalid product locations.
        /// </summary>
        public const string InvalidProductLocations = "InvalidProductLocations";

        /// <summary>
        /// The node storage location name required.
        /// </summary>
        public const string NodeStorageLocationNameRequired = "NodeStorageLocationNameRequired";

        /// <summary>
        /// The one or more storage location does not exists.
        /// </summary>
        public const string OneOrMoreStorageLocationDoesNotExists = "OneOrMoreStorageLocationDoesNotExists";

        /// <summary>
        /// The one or more product does not belong to database storage location.
        /// </summary>
        public const string OneOrMoreProductDoesNotBelongToStorageLocation = "OneOrMoreProductDoesNotBelongToStorageLocation";

        /// <summary>
        /// The one or more product does not belog to datatable storage location.
        /// </summary>
        public const string OneOrMoreProductDoesNotBelongToPassedStorageLocation = "OneOrMoreProductDoesNotBelongToPassedStorageLocation";

        /// <summary>
        /// The duplicate node storage location identifier.
        /// </summary>
        public const string DuplicateNodeStorageLocationId = "DuplicateNodeStorageLocationId";

        /// <summary>
        /// The duplicate storage location product identifier.
        /// </summary>
        public const string DuplicateStorageLocationProductId = "DuplicateStorageLocationProductId";

        /// <summary>
        /// The category not found.
        /// </summary>
        public const string CategoryNotExists = "CategoryNotExists";

        /// <summary>
        /// The category element not found.
        /// </summary>
        public const string CategoryElementNotExists = "CategoryElementNotExists";

        /// <summary>
        /// The node does not exists.
        /// </summary>
        public const string NodeDoesNotExists = "NodeDoesNotExists";

        /// <summary>
        /// The ownership node dose not exists.
        /// </summary>
        public const string OwnershipNodeDoseNotExists = "OwnershipNodeDoesNotExists";

        /// <summary>
        /// The element does not exists message.
        /// </summary>
        public const string ElementDoesNotExist = "ElementDoesNotExist";

        /// <summary>
        /// The Category does not exist message.
        /// </summary>
        public const string CategoryNotExist = "CategoryNotExist";

        /// <summary>
        /// The annulation does not exist.
        /// </summary>
        public const string AnnulationDoesNotExist = "AnnulationDoesNotExist";

        /// <summary>
        /// The annulation exists.
        /// </summary>
        public const string AnnulationExists = "AnnulationExists";

        /// <summary>
        /// The node create success.
        /// </summary>
        public const string NodeCreateSuccess = "NodeCreateSuccess";

        /// <summary>
        /// The node update message.
        /// </summary>
        public const string NodeUpdateSuccess = "NodeUpdateSuccess";

        /// <summary>
        /// The node not found.
        /// </summary>
        public const string NodeTypeNotFound = "NodeTypeNotFound";

        /// <summary>
        /// The ownership node update success message.
        /// </summary>
        public const string OwnershipNodeUpdatedSuccessfully = "OwnershipNodeUpdatedSuccessfully";

        /// <summary>
        /// The ownership node update success message.
        /// </summary>
        public const string DeltaNodeUpdatedSuccessfully = "DeltaNodeUpdatedSuccessfully";

        /// <summary>
        /// The ownership node update failure message.
        /// </summary>
        public const string OwnershipNodeUpdateFailure = "OwnershipNodeUpdateFailure";

        /// <summary>
        /// The invalid request status.
        /// </summary>
        public const string InvalidRequestStatus = "InvalidRequestStatus";

        /// <summary>
        /// The node storage location product updated successfully.
        /// </summary>
        public const string NodeStorageLocationProductUpdatedSuccessfully = "NODESTORAGELOCATIONPRODUCT_UPDATE_SUCCESS";

        /// <summary>
        /// The node storage location product owners updated successfully.
        /// </summary>
        public const string NodeStorageLocationProductOwnerUpdatedSuccessfully = "NODESTORAGELOCATIONPRODUCTOWNER_UPDATE_SUCCESS";

        /// <summary>
        /// Invalid priority.
        /// </summary>
        public const string InvalidPriority = "INVALID_PRIORITY";

        /// <summary>
        /// The node product does not exists.
        /// </summary>
        public const string NodeStorageLocationProductDoesNotExists = "NODESTORAGELOCATIONPRODUCT_NOTEXISTS";

        /// <summary>
        /// The node product does not exists.
        /// </summary>
        public const string NodeStorageLocationProductOwnerDoesNotExists = "NODESTORAGELOCATIONPRODUCTOWNER_NOTEXISTS";

        /// <summary>
        /// The category create message.
        /// </summary>
        public const string CategoryCreatedSuccessfully = "CategoryCreatedSuccessfully";

        /// <summary>
        /// The category update message.
        /// </summary>
        public const string CategoryUpdateSuccessfully = "CategoryUpdateSuccessfully";

        /// <summary>
        /// The Annulation create message.
        /// </summary>
        public const string AnnulationCreatedSuccessfully = "AnnulationCreatedSuccessfully";

        /// <summary>
        /// The Product create message.
        /// </summary>
        public const string ProductCreatedSuccessfully = "ProductCreatedSuccessfully";

        /// <summary>
        /// The Product update message.
        /// </summary>
        public const string ProductUpdatedSuccessfully = "ProductUpdatedSuccessfully";

        /// <summary>
        /// The annulation updated successfully.
        /// </summary>
        public const string AnnulationUpdatedSuccessfully = "AnnulationUpdatedSuccessfully";

        /// <summary>
        /// The category create message.
        /// </summary>
        public const string CategoryElementCreatedSuccessfully = "CategoryElementCreatedSuccessfully";

        /// <summary>
        /// The category update message.
        /// </summary>
        public const string CategoryElementUpdatedSuccessfully = "CategoryElementUpdatedSuccessfully";

        /// <summary>
        /// The node should have atleast one store.
        /// </summary>
        public const string HomologationShouldHaveAtleastOneGroup = "HOMOLOGATION_CREATE_GROUPVALIDATION";

        /// <summary>
        /// Homologation groups should not be repeated.
        /// </summary>
        public const string HomologationGroupShouldNotRepeat = "HOMOLOGATION_DUPLICATEGROUPTYPEID";

        /// <summary>
        /// Homologation object  should not be repeated.
        /// </summary>
        public const string HomologationObjectShouldNotRepeat = "HOMOLOGATION_DUPLICATEOBJECT";

        /// <summary>
        /// The source value used by the mapping should be unique for the same system.
        /// </summary>
        public const string SourceValueShouldBeUnique = "HOMOLOGATION_DUPLICATESOURCEVALUE";

        /// <summary>
        /// The destination value used by the mapping should be unique for the same system.
        /// </summary>
        public const string DestinationValueShouldBeUnique = "HOMOLOGATION_DUPLICATEDESTINATIONVALUE";

        /// <summary>
        /// The node should have atleast one homologation objects.
        /// </summary>
        public const string HomologationShouldHaveAtleastOneHomologationObjects = "HOMOLOGATION_CREATE_OBJECTSVALIDATION";

        /// <summary>
        /// The node should have atleast one data mapping.
        /// </summary>
        public const string HomologationShouldHaveAtleastOneDataMapping = "HOMOLOGATION_CREATE_MAPPINGVALIDATION";

        /// <summary>
        /// The homologation create success.
        /// </summary>
        public const string HomologationCreateSuccess = "HOMOLOGATION_CREATE_SUCCESS";

        /// <summary>
        /// The ticket create success.
        /// </summary>
        public const string TicketCreateSuccess = "TICKET_CREATE_SUCCESS";

        /// <summary>
        /// The ticket create success.
        /// </summary>
        public const string NodeOwnershipPublishSuccess = "OWNERSHIP_PUBLISH_SUCCESS";

        /// <summary>
        /// Homologation Does not Exists.
        /// </summary>
        public const string HomologationDoesNotExists = "HOMOLOGATION_NOTFOUND";

        /// <summary>
        /// Homologation input and output system must be different.
        /// </summary>
        public const string HomologationInputOutputDifferent = "HOMOLOGATION_CREATE_SAMESYSTEMVALIDATION";

        /// <summary>
        /// The homologation group does not exists.
        /// </summary>
        public const string HomologationGroupDoesNotExists = "HOMOLOGATION_GROUP_NOTFOUND";

        /// <summary>
        /// The source value required.
        /// </summary>
        public const string SourceValueRequired = "HOMOLOGATION_SOURCEVALUE_REQUIREDVALIDATION";

        /// <summary>
        /// The source value maximum length of 100 characters.
        /// </summary>
        public const string SourceValueMaxLength100 = "HOMOLOGATION_SOURCEVALUE_LENGTHVALIDATION";

        /// <summary>
        /// The destination value required.
        /// </summary>
        public const string DestinationValueRequired = "HOMOLOGATION_DESTINATIONVALUE_REQUIREDVALIDATION";

        /// <summary>
        /// The destination value maximum length of 100 characters.
        /// </summary>
        public const string DestinationValueMaxLength100 = "HOMOLOGATION_DESTINATIONVALUE_LENGTHVALIDATION";

        /// <summary>
        /// The destination value required.
        /// </summary>
        public const string HomologationObjectTypeIdRequired = "HOMOLOGATION_SOURCEOBJECTNAME_REQUIREDVALIDATION";

        /// <summary>
        /// The source value maximum length of 30 characters.
        /// </summary>
        public const string SourceObjectNameMaxLength30 = "HOMOLOGATION_SOURCEOBJECTNAME_LENGTHVALIDATION";

        /// <summary>
        /// The input or output system type must be TRUE.
        /// </summary>
        public const string InputOrOutputMustBeTrue = "HOMOLOGATION_CREATE_SYSTEMSVALIDATION";

        /// <summary>
        /// The input or output system type must be TRUE.
        /// </summary>
        public const string HomologationAlreadyExists = "HOMOLOGATION_SAMESOURCEANDDESTINATION";

        /// <summary>
        /// The input type is invalid.
        /// </summary>
        public const string InvalidInputType = "InvalidInputType";

        /// <summary>
        /// This allow letters space regex.
        /// </summary>
        public const string AllowLettersWithOutSpaceRegex = "^[A-Za-zÑÁÉÍÓÚñáéíóúü]+$";

        /// <summary>
        /// The allow letters space message.
        /// </summary>
        public const string AllowLettersWithOutSpaceRegexMessage = "HOMOLOGATION_SOURCEOBJECTNAME_FORMATVALIDATION";

        /// <summary>
        /// The group type id required.
        /// </summary>
        public const string GroupTypeIdRequired = "HOMOLOGATION_DATASOURCEID_REQUIREDVALIDATION";

        /// <summary>
        /// The source system id required.
        /// </summary>
        public const string SourceSystemIdRequired = "HOMOLOGATION_SOURCESYSTEMID_REQUIREDVALIDATION";

        /// <summary>
        /// The destination system id required.
        /// </summary>
        public const string DestinationSystemIdRequired = "HOMOLOGATION_DESTINATIONSYSTEMID_REQUIREDVALIDATION";

        /// <summary>
        /// The source system name required.
        /// </summary>
        public const string SourceSystemNameRequired = "4009-El nombre del sistema origen es obligatorio";

        /// <summary>
        /// The event type required.
        /// <summary>
        public const string EventTypeRequired = "4010-El tipo de evento es obligatorio";

        /// <summary>
        /// The inventory identifier required.
        /// </summary>
        public const string InventoryIdRequired = "4011-El identificador del inventario es obligatorio";

        /// <summary>
        /// The inventory date required.
        /// </summary>
        public const string InventoryDateRequired = "4012-La fecha del inventario es obligatoria";

        /// <summary>
        /// The node identifier required.
        /// </summary>
        public const string NodeIdRequired = "4013-El identificador del nodo es obligatorio";

        /// <summary>
        /// The products required.
        /// </summary>
        public const string ProductsRequired = "4027-Los productos del inventario son  obligatorios";

        /// <summary>
        /// The creation date required.
        /// </summary>
        /// NOTE : THIS NEEDS TO BE REPLACE.
        public const string CreationDateRequired = "To be filled";

        /// <summary>
        /// The scenario length exceeded.
        /// </summary>
        public const string ScenarioLengthExceeded = "El escenario puede contener máximo 50 caracteres";

        /// <summary>
        /// The observation length exceeded.
        /// </summary>
        public const string ObservationLengthExceeded = "Las observaciones pueden contener máximo 150 caracteres";

        /// <summary>
        /// The event type length exceeded.
        /// </summary>
        public const string EventTypeLengthExceeded = "El tipo de evento puede contener máximo 10 caracteres";

        /// <summary>
        /// The products identifier required.
        /// </summary>
        public const string ProductsIdRequired = "El identificador del producto es obligatorio";

        /// <summary>
        /// The products type required.
        /// </summary>
        public const string ProductsTypeRequired = "El identificador del tipo de producto es obligatorio";

        /// <summary>
        /// The products volume required.
        /// </summary>
        public const string ProductsVolumeRequired = "El volumen es obligatorio";

        /// <summary>
        /// The attribute description required.
        /// </summary>
        public const string AttributeIdRequired = "El identificador del atributo es obligatorio";

        /// <summary>
        /// The attribute value required.
        /// </summary>
        public const string AttributeValueRequired = "El valor del atributo es obligatorio";

        /// <summary>
        /// The value attribute unit required.
        /// </summary>
        public const string ValueAttributeUnitRequired = "La unidad de medida del atributo es obligatoria";

        /// <summary>
        /// The attribute description required.
        /// </summary>
        public const string AttributeDescriptionRequired = "La descripción del atributo puede contener máximo 150 caracteres";

        /// <summary>
        /// The owner identifier required.
        /// </summary>
        public const string OwnerIdRequired = "El identificador del propietario es obligatorio";

        /// <summary>
        /// The ownership value required.
        /// </summary>
        public const string OwnershipValueRequired = "El valor de la propiedad es obligatorio";

        /// <summary>
        /// The ownership value unit required.
        /// </summary>
        public const string OwnershipValueUnitRequired = "La unidad del valor de la propiedad es obligatoria";

        /// <summary>
        /// The node connection created successfully.
        /// </summary>
        public const string NodeConnectionCreatedSuccessfully = "NODECONNECTION_CREATE_SUCCESS";

        /// <summary>
        /// The node connection updated successfully.
        /// </summary>
        public const string NodeConnectionUpdatedSuccessfully = "NODECONNECTION_UPDATE_SUCCESS";

        /// <summary>
        /// The node connection deleted successfully.
        /// </summary>
        public const string NodeConnectionDeletedSuccessfully = "NODECONNECTION_DELETE_SUCCESS";

        /// <summary>
        /// The node cost center created successfully.
        /// </summary>
        public const string NodeCosCenterCreatedSuccessfully = "NODECOSTCENTER_CREATE_SUCCESS";

        /// <summary>
        /// The node cost center updated successfully.
        /// </summary>
        public const string NodeCosCenterUpdatedSuccessfully = "NODECOSTCENTER_UPDATE_SUCCESS";

        /// <summary>
        /// The node cost center created successfully.
        /// </summary>
        public const string NodeCostCenterDeletedSuccessfully = "NODECOSTCENTER_DELETE_SUCCESS";

        /// <summary>
        /// The node connection description lenght validation.
        /// </summary>
        public const string NodeConnectionDescriptionLengthValidation = "NODECONNECTION_DESCRIPTION_LENGTHVALIDATION";

        /// <summary>
        /// The node connection status required validation.
        /// </summary>
        public const string NodeConnectionStatusRequiredValidation = "NODECONNECTION_STATUS_REQUIREDVALIDATION";

        /// <summary>
        /// The node connection source node required validation.
        /// </summary>
        public const string NodeConnectionSourceNodeRequiredValidation = "NODECONNECTION_SOURCENODEID_REQUIREDVALIDATION";

        /// <summary>
        /// The node connection destination node required validation.
        /// </summary>
        public const string NodeConnectionDestinationNodeRequiredValidation = "NODECONNECTION_DESTINATIONNODEID_REQUIREDVALIDATION";

        /// <summary>
        /// The node connection already exists.
        /// </summary>
        public const string NodeConnectionAlreadyExists = "NODECONNECTION_CONNECTION_ALREADYEXISTS";

        /// <summary>
        /// The node connection does not exists.
        /// </summary>
        public const string NodeConnectionDoesNotExists = "NODECONNECTION_CONNECTION_NOTEXISTS";

        /// <summary>
        /// The node connection product does not exists.
        /// </summary>
        public const string NodeConnectionProductDoesNotExists = "NODECONNECTION_PRODUCT_NOTEXISTS";

        /// <summary>
        /// The node connection not found.
        /// </summary>
        public const string NodeConnectionNotFound = "NODECONNECTION_CONNECTION_NOTFOUND";

        /// <summary>
        /// The node connection delete conflict.
        /// </summary>
        public const string NodeConnectionDeleteConflict = "NODECONNECTION_DELETE_CONFLICT";

        /// <summary>
        /// The source node identifier not found.
        /// </summary>
        public const string SourceNodeIdentifierNotFound = "NODECONNECTION_SOURCENODEID_NOTFOUND";

        /// <summary>
        /// The destination node identifier not found.
        /// </summary>
        public const string DestinationNodeIdentifierNotFound = "NODECONNECTION_DESTINATIONNODEID_NOTFOUND";

        /// <summary>
        /// The invalid node status for connection.
        /// </summary>
        public const string InvalidNodeStatusForConnection = "INVALID_NODE_STATUS_FOR_CONNECTION";

        /// <summary>
        /// The source and destination node can not same.
        /// </summary>
        public const string SourceAndDestinationNodeCanNotSame = "NODECONNECTION_SOURCE_DESTINATION_CANNOTSAME";

        /// <summary>
        /// The allow alphanumeric with special characters and space message.
        /// </summary>
        public const string InvalidEventType = "El tipo de evento solo admite letras";

        /// <summary>
        /// The movement identifier is mandatory.
        /// </summary>
        public const string MovementIdentifierRequired = "El identificador del movimiento es obligatorio";

        /// <summary>
        /// The movement type identifier is mandatory.
        /// </summary>
        public const string MovementTypeRequired = "El identificador del tipo de movimiento es obligatorio";

        /// <summary>
        /// The operational date is mandatory.
        /// </summary>
        public const string OperationDateRequired = "La fecha operacional es obligatoria";

        /// <summary>
        /// The node connection product required.
        /// </summary>
        public const string NodeConnectionProductRequired = "NODECONNECTION_PRODUCTID_REQUIREDVALIDATION";

        /// <summary>
        /// The node connection product owner required.
        /// </summary>
        public const string NodeConnectionProductOwnerRequired = "OWNERID_REQUIREDVALIDATION";

        /// <summary>
        /// The product owner ship total value validation.
        /// </summary>
        public const string ProductOwnerShipTotalValueValidation = "OWNERSHIPVALUE_TOTALVALIDATION";

        /// <summary>
        /// The product owner ship required.
        /// </summary>
        public const string ProductOwnerShipRequired = "OWNERSHIPPERCENTAGE_REQUIREDVALIDATION";

        /// <summary>
        /// The node connection product updated successfully.
        /// </summary>
        public const string NodeConnectionProductUpdatedSuccessfully = "NODECONNECTIONPRODUCT_UPDATE_SUCCESS";

        /// <summary>
        /// The node connection product updated successfully.
        /// </summary>
        public const string NodeStorageLocationProductOwnersUpdatedSuccessfully = "NODESTORAGELOCATIONPRODUCTOWNERS_UPDATE_SUCCESS";

        /// <summary>
        /// The node connection product owners updated successfully.
        /// </summary>
        public const string NodeConnectionProductOwnersUpdatedSuccessfully = "NODECONNECTIONPRODUCTOWNERS_UPDATE_SUCCESS";

        /// <summary>
        /// The period is mandatory.
        /// </summary>
        public const string PeriodIsMandatory = "El periodo es obligatorio";

        /// <summary>
        /// The movement start time is mandatory.
        /// </summary>
        public const string MovementStartTimeIsMandatory = "La hora de inicio del movimiento es obligatoria";

        /// <summary>
        /// The movement end time is mandatory.
        /// </summary>
        public const string MovementEndTimeIsMandatory = "La hora final del movimiento es obligatoria";

        /// <summary>
        /// The Classification is mandatory.
        /// </summary>
        public const string MovementClassificationIsMandatory = "La clasificación del movimiento es obligatoria";

        /// <summary>
        /// The net standard volume is mandatory.
        /// </summary>
        public const string NetStandardVolumeIsMandatory = "El volumen neto es obligatorio";

        /// <summary>
        /// The Regular Expression for letters only.
        /// </summary>
        public const string AllowLettersOnly = "^[A-Za-z]+$";

        /// <summary>
        /// The movement classification length max 30 characters.
        /// </summary>
        public const string ClassificationLengthExceeded = "La clasificación del movimiento puede contener máximo 30 caracteres";

        /// <summary>
        /// The movement classification format is invalid.
        /// </summary>
        public const string InvalidClassificationMessage = "La clasificación del movimiento solo admite letras";

        /// <summary>
        /// Gets the Movement SOURCE NODEIDREQUIREDVALIDATION.
        /// </summary>
        /// <value>
        /// The MOVEMENT SOURCENODEID REQUIREDVALIDATION.
        /// </value>
        public const string NodeConnectionSourceNodeIdRequired = "El identificador del nodo origen es obligatorio";

        /// <summary>
        /// Gets the MOVEMENT SOURCE PRODUCTTYPE REQUIREDVALIDATION REQUIRED VALIDATION.
        /// </summary>
        /// <value>
        /// The MOVEMENT SOURCE PRODUCT TYPEID_REQUIREDVALIDATION REQUIRED VALIDATION.
        /// </value>
        public const string MovementSourceProductTypeIdRequired = "El identificador del tipo de producto origen es obligatorio";

        /// <summary>
        /// Gets the Movement destination required validation.
        /// </summary>
        /// <value>
        /// The MOVEMENT DESTINATION REQUIRED VALIDATION.
        /// </value>
        public const string NodeConnectionDestinationNodeIdRequired = "5082-El identificador del nodo destino es obligatorio";

        /// <summary>
        /// The register files uploded successfully.
        /// </summary>
        public const string RegisterFilesUploadedSuccessfully = "RegisterFilesUploadedSuccessfully";

        /// <summary>
        /// The register file name required.
        /// </summary>
        public const string RegisterFileNameRequired = "RegisterFileNameRequired";

        /// <summary>
        /// The register file action type required.
        /// </summary>
        public const string RegisterFileActionTypeRequired = "RegisterFileActionTypeRequired";

        /// <summary>
        /// The register file BLOB path required.
        /// </summary>
        public const string RegisterFileBlobPathRequired = "RegisterFileBlobPathRequired";

        /// <summary>
        /// The register file BLOB path maximum length500.
        /// </summary>
        public const string RegisterFileBlobPathMaxLength500 = "RegisterFileBlobPathMaxLength500";

        /// <summary>
        /// The register file upload file identifier required.
        /// </summary>
        public const string RegisterFileUploadFileIdRequired = "RegisterFileUploadFileIdRequired";

        /// <summary>
        /// The register file upload identifier must be unique.
        /// </summary>
        public const string RegisterFileUploadIdMustBeUnique = "RegisterFileUploadIdMustBeUnique";

        /// <summary>
        /// The unique identifier regex.
        /// </summary>
        public const string GuidRegex = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";

        /// <summary>
        /// The only unique identifier is allowed.
        /// </summary>
        public const string OnlyGuidIsAllowed = "OnlyGuidIsAllowed";

        /// <summary>
        /// The homologation group does not exists.
        /// </summary>
        public const string AssociationSuccess = "NODE_ASSOCIATION_SUCCESS";

        /// <summary>
        /// The operational cut off save success.
        /// </summary>
        public const string OperationalCutOffSaveSuccess = "OperationalCutOffSaveSuccess";

        /// <summary>
        /// The comment maximum length1000.
        /// </summary>
        public const string CommentMaxLength1000 = "CommentMaxLength1000";

        /// <summary>
        /// The comments required.
        /// </summary>
        public const string CommentsRequired = "CommentsRequired";

        /// <summary>
        /// The ticket does not exist.
        /// </summary>
        public const string TicketNotExists = "TicketNotExists";

        /// <summary>
        /// The tank name length exceeded.
        /// </summary>
        public const string TankNameLengthExceeded = "5083-El Tanque puede contener máximo 20 caracteres";

        /// <summary>
        /// The transformation create message.
        /// </summary>
        public const string TransformationCreatedSuccessfully = "TransformationCreatedSuccessfully";

        /// <summary>
        /// The transformation deleted successfully.
        /// </summary>
        public const string TransformationDeletedSuccessfully = "TransformationDeletedSuccessfully";

        /// <summary>
        /// The transformation does not exists.
        /// </summary>
        public const string TransformationDoesNotExists = "TRANSFORMATION_NOTEXISTS";

        /// <summary>
        /// The transformation update message.
        /// </summary>
        public const string TransformationUpdatedSuccessfully = "TransformationUpdatedSuccessfully";

        /// <summary>
        /// The ticket create success.
        /// </summary>
        public const string ErrorCommentUpdatedSuccessfully = "ErrorCommentUpdatedSuccessfully";

        /// <summary>
        /// The pending transaction not exists.
        /// </summary>
        public const string PendingTransactionNotExists = "PendingTransactionNotExists";

        /// <summary>
        /// The ownership node for approval sent successfully.
        /// </summary>
        public const string OwnershipNodeForApprovalSentSuccessfully = "OwnershipNodeForApprovalSentSuccessfully";

        /// <summary>
        /// The ownership node for approval sent successfully.
        /// </summary>
        public const string OwnershipNodeForApprovalFailed = "OwnershipNodeForApprovalFailed";

        /// <summary>
        /// The homologation deleted successfully.
        /// </summary>
        public const string HomologationGroupDeletedSuccessfully = "HOMOLOGATION_GROUP_DELETE_SUCCESS";

        /// <summary>
        /// The no data found for logistic file.
        /// </summary>
        public const string NoDataFoundForLogisticFile = "No hay información logística para los criterios dados.";

        /// <summary>
        /// The no inventory data found for logistic file.
        /// </summary>
        public const string NoInventoryDataFoundForLogisticFile = "No hay información logística de inventarios para los criterios dados.";

        /// <summary>
        /// The logistic file static message.
        /// </summary>
        public const string LogisticFileStaticMessage = "Información operativa con propiedad para el segmento de transporte.";

        /// <summary>
        /// The no sap homologation found for movement type.
        /// </summary>
        public const string NoSapHomologationFoundForMovementType = "No se encontró una homologación entre TRUE y SIV, por favor revise la homologación.";

        /// <summary>
        /// The invalid combination to siv movement.
        /// </summary>
        public const string InvalidCombinationToSivMovement = "La combinación de producto, centro logístico y almacén es inválida y no está considerada para envío a SIV";

        /// <summary>
        /// The no transfer with algorithm identifier message.
        /// </summary>
        public const string NoTransferWithAlgorithmIdMessage = "Si la conexión no es un punto de transferencia, no debe tener identificador de algoritmo";

        /// <summary>
        /// The transfer with no algorithm identifier message.
        /// </summary>
        public const string TransferWithNoAlgorithmIdMessage = "Si la conexión es un punto de transferencia debe tener un identificador de algoritmo";

        /// <summary>
        /// The homologation create success.
        /// </summary>
        public const string HomologationUpdateSuccess = "HOMOLOGATION_UPDATE_SUCCESS";

        /// <summary>
        /// Gets the EVENT Type Id REQUIREDVALIDATION.
        /// </summary>
        public const string EventTypeIdRequired = "El evento de propiedad es obligatorio";

        /// <summary>
        /// Gets the EVENT SourceNodeId REQUIREDVALIDATION.
        /// </summary>
        public const string SourceNodeIdRequired = "El nodo origen es obligatorio";

        /// <summary>
        /// Gets the EVENT DestinationNodeId REQUIREDVALIDATION.
        /// </summary>
        public const string DestinationNodeIdRequired = "El nodo destino es obligatorio";

        /// <summary>
        /// Gets the EVENT SourceProductId REQUIREDVALIDATION.
        /// </summary>
        public const string EventSourceProductIdRequired = "El producto origen es obligatorio";

        /// <summary>
        /// Gets the EVENT DestinationProductId REQUIREDVALIDATION.
        /// </summary>
        public const string EventDestinationProductIdRequired = "El producto destino es obligatorio";

        /// <summary>
        /// Gets the EVENT StartDate REQUIREDVALIDATION.
        /// </summary>
        public const string EventStartDateIsMandatory = "La fecha inicial es obligatoria";

        /// <summary>
        /// Gets the EVENT EndDate REQUIREDVALIDATION.
        /// </summary>
        public const string EventEndDateIsMandatory = "La fecha final es obligatoria";

        /// <summary>
        /// Gets the EVENT OwnerId REQUIREDVALIDATION.
        /// </summary>
        public const string EventOwnerIdRequired = "El propietario es obligatorio";

        /// <summary>
        /// Gets the EVENT Volumne REQUIREDVALIDATION.
        /// </summary>
        public const string EventVolumeRequired = "El valor del evento es obligatorio";

        /// <summary>
        /// Gets the EVENT UnitId REQUIREDVALIDATION.
        /// </summary>
        public const string EventUnitIdRequired = "La unidad es obligatoria";

        /// <summary>
        /// Gets the Contract Document REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellDocumentRequiredValidation = "El documento es obligatorio";

        /// <summary>
        /// Gets the Contract Position REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellPositionRequiredValidation = "El position es obligatorio";

        /// <summary>
        /// Gets the Contract Type REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellTypeRequiredValidation = "El tipo es obligatorio";

        /// <summary>
        /// Gets the Contract Product REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellProductRequiredValidation = "El tipo es obligatorio";

        /// <summary>
        /// Gets the Contract Commercial REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellCommercialRequiredValidation = "El comercial es obligatorio";

        /// <summary>
        /// Gets the Contract Value REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellValueRequiredValidation = "El valor es obligatorio";

        /// <summary>
        /// Gets the Contract Unit REQUIREDVALIDATION.
        /// </summary>
        public const string PurchaseAndSellUnitRequiredValidation = "Identificador de la unidad no encontrado";

        /// <summary>
        /// Gets the Contract not found.
        /// </summary>
        public const string FrequencyRequiredValidation = "La frecuencia es obligatoria";

        /// <summary>
        /// Gets The ownership node reopened successfully REQUIREDVALIDATION.
        /// </summary>
        public const string OwnershipNodeReopenedSuccessfully = "El nodo de ticket de propiedad se ha vuelto a abrir correctamente";

        /// <summary>
        /// Gets The ownership node not found REQUIREDVALIDATION.
        /// </summary>
        public const string OwnershipNodeNotFound = "El nodo de propiedad para el ticketid no existe o no está aprobado";

        /// <summary>
        /// The rejected status comment needed REQUIREDVALIDATION.
        /// </summary>
        public const string RejectedStatusCommentNeeded = "Es obligatorio un comentario para cambio de estado del nodo.";

        /// <summary>
        /// The invalid node state approval REQUIREDVALIDATION.
        /// </summary>
        public const string InvalidNodeStateApproval = "El nodo tiene un estado no válido para aprobación, verificar que no haya cambios pendientes de publicación o publicarlos antes de aprobación.";

        /// <summary>
        /// The node identifier mandatory REQUIREDVALIDATION.
        /// </summary>
        public const string OwnershipNodeIdMandatory = "El parámetro propiedadNodoId es obligatorio.";

        /// <summary>
        /// The approver alias mandatory REQUIREDVALIDATION.
        /// </summary>
        public const string ApproverAliasMandatory = "El parámetro ApproverAlias es obligatorio.";

        /// <summary>
        /// The status mandatory REQUIREDVALIDATION.
        /// </summary>
        public const string StatusMandatory = "El parámetro Estado es obligatorio.";

        /// <summary>
        /// The node relationship not found.
        /// </summary>
        public const string NodeRelationshipNotFound = "NODE_RELATIONSHIP_NOTFOUND";

        /// <summary>
        /// The node relationship created successfully.
        /// </summary>
        public const string NodeRelationshipCreatedSuccessfully = "NODE_RELATIONSHIP_CREATE_SUCCESS";

        /// <summary>
        /// The node relationship updated successfully.
        /// </summary>
        public const string NodeRelationshipUpdatedSuccessfully = "NODE_RELATIONSHIP_UPDATE_SUCCESS";

        /// <summary>
        /// The node relationship deleted successfully.
        /// </summary>
        public const string NodeRelationshipDeletedSuccessfully = "NODE_RELATIONSHIP_DELETE_SUCCESS";

        /// <summary>
        /// The transfer point cannot be null or empty.
        /// </summary>
        public const string TransferPointCannotBeNullOrEmpty = "TRANSFER_POINT_CANNOT_BE_NULL_OR _EMPTY";

        /// <summary>
        /// The movement type cannot be null or empty.
        /// </summary>
        public const string MovementTypeCannotBeNullOrEmpty = "MOVEMENT_TYPE_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The source node cannot be null or empty.
        /// </summary>
        public const string NodeRelationshipIdentityCannotBeNull = "NODE_RELATIONSHIP_IDENTITY_CANNOT_BE_NULL";

        /// <summary>
        /// The source node cannot be null or empty.
        /// </summary>
        public const string SourceNodeCannotBeNullOrEmpty = "SOURCE_NODE_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The desitination node cannot be null or empty.
        /// </summary>
        public const string DestinationNodeCannotBeNullOrEmpty = "DESTINATION_NODE_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The source node type cannot be null or empty.
        /// </summary>
        public const string SourceNodeTypeCannotBeNullOrEmpty = "SOURCE_NODE_TYPE_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The destination node type cannot be null or empty.
        /// </summary>
        public const string DestinationNodeTypeCannotBeNullOrEmpty = "DESTINATION_NODE_TYPE_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The source product cannot be null or empty.
        /// </summary>
        public const string SourceProductCannotBeNullOrEmpty = "SOURCE_PRODUCT_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The source product type cannot be null or empty.
        /// </summary>
        public const string SourceProductTypeCannotBeNullOrEmpty = "SOURCE_PRODUCT_TYPE_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The campo cannot be null or empty.
        /// </summary>
        public const string SourceFieldCannotBeNullOrEmpty = "SOURCE_FIELD_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The agua campo cannot be null or empty.
        /// </summary>
        public const string FieldWaterProductionCannotBeNullOrEmpty = "FIELD_WATER_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The casos correlacionados cannot be null or empty.
        /// </summary>
        public const string RelatedSourceFieldCannotBeNullOrEmpty = "RELATED_SOURCE_FIELD_CANNOT_BENULL_OR_EMPTY";

        /// <summary>
        /// The max200 characters allowed.
        /// </summary>
        public const string Max200CharactersAllowed = "MAXIMUM_200_LENGTHVALIDATION";

        /// <summary>
        /// The max1000 characters allowed.
        /// </summary>
        public const string Max1000CharactersAllowed = "MAXIMUM_1000_LENGTHVALIDATION";

        /// <summary>
        /// The notes cannot be null or empty.
        /// </summary>
        public const string NotesCannotBeNullOrEmpty = "NOTES_CANNOT_BE_NULL_OR_EMPTY";

        /// <summary>
        /// The transfer association duplicate.
        /// </summary>
        public const string TransferAssociationDuplicate = "TRANSFER_ASSOCIATION_DUPLICATE";

        /// <summary>
        /// Gets the Percentage validation message.
        /// </summary>
        /// <value>
        /// The Percentage validation message.
        /// </value>
        public const string PercentageValidationMessage = "El porcentaje debe ser un número entre 0 y 100";

        /// <summary>
        /// Gets the Percentage validation message.
        /// </summary>
        /// <value>
        /// The Percentage validation message.
        /// </value>
        public const string DecimalValidationMessage = "Solo se admiten hasta 16 enteros y hasta 2 decimales";

        /// <summary>
        /// The green color code.
        /// </summary>
        public const string GreenColorCode = "#025449";

        /// <summary>
        /// Gets the events owne r1 requiredvalidation.
        /// </summary>
        /// <value>
        /// The events owne r1 requiredvalidation.
        /// </value>
        public const string Owner1Requiredvalidation = "El propietario 1 es obligatorio";

        /// <summary>
        /// Gets the events owne r2 requiredvalidation.
        /// </summary>
        /// <value>
        /// The events owne r2 requiredvalidation.
        /// </value>
        public const string Owner2Requiredvalidation = "El propietario 2 es obligatorio";

        /// <summary>
        /// The logistic center name not found.
        /// </summary>
        public const string LogisticCenterNameNotFound = "LogisticCenterNameNotFound";

        /// <summary>
        /// The ticket create success.
        /// </summary>
        public const string OwnershipRuleBulkUpdateSuccess = "OWNERSHIPRULE_BULKUPDATE_SUCCESS";

        /// <summary>
        /// Gets the Capacity validation message.
        /// </summary>
        /// <value>
        /// The Capacity validation message.
        /// </value>
        public const string NodeCapacityRequiredValidation = "NODE_CAPACITY_REQUIREDVALIDATION";

        /// <summary>
        /// Gets the Unit validation message.
        /// </summary>
        /// <value>
        /// The Unit validation message.
        /// </value>
        public const string NodeCapacityUnitRequiredValidation = "NODE_CAPACITYUNIT_REQUIREDVALIDATION";

        /// <summary>
        /// The row concurrency conflict.
        /// </summary>
        public const string RowConcurrencyConflict = "RowConcurrencyConflict";

        /// <summary>
        /// The retrigger of the deadletter message.
        /// </summary>
        public const string ReTriggerSuccessful = "ReTriggerSuccessful";

        /// <summary>
        /// The retrigger of the deadletter message.
        /// </summary>
        public const string ReTriggerFailed = "InvalidRecords";

        /// <summary>
        /// The reset critical successful status.
        /// </summary>
        public const string ResetCriticalSuccessful = "ResetCriticalSuccessful";

        /// <summary>
        /// The scenariod ID validation.
        /// </summary>
        public const string ScenarioIdValueRangeFailed = "El escenario suministrado no es válido.";

        /// <summary>
        /// The scenariod ID validation.
        /// </summary>
        public const string BothSourceDestinationMandatory = "Es obligatorio reportar información del origen o del destino. (Ambas no pueden estar vacías).";

        /// <summary>
        /// The DestinationProductId validation.
        /// </summary>
        public const string DestinationProductIdRequired = "El identificador del producto destino es obligatorio.";

        /// <summary>
        /// The DestinationProductId validation.
        /// </summary>
        public const string SourceProductIdRequired = "El identificador del producto origen es obligatorio.";

        /// <summary>
        /// The source transformation duplicate.
        /// </summary>
        public const string SourceTransformationDuplicate = "Ya existe una Transformación origen para los valores ingresados.";

        /// <summary>
        /// Category name validation.
        /// </summary>
        public const string CategoryNameAlreadyExist = "CategoryNameAlreadyExist";

        /// <summary>
        /// Annulation source Exists.
        /// </summary>
        public const string AnnulationSourceExists = "No se puede crear la relación.";

        /// <summary>
        /// Operative Transfer Relationship Exists.
        /// </summary>
        public const string OperativeTransferRelationshipExists = "Error de duplicados.";

        /// <summary>
        /// Element Name Exists.
        /// </summary>
        public const string ElementNameExists = "El nombre del elemento ya existe.";

        /// <summary>
        /// The file upload date range exceeded.
        /// </summary>
        public const string DateRangeExceeded = "6005-El rango de días elegidos debe ser menor a {0} días";

        /// <summary>
        /// The no records found.
        /// </summary>
        public const string NoRecordsFound = "NoRecordsFound";

        /// <summary>
        /// The more than2 records found.
        /// </summary>
        public const string MoreThan2RecordsFound = "MoreThan2RecordsFound";

        /// <summary>
        /// The official information required.
        /// </summary>
        public const string OfficialInformationRequired = "OfficialInformationRequired";

        /// <summary>
        /// Official point error.
        /// </summary>
        public const string OfficialPointError = "OfficialPointError";

        /// <summary>
        /// The backup movement identifier required.
        /// </summary>
        public const string BackupMovementIdRequired = "BackupMovementIdRequired";

        /// <summary>
        /// The same backup movement identifier.
        /// </summary>
        public const string SameBackupMovementId = "SameBackupMovementId";

        /// <summary>
        /// The same global movement identifier.
        /// </summary>
        public const string SameGlobalMovementId = "SameGlobalMovementId";

        /// <summary>
        /// The single movement official point.
        /// </summary>
        public const string SingleMovementOfficialPoint = "SingleMovementOfficialPoint";

        /// <summary>
        /// The single movement stored.
        /// </summary>
        public const string SingleMovementStored = "SingleMovementStored";

        /// <summary>
        /// At least one movement stored.
        /// </summary>
        public const string AtLeastOneMovementStored = "AtLeastOneMovementStored";

        /// <summary>
        /// The movement invalid data type.
        /// </summary>
        public const string MovementInvalidDataType = "AtLeastOneMovementStored";

        /// <summary>
        /// The single movement data not match stored.
        /// </summary>
        public const string SingleMovementDataNotMatchStored = "SingleMovementDataNotMatchStored";

        /// <summary>
        /// The multiple movement data not match stored.
        /// </summary>
        public const string MultipleMovementDataNotMatchStored = "MultipleMovementDataNotMatchStored";

        /// <summary>
        /// The both movement data notvalid.
        /// </summary>
        public const string BothMovementDataNotvalid = "BothMovementDataNotvalid";

        /// <summary>
        /// The cutoff already running.
        /// </summary>
        public const string CutoffAlreadyRunning = "cutoffAlreadyRunning";

        /// <summary>
        /// The delta already running.
        /// </summary>
        public const string DeltaAlreadyRunning = "deltaAlreadyRunning";

        /// <summary>
        /// The single movement not send to sap.
        /// </summary>
        public const string SingleMovementNotSendToSap = "SingleMovementNotSendToSap";

        /// <summary>
        /// The multiple movement not send to sap.
        /// </summary>
        public const string MultipleMovementNotSendToSap = "MultipleMovementNotSendToSap";

        /// <summary>
        /// The deltaExceptions does not exist.
        /// </summary>
        public const string DeltaExceptionsNotExists = "DeltaExceptionsNotExists";

        /// <summary>
        /// The operational cutoff fail.
        /// </summary>
        public const string OperationalCutoffFail = "OperationalCutoffFail";

        /// <summary>
        /// The pending transaction errors do not exist.
        /// </summary>
        public const string PendingTransactionErrorsDoNotExist = "PendingTransactionErrorsDoNotExist";

        /// <summary>
        /// The retry errors do not exist.
        /// </summary>
        public const string RetryErrorsDoNotExist = "RetryErrorsDoNotExist";

        /// <summary>
        /// The blockchain page size exceeded.
        /// </summary>
        public const string InvalidBlockchainPageSize = "InvalidBlockchainPageSize";

        /// <summary>
        /// The invalid blocknumber supplied.
        /// </summary>
        public const string InvalidBlockNumberSupplied = "InvalidBlocknumberSupplied";

        /// <summary>
        /// The invalid transaction hash supplied.
        /// </summary>
        public const string InvalidTransactionHashSupplied = "InvalidTransactionHashSupplied";

        /// <summary>
        /// The transaction hash required.
        /// </summary>
        public const string TransactionHashRequired = "TransactionHashRequired";

        /// <summary>
        /// The block number required.
        /// </summary>
        public const string BlockNumberRequired = "BlockNumberRequired";

        /// <summary>
        /// The version maximum length of 50 characters.
        /// </summary>
        public const string VersionIdMax50Characters = "El identificador del version puede contener máximo 50 caracteres.";

        /// <summary>
        /// The destination system length exceeded.
        /// </summary>
        public const string DestinationSystemLengthExceeded = "El nombre del sistema destino admite hasta 25 caracteres";

        /// <summary>
        /// The node ID validation.
        /// </summary>
        public const string NodeIdValueFailed = "El nodo suministrado no es válido.";

        /// <summary>
        /// The movement type identifier length exceeded.
        /// </summary>
        public const string MovementTypeIdentifierLengthExceeded = "El identificador del tipo de movimiento admite hasta 150 caracteres";

        /// <summary>
        /// The measurement unit is mandatory.
        /// </summary>
        public const string MeasurementUnitRequired = "La unidad de medida es obligatoria";

        /// <summary>
        /// The measurement unit length exceeded.
        /// </summary>
        public const string MeasurementUnitLengthExceeded = "La unidad de medida admite hasta 50 caracteres";

        /// <summary>
        /// The balance status length exceeded.
        /// </summary>
        public const string SapProcessStatusLengthExceeded = "El estado del proceso admite hasta 50 caracteres";

        /// <summary>
        /// The inventory id length exceeded.
        /// </summary>
        public const string InventoryIdLengthExceeded = "Identificador del inventario admite hasta 50 caracteres";

        /// <summary>
        /// The movement identifier length exceeded.
        /// </summary>
        public const string MovementIdentifierLengthExceeded = "El identificador del movimiento admite hasta 50 caracteres";

        /// <summary>
        /// The RPC timeout exception.
        /// </summary>
        public const string RpcTimeoutException = "RpcTimeoutException";

        /// <summary>
        /// The report name required.
        /// </summary>
        public const string ReportNameRequired = "ReportNameRequired";

        /// <summary>
        /// The report already processing.
        /// </summary>
        public const string ReportAlreadyProcessing = "ReportAlreadyProcessing";

        /// <summary>
        /// The Source System required.
        /// </summary>
        public const string ContractSourceSystemRequired = "El origen del sistema es obligatorio";

        /// <summary>
        /// The Source MessageId.
        /// </summary>
        public const string ContractMessageIdRequired = "El Identificador creado por SAP (MessageId) es obligatorio";

        /// <summary>
        /// The create action.
        /// </summary>
        public const string EventSapCreate = "CREAR";

        /// <summary>
        /// The update action.
        /// </summary>
        public const string EventSapUpdate = "MODIFICAR";

        /// <summary>
        /// The percentage out of tolerance.
        /// </summary>
        public const string PercentageOutOfTolerance = "Porcentaje fuera de la tolerancia";

        /// <summary>
        /// The percentage out of tolerance.
        /// </summary>
        public const string StartDateMustBeLessThanEndDate = "START_DATE_MUST_BE_LESS_THAN_END_DATE";

        /// <summary>
        /// The spot orders.
        /// </summary>
        public const string SpotOrders = "ORDENES SPOT";

        /// <summary>
        /// The RecurringOrders.
        /// </summary>
        public const string RecurringOrders = "ORDENES RECURRENTE";

        /// <summary>
        /// The Transfers.
        /// </summary>
        public const string Transfers = "TRASLADOS";

        /// <summary>
        /// The maximum validation number.
        /// </summary>
        public const double MaxNumberValidator = 99999999999999999.99;

        /// <summary>
        /// The minimum validation number.
        /// </summary>
        public const double MinNumberValidator = -99999999999999999.99;

        /// <summary>
        /// The Push Message To Queue create success.
        /// </summary>
        public const string PushMessageOwnerShipCreateSuccess = "PUSH_MESSAGE_CREATE_SUCCESS";

        /// <summary>
        /// The Push Message To Queue create success.
        /// </summary>
        public const string PushMessagebalanceCreateSuccess = "PUSH_MESSAGE_CREATE_SUCCESS";
    }
}
