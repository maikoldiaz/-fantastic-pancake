import React from 'react';
import { navigationService } from '../../../../common/services/navigationService';
import { routerActions } from '../../../../common/router/routerActions';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';
import OwnershipNodeGrid from './ownershipNodeGrid.jsx';
import NodeOwnership from './nodeOwnership.jsx';

export default class OwnershipNodeNavigator extends React.Component {
    constructor() {
        super();
        routerActions.configure('returnListing', () => navigationService.goBack());
    }

    isValidSuffix(suffix) {
        return !suffix || utilities.equalsIgnoreCase(suffix, constants.DetailsPageType.Grid) || utilities.equalsIgnoreCase(suffix, constants.DetailsPageType.Details);
    }

    getInfo() {
        const url = navigationService.getParamByName('ticketNodeId').split('_');
        const suffix = url.length > 1 ? url[1] : null;

        if (!this.isValidSuffix(suffix)) {
            navigationService.handleError(constants.Errors.NotFound);
            return null;
        }

        return {
            suffix,
            id: url[0]
        };
    }

    render() {
        const info = this.getInfo();
        return (
            <>
                {info && <section className="ep-content">
                    <>
                        {utilities.equalsIgnoreCase(info.suffix, constants.DetailsPageType.Grid) ?
                            <OwnershipNodeGrid ticketId={info.id} {...this.props} hideAction="ownershipNodeActionBar" /> :
                            <NodeOwnership ownershipNodeId={info.id} {...this.props} showAction="ownershipNodeActionBar" />}
                    </>
                </section>}
            </>
        );
    }
}
