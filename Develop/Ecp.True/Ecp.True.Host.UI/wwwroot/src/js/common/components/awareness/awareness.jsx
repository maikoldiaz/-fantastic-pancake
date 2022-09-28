import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider';
import classNames from 'classnames/bind';

export class Awareness extends React.Component {
    constructor() {
        super();
        this.onCancel = this.onCancel.bind(this);
        this.onPublish = this.onPublish.bind(this);
        this.onDiscard = this.onDiscard.bind(this);
    }

    onDiscard() {
        this.props.onDiscard();
        this.props.close();
    }

    onPublish() {
        this.props.onPublish();
        this.props.close();
    }

    onCancel() {
        this.props.onCancel();
    }

    render() {
        return (
            <div className={classNames('ep-modal', { ['hidden']: !this.props.isOpen })} id="cont_common_awarenessModal" >
                <div className="ep-modal__overlay" />
                <div className="ep-modal__body">
                    <header className="ep-modal__header">
                        <h1 className="ep-modal__title" id="h1_modal_title">{resourceProvider.read('confirmation')}</h1>
                        <span className="ep-modal__close" id="lbl_modal_close" onClick={this.onCancel}><i className="far fa-times-circle" /></span>
                    </header>
                    <section className="ep-modal__content">
                        <h1 id="lbl_localized_youHaveNotSavedYourChanges" className="toyo-modal__body-header">{resourceProvider.read('publishChangesBeforeLeaving')}</h1>
                    </section>
                    <footer className="ep-modal__footer">
                        <div className="ep-modal__footer-actions">
                            <span className="float-r">
                                <button id="btn_awareness_cancel" type="button" className="ep-btn ep-btn--link" onClick={this.onCancel}>{resourceProvider.read('cancel')}</button>
                                <button id="btn_awareness_discard" type="button" className="ep-btn ep-btn--sm" onClick={this.onDiscard} >{resourceProvider.read('accept')}</button>
                                {this.props.enablePublish &&
                                    <button id="btn_awareness_discard" type="button" className="ep-btn ep-btn--sm" onClick={this.onPublish} >{resourceProvider.read('publish')}</button>}
                            </span>
                        </div>
                    </footer>
                </div>
            </div>
        );
    }
}

const mapDispatchToProps = () => {
    return {};
};

export default connect(null, mapDispatchToProps)(Awareness);
