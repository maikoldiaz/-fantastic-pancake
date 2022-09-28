const NodeConnectionsFactory = artifacts.require("NodeConnectionsFactory");
const truffleAssert = require('truffle-assertions');

contract('NodeConnectionsFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    beforeEach(async function () {
        contract = await NodeConnectionsFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no node connection by default', async function () {
        let nodeConnection = await contract.Get.call('1');

        expect(nodeConnection.NodeConnectionId).to.equal('');
        expect(nodeConnection.IsActive).to.equal(false);
        expect(toNumbers(nodeConnection.SourceNodeId)).to.equal(0);
        expect(toNumbers(nodeConnection.DestinationNodeId)).to.equal(0);
        expect(nodeConnection.IsDeleted).to.equal(false);
        expect(nodeConnection.Exists).to.equal(false);
    });

    it('Register node connection and verify details', async function () {
        let create = await contract.Register('1', true, false, 1, 2);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 9 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueNodeConnectionLog', (ev) => {
            return ev.SourceNodeId == 1 && 
            ev.DestinationNodeId == 2 &&
            ev.Version == 2 &&
            ev.IsActive == true &&
            ev.IsDeleted == false;
        });
        let nodeConnection = await contract.Get.call('1');
        
        expect(nodeConnection.NodeConnectionId).to.equal('1');
        expect(nodeConnection.IsActive).to.equal(true);
        expect(toNumbers(nodeConnection.SourceNodeId)).to.equal(1);
        expect(toNumbers(nodeConnection.DestinationNodeId)).to.equal(2);
        expect(nodeConnection.IsDeleted).to.equal(false);
        expect(nodeConnection.Exists).to.equal(true);
    });

    it('Try to register a new node connection as deleted', async function () {
        await truffleAssert.fails(
            contract.Register.call('1', true, true, 1, 2),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" A connection cannot be registered as deleted."
        );
    });

    it('Register node connection and update it', async function () {
        let create = await contract.Register('1', true, false, 1, 2);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueNodeConnectionLog');

        let update = await contract.Register('1', true, false, 3, 4);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 9 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueNodeConnectionLog', (ev) => {
            return ev.SourceNodeId == 3 && 
            ev.DestinationNodeId == 4 &&
            ev.Version == 2 &&
            ev.IsActive == true &&
            ev.IsDeleted == false;
        });
        let nodeConnection = await contract.Get.call('1');
        
        expect(nodeConnection.NodeConnectionId).to.equal('1');
        expect(nodeConnection.IsActive).to.equal(true);
        expect(toNumbers(nodeConnection.SourceNodeId)).to.equal(3);
        expect(toNumbers(nodeConnection.DestinationNodeId)).to.equal(4);
        expect(nodeConnection.IsDeleted).to.equal(false);
        expect(nodeConnection.Exists).to.equal(true);
    });

    it('Try to update a deleted node', async function () {
        let create = await contract.Register('1', true, false, 1, 2);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueNodeConnectionLog');

        let update = await contract.Register('1', true, true, 1, 2);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 9 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueNodeConnectionLog', (ev) => {
            return ev.SourceNodeId == 1 && 
            ev.DestinationNodeId == 2 &&
            ev.Version == 2 &&
            ev.IsActive == true &&
            ev.IsDeleted == true;
        });

        await truffleAssert.fails(
            contract.Register.call('1', true, false, 3, 4),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" The connection is already deleted"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});