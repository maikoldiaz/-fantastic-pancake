import React from 'react';
import { constants } from './../../services/constants.js';
import { responsiveService } from './../../services/responsiveService.js';

export default class MobileOrTablet extends React.Component {
    render() {
        return responsiveService.getResponsiveElement(this.props.children, { minWidth: constants.ResponsiveBreakpoints.MOBILEMIN, maxWidth: constants.ResponsiveBreakpoints.TABLETMAX });
    }
}
