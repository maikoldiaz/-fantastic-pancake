import React from 'react';
import Responsive from 'react-responsive';
export const responsiveService = {
    getResponsiveElement: (children, props) => {
        return <Responsive {...props}>{children}</Responsive>;
    }
};
