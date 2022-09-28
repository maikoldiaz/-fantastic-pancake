pragma solidity ^0.5.0;

import '../ContractBase.sol';
import './BridgeBase.sol';

contract InventoryProductOwnershipsFactoryBridge is ContractBase(6), BridgeBase(msg.sender) {
    mapping(bytes32 => InventoryProductOwnership) Ownerships;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueInventoryProductOwnershipLog(
        bytes32 indexed Id,
        bytes32 indexed InventoryProductId,
        int64 indexed TicketId,
        uint8 Version,
        uint8 ActionType,
        int256 Volume,
        int256 Percentage,
        string Metadata,
        uint Timestamp
    );

    // Metadata => [OwnerId|RuleName|RuleVersion]
    struct InventoryProductOwnership {
        string InventoryProductId;
        int64 TicketId;
        int256 Volume;
        int256 Percentage;
        string Metadata;
        bool Exists;
    }

    constructor() public {}

    function Insert(
        string memory inventoryProductId,
        string memory inventoryProductOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string memory metadata
    )
    public restricted
    {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        Ownerships[key] = InventoryProductOwnership(inventoryProductId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnershipLog(key, keccak256(bytes(inventoryProductId)), ticketId, 2,
                                    GetInsertType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Update(
        string memory inventoryProductId,
        string memory inventoryProductOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string memory metadata
    )
    public restricted {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        Ownerships[key] = InventoryProductOwnership(inventoryProductId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnershipLog(key, keccak256(bytes(inventoryProductId)), ticketId, 2,
                                    GetUpdateType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Delete(
        string memory inventoryProductId,
        string memory inventoryProductOwnershipId,
        int64 ticketId
    )
    public restricted {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        delete Ownerships[key];

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnershipLog(key, keccak256(bytes(inventoryProductId)), ticketId, 2, GetDeleteType(), 0, 0, "", now);
    }

    function Get(string memory inventoryProductOwnershipId)
        public
        view
        returns (
        string memory InventoryProductId,
        int64 TicketId,
        int256 Volume,
        int256 Percentage,
        string memory Metadata,
        bool Exists
        ) {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        InventoryProductOwnership memory ownership = Ownerships[key];
        return (
            ownership.InventoryProductId,
            ownership.TicketId,
            ownership.Volume,
            ownership.Percentage,
            ownership.Metadata,
            ownership.Exists
        );
    }
}
