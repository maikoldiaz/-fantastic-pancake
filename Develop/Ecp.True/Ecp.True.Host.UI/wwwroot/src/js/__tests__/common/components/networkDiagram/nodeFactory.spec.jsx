import NetworkDiagramServiceProvider from '../../../../common/components/networkDiagram/NetworkDiagramServiceProvider';
it('widget renders without crashing', () =>{
    const event = {
        model: {}
    };
    var provider = new NetworkDiagramServiceProvider();
    const factory = provider.getNodeFactory();
    const widget = factory.generateReactWidget(event);
    expect(widget).toBeDefined();
});
