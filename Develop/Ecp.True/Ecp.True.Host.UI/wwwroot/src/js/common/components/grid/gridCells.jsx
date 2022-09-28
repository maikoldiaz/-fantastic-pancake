import React from 'react';
import { resourceProvider } from '../../services/resourceProvider';
import { utilities } from '../../services/utilities';
import { dateService } from '../../services/dateService';
import Tooltip from './../../../common/components/tooltip/tooltip.jsx';
import { constants } from './../../services/constants';
import { numberService } from './../../services/numberService.js';
import NumberFormatter from './../../../common/components/formControl/numberFormatter.jsx';
import classNames from 'classnames/bind';

export class DateCell extends React.Component {
    render() {
        const date = utilities.getValue(this.props.original, this.props.column.id);
        const show = !this.props.ignoreMax || new Date(date).getFullYear() !== 9999;
        const dateString = utilities.isNullOrUndefined(this.props.dateWithTime) ? dateService.format(date) : dateService.format(date, 'DD-MMM-YY HH:mm');
        return (
            <span className="text-caps">{show ? dateString : ''}</span>
        );
    }
}

export class UploadStatusCell extends React.Component {
    getStatusIcnClass(status) {
        let cssClass = '';
        const submitForApproval = 'Enviado a aprobaci' + String.fromCharCode(243) + 'n';
        switch (status) {
        case 'Procesando':
        case 'PROCESSING':
        case 'PUBLISHING':
        case 'Publicando':
            cssClass = 'fas fa-spinner fas--spin m-r-2';
            break;
        case 'Finalizado':
        case 'FINALIZED':
        case 'PROCESSED':
        case 'APPROVED':
        case 'Aprobado':
        case 'DELTA':
        case 'Deltas':
        case 'CONCILIATED':
        case 'Conciliado':
        case 'OWNERSHIP':
        case 'Propiedad':
            cssClass = 'fas fa-check-circle fas--success m-r-2';
            break;
        case 'CONCILIATIONFAILED':
        case 'Fallo Conciliación':
        case 'Fallido':
        case 'FAILED':
        case 'REJECTED':
        case 'Rechazado':
            cssClass = 'fas fa-times-circle fas--error m-r-2';
            break;
        case 'Enviado':
        case 'SENT':
            cssClass = 'fas fa-file-import m-r-2';
            break;
        case 'LOCKED':
        case 'Bloqueado':
            cssClass = 'fas fa-lock m-r-2';
            break;
        case 'UNLOCKED':
        case 'Desbloqueado':
            cssClass = 'fas fa-unlock m-r-2';
            break;
        case 'PUBLISHED':
        case 'Publicado':
            cssClass = 'fas fa-upload m-r-2';
            break;
        case 'SUBMITFORAPPROVAL':
        case submitForApproval:
            cssClass = 'fas fa-file-signature m-r-2';
            break;
        case 'REOPENED':
        case 'Reabierto':
            cssClass = 'fas fa-redo m-r-2';
            break;
        case 'Visualizacion':
        case 'VISUALIZATION':
            cssClass = 'fas fa-binoculars fas--success m-r-2';
            break;
        case 'NOTCONCILIATED':
        case 'No Conciliado':
            cssClass = 'fas fa-exclamation-circle fas--warning m-r-2';
            break;
        case 'Error':
        case 'ERROR':
            cssClass = 'fas fa-exclamation-circle fas--error m-r-2';
            break;
        case 'Cancelado':
        case 'Cancelled':
            cssClass = 'fas fa-ban fas--error m-r-2';
            break;
        default:
            cssClass = 'fas fa-spinner fas--spin m-r-2';
        }

        return cssClass;
    }
    render() {
        const status = this.props.getStatus(this.props.original);
        const noLocalize = utilities.isFunction(this.props.noLocalize) ? this.props.noLocalize(this.props.original) : this.props.noLocalize;

        return (
            <>
                <i id={`lnk_${this.props.name}_status`} className={this.getStatusIcnClass(status)} />
                <span className="d-inline-block text-fl-ucase">{noLocalize ? status : resourceProvider.read(utilities.toLowerCase(status))}</span>
            </>
        );
    }
}

