import { AuthPage, initAuthEvents } from './pages/AuthPage.js';
import { RegisterPage, initRegisterEvents } from './pages/RegisterPage.js';
import { ClientsPage, initClientsEvents } from './pages/ClientsPage.js';

const routes = {
    '/': { page: AuthPage, init: initAuthEvents },
    '/auth': { page: AuthPage, init: initAuthEvents },
    '/register': { page: RegisterPage, init: initRegisterEvents },
    '/clients': { page: ClientsPage, init: initClientsEvents }
};

export async function router() {
    const path = window.location.pathname;
    const route = routes[path] || routes['/auth'];

    const appRoot = document.getElementById('app-root');
    appRoot.innerHTML = await route.page();

    // Инициализируем событие формы/кнопок ПОСЛЕ добавления элементов в DOM
    if (route.init) {
        route.init();
    }
}

export function navigateTo(url) {
    window.history.pushState(null, null, url);
    router();
}

window.addEventListener('popstate', router);