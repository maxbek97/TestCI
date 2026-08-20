import { Header } from './components/Header.js';
import { router, navigateTo } from './router.js';

// Делаем навигацию доступной глобально
window.navigateTo = navigateTo;

document.addEventListener('DOMContentLoaded', () => {
    // Вставляем шапку
    const headerRoot = document.getElementById('header-root');
    headerRoot.innerHTML = Header();

    // Перехватываем стандартные ссылки `<a href="/register" data-link>`
    document.body.addEventListener('click', (e) => {
        const link = e.target.closest('[data-link]');
        if (link) {
            e.preventDefault();
            navigateTo(link.getAttribute('href'));
        }
    });

    // Запускаем роутер для первоначальной отрисовки
    router();
});