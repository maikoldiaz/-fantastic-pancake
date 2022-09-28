import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputTextarea } from '../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { addComment } from '../actions';
import { saveErrorComment } from '../../../administration/exceptions/actions';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import ModalFooter from '../../../../common/components/footer/modalFooter.jsx';
import { refreshGrid } from '../../../../common/components/grid/actions';

class AddComment extends React.Component {
    constructor() {
        super();
        this.saveComment = this.saveComment.bind(this);
    }

    saveComment(values) {
        if (this.props.componentType === constants.CommentType.Cutoff) {
            this.props.addComment(values.comment);
            this.props.closeModal();
        } else {
            const errorComments = {
                ErrorId: this.props.selectedData.map(e => e.errorId),
                Comment: values.comment
            };
            this.props.saveErrorComment(errorComments);
        }
    }

    render() {
        return (
            <form id={`frm_cutoff_addComments`} className="ep-form" onSubmit={this.props.handleSubmit(this.saveComment)}>
                <section className="ep-modal__content">
                    <p className="fs-16 m-b-3">
                        <span>{this.props.preText}</span>
                        <span id="span_records_count" className="fw-bold m-l-1 fc-secondary">{this.props.count}</span>
                        <span className="m-l-1">{this.props.postText}</span>
                    </p>
                    <div className="ep-control-group">
                        <label className="ep-label">{resourceProvider.read('comment')}</label>
                        <Field type="text" id="txt_addComment_comment" component={inputTextarea}
                            placeholder={resourceProvider.read('comment')} name="comment"
                            validate={[required({ msg: { presence: resourceProvider.read('required') } }),
                                length({
                                    max: 1000, msg: this.props.componentType === constants.CommentType.Cutoff ?
                                        resourceProvider.read('invalidBalanceNoteFormat') : resourceProvider.read('shortDescriptionLengthValidation')
                                })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('addComment')} />
            </form>
        );
    }

    componentDidUpdate(prevProps) {
        if (prevProps.refreshToggler !== this.props.refreshToggler && this.props.componentType === constants.CommentType.Error) {
            this.props.refreshGrid();
            this.props.closeModal();
        }
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        addComment: comment => {
            dispatch(addComment(comment));
        },
        saveErrorComment: errorComments => {
            dispatch(saveErrorComment(errorComments));
        },
        refreshGrid: () => {
            dispatch(refreshGrid('pendingTransactionErrors', true));
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    const isCutoffType = ownProps.componentType === constants.CommentType.Cutoff;
    return {
        count: isCutoffType ? state.cutoff.operationalCut[state.cutoff.operationalCut.name].count : state.controlexception.controlException.pendingTransactionErrors.count,
        preText: isCutoffType ? state.cutoff.operationalCut[state.cutoff.operationalCut.name].preText : state.controlexception.controlException.pendingTransactionErrors.preText,
        postText: isCutoffType ? state.cutoff.operationalCut[state.cutoff.operationalCut.name].postText : state.controlexception.controlException.pendingTransactionErrors.postText,
        selectedData: isCutoffType ? [] : state.controlexception.controlException.selectedData,
        refreshToggler: isCutoffType ? false : state.controlexception.controlException.refreshToggler,
        gridData: isCutoffType ? false : !utilities.isNullOrUndefined(state.grid.pendingTransactionErrors)
    };
};

const addCommentForm = reduxForm({
    form: 'addComments'
})(AddComment);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(addCommentForm);
