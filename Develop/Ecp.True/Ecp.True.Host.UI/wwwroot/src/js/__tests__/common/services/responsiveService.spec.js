import { constants } from './../../../common/services/constants';
import { responsiveService } from '../../../common/services/responsiveService';

describe('responsive service',
    () => {
        it('should build api for query nodeConnection',
            () => {
                expect(responsiveService.getResponsiveElement('test', { minWidth: constants.ResponsiveBreakpoints.DESKTOPMIN }))
                    .toBeInstanceOf(Object);
            });
    });
