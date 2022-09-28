import { gridUtils } from '../../../../common/components/grid/gridUtils.js';
import { DateFilter, StatusCell, TextboxFilter, ActionCell, OwnershipCell } from '../../../../common/components/grid/gridFilters.jsx';
import { utilities } from '../../../../common/services/utilities.js';
import NumberFormatter from '../../../../common/components/formControl/numberFormatter.jsx';
import { constants } from '../../../../common/services/constants';

describe('Grid Utils', () => {
    const props = {
        name: 'categoriesGrid', history: {
            length: 2, action: 'POP',
            location: { pathname: '/category/manage', search: '', hash: '', key: '01test' }
        },
        location: { pathname: '/category/manage', search: '', hash: '', key: '01test' },
        match: { path: '/category', url: '/category', isExact: false, params: {} },
        routerConfig: {
            pageKey: 'category', config: {
                routes: {
                    category: {
                        routeKey: 'manage', title: 'manageCategories',
                        component: { compare: null, displayName: 'Connect(Connect(DataGrid))' },
                        actions: [{ title: 'createCategory', iconClass: 'fas fa-search', type: 'Button', actionType: 'modal', key: 'createCategory', mode: 'create' }]
                    }
                }
            }
        },
        edit: true,
        netVol: '1200'
    };
    it('should build Text Column', () => {
        const type = utilities.toLowerCase('name').includes('id') ? 'number' : 'text';
        const textFilter = rowProps => <TextboxFilter {...props} {...rowProps} type={type} />;
        const returnData = gridUtils.buildTextColumn('name', props, textFilter);
        expect(returnData.Cell).toEqual(textFilter);
    });
    it('should build Date Column', () => {
        const dateFilter = rowProps => <DateFilter {...props} {...rowProps} />;
        const returnData = gridUtils.buildDateColumn('createdDate', props, dateFilter);
        expect(returnData.Cell).toEqual(dateFilter);
    });
    it('should build Select Column', () => {
        const statusCell = rowProps => <StatusCell {...props} {...rowProps} />;
        const returnData = gridUtils.buildSelectColumn('isActive', props, statusCell);
        expect(returnData.Cell).toEqual(statusCell);
    });
    it('should build Action Column', () => {
        const actions = gridProps => <ActionCell id={this.props.name} {...this.props} {...gridProps} />;
        const returnData = gridUtils.buildActionColumn(actions);
        expect(returnData.filterable).toEqual(false);
        expect(returnData.sortable).toEqual(false);
        expect(returnData.actionColumn).toEqual(true);
        expect(returnData.Cell).toEqual(actions);
    });
    it('should build Header Column', () => {
        const Uncertainty = rowProps => <ActionCell {...props} {...rowProps} />;
        const Ownership = rowProps => <OwnershipCell {...rowProps} {...props} />;
        const returnData = gridUtils.buildHeaderColumn('movements',
            gridUtils.buildTextColumn('uncertaintyPercentage', props, Uncertainty, 'uncertainty'),
            gridUtils.buildTextColumn('owners', props, Ownership, 'owners', false)
        );
        expect(returnData.columns.length).toEqual(2);
        expect(returnData.headerClassName).toEqual('ep-table__col-group-header');
    });
    it('should build Number Column', () => {
        const numberCell = (<NumberFormatter
            className="float-r"
            displayType="text"
            value={1200}
        />);
        const row = {
            original: props
        };
        const returnData = gridUtils.buildNumberColumn('netVol', props, 'netVol', { sortable: false, filterable: false });
        expect(returnData.Cell(row)).toEqual(numberCell);
    });

    it('should build Decimal Column', () => {
        const numberCell = (<NumberFormatter
            className="float-r"
            prefix={null}
            suffix={null}
            displayType="text"
            isNumericString={true}
            value="1200.00"
        />);
        const row = {
            row: props,
            original: {
                netVol: 1200
            }
        };
        const returnData = gridUtils.buildDecimalColumn('netVol', props, 'netVol', { sortable: false, filterable: false });
        expect(returnData.Cell(row)).toEqual(numberCell);
    });

    it('should build Text Column and does not hide filter text box', () => {
        const type = utilities.toLowerCase('name').includes('id') ? 'number' : 'text';
        const textFilter = rowProps => <TextboxFilter {...props} {...rowProps} type={type} />;
        const returnData = gridUtils.buildTextColumn('name', props, textFilter);
        expect(returnData.Cell).toEqual(textFilter);
        expect(returnData.hideFilterTextBox).toEqual(false);
    });

    it('should build Text Column and hides filter text box', () => {
        const type = utilities.toLowerCase('name').includes('id') ? 'number' : 'text';
        const textFilter = rowProps => <TextboxFilter {...props} {...rowProps} type={type} />;
        const returnData = gridUtils.buildTextColumn('name', props, textFilter, 'name', { width: 150, hideFilterTextBox: true });
        expect(returnData.Cell).toEqual(textFilter);
        expect(returnData.hideFilterTextBox).toEqual(true);
    });
});
