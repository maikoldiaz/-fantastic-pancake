
export const SET_NODE_ERROR = 'SET_NODE_ERROR';
export const SET_GRID_VALUES = 'SET_GRID_VALUES';
export const REFRESH_STATUS = 'REFRESH_STATUS';
export const SET_REPORT_TRIGGER_TOGGLER = 'SET_REPORT_TRIGGER_TOGGLER';
export const SET_DELTA_NODE_SOURCE = 'SET_DELTA_NODE_SOURCE';


export const setGridValues = body => {
    return {
        type: SET_GRID_VALUES,
        data: body
    };
};
export const refreshStatus = () => {
    return {
        type: REFRESH_STATUS
    };
};

export const setReportTriggerToggler = toggle => {
    return {
        type: SET_REPORT_TRIGGER_TOGGLER,
        nodeGridToggler: toggle
    };
};

export const setDeltaNodeSource = source => {
    return {
        type: SET_DELTA_NODE_SOURCE,
        source
    };
};

export const initializeNodeError = nodeData => {
    return {
        type: SET_NODE_ERROR,
        node: nodeData
    };
};
