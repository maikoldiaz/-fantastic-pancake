import React from 'react';
import { connect } from 'react-redux';
import TabPanels from '../../../../common/components/tabPanels/tabPanels.jsx';
import TransformationGrid from './transformationGrid.jsx';
import { routerActions } from '../../../../common/router/routerActions';
import { openModal } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';

class TransformSettingsPanel extends React.Component {
    constructor() {
        super();

        routerActions.configure('transformation', () => this.props.onTransform(this.props.activeTab));
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <TabPanels name="transformSettingsPanel" defaultTab ="movements">
                        <div title="movements"><TransformationGrid mode={this.props.route.mode} name="movements" /></div>
                        <div title="inventories"><TransformationGrid mode={this.props.route.mode} name="inventories" /></div>
                    </TabPanels>
                </div>
            </section>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        activeTab: state.tabs.transformSettingsPanel ? state.tabs.transformSettingsPanel.activeTab : null
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onTransform: activeTab => {
            dispatch(openModal('transformation', constants.Modes.Create, `${activeTab}transformation`));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(TransformSettingsPanel);
