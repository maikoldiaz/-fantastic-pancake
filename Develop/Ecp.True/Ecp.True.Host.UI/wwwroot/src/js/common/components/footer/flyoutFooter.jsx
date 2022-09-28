import React from 'react';
import { resourceProvider } from '../../services/resourceProvider';

export default class FlyoutFooter extends React.Component {
    render() {
        const config = this.props.config;
        return (
            <footer className="ep-flyout__footer">
                <button id={`btn_${config.key}_submit`} className={config.className} type={config.type} onClick={config.onClick}>{resourceProvider.read(config.text)}</button>
            </footer>
        );
    }
}
