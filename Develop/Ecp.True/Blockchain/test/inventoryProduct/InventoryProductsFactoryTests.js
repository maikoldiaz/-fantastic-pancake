const InventoryProductsFactory = artifacts.require("InventoryProductsFactory");
const truffleAssert = require('truffle-assertions');

contract('InventoryProductsFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    async function checkDetails(transactionId, uniqueId, inventoryProductId, productVolume, measurementUnit, inventoryDate, expectedTransactionId, metadata, exists, deletedCase = false) {
        let inventoryProductByTransactionId = await contract.GetByTransactionId.call(transactionId);

        expect(inventoryProductByTransactionId.InventoryProductId).to.equal(inventoryProductId);
        expect(toNumbers(inventoryProductByTransactionId.ProductVolume)).to.equal(productVolume);
        expect(inventoryProductByTransactionId.MeasurementUnit).to.equal(measurementUnit);
        expect(toNumbers(inventoryProductByTransactionId.InventoryDate)).to.equal(inventoryDate);
        expect(inventoryProductByTransactionId.TransactionId).to.equal(expectedTransactionId);
        expect(inventoryProductByTransactionId.Metadata).to.equal(metadata);
        expect(inventoryProductByTransactionId.Exists).to.equal(exists);

        if(!deletedCase) {
            let inventoryProductByUniqueId = await contract.GetByInventoryProductUniqueId.call(uniqueId);
            assert.equal(inventoryProductByTransactionId.InventoryProductId,inventoryProductByUniqueId.InventoryProductId);
            assert.equal(toNumbers(inventoryProductByTransactionId.ProductVolume),toNumbers(inventoryProductByUniqueId.ProductVolume));
            assert.equal(inventoryProductByTransactionId.MeasurementUnit,inventoryProductByUniqueId.MeasurementUnit);
            assert.equal(toNumbers(inventoryProductByTransactionId.InventoryDate),toNumbers(inventoryProductByUniqueId.InventoryDate));
            assert.equal(inventoryProductByTransactionId.TransactionId,inventoryProductByUniqueId.TransactionId);
            assert.equal(inventoryProductByTransactionId.Metadata,inventoryProductByUniqueId.Metadata);
            assert.equal(inventoryProductByTransactionId.Exists,inventoryProductByUniqueId.Exists);
        }
    }


    beforeEach(async function () {
        contract = await InventoryProductsFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no inventory product by default', async function () {
        await checkDetails('1','1A', '', 0, '', 0, '', '', false);
    });
    
    it('Register inventory product and verify details', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 3 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueInventoryProductLog', (ev) => {
            return ev.InventoryDate == inventoryDate && 
            ev.Version == 2 &&
            ev.Volume == productVolume &&
            ev.Unit === measurementUnit &&
            ev.Metadata === metadata;
        });

        await checkDetails(transactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Try to register inventory products with same inventory product identifier', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductLog');

        await truffleAssert.fails(
            contract.Insert.call(inventoryProductId, '2', 2, metadata, productVolume, measurementUnit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Inventory product identifier is already present"
        );

        await checkDetails(transactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Register inventory products with different inventory product identifier', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let ip1 = await contract.Insert(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(ip1, 'TrueLog');

        truffleAssert.eventEmitted(ip1, 'TrueInventoryProductLog');

        inventoryProductId = '2A';
        let ip2 = await contract.Insert(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(ip2, 'TrueLog', (ev) => {
            return ev.Type == 3 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(ip2, 'TrueInventoryProductLog', (ev) => {
            return ev.InventoryDate == inventoryDate && 
            ev.Version == 2 &&
            ev.Volume == productVolume &&
            ev.Unit === measurementUnit &&
            ev.Metadata === metadata;
        });

        await checkDetails(transactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Try to update inventory product which is not present', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';        

        await truffleAssert.fails(
            contract.Update.call(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Inventory product identifier must already be present"
        );
    });
    
    it('Try to update inventory product to a non-unique transaction id', async function () {
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let ip1 = await contract.Insert(inventoryProductId, firstTransactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(ip1, 'TrueLog');

        truffleAssert.eventEmitted(ip1, 'TrueInventoryProductLog');

        inventoryProductId = '2A';
        let secondTransactionId = '2';
        let ip2 = await contract.Insert(inventoryProductId, secondTransactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(ip2, 'TrueLog');

        truffleAssert.eventEmitted(ip2, 'TrueInventoryProductLog');

        await truffleAssert.fails(
            contract.Update.call(inventoryProductId, firstTransactionId, inventoryDate, metadata, productVolume, measurementUnit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Inventory product transaction identifier must be unique"
        );

        await checkDetails(secondTransactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            secondTransactionId,
            metadata,
            true
        );
    });
    
    it('Update inventory product and verify details', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductLog');

        let secondTransactionId = '2';
        let update = await contract.Update(inventoryProductId, secondTransactionId, inventoryDate, metadata, productVolume, measurementUnit);
        
        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 3 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueInventoryProductLog', (ev) => {
            return ev.InventoryDate == inventoryDate && 
            ev.Version == 2 &&
            ev.Volume == productVolume &&
            ev.Unit === measurementUnit &&
            ev.Metadata === metadata;
        });

        await checkDetails(secondTransactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            secondTransactionId,
            metadata,
            true
        );
    });
    
    it('Try to delete inventory product which is not present', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';    

        await truffleAssert.fails(
            contract.Delete.call(inventoryProductId, transactionId, inventoryDate, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Inventory product identifier is already present"
        );
    });
    
    it('Try to delete a inventory product with duplicate transaction identifiers', async function () {
        let inventoryProductId = '1A';
        let transactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(inventoryProductId, transactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductLog');

        await truffleAssert.fails(
            contract.Delete.call(inventoryProductId, transactionId, inventoryDate, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Inventory product transaction identifier must be unique"
        );

        await checkDetails(transactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Delete a inventory product', async function () {
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(inventoryProductId, firstTransactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductLog');

        let secondTransactionId = '2';
        let del = await contract.Delete(inventoryProductId, secondTransactionId, inventoryDate, metadata);

        truffleAssert.eventEmitted(del, 'TrueLog', (ev) => {
            return ev.Type == 3 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(del, 'TrueInventoryProductLog', (ev) => {
            return ev.InventoryDate == inventoryDate && 
            ev.Version == 2 &&
            ev.Volume == 0 &&
            ev.Unit === '' &&
            ev.Metadata === metadata;
        });

        await checkDetails(firstTransactionId, 
            inventoryProductId,
            inventoryProductId,
            productVolume,
            measurementUnit,
            inventoryDate,
            firstTransactionId,
            metadata,
            true,
            true
        );

        let inventoryProductByUniqueId = await contract.GetByInventoryProductUniqueId.call(inventoryProductId);
        assert.equal(inventoryProductByUniqueId.InventoryProductId, '');
        assert.equal(toNumbers(inventoryProductByUniqueId.ProductVolume),0);
        assert.equal(inventoryProductByUniqueId.MeasurementUnit,'');
        assert.equal(toNumbers(inventoryProductByUniqueId.InventoryDate),0);
        assert.equal(inventoryProductByUniqueId.TransactionId,'');
        assert.equal(inventoryProductByUniqueId.Metadata,'');
        assert.equal(inventoryProductByUniqueId.Exists,false);

        await checkDetails(secondTransactionId, 
            inventoryProductId,
            '',
            0,
            '',
            0,
            '',
            '',
            false
        );
    });
    
    it('Try to update a deleted inventory product', async function () {
        let inventoryProductId = '1A';
        let firstTransactionId = '1';
        let inventoryDate = 1;
        let metadata = 'ABC';
        let productVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(inventoryProductId, firstTransactionId, inventoryDate, metadata, productVolume, measurementUnit);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueInventoryProductLog');

        let secondTransactionId = '2';
        let del = await contract.Delete(inventoryProductId, secondTransactionId, inventoryDate, metadata);

        truffleAssert.eventEmitted(del, 'TrueLog');

        truffleAssert.eventEmitted(del, 'TrueInventoryProductLog');

        let thirdTransactionId = '3';
        await truffleAssert.fails(
            contract.Update.call(inventoryProductId, thirdTransactionId, inventoryDate, metadata, productVolume, measurementUnit),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Inventory product identifier must already be present"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});