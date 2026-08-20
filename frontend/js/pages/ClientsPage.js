import { api } from '/js/api.js';
import { ClientRow } from '/js/components/ClientRow.js';
import { navigateTo } from '/js/router.js';

export function ClientsPage() {
    return `
        <div class="request-wrapper-global">
            <div class="cf-header">
                <h1 class="cf-title h1">Клиенты</h1>
            </div>
        </div>
        <div class="table-container">
            <div class="table-header">
                <div class="search-box">
                    <input type="text" id="searchInput" placeholder="Поиск по таблице (Enter для поиска)...">
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
                <div class="header-buttons">
                    <button class="action-button" id="createClientBtn">Создать клиента</button>
                </div>
            </div>

            <table class="data-table">
                <thead>
                    <tr>
                        <th>mid</th>
                        <th>Фамилия</th>
                        <th>Имя</th>
                        <th>Отчество</th>
                        <th>Код на платформе ЦР</th>
                    </tr>
                </thead>
                <tbody id="tableBody">
                    <tr><td colspan="5" style="text-align:center;">Загрузка данных...</td></tr>
                </tbody>
            </table>

            <div class="table-footer">
                <div class="pagination-info" id="paginationInfo">
                    Загрузка...
                </div>
                <div class="pagination">
                    <button class="pagination-btn" id="prevBtn" disabled>&laquo; Назад</button>
                    <div class="pagination-pages" id="paginationPages"></div>
                    <button class="pagination-btn" id="nextBtn" disabled>Вперед &raquo;</button>
                </div>
            </div>
        </div>
    `;
}

export function initClientsEvents() {
    let currentPage = 1;

    const searchInput = document.getElementById('searchInput');
    const pageSizeSelect = document.getElementById('pageSize');
    const prevBtn = document.getElementById('prevBtn');
    const nextBtn = document.getElementById('nextBtn');
    const paginationPages = document.getElementById('paginationPages');
    const tbody = document.getElementById('tableBody');
    const createBtn = document.getElementById('createClientBtn');

    if (createBtn) {
        createBtn.addEventListener('click', () => navigateTo('/clients/create'));
    }
    async function loadClients() {
        const paginationInfo = document.getElementById('paginationInfo');
        if (!tbody) return;

        tbody.innerHTML = '<tr><td colspan="5" style="text-align:center;">Загрузка...</td></tr>';

        const search = searchInput ? searchInput.value.trim() : '';
        const pageSize = pageSizeSelect ? parseInt(pageSizeSelect.value, 10) : 10;

        try {
            const queryParams = new URLSearchParams({
                Page: currentPage,
                PageSize: pageSize
            });

            if (search) {
                queryParams.append('Search', search);
            }

            const data = await api.get(`/clients?${queryParams.toString()}`);
            const clients = data.clients || [];
            const totalCount = data.totalCount || 0;

            if (clients.length === 0) {
                tbody.innerHTML = '<tr><td colspan="5" style="text-align:center;">Записи не найдены</td></tr>';
            } else {
                tbody.innerHTML = clients.map(client => ClientRow(client)).join('');
            }

            const totalPages = Math.ceil(totalCount / pageSize) || 1;
            const startRecord = totalCount === 0 ? 0 : (currentPage - 1) * pageSize + 1;
            const endRecord = Math.min(currentPage * pageSize, totalCount);

            if (paginationInfo) {
                paginationInfo.innerHTML = `Показано <span>${startRecord}–${endRecord}</span> из <span>${totalCount}</span> записей`;
            }

            updatePaginationControls(totalPages);

        } catch (err) {
            console.error('Ошибка загрузки клиентов:', err);
            tbody.innerHTML = `<tr><td colspan="5" style="text-align:center; color:red;">${err.message || 'Ошибка загрузки'}</td></tr>`;
        }
    }

    function updatePaginationControls(totalPages) {
        if (prevBtn) prevBtn.disabled = currentPage <= 1;
        if (nextBtn) nextBtn.disabled = currentPage >= totalPages;

        if (!paginationPages) return;
        paginationPages.innerHTML = '';

        for (let i = 1; i <= totalPages; i++) {
            const btn = document.createElement('button');
            btn.className = `page-num ${i === currentPage ? 'active' : ''}`;
            btn.textContent = i;
            btn.addEventListener('click', () => {
                if (currentPage !== i) {
                    currentPage = i;
                    loadClients();
                }
            });
            paginationPages.appendChild(btn);
        }
    }

    // ЕДИНСТВЕННЫЙ слушатель кликов по таблице
    if (tbody) {
        tbody.addEventListener('click', (e) => {
            const row = e.target.closest('.clickable-row');
            if (row) {
                const mid = row.dataset.mid;
                const fullName = row.dataset.fullname;
    
                if (mid) {
                    sessionStorage.setItem('selectedClientName', fullName);
                    navigateTo(`/clients/${mid}/wallets`);
                }
            }
        });
    }

    if (searchInput) {
        searchInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                currentPage = 1;
                loadClients();
            }
        });
    }

    if (pageSizeSelect) {
        pageSizeSelect.addEventListener('change', () => {
            currentPage = 1;
            loadClients();
        });
    }

    if (prevBtn) {
        prevBtn.addEventListener('click', () => {
            if (currentPage > 1) {
                currentPage--;
                loadClients();
            }
        });
    }

    if (nextBtn) {
        nextBtn.addEventListener('click', () => {
            currentPage++;
            loadClients();
        });
    }

    loadClients();
}