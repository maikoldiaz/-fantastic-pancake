import React from 'react';
import { connect } from 'react-redux';
import TabPanels from '../../../../common/components/tabPanels/tabPanels.jsx';
import StorageLocationProductsGrid from './storageLocationProduct/storageLocationProductsGrid.jsx';
import ProductsGrid from './product/productsGrid.jsx';
import { changeTab } from '../../../../common/actions';

class ProductConfigurationPanel extends React.Component {
    constructor() {
        super();
    }

    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <TabPanels name="productConfigurationPanel">
                        <div title="products">
                            <ProductsGrid />
                        </div>
                        <div title="storageLocationProducts">
                            <StorageLocationProductsGrid />
                        </div>
                    </TabPanels>
                </div>
            </section>
        );
    }

    componentWillUnmount() {
        this.props.changeTabPanel('productConfigurationPanel', 'products');
    }
}

/* istanbul ignore next */
const mapStateToProps = state => {
    return {
        activeTab: state.tabs.productConfigurationPanel ? state.tabs.productConfigurationPanel.activeTab : null
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        changeTabPanel: (name, activeTab) => {
            dispatch(changeTab(name, activeTab));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(ProductConfigurationPanel);
