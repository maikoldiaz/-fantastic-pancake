import React from 'react';
import { constants } from './../../services/constants.js';
import { responsiveService } from './../../services/responsiveService.js';

export default class DesktopOrTablet extends React.Component {
    render() {
        return responsiveService.getResponsiveElement(this.props.children, { minWidth: constants.ResponsiveBreakpoints.TABLETMIN });
    }
}
