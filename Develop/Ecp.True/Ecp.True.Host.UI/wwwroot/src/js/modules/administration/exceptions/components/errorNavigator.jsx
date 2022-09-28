import React from 'react';
import { navigationService } from '../../../../common/services/navigationService';
import { routerActions } from '../../../../common/router/routerActions';
import ErrorDetail from './errorDetail.jsx';
import ErrorsGrid from './errorsGrid.jsx';
import { constants } from '../../../../common/services/constants';
import { utilities } from '../../../../common/services/utilities';

export default class ErrorNavigator extends React.Component {
    constructor() {
        super();
        routerActions.configure('returnListing', () => navigationService.goBack());
    }

    isValidSuffix(suffix) {
        return !suffix || utilities.equalsIgnoreCase(suffix, 'F') || utilities.equalsIgnoreCase(suffix, 'P');
    }

    getInfo() {
        const url = navigationService.getParamByName('errorId').split('_');
        const type = url.length === 1 ? constants.DetailsPageType.Grid : constants.DetailsPageType.Details;
        const suffix = url.length > 1 ? url[1] : null;

        if (!this.isValidSuffix(suffix)) {
            navigationService.handleError(constants.Errors.NotFound);
            return null;
        }

        return {
            type,
            id: url[0]
        };
    }

    render() {
        const info = this.getInfo();
        return (
            <>
                {info && <section className="ep-content">
                    <>
                        {info.type === constants.DetailsPageType.Details ?
                            <ErrorDetail errorId={info.id} {...this.props} componentType="exceptions/manage" /> :
                            <ErrorsGrid errorId={info.id} {...this.props} />}
                    </>
                </section>}
            </>
        );
    }
}
