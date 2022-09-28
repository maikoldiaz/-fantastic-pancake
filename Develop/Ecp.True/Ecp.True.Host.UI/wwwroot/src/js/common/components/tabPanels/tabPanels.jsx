import React from 'react';
import { connect } from 'react-redux';
import { changeTab, initTab } from '../../actions.js';
import { resourceProvider } from './../../services/resourceProvider';

class TabPanels extends React.Component {
    componentDidMount() {
        const { tabs } = this.props;
        const defaultTab = this.props.defaultTab;
        const tabActivated = tabs && tabs.activeTab;
        if (this.props.name) {
            if (defaultTab) {
                const tabTitle = defaultTab;
                this.props.initTabPanel(this.props.name, tabTitle);
            } else {
                const tabTitle = tabActivated ? tabActivated : this.props.children[0].props.title;
                this.props.initTabPanel(this.props.name, tabTitle);
            }
        }
    }
    render() {
        const tabs = this.props.tabs;
        const children = this.props.children;
        const activeTab = (tabs && tabs.activeTab) ? this.props.tabs.activeTab : this.props.children[0].props.title;

        return (
            <section className="ep-tabpanels">
                <ul className="ep-tabpanels__tabs">
                    {children.map(child => {
                        const { title, onTabClick } = child.props;
                        return (
                            <Tab
                                key={title}
                                title={title}
                                activeTab={activeTab}
                                onClick={this.props.onClickTabItem} onTabClick={onTabClick} {...this.props}
                            />
                        );
                    })}
                </ul>

                <div className="ep-tabpanels__panels">
                    <div className="ep-tabpanels__panel">
                        {children.map(child => {
                            if (child.props.title !== activeTab) {
                                return null;
                            }
                            return child.props.children;
                        })}
                    </div>
                </div>
            </section>
        );
    }
}

class Tab extends React.Component {
    onTabClick(name, title, onTabClick) {
        if (typeof (onTabClick) === 'function') {
            onTabClick();
        } else {
            this.props.onClickTabItem(name, title);
        }
    }
    render() {
        const { title, activeTab, name, onTabClick } = this.props;

        let className = 'ep-tabpanels__tab';

        if (activeTab === title) {
            className = className + ' ep-tabpanels__tab--active';
        }

        return (
            <li className={className} onClick={() => this.onTabClick(name, title, onTabClick)}>{resourceProvider.read(title)}</li>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        tabs: state.tabs[ownProps.name]
    };
};

/* istanbul ignore next */
const mapDispatchToProps = dispatch => {
    return {
        initTabPanel: (name, activeTab, canChangeTab) => {
            dispatch(initTab(name, activeTab, canChangeTab));
        },
        onClickTabItem: (name, activeTab) => {
            dispatch(changeTab(name, activeTab));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps)(TabPanels);
