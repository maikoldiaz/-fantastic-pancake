pragma solidity ^0.5.0;

import '../ContractBase.sol';
import './BridgeBase.sol';

contract InventoryProductOwnersFactoryBridge is ContractBase(4), BridgeBase(msg.sender) {
    mapping(bytes32 => OwnerDetail) Owners;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueInventoryProductOwnerLog(
        bytes32 indexed Id,
        bytes32 indexed InventoryProductId,
        uint8 Version,
        uint8 ActionType,
        string OwnerId,
        int256 Value,
        string Unit,
        uint Timestamp
    );

    struct OwnerDetail {
        string InventoryProductId;
        string TransactionId;
        string OwnerId;
        int256 Value;
        string Unit;
        bool Exists;
    }

    constructor() public {}

    function Insert(
        string memory inventoryProductOwnerId,
        string memory inventoryProductId,
        string memory ownerId,
        string memory transactionId,
        int256 ownershipValue,
        string memory ownershipValueUnit
    )
    public restricted {
        bytes32 key = keccak256(bytes(inventoryProductOwnerId));
        Owners[key] = OwnerDetail(inventoryProductId, transactionId, ownerId, ownershipValue, ownershipValueUnit, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnerLog(key, keccak256(bytes(inventoryProductId)), 2, GetInsertType(),
                                ownerId, ownershipValue, ownershipValueUnit, now);
    }

    function Update(
        string memory inventoryProductOwnerId,
        string memory inventoryProductId,
        string memory ownerId,
        string memory transactionId,
        int256 ownershipValue,
        string memory ownershipValueUnit
    )
    public restricted {
        bytes32 key = keccak256(bytes(inventoryProductOwnerId));
        Owners[key] = OwnerDetail(inventoryProductId, transactionId, ownerId, ownershipValue, ownershipValueUnit, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnerLog(key, keccak256(bytes(inventoryProductId)), 2, GetUpdateType(),
                                ownerId, ownershipValue, ownershipValueUnit, now);
    }

    function Delete(
        string memory inventoryProductOwnerId,
        string memory inventoryProductId,
        string memory ownerId
    )
    public restricted {
        bytes32 key = keccak256(bytes(inventoryProductOwnerId));
        delete Owners[key];

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnerLog(key, keccak256(bytes(inventoryProductId)), 2, GetDeleteType(), ownerId, 0, "", now);
    }

    function Get(string memory inventoryProductOwnerId)
        public
        view
        returns (
        string memory InventoryProductId,
        string memory TransactionId,
        string memory OwnerId,
        int256 Value,
        string memory Unit,
        bool Exists
        ) {
        bytes32 key = keccak256(bytes(inventoryProductOwnerId));
        OwnerDetail memory owner = Owners[key];
        return (
            owner.InventoryProductId,
            owner.TransactionId,
            owner.OwnerId,
            owner.Value,
            owner.Unit,
            owner.Exists
        );
    }
}