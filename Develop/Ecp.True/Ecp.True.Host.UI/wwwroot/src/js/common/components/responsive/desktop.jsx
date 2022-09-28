import React from 'react';
import { constants } from './../../services/constants.js';
import { responsiveService } from './../../services/responsiveService.js';

export default class Desktop extends React.Component {
    render() {
        return responsiveService.getResponsiveElement(this.props.children, { minWidth: constants.ResponsiveBreakpoints.DESKTOPMIN });
    }
}
