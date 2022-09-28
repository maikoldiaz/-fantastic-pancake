import React from 'react';
import ReactDOM from 'react-dom';

export default class Portal extends React.Component {
    constructor() {
        super();

        this.rootSelector = document.getElementById('epPortals');
        this.container = document.createElement('div');
    }

    render() {
        return ReactDOM.createPortal(this.props.children, this.container);
    }

    componentDidMount() {
        if (this.rootSelector) {
            this.rootSelector.appendChild(this.container);
        }
    }

    componentWillUnmount() {
        this.rootSelector.removeChild(this.container);
    }
}
