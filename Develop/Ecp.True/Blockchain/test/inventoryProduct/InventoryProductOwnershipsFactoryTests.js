const InventoryProductOwnershipsFactory = artifacts.require("InventoryProductOwnershipsFactory");
const truffleAssert = require('truffle-assertions');

contract('InventoryProductOwnershipsFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    async function checkDetails(inventoryProductOwnershipId, inventoryProductId, ticketId, ownershipVolume, ownershipPercentage, metadata, exists, shouldHaveOwnership) {
        let inventoryProductOwnership = await contract.Get.call(inventoryProductOwnershipId);

        expect(inventoryProductOwnership.InventoryProductId).to.equal(inventoryProductId);
        expect(toNumbers(inventoryProductOwnership.TicketId)).to.equal(ticketId);
        expect(toNumbers(inventoryProductOwnership.Volume)).to.equal(ownershipVolume);
        expect(toNumbers(inventoryProductOwnership.Percentage)).to.equal(ownershipPercentage);
        expect(inventoryProductOwnership.Metadata).to.equal(metadata);
        expect(inventoryProductOwnership.Exists).to.equal(exists);

        let hasOwner = await contract.HasOwnership.call(inventoryProductOwnershipId);
        expect(hasOwner).to.equal(shouldHaveOwnership);
    }


    beforeEach(async function () {
        contract = await InventoryProductOwnershipsFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no inventory product ownership by default', async function () {
        await checkDetails('1', '', 0, 0, 0, '', false, false);
    });
    
    it('Register inventory product ownership and verify details', async function () {
        let inventoryProductOwnershipId = '1';
        let inventoryProductId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(inventoryProductId, inventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 6 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnershipLog', (ev) => {
            return ev.TicketId == ticketId && 
            ev.Version == 2 &&
            ev.Volume == ownershipVolume &&
            ev.Percentage == ownershipPercentage &&
            ev.Metadata === metadata;
        });

        await checkDetails(inventoryProductOwnershipId, 
            inventoryProductId,
            ticketId,
            ownershipVolume,
            ownershipPercentage,
            metadata,
            true,
            true
        );
    });
    
    it('Try to register inventory product ownerships with same inventory product ownership identifier', async function () {
        let inventoryProductOwnershipId = '1';
        let inventoryProductId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let ownership1 = await contract.Insert(inventoryProductId, inventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(ownership1, 'TrueLog');

        truffleAssert.eventEmitted(ownership1, 'TrueInventoryProductOwnershipLog');

        let secondTicketId = 2;

        await truffleAssert.fails(
            contract.Insert.call(inventoryProductId, inventoryProductOwnershipId, secondTicketId, ownershipVolume, ownershipPercentage, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });
    
    it('Register inventory product ownerships with different inventory product ownership identifier', async function () {
        let inventoryProductOwnershipId = '1';
        let inventoryProductId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let ownership1 = await contract.Insert(inventoryProductId, inventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(ownership1, 'TrueLog');

        truffleAssert.eventEmitted(ownership1, 'TrueInventoryProductOwnershipLog');

        let secondInventoryProductOwnershipId = '2';
        let ownership2 = await contract.Insert(inventoryProductId, secondInventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(ownership2, 'TrueLog', (ev) => {
            return ev.Type == 6 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(ownership2, 'TrueInventoryProductOwnershipLog', (ev) => {
            return ev.TicketId == ticketId && 
            ev.Version == 2 &&
            ev.Volume == ownershipVolume &&
            ev.Percentage == ownershipPercentage &&
            ev.Metadata === metadata;
        });

        await checkDetails(secondInventoryProductOwnershipId, 
            inventoryProductId,
            ticketId,
            ownershipVolume,
            ownershipPercentage,
            metadata,
            true,
            true
        );
    });
    
    it('Try to update non-existing inventory product ownership', async function () {
        await truffleAssert.fails(
            contract.Update.call('1A', '1', 1, 100, 50, 'ABC'),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });
    
    it('Update inventory product ownership', async function () {
        let inventoryProductOwnershipId = '1';
        let inventoryProductId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(inventoryProductId, inventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnershipLog');

        let secondTicketId = 2;
        let update = await contract.Update(inventoryProductId, inventoryProductOwnershipId, secondTicketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 6 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueInventoryProductOwnershipLog', (ev) => {
            return ev.TicketId == secondTicketId && 
            ev.Version == 2 &&
            ev.Volume == ownershipVolume &&
            ev.Percentage == ownershipPercentage &&
            ev.Metadata === metadata;
        });

        await checkDetails(inventoryProductOwnershipId, 
            inventoryProductId,
            secondTicketId,
            ownershipVolume,
            ownershipPercentage,
            metadata,
            true,
            true
        );
    });
    
    it('Try to delete non-existing inventory product ownership', async function () {
        await truffleAssert.fails(
            contract.Delete.call('1A', '1', 1),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });
    
    it('Delete inventory product ownership', async function () {
        let inventoryProductOwnershipId = '1';
        let inventoryProductId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(inventoryProductId, inventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnershipLog');
        let del = await contract.Delete(inventoryProductId, inventoryProductOwnershipId, ticketId);
        
        truffleAssert.eventEmitted(del, 'TrueLog', (ev) => {
            return ev.Type == 6 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(del, 'TrueInventoryProductOwnershipLog', (ev) => {
            return ev.TicketId == ticketId && 
            ev.Version == 2 &&
            ev.Volume == 0 &&
            ev.Percentage == 0 &&
            ev.Metadata === '';
        });

        await checkDetails(inventoryProductOwnershipId, '', 0, 0, 0, '', false, false);
    });
    
    it('Try to update a deleted inventory product ownership', async function () {
        let inventoryProductOwnershipId = '1';
        let inventoryProductId = '1A';
        let ticketId = 1;
        let ownershipVolume =  100;
        let ownershipPercentage = 50;
        let metadata = 'ABC';
        let create = await contract.Insert(inventoryProductId, inventoryProductOwnershipId, ticketId, ownershipVolume, ownershipPercentage, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnershipLog');
        let del = await contract.Delete(inventoryProductId, inventoryProductOwnershipId, ticketId);

        truffleAssert.eventEmitted(del, 'TrueLog');

        truffleAssert.eventEmitted(del, 'TrueInventoryProductOwnershipLog');

        let secondTicketId = 2;
        await truffleAssert.fails(
            contract.Update.call(inventoryProductId, inventoryProductOwnershipId, secondTicketId, ownershipVolume, ownershipPercentage, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid movement ownership identifier"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});