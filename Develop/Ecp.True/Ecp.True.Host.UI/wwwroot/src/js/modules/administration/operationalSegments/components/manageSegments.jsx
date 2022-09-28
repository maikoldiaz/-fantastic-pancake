import React from 'react';
import { connect } from 'react-redux';
import DualSelect from '../../../../common/components/dualSelect/dualSelect.jsx';
import { resourceProvider } from '../../../../common/services/resourceProvider';
import { saveSegments, updateSegments } from '../actions';
import { getCategoryElements, initAllItems, updateTargetItems, openMessageModal, configureDualSelect, clearSearchText } from '../../../../common/actions';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';

export class ManageSegments extends React.Component {
    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <div className="ep-section ep-section--panel">
                        <h2 className="ep-section__title fs-24 fw-600 m-t-5 m-l-4">{resourceProvider.read('operationalSegments')}</h2>
                        <div className="ep-section__body ep-section__body--h ep-section__body--w660 m-auto">
                            <p className="m-b-6 fs-16">{resourceProvider.read('configDeSegmentMsg')}</p>
                            {this.props.groupedCategoryElements &&
                                <DualSelect name={this.props.name} sourceTitle="dualSelectLeftTtl" targetTitle="dualSelectRightTtl" />}
                            <div className="text-right m-t-4">
                                <button type="submit" className="ep-btn ep-btn--sm" id="btn_segments_submit"
                                    disabled={this.props.target.length === 0}
                                    onClick={() => this.props.saveSegments(this.props.target.map(({ id, rowVersion }) => ({ elementId: id, rowVersion })))}>
                                    {resourceProvider.read('submit')}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        );
    }

    componentDidMount() {
        this.props.configureDualSelect(this.props.name);
        this.props.getCategoryElements(true);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.categoryElementsDataToggler !== this.props.categoryElementsDataToggler) {
            this.buildSegments();
        }
        if (prevProps.updateSonSegmentsFailureToggler !== this.props.updateSonSegmentsFailureToggler) {
            this.props.openModal(resourceProvider.read('updateSonSegmentsFailureTitle'), resourceProvider.read('updateSonSegmentsFailure'));
        }
        if (prevProps.updateSonSegmentsSuccessToggler !== this.props.updateSonSegmentsSuccessToggler) {
            this.props.getCategoryElements(true);
        }
        if (prevProps.updateSegmentsToggler !== this.props.updateSegmentsToggler) {
            this.props.initAllItems(this.props.others);
            this.props.updateTargetItems(this.props.operational);
        }
    }

    buildSegments() {
        this.props.clearSearchText(this.props.name);
        const others = this.props.groupedCategoryElements[constants.Category.Segment].filter(x => x.isOperationalSegment !== true && x.isActive === true);
        utilities.sortBy(others, 'name');

        const operational = this.props.operationalSegments;
        utilities.sortBy(operational, 'name');

        this.props.updateSegments(operational, others);
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        name: 'segments',
        groupedCategoryElements: state.shared.groupedCategoryElements,
        operationalSegments: state.shared.operationalSegments,
        categoryElementsDataToggler: state.shared.categoryElementsDataToggler,
        target: state.dualSelect.segments ? state.dualSelect.segments.target : [],
        updateSonSegmentsFailureToggler: state.manageSegment.segments.updateSonSegmentsFailureToggler,
        updateSonSegmentsSuccessToggler: state.manageSegment.segments.updateSonSegmentsSuccessToggler,
        updateSegmentsToggler: state.manageSegment.segments.updateSegmentsToggler,
        operational: state.manageSegment.segments.operational,
        others: state.manageSegment.segments.others
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        saveSegments: target => {
            dispatch(saveSegments(target));
        },
        getCategoryElements: forceFetch => {
            dispatch(getCategoryElements(forceFetch));
        },
        initAllItems: sourceItems => {
            dispatch(initAllItems(sourceItems, 'segments'));
        },
        updateTargetItems: targetItems => {
            dispatch(updateTargetItems(targetItems, 'segments'));
        },
        openModal: (title, message) => {
            dispatch(openMessageModal(message, {
                title: title,
                titleClassName: 'text-unset'
            }));
        },
        resetDualSelect: name => {
            dispatch(configureDualSelect(name));
        },
        updateSegments: (operational, others) => {
            dispatch(updateSegments(operational, others));
        },
        clearSearchText: name => {
            dispatch(clearSearchText(name));
        },
        configureDualSelect: name => {
            dispatch(configureDualSelect(name));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ManageSegments);
