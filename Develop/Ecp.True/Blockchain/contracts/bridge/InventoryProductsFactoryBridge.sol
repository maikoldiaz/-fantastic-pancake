pragma solidity ^0.5.0;

import '../ContractBase.sol';
import './BridgeBase.sol';

contract InventoryProductsFactoryBridge is ContractBase(3), BridgeBase(msg.sender) {
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
        string memory inventoryProductId,
        string memory transactionId,
        int64 inventoryDate,
        string memory metadata,
        int256 productVolume,
        string memory measurementUnit
    )
    public restricted {
        Transactions[inventoryProductId] = transactionId;
        InventoryProducts[transactionId] = InventoryProduct(inventoryProductId, productVolume,
                                                        measurementUnit, inventoryDate, transactionId, metadata, true);
        bytes32 id = keccak256(bytes(transactionId));

        emit TrueLog(GetEventType(), 2, id, "", now);
        emit TrueInventoryProductLog(id, keccak256(bytes(inventoryProductId)), inventoryDate, 2,
                                productVolume, GetInsertType(), measurementUnit, metadata, now);
    }

    function Update(
        string memory inventoryProductId,
        string memory transactionId,
        int64 inventoryDate,
        string memory metadata,
        int256 productVolume,
        string memory measurementUnit
    )
    public restricted {
        Transactions[inventoryProductId] = transactionId;
        InventoryProducts[transactionId] = InventoryProduct(inventoryProductId, productVolume,
                                                        measurementUnit, inventoryDate, transactionId, metadata, true);
        bytes32 id = keccak256(bytes(transactionId));

        emit TrueLog(GetEventType(), 2, id, "", now);
        emit TrueInventoryProductLog(id, keccak256(bytes(inventoryProductId)), inventoryDate, 2,
                                productVolume, GetInsertType(), measurementUnit, metadata, now);
    }

    function Delete(
        string memory inventoryProductId,
        string memory transactionId,
        int64 inventoryDate,
        string memory metadata
    )
    public restricted {
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