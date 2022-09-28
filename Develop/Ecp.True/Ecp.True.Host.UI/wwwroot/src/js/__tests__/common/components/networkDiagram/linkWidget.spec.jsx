import { NetworkDiagramLinkWidget, LinkHoverWidget } from '../../../../common/components/networkDiagram/linkWidget';
import { NetworkDiagramPortModel } from '../../../../common/components/networkDiagram/portModel';
describe('linkWidget', () => {
    it('should be created successfully', () => {
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
        const linkWidget = new NetworkDiagramLinkWidget(event.model.options);
        expect(linkWidget).toBeDefined();
    });

    it('generateHoverPath should set link hover path', () => {
        var portModel = new NetworkDiagramPortModel({ in: true, name: 'in', id: 'someId', color: 'blue' });
        const linkModel = portModel.createLinkModel();
        var points = linkModel.getPoints();
        const event = {
            model: {
                options:{
                    isNew: true,
                    width: 24,
                    color: 'blue',
                    type: 'arrow',
                    link: linkModel
                }
            }
        };
        const linkWidget = new NetworkDiagramLinkWidget(event.model.options);
        var hoverPath = linkWidget.generateHoverPath(points[0], {}, event.model.options);
        expect(linkWidget).toBeDefined();
        expect(hoverPath).toBeDefined();
    });

    it('generateArrow should generate the arrow', () => {
        var portModel = new NetworkDiagramPortModel({ in: true, name: 'in', id: 'someId', color: 'blue' });
        const linkModel = portModel.createLinkModel();
        var points = linkModel.getPoints();
        const event = {
            model: {
                options:{
                    isNew: true,
                    width: 24,
                    color: 'blue',
                    type: 'arrow',
                    link: linkModel
                }
            }
        };
        const linkWidget = new NetworkDiagramLinkWidget(event.model.options);
        var arrow = linkWidget.generateArrow(points[0], {});
        expect(linkWidget).toBeDefined();
        expect(arrow).toBeDefined();
    });

    it('getLinkTypeClass should get the class', () => {
        var portModel = new NetworkDiagramPortModel({ in: true, name: 'in', id: 'someId', color: 'blue' });
        const linkModel = portModel.createLinkModel();
        const event = {
            model: {
                options:{
                    isNew: true,
                    width: 24,
                    color: 'blue',
                    type: 'arrow',
                    link: linkModel
                }
            }
        };
        const linkWidget = new NetworkDiagramLinkWidget(event.model.options);
        var linkTypeClass = linkWidget.getLinkTypeClass(true);
        expect(linkWidget).toBeDefined();
        expect(linkTypeClass).toEqual("ep-diagram__path--active ep-diagram__path--selected");
    });

    it('generateCurvePath should generate the curve path', () => {
        var portModel = new NetworkDiagramPortModel({ in: true, name: 'in', id: 'someId', color: 'blue' });
        const linkModel = portModel.createLinkModel();
        var points = linkModel.getPoints();
        const event = {
            model: {
                options:{
                    isNew: true,
                    width: 24,
                    color: 'blue',
                    type: 'arrow',
                    link: linkModel
                }
            }
        };
        const linkWidget = new NetworkDiagramLinkWidget(event.model.options);
        var curvePath = linkWidget.generateCurvePath(points[0], points[1]);
        expect(curvePath).toBeDefined();
    });
});
