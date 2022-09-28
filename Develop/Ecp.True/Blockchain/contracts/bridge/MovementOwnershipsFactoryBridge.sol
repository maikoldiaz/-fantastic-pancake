pragma solidity ^0.5.0;

import '../ContractBase.sol';
import './BridgeBase.sol';

contract MovementOwnershipsFactoryBridge is ContractBase(5), BridgeBase(msg.sender) {
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

    function Insert(
        string memory movementId,
        string memory movementOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string memory metadata
    )
    public restricted {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        Ownerships[key] = MovementOwnership(movementId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnershipLog(key, keccak256(bytes(movementId)), ticketId, 2,
                                    GetInsertType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Update(
        string memory movementId,
        string memory movementOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string memory metadata
    )
    public restricted {
        bytes32 key = keccak256(bytes(movementOwnershipId));
        Ownerships[key] = MovementOwnership(movementId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueMovementOwnershipLog(key, keccak256(bytes(movementId)), ticketId, 2,
                                    GetUpdateType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Delete(
        string memory movementId,
        string memory movementOwnershipId,
        int64 ticketId
    )
    public restricted {
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
}
