import { dateService } from './dateService.js';
import { constants } from '../../common/services/constants';

const reportTriggerService = (function () {
    function getNodeGridRequestObject(gridValues, fromList) {
        const initialDate = dateService.convertToColombian(gridValues.startDate);
        const finalDate = dateService.convertToColombian(gridValues.endDate);
        const element = {
            name: gridValues.segment,
            elementId: gridValues.segmentId
        };
        const filters = {
            element: element,
            elementName: gridValues.segment,
            elementId: gridValues.segmentId,
            nodeName: gridValues.nodeName,
            nodeId: gridValues.nodeId,
            initialDate: initialDate,
            finalDate: finalDate,
            reportType: constants.Report.OfficialNodeBalanceReport,
            nodeStatus: gridValues.status,
            ticketStatus: gridValues.ticketStatus,
            deltaNodeId: gridValues.deltaNodeId,
            fromList: fromList
        };
        const data = {
            categoryId: constants.Category.Segment,
            elementId: gridValues.segmentId,
            nodeId: gridValues.nodeId,
            startDate: initialDate,
            endDate: finalDate,
            reportTypeId: constants.ReportTypeName.OfficialNodeBalance,
            scenarioId: 'OFFICER',
            name: 'OfficialNodeBalanceReport'
        };
        return { filters, data };
    }

    return {
        buildRequestData: function (reportType, data, fromList) {
            if (reportType === constants.Report.OfficialNodeBalanceReport) {
                return getNodeGridRequestObject(data, fromList);
            }
            return null;
        }
    };
}());

export { reportTriggerService };