export class StatusCell extends React.Component {
    render() {
        const status = this.props.original[this.props.statusKey ? this.props.statusKey : this.props.column.id];
        const className = status ? this.props.trueClass : this.props.falseClass;
        const statusKey = status ? this.props.trueKey : this.props.falseKey;

        return (
            <>
                <i id={`lnk_${this.props.name}_status`} className={className} />
                <span className="d-inline-block text-fl-ucase">{resourceProvider.read(statusKey)}</span>
            </>
        );
    }
}
export class TranslateCell extends React.Component {
    render() {
        const status = this.props.original[this.props.statusKey ? this.props.statusKey : this.props.column.id];
        const toLowercase = !!this.props.transformToLowercase;

        return (
            <span className="">{resourceProvider.read(toLowercase ? status.toLowerCase() : status)}</span>
        );
    }
}

export class CheckBoxCell extends React.Component {
    render() {
        const status = this.props.original[this.props.statusKey ? this.props.statusKey : this.props.column.id];

        return (
            <>
                <label className="ep-checkbox ep-checkbox--gr-sel">
                    <input className="ep-checkbox__input" type="checkbox" id={`chk_${this.props.name}_checkbox`} name={`chk_${this.props.name}_checkbox`} checked={status} disabled />
                    <span className="ep-checkbox__action" />
                    <span className="sr-only">{resourceProvider.read('checkbox')}</span>
                </label>
            </>
        );
    }
}

export class TogglerCell extends React.Component {
    render() {
        const isRequiredMapping = this.props.original[this.props.column.id];
        const homologationObjectType = this.props.original;
        return (
            <label className="ep-toggler">
                <input className="ep-toggler__input" type="checkbox"
                    checked={isRequiredMapping} onChange={v => this.props.updateHomologationObjectType(homologationObjectType, v.target.checked)} />
                <span className="ep-toggler__action" />
                <span className="sr-only">{resourceProvider.read('toggler')}</span>
            </label>
        );
    }
}

export class ActionCell extends React.Component {
    constructor(props) {
        super(props);

        const defaultActions = [
            {
                show: this.props.execute,
                enabled: this.props.enableExecute ? this.props.enableExecute : true,
                className: 'ep-icon ep-icon__double-arrow',
                title: this.props.executeTitle || 'execute',
                clickAction: this.props.onExecute,
                actionKey: this.props.executeActionKey
            },
            {
                show: this.props.detail,
                enabled: this.props.enableDetail ? this.props.enableDetail : true,
                className: 'ep-icon ep-icon__detail',
                title: this.props.detailTitle || 'detail',
                clickAction: this.props.onDetail,
                actionKey: this.props.detailActionKey
            },
            {
                show: this.props.info,
                enabled: this.props.enableInfo ? this.props.enableInfo : true,
                className: 'fas fa-info-circle',
                title: this.props.infoTitle || 'detail',
                clickAction: this.props.onInfo,
                actionKey: this.props.infoActionKey
            },
            {
                show: this.props.view,
                enabled: this.props.enableView ? this.props.enableView : true,
                className: 'far fa-eye',
                title: this.props.viewTitle || 'view',
                clickAction: this.props.onView,
                actionKey: this.props.viewActionKey
            },
            {
                show: this.props.refresh,
                enabled: this.props.enableRefresh ? this.props.enableRefresh : true,
                className: 'fas fa-redo',
                title: this.props.refreshTitle || 'refresh',
                clickAction: this.props.onRefresh,
                actionKey: this.props.refreshActionKey
            },
            {
                show: this.props.download,
                enabled: this.props.enableDownload ? this.props.enableDownload : true,
                className: 'fas fa-file-download',
                title: this.props.downloadTitle || 'download',
                clickAction: this.props.onDownload,
                actionKey: this.props.downloadActionKey
            },
            {
                show: this.props.edit,
                enabled: this.props.enableEdit ? this.props.enableEdit : true,
                className: 'fas fa-edit',
                title: this.props.editTitle || 'edit',
                clickAction: this.props.onEdit,
                actionKey: this.props.editActionKey
            },
            {
                show: this.props.expire,
                enabled: this.props.enableExpire ? this.props.enableExpire : true,
                className: 'fas fa-calendar-times',
                title: 'expire',
                clickAction: this.props.onExpire
            },
            {
                show: this.props.delete,
                enabled: this.props.enableDelete ? this.props.enableDelete : true,
                className: 'fas fa-trash',
                title: this.props.deleteTitle || 'delete',
                clickAction: this.props.onDelete,
                actionKey: this.props.deleteActionKey
            },
            {
                show: this.props.continue,
                enabled: this.props.enableContinue ? this.props.enableContinue : true,
                className: 'fas fa-arrow-circle-right',
                title: this.props.continueTitle || 'continue',
                clickAction: this.props.onContinue,
                actionKey: this.props.continueActionKey
            }
        ];

        this.actions = defaultActions;
        this.onClick = this.onClick.bind(this);
        this.addAction = this.addAction.bind(this);
        this.onActionClick = this.onActionClick.bind(this);
        this.buildValue = this.buildValue.bind(this);
    }

