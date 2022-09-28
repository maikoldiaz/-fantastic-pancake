import * as React from 'react';
import { DefaultLinkWidget } from '@projectstorm/react-diagrams';
import { LinkWidget } from '@projectstorm/react-diagrams-core';
import { constants } from '../../services/constants';
import { networkBuilderService } from '../../services/networkBuilderService.js';
import EnableConfigurationButton from './enableConfigurationButton.jsx';
import DisableConfigurationButton from './disableConfigurationButton.jsx';
import TransferPointConfigurationButton from './transferPointConfigurationButton.jsx';
import DeleteConfigurationButton from './deleteConfigurationButton.jsx';
import { svgPaths } from '../../services/svgPathService';

const LinkArrowWidget = props => {
    const { point, previousPoint } = props;

    const angle = 90 +
        (Math.atan2(
            point.getPosition().y - previousPoint.getPosition().y,
            point.getPosition().x - previousPoint.getPosition().x
        ) * 180) / Math.PI;

    const translateX = point.getPosition().x;
    const translateY = point.getPosition().y;

    return (
        <g className="arrow" transform={'translate(' + translateX + ', ' + translateY + ')'}>
            <g style={{ transform: 'rotate(' + angle + 'deg)' }}>
                <g>
                    <path d="M4.414,1.274a1,1,0,0,1,1.672,0l3.4,5.177A1,1,0,0,1,8.648,8h-6.8a1,1,0,0,1-.836-1.549Z"
                        transform="translate(-5 10)" fill={props.color} />
                    <path d="M4.414,1.274a1,1,0,0,1,1.672,0l3.4,5.177A1,1,0,0,1,8.648,8h-6.8a1,1,0,0,1-.836-1.549Z"
                        transform="translate(-5 14)" fill={props.color} />
                </g>
            </g>
        </g>
    );
};

class LinkHoverWidget extends React.Component {
    constructor(props) {
        super(props);
        this.showSettingsOptions = false;
    }
    onSettingsClick() {
        this.showSettingsOptions = !this.showSettingsOptions;
        networkBuilderService.repaint();
    }

    onMouseLeave(e) {
        this.props.onMouseLeave(e);
    }

