import React from 'react';
import FileUploadGrid from '../../../transportBalance/fileUploads/components/fileUploadGrid.jsx';

export default class ContractsGrid extends React.Component {
    render() {
        return (
            <FileUploadGrid {...this.props} />
        );
    }
}
