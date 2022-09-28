import { constants } from './constants';

const authService = (function () {
    let context = {};
    return {
        initialize: ctx => {
            context = ctx;
        },
        getUserName: () => {
            return context.name;
        },
        getUserId: () => {
            return context.userId;
        },
        getUserImage: () => {
            return context.image;
        },
        isAuthorized: roles => {
            return roles.includes(constants.Roles.Anonymous) || context.roles.some(r => roles.indexOf(r) >= 0);
        }
    };
}());

export { authService };
