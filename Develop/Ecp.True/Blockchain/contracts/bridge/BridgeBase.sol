pragma solidity ^0.5.0;

contract BridgeBase {
    address owner;
    
    constructor(address ownr) public {
        owner = ownr;
    }
    
    modifier restricted() {
    require(
            msg.sender == owner,
            "Only owner can call this function."
        );
        _;
    }
}