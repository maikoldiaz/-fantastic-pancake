import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../services/resourceProvider.js';
import { openComponentModal } from '../../actions.js';
import ErrorModal from './errorModal.jsx';
import { constants } from './../../../common/services/constants.js';

export class Error extends React.Component {
    render() {
        return (
            <>
                {this.props.errorCode &&
                    <section className="ep-error-wrapper">
                        <div className="ep-error">
                            <span className="ep-error__img">
                                <img className="ep-error__mascot" src="./../dist/images/error-mascot.png" alt={resourceProvider.read('epMascot')}/>
                            </span>
                            <div className="ep-error__msg">
                                <h1 className="ep-error__code" id="h1_error_code">{this.props.errorCode}</h1>
                                <h2 className="ep-error__title" id="h2_error_title">{resourceProvider.read(`errorTitle${this.props.errorCode}`)}</h2>
                                <p className="ep-error__desc" id="lbl_error_desc">
                                    {resourceProvider.read(`errorMessage${this.props.errorCode}`)}
                                    {this.props.errorCode === constants.Errors.ServerError &&
                                        <span className="text-italic">{resourceProvider.read(`errorMessage${this.props.errorCode}Italics`)}</span>}
                                </p>
                                {this.props.errorCode === constants.Errors.ServerError &&
                                    <div className="text-right">
                                        <button id="ep-error_link" type="button" className="ep-btn ep-btn--link" onClick={this.props.openErrorModal}>
                                            {resourceProvider.read('masInformacion500')}
                                        </button>
                                    </div>}
                            </div>
                        </div>
                    </section>}
            </>
        );
    }
}

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        openErrorModal: () => {
            dispatch(openComponentModal(ErrorModal, '', resourceProvider.read('atencionTrueModalTtl'), 'ep-modal', 'text-unset'));
        }
    };
};


/* istanbul ignore next */
export default connect(null, mapDispatchToProps)(Error);
