import { NetworkDiagramPortModel } from '../../../../common/components/networkDiagram/portModel';
it('model renders without crashing', () =>{
    var portModel = new NetworkDiagramPortModel({ in: true, name: 'in', id: 'someId', color: 'blue' });
    const linkModel = portModel.createLinkModel();
    expect(linkModel).toBeDefined();
    expect(linkModel.options.color).toMatch('blue');
    expect(linkModel.options.type).toMatch('arrow');
});
