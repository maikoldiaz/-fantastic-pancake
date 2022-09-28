import React from 'react';
import { connect } from 'react-redux';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { navigationService } from '../../../../common/services/navigationService';
import { dateService } from '../../../../common/services/dateService';
import { routerActions } from '../../../../common/router/routerActions';
import { openModal, enableDisablePageAction, setModuleName } from '../../../../common/actions';
import { initErrorAddComment, setSelectedData, getErrorDetail, requestRetryRecord } from '../actions';
import { utilities } from '../../../../common/services/utilities';
import NumberFormatter from '../../../../common/components/formControl/numberFormatter.jsx';

class ErrorDetail extends React.Component {
    constructor() {
        super();

        this.onDiscardException = this.onDiscardException.bind(this);
        this.getProcessName = this.getProcessName.bind(this);
        this.onRetryRecord = this.onRetryRecord.bind(this);

        routerActions.configure('discardException', this.onDiscardException);
        routerActions.configure('retryRecord', this.onRetryRecord);
    }

    onDiscardException() {
        this.props.onDiscardException(resourceProvider.read('addCommentPreTextSingular'), resourceProvider.read('addCommentPostTextSingular'), 1);
    }

    onRetryRecord() {
        this.props.onRetry([this.props.errorDetail.fileRegistrationTransactionId]);
    }

    getProcessName() {
        if (!this.props.errorDetail) {
            return '';
        }

        if (this.props.errorDetail.process) {
            return `${utilities.toLowerCase(this.props.errorDetail.process)}Register`;
        }

        return '';
    }

    getErrors() {
        if (!this.props.errorDetail || !this.props.errorDetail.error) {
            return [];
        }

        return this.props.errorDetail.error.split('__').filter(a => !utilities.isNullOrWhitespace(a));
    }

