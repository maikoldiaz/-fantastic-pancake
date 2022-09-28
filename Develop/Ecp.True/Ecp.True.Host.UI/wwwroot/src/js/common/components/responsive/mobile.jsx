import React from 'react';
import { constants } from './../../services/constants.js';
import { responsiveService } from './../../services/responsiveService.js';

export default class Mobile extends React.Component {
    render() {
        return responsiveService.getResponsiveElement(this.props.children,
            { minWidth: constants.ResponsiveBreakpoints.MOBILEMIN, maxWidth: constants.ResponsiveBreakpoints.MOBILEMAX });
    }
}
