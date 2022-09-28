import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../../common/services/resourceProvider';
import { IconPicker } from '../../../../../common/components/icons/iconPicker.jsx';
import { openIconModal } from '../actions';
import { setIconId } from '../../../../../common/actions';
import { change } from 'redux-form';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class IconModal extends React.Component {
    constructor() {
        super();
    }

    render() {
        return (
            <div id="icon_modal" className="ep-modal ep-modal--sm">
                <div id="icon_modal_overlay" className="ep-modal__overlay ep-modal__overlay--addon">
                    <form className="ep-form">
                        <section id="icon_modal_body" className="ep-modal__body">
                            <header className="ep-modal__header">
                                <h1 className="ep-modal__title" id="h1_modal_title">{resourceProvider.read('selectIcons')}</h1>
                                <span className="ep-modal__close" id="lbl_modal_close" onClick={() => {
                                    this.props.closeIconModal();
                                }}><i className="far fa-times-circle" /></span>
                            </header>
                            <form className="ep-form">
                                <section className="ep-modal__content">
                                    <IconPicker id="select_icon" icon={this.props.icons} {...this.props} />
                                </section>
                                <ModalFooter config={footerConfigService.getCommonConfig('icon', {
                                    acceptActions: [openIconModal(),
                                        change('createElement', 'icon', `${this.props.icon.name}.svg`)], cancelActions: [openIconModal()], acceptText: 'select'
                                })} />
                            </form>
                        </section>
                    </form>
                </div>
            </div>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        icon: state.iconPicker ? state.iconPicker : null
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        setIconId: (iconId, name) => {
            dispatch(setIconId(iconId, name));
        },
        selectIcon: value => {
            dispatch(openIconModal());
            dispatch(change('createElement', 'icon', value + '.svg'));
        },
        closeIconModal: () => {
            dispatch(openIconModal());
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(IconModal);
