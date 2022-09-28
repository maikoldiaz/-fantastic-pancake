pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract InventoryProductsFactory is ContractBase(3) {
    mapping(string => string) Transactions;
    mapping(string => InventoryProduct) InventoryProducts;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueInventoryProductLog(
        bytes32 indexed Id,
        bytes32 indexed InventoryProductId,
        int64 indexed InventoryDate,
        uint8 Version,
        int256 Volume,
        uint8 ActionType,
        string Unit,
        string Metadata,
        uint Timestamp
    );

    struct InventoryProduct {
        string InventoryProductId;
        int256 ProductVolume;
        string MeasurementUnit;
        int64 InventoryDate;
        string TransactionId;
        string Metadata;
        bool Exists;
    }

    constructor() public {}

    function Insert(
        string calldata inventoryProductId,
        string calldata transactionId,
        int64 inventoryDate,
        string calldata metadata,
        int256 productVolume,
        string calldata measurementUnit
    )
    external {
        string memory existingTransactionId = Transactions[inventoryProductId];
        require(compareStrings(existingTransactionId, ""), "Inventory product identifier is already present");

        Transactions[inventoryProductId] = transactionId;
        InventoryProducts[transactionId] = InventoryProduct(inventoryProductId, productVolume,
                                                        measurementUnit, inventoryDate, transactionId, metadata, true);
        bytes32 id = keccak256(bytes(transactionId));

        emit TrueLog(GetEventType(), 2, id, "", now);
        emit TrueInventoryProductLog(id, keccak256(bytes(inventoryProductId)), inventoryDate, 2,
                                productVolume, GetInsertType(), measurementUnit, metadata, now);
    }

    function Update(
        string calldata inventoryProductId,
        string calldata transactionId,
        int64 inventoryDate,
        string calldata metadata,
        int256 productVolume,
        string calldata measurementUnit
    )
    external {
        string memory existingTransactionId = Transactions[inventoryProductId];
        require(!compareStrings(existingTransactionId, ""), "Inventory product identifier must already be present");
        require(InventoryProducts[transactionId].Exists == false, "Inventory product transaction identifier must be unique");

        Transactions[inventoryProductId] = transactionId;
        InventoryProducts[transactionId] = InventoryProduct(inventoryProductId, productVolume,
                                                        measurementUnit, inventoryDate, transactionId, metadata, true);
        bytes32 id = keccak256(bytes(transactionId));

        emit TrueLog(GetEventType(), 2, id, "", now);
        emit TrueInventoryProductLog(id, keccak256(bytes(inventoryProductId)), inventoryDate, 2,
                                productVolume, GetInsertType(), measurementUnit, metadata, now);
    }

    function Delete(
        string calldata inventoryProductId,
        string calldata transactionId,
        int64 inventoryDate,
        string calldata metadata
    )
    external {
        string memory existingTransactionId = Transactions[inventoryProductId];
        require(!compareStrings(existingTransactionId, ""), "Inventory product identifier is already present");
        require(InventoryProducts[transactionId].Exists == false, "Inventory product transaction identifier must be unique");

        delete Transactions[inventoryProductId];
        delete InventoryProducts[transactionId];

        bytes32 id = keccak256(bytes(transactionId));

        emit TrueLog(GetEventType(), 2, id, "", now);
        emit TrueInventoryProductLog(id, keccak256(bytes(inventoryProductId)), inventoryDate, 2, 0, GetInsertType(), "", metadata, now);
    }

    function GetByTransactionId(string memory transactionId)
        public
        view
        returns (
            string memory InventoryProductId,
            int256 ProductVolume,
            string memory MeasurementUnit,
            int64 InventoryDate,
            string memory TransactionId,
            string memory Metadata,
            bool Exists
        )
    {
        InventoryProduct memory inventoryProduct = InventoryProducts[transactionId];

        return (
            inventoryProduct.InventoryProductId,
            inventoryProduct.ProductVolume,
            inventoryProduct.MeasurementUnit,
            inventoryProduct.InventoryDate,
            inventoryProduct.TransactionId,
            inventoryProduct.Metadata,
            inventoryProduct.Exists
        );
    }

    function GetByInventoryProductUniqueId(string memory inventoryProductUniqueId)
        public
        view
        returns (
            string memory InventoryProductId,
            int256 ProductVolume,
            string memory MeasurementUnit,
            int64 InventoryDate,
            string memory TransactionId,
            string memory Metadata,
            bool Exists
        )
    {
        string memory transactionId = Transactions[inventoryProductUniqueId];
        InventoryProduct memory inventoryProduct = InventoryProducts[transactionId];

        return (
            inventoryProduct.InventoryProductId,
            inventoryProduct.ProductVolume,
            inventoryProduct.MeasurementUnit,
            inventoryProduct.InventoryDate,
            inventoryProduct.TransactionId,
            inventoryProduct.Metadata,
            inventoryProduct.Exists
        );
    }
}