import React from 'react';
import FileUploadsGrid from '../../fileUploads/components/fileUploadGrid.jsx';

export default class FileUploadGrid extends React.Component {
    render() {
        return (
            <FileUploadsGrid {...this.props} />
        );
    }
}