    render() {
        const { point, previousPoint, position, isHover, linkId, options, isSelfConnection } = this.props;
        let settingsPosX = {};
        let settingsPosY = {};

        const angle =
            90 +
            (Math.atan2(
                point.getPosition().y - previousPoint.getPosition().y,
                point.getPosition().x - previousPoint.getPosition().x
            ) *
                180) /
            Math.PI;

        const svg = document.querySelector('.ep-diagram__canvas svg');
        if (svg) {
            const pt = svg.createSVGPoint();

            pt.x = position ? position.x : 0;
            pt.y = position ? position.y : 0;
            const svgP = pt.matrixTransform(svg.getScreenCTM().inverse());
            settingsPosX = svgP.x;
            settingsPosY = svgP.y;
        }
        const translateX = point.getPosition().x;
        const translateY = isSelfConnection ? (point.getPosition().y + 50) : point.getPosition().y;
        const translateXPrev = previousPoint.getPosition().x;
        const translateYPrev = isSelfConnection ? (previousPoint.getPosition().y + 50) : previousPoint.getPosition().y;

        return (
            <>
                {options.state !== constants.NodeConnectionState.Inactive &&
                    <g className="ep-diagram__path-arrow" transform={'translate(' + translateX + ', ' + translateY + ')'}>
                        <g style={{ transform: 'rotate(' + angle + 'deg)' }}>
                            <g>
                                <path d="M4.414,1.274a1,1,0,0,1,1.672,0l3.4,5.177A1,1,0,0,1,8.648,8h-6.8a1,1,0,0,1-.836-1.549Z"
                                    transform={'translate(-5 , 34)'} fill={this.props.colorSelected} />
                                <path d="M4.414,1.274a1,1,0,0,1,1.672,0l3.4,5.177A1,1,0,0,1,8.648,8h-6.8a1,1,0,0,1-.836-1.549Z"
                                    transform={'translate(-5 , 30)'} fill={this.props.colorSelected} />
                            </g>
                        </g>
                    </g>
                }
                {(isHover || this.showSettingsOptions) &&
                    <g onMouseLeave={e => this.onMouseLeave(e)} onClick={() => this.onSettingsClick()} className="ep-diagram__path-settings"
                        transform={'translate(' + settingsPosX + ', ' + settingsPosY + ')'}>
                        <g id={`btnSettings_${linkId}`} className="ep-diagram__btn-settings">
                            <g fill="#fff">
                                <path d={svgPaths.settingButton.outerCircleOutline} stroke="none" />
                                <path d={svgPaths.settingButton.outerCircle} stroke="none" fill="#1592e6" />
                            </g>
                            <g transform="translate(-35.605 -30.907)">
                                <path d="M13.331,15.125l1.838-1.85,3.681,3.769.325.481-.131.494-.362.531-.425.381-.437.231h-.637l-1.031-.975Z" transform="translate(35.605 30.907)" fill="#1592e6" />
                                <path d={svgPaths.settingButton.iconToolRance} fill="#1592e6" />
                                <path d={svgPaths.settingButton.iconToolRanceOutline} fill="#fff" stroke="#fff" strokeLinecap="round" strokeLinejoin="round" strokeWidth="0.2" />
                                <path d={svgPaths.settingButton.iconToolDriver} transform="translate(-31.031 -31.236)" fill="#1592e6" />
                                <path d={svgPaths.settingButton.iconToolDriverOutline} transform="translate(-2.358 -2.721)" fill="#1592e6" />
                                <path d="M89.781,89.311a.175.175,0,0,1-.122-.051l-3-3a.175.175,0,1,1,.248-.246l3,3a.174.174,0,0,1-.124.3Z" transform="translate(-36.725 -40.069)" fill="#fff" />
                                <path d="M93.862,85.69a.175.175,0,0,1-.124-.051l-3-3a.175.175,0,0,1,.246-.246l3,3a.175.175,0,0,1-.124.3Z" transform="translate(-40.1 -37.086)" fill="#fff" />
                            </g>
                        </g>
                        {this.showSettingsOptions &&
                            <>
                                <EnableConfigurationButton linkId={linkId} />
                                <DisableConfigurationButton linkId={linkId} />
                                <TransferPointConfigurationButton linkId={linkId} />
                                <DeleteConfigurationButton linkId={linkId} />
                            </>
                        }
                    </g>}
                {options.state !== constants.NodeConnectionState.Inactive &&
                    <g className="ep-diagram__path-arrow" transform={'translate(' + translateXPrev + ', ' + translateYPrev + ')'}>
                        <g style={{ transform: 'rotate(' + angle + 'deg)' }}>
                            <g>
                                <path d="M4.414,1.274a1,1,0,0,1,1.672,0l3.4,5.177A1,1,0,0,1,8.648,8h-6.8a1,1,0,0,1-.836-1.549Z"
                                    transform={'translate(-5 , -34)'} fill={this.props.colorSelected} />
                                <path d="M4.414,1.274a1,1,0,0,1,1.672,0l3.4,5.177A1,1,0,0,1,8.648,8h-6.8a1,1,0,0,1-.836-1.549Z"
                                    transform={'translate(-5 , -30)'} fill={this.props.colorSelected} />
                            </g>
                        </g>
                    </g>
                }
            </>
        );
    }
}

export class NetworkDiagramLinkWidget extends DefaultLinkWidget {
    constructor(props) {
        super(props);
        this.hoverPos = null;
        this.state = {
            isLinkClicked: false,
            isLinkHover: false
        };
    }

    onLinkClick() {
        this.setState(state => ({
            isLinkClicked: !state.isLinkClicked
        }));
    }

    onLinkEnter(e) {
        if (e.target.classList.contains('ep-diagram__spath')) {
            this.setState(() => ({
                isLinkHover: true
            }));
            this.hoverPos = { x: e.clientX, y: e.clientY };
        }
    }

    onLinkLeave() {
        this.setState(() => ({
            isLinkHover: false
        }));
    }

