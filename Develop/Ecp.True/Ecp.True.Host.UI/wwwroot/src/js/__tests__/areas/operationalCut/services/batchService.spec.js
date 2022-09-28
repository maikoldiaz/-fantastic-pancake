import { batchService } from '../../../../modules/transportBalance/cutOff/services/batchService';

describe('batch service tests',
    () => {
        it('should start the batch process and return true for errors',
            () => {
                const comments = {
                    pendingTransactionErrors: [{
                        errorId: 1,
                        comment: 'test'
                    }],
                    unbalances: [],
                    officialMovements: [],
                    ticket: {
                        categoryElementId: 1
                    }
                };

                expect(batchService.start(comments)).toEqual(true);
            });

        it('should start the batch process and return true for unbalances',
            () => {
                const comments = {
                    pendingTransactionErrors: [],
                    unbalances: [{
                        nodeId: 1,
                        comment: 'test'
                    }],
                    officialMovements: [],
                    ticket: {
                        categoryElementId: 1
                    }
                };

                expect(batchService.start(comments)).toEqual(true);
            });

        it('should start the batch process and return true for transfer points',
            () => {
                const comments = {
                    pendingTransactionErrors: [],
                    unbalances: [],
                    officialMovements: [{
                        sapTrackingId: 1,
                        comment: 'test'
                    }],
                    ticket: {
                        categoryElementId: 1
                    }
                };

                expect(batchService.start(comments)).toEqual(true);
            });

        it('should start the batch process and return false',
            () => {
                const comments = {
                    pendingTransactionErrors: [],
                    unbalances: [],
                    officialMovements: [],
                    ticket: {
                        categoryElementId: 1
                    }
                };

                expect(batchService.start(comments)).toEqual(false);
            });

        it('should return session id',
            () => {
                const comments = {
                    pendingTransactionErrors: [],
                    unbalances: [],
                    officialMovements: [],
                    ticket: {
                        categoryElementId: 1
                    }
                };
                batchService.start(comments);
                const sessionId = batchService.getSessionId();

                expect(sessionId).toBeTruthy();
            });
    });

