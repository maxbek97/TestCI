import { AuthPage, initAuthEvents } from './pages/AuthPage.js';
import { RegisterPage, initRegisterEvents } from './pages/RegisterPage.js';
import { ClientsPage, initClientsEvents } from './pages/ClientsPage.js';
import { WalletsPage, initWalletsEvents } from './pages/WalletPage.js';
import { CreateClientPage, initCreateClientEvents } from './pages/CreateClientPage.js';

const routes = {
    '/': { page: AuthPage, init: initAuthEvents },
    '/auth': { page: AuthPage, init: initAuthEvents },
    '/register': { page: RegisterPage, init: initRegisterEvents },
    '/clients': { page: ClientsPage, init: initClientsEvents },
    '/clients/create': {page: CreateClientPage, init: initCreateClientEvents}
};

export async function router() {
    const path = window.location.pathname;
    let route = routes[path];
    let params = {};

    // Проверка динамического маршрута: /clients/{id}/wallets
    if (!route) {
        const walletMatch = path.match(/^\/clients\/([^\/]+)\/wallets$/);
        if (walletMatch) {
            route = { page: WalletsPage, init: initWalletsEvents };
            params = { clientId: walletMatch[1] };
        }
    }

    if (!route) {
        route = routes['/auth'];
    }

    const appRoot = document.getElementById('app-root');
    appRoot.innerHTML = await route.page(params);

    if (route.init) {
        route.init(params);
    }
}

export function navigateTo(url) {
    window.history.pushState(null, null, url);
    router();
}

window.addEventListener('popstate', router);