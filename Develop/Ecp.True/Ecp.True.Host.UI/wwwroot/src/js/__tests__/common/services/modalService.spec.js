import { modalService } from '../../../common/services/modalService';

describe('modal service', () => {
    it('does not initialize modal service, if modal config is not returned.', () => {
        const result = modalService.initialize('title');
        expect(result).toBeUndefined();
    });

    it('getProps returns props.', () => {
        const result = modalService.getProps();
        expect(result).toBe(null);
    });

    it('reset returns is open as false.', () => {
        const result = modalService.reset();
        expect(result).toBeUndefined();
    });

    it('isOpen returns isOpen.', () => {
        const result = modalService.isOpen();
        expect(result).toBe(false);
    });
});