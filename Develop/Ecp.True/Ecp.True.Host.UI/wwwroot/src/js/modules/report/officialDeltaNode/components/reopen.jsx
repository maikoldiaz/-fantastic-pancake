import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputTextarea } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { utilities } from '../../../../common/services/utilities';
import { reopenDeltaNode, setSubmissionType } from '../actions';
import { navigationService } from '../../../../common/services/navigationService';
import { constants } from '../../../../common/services/constants';
import GridNoData from '../../../../common/components/grid/gridNoData.jsx';

class Reopen extends React.Component {
    constructor() {
        super();
        this.onSubmit = this.onSubmit.bind(this);
    }
    onSubmit(data) {
        const deltaNodeIdArr = [];
        deltaNodeIdArr.push(this.props.filters.deltaNodeId);
        if (this.props.submissionType === constants.ReopenDeltaNodeActionType.DeltaNodeAndSuccessor) {
            if (this.props.reopenNodesList && this.props.reopenNodesList.length > 0) {
                this.props.reopenNodesList.forEach(reopenNode => {
                    deltaNodeIdArr.push(reopenNode.deltaNode);
                });
            }
        }
        const request = {
            comment: data.note,
            deltaNodeId: deltaNodeIdArr
        };
        this.props.reopenNodes(request);
    }

    render() {
        return (
            <form id={`frm_${this.props.mode}_deltanode`} className="ep-form" onSubmit={this.props.handleSubmit(this.onSubmit)}>
                <section className="ep-modal__content">
                    <div className="ep-control-group">
                        <div className="d-flex">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('node')}:</label>
                            <span className="ep-data fw-bold">{this.props.filters.nodeName}</span>
                        </div>
                        <div className="d-flex">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('segment')}:</label>
                            <span className="ep-data fw-bold">{this.props.filters.elementName}</span>
                        </div>
                        <div className="d-flex">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('period')}:</label>
                            <span className="ep-data text-caps fw-bold m-r-1">{this.props.filters.initialDateShort}</span>
                            <span className="fw-sb fc-label m-r-1">{resourceProvider.read('to')}</span>
                            <span className="ep-data text-caps fw-bold">{this.props.filters.finalDateShort}</span>
                        </div>
                    </div>
                    <div className="ep-control-group">
                        <div className="ep-control-group m-b-0 m-r-6">
                            <span className="ep-data fw-bold">{resourceProvider.read('reopenNodesDependencyInfo1')}</span>
                            <span className="ep-data fw-bold">{resourceProvider.read('reopenNodesDependencyInfo2')}</span>
                        </div>
                    </div>
                    <div className="ep-control-group m-x-8">
                        <div className="d-flex">
                            <label className="ep-label d-inline-block m-r-1">{resourceProvider.read('totalNodes')}:</label>
                            <span className="ep-data m-l-2 fw-600 fc-green">{this.props.reopenNodesList ? this.props.reopenNodesList.length : 0}</span>
                        </div>
                        <section className="ep-table-wrap">
                            <div className="ep-table ep-table--smpl ep-table--alt-row ep-table--mh120 ep-table--h170" id="tbl_validate_unapprovedNodes">
                                <table>
                                    <colgroup>
                                        <col style={{ width: '30%' }} />
                                        <col style={{ width: '40%' }} />
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            <th className=" fw-bold">{resourceProvider.read('segment')}</th>
                                            <th className=" fw-bold">{resourceProvider.read('nodeName')}</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {
                                            this.props.reopenNodesList.map((item, index) => {
                                                return (
                                                    <tr key={`row-${index}`}>
                                                        <td>{item.segment}</td>
                                                        <td>{item.node}</td>
                                                    </tr>
                                                );
                                            })
                                        }
                                    </tbody>
                                </table>
                                {this.props.reopenNodesList.length === 0 && <GridNoData {...this.props} />}
                            </div>
                        </section>
                    </div>
                    <div className="ep-control-group">
                        <label className="ep-label" htmlFor="txtarea_deltanode_description">{resourceProvider.read('note')}:</label>
                        <Field component={inputTextarea} name="note" id="txtarea_deltanode_description"
                            placeholder={resourceProvider.read('note')}
                            validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                length({ max: 1000, msg: resourceProvider.read('descriptionLengthValidation') })]} />
                    </div>
                </section>
                <footer className="ep-modal__footer">
                    <div className="ep-modal__footer-actions">
                        <span className="float-r">
                            <button id="btn_deltanode_cancel" type="button" className="ep-btn ep-btn--link" onClick={this.props.closeModal}>{resourceProvider.read('cancel')}</button>
                            <button id="btn_deltanode_reopen" type="submit" onClick={() => this.props.setSubmissionType(constants.ReopenDeltaNodeActionType.DeltaNode)}
                                className="ep-btn ep-btn--sm">{resourceProvider.read('reopenNode')}</button>
                            <button id="btn_deltanode_reopenall" type="submit" onClick={() => this.props.setSubmissionType(constants.ReopenDeltaNodeActionType.DeltaNodeAndSuccessor)}
                                className="ep-btn ep-btn--sm">{resourceProvider.read('reopenNodeDependency')}</button>
                        </span>
                    </div>
                </footer>
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.reopenConfirmationToggler !== this.props.reopenConfirmationToggler) {
            this.props.closeModal();
            navigationService.navigateToModule('officialdeltapernode/manage');
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        reopenNodesList: state.report.officialDeltaNode.reopenNodesList,
        filters: state.report.officialDeltaNode.filters,
        submissionType: state.report.officialDeltaNode.submissionType,
        reopenConfirmationToggler: state.report.officialDeltaNode.reopenConfirmationToggler
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        setSubmissionType: type => {
            dispatch(setSubmissionType(type));
            return true;
        },
        reopenNodes: data => {
            dispatch(reopenDeltaNode(data));
        }
    };
};

const ReopenDeltaNodeForm = reduxForm({
    form: 'reopenDelta',
    enableReinitialize: true,
    shouldAsyncValidate: utilities.shouldAsyncValidate
})(Reopen);

export default connect(mapStateToProps, mapDispatchToProps)(ReopenDeltaNodeForm);
