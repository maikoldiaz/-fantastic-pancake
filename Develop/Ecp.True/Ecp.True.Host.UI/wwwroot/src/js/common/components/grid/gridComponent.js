import React from 'react';
import { configureGrid } from './actions';
import { utilities } from '../../services/utilities.js';
import { dispatcher } from '../../store/dispatcher.js';

// A very simple HOC to configure grid...
export function dataGrid(WrappedComponent, ...gridConfig) {
    class DataGrid extends React.Component {
    	constructor(props) {
            super(props);

            this.isReady = false;
            this.getConfig = this.getConfig.bind(this);
    	}

        getConfig(config) {
            return utilities.isFunction(config) ? config(this.props) : config;
        }

        render() {
            if (!this.isReady) {
	    		gridConfig.forEach(c => dispatcher.dispatch(configureGrid(this.getConfig(c))));
		    	this.isReady = true;
            }

            const nameProps = {};
            if (gridConfig.length === 1) {
                nameProps.name = this.getConfig(gridConfig[0]).name;
                nameProps.idField = this.getConfig(gridConfig[0]).idField;
            }

		    return (
			    <WrappedComponent {...nameProps} {...this.props}/>
		    );
        }
    }

    return DataGrid;
}

