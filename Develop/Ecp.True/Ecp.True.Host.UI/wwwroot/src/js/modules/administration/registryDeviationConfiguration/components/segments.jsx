import React from 'react';
import { connect } from 'react-redux';
import { reduxForm, FieldArray } from 'redux-form';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { filterCategoryElements, updateDeviation } from '../actions';
import SectionFooter from '../../../../common/components/footer/sectionFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';
import { utilities } from '../../../../common/services/utilities';
import { showSuccess } from '../../../../common/actions';
import InputSegment from './inputSegment.jsx';

export class Segments extends React.Component {
    constructor() {
        super();

        this.onSubmit = this.onSubmit.bind(this);
        this.updateForm = this.updateForm.bind(this);
        this.handleFilterChange = this.handleFilterChange.bind(this);
        this.state = {
            filterText: ''
        };
    }

    onSubmit(values) {
        const segments = this.filterSegments(values).map(item => ({
            ...item,
            deviationPercentage: parseFloat(item.deviationPercentage)
        }));

        if (segments.length > 0) {
            this.props.updateDeviation(segments);
        }
    }

    filterSegments(values) {
        const segments = this.props.segments.map(item =>
            values.segments.find(segment => item.elementId === segment.elementId
                && parseFloat(item.deviationPercentage) !== parseFloat(segment.deviationPercentage)));

        return segments.filter(item => !utilities.isNullOrUndefined(item));
    }

    updateForm(event) {
        this.handleKeyDown(event, true);
    }

    handleKeyDown(event, isFiltered) {
        if (event.keyCode === 13 && !isFiltered) {
            event.preventDefault();
        } else if (event.keyCode === 13 && isFiltered) {
            this.props.updateSegments(event.target.value.trim());
        }
    }

    handleFilterChange(event) {
        this.setFilterText(event.target.value);
    }

    setFilterText(value) {
        this.setState({ filterText: value });
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <div className="ep-section__body ep-section__body--h">
                            <section className="ep-section__content">
                                <h2 className="ep-control-group-title m-l-3">{resourceProvider.read('assignLogDeviationPercentage')}</h2>
                                <div className="row m-b-6">
                                    <div className="col-md-3 m-l-3">
                                        <label className="ep-label">{resourceProvider.read('name') + ' ' + resourceProvider.read('segment')}:</label>
                                        <div className="ep-control ep-control--decimal ep-control--addon">
                                            <div className="ep-control__inner">
                                                <input id="filter-text-input" className="ep-textbox" onKeyDown={this.updateForm} value={this.state.filterText} onChange={this.handleFilterChange} />
                                                <span className="ep-control__inner-addon"><i className="fas fa-search" /></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <form className="full-height" onSubmit={this.props.handleSubmit(this.onSubmit)} onKeyDown={e => this.handleKeyDown(e, false)}>
                                    <div className="row">
                                        <FieldArray name="segments"
                                            component={InputSegment}
                                            maxDeviationPercentage={this.props.maxDeviationPercentage}
                                            minDeviationPercentage={this.props.minDeviationPercentage} />
                                    </div>

                                    <SectionFooter floatRight={true} config={footerConfigService.getAcceptConfig('segmentFields',
                                        {
                                            disableAccept: this.props.invalid,
                                            acceptText: resourceProvider.read('submit')
                                        })} />
                                </form>
                            </section>
                        </div>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.updateSegments('');
    }

    componentDidUpdate(prevProps) {
        if (this.props.updatedDeviationToggler !== prevProps.updatedDeviationToggler) {
            this.setFilterText('');
            this.props.updateSegments('');
            this.props.showSuccess(resourceProvider.read('segmentDeviationSuccess'));
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        initialValues: {
            segments: state.registryDeviation.categoryElements
        },
        segments: state.registryDeviation.categoryElements,
        updatedDeviationToggler: state.registryDeviation.updatedDeviationToggler,
        maxDeviationPercentage: utilities.parseFloat(state.root.systemConfig.maxDeviationPercentage),
        minDeviationPercentage: 0
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        updateSegments: value => {
            const filter = `$filter=contains(name,'${value}') and categoryId eq 2 and isActive eq true`;
            dispatch(filterCategoryElements(filter));
        },
        updateDeviation: segments => {
            dispatch(updateDeviation(segments));
        },
        showSuccess: message => {
            dispatch(showSuccess(message));
        }
    };
};

const SegmentsForm = reduxForm({
    form: 'segments',
    enableReinitialize: true
})(Segments);

export default connect(mapStateToProps, mapDispatchToProps)(SegmentsForm);
