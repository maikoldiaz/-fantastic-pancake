import { resourceProvider } from '../../../../common/services/resourceProvider';
import { constants } from '../../../../common/services/constants';
import { movementValidator } from '../../../../modules/transportBalance/nodeOwnership/services/movementValidator';
import { SubmissionError } from 'redux-form';
import { dispatcher } from '../../../../common/store/dispatcher';

describe('Movement Validator',
    () => {
        beforeAll(() => {
            resourceProvider.read = jest.fn(key => key);
            dispatcher.dispatch = jest.fn();
        });
        it('should throw inventoryVolumeError on submit if ownership volume for some movement and inventories is empty', () => {
            expect(() => {
                movementValidator.validateMovement({}, [
                    {
                        ownershipVolume: '',
                        ownershipPercentage: 20
                    }
                ], {});
            }).toThrow(SubmissionError);
        });
        it('should throw inventoryPercentageError on submit if ownership percentage for some movement and inventories is empty', () => {
            expect(() => {
                movementValidator.validateMovement({}, [
                    {
                        ownershipVolume: 2345,
                        ownershipPercentage: null
                    }
                ], {});
            }).toThrow(SubmissionError);
        });
        it('should throw selectSourceOrDestinationNode on submit if both source node and destination node are empty in case of selected variable type Tolerance or Unidentified loss', () => {
            const errorCheckObject = {
                sourceNodeId: 1,
                destinationNodeId: 1
            };
            const formValues = { variable: { variableTypeId: constants.VariableType.Tolerance }, netVolume: '235' };
            expect(() => {
                movementValidator.validateMovement(formValues, [
                    {
                        ownershipVolume: 2345,
                        ownershipPercentage: 100
                    }
                ], errorCheckObject);
            }).toThrow(SubmissionError);
        });
        it('should throw allowPositiveAndTwoDecimals on submit if net volume is negative or more than two decimal points', () => {
            const errorCheckObject = {
                sourceNodeId: 1,
                destinationNodeId: 1
            };
            const formValues = { variable: { variableTypeId: constants.VariableType.Input }, netVolume: '-235.987' };
            expect(() => {
                movementValidator.validateMovement(formValues, [
                    {
                        ownershipVolume: 2345,
                        ownershipPercentage: 100
                    }
                ], errorCheckObject);
            }).toThrow(SubmissionError);
        });
        it('should show error movementOwnershipError on submit if net volume is not equal to the sum of individual ownership volumes', () => {
            const errorCheckObject = {
                sourceNodeId: 1,
                destinationNodeId: 1
            };
            const formValues = { variable: { variableTypeId: constants.VariableType.Input }, netVolume: '235' };
            expect(() => {
                movementValidator.validateMovement(formValues, [
                    {
                        ownershipVolume: 2345,
                        ownershipPercentage: 100
                    }
                ], errorCheckObject);
            }).toThrow(SubmissionError);
        });
        it('should show error nodeConnectionOwnerNotAvailable on submit if there are no owners available for this input/output movement', () => {
            const errorCheckObject = {
                sourceNodeId: 1,
                destinationNodeId: 1
            };
            const formValues = { variable: { variableTypeId: constants.VariableType.Input }, netVolume: '235' };
            expect(() => {
                movementValidator.validateMovement(formValues, [], errorCheckObject);
            }).toThrow(SubmissionError);
        });
        it('should show error nodeProductOwnerNotAvailable on submit if there are no owners available for corresponding source or destination product and movement is not input/output', () => {
            const errorCheckObject = {
                sourceNodeId: 1,
                destinationNodeId: 1
            };
            const formValues = { variable: { variableTypeId: constants.VariableType.Interface }, netVolume: '235' };
            expect(() => {
                movementValidator.validateMovement(formValues, [], errorCheckObject);
            }).toThrow(SubmissionError);
        });
    });
