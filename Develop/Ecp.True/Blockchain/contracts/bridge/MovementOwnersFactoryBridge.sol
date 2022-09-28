pragma solidity ^0.5.0;

import '../ContractBase.sol';
import './BridgeBase.sol';

contract MovementOwnersFactoryBridge is ContractBase(2), BridgeBase(msg.sender) {
    mapping(bytes32 => OwnerDetail) Owners;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueMovementOwnerLog(
        bytes32 indexed Id,
        bytes32 indexed MovementId,
        uint8 Version,
        uint8 ActionType,
        string OwnerId,
        int256 Value,
        string Unit,
        uint Timestamp
    );

    struct OwnerDetail {
        string MovementId;
        string TransactionId;
        string OwnerId;
        int256 Value;
        string Unit;
        bool Exists;
    }

    constructor() public {}

    function Insert(
        string memory movementOwnerId,
        string memory movementId,
        string memory ownerId,
        string memory transactionId,
        int256 ownershipValue,
        string memory ownershipValueUnit
    )
    public restricted {
        bytes32 key = keccak256(bytes(movementOwnerId));
        Owners[key] = OwnerDetail(movementId, transactionId, ownerId, ownershipValue, ownershipValueUnit, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnerLog(key, keccak256(bytes(movementId)), 2, GetInsertType(), ownerId, ownershipValue, ownershipValueUnit, now);
    }

    function Update(
        string memory movementOwnerId,
        string memory movementId,
        string memory ownerId,
        string memory transactionId,
        int256 ownershipValue,
        string memory ownershipValueUnit
    )
    public restricted {
        bytes32 key = keccak256(bytes(movementOwnerId));
        Owners[key] = OwnerDetail(movementId, transactionId, ownerId, ownershipValue, ownershipValueUnit, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnerLog(key, keccak256(bytes(movementId)), 2, GetUpdateType(), ownerId, ownershipValue, ownershipValueUnit, now);
    }

    function Delete(
        string memory movementOwnerId,
        string memory movementId,
        string memory ownerId
    )
    public restricted {
        bytes32 key = keccak256(bytes(movementOwnerId));
        delete Owners[key];

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnerLog(key, keccak256(bytes(movementId)), 2, GetDeleteType(), ownerId, 0, "", now);
    }

    function Get(string memory movementOwnerId)
        public
        view
        returns (
        string memory MovementId,
        string memory TransactionId,
        string memory OwnerId,
        int256 Value,
        string memory Unit,
        bool Exists
        ) {
        bytes32 key = keccak256(bytes(movementOwnerId));
        OwnerDetail memory owner = Owners[key];
        return (
            owner.MovementId,
            owner.TransactionId,
            owner.OwnerId,
            owner.Value,
            owner.Unit,
            owner.Exists
        );
    }
}