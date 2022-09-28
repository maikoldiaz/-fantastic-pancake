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
    const factory = provider.getLinkFactory();
    const networkLinkWidget = factory.generateReactWidget(event);
    const linkSegment = factory.generateLinkSegment(event.model, false, 'somePath');
    expect(networkLinkWidget).toBeDefined();
    expect(linkSegment).toBeDefined();
});
