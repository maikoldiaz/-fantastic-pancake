pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract NodeConnectionsFactory is ContractBase(9) {
    constructor() public {}

    struct NodeConnection {
        string NodeConnectionId;
        bool IsActive;
        int64 SourceNodeId;
        int64 DestinationNodeId;
        bool IsDeleted;
        bool Exists;
    }

    mapping(string => NodeConnection) NodeConnections;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueNodeConnectionLog(
        bytes32 indexed Id,
        int64 indexed SourceNodeId,
        int64 indexed DestinationNodeId,
        uint8 Version,
        bool IsActive,
        bool IsDeleted,
        uint Timestamp
    );

    function Register(
        string calldata nodeConnectionId,
        bool isActive,
        bool isDeleted,
        int64 sourceNodeId,
        int64 destinationNodeId)
        external {
            NodeConnection memory existing = NodeConnections[nodeConnectionId];
            require(existing.IsDeleted == false, "The connection is already deleted");
            require(existing.Exists == true || (existing.Exists == false && isDeleted == false),
                    "A connection cannot be registered as deleted.");

            NodeConnections[nodeConnectionId] = NodeConnection(nodeConnectionId, isActive, sourceNodeId,
                                                                            destinationNodeId, isDeleted, true);
            bytes32 id = keccak256(bytes(nodeConnectionId));

            emit TrueLog(GetEventType(), 2, id, "", now);
            emit TrueNodeConnectionLog(id, sourceNodeId, destinationNodeId, 2, isActive, isDeleted, now);
    }

    function Get(string memory nodeConnectionId) public view returns (
        string memory NodeConnectionId,
        bool IsActive,
        int64 SourceNodeId,
        int64 DestinationNodeId,
        bool IsDeleted,
        bool Exists)
    {
         NodeConnection memory connection = NodeConnections[nodeConnectionId];
         return (
             connection.NodeConnectionId,
             connection.IsActive,
             connection.SourceNodeId,
             connection.DestinationNodeId,
             connection.IsDeleted,
             connection.Exists);
    }
}