const InventoryProductOwnersFactory = artifacts.require("InventoryProductOwnersFactory");
const truffleAssert = require('truffle-assertions');

contract('InventoryProductOwnersFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    async function checkDetails(inventoryProductOwnerId, inventoryProductId, transactionId, ownerId, value, unit, exists, shouldHaveOwner) {
        let inventoryProductOwner = await contract.Get.call(inventoryProductOwnerId);

        expect(inventoryProductOwner.InventoryProductId).to.equal(inventoryProductId);
        expect(inventoryProductOwner.TransactionId).to.equal(transactionId);
        expect(inventoryProductOwner.OwnerId).to.equal(ownerId);
        expect(toNumbers(inventoryProductOwner.Value)).to.equal(value);
        expect(inventoryProductOwner.Unit).to.equal(unit);
        expect(inventoryProductOwner.Exists).to.equal(exists);

        let hasOwner = await contract.HasOwner.call(inventoryProductOwnerId);
        expect(hasOwner).to.equal(shouldHaveOwner);
    }


    beforeEach(async function () {
        contract = await InventoryProductOwnersFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no inventory product owner by default', async function () {
        await checkDetails('1','', '','', 0, '', false, false);
    });
    
    it('Register inventory product owner and verify details', async function () {
        let inventoryProductOwnerId = '1';
        let inventoryProductId = '1A';
        let transactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(inventoryProductOwnerId, inventoryProductId, ownerId, transactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 4 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == value &&
            ev.Unit === unit;
        });

        await checkDetails(inventoryProductOwnerId, 
            inventoryProductId,
            transactionId,
            ownerId,
            value,
            unit,
            true,
            true
        );
    });
    
    it('Try to register inventory product owners with same inventory product owner identifier', async function () {
        let inventoryProductOwnerId = '1';
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let owner1 = await contract.Insert(inventoryProductOwnerId, inventoryProductId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(owner1, 'TrueLog');

        truffleAssert.eventEmitted(owner1, 'TrueInventoryProductOwnerLog');

        let secondTransactionId = '2';

        await truffleAssert.fails(
            contract.Insert.call(inventoryProductOwnerId, inventoryProductId, ownerId, secondTransactionId, value, unit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid inventory product owner identifier"
        );
    });
    
    it('Register inventory product owners with different inventory product owner identifier', async function () {
        let inventoryProductOwnerId = '1';
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let owner1 = await contract.Insert(inventoryProductOwnerId, inventoryProductId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(owner1, 'TrueLog');

        truffleAssert.eventEmitted(owner1, 'TrueInventoryProductOwnerLog');

        let secondInventoryProductOwnerId = '2';
        let secondTransactionId = '2';
        let owner2 = await contract.Insert(secondInventoryProductOwnerId, inventoryProductId, ownerId, secondTransactionId, value, unit);

        truffleAssert.eventEmitted(owner2, 'TrueLog', (ev) => {
            return ev.Type == 4 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(owner2, 'TrueInventoryProductOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == value &&
            ev.Unit === unit;
        });

        await checkDetails(secondInventoryProductOwnerId, 
            inventoryProductId,
            secondTransactionId,
            ownerId,
            value,
            unit,
            true,
            true
        );
    });
    
    it('Try to update non-existing inventory product owner', async function () {
        await truffleAssert.fails(
            contract.Update.call('1', '1A', '1', '1', 100, 'Bbl'),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid inventory product owner identifier"
        );
    });
    
    it('Update inventory product owner', async function () {
        let inventoryProductOwnerId = '1';
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(inventoryProductOwnerId, inventoryProductId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnerLog');
        let secondTransactionId = '2';
        let update = await contract.Update(inventoryProductOwnerId, inventoryProductId, ownerId, secondTransactionId, value, unit);

        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 4 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueInventoryProductOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == value &&
            ev.Unit === unit;
        });

        await checkDetails(inventoryProductOwnerId, 
            inventoryProductId,
            secondTransactionId,
            ownerId,
            value,
            unit,
            true,
            true
        );
    });
    
    it('Try to delete non-existing inventory product owner', async function () {
        await truffleAssert.fails(
            contract.Delete.call('1', '1A', '1'),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid inventory product owner identifier"
        );
    });
    
    it('Delete inventory product owner', async function () {
        let inventoryProductOwnerId = '1';
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(inventoryProductOwnerId, inventoryProductId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnerLog');
        let del = await contract.Delete(inventoryProductOwnerId, inventoryProductId, ownerId);

        truffleAssert.eventEmitted(del, 'TrueLog', (ev) => {
            return ev.Type == 4 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(del, 'TrueInventoryProductOwnerLog', (ev) => {
            return ev.OwnerId === ownerId && 
            ev.Version == 2 &&
            ev.Value == 0 &&
            ev.Unit === '';
        });

        await checkDetails(inventoryProductOwnerId, '', '','', 0, '', false, false);
    });
    
    it('Try to update a deleted inventory product owner', async function () {
        let inventoryProductOwnerId = '1';
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let ownerId = '1';
        let value = 100;
        let unit = 'Bbl';
        let create = await contract.Insert(inventoryProductOwnerId, inventoryProductId, ownerId, firstTransactionId, value, unit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductOwnerLog');
        let del = await contract.Delete(inventoryProductOwnerId, inventoryProductId, ownerId);

        truffleAssert.eventEmitted(del, 'TrueLog');

        truffleAssert.eventEmitted(del, 'TrueInventoryProductOwnerLog');

        let secondTransactionId = '2'
        await truffleAssert.fails(
            contract.Update.call(inventoryProductOwnerId, inventoryProductId, ownerId, secondTransactionId, value, unit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Invalid inventory product owner identifier"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});