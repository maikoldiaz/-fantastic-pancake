const MovementsFactory = artifacts.require("MovementsFactory");
const truffleAssert = require('truffle-assertions');

contract('MovementsFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    const ERROR_MSG = 'Returned error: VM Exception while processing transaction: revert';

    async function checkDetails(transactionId, movementIdToGet, movementId, netStandardVolume, measurementUnit, operationalDate, expectedTransactionId, metadata, exists) {
        let movementByTransactionId = await contract.GetByTransactionId.call(transactionId);

        expect(movementByTransactionId.MovementId).to.equal(movementId);
        expect(toNumbers(movementByTransactionId.NetStandardVolume)).to.equal(netStandardVolume);
        expect(movementByTransactionId.MeasurementUnit).to.equal(measurementUnit);
        expect(toNumbers(movementByTransactionId.OperationalDate)).to.equal(operationalDate);
        expect(movementByTransactionId.TransactionId).to.equal(expectedTransactionId);
        expect(movementByTransactionId.Metadata).to.equal(metadata);
        expect(movementByTransactionId.Exists).to.equal(exists);

        let movementByMovementId = await contract.GetByMovementId.call(movementIdToGet);
        assert.equal(movementByTransactionId.MovementId,movementByMovementId.MovementId);
        assert.equal(toNumbers(movementByTransactionId.NetStandardVolume),toNumbers(movementByMovementId.NetStandardVolume));
        assert.equal(movementByTransactionId.MeasurementUnit,movementByMovementId.MeasurementUnit);
        assert.equal(toNumbers(movementByTransactionId.OperationalDate),toNumbers(movementByMovementId.OperationalDate));
        assert.equal(movementByTransactionId.TransactionId,movementByMovementId.TransactionId);
        assert.equal(movementByTransactionId.Metadata,movementByMovementId.Metadata);
        assert.equal(movementByTransactionId.Exists,movementByMovementId.Exists);
    }


    beforeEach(async function () {
        contract = await MovementsFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no movement by default', async function () {
        await checkDetails('1','1A', '', 0, '', 0, '', '', false);
    });
    
    it('Register movement and verify details', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog', (ev) => {
            return ev.Type == 1 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(create, 'TrueMovementLog', (ev) => {
            return ev.OperationalDate == operationalDate && 
            ev.Version == 2 &&
            ev.Volume == netStandardVolume &&
            ev.Unit === measurementUnit &&
            ev.Metadata === metadata;
        });

        await checkDetails(transactionId, 
            movementId,
            movementId,
            netStandardVolume,
            measurementUnit,
            operationalDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Try to register movements with same movement identifier', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementLog');

        await truffleAssert.fails(
            contract.Insert.call(movementId, netStandardVolume, '2', 2, measurementUnit, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Movement identifier is already present"
        );

        await checkDetails(transactionId, 
            movementId,
            movementId,
            netStandardVolume,
            measurementUnit,
            operationalDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Register movements with different movement identifier', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let ip1 = await contract.Insert(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(ip1, 'TrueLog');

        truffleAssert.eventEmitted(ip1, 'TrueMovementLog');

        movementId = '2A';
        let ip2 = await contract.Insert(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(ip2, 'TrueLog', (ev) => {
            return ev.Type == 1 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(ip2, 'TrueMovementLog', (ev) => {
            return ev.OperationalDate == operationalDate && 
            ev.Version == 2 &&
            ev.Volume == netStandardVolume &&
            ev.Unit === measurementUnit &&
            ev.Metadata === metadata;
        });

        await checkDetails(transactionId, 
            movementId,
            movementId,
            netStandardVolume,
            measurementUnit,
            operationalDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Try to update movement which is not present', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';        

        await truffleAssert.fails(
            contract.Update.call(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Movement identifier must be present"
        );
    });
    
    it('Try to update movement to a non-unique transaction id', async function () {
        let movementId = '1A';
        let firstTransactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let ip1 = await contract.Insert(movementId, netStandardVolume, firstTransactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(ip1, 'TrueLog');

        truffleAssert.eventEmitted(ip1, 'TrueMovementLog');

        movementId = '2A';
        let secondTransactionId = '2';
        let ip2 = await contract.Insert(movementId, netStandardVolume, secondTransactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(ip2, 'TrueLog');

        truffleAssert.eventEmitted(ip2, 'TrueMovementLog');

        await truffleAssert.fails(
            contract.Update.call(movementId, netStandardVolume, firstTransactionId, operationalDate, measurementUnit, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Movement transaction identifier must be unique"
        );

        await checkDetails(secondTransactionId, 
            movementId,
            movementId,
            netStandardVolume,
            measurementUnit,
            operationalDate,
            secondTransactionId,
            metadata,
            true
        );
    });
    
    it('Update movement and verify details', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementLog');

        let secondTransactionId = '2';
        let update = await contract.Update(movementId, netStandardVolume, secondTransactionId, operationalDate, measurementUnit, metadata);
        
        truffleAssert.eventEmitted(update, 'TrueLog', (ev) => {
            return ev.Type == 1 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(update, 'TrueMovementLog', (ev) => {
            return ev.OperationalDate == operationalDate && 
            ev.Version == 2 &&
            ev.Volume == netStandardVolume &&
            ev.Unit === measurementUnit &&
            ev.Metadata === metadata;
        });

        await checkDetails(secondTransactionId, 
            movementId,
            movementId,
            netStandardVolume,
            measurementUnit,
            operationalDate,
            secondTransactionId,
            metadata,
            true
        );
    });
    
    it('Try to delete movement which is not present', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';    

        await truffleAssert.fails(
            contract.Delete.call(movementId, transactionId, operationalDate, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Movement identifier must be present"
        );
    });
    
    it('Try to delete a movement with duplicate transaction identifiers', async function () {
        let movementId = '1A';
        let transactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(movementId, netStandardVolume, transactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementLog');

        await truffleAssert.fails(
            contract.Delete.call(movementId, transactionId, operationalDate, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Movement transaction identifier must be unique"
        );

        await checkDetails(transactionId, 
            movementId,
            movementId,
            netStandardVolume,
            measurementUnit,
            operationalDate,
            transactionId,
            metadata,
            true
        );
    });
    
    it('Delete a movement', async function () {
        let movementId = '1A';
        let firstTransactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(movementId, netStandardVolume, firstTransactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementLog');

        let secondTransactionId = '2';
        let del = await contract.Delete(movementId, secondTransactionId, operationalDate, metadata);

        truffleAssert.eventEmitted(del, 'TrueLog', (ev) => {
            return ev.Type == 1 && 
            ev.Version == 2 &&
            ev.Data === '';
        });

        truffleAssert.eventEmitted(del, 'TrueMovementLog', (ev) => {
            return ev.OperationalDate == operationalDate && 
            ev.Version == 2 &&
            ev.Volume == 0 &&
            ev.Unit === '' &&
            ev.Metadata === metadata;
        });

        await checkDetails(firstTransactionId, 
            movementId,
            '',
            0,
            '',
            0,
            '',
            '',
            false
        );

        await checkDetails(secondTransactionId, 
            movementId,
            '',
            0,
            '',
            0,
            '',
            '',
            false
        );
    });
    
    it('Try to update a deleted movement', async function () {
        let movementId = '1A';
        let firstTransactionId = '1';
        let operationalDate = 1;
        let metadata = 'ABC';
        let netStandardVolume = 100;
        let measurementUnit = 'Bbl';
        let create = await contract.Insert(movementId, netStandardVolume, firstTransactionId, operationalDate, measurementUnit, metadata);

        truffleAssert.eventEmitted(create, 'TrueLog');

        truffleAssert.eventEmitted(create, 'TrueMovementLog');

        let secondTransactionId = '2';
        let del = await contract.Delete(movementId, secondTransactionId, operationalDate, metadata);

        truffleAssert.eventEmitted(del, 'TrueLog');

        truffleAssert.eventEmitted(del, 'TrueMovementLog');

        let thirdTransactionId = '3';
        await truffleAssert.fails(
            contract.Update.call(movementId, netStandardVolume, thirdTransactionId, operationalDate, measurementUnit, metadata),
            truffleAssert.ErrorType.REVERT,
            ERROR_MSG+" Movement identifier must be present"
        );
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});