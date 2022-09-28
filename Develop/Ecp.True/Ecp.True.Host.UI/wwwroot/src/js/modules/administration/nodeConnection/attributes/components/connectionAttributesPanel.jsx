import React from 'react';
import { connect } from 'react-redux';
import TabPanels from '../../../../../common/components/tabPanels/tabPanels.jsx';
import ConnectionAttributes from './connectionAttributes.jsx';
import NodeCostCenterGrid from './costCenter/nodeCostCenterGrid.jsx';
import { changeTab } from '../../../../../common/actions';

class ConnectionAttributesPanel extends React.Component {
    constructor() {
        super();
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <TabPanels name="connectionAttributesPanel">
                        <div title="connectionAttributes">
                            <ConnectionAttributes />
                        </div>
                        <div title="costCenter">
                            <NodeCostCenterGrid/>
                        </div>
                    </TabPanels>
                </div>
            </section>
        );
    }

    componentWillUnmount() {
        this.props.changeTabPanel('connectionAttributesPanel', 'connectionAttributes');
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        activeTab: state.tabs.connectionAttributesPanel ? state.tabs.connectionAttributesPanel.activeTab : null
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        changeTabPanel: (name, activeTab) => {
            dispatch(changeTab(name, activeTab));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ConnectionAttributesPanel);
