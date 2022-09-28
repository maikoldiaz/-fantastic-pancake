pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract NodesFactory is ContractBase(8) {
    constructor() public {}

    struct Node {
        string NodeId;
        string Name;
        uint8 State;
        int64 LastUpdateDate;
        bool IsActive;
        bool Exists;
    }

    mapping(string => Node) Nodes;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueNodeLog(
        bytes32 indexed Id,
        uint8 Version,
        string Name,
        uint8 State,
        int64 LastUpdateDate,
        bool IsActive,
        uint Timestamp
    );

    function Register(
        string calldata nodeId,
        string calldata name,
        uint8 state,
        int64 lastUpdateDate,
        bool isActive)
    external {
        // Can only pass valid states 1 - Created, 2 - Updated, 3 - BalanceCalculated, 4 - OwnershipCalculated
        require(state > 0 && state < 5, "Invalid node state");

        Node memory existing = Nodes[nodeId];

        // Create a node first before updating any other state.
        require(existing.Exists == true || state == 1, "The node is not created");

        // Can only create a node once.
        require(existing.Exists == false || state > 1, "This node is already created");

        Nodes[nodeId] = Node(nodeId, name, state, lastUpdateDate, isActive, true);

        bytes32 id = keccak256(bytes(nodeId));

        emit TrueLog(GetEventType(), 2, id, "", now);
        emit TrueNodeLog(id, 2, name, state, lastUpdateDate, isActive, now);
    }

    function Get(string memory nodeId) public view returns (
        string memory NodeId,
        string memory Name,
        uint8 State,
        int64 LastUpdateDate,
        bool IsActive,
        bool Exists)
    {
         Node memory node = Nodes[nodeId];
         return (
             node.NodeId,
             node.Name,
             node.State,
             node.LastUpdateDate,
             node.IsActive,
             node.Exists);
    }

    function HasNode(string memory nodeId) public view returns (bool) {
        return Nodes[nodeId].Exists;
    }
}