    addAction(action) {
        if (action) {
            this.actions.push(action);
        }
    }

    onActionClick(action, dataItem) {
        if (!this.disabled(action) && action.clickAction) {
            action.clickAction(dataItem);
        }
    }

    onClick() {
        if (this.props.onClick) {
            this.props.onClick(this.props.dataItem);
        }
    }

    showAction(action) {
        if (utilities.isFunction(action.show)) {
            return action.show(this.props.original);
        }
        return action.show;
    }

    disabled(action) {
        if (utilities.isFunction(action.enabled)) {
            return !action.enabled(this.props.original);
        }
        return !action.enabled;
    }

    buildValue(action) {
        if (utilities.isFunction(action.actionKey)) {
            return action.actionKey(this.props.original);
        }

        return this.props.original[action.actionKey];
    }

    getTitle(action) {
        if (utilities.isFunction(action.title)) {
            return action.title(this.props.original);
        }

        return action.title;
    }

    generateIdSuffix() {
        return this.props.idField ? this.props.original[this.props.idField] : this.props.index;
    }

    render() {
        return (
            <ul className={`ep-table__actions ${this.props.wrapClassName}`} id={`ul_${this.props.name}_${this.generateIdSuffix()}`}>
                {this.actions.map(action => this.showAction(action) &&
                    <li id={`li_${this.props.name}_${this.getTitle(action)}_${this.generateIdSuffix()}`}
                        className="ep-table__actions-itm" key={this.getTitle(action) + '-itm'} disabled={this.disabled(action)}>
                        {action.actionKey && <span className="m-r-2 ellipsis" id={`lbl_${this.props.name}_${this.getTitle(action)}`}>{this.buildValue(action)}</span>}
                        <a className="ep-table__actions-lnk" id={`lnk_${this.props.name}_${this.getTitle(action)}_${this.generateIdSuffix()}`}
                            key={this.getTitle(action)} disabled={this.disabled(action)}
                            onClick={() => this.onActionClick(action, this.props.original)}>
                            <Tooltip body={resourceProvider.read(this.getTitle(action))}>
                                <em className={action.className} aria-hidden="true" />
                            </Tooltip>
                        </a>
                    </li>
                )}
            </ul>
        );
    }
}

export class MessageCountCell extends React.Component {
    render() {
        const errorsCount = this.props.getErrorCount(this.props.original);

        const icnClass = classNames('fas', { ['fa-exclamation-circle']: (errorsCount > 0), ['fa-check-circle']: errorsCount === 0 });
        const msgClass = classNames('ep-msg', { ['ep-msg--error']: (errorsCount > 0), ['ep-msg--success']: (errorsCount === 0) });

        return (
            <dl className={msgClass} id={`dl_${this.props.name}_${this.props.column.id}`} >
                <dt className="ep-msg__title"><i className={icnClass} /></dt>
                <dd className="ep-msg__val">{errorsCount}</dd>
            </dl>
        );
    }
}

export class ProductsCell extends React.Component {
    render() {
        const nodeStorageLocation = this.props.original;
        const products = nodeStorageLocation[this.props.column.id];
        return (
            <button type="button" id={`btn_${this.props.name}_addProducts`} className="ep-btn ep-btn--link" onClick={() => this.props.addProducts(nodeStorageLocation)}>
                <i className={classNames('fas m-r-1', { ['fas fa-eye']: products.length > 0, ['fa-plus-circle']: products.length === 0 })} />
                {
                    products.length > 0 ? `${products.length} ${resourceProvider.read('products')}` : `${resourceProvider.read('add')} ${resourceProvider.read('products')}`
                }
            </button>
        );
    }
}

