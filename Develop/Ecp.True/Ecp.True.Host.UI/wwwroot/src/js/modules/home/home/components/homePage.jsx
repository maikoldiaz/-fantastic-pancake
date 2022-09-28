import React from 'react';
import { resourceProvider } from '../../../../common/services/resourceProvider';

export default class HomePage extends React.Component {
    render() {
        return (
            <section className="ep-content">
                <div className="ep-content__body">
                    <section className="ep-section ep-section--panel ep-section--panel-noscroll">
                        <img className="ep-section__banner" id="img_true_home" src="./../dist/images/home.svg" alt={resourceProvider.read('home')} />
                    </section>
                </div>
            </section>
        );
    }
}
