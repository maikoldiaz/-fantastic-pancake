import NetworkDiagramServiceProvider from '../../../../common/components/networkDiagram/NetworkDiagramServiceProvider';
it('model renders without crashing', () =>{
    const event = {
        model: {
            options:{
                isNew: true,
                width: 24,
                color: 'blue',
                type: 'arrow'
            }
        }
    };
    const provider = new NetworkDiagramServiceProvider();
    const nodeModel = provider.getNodeModel(event.model.options);
    nodeModel.addNodePorts();
    const serializedModel = nodeModel.serialize();
    expect(nodeModel).toBeDefined();
    expect(serializedModel).toBeDefined();
    expect(serializedModel.color).toBe('blue');
    expect(serializedModel.ports).toHaveLength(2);
});
