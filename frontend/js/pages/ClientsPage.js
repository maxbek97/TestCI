import { ClientRow } from '../components/ClientRow.js';

export async function ClientsPage() {
    // В реальном приложении рендерим данные после fetch, пока покажем фейковые
    const clients = [
        { mid: 1, lastName: 'Иван', firstName: 'Иванов', email: 'ivan@example.com', role: 'Администратор' },
        { mid: 2, lastName: 'Петр', firstName: 'Петров', email: 'petr@example.com', role: 'Пользователь' }
    ];

    const rowsHtml = clients.map(client => ClientRow(client)).join('');

    return `
        <div class="request-wrapper-global">
            <div class="cf-header">
                <h1 class="cf-title h1">Клиенты</h1>
            </div>
        </div>
        <div class="table-container">
            <div class="table-header">
                <div class="search-box">
                    <input type="text" id="searchInput" placeholder="Поиск по таблице...">
                </div>
                <div class="page-size-selector">
                    <label for="pageSize">Показывать по:</label>
                    <select id="pageSize" name="pageSize">
                        <option value="5">5</option>
                        <option value="10" selected>10</option>
                        <option value="25">25</option>
                        <option value="50">50</option>
                    </select>
                </div>
            </div>

            <table class="data-table">
                <thead>
                    <tr>
                        <th>mid</th>
                        <th>Фамилия</th>
                        <th>Имя</th>
                        <th>Отчество / Роль</th>
                    </tr>
                </thead>
                <tbody id="tableBody">
                    ${rowsHtml}
                </tbody>
            </table>

            <div class="table-footer">
                <div class="pagination-info">
                    Показано <span>1–10</span> из <span>50</span> записей
                </div>
                <div class="pagination">
                    <button class="pagination-btn" id="prevBtn" disabled>&laquo; Назад</button>
                    <div class="pagination-pages" id="paginationPages">
                        <button class="page-num active">1</button>
                        <button class="page-num">2</button>
                        <button class="page-num">3</button>
                        <span class="dots">...</span>
                        <button class="page-num">5</button>
                    </div>
                    <button class="pagination-btn" id="nextBtn">Вперед &raquo;</button>
                </div>
            </div>
        </div>
    `;
}

export function initClientsEvents() {
    // Инициализация поиска, пагинации и динамических запросов к API
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('input', (e) => {
            console.log('Поиск:', e.target.value);
        });
    }
}