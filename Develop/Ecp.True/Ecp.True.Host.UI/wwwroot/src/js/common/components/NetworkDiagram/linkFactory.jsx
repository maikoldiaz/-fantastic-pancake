import React from 'react';
import { NetworkDiagramLinkWidget } from './linkWidget.jsx';
import { NetworkDiagramLinkModel } from './portModel.jsx';
import { DefaultLinkFactory } from '@projectstorm/react-diagrams';
import classNames from 'classnames/bind';

export class NetworkDiagramLinkFactory extends DefaultLinkFactory {
    constructor() {
        super('arrow');
    }

    generateReactWidget(event) {
        return <NetworkDiagramLinkWidget link={event.model} diagramEngine={this.engine} />;
    }

    getNewInstance() {
        return new NetworkDiagramLinkModel();
    }

    generateLinkSegment(model, selected, path) {
        return (
            <path
                className={classNames({ ['ep-diagram__path--path-unsaved']: model.options.isNew, ['ep-diagram__spath']: !model.options.isNew })}
                strokeWidth={model.options.width}
                stroke={model.options.color}
                d={path}
                fill="none"
            />
        );
    }
}
