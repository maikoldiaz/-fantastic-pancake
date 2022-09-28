import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import Flyout from './../../../../common/components/flyout/flyout.jsx';
import { closeFlyout } from './../../../../common/actions';
import { inputSelect } from './../../../../common/components/formControl/formControl.jsx';
import { resourceProvider } from './../../../../common/services/resourceProvider';
import FlyoutFooter from '../../../../common/components/footer/flyoutFooter.jsx';
import { footerConfigService } from '../../../../common/services/footerConfigService';

class NodeAttributesFilter extends React.Component {
    render() {
        const props = this.props;
        return (
            <Flyout name={props.name}>
                <header className="ep-flyout__header">
                    <button className="ep-btn ep-btn--tr" onClick={() => this.props.closeFlyout(props.name)}>{resourceProvider.read('cancel')}</button>
                    <h1 className="ep-flyout__title">{resourceProvider.read('searchCriteria')}</h1>
                    <button className="ep-btn ep-btn--tr">{resourceProvider.read('reset')}</button>
                </header>
                <section className="ep-flyout__body">
                    <section className="ep-filters">
                        <article className="ep-filter">
                            <div className="ep-filter__count">1</div>
                            <div className="ep-filter__body">
                                <div className="ep-control-group m-b-3">
                                    <span className="ep-label">{resourceProvider.read('category')}</span>
                                    <Field component={inputSelect} placeholder="category" name="category" />
                                </div>
                                <div className="ep-control-group m-b-0">
                                    <span className="ep-label">{resourceProvider.read('element')}</span>
                                    <Field component={inputSelect} placeholder="element" name="element" />
                                </div>
                            </div>
                        </article>
                        <article className="ep-filter">
                            <div className="ep-filter__count">2</div>
                            <div className="ep-filter__body">
                                <div className="ep-control-group m-b-3">
                                    <label className="ep-label">{resourceProvider.read('category')}</label>
                                    <Field component={inputSelect} placeholder="category" name="category" />
                                </div>
                                <div className="ep-control-group m-b-0">
                                    <label className="ep-label">{resourceProvider.read('element')}</label>
                                    <Field component={inputSelect} placeholder="element" name="element" />
                                </div>
                                <label className="ep-filter__type">
                                    <input className="ep-filter__type-inp" type="checkbox" />
                                    <span className="ep-filter__type-y">Y</span>
                                    <span className="ep-filter__type-o">O</span>
                                </label>
                            </div>
                            <button className="ep-filter__action"><i className="fas fa-trash" /></button>
                        </article>
                        <div className="m-t-3 float-r">
                            <button className="ep-btn ep-btn--link">{resourceProvider.read('addOrFilter')}</button>
                            <button className="ep-btn ep-btn--link">{resourceProvider.read('addAndFilter')}</button>
                        </div>
                    </section>
                </section>
                <FlyoutFooter config={footerConfigService.getFlyoutConfig('attributeFilter', 'applyFilters')} />
            </Flyout>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = () => {
    return {

    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        closeFlyout: flyoutName => {
            dispatch(closeFlyout(flyoutName));
        }
    };
};

const NodeAttributesFilterForm = reduxForm({ form: 'nodeAttributesFilter' })(NodeAttributesFilter);

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(NodeAttributesFilterForm);