    generateHoverPath(point, previousPoint, options) {
        return (
            <LinkHoverWidget
                key={point.getID()}
                point={point}
                previousPoint={previousPoint}
                colorSelected={this.props.link.getOptions().selectedColor}
                color={this.props.link.getOptions().color}
                options={this.props.link.options}
                onMouseLeave={e => this.onLinkLeave(e)}
                position={options.hoverPos}
                isSelected={options.isSelected}
                isHover={options.isHover}
                linkId={options.linkId}
                isSelfConnection={options.isSelfConnection}
            />
        );
    }

    generateArrow(point, previousPoint) {
        return (
            <LinkArrowWidget
                key={point.getID()}
                point={point}
                previousPoint={previousPoint}
                colorSelected={this.props.link.getOptions().selectedColor}
                color={this.props.link.getOptions().color}
            />
        );
    }

    generateCurvePath(firstPoint, lastPoint) {
        return `M${firstPoint.getX()},${firstPoint.getY()} C ${firstPoint.getX() + 90},${firstPoint.getY() + 80} ${lastPoint.getX() - 90},${lastPoint.getY() + 80}
            ${lastPoint.getX()},${lastPoint.getY()}`;
    }

    getLinkTypeClass(isLinkSelected) {
        const state = this.props.link.options.state;
        const styleClass = isLinkSelected ? 'ep-diagram__path--selected' : '';
        switch (state) {
        case constants.NodeConnectionState.Active:
            return 'ep-diagram__path--active ' + styleClass;
        case constants.NodeConnectionState.Inactive:
            return 'ep-diagram__path--inactive ' + styleClass;
        case constants.NodeConnectionState.TransferPoint:
            return 'ep-diagram__path--transfer ' + styleClass;
        case constants.NodeConnectionState.Unsaved:
            return 'ep-diagram__path--unsaved ' + styleClass;
        case constants.NodeConnectionState.Invalid:
            return 'ep-diagram__path--invalid ' + styleClass;
        default:
            return 'ep-diagram__path--active ' + styleClass;
        }
    }

    render() {
        // ensure id is present for all points on the path
        const points = this.props.link.getPoints();
        const paths = [];
        this.refPaths = [];
        let linkId = this.props.link.getID();

        const isSelfConnection = networkBuilderService.isSelfConnection(this.props.link);

        // draw the multiple anchors and complex line instead
        if (isSelfConnection) {
            for (let j = 0; j < points.length - 1; j++) {
                paths.push(
                    this.generateLink(
                        this.generateCurvePath(points[j], points[j + 1])
                    )
                );
            }
        } else {
            for (let j = 0; j < points.length - 1; j++) {
                paths.push(
                    this.generateLink(
                        LinkWidget.generateLinePath(points[j], points[j + 1]), { 'data-linkid': linkId, 'data-point': j }, j
                    )
                );
                if (this.props.link.options.state === constants.NodeConnectionState.TransferPoint) {
                    paths.push(
                        this.generateLink(
                            LinkWidget.generateLinePath(points[j], points[j + 1])
                        )
                    );
                }
            }
        }

        if (this.props.link.options.state === constants.NodeConnectionState.Inactive) {
            paths.push(this.generateArrow(points[points.length - 1], points[points.length - 2]));
        }

        const sourceTargetExists = this.props.link.getSourcePort() && this.props.link.getTargetPort();
        if (sourceTargetExists) {
            linkId = this.props.link.getSourcePort().getID() + '-' + this.props.link.getTargetPort().getID();
        }

        if (this.state.isLinkClicked || this.props.link.options.isSelected) {
            this.props.link.options.color = '#1592E6';
        } else {
            this.props.link.options.color = '#3E2F36';
        }

        if (this.state.isLinkHover || this.state.isLinkClicked || this.props.link.options.isSelected) {
            paths.push(this.generateHoverPath(points[points.length - 1],
                points[points.length - 2], {
                    hoverPos: this.hoverPos,
                    isSelected: this.state.isLinkClicked || this.props.link.options.isSelected,
                    isHover: this.state.isLinkHover,
                    linkId,
                    isSelfConnection
                }
            ));
        }

        return (<g onClick={e => this.onLinkClick(e)} onMouseLeave={e => this.onLinkLeave(e)}
            onMouseEnter={e => this.onLinkEnter(e)} id={linkId}
            className={`ep-diagram__path ${this.getLinkTypeClass(this.state.isLinkClicked)}`}>{paths}</g>);
    }
}
