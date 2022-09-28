import React from 'react';
import { connect } from 'react-redux';
import { openModal, triggerPopup } from '../../../../../common/actions';
import ProductsGrid from './nodeProducts.jsx';
import { resourceProvider } from '../../../../../common/services/resourceProvider.js';
import { utilities } from '../../../../../common/services/utilities';
import { getNode } from '../actions';
import { navigationService } from '../../../../../common/services/navigationService';
import Tooltip from '../../../../../common/components/tooltip/tooltip.jsx';
import PopupFactory from '../../../../../common/components/modal/popupFactory.jsx';
import { constants } from '../../../../../common/services/constants';
import NumberFormatter from '../../../../../common/components/formControl/numberFormatter.jsx';

class NodeDetails extends React.Component {
    constructor() {
        super();
        this.buildBalance = this.buildBalance.bind(this);
        this.ruleEdit = this.ruleEdit.bind(this);
    }

    buildBalance(balance) {
        return balance ? `${balance}%` : '';
    }

    buildRuleName(rule) {
        return !utilities.isNullOrUndefined(rule) ? rule.ruleName : '';
    }

    ruleEdit() {
        this.props.onRuleEdit(this.props.node);
    }

    render() {
        return (
            <section className="ep-content">
                <header className="ep-content__header ep-content__header--h130 d-block ">
                    <div className="row">
                        <div className="col-md-3">
                            <section className="ep-card">
                                <div className="ep-card__icn"><i className="fas fa-database" /></div>
                                <div className="ep-card__content">
                                    <Tooltip body={this.props.node.name}>
                                        <span className="ep-card__data" id="lbl_nodeAttributes_name">{this.props.node.name}</span>
                                    </Tooltip>
                                    <span className="ep-card__lbl">{resourceProvider.read('node')}</span>
                                </div>
                            </section>
                        </div>
                        <div className="col-md-3">
                            <section className="ep-card">
                                <div className="ep-card__icn"><i className="fas fa-tachometer-alt" /></div>
                                <div className="ep-card__content">
                                    <Tooltip body={
                                        <NumberFormatter
                                            value={this.props.node.controlLimit}
                                            displayType="text"
                                            prefix={constants.Prefix}
                                            isNumericString={true} />
                                    }>
                                        <span className="ep-card__data ep-card__data--teal" id="lbl_nodeAttributes_controlLimit">
                                            <NumberFormatter
                                                value={this.props.node.controlLimit}
                                                displayType="text"
                                                prefix={constants.Prefix}
                                                isNumericString={true} />
                                        </span>
                                    </Tooltip>
                                    <span className="ep-card__lbl">{resourceProvider.read('controlLimit')}</span>
                                </div>
                                <a className="ep-card__action" id="lnk_nodeAttributes_controlLimit_edit" onClick={this.props.onEdit}>
                                    <Tooltip body={resourceProvider.read('edit')}><i className="fas fa-edit" /></Tooltip>
                                </a>
                            </section>
                        </div>
                        <div className="col-md-3">
                            <section className="ep-card">
                                <div className="ep-card__icn"><i className="fas fa-balance-scale" /></div>
                                <div className="ep-card__content">
                                    <Tooltip body={
                                        <NumberFormatter
                                            value={this.props.node.acceptableBalancePercentage}
                                            displayType="text"
                                            suffix={constants.Suffix}
                                            isNumericString={true} />
                                    }>
                                        <span className="ep-card__data ep-card__data--teal" id="lbl_nodeAttributes_acceptableBalancePercentage">
                                            <NumberFormatter
                                                value={this.props.node.acceptableBalancePercentage}
                                                displayType="text"
                                                suffix={constants.Suffix}
                                                isNumericString={true} />
                                        </span>
                                    </Tooltip>
                                    <span className="ep-card__lbl">{resourceProvider.read('acceptableBalance')}</span>
                                </div>
                                <a className="ep-card__action" id="lnk_nodeAttributes_acceptableBalance_edit" onClick={this.props.onEdit}>
                                    <Tooltip body={resourceProvider.read('edit')}>
                                        <i className="fas fa-edit" />
                                    </Tooltip>
                                </a>
                            </section>
                        </div>
                        <div className="col-md-3">
                            <section className="ep-card">
                                <div className="ep-card__icn"><i className="fas fa-chart-line" /></div>
                                <div className="ep-card__content">
                                    <Tooltip body={this.buildRuleName(this.props.node.nodeOwnershipRule)}>
                                        <span className="ep-card__data"
                                            id="lbl_nodeAttributes_nodeOwnershipRule">{this.buildRuleName(this.props.node.nodeOwnershipRule)}</span>
                                    </Tooltip>
                                    <span className="ep-card__lbl">{resourceProvider.read('ilFunction')}</span>
                                </div>
                                <a className="ep-card__action" id="lnk_nodeAttributes_nodeOwnershipRule_edit" onClick={this.ruleEdit}>
                                    <Tooltip body={resourceProvider.read('edit')}><i className="fas fa-edit" /></Tooltip>
                                </a>
                            </section>
                        </div>
                    </div>
                </header>
                <div className="ep-content__body">
                    <ProductsGrid />
                    <PopupFactory type={constants.RuleType.NodeAttribute} ruleNamePath="nodeOwnershipRule.ruleName" />
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getNode();
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        node: state.node.attributes.node
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onEdit: () => {
            dispatch(openModal('editNodeControlLimit'));
        },
        getNode: () => {
            dispatch(getNode(navigationService.getParamByName('nodeId')));
        },
        onRuleEdit: item => {
            dispatch(triggerPopup([item], constants.RuleType.NodeAttribute, 'nodeOwnershipRule.ruleName'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(NodeDetails);

