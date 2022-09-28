import React from 'react';
import NetworkDiagramNodeWidget from './nodeWidget.jsx';
import { AbstractReactFactory } from '@projectstorm/react-canvas-core';

export class NetworkDiagramNodeFactory extends AbstractReactFactory {
    constructor() {
        super('js-custom-node');
    }

    generateReactWidget(event) {
        return <NetworkDiagramNodeWidget engine={this.engine} node={event.model} />;
    }
}
