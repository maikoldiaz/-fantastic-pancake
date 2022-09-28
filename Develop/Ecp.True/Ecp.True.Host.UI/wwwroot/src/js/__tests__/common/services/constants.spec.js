import { constants } from '../../../common/services/constants';

describe('constants',
    () => {
        it('should return constants for Config_Constants DEV',
            () => {
                expect(constants.Config_Constants.DEV)
                    .toMatch('development');
            });
        it('should return constants for Config_Constants PROD',
            () => {
                expect(constants.Config_Constants.PROD)
                    .toMatch('production');
            });
        it('should return constants for ResponsiveBreakpoints DESKTOPMIN',
            () => {
                expect(constants.ResponsiveBreakpoints.DESKTOPMIN)
                    .toStrictEqual(1367);
            });
        it('should return constants for ResponsiveBreakpoints MOBILEMIN',
            () => {
                expect(constants.ResponsiveBreakpoints.MOBILEMIN)
                    .toStrictEqual(360);
            });
        it('should return constants for ResponsiveBreakpoints MOBILEMAX',
            () => {
                expect(constants.ResponsiveBreakpoints.MOBILEMAX)
                    .toStrictEqual(767);
            });
        it('should return constants for ResponsiveBreakpoints TABLETMIN',
            () => {
                expect(constants.ResponsiveBreakpoints.TABLETMIN)
                    .toStrictEqual(768);
            });
        it('should return constants for ResponsiveBreakpoints TABLETMAX',
            () => {
                expect(constants.ResponsiveBreakpoints.TABLETMAX)
                    .toStrictEqual(1366);
            });
        it('should return constants for RouterActions Button',
            () => {
                expect(constants.RouterActions.Type.Button)
                    .toMatch('Button');
            });
        it('should return constants for RouterActions Dropdown',
            () => {
                expect(constants.RouterActions.Type.Dropdown)
                    .toMatch('Dropdown');
            });
        it('should return constants for NotificationType Success',
            () => {
                expect(constants.NotificationType.Success)
                    .toMatch('success');
            });
        it('should return constants for NotificationType Error',
            () => {
                expect(constants.NotificationType.Error)
                    .toMatch('error');
            });
        it('should return constants for NotificationType Warning',
            () => {
                expect(constants.NotificationType.Warning)
                    .toMatch('warning');
            });
        it('should return constants for NotificationType Info',
            () => {
                expect(constants.NotificationType.Info)
                    .toMatch('info');
            });
        it('should return constants for Modes Create',
            () => {
                expect(constants.Modes.Create)
                    .toMatch('create');
            });
        it('should return constants for Modes Read',
            () => {
                expect(constants.Modes.Read)
                    .toMatch('read');
            });
        it('should return constants for Modes Update',
            () => {
                expect(constants.Modes.Update)
                    .toMatch('update');
            });
        it('should return constants for Modes Delete',
            () => {
                expect(constants.Modes.Delete)
                    .toMatch('delete');
            });
        it('should return constants for ChartColors a',
            () => {
                expect(constants.ChartColors.a)
                    .toMatch('#74B44A');
            });
        it('should return constants for ChartColors b',
            () => {
                expect(constants.ChartColors.b)
                    .toMatch('#F7DF00');
            });
        it('should return constants for ChartColors c',
            () => {
                expect(constants.ChartColors.c)
                    .toMatch('#656565');
            });
        it('should return constants for ChartColors d',
            () => {
                expect(constants.ChartColors.d)
                    .toMatch('#E0F0FA');
            });

        it('should return constants for ChartColors e',
            () => {
                expect(constants.ChartColors.e)
                    .toMatch('#5995AF');
            });
        it('should return constants for ChartColors f',
            () => {
                expect(constants.ChartColors.f)
                    .toMatch('#1592E6');
            });
        it('should return constants for ChartColors g',
            () => {
                expect(constants.ChartColors.g)
                    .toMatch('#93BCBB');
            });
        it('should return constants for ChartColors h',
            () => {
                expect(constants.ChartColors.h)
                    .toMatch('#C3D2CB');
            });

        it('should return constants for ChartColors i',
            () => {
                expect(constants.ChartColors.i)
                    .toMatch('#00CB90');
            });
        it('should return constants for ChartColors j',
            () => {
                expect(constants.ChartColors.j)
                    .toMatch('#CCD325');
            });
        it('should return constants for ChartColors k',
            () => {
                expect(constants.ChartColors.k)
                    .toMatch('#F6B41A');
            });
        it('should return constants for ChartColors l',
            () => {
                expect(constants.ChartColors.l)
                    .toMatch('#C55A11');
            });

        it('should return constants for ChartColors m',
            () => {
                expect(constants.ChartColors.m)
                    .toMatch('#7030A0');
            });
        it('should return constants for ChartColors n',
            () => {
                expect(constants.ChartColors.n)
                    .toMatch('#3B3838');
            });
        it('should return constants for ChartColors o',
            () => {
                expect(constants.ChartColors.o)
                    .toMatch('#7A8B58');
            });
        it('should return constants for ChartColors p',
            () => {
                expect(constants.ChartColors.p)
                    .toMatch('#93A7BE');
            });

        it('should return constants for ChartColors q',
            () => {
                expect(constants.ChartColors.q)
                    .toMatch('#5F6867');
            });
        it('should return constants for ChartColors r',
            () => {
                expect(constants.ChartColors.r)
                    .toMatch('#003427');
            });
        it('should return constants for Report WithOwner',
            () => {
                expect(constants.Report.WithOwner)
                    .toMatch('10.10.17BalanceOperativoConPropiedadPorNodo17');
            });
        it('should return constants for Report WithoutOwner',
            () => {
                expect(constants.Report.WithoutOwner)
                    .toMatch('10.10.01BalanceOperativoSinPropiedad01');
            });
        it('should return constants for Report AnalyticsReport',
            () => {
                expect(constants.Report.AnalyticsReport)
                    .toMatch('10.10.03EvaluacionModelosAnaliticosPorcentajePropiedad03');
            });
        it('should return constants for Report WithoutCutoff',
            () => {
                expect(constants.Report.WithoutCutoff)
                    .toMatch('10.10.04BalanceOperativo04');
            });
        it('should return constants for Report BalanceControlChart',
            () => {
                expect(constants.Report.BalanceControlChart)
                    .toMatch('10.10.05CartadeControl05');
            });
        it('should return constants for Report EventContractReport',
            () => {
                expect(constants.Report.EventContractReport)
                    .toMatch('10.10.06ComprasyVentas06');
            });
        it('should return constants for Report NodeStatusReport',
            () => {
                expect(constants.Report.NodeStatusReport)
                    .toMatch('10.10.07AprobacionesBalanceConPropiedadPorNodo07');
            });
        it('should return constants for Report NodeConfigurationReport',
            () => {
                expect(constants.Report.NodeConfigurationReport)
                    .toMatch('10.10.08ConfiguracionDetalladaPorNodo08');
            });
    });
