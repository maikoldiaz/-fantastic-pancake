import { dispatcher } from '../../../../common/store/dispatcher';
import uuid from 'uuid';
import { requestUpdateCutOffComment } from '../actions';

const batchService = (function () {
    let data = {};
    const batchSize = 100;
    let currentBatch = 1;
    let sessionId;
    let segmentId;

    function getBatch() {
        let batch = [];
        if (data.errors.length > 0) {
            if (data.errors.length >= batchSize) {
                batch = data.errors.slice((currentBatch - 1) * batchSize, batchSize);
                data.errors = data.errors.slice((currentBatch) * batchSize);
            } else {
                batch = data.errors;
                data.errors = [];
            }
            return { type: 1, sessionId, errors: batch, segmentId };
        }

        if (data.unbalances.length > 0) {
            if (data.unbalances.length >= batchSize) {
                batch = data.unbalances.slice((currentBatch - 1) * batchSize, batchSize);
                data.unbalances = data.unbalances.slice((currentBatch) * batchSize);
            } else {
                batch = data.unbalances;
                data.unbalances = [];
            }
            return { type: 2, sessionId, unbalances: batch, segmentId };
        }

        if (data.transferPoints.length > 0) {
            if (data.transferPoints.length >= batchSize) {
                batch = data.transferPoints.slice((currentBatch - 1) * batchSize, batchSize);
                data.transferPoints = data.transferPoints.slice((currentBatch) * batchSize);
            } else {
                batch = data.transferPoints;
                data.transferPoints = [];
            }
            return { type: 3, sessionId, transferPoints: batch, segmentId };
        }

        return [];
    }
    function processNext() {
        const batch = getBatch();
        if (batch.length === 0) {
            return false;
        }

        dispatcher.dispatch(requestUpdateCutOffComment(batch));
        this.currentBatch++;
        return true;
    }

    function start(comments) {
        currentBatch = 1;
        const commentsObject = {
            errors: comments.pendingTransactionErrors,
            unbalances: comments.unbalances,
            transferPoints: comments.officialMovements
        };
        data = commentsObject;
        segmentId = comments.ticket.categoryElementId;
        sessionId = uuid();
        return this.processNext();
    }

    function getSessionId() {
        return sessionId;
    }

    return {
        start: start,
        processNext: processNext,
        getSessionId: getSessionId
    };
}());
export { batchService };
