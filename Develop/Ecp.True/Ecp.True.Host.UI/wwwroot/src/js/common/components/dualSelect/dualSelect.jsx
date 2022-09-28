import React from 'react';
import { connect } from 'react-redux';
import { searchSource, searchTarget, move, moveBack, moveAll, moveBackAll, selectSource, selectTarget } from './../../actions';
import { resourceProvider } from '../../services/resourceProvider';
import { utilities } from '../../services/utilities';
import classNames from 'classnames/bind';

// Item Schema: { id: 1, name: 'ecp', value: 5, selected: true }
class DualSelect extends React.Component {
    render() {
        return (
            <section className="ep-dual-sel" id={`cont_${this.props.name}_select`}>
                <section className="ep-dual-sel__left">
                    {!utilities.isNullOrUndefined(this.props.sourceTitle) && <h3 className="fs-16 fw-bold fas--primary m-b-2" id={`txt_${this.props.name}_left_title`}>
                        {resourceProvider.read(this.props.sourceTitle)}</h3>}
                    <div className="ep-dual-sel__left-cont">
                        <div className="ep-dual-sel__control">
                            <label className="ep-label sr-only" htmlFor={`txt_${this.props.name}_source_search`}>{resourceProvider.read('search')}</label>
                            <input className="ep-dual-sel__input" type="text" id={`txt_${this.props.name}_source_search`}
                                value={this.props.sourceSearchText}
                                onChange={e => this.props.searchSource(e.target.value)}
                                placeholder={resourceProvider.read('browse')} />
                            {this.props.sourceSearchText &&
                                <span className="ep-dual-sel__clear" onClick={()=>this.props.searchSource('')}><i className="fas fa-times" /></span>
                            }
                        </div>
                        <ul className="ep-dual-sel__lst">
                            {this.props.sourceSearch.map(s =>
                                (<li id={`li_${this.props.name}_source_${s.id}`} key={s.id}
                                    onClick={e => this.props.selectSource(s.id, e.ctrlKey)} className={classNames('ep-dual-sel__lst-itm', { ['selected']: s.selected })}>{s.name}</li>))}
                        </ul>
                    </div>
                </section>
                <div className="ep-dual-sel__actions">
                    <button className="ep-dual-sel__btn" id={`btn_${this.props.name}_source_moveAll`} disabled={this.props.sourceSearch.length === 0} onClick={() => this.props.moveAll()}>
                        <i className="fas fa-angle-double-right" />
                        <span className="sr-only">{resourceProvider.read('sourceMoveAll')}</span>
                    </button>
                    <button className="ep-dual-sel__btn" id={`btn_${this.props.name}_source_move`}
                        disabled={this.props.sourceSearch.every(s => s.selected === false)} onClick={() => this.props.move()}>
                        <i className="fas fa-angle-right" />
                        <span className="sr-only">{resourceProvider.read('sourceMove')}</span>
                    </button>
                    <button className="ep-dual-sel__btn" id={`btn_${this.props.name}_target_move`}
                        disabled={this.props.targetSearch.every(s => s.selected === false)} onClick={() => this.props.moveBack()}>
                        <i className="fas fa-angle-left" />
                        <span className="sr-only">{resourceProvider.read('targetMove')}</span>
                    </button>
                    <button className="ep-dual-sel__btn m-b-0" id={`btn_${this.props.name}_target_moveAll`}
                        disabled={this.props.targetSearch.length === 0} onClick={() => this.props.moveBackAll()}>
                        <i className="fas fa-angle-double-left" />
                        <span className="sr-only">{resourceProvider.read('targetMoveAll')}</span>
                    </button>
                </div>
                <section className="ep-dual-sel__right">
                    {!utilities.isNullOrUndefined(this.props.targetTitle) && <h3 className="fs-16 fw-bold fas--primary m-b-2" id={`txt_${this.props.name}_right_title`}>
                        {resourceProvider.read(this.props.targetTitle)}</h3>}
                    <div className="ep-dual-sel__right-cont">
                        <div className="ep-dual-sel__control">
                            <label className="ep-label sr-only" htmlFor={`txt_${this.props.name}_target_search`}>{resourceProvider.read('search')}</label>
                            <input className="ep-dual-sel__input" type="text" id={`txt_${this.props.name}_target_search`}
                                value={this.props.targetSearchText}
                                onChange={e => this.props.searchTarget(e.target.value)}
                                placeholder={resourceProvider.read('browse')} />
                            {this.props.targetSearchText &&
                                <span className="ep-dual-sel__clear" onClick={()=>this.props.searchTarget('')}><i className="fas fa-times" /></span>
                            }
                        </div>
                        <ul className="ep-dual-sel__lst">
                            {this.props.targetSearch.map(s =>
                                (<li id={`li_${this.props.name}_target_${s.id}`} key={s.id}
                                    onClick={e => this.props.selectTarget(s.id, e.ctrlKey)} className={classNames('ep-dual-sel__lst-itm', { ['selected']: s.selected })}>{s.name}</li>))}
                        </ul>
                    </div>
                </section>
            </section>
        );
    }
}

/* istanbul ignore next */
const mapStateToProps = (state, ownProps) => {
    return {
        sourceSearch: state.dualSelect[ownProps.name] ? state.dualSelect[ownProps.name].sourceSearch : [],
        sourceSearchText: state.dualSelect[ownProps.name] ? state.dualSelect[ownProps.name].sourceText : '',
        targetSearch: state.dualSelect[ownProps.name] ? state.dualSelect[ownProps.name].targetSearch : [],
        targetSearchText: state.dualSelect[ownProps.name] ? state.dualSelect[ownProps.name].targetText : ''
    };
};

/* istanbul ignore next */
const mapDispatchToProps = (dispatch, ownProps) =>{
    return {
        selectSource: (id, ctrlKey) => {
            dispatch(selectSource(id, ctrlKey, ownProps.name));
        },
        selectTarget: (id, ctrlKey) => {
            dispatch(selectTarget(id, ctrlKey, ownProps.name));
        },
        searchSource: text => {
            dispatch(searchSource(text, ownProps.name));
        },
        searchTarget: text => {
            dispatch(searchTarget(text, ownProps.name));
        },
        move: () => {
            dispatch(move(ownProps.name));
        },
        moveAll: () => {
            dispatch(moveAll(ownProps.name));
        },
        moveBack: () => {
            dispatch(moveBack(ownProps.name));
        },
        moveBackAll: () => {
            dispatch(moveBackAll(ownProps.name));
        }
    };
};

/* istanbul ignore next */
export default connect(mapStateToProps, mapDispatchToProps, utilities.merge)(DualSelect);
