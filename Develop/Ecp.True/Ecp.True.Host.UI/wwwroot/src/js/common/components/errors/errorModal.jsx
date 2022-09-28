import React from 'react';
import { resourceProvider } from '../../services/resourceProvider';
import { supportConfigService } from '../../../common/services/supportConfigService';
import ModalFooter from '../footer/modalFooter.jsx';
import { footerConfigService } from '../../services/footerConfigService';

export default class ErrorModal extends React.Component {
    getDesc(key) {
        if (key === '1') {
            return (
                <>
                    {resourceProvider.read('validationCanalDeAtencionMsg')} <span className="text-italic">{resourceProvider.read('serviceDeskItalics')}.</span>
                </>
            );
        }

        if (key === '2') {
            return (
                <>
                    {resourceProvider.read('validationTicketMsgGen')} <span className="text-lowercase text-italic">{resourceProvider.read('ticketItalics')} </span>
                    {resourceProvider.read('validationTicketMsgCaseAten')}
                </>
            );
        }

        return '';
    }

    render() {
        const autoService = supportConfigService.getAutoServicePortalLink();
        const chatBot = supportConfigService.getChatbotServiceLink();
        const phone = supportConfigService.getAttentionLinePhoneNumber();
        const phoneExtn = supportConfigService.getAttentionLinePhoneNumberExtension();
        const email = supportConfigService.getAttentionLineEmail();
        return (
            <>
                <section className="ep-modal__content">
                    <div className="row">
                        <div className="col-md-5">
                            <div className="ep-section">
                                <h3 className="fs-20 ep-label m-b-3">{resourceProvider.read('sectionCanalesDeAtencionTitle')}</h3>
                                <ul className="ep-content__lst">
                                    <li className="ep-content__item">
                                        <span className="ep-content__item-icn">
                                            <img className="ep-icn-desktop" src="./../dist/images/desktop.svg" height="50" alt={resourceProvider.read('portalAutoservicioLbl')} />
                                        </span>
                                        <span className="ep-content__item-cont">
                                            <a href={autoService} target="_blank" rel="noopener noreferrer" className="ep-content__item-lbl">{resourceProvider.read('portalAutoservicioLbl')}</a>
                                        </span>
                                    </li>
                                    <li className="ep-content__item">
                                        <span className="ep-content__item-icn">
                                            <img className="ep-icn-chatbot" src="./../dist/images/tico-chatbot.svg" height="50" alt={resourceProvider.read('chatbotTICOLbl')} />
                                        </span>
                                        <span className="ep-content__item-cont">
                                            <a href={chatBot} target="_blank" rel="noopener noreferrer" className="ep-content__item-lbl">{resourceProvider.read('chatbotTICOLbl')}</a>
                                        </span>
                                    </li>
                                    <li className="ep-content__item">
                                        <span className="ep-content__item-icn">
                                            <img className="ep-icn-phone" src="./../dist/images/phone.svg" height="50" alt={resourceProvider.read('lineaDeAtencionLbl')} />
                                        </span>
                                        <span className="ep-content__item-cont">
                                            <label className="ep-content__item-lbl">{resourceProvider.read('lineaDeAtencionLbl')}</label>
                                            <span className="ep-content__item-val"><a href={`tel:${phone}`}>{phone}</a> {resourceProvider.read('options')} {phoneExtn}</span>
                                        </span>
                                    </li>
                                    <li className="ep-content__item">
                                        <span className="ep-content__item-icn">
                                            <img className="ep-icn-mail" src="./../dist/images/mail.svg" height="50" alt={resourceProvider.read('mailElectronicLbl')} />
                                        </span>
                                        <span className="ep-content__item-cont">
                                            <label className="ep-content__item-lbl">{resourceProvider.read('mailElectronicLbl')}</label>
                                            <span className="ep-content__item-val"><a href={`mailto:${email}`}>{email}</a></span>
                                        </span>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div className="col-md-7">
                            <div className="ep-section">
                                <h3 className="fs-20 ep-label m-b-3">{resourceProvider.read('sectionPasosDelReporteTitle')}</h3>
                                <ul className="ep-validation__lst m-t-6 m-l-2">
                                    {supportConfigService.getProcesses().map(m => (
                                        <li className="ep-validation__itm" key={m}>
                                            <h1 className="ep-validation__itm-title">
                                                <span className="ep-validation__itm-icn success"><i className="fas fa-check" /></span>
                                                {supportConfigService.getProcessTitle(m)}
                                            </h1>
                                            <p className="ep-validation__itm-desc">
                                                {supportConfigService.getProcessDesc(m) || this.getDesc(m)}
                                            </p>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </div>
                    </div>
                </section>
                <ModalFooter config={footerConfigService.getAcceptConfig('errorModal', { closeModal: true, acceptText: 'accept' })} />
            </>
        );
    }
}
