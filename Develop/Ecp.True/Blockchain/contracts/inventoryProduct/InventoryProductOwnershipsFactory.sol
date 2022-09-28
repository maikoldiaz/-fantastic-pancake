pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract InventoryProductOwnershipsFactory is ContractBase(6) {
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

    modifier isValidOwner(string memory inventoryOwnershipId, bool condition) {
        bytes32 key = keccak256(bytes(inventoryOwnershipId));
        require(Ownerships[key].Exists == condition, "Invalid movement ownership identifier");
        _;
    }

    function Insert(
        string calldata inventoryProductId,
        string calldata inventoryProductOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string calldata metadata
    )
    external
    isValidOwner(inventoryProductOwnershipId, false)
    {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        Ownerships[key] = InventoryProductOwnership(inventoryProductId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnershipLog(key, keccak256(bytes(inventoryProductId)), ticketId, 2,
                                    GetInsertType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Update(
        string calldata inventoryProductId,
        string calldata inventoryProductOwnershipId,
        int64 ticketId,
        int256 ownershipVolume,
        int256 ownershipPercentage,
        string calldata metadata
    )
    external
    isValidOwner(inventoryProductOwnershipId, true) {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        Ownerships[key] = InventoryProductOwnership(inventoryProductId, ticketId, ownershipVolume, ownershipPercentage, metadata, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueInventoryProductOwnershipLog(key, keccak256(bytes(inventoryProductId)), ticketId, 2,
                                    GetUpdateType(), ownershipVolume, ownershipPercentage, metadata, now);
    }

    function Delete(
        string calldata inventoryProductId,
        string calldata inventoryProductOwnershipId,
        int64 ticketId
    )
    external
    isValidOwner(inventoryProductOwnershipId, true) {
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

    function HasOwnership(string memory inventoryProductOwnershipId) public view returns (bool) {
        bytes32 key = keccak256(bytes(inventoryProductOwnershipId));
        InventoryProductOwnership memory ownership = Ownerships[key];
        return ownership.Exists;
    }
}
