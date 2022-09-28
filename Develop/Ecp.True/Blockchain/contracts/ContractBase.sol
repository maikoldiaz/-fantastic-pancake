pragma solidity ^0.5.0;

contract ContractBase {
    uint8 eventType;
    constructor(uint8 evtType) public {
        eventType = evtType;
    }

    enum ActionType { Insert, Update, Delete }

    function compareStrings(string memory a, string memory b) internal pure returns (bool){
        return keccak256(abi.encodePacked(a)) == keccak256(abi.encodePacked(b));
    }

    function GetEventType() internal view returns (uint8) {
        return eventType;
    }

    function GetInsertType() internal pure returns (uint8) {
        return uint8(ActionType.Insert);
    }

    function GetUpdateType() internal pure returns (uint8) {
        return uint8(ActionType.Update);
    }

    function GetDeleteType() internal pure returns (uint8) {
        return uint8(ActionType.Delete);
    }
}