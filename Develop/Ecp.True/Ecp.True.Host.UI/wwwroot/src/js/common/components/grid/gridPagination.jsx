import React from 'react';
import classNames from 'classnames/bind';

export default class GridPagination extends React.Component {
    constructor(props) {
        super(props);
        this.changePage = this.changePage.bind(this);
    }

    renderPageItems(pages, page, pageButtonCount) {
        const maxButtons = pageButtonCount;
        const pageItems = [];

        const st = maxButtons * Math.floor(page / maxButtons);
        const ed = st + maxButtons > pages ? st + (pages % maxButtons) : st + maxButtons;

        if (st >= maxButtons) {
            pageItems.push(<li key={`page${st - 1}`} id={`page${st - 1}`} className={'ep-table__pagination-itm'} onClick={() => this.changePage(st - 1)}>{'...'}</li>);
        }

        for (let i = st; i < ed; i++) {
            pageItems.push(<li key={`page${i}`} id={`page${i}`} className={classNames('ep-table__pagination-itm', { ['active']: (page === i) })} onClick={() => this.changePage(i)}>{i + 1}</li>);
        }

        if (ed < pages) {
            pageItems.push(<li key={`page${ed}`} id={`page${ed}`} className={'ep-table__pagination-itm'} onClick={() => this.changePage(ed)}>{'...'}</li>);
        }
        return pageItems;
    }

    changePage(page) {
        const pages = Math.ceil(this.props.totalItems / this.props.pageSize);
        if (page >= 0 && page < pages && page !== this.props.page) {
            this.props.onPageChange(page);
        }
    }

    render() {
        const { pageSizeOptions, pageSize, totalItems, page, rowsText, ofText, rowsSelectorText, showPageSizeOptions, pageButtonCount } = this.props;

        const size = Number(pageSize);
        let count = totalItems ? totalItems : 0;

        if (!this.props.odata) {
            count = this.props.sortedData.length;
        }

        const pages = Math.ceil(count / size);

        const activePage = count > 0 ? page : -1;

        const pageStart = activePage >= 0 ? (activePage * size) + 1 : 0;
        const pageEnd = pageStart + size - 1 < count ? pageStart + size - 1 : count;
        const pageNavClass = (pages === 1 ? 'ep-table__pagination-itm disable' : 'ep-table__pagination-itm');
        const pageButtons = this.renderPageItems(pages, this.props.page, pageButtonCount);

        if (pages === 0) {
            return null;
        }

        return (
            <div className="ep-table__pagination">
                <div className="ep-table__pagination-txt">{`${pageStart} - ${pageEnd} ${ofText} ${count}`} <span className="d-inline-block text-fl-ucase">{rowsText}</span></div>
                <div className="ep-table__pagination-control">
                    <ul className="ep-table__pagination-nav">
                        <li className={pageNavClass} onClick={() => this.changePage(0, pages)}><i className="fas fa-step-backward" /></li>
                        <li className={pageNavClass} onClick={() => this.changePage(this.props.page - 1)}><i className="fas fa-caret-left" /></li>
                        {pageButtons}
                        <li className={pageNavClass} onClick={() => this.changePage(this.props.page + 1)}><i className="fas fa-caret-right" /></li>
                        <li className={pageNavClass} onClick={() => this.changePage(pages - 1)}><i className="fas fa-step-forward" /></li>
                    </ul>
                    {showPageSizeOptions === true &&
                        <>
                            <select className="ep-table__pagination-dd" id="dd_select_pagination" value={size} onChange={e => this.props.onPageSizeChange(e.target.value)}>
                                {pageSizeOptions && pageSizeOptions.map((v, k) => {
                                    return (<option key={k} value={v}>{v}</option>);
                                })}
                            </select>
                            <label className="ep-table__pagination-txt" htmlFor="dd_select_pagination">{rowsSelectorText}</label>
                        </>
                    }
                </div>
            </div>
        );
    }
}
