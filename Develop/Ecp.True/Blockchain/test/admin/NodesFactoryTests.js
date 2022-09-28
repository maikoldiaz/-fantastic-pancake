const NodesFactory = artifacts.require("NodesFactory");
const truffleAssert = require('truffle-assertions');

contract('NodesFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    beforeEach(async function () {
        contract = await NodesFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no node by default', async function () {
        let node = await contract.Get.call('1');

        expect(node.NodeId).to.equal('');
        expect(node.Name).to.equal('');
        expect(toNumbers(node.State)).to.equal(0);
        expect(toNumbers(node.LastUpdateDate)).to.equal(0);
        expect(node.IsActive).to.equal(false);
        expect(node.Exists).to.equal(false);
    });

    it('Register a node and verify its details ', async function () {
        let create = await contract.Register('1', 'node1', 1, 10, true);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 8 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueNodeLog', (ev) => {
            return ev.Version == 2 && 
            ev.Name === 'node1' &&
            ev.State == 1 &&
            ev.LastUpdateDate == 10 &&
            ev.IsActive == true;
        });

        let node = await contract.Get.call('1');

        expect(node.NodeId).to.equal('1');
        expect(node.Name).to.equal('node1');
        expect(toNumbers(node.State)).to.equal(1);
        expect(toNumbers(node.LastUpdateDate)).to.equal(10);
        expect(node.IsActive).to.equal(true);
        expect(node.Exists).to.equal(true);

        let nodeExists = await contract.HasNode.call('1');
        expect(nodeExists).to.equal(true);
    });

    it('Try to register a node with invalid state', async function () {
        await truffleAssert.fails(
            contract.Register.call('1', 'node1', 0, 10, true),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid node state"
        );

        let nodeExists = await contract.HasNode.call('1');
        expect(nodeExists).to.equal(false);
    });

    it('Try to register a new node with a state other than 1', async function () {
        await truffleAssert.fails(
            contract.Register.call('1', 'node1', 2, 10, true),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" The node is not created"
        );

        let nodeExists = await contract.HasNode.call('1');
        expect(nodeExists).to.equal(false);
    });

    it('Update an existing node to higher state', async function () {
        let create = await contract.Register('1', 'node1', 1, 10, true);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueNodeLog');

        let update = await contract.Register('1', 'node1', 2, 11, false);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 8 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueNodeLog', (ev) => {
            return ev.Version == 2 && 
            ev.Name === 'node1' &&
            ev.State == 2 &&
            ev.LastUpdateDate == 11 &&
            ev.IsActive == false;
        });

        let node = await contract.Get.call('1');

        expect(node.NodeId).to.equal('1');
        expect(node.Name).to.equal('node1');
        expect(toNumbers(node.State)).to.equal(2);
        expect(toNumbers(node.LastUpdateDate)).to.equal(11);
        expect(node.IsActive).to.equal(false);
        expect(node.Exists).to.equal(true);

        let nodeExists = await contract.HasNode.call('1');
        expect(nodeExists).to.equal(true);
    });

    it('Try to update an existing node to create state again', async function () {
        let create = await contract.Register('1', 'node1', 1, 10, true);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueNodeLog');

        await truffleAssert.fails(
            contract.Register.call('1', 'node1', 1, 10, true),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" This node is already created"
        );

        let node = await contract.Get.call('1');

        expect(node.NodeId).to.equal('1');
        expect(node.Name).to.equal('node1');
        expect(toNumbers(node.State)).to.equal(1);
        expect(toNumbers(node.LastUpdateDate)).to.equal(10);
        expect(node.IsActive).to.equal(true);
        expect(node.Exists).to.equal(true);

        let nodeExists = await contract.HasNode.call('1');
        expect(nodeExists).to.equal(true);
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber(); 
    }
});