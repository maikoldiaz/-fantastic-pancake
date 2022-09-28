import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { required, length } from 'redux-form-validators';
import { inputTextarea } from '../formControl/formControl.jsx';
import { resourceProvider } from '../../services/resourceProvider';
import { utilities } from '../../services/utilities';
import { addComment } from '../../actions.js';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';

class AddComment extends React.Component {
    constructor() {
        super();
        this.saveComment = this.saveComment.bind(this);
    }

    saveComment(values) {
        this.props.addComment(values.comment);
        this.props.closeModal();
    }

    render() {
        const MessageComponent = this.props.component;
        return (
            <form id={`frm_${this.props.mode}_addComments`} className="ep-form" onSubmit={this.props.handleSubmit(this.saveComment)}>
                <section className="ep-modal__content">
                    {MessageComponent && <MessageComponent />}
                    <p className="fs-16 m-b-3">{this.props.message}</p>
                    <div className="ep-control-group">
                        <label className="ep-label">{resourceProvider.read('note')}</label>
                        <Field type="text" id="txt_addComment_comment" component={inputTextarea}
                            placeholder={resourceProvider.read(!utilities.isNullOrUndefined(this.props.placeholder) ? this.props.placeholder : 'note')} name="comment"
                            validate={[required({ msg: { presence: resourceProvider.read(!utilities.isNullOrUndefined(this.props.required) ? this.props.required : 'balanceNoteRequired') } }),
                                length({ max: 1000, msg: resourceProvider.read('invalidBalanceNoteFormat') })]} />
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('addComment')} />
            </form>
        );
    }
}

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownprops) => {
    return {
        addComment: comment => {
            dispatch(addComment(comment, ownprops.name));
        }
    };
};

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        message: state.addComment[state.addComment.name] ? state.addComment[state.addComment.name].message : null,
        component: state.addComment[state.addComment.name] ? state.addComment[state.addComment.name].component : null,
        placeholder: state.addComment[state.addComment.name] ? state.addComment[state.addComment.name].placeholder : null,
        required: state.addComment[state.addComment.name] ? state.addComment[state.addComment.name].required : null
    };
};

const addCommentForm = reduxForm({
    form: 'addComments'
})(AddComment);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(addCommentForm);
