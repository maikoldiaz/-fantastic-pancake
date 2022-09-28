pragma solidity ^0.5.0;

import '../ContractBase.sol';

contract UnbalancesFactory is ContractBase(7)
{
    constructor() public {}

    struct Unbalance {
        string Values;
        int64 TicketId;
        bool Exists;
    }

    mapping(bytes32 => Unbalance) Unbalances;

    event TrueLog(
        int64 indexed Type,
        int64 indexed Version,
        bytes32 indexed Id,
        string Data,
        uint Timestamp
    );

    event TrueUnbalanceLog(
        bytes32 indexed Id,
        int64 indexed TicketId,
        uint8 Version,
        string Values,
        uint Timestamp,
        string Metadata
    );

    function Register(
        string calldata dateNodeProductKey,
        string calldata calculatedValues,
        int64 ticketId)
    external {
        bytes32 key = keccak256(bytes(dateNodeProductKey));
        Unbalance memory existing = Unbalances[key];

        // Can register unbalance for a date node product only once for a ticket id.
        require(existing.Exists == false || existing.TicketId != ticketId, "This unbalance is already registered");

        Unbalances[key] = Unbalance(calculatedValues, ticketId, true);

        emit TrueLog(GetEventType(), 2, key, "", now);
        emit TrueUnbalanceLog(key, ticketId, 2, calculatedValues, now, dateNodeProductKey);
    }

    function Get(string memory dateNodeProductKey) public view returns (
        string memory Values,
        int64 TicketId,
        bool Exists) {
        bytes32 key = keccak256(bytes(dateNodeProductKey));
        Unbalance memory calculation = Unbalances[key];
        return (
            calculation.Values,
            calculation.TicketId,
            calculation.Exists
        );
    }
}