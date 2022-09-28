import { calculationService } from './../../../../modules/transportBalance/nodeOwnership/services/calculationService';

describe('calculation service',
    () => {
        it('should sum ownership balance groups',
            () => {
                expect(calculationService.sumOwnershipBalanceGroup([10, 20])).toBe('30.00');
            });
        it('should calculate total control percent when no rows are passed',
            () => {
                expect(calculationService.calculateTotalControlPercent({ data: {} })).toBeNull();
            });
        it('should calculate total control percent',
            () => {
                expect(calculationService.calculateTotalControlPercent({ data: [{ volume: 10, inputs: 20 }, { volume: 10, inputs: 20 }] })).toMatchObject({ inputsTotal: 40, volumeTotal: 20 });
            });
        it('should calculate control percent when data has row',
            () => {
                expect(calculationService.calculateControlPercent({ row: { volume: 10, inputs: 20 } })).toMatchObject({ inputs: 20, volume: 10 });
            });
        it('should calculate control percent when data has no row',
            () => {
                expect(calculationService.calculateControlPercent({})).toBeNull();
            });
        it('should aggregate text col template when row is passed',
            () => {
                expect(calculationService.aggregatedTextColTemplate([10, 20])).toBe(10);
            });
        it('should aggregate text col template return null when no row is passed',
            () => {
                expect(calculationService.aggregatedTextColTemplate()).toBeNull();
            });
        it('should get total sum',
            () => {
                expect(calculationService.getTotalSum({ data: [{ testKey: 20 }, { testKey: 30 }] }, 'testKey')).toBe(50);
            });
        it('should get sum total ownership balance when rows has single data',
            () => {
                expect(calculationService.sumTotalOwnershipBalance({ data: [{ testKey: 20 }] }, 'testKey')).toMatch('20.00');
            });
        it('should get sum total ownership balance when rows has more than one data',
            () => {
                expect(calculationService.sumTotalOwnershipBalance({ data: [{ testKey: 20 }, { testKey: 30 }] }, 'testKey')).toBe(50);
            });
        it('should calculate sum total volume percentage ownership when rows has one data',
            () => {
                expect(calculationService.sumTotalVolumePercentageOwnership({ data: [{ testKey: 20 }] }, 'testKey')).toBe(20);
            });
        it('should calculate sum total volume percentage ownership when rows has more than one data',
            () => {
                expect(calculationService.sumTotalVolumePercentageOwnership({ data: [{ testKey: 20 }, { testKey: 30 }] }, 'testKey')).toBe(50);
            });
        it('should calculate',
            () => {
                expect(calculationService.calculate(1234,
                    [{ variableTypeId: 'initialInventory' }], [{ inputs: 10, outputs: 20, identifiedLosses: 30, interface: 40, tolerance: 50, unidentifiedLosses: 60 }]));
            });
        it('should calculate input',
            () => {
                const summary = [];
                summary.push({
                    createdBy: null,
                    createdDate: null,
                    finalInventory: 0.00,
                    identifiedLosses: 0.00,
                    initialInventory: 0.00,
                    inputs: 216871.82,
                    interface: 0.00,
                    measurementUnit: 'Bbl',
                    outputs: 0.00,
                    owner: 'ECOPETROL',
                    product: 'CRUDO CAMPO MAMBO',
                    productId: 10000002318,
                    tolerance: -693.40,
                    unidentifiedLosses: -216178.42,
                    volume: 0
                });
                summary.push({
                    createdBy: null,
                    createdDate: null,
                    finalInventory: 0.00,
                    identifiedLosses: 0.00,
                    initialInventory: 0.00,
                    inputs: 416871.82,
                    interface: 0.00,
                    measurementUnit: 'Bbl',
                    outputs: 0.00,
                    owner: 'ECOPETROL',
                    product: 'CRUDO CAMPO CUSUCO',
                    productId: 10000002372,
                    tolerance: -1332.85,
                    unidentifiedLosses: -415538.97,
                    volume: 0
                });

                const data = [];
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_eh814',
                    destinationNodeId: 29254,
                    destinationProduct: 'CRUDO CAMPO MAMBO',
                    destinationProductId: 10000002318,
                    isMovement: 1,
                    movementId: '1833675',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 100,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 30,
                    ownerName: 'ECOPETROL',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 100,
                    sourceNode: 'Automation_nou10',
                    sourceNodeId: 29251,
                    sourceProduct: 'CRUDO CAMPO MAMBO',
                    sourceProductId: 10000002318,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 5
                });
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_eh814',
                    destinationNodeId: 29254,
                    destinationProduct: 'CRUDO CAMPO CUSUCO',
                    destinationProductId: 10000002372,
                    isMovement: 1,
                    movementId: '3963394',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 200,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 30,
                    ownerName: 'ECOPETROL',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 200,
                    sourceNode: 'Automation_nou10',
                    sourceNodeId: 29251,
                    sourceProduct: 'CRUDO CAMPO CUSUCO',
                    sourceProductId: 10000002372,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 5
                });
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_eh814',
                    destinationNodeId: 29254,
                    destinationProduct: 'CRUDO CAMPO MAMBO',
                    destinationProductId: 10000002318,
                    isMovement: 1,
                    movementId: '3963394',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 300,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 27,
                    ownerName: 'REFICAR',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 300,
                    sourceNode: 'Automation_nou10',
                    sourceNodeId: 29251,
                    sourceProduct: 'CRUDO CAMPO MAMBO',
                    sourceProductId: 10000002318,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 5
                });
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_eh814',
                    destinationNodeId: 29254,
                    destinationProduct: 'CRUDO CAMPO MAMBO',
                    destinationProductId: 10000002318,
                    isMovement: 1,
                    movementId: '3963394',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 400,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 29,
                    ownerName: 'EQUION',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 400,
                    sourceNode: 'Automation_nou10',
                    sourceNodeId: 29251,
                    sourceProduct: 'CRUDO CAMPO MAMBO',
                    sourceProductId: 10000002318,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 6
                });
                calculationService.calculate(29254, data, summary);
                expect(summary).toHaveLength(4);
                // REFICAR owner
                expect(data[3].owner).toEqual(summary[3].ownerName);
                expect(data[3].destinationProductId).toEqual(summary[3].productId);
                expect(data[3].netVolume).toEqual(summary[3].inputs);
            });
        it('should calculate output',
            () => {
                const summary = [];
                summary.push({
                    createdBy: null,
                    createdDate: null,
                    finalInventory: 0.00,
                    identifiedLosses: 0.00,
                    initialInventory: 0.00,
                    inputs: 0,
                    interface: 0.00,
                    measurementUnit: 'Bbl',
                    outputs: 116871.82,
                    owner: 'ECOPETROL',
                    product: 'CRUDO CAMPO CUSUCO',
                    productId: 10000002372,
                    tolerance: -693.40,
                    unidentifiedLosses: -116871.82,
                    volume: 0
                });
                const data = [];
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_nou10',
                    destinationNodeId: 29251,
                    destinationProduct: 'CRUDO CAMPO CUSUCO',
                    destinationProductId: 10000002372,
                    isMovement: 1,
                    movementId: '3963394',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 100,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 30,
                    ownerName: 'ECOPETROL',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 100,
                    sourceNode: 'Automation_092gi',
                    sourceNodeId: 29253,
                    sourceProduct: 'CRUDO CAMPO CUSUCO',
                    sourceProductId: 10000002372,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 6
                });
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_nou10',
                    destinationNodeId: 29251,
                    destinationProduct: 'CRUDO CAMPO MAMBO',
                    destinationProductId: 10000002318,
                    isMovement: 1,
                    movementId: '3963394',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 200,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 27,
                    ownerName: 'REFICAR',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 200,
                    sourceNode: 'Automation_092gi',
                    sourceNodeId: 29253,
                    sourceProduct: 'CRUDO CAMPO MAMBO',
                    sourceProductId: 10000002318,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 5
                });
                data.push({
                    color: '#00903B',
                    destinationNode: 'Automation_nou10',
                    destinationNodeId: 29251,
                    destinationProduct: 'CRUDO CAMPO MAMBO',
                    destinationProductId: 10000002318,
                    isMovement: 1,
                    movementId: '3963394',
                    movementType: 'Automation_test',
                    movementTypeId: 129951,
                    netVolume: 300,
                    operationalDate: '2020-07-02T00:00:00',
                    ownerId: 29,
                    ownerName: 'EQUION',
                    ownershipFunction: '1',
                    ownershipPercentage: 100.00,
                    ownershipVolume: 300,
                    sourceNode: 'Automation_092gi',
                    sourceNodeId: 29253,
                    sourceProduct: 'CRUDO CAMPO MAMBO',
                    sourceProductId: 10000002318,
                    transactionId: 20313,
                    unit: 'Bbl',
                    variableTypeId: 6
                });
                calculationService.calculate(29253, data, summary);
                expect(summary).toHaveLength(3);
                // REFICAR owner
                expect(data[2].owner).toEqual(summary[2].ownerName);
                expect(data[2].sourceProductId).toEqual(summary[2].productId);
                expect(data[2].netVolume).toEqual(summary[2].outputs);
            });
    });
