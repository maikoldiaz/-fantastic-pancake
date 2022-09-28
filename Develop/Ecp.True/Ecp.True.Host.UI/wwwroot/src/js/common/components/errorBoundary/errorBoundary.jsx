import React from 'react';

class ErrorBoundary extends React.Component {
    constructor() {
        super();
        this.state = { error: null, errorInfo: null };
    }

    componentDidCatch(error, errorInfo) {
        this.setState({
            error: error,
            errorInfo: errorInfo
        });
    }

    render() {
        if (this.state.errorInfo) {
            return (
                <div className="toyo-message-wrapper toyo-message-wrapper--err">
                    <div className="toyo-message toyo-message--tr">
                        <i className="fas fa-exclamation-triangle toyo-message__icn" />
                        <div className="toyo-message__content">Something Went Wrong</div>
                    </div>
                </div>
            );
        }
        return this.props.children;
    }
}

export default ErrorBoundary;
