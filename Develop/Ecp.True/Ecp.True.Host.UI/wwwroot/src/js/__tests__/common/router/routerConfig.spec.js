import RouterConfig from '../../../common/router/routerConfig';
it('router config returns routes', () =>{
    var routerConfig = new RouterConfig('', { routes: [{ name: 'someName' }, { name: 'someName1' }] });
    var routes = routerConfig.getRoutes();
    expect(routes).toHaveLength(2);
});

it('router whether route exists', () =>{
    var routerConfig = new RouterConfig('someKey', { routes: [{ name: 'someName' }, { name: 'someName1' }], noRoute: false });
    var isNoRoute = routerConfig.isNoRoute();
    expect(isNoRoute).toBe(false);
});

it('get page title returns page title whether route exists', () =>{
    var routerConfig = new RouterConfig('someKey', { routes: [{ name: 'someName', routeKey: 'someKey' }, { name: 'someName1', routeKey: 'someKey1' }], noRoute: false });
    var pageTitle = routerConfig.getPageTitle();
    expect(pageTitle).toBe('someKey');
});

it('get page names returns all page names', () =>{
    var routerConfig = new RouterConfig('someKey', { routes: [{ name: 'someName' }, { name: 'someName1' }], noRoute: false });
    var pageNames = routerConfig.getPageNames();
    expect(pageNames).toHaveLength(2);
});

it('get page details returns page details', () =>{
    var routerConfig = new RouterConfig('someKey', { routes: [{ name: 'someName', routeKey: 'someKey' }, { name: 'someName1', routeKey: 'someKey1' }], noRoute: false });
    var pageDetails = routerConfig.getPageDetails('somekey');
    expect(pageDetails).toBeNull();
});

it('build navigation path returns navigation path', () =>{
    var routerConfig = new RouterConfig('someKey', { routes: [{ name: 'someName', routeKey: 'someKey' }, { name: 'someName1', routeKey: 'someKey1' }], noRoute: false });
    var navigationPath = routerConfig.buildNavPath('somekey');
    expect(navigationPath).toHaveLength(0);
});

it('get page roles returns navigation path', () =>{
    var routerConfig = new RouterConfig('someKey', { routes: [{ name: 'someName', routeKey: 'someKey' }, { name: 'someName1', routeKey: 'someKey1' }], noRoute: false });
    var pageRoles = routerConfig.getPageRoles('somekey');
    expect(pageRoles).toHaveLength(1);
});