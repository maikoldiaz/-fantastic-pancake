import { utilities } from './utilities';
import { resourceProvider } from './resourceProvider';

const supportConfigService = (function () {
    let supportConfiguration = {};
    const supportProcess = {
        0: {
            title: resourceProvider.read('validationSolicitudLbl'),
            desc: resourceProvider.read('validationSolicitudMsg')
        },
        1: {
            title: resourceProvider.read('validationCanalDeAtencionLbl')
        },
        2: {
            title: resourceProvider.read('validationTicketLbl')
        },
        3: {
            title: resourceProvider.read('validationSoporteLbl'),
            desc: resourceProvider.read('validationSoporteMsg')
        },
        4: {
            title: resourceProvider.read('validationNotificationLbl'),
            desc: resourceProvider.read('validationNotificationMsg')
        },
        5: {
            title: resourceProvider.read('validationSolutionDelCaseLbl'),
            desc: resourceProvider.read('validationSolutionDelCaseMsg')
        }
    };

    return {
        initialize: configuration => {
            supportConfiguration = configuration;
        },
        getAttentionLinePhoneNumber: () => {
            return utilities.getValueOrDefault(supportConfiguration.attentionLinePhoneNumber, '2345000');
        },
        getAttentionLinePhoneNumberExtension: () => {
            return utilities.getValueOrDefault(supportConfiguration.getAttentionLinePhoneNumberExtension, '4-4-1');
        },
        getAttentionLineEmail: () => {
            return utilities.getValueOrDefault(supportConfiguration.attentionLineEmail, 'servicedesk@ecopetrol.com.co');
        },
        getChatbotServiceLink: () => {
            return utilities.getValueOrDefault(supportConfiguration.chatbotServiceLink, 'https://www.ecopetrol.com.co/');
        },
        getAutoServicePortalLink: () => {
            return utilities.getValueOrDefault(supportConfiguration.autoServicePortalLink, 'https://www.ecopetrol.com.co/');
        },
        getProcessTitle: index => {
            return supportProcess[index].title;
        },
        getProcessDesc: index => {
            return supportProcess[index].desc;
        },
        getProcesses: () => {
            return Object.keys(supportProcess);
        }
    };
}());

export { supportConfigService };