export class OwnershipCell extends React.Component {
    constructor(props) {
        super(props);
        this.getData = this.getData.bind(this);
    }

    getData() {
        return this.props.original.owners.map(o => {
            return {
                prop: o.ownershipPercentage,
                type: o.owner.color || constants.DefaultColorCode
            };
        });
    }
    render() {
        return (
            <div className="d-flex d-flex--a-start">
                <ul className="ep-sbar" id={`ul_${this.props.name}_ownership`}>
                    {this.getData().map(v => {
                        const style = { width: `${v.prop}%`, backgroundColor: v.type };
                        return (<li key={v.type} className={`ep-sbar__item ep-sbar__item--${v.type}`} style={style} />);
                    })}
                </ul>
                <a className="ep-table__actions-lnk m-l-6" id={`lnk_${this.props.name}_ownership_edit`} onClick={() => this.props.onEditOwners(this.props.original)}>
                    <Tooltip body={resourceProvider.read('edit')}>
                        <em className="fas fa-edit" aria-hidden="true" />
                    </Tooltip>
                </a>
            </div>
        );
    }
}

const checkIfNumberAllowed = input => {
    return numberService.createBigDecimal(input.value).dp() <= 2;
};

export class OwnershipVolumeInputNumberCell extends React.Component {
    render() {
        const ownershipData = this.props.original;
        const isReadonly = this.props.mode === constants.Modes.Read || this.props.mode === constants.Modes.Delete;
        return (
            <NumberFormatter
                value={utilities.parseFloat(ownershipData.ownershipVolume)}
                readOnly={isReadonly}
                isNumericString={true}
                decimalScale={2}
                className="ep-textbox text-right"
                onValueChange={values => {
                    if (!utilities.isNullOrWhitespace(ownershipData.ownershipVolume) && utilities.parseFloat(ownershipData.ownershipVolume) === utilities.parseFloat(values.value)) {
                        return;
                    }
                    this.props.updateOwnershipVolume(ownershipData, values.value);
                }}
                isAllowed={values => checkIfNumberAllowed(values)} />
        );
    }
}

export class OwnershipPercentageInputTextCell extends React.Component {
    render() {
        const ownershipData = this.props.original;
        const isReadonly = this.props.mode === constants.Modes.Read || this.props.mode === constants.Modes.Delete;
        return (
            <div className="ep-control ep-control--addon">
                <div className="ep-control__inner ep-control__inner-bgwt">
                    <NumberFormatter
                        min={constants.DecimalRange.Min}
                        max={constants.PercentageRange.Max}
                        value={utilities.parseFloat(ownershipData.ownershipPercentage)}
                        readOnly={isReadonly}
                        isNumericString={true}
                        className="ep-textbox text-right"
                        onValueChange={values => {
                            if (!utilities.isNullOrWhitespace(ownershipData.ownershipPercentage) && utilities.parseFloat(ownershipData.ownershipPercentage) === utilities.parseFloat(values.value)) {
                                return;
                            }
                            this.props.updateOwnershipPercentage(ownershipData, values.value);
                        }}
                        isAllowed={values => checkIfNumberAllowed(values)} />
                    <span className="ep-control__inner-addon"><i className="fas fa-percentage" aria-hidden="true" /></span>
                </div>
            </div>
        );
    }
}

export class OwnershipStatusCell extends React.Component {
    render() {
        return (
            <div className="ep-sbar ep-sbar--ad">
                <div className="ep-sbar__item ep-sbar__item"
                    style={{ width: `${this.props.original.ownershipPercentage || 0}%`, backgroundColor: `${this.props.original.color}` }} />
            </div>
        );
    }
}

export class TextWithToolTipCell extends React.Component {
    render() {
        const message = this.props.original[this.props.key ? this.props.key : this.props.column.id];
        return (
            <Tooltip body={message} overlayClassName="ep-tooltip--ar-l">
                <div className="ellipsis">{message}</div>
            </Tooltip>
        );
    }
}
