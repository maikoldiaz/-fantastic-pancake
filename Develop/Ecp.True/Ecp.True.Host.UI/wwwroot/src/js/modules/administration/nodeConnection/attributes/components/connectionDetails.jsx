import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, Field } from 'redux-form';
import { openModal } from '../../../../../common/actions';
import ProductsGrid from './connectionProducts.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { utilities } from '../../../../../common/services/utilities';
import { getConnection, getAlgorithmList, setControlLimitSource, changeStatusNodeConnectionAttributes, updateConnection } from './../actions';
import { navigationService } from '../../../../../common/services/navigationService';
import { constants } from '../../../../../common/services/constants';
import Tooltip from '../../../../../common/components/tooltip/tooltip.jsx';
import classNames from 'classnames/bind';
import NumberFormatter from '../../../../../common/components/formControl/numberFormatter.jsx';
import { inputToggler } from '../../../../../common/components/formControl/formControl.jsx';

export class ConnectionDetails extends React.Component {
    constructor() {
        super();
        this.buildConnectionName = this.buildConnectionName.bind(this);
        this.buildAlgorithmName = this.buildAlgorithmName.bind(this);
        this.isDisabled = this.isDisabled.bind(this);
        this.onEdit = this.onEdit.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    buildAlgorithmName() {
        return `${utilities.getValue(this.props.connection, 'algorithm.modelName', 'N/A')}`;
    }

    buildConnectionName() {
        return `${utilities.getValue(this.props.connection, 'sourceNode.name', 'N/A')}-${utilities.getValue(this.props.connection, 'destinationNode.name', 'N/A')}`;
    }

    isDisabled() {
        return this.props.connection.sourceNode.isActive === false || this.props.connection.destinationNode.isActive === false;
    }

    onEdit() {
        if (this.isDisabled()) {
            return;
        }
        this.props.onEdit();
    }


    handleChange(event) {
        this.props.changeStatus({
            isActive: event,
            rowVersion: this.props.initialValues.rowVersion,
            sourceNodeId: this.props.initialValues.sourceNodeId,
            destinationNodeId: this.props.initialValues.destinationNodeId
        });
    }

    render() {
        return (
            <section className="ep-content">
                <header className="ep-content__header ep-content__header--h130 d-block">
                    <div className="row">
                        <div className={classNames({ ['col-md-3']: this.props.connection.isTransfer, ['col-md-6']: !this.props.connection.isTransfer })}>
                            <section className="ep-card" >
                                <div className="ep-card__icn">
                                    <i className="fas fa-project-diagram" />
                                </div>
                                <div className={
                                    classNames({
                                        ['ep-card__content']: this.props.connection.isTransfer,
                                        ['ep-card__content__full']: !this.props.connection.isTransfer
                                    })} style={this.props.connection.isTransfer ? { maxWidth: '50%' } : { maxWidth: '75%' }}>
                                    <Tooltip body={this.buildConnectionName()}>
                                        <span className="ep-card__data" id="lbl_connAttributes_name">
                                            {this.buildConnectionName()}
                                        </span>
                                    </Tooltip>
                                    <span className="ep-card__lbl">
                                        {resourceProvider.read('connection')}
                                    </span>
                                </div>
                                <form id={`frm_node_cost_center_toggle`} className="ep-form" style={this.props.connection.isTransfer ? { maxWidth: '20%' } : {}}>
                                    <div className="ep-control-group  m-b-0">
                                        <label className="ep-label"> {resourceProvider.read('state')} </label>
                                        <Field component={inputToggler} name="isActive" id="tog_category_active" onChange={this.handleChange} />
                                    </div>

                                </form>
                            </section>
                        </div>
                        <div className={classNames({ ['col-md-3']: this.props.connection.isTransfer, ['col-md-6']: !this.props.connection.isTransfer })}>
                            <section className="ep-card">
                                <div className="ep-card__icn">
                                    <i className="fas fa-tachometer-alt" />
                                </div>
                                <div className="ep-card__content">
                                    <Tooltip body={
                                        <NumberFormatter
                                            value={this.props.connection.controlLimit}
                                            displayType="text"
                                            prefix={constants.Prefix}
                                            isNumericString={true} />}>
                                        <span className="ep-card__data ep-card__data--teal" id="lbl_connAttributes_controlLimit">
                                            <NumberFormatter
                                                value={this.props.connection.controlLimit}
                                                displayType="text"
                                                prefix={constants.Prefix}
                                                isNumericString={true} />
                                        </span>
                                    </Tooltip>
                                    <span className="ep-card__lbl">
                                        {resourceProvider.read('controlLimit')}
                                    </span>
                                </div>
                                <a className={classNames('ep-card__action', { ['ep-card__action--disabled']: this.isDisabled() })}
                                    id="lnk_connAttributes_controlLimit_edit" onClick={this.onEdit}>
                                    <Tooltip body={resourceProvider.read('edit')}>
                                        <i className="fas fa-edit" />
                                    </Tooltip>
                                </a>
                            </section>
                        </div>
                        {this.props.connection.isTransfer &&
                            <>
                                <div className="col-md-3">
                                    <section className="ep-card">
                                        <div className="ep-card__content">
                                            <span className="ep-card__data ep-card__data--teal" id="lbl_connAttributes_isTransfer">
                                                <i className="far fa-check-square fs-20" />
                                                {this.props.connection.isTransfer}
                                            </span>
                                            <span className="ep-card__lbl">
                                                {resourceProvider.read('isTransferValue')}
                                            </span>
                                        </div>
                                        <a className={classNames('ep-card__action', { ['ep-card__action--disabled']: this.isDisabled() })}
                                            id="lnk_connAttributes_isTransfer_edit" onClick={this.onEdit}>
                                            <Tooltip body={resourceProvider.read('edit')}>
                                                <i className="fas fa-edit" />
                                            </Tooltip>
                                        </a>
                                    </section>
                                </div>
                                <div className="col-md-3">
                                    <section className="ep-card">
                                        <div className="ep-card__icn">
                                            <i className="fas fa-chart-line" />
                                        </div>
                                        <div className="ep-card__content">
                                            <Tooltip body={this.buildAlgorithmName()}>
                                                <span className="ep-card__data" id="lbl_connAttributes_algorithm">
                                                    {this.buildAlgorithmName()}
                                                </span>
                                            </Tooltip>
                                            <span className="ep-card__lbl">
                                                {resourceProvider.read('analyticalModelTitle')}
                                            </span>
                                        </div>
                                        <Tooltip body={resourceProvider.read('edit')}>
                                            <a className={classNames('ep-card__action', { ['ep-card__action--disabled']: this.isDisabled() })}
                                                id="lnk_connAttributes_isTransfer_edit" onClick={this.onEdit}>
                                                <i className="fas fa-edit" />
                                            </a>
                                        </Tooltip>
                                    </section>
                                </div>
                            </>}
                    </div>
                </header>
                <div className="ep-content__body">
                    <ProductsGrid />
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getConnection();
        this.props.setControlLimitSource('connectionattributes');
    }

    componentDidUpdate() {
        const { isUpdated, changeStatusFlag } = this.props;
        if (!isUpdated) {
            return;
        }
        changeStatusFlag(false);
        this.props.getConnection();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        isUpdated: state.nodeConnection.attributes.status,
        connection: state.nodeConnection.attributes.connection,
        initialValues: {
            isActive: state.nodeConnection.attributes.connection.isActive,
            rowVersion: state.nodeConnection.attributes.connection.rowVersion,
            sourceNodeId: state.nodeConnection.attributes.connection.sourceNodeId,
            destinationNodeId: state.nodeConnection.attributes.connection.destinationNodeId
        }
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: () => {
            dispatch(getAlgorithmList());
            dispatch(openModal('editConnControlLimit'));
        },
        getConnection: () => {
            dispatch(
                getConnection(navigationService.getParamByName('nodeConnectionId'))
            );
        },
        setControlLimitSource: controlLimitSource => {
            dispatch(setControlLimitSource(controlLimitSource));
        },
        changeStatus: row => {
            dispatch(updateConnection(row));
        },
        changeStatusFlag: status => {
            dispatch(changeStatusNodeConnectionAttributes(status));
        }
    };
};

/* istanbul ignore next */
const connectionDetailsForm = reduxForm({
    form: 'connectionDetails',
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(ConnectionDetails);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(connectionDetailsForm);
