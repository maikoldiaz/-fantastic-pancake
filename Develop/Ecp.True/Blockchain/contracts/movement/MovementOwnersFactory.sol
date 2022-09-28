pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract MovementOwnersFactory is ContractBase(2) {
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

    modifier isValidOwner(string memory movementOwnerId, bool condition) {
        bytes32 key = keccak256(bytes(movementOwnerId));
        require(Owners[key].Exists == condition, "Invalid movement owner identifier");
        _;
    }

    function Insert(
        string calldata movementOwnerId,
        string calldata movementId,
        string calldata ownerId,
        string calldata transactionId,
        int256 ownershipValue,
        string calldata ownershipValueUnit
    )
    external
    isValidOwner(movementOwnerId, false) {
        bytes32 key = keccak256(bytes(movementOwnerId));
        Owners[key] = OwnerDetail(movementId, transactionId, ownerId, ownershipValue, ownershipValueUnit, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnerLog(key, keccak256(bytes(movementId)), 2, GetInsertType(), ownerId, ownershipValue, ownershipValueUnit, now);
    }

    function Update(
        string calldata movementOwnerId,
        string calldata movementId,
        string calldata ownerId,
        string calldata transactionId,
        int256 ownershipValue,
        string calldata ownershipValueUnit
    )
    external
    isValidOwner(movementOwnerId, true) {
        bytes32 key = keccak256(bytes(movementOwnerId));
        Owners[key] = OwnerDetail(movementId, transactionId, ownerId, ownershipValue, ownershipValueUnit, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnerLog(key, keccak256(bytes(movementId)), 2, GetUpdateType(), ownerId, ownershipValue, ownershipValueUnit, now);
    }

    function Delete(
        string calldata movementOwnerId,
        string calldata movementId,
        string calldata ownerId
    )
    external
    isValidOwner(movementOwnerId, true) {
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

    function HasOwner(string memory movementOwnerId) public view returns (bool) {
        bytes32 key = keccak256(bytes(movementOwnerId));
        OwnerDetail memory owner = Owners[key];
        return owner.Exists;
    }
}