    getSystemName(systemName, systemTypeName) {
        if (utilities.isNullOrUndefined(systemName) || utilities.isNullOrWhitespace(systemName)) {
            return systemTypeName;
        }
        return systemName;
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <header className="ep-section__header p-a-0">
                            <div className="row m-x-0">
                                <div className="col-md-3 p-x-0">
                                    <div className="ep-control-group ep-control-group--mh84 p-a-4 m-b-0 br-l-1">
                                        <h3 className="ep-section__subtitle">
                                            {resourceProvider.read('systemType')}
                                        </h3>
                                        <span className="ep-data">
                                            {this.props.errorDetail ? this.getSystemName(this.props.errorDetail.systemName, this.props.errorDetail.systemTypeName) : ''}
                                        </span>
                                    </div>
                                </div>
                                <div className="col-md-3 p-x-0">
                                    <div className="ep-control-group ep-control-group--mh84 p-a-4 m-b-0 br-l-1">
                                        <h3 className="ep-section__subtitle">
                                            {resourceProvider.read('exceptionDate')}
                                        </h3>
                                        <span className="ep-data text-caps">
                                            {dateService.format(this.props.errorDetail ? this.props.errorDetail.creationDate : '')}
                                        </span>
                                    </div>
                                </div>
                                <div className="col-md-6 p-x-0">
                                    <div className="ep-control-group ep-control-group--mh84 p-a-4 m-b-0">
                                        <h3 className="ep-section__subtitle">
                                            {resourceProvider.read('nameOffile')}
                                        </h3>
                                        <span className="ep-data text-caps">
                                            {this.props.errorDetail ? this.props.errorDetail.fileName : ''}
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </header>
                        <div className="ep-section__body ep-section__body--f p-a-0">
                            <h3 className="ep-section__subtitle p-a-4 p-b-0">
                                {resourceProvider.read('detailOfExceptions')}
                            </h3>
                            <section className="p-a-4 p-b-0">
                                <h3 className="ep-section__subtitle ep-section__subtitle--bg p-a-2 m-b-3">
                                    {resourceProvider.read('error')}
                                </h3>
                                <div className="row">
                                    <div className="col-md-12">
                                        {this.getErrors().map((value, index) => {
                                            return <p className="m-t-0 m-l-2" key={index}>{value}</p>;
                                        })}
                                    </div>
                                </div>
                            </section>
                            <section className="p-a-4 p-b-0">
                                <h3 className="ep-section__subtitle ep-section__subtitle--bg p-a-2 m-b-3">
                                    General
                                </h3>
                                <div className="row">
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('uploadFileId')}</label>
                                            <span className="ep-data">
                                                {this.props.errorDetail ? this.props.errorDetail.identifier : ''}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('type')}</label>
                                            <span className="ep-data">
                                                {this.props.errorDetail ? this.props.errorDetail.type : ''}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('segment')}</label>
                                            <span className="ep-data">
                                                {this.props.errorDetail ? this.props.errorDetail.segment : ''}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('process')}</label>
                                            <span className="ep-data">
                                                {resourceProvider.read(this.getProcessName())}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div className="row">
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('netVolume')}</label>
                                            <NumberFormatter
                                                value={this.props.errorDetail ? utilities.parseFloat(this.props.errorDetail.volume) : ''}
                                                isNumericString={true}
                                                displayType="text"
                                                className="ep-data" />
                                        </div>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('units')}</label>
                                            <span className="ep-data">
                                                {this.props.errorDetail ? this.props.errorDetail.measurementUnit : ''}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('initialDate')}</label>
                                            <span className="ep-data text-caps">
                                                {this.props.errorDetail ? dateService.format(this.props.errorDetail.initialDate) : ''}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="col-md-3">
                                        <div className="ep-control-group m-b-6">
                                            <label className="ep-label">{resourceProvider.read('finalDate')}</label>
                                            <span className="ep-data text-caps">
                                                {this.props.errorDetail ? dateService.format(this.props.errorDetail.finalDate) : ''}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </section>
                            <div className="row m-x-0">
                                <div className="col-md-6 p-x-0">
                                    <section className="p-a-4 p-t-0 p-r-2">
                                        <h3 className="ep-section__subtitle ep-section__subtitle--bg p-a-2 m-b-3">
                                            {resourceProvider.read('node')}
                                        </h3>
                                        <div className="row m-x-0">
                                            <div className="col-md-6 p-x-0">
                                                <div className="ep-control-group m-b-6">
                                                    <label className="ep-label">{resourceProvider.read('originLbl')}</label>
                                                    <span className="ep-data">
                                                        {this.props.errorDetail ? this.props.errorDetail.sourceNode : ''}
                                                    </span>
                                                </div>
                                            </div>
                                            <div className="col-md-6 p-x-0">
                                                <div className="ep-control-group m-b-6">
                                                    <label className="ep-label">{resourceProvider.read('destinationLbl')}</label>
                                                    <span className="ep-data">
                                                        {this.props.errorDetail ? this.props.errorDetail.destinationNode : ''}
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </div>
                                <div className="col-md-6 p-x-0">
                                    <section className="p-a-4 p-t-0 p-l-2">
                                        <h3 className="ep-section__subtitle ep-section__subtitle--bg p-a-2 m-b-3">
                                            {resourceProvider.read('product')}
                                        </h3>
                                        <div className="row m-x-0">
                                            <div className="col-md-6 p-x-0">
                                                <div className="ep-control-group m-b-6">
                                                    <label className="ep-label">{resourceProvider.read('originLbl')}</label>
                                                    <span className="ep-data">
                                                        {this.props.errorDetail ? this.props.errorDetail.sourceProduct : ''}
                                                    </span>
                                                </div>
                                            </div>
                                            <div className="col-md-6 p-x-0">
                                                <div className="ep-control-group m-b-6">
                                                    <label className="ep-label">{resourceProvider.read('destinationLbl')}</label>
                                                    <span className="ep-data">
                                                        {this.props.errorDetail ? this.props.errorDetail.destinationProduct : ''}
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </section>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.getErrorDetail();
    }

    componentDidUpdate(prevProps) {
        if (this.props.errorDetail) {
            this.props.enableDisableRetry(!this.props.errorDetail.isRetry);
        }

        if (prevProps.retryToggler !== this.props.retryToggler) {
            navigationService.goBack();
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        errorDetail: state.controlexception.controlException.errorDetail[0],
        retryToggler: state.controlexception.controlException.retryToggler
    };
};

const mapDispatchToProps = dispatch => {
    return {
        getErrorDetail: () => {
            dispatch(getErrorDetail(navigationService.getParamByName('errorId')));
        },
        enableDisableRetry: disabled => {
            dispatch(enableDisablePageAction('retryRecord', disabled));
            dispatch(enableDisablePageAction('discardException', true));
        },
        onDiscardException: (preText, postText, count) => {
            const errorData = {
                errorId: navigationService.getParamByName('errorId')
            };

            dispatch(setSelectedData([errorData]));
            dispatch(openModal('addComments'));
            dispatch(initErrorAddComment('pendingTransactionErrors', preText, postText, count));
        },
        setModuleName: moduleName => {
            dispatch(setModuleName(moduleName));
        },
        onRetry: pendingTransactionErrors => {
            dispatch(requestRetryRecord(pendingTransactionErrors));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(ErrorDetail);
