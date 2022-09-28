import React from 'react';
import { connect } from 'react-redux';
import { openModal } from '../../../../../common/actions';
import { PieChart, Pie, Legend, Cell } from 'recharts';
import { ChartLegend } from './../../../../../common/components/chart/chartComponents.jsx';
import { constants } from './../../../../../common/services/constants';
import ModalFooter from '../../../../../common/components/footer/modalFooter.jsx';
import { footerConfigService } from '../../../../../common/services/footerConfigService';

class OwnersPie extends React.Component {
    constructor() {
        super();
        this.buildData = this.buildData.bind(this);
        this.editOwners = this.editOwners.bind(this);
    }

    buildData() {
        return this.props.connectionProduct.owners.map(o => {
            return {
                name: o.owner.name,
                value: Number(o.ownershipPercentage),
                color: o.owner.color || constants.DefaultColorCode
            };
        });
    }

    editOwners() {
        this.props.closeModal();
        this.props.editOwners();
    }

    render() {
        const data = this.buildData();
        const legend = p => <ChartLegend {...p} units="%" />;
        return (
            <>
                <section className="ep-modal__content ep-modal__content--scroll" id="cont_connectionProducts_ownership_pie">
                    <PieChart className="ep-chart" width={530} height={250}>
                        <Pie data={data} dataKey="value" cx="32%" cy="50%" innerRadius={55} outerRadius={90} paddingAngle={0}> {
                            data.map(entry => <Cell key={entry.name} fill={entry.color} />)
                        }
                        </Pie>
                        <Legend align="right" verticalAlign="middle" layout="vertical" iconType="square" height={36} content={legend} />
                    </PieChart>
                </section>
                <ModalFooter config={footerConfigService.getCommonConfig('connectionProducts_ownership_pie',
                    { onAccept: this.editOwners, acceptText: 'edit' })} />
            </>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        connectionProduct: state.nodeConnection.attributes.connectionProduct
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        editOwners: () => {
            dispatch(openModal('editOwners'));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(OwnersPie);
