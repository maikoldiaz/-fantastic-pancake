import { supportConfigService } from '../../../common/services/supportConfigService';
describe('support configuration service',
    () => {
        it('should get Attention Line Phone Number', () =>{
            const configuration = {
                attentionLinePhoneNumber: '2345000'
            };
            supportConfigService.initialize(configuration);
            const attentionLinePhoneNumber = supportConfigService.getAttentionLinePhoneNumber();
            expect(attentionLinePhoneNumber).toBe('2345000');
        });
        it('should get Attention Line Phone Number Extention', () =>{
            const configuration = {
                attentionLinePhoneNumberExtention: '4-4-1'
            };
            supportConfigService.initialize(configuration);
            const attentionLinePhoneNumberExtention = supportConfigService.getAttentionLinePhoneNumberExtension();
            expect(attentionLinePhoneNumberExtention).toBe('4-4-1');
        });
        it('should get Attention Line Email', () =>{
            const configuration = {
                attentionLineEmail: 'servicedesk@ecopetrol.com.co'
            };
            supportConfigService.initialize(configuration);
            const attentionLineEmail = supportConfigService.getAttentionLineEmail();
            expect(attentionLineEmail).toBe('servicedesk@ecopetrol.com.co');
        });
        it('should get Chatbot Service Link', () =>{
            const configuration = {
                chatbotServiceLink: 'https://true.ecopetrol.com.co/'
            };
            supportConfigService.initialize(configuration);
            const chatbotServiceLink = supportConfigService.getChatbotServiceLink();
            expect(chatbotServiceLink).toBe('https://true.ecopetrol.com.co/');
        });
        it('should get Auto Service Portal Link', () =>{
            const configuration = {
                autoServicePortalLink: 'https://true.ecopetrol.com.co/'
            };
            supportConfigService.initialize(configuration);
            const autoServicePortalLink = supportConfigService.getAutoServicePortalLink();
            expect(autoServicePortalLink).toBe('https://true.ecopetrol.com.co/');
        });
    });

