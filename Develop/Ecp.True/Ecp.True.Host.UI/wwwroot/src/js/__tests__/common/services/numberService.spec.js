import { numberService } from '../../../common/services/numberService';

describe('NumberService service', () => {
    it('should return GroupingSymbol',
        () => {
            const result = numberService.getGroupingSymbol();
            expect(result).toEqual('.');
        });

        it('should return DecimalSymbol',
        () => {
            const result = numberService.getDecimalSymbol();
            expect(result).toEqual(',');
        });

        it('should create BigDecimal',
        () => {
            const result = numberService.createBigDecimal('123');
            expect(result.c[0]).toEqual(123);
        });

        it('should create BigDecimalString',
        () => {
            const result = numberService.createBigDecimalString(123);
            expect(result).toEqual('123');
        });

        it('should return maxOfBigNumber',
        () => {
            const result = numberService.maxOfBigNumber(1,123);
            expect(result).toEqual('123');
        });

        it('should return minOfBigNumber',
        () => {
            const result = numberService.minOfBigNumber(1,123.99);
            expect(result).toEqual('1');
        });

        it('should check If DecimalInRange',
        () => {
            const result = numberService.checkIfDecimalInRange(1, 2, 123.99);
            expect(result).toEqual(true);
        });

        it('should check If changeFormat',
        () => {
            const result = numberService.changeFormat('123,99');
            expect(result).toEqual('123.99');
        });
});