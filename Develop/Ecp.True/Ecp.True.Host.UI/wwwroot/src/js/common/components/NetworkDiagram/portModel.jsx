
import { DefaultPortModel, DefaultLinkModel } from '@projectstorm/react-diagrams';

export class NetworkDiagramLinkModel extends DefaultLinkModel {
    constructor(options) {
        super({
            type: options.type,
            width: 2,
            color: options.color,
            locked: true
        });
    }
}

export class NetworkDiagramPortModel extends DefaultPortModel {
    constructor(options = {}) {
        super({
            in: options.in,
            name: options.name,
            id: options.id,
            type: 'arrow',
            color: options.color,
            state: options.state
        });
    }
    createLinkModel() {
        return new NetworkDiagramLinkModel(this.options);
    }
}
