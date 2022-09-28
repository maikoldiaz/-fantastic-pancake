import { NetworkDiagramNodeFactory } from './nodeFactory.jsx';
import { NetworkDiagramLinkFactory } from './linkFactory.jsx';
import { NetworkDiagramNodeModel } from './nodeModel.jsx';

export default class NetworkDiagramServiceProvider {
    getNodeFactory() {
        return new NetworkDiagramNodeFactory();
    }

    getLinkFactory() {
        return new NetworkDiagramLinkFactory();
    }

    getNodeModel(options) {
        return new NetworkDiagramNodeModel(options);
    }

    getNewLink(engine) {
        return engine.getStateMachine().currentState.dragNewLink.link;
    }

    registerNodeFactory(engine) {
        engine.getNodeFactories().registerFactory(this.getNodeFactory());
    }
    registerLinkFactory(engine) {
        engine.getLinkFactories().registerFactory(this.getLinkFactory());
    }
}
