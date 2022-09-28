pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract MovementOwnershipsFactory is ContractBase(5) {
    mapping(bytes32 => MovementOwnership) Ownerships;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueMovementOwnershipLog(
        bytes32 indexed Id,
        bytes32 indexed MovementId,
        int64 indexed TicketId,
        uint8 Version,
        uint8 ActionType,
        int256 Volume,
        int256 Percentage,
        string Metadata,
        uint Timestamp
    );

    // Metadata => [OwnerId,RuleName,RuleVersion]
    struct MovementOwnership {
        string MovementId;
        int64 TicketId;
        int256 Volume;
        int256 Percentage;
        string Metadata;
        bool Exists;
    }

    constructor() public {}

    modifier isValidOwner(string memory movementOwnershipId, bool condition) {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        require(Ownerships[key].Exists == condition, "Invalid movement ownership identifier");
        _;
    }

    function Insert(
        string calldata movementId,
        string calldata movementOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string calldata metadata
    )
    external
    isValidOwner(movementOwnershipId, false) {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        Ownerships[key] = MovementOwnership(movementId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnershipLog(key, keccak256(bytes(movementId)), ticketId, 2,
                                    GetInsertType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Update(
        string calldata movementId,
        string calldata movementOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string calldata metadata
    )
    external
    isValidOwner(movementOwnershipId, true) {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        Ownerships[key] = MovementOwnership(movementId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnershipLog(key, keccak256(bytes(movementId)), ticketId, 2,
                                    GetUpdateType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Delete(
        string calldata movementId,
        string calldata movementOwnershipId,
        int64 ticketId
    )
    external
    isValidOwner(movementOwnershipId, true) {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        delete Ownerships[key];

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnershipLog(key, keccak256(bytes(movementId)), ticketId, 2, GetDeleteType(), 0, 0, "", now);
    }

    function Get(string memory movementOwnershipId)
        public
        view
        returns (
        string memory MovementId,
        int64 TicketId,
        int256 Volume,
        int256 Percentage,
        string memory Metadata,
        bool Exists
        ) {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        MovementOwnership memory ownership = Ownerships[key];
        return (
            ownership.MovementId,
            ownership.TicketId,
            ownership.Volume,
            ownership.Percentage,
            ownership.Metadata,
            ownership.Exists
        );
    }

    function HasOwnership(string memory movementOwnershipId) public view returns (bool) {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        MovementOwnership memory ownership = Ownerships[key];
        return ownership.Exists;
    }
}
