import React from 'react';
import classNames from 'classnames/bind';

export default class GridIncrementalPagination extends React.Component {
    constructor(props) {
        super(props);
        this.changePage = this.changePage.bind(this);
    }

    renderPageItems() {
        const pageItems = [];
        pageItems.push(<li key={`page${0}`} id={`page${0}`} className={classNames('ep-table__pagination-itm', 'active')} >{1}</li>);
        return pageItems;
    }

    changePage(page, isNext) {
        if (page >= 0) {
            this.props.onIncrementalPageChange(page, isNext);
        }
    }

    render() {
        const { pageSizeOptions, pageSize, totalItems, rowsText, ofText, rowsSelectorText, showPageSizeOptions } = this.props;

        const size = Number(pageSize);
        let count = totalItems ? totalItems : 0;

        if (!this.props.odata) {
            count = this.props.sortedData.length;
        }
        const pageNavClassPrevious = (this.props.enablePrevious ? 'ep-table__pagination-itm' : 'ep-table__pagination-itm disable');
        const pageNavClassNext = ('ep-table__pagination-itm');
        const pageButtons = this.renderPageItems();

        return (
            <div className="ep-table__pagination">
                <div className="ep-table__pagination-txt">{`${1} - ${count} ${ofText} ${count}`} <span className="d-inline-block text-fl-ucase">{rowsText}</span></div>
                <div className="ep-table__pagination-control">
                    <ul className="ep-table__pagination-nav">
                        <li className={pageNavClassPrevious} onClick={() => this.changePage(this.props.page, false)}><i className="fas fa-caret-left" /></li>
                        {pageButtons}
                        <li className={pageNavClassNext} onClick={() => this.changePage(this.props.page, true)}><i className="fas fa-caret-right" /></li>
                    </ul>
                    {showPageSizeOptions === true &&
                        <>
                            <select className="ep-table__pagination-dd" value={size} onChange={e => this.props.onPageSizeChange(e.target.value)}>
                                {pageSizeOptions && pageSizeOptions.map((v, k) => {
                                    return (<option key={k} value={v}>{v}</option>);
                                })}
                            </select>
                            <span className="ep-table__pagination-txt">{rowsSelectorText}</span>
                        </>
                    }
                </div>
            </div>
        );
    }
}
