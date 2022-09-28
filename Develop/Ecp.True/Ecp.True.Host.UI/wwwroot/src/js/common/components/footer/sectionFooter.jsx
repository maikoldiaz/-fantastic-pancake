import React from 'react';
import ModalFooter from './modalFooter.jsx';
import { constants } from '../../services/constants';

export default class SectionFooter extends React.Component {
    render() {
        return (
            <ModalFooter floatRight={this.props.floatRight} config={this.props.config} type={constants.Footer.Section} />
        );
    }
}
