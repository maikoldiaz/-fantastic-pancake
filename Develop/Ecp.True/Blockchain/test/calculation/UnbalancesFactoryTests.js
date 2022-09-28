const UnbalancesFactory = artifacts.require("UnbalancesFactory");
const truffleAssert = require('truffle-assertions');

contract('UnbalancesFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    beforeEach(async function () {
        contract = await UnbalancesFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no unbalance by default', async function () {
        let unbalance = await contract.Get.call('1');

        expect(unbalance.Values).to.equal('');
        expect(toNumbers(unbalance.TicketId)).to.equal(0);
        expect(unbalance.Exists).to.equal(false);
    });

    it('Register an unbalance and verify its details', async function () {
        let create = await contract.Register('1', '100', 1);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 7 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueUnbalanceLog', (ev) => {
            return ev.Version == 2 && 
            ev.TicketId == 1 &&
            ev.Values == '100' &&
            ev.Metadata === '1';
        });

        let unbalance = await contract.Get.call('1');

        expect(unbalance.Values).to.equal('100');
        expect(toNumbers(unbalance.TicketId)).to.equal(1);
        expect(unbalance.Exists).to.equal(true);
    });

    it('Register an existing unbalance with same ticketid', async function () {
        let create = await contract.Register('1', '100', 1);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueUnbalanceLog');

        await truffleAssert.fails(
            contract.Register.call('1', '200', 1),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" This unbalance is already registered"
        );

        let unbalance = await contract.Get.call('1');

        expect(unbalance.Values).to.equal('100');
        expect(toNumbers(unbalance.TicketId)).to.equal(1);
        expect(unbalance.Exists).to.equal(true);
    });

    it('Register an existing unbalance with different ticketid', async function () {
        let create = await contract.Register('1', '100', 1);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueUnbalanceLog');

        let update = await contract.Register('1', '100', 2);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 7 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueUnbalanceLog', (ev) => {
            return ev.Version == 2 && 
            ev.TicketId == 2 &&
            ev.Values == '100' &&
            ev.Metadata === '1';
        });

        let unbalance = await contract.Get.call('1');

        expect(unbalance.Values).to.equal('100');
        expect(toNumbers(unbalance.TicketId)).to.equal(2);
        expect(unbalance.Exists).to.equal(true);
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});