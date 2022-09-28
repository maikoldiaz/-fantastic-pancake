import { NodeModel } from '@projectstorm/react-diagrams';
import { NetworkDiagramPortModel } from './portModel.jsx';

export class NetworkDiagramNodeModel extends NodeModel {
    constructor(options = {}) {
        super({
            ...options,
            type: 'js-custom-node'
        });
        this.color = options.color || '#3E2F36';
        this.id = options.id;


        this.addNodePorts();
    }

    addNodePorts() {
        this.addPort(
            new NetworkDiagramPortModel({ in: true, name: 'in', id: `in_${this.id}`, color: this.color })
        );
        this.addPort(
            new NetworkDiagramPortModel({ in: false, name: 'out', id: `out_${this.id}`, color: this.color })
        );
    }

    serialize() {
        return {
            ...super.serialize(),
            color: this.options.color
        };
    }

    deserialize(ob, engine) {
        super.deserialize(ob, engine);
        this.color = ob.color;
    }
}

