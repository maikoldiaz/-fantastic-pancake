const MovementOwnershipsFactory = artifacts.require("MovementOwnershipsFactory");
const truffleAssert = require('truffle-assertions');

contract('MovementOwnershipsFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    async function checkDetails(movementOwnershipId, movementId, ticketId, ownershipVolume, ownershipPercentage, metadata, exists, shouldHaveOwnership) {
        let movementOwnership = await contract.Get.call(movementOwnershipId);

        expect(movementOwnership.MovementId).to.equal(movementId);
        expect(toNumbers(movementOwnership.TicketId)).to.equal(ticketId);
        expect(toNumbers(movementOwnership.Volume)).to.equal(ownershipVolume);
        expect(toNumbers(movementOwnership.Percentage)).to.equal(ownershipPercentage);
        expect(movementOwnership.Metadata).to.equal(metadata);
        expect(movementOwnership.Exists).to.equal(exists);

        let hasOwner = await contract.HasOwnership.call(movementOwnershipId);
        expect(hasOwner).to.equal(shouldHaveOwnership);
    }


    beforeEach(async function () {
        contract = await MovementOwnershipsFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no movement ownership by default', async function () {
        await checkDetails('1', '', 0, 0, 0, '', false, false);
    });
    
    it('Register movement ownership and verify details', async function () {
        let movementOwnershipId = '1';
        let movementId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(movementId, movementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 5 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueMovementOwnershipLog', (ev) => {
            return ev.TicketId == ticketId && 
            ev.Version == 2 &&
            ev.Volume == ownershipVolume &&
            ev.Percentage == ownershipPercentage &&
            ev.Metadata === metadata;
        });

        await checkDetails(movementOwnershipId, 
            movementId,
            ticketId,
            ownershipVolume,
            ownershipPercentage,
            metadata,
            true,
            true
        );
    });
    
    it('Try to register movement ownerships with same movement ownership identifier', async function () {
        let movementOwnershipId = '1';
        let movementId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let ownership1 = await contract.Insert(movementId, movementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(ownership1, 'TrueLog');

        truffleAssert.eventEmitted(ownership1, 'TrueMovementOwnershipLog');

        let secondTicketId = 2;

        await truffleAssert.fails(
            contract.Insert.call(movementId, movementOwnershipId, secondTicketId, ownershipVolume, ownershipPercentage, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });
    
    it('Register movement ownerships with different movement ownership identifier', async function () {
        let movementOwnershipId = '1';
        let movementId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let ownership1 = await contract.Insert(movementId, movementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(ownership1, 'TrueLog');

        truffleAssert.eventEmitted(ownership1, 'TrueMovementOwnershipLog');

        let secondMovementOwnershipId = '2';
        let ownership2 = await contract.Insert(movementId, secondMovementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(ownership2, 'TrueLog', (ev) => {
            return ev.Type == 5 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(ownership2, 'TrueMovementOwnershipLog', (ev) => {
            return ev.TicketId == ticketId && 
            ev.Version == 2 &&
            ev.Volume == ownershipVolume &&
            ev.Percentage == ownershipPercentage &&
            ev.Metadata === metadata;
        });

        await checkDetails(secondMovementOwnershipId, 
            movementId,
            ticketId,
            ownershipVolume,
            ownershipPercentage,
            metadata,
            true,
            true
        );
    });
    
    it('Try to update non-existing movement ownership', async function () {
        await truffleAssert.fails(
            contract.Update.call('1A', '1', 1, 100, 50, 'ABC'),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });
    
    it('Update movement ownership', async function () {
        let movementOwnershipId = '1';
        let movementId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(movementId, movementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementOwnershipLog');

        let secondTicketId = 2;
        let update = await contract.Update(movementId, movementOwnershipId, secondTicketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 5 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueMovementOwnershipLog', (ev) => {
            return ev.TicketId == secondTicketId && 
            ev.Version == 2 &&
            ev.Volume == ownershipVolume &&
            ev.Percentage == ownershipPercentage &&
            ev.Metadata === metadata;
        });

        await checkDetails(movementOwnershipId, 
            movementId,
            secondTicketId,
            ownershipVolume,
            ownershipPercentage,
            metadata,
            true,
            true
        );
    });
    
    it('Try to delete non-existing movement ownership', async function () {
        await truffleAssert.fails(
            contract.Delete.call('1A', '1', 1),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });
    
    it('Delete movement ownership', async function () {
        let movementOwnershipId = '1';
        let movementId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(movementId, movementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementOwnershipLog');
        let del = await contract.Delete(movementId, movementOwnershipId, ticketId);
        
        truffleAssert.eventEmitted(del, 'TrueLog', (ev) => {
            return ev.Type == 5 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(del, 'TrueMovementOwnershipLog', (ev) => {
            return ev.TicketId == ticketId && 
            ev.Version == 2 &&
            ev.Volume == 0 &&
            ev.Percentage == 0 &&
            ev.Metadata === '';
        });

        await checkDetails(movementOwnershipId, '', 0, 0, 0, '', false, false);
    });
    
    it('Try to update a deleted movement ownership', async function () {
        let movementOwnershipId = '1';
        let movementId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(movementId, movementOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementOwnershipLog');
        let del = await contract.Delete(movementId, movementOwnershipId, ticketId);

        truffleAssert.eventEmitted(del, 'TrueLog');

        truffleAssert.eventEmitted(del, 'TrueMovementOwnershipLog');

        let secondTicketId = 2;
        await truffleAssert.fails(
            contract.Update.call(movementId, movementOwnershipId, secondTicketId, ownershipVolume, ownershipPercentage, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});