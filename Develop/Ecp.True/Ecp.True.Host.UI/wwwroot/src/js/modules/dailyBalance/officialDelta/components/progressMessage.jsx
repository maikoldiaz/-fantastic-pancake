import React from 'react';
import { resourceProvider } from '../../../../common/services/resourceProvider';

export class OfficialDeltaInProgressMessage extends React.Component {
    render() {
        const message = resourceProvider.read('officialDeltaInProgress');
        const messageParts = message.split(',');
        return (
            <>{messageParts[0]}<strong>{messageParts[1]}</strong>{messageParts[2]}</>
        );
    }
}
