import React from 'react';
import { resourceProvider } from '../../../../common/services/resourceProvider';

export class OfficialPointsCommentMessage extends React.Component {
    render() {
        const message = resourceProvider.read('officialAddNoteMessage');
        const messageParts = message.split(',');
        return (
            <p> <span className="fs-16 m-b-3">{messageParts[0]}</span><span className="fw-bold fs-16 m-b-3 fc-secondary">{messageParts[1]}</span>
                <span className="fs-16 m-b-3">{messageParts[2]}</span><span className="fw-bold fs-16 m-b-3 fc-secondary">{messageParts[3]}</span>
                <span className="fs-16 m-b-3">{messageParts[4]}</span></p>
        );
    }
}
