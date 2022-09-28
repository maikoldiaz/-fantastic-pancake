import { serverValidator } from '../../../common/services/serverValidator';
import { apiService } from '../../../common/services/apiService';

describe('server validator service',
    () => {
        it('should call validateCategoryName',
            () => {
                const mock = apiService.category.validateCategoryName = jest.fn(() => 'test');
                serverValidator.validateCategoryName('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateTicket',
            () => {
                const mock = apiService.operationalCutOff.validate = jest.fn(() => 'test');
                serverValidator.validateTicket('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateTransformation',
            () => {
                const mock = apiService.transformation.validate = jest.fn(() => 'test');
                serverValidator.validateTransformation('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateElementName',
            () => {
                const mock = apiService.category.validateElementName = jest.fn(() => 'test');
                serverValidator.validateElementName('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateNodeName',
            () => {
                const mock = apiService.node.validateNodeName = jest.fn(() => 'test');
                serverValidator.validateNodeName('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateNodeOrder',
            () => {
                const mock = apiService.node.validateNodeOrder = jest.fn(() => 'test');
                serverValidator.validateNodeOrder('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateOwnership',
            () => {
                const mock = apiService.ownership.validate = jest.fn(() => 'test');
                serverValidator.validateOwnership('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateHomologationGroup',
            () => {
                const mock = apiService.homologation.validateHomologationGroup = jest.fn(() => 'test');
                serverValidator.validateHomologationGroup('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateLogisticsTransferPoint',
            () => {
                const mock = apiService.nodeRelationship.logisticsTransferPointExists = jest.fn(() => 'test');
                serverValidator.validateLogisticsTransferPoint('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        it('should call validateOperativeTransferPoint',
            () => {
                const mock = apiService.nodeRelationship.operativeTransferPointExists = jest.fn(() => 'test');
                serverValidator.validateOperativeTransferPoint('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        
        it('should call validateAnnulation',
            () => {
                const mock = apiService.annulation.exists = jest.fn(() => 'test');
                serverValidator.validateAnnulation('test');
                expect(mock.mock.calls.length).toBe(1);
            });

        it('should call validateUniqueSegmentTicket',
            () => {
                const mock = apiService.operationalCutOff.validateUniqueSegmentTicket = jest.fn(() => 'test');
                serverValidator.validateUniqueSegmentTicket('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        
        it('should call validateDeltaTicket',
            () => {
                const mock = apiService.operationalCutOff.validateDeltaTicket = jest.fn(() => 'test');
                serverValidator.validateDeltaTicket('test');
                expect(mock.mock.calls.length).toBe(1);
            });
        
        it('should call validateDeltaProcessingStatus',
            () => {
                const mock = apiService.operationalDelta.validateDeltaProcessingStatus = jest.fn(() => 'test');
                serverValidator.validateDeltaProcessingStatus('test');
                expect(mock.mock.calls.length).toBe(1);
            });
    });
