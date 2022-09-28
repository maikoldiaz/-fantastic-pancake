import React from 'react';
import classNames from 'classnames/bind';
import { connect } from 'react-redux';

export class Wrapper extends React.Component {
    render() {
        const {
            children,
            isLastOne,
            onAddItem,
            onRemoveItem
        } = this.props;

        return (<>
            <section className={`ep-card ep-card__pointerEvent ep-card__pointerEvent--${isLastOne ? 'off' : 'on'}`}>
                <div className="ep-card__content__full" >
                    <section className="text-right">
                        {onAddItem &&
                            <button type="button"
                                className="ep-btn ep-btn--link ep-btn--pointerEventOn"
                                onClick={() => onAddItem()}>
                                <i className={classNames('fas m-r-1', 'fas fa-plus-circle')} />
                            </button>
                        }
                        {onRemoveItem &&
                            <button type="button"
                                className="ep-btn ep-btn--link"
                                onClick={() => onRemoveItem()}>
                                <i className={classNames('fas m-r-1', 'fas fa-trash')} />
                            </button>
                        }
                    </section>
                    <div className={classNames('row', isLastOne && 'ep-card__content__lock')} >
                        {children}
                    </div>
                </div>
            </section>
        </>);
    }
}

/* istanbul ignore next */
const mapStateTopProps = (state, ownProps) => {
    return {
        isLastOne: ownProps.isLastOne,
        onAddItem: ownProps.onAddItem,
        onRemoveItem: ownProps.onRemoveItem
    };
};

export default connect(mapStateTopProps)(Wrapper);
