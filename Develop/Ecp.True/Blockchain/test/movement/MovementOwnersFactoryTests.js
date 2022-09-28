const MovementOwnersFactory = artifacts.require("MovementOwnersFactory");
const truffleAssert = require('truffle-assertions');

contract('MovementOwnersFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    async function checkDetails(movementOwnerId, movementId, transactionId, ownerId, value, unit, exists, shouldHaveOwner) {
        let movementOwner = await contract.Get.call(movementOwnerId);

        expect(movementOwner.MovementId).to.equal(movementId);
        expect(movementOwner.TransactionId).to.equal(transactionId);
        expect(movementOwner.OwnerId).to.equal(ownerId);
        expect(toNumbers(movementOwner.Value)).to.equal(value);
        expect(movementOwner.Unit).to.equal(unit);
        expect(movementOwner.Exists).to.equal(exists);

        let hasOwner = await contract.HasOwner.call(movementOwnerId);
        expect(hasOwner).to.equal(shouldHaveOwner);
    }


    beforeEach(async function () {
        contract = await MovementOwnersFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no movement owner by default', async function () {
        await checkDetails('1','', '','', 0, '', false, false);
    });
    
    it('Register movement owner and verify details', async function () {
        let movementOwnerId = '1';
        let movementId = '1A';
        let transactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(movementOwnerId, movementId, ownerId, transactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 2 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueMovementOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == value &&
            ev.Unit === unit;
        });

        await checkDetails(movementOwnerId, 
            movementId,
            transactionId,
            ownerId,
            value,
            unit,
            true,
            true
        );
    });
    
    it('Try to register movement owners with same movement owner identifier', async function () {
        let movementOwnerId = '1';
        let movementId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let owner1 = await contract.Insert(movementOwnerId, movementId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(owner1, 'TrueLog');

        truffleAssert.eventEmitted(owner1, 'TrueMovementOwnerLog');

        let secondTransactionId = '2';

        await truffleAssert.fails(
            contract.Insert.call(movementOwnerId, movementId, ownerId, secondTransactionId, value, unit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement owner identifier"
        );
    });
    
    it('Register movement owners with different movement owner identifier', async function () {
        let movementOwnerId = '1';
        let movementId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let owner1 = await contract.Insert(movementOwnerId, movementId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(owner1, 'TrueLog');

        truffleAssert.eventEmitted(owner1, 'TrueMovementOwnerLog');

        let secondMovementOwnerId = '2';
        let secondTransactionId = '2';
        let owner2 = await contract.Insert(secondMovementOwnerId, movementId, ownerId, secondTransactionId, value, unit);

        truffleAssert.eventEmitted(owner2, 'TrueLog', (ev) => {
            return ev.Type == 2 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(owner2, 'TrueMovementOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == value &&
            ev.Unit === unit;
        });

        await checkDetails(secondMovementOwnerId, 
            movementId,
            secondTransactionId,
            ownerId,
            value,
            unit,
            true,
            true
        );
    });
    
    it('Try to update non-existing movement owner', async function () {
        await truffleAssert.fails(
            contract.Update.call('1', '1A', '1', '1', 100, 'Bbl'),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement owner identifier"
        );
    });
    
    it('Update movement owner', async function () {
        let movementOwnerId = '1';
        let movementId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(movementOwnerId, movementId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementOwnerLog');
        let secondTransactionId = '2';
        let update = await contract.Update(movementOwnerId, movementId, ownerId, secondTransactionId, value, unit);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 2 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueMovementOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == value &&
            ev.Unit === unit;
        });

        await checkDetails(movementOwnerId, 
            movementId,
            secondTransactionId,
            ownerId,
            value,
            unit,
            true,
            true
        );
    });
    
    it('Try to delete non-existing movement owner', async function () {
        await truffleAssert.fails(
            contract.Delete.call('1', '1A', '1'),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement owner identifier"
        );
    });
    
    it('Delete movement owner', async function () {
        let movementOwnerId = '1';
        let movementId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(movementOwnerId, movementId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementOwnerLog');
        let del = await contract.Delete(movementOwnerId, movementId, ownerId);

        truffleAssert.eventEmitted(del, 'TrueLog', (ev) => {
            return ev.Type == 2 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(del, 'TrueMovementOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == 0 &&
            ev.Unit === '';
        });

        await checkDetails(movementOwnerId, '', '','', 0, '', false, false);
    });
    
    it('Try to update a deleted movement owner', async function () {
        let movementOwnerId = '1';
        let movementId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(movementOwnerId, movementId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementOwnerLog');
        let del = await contract.Delete(movementOwnerId, movementId, ownerId);

        truffleAssert.eventEmitted(del, 'TrueLog');

        truffleAssert.eventEmitted(del, 'TrueMovementOwnerLog');

        let secondTransactionId = '2'
        await truffleAssert.fails(
            contract.Update.call(movementOwnerId, movementId, ownerId, secondTransactionId, value, unit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement owner identifier"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});