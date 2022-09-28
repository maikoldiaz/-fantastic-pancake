import { apiService } from '../../../../src/js/common/services/apiService.js';
import { navigationService } from './navigationService';
import { httpService } from '../services/httpService.js';
import uuid from 'uuid';
import { constants } from './constants.js';
const serverValidator = (function () {
    async function getRequest(url) {
        const data = await (await (fetch(url,
            {
                method: 'GET',
                credentials: 'same-origin'
            })
            .then(response => {
                if (response.status === 401 || response.status === 500) {
                    navigationService.handleError(response.status);
                }
                return response.json();
            })
            .catch(() => { })
        ));
        return data;
    }

    async function postRequest(url, body) {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        headers.append(constants.ForgeryTokenName, httpService.readAntiforgeryToken());
        headers.append('Request-Id', uuid.v4());
        headers.append('True-Origin', 'true');
        const data = await (await (fetch(url,
            {
                method: 'POST',
                body: JSON.stringify(body),
                credentials: 'same-origin',
                headers
            })
            .then(response => {
                if (response.status === 401 || response.status === 403) {
                    if (response.status === 401 && response.headers.has('RedirectPathOnAuthFailure')) {
                        const redirectPath = response.headers.get('RedirectPathOnAuthFailure') + '?returnPath=' + window.location.pathname + window.location.search;
                        window.location = encodeURI(redirectPath);
                    }
                } else if (response.status === 404) {
                    console.warn(response);
                }

                return response.json().then(info => ({ status: response.status, body: info }));
            })
            .catch(() => { })
        ));
        return data;
    }

    return {
        validateCategoryName: name => {
            return getRequest(apiService.category.validateCategoryName(name));
        },
        validateElementName: element => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.category.validateElementName(), element));
                });
        },
        validateNodeName: name => {
            return getRequest(apiService.node.validateNodeName(name));
        },
        validateNodeOrder: (nodeId, segmentId, order) => {
            return getRequest(apiService.node.validateNodeOrder(nodeId, segmentId, order));
        },
        validateTicket: ticket => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.operationalCutOff.validate(), ticket));
                });
        },
        validateOwnership: ownership => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.ownership.validate(), ownership));
                });
        },
        validateTransformation: transformation => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.transformation.validate(), transformation));
                });
        },
        validateHomologationGroup: homologationGroup => {
            return new Promise(
                function (resolve) {
                    resolve(getRequest(apiService.homologation.validateHomologationGroup(homologationGroup)));
                });
        },
        validateLogisticsTransferPoint: transferPoint => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.nodeRelationship.logisticsTransferPointExists(), transferPoint));
                });
        },
        validateOperativeTransferPoint: transferPoint => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.nodeRelationship.operativeTransferPointExists(), transferPoint));
                });
        },
        validateAnnulation: annulation => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.annulation.exists(), annulation));
                });
        },
        validateUniqueSegmentTicket: ticket => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.operationalCutOff.validateUniqueSegmentTicket(), ticket));
                });
        },
        validateDeltaTicket: (segmentId, ticketType) => {
            return new Promise(
                function (resolve) {
                    resolve(getRequest(apiService.operationalCutOff.validateDeltaTicket(segmentId, ticketType)));
                });
        },
        validateSapTicket: ticket => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.logistic.postValidateNodesAvailables(), ticket));
                });
        },
        validateDeltaProcessingStatus: (segmentId, isOwnershipCalculation) => {
            return new Promise(
                function (resolve) {
                    resolve(getRequest(apiService.operationalDelta.validateDeltaProcessingStatus(segmentId, isOwnershipCalculation)));
                });
        },
        validateOfficialDeltaProcessingStatus: segmentId => {
            return new Promise(
                function (resolve) {
                    resolve(getRequest(apiService.officialDelta.validateTicketProcessingStatus(segmentId)));
                });
        },
        validatePreviousOfficialPeriod: ticket => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.officialDelta.validatePreviousOfficialPeriod(), ticket));
                });
        },
        validateBlockchainTransactionExists: transaction => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.blockchain.transactionExists(), transaction));
                });
        },
        validateReportExecution: (reportExecution, reportType) => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.reports.exists(reportType), reportExecution));
                });
        },
        validateBlockRangeExists: blockRange => {
            return new Promise(
                function (resolve) {
                    resolve(postRequest(apiService.blockchain.rangeExists(), blockRange));
                });
        }
    };
}());

export { serverValidator };
