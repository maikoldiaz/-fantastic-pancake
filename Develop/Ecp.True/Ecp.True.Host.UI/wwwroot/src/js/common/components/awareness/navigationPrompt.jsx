import React from 'react';
import { withRouter } from 'react-router-dom';
import { resourceProvider } from '../../services/resourceProvider';

class NavigationPrompt extends React.Component {
    constructor() {
        super();
        this.state = {
            nextLocation: null,
            openModal: false
        };
        this.when = this.when.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.close = this.close.bind(this);
        this.onDiscard = this.onDiscard.bind(this);
        this.onPublish = this.onPublish.bind(this);
        this.onBeforeUnload = this.onBeforeUnload.bind(this);
    }

    when(nextLocation) {
        if (typeof this.props.when === 'function') {
            return this.props.when(nextLocation, this.props.location);
        }
        return this.props.when;
    }

    onBeforeUnload(e) {
        if (!this.when()) {
            return;
        }
        const msg = resourceProvider.read('YouHaveNotSavedYourChanges');
        e.returnValue = msg;
    }

    onCancel() {
        this.setState({
            nextLocation: null,
            openModal: false
        });
    }

    close() {
        this.setState({
            openModal: false
        });
    }

    onDiscard() {
        this.props.onDiscard();
        this.unblock();
        this.props.history.push(this.state.nextLocation.pathname.toLowerCase());
    }

    onPublish() {
        this.props.onPublish();
        this.setState({
            openModal: false
        });
    }

    render() {
        return (
            <React.Fragment>
                {this.props.children(this.state.openModal, this.onDiscard, this.onCancel, this.close, this.onPublish, this.props.enablePublish)}
            </React.Fragment>
        );
    }

    componentDidMount() {
        this.unblock = this.props.history.block(nextLocation => {
            const result = this.when(nextLocation) || this.props.blockNavigation;
            if (result) {
                this.setState({
                    openModal: !this.props.blockNavigation,
                    nextLocation
                });
            }
            return !result;
        });
        window.addEventListener('beforeunload', this.onBeforeUnload);
    }

    componentWillUnmount() {
        this.unblock();
        window.removeEventListener('beforeunload', this.onBeforeUnload);
    }
}

export default withRouter(NavigationPrompt);
