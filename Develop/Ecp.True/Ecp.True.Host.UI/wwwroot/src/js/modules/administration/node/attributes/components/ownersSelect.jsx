import React from 'react';
import { connect } from 'react-redux';
import DualSelect from '../../../../../common/components/dualSelect/dualSelect.jsx';
import { utilities } from '../../../../../common/services/utilities';
import { wizardNextStep, initAllItems } from '../../../../../common/actions';
import { getOwners } from './../../../nodeConnection/attributes/actions';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

export class OwnersSelect extends React.Component {
    render() {
        return (
            <>
                <section className="ep-modal__content">
                    {this.props.owners && <DualSelect sourceTitle="systemOwners" targetTitle="configuredOwners" name="productOwners" />}
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('nodeProducts_ownersselect',
                    { onAccept: this.props.onWizardNext, disableAccept: (this.props.target.length === 0), acceptText: 'next' })} />
            </>
        );
    }

    componentDidMount() {
        if (!this.props.owners || !utilities.isNotEmpty(this.props.owners)) {
            this.props.getOwners();
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.ownersToggler !== this.props.ownersToggler) {
            const all = [];
            this.props.owners.forEach(e => all.push({
                id: e.elementId,
                name: e.name,
                value: 0,
                selected: false
            }));

            this.props.initAllItems(all);
        }
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        owners: state.nodeConnection.attributes.owners,
        ownersToggler: state.nodeConnection.attributes.ownersToggler,
        target: state.dualSelect.productOwners ? state.dualSelect.productOwners.target : [],
        source: state.dualSelect.productOwners ? state.dualSelect.productOwners.source : []
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        onWizardNext: () => {
            dispatch(wizardNextStep('editNodeOwnership'));
        },
        getOwners: () => {
            dispatch(getOwners());
        },
        initAllItems: all => {
            dispatch(initAllItems(all, 'productOwners'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(OwnersSelect);
