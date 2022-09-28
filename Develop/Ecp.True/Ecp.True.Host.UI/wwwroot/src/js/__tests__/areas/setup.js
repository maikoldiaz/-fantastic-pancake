import { configure } from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

const window = {
    document: {}
};

function copyProps(src, target) {
    const props = Object.getOwnPropertyNames(src)
        .filter(prop => typeof target[prop] === 'undefined')
        .reduce((result, prop) => ({
            ...result,
            [prop]: Object.getOwnPropertyDescriptor(src, prop)
        }),
        {});
    Object.defineProperties(target, props);
}

global.window = window;
global.document = window.document;
global.navigator = {
    userAgent: 'node.js'
};
copyProps(window, global);

// configure enzyme
configure({ adapter: new Adapter() });

HTMLCanvasElement.prototype.getContext = () => {
    // return whatever getContext has to return
};
