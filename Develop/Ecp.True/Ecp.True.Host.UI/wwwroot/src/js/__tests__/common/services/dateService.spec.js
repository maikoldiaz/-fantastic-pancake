import { dateService } from '../../../common/services/dateService';
import moment from 'moment-timezone';

describe('date service',
    () => {
        beforeAll(() => {
            dateService.initialize();
        });

        it('dateService format',
            () => {
                expect(dateService.format(new Date('2019-10-10T00:00:00.000Z'), 'DD/MM/YYYY')).toMatch('10/10/2019');
            });
        it('dateService isValid',
            () => {
                expect(dateService.isValid(new Date('2019-10-10T00:00:00.000Z'))).toStrictEqual(true);
            });
        it('dateService invalid date',
            () => {
                expect(dateService.isValid()).toStrictEqual(false);
            });
        it('dateService now',
            () => {
                expect(dateService.now(new Date('2019-10-10T00:00:00.000Z'))).toBeInstanceOf(moment);
            });
        it('dateService add',
            () => {
                expect(dateService.add(moment(), 1, 'd')).toBeInstanceOf(moment);
            });
        it('dateService subtract',
            () => {
                expect(dateService.subtract(moment(), 1, 'd')).toBeInstanceOf(moment);
            });
        it('dateService getDiff',
            () => {
                expect(dateService.getDiff(new Date('2019-10-10T00:00:00.000Z'), new Date('2019-12-10T00:00:00.000Z'), 'DD/MM/YYYY')).toStrictEqual(-5270400000);
            });
        it('dateService parse',
            () => {
                expect(dateService.parse(new Date('2019-10-10T00:00:00.000Z'), 'DD/MM/YYYY')).toBeInstanceOf(moment);
            });
        it('dateService no format parse',
            () => {
                expect(dateService.parse(new Date('2019-10-10T00:00:00.000Z'))).toBeInstanceOf(moment);
            });
        it('dateService null parse',
            () => {
                expect(dateService.parse()).toBeNull();
            });
        it('dateService parse to pbi date string',
            () => {
                expect(dateService.parseToPBIDateString(new Date('2019-10-10T00:00:00.000Z'), 'DD/MM/YYYY')).toMatch('2019-10-10');
            });
    });
