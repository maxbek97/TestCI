import { api } from '/js/api.js';
import { WalletRow } from '/js/components/WalletRow.js';
import { navigateTo } from '/js/router.js';

const STATUS_OPTIONS = ['Actv', 'Clsd', 'Blck', 'Prcs'];

function generateUUID() {
    return '10000000-1000-4000-8000-100000000000'.replace(/[018]/g, c =>
        (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
    );
}

// Извлечение clientId из параметров роутера или из текущего URL пути
function getClientId(params) {
    if (params?.clientId) return params.clientId;
    if (params?.id) return params.id;

    // Резервный разбор URL: /clients/{clientId}/wallets
    const match = window.location.pathname.match(/\/clients\/([^\/]+)\/wallets/);
    return match ? match[1] : null;
}

export function WalletsPage() {
    const clientName = sessionStorage.getItem('selectedClientName');
    const titleText = clientName ? `Кошелек (${clientName})` : 'Кошелек клиента';

    return `
        <div class="request-wrapper-global">
            <div class="cf-header">
                <h1 class="cf-title h1">${titleText}</h1>
                <div class="header-buttons">
                    <button class="action-button" id="backToClientsBtn">&laquo; К клиентам</button>
                    <button class="action-button" id="addWalletBtn">+ Создать кошелек</button>
                </div>
            </div>
        </div>

        <div class="table-container">
            <table class="data-table">
                <thead>
                    <tr>
                        <th>ID кошелька</th>
                        <th>Статус</th>
                        <th>Номер счета</th>
                    </tr>
                </thead>
                <tbody id="walletsTableBody">
                    <tr><td colspan="3" style="text-align:center;">Загрузка...</td></tr>
                </tbody>
            </table>
        </div>
    `;
}

export function initWalletsEvents(params) {
    const clientId = getClientId(params);
    const backBtn = document.getElementById('backToClientsBtn');
    const addWalletBtn = document.getElementById('addWalletBtn');
    const tbody = document.getElementById('walletsTableBody');

    if (backBtn) {
        backBtn.addEventListener('click', () => navigateTo('/clients'));
    }

    if (addWalletBtn && clientId) {
        addWalletBtn.addEventListener('click', () => {
            renderNewWalletRow(clientId);
        });
    }

    if (clientId) {
        loadWallets(clientId);
    }

    if (tbody) {
        tbody.addEventListener('click', (e) => {
            const statusCell = e.target.closest('.status-cell');
            if (statusCell && !statusCell.querySelector('select')) {
                // ПЕРЕДАЕМ clientId в обработчик
                activateStatusSelect(statusCell, clientId);
            }
        });
    }
}

function renderNewWalletRow(clientId) {
    const tbody = document.getElementById('walletsTableBody');
    if (!tbody) return;

    if (tbody.querySelector('td[colspan="3"]')) {
        tbody.innerHTML = '';
    }

    if (document.getElementById('new-wallet-row')) return;

    const newWalletId = generateUUID();
    const tr = document.createElement('tr');
    tr.id = 'new-wallet-row';

    tr.innerHTML = `
        <td>${newWalletId}</td>
        <td>
            <select id="newWalletStatus" class="status-select">
                ${STATUS_OPTIONS.map(opt => `<option value="${opt}">${opt}</option>`).join('')}
            </select>
        </td>
        <td>
            <button class="action-button" id="confirmCreateWalletBtn">Подтвердить</button>
            <button class="action-button secondary" id="cancelCreateWalletBtn" style="margin-left: 5px;">Отмена</button>
        </td>
    `;

    tbody.prepend(tr);

    const confirmBtn = tr.querySelector('#confirmCreateWalletBtn');
    const cancelBtn = tr.querySelector('#cancelCreateWalletBtn');

    confirmBtn.addEventListener('click', async () => {
        const selectedStatus = tr.querySelector('#newWalletStatus').value;

        const payload = {
            ClientId: clientId,
            Id_DRw: newWalletId,
            Status: selectedStatus
        };

        try {
            confirmBtn.disabled = true;
            confirmBtn.textContent = 'Сохранение...';

            await api.post('/wallets/create', payload);
            await loadWallets(clientId);
        } catch (err) {
            console.error('Ошибка создания кошелька:', err);
            alert(`Ошибка: ${err.message || 'Не удалось создать кошелек'}`);
            confirmBtn.disabled = false;
            confirmBtn.textContent = 'Подтвердить';
        }
    });

    cancelBtn.addEventListener('click', () => {
        tr.remove();
        if (tbody.children.length === 0) {
            tbody.innerHTML = '<tr><td colspan="3" style="text-align:center;">Кошельки не найдены</td></tr>';
        }
    });
}

async function loadWallets(clientId) {
    const tbody = document.getElementById('walletsTableBody');
    if (!tbody) return;

    try {
        const response = await api.get(`/clients/${clientId}/wallets`);
        const wallets = response.wallets || [];

        if (wallets.length === 0) {
            tbody.innerHTML = '<tr><td colspan="3" style="text-align:center;">Кошельки не найдены</td></tr>';
            return;
        }

        tbody.innerHTML = wallets.map(wallet => WalletRow(wallet)).join('');
    } catch (err) {
        console.error('Ошибка при запросе кошельков:', err);
        tbody.innerHTML = `<tr><td colspan="3" style="text-align:center; color:red;">${err.message || 'Ошибка загрузки кошельков'}</td></tr>`;
    }
}

function activateStatusSelect(cell, clientId) {
    const originalText = cell.querySelector('.status-text')?.textContent.trim() || '';
    const row = cell.closest('tr');
    const walletId = row.dataset.walletId || row.dataset.id_drw || row.dataset.id;

    cell.innerHTML = `
        <span>Заменить на: </span>
        <select class="status-select">
            <option value="" disabled selected>Выберите...</option>
            ${STATUS_OPTIONS.map(opt => `<option value="${opt}">${opt}</option>`).join('')}
        </select>
    `;

    const select = cell.querySelector('.status-select');
    select.focus();

    let isSubmitting = false;

    const revert = () => {
        // Откатываемся к старому значению, только если запрос НЕ выполняется
        if (!isSubmitting) {
            cell.innerHTML = `<span class="status-text">${originalText}</span>`;
        }
    };

    select.addEventListener('change', async (e) => {
        const newStatus = e.target.value;
        if (!newStatus) return;

        // Блокируем срабатывание blur при клике/выборе
        isSubmitting = true;

        const payload = {
            ClientId: clientId,
            Id_Dr: walletId,
            newStatus: newStatus
        };

        try {
            // Показываем индикатор обновления
            cell.innerHTML = `<span>Обновление...</span>`;

            await api.patch('/wallets/update', payload);
            cell.innerHTML = `<span class="status-text">${newStatus}</span>`;
        } catch (err) {
            console.error('Ошибка изменения статуса:', err);
            alert(err.message || 'Не удалось обновить статус');
            
            // Ошибка: возвращаем исходный текст
            isSubmitting = false;
            revert();
        }
    });

    select.addEventListener('blur', () => {
        // Если выбор не был сделан (просто кликнули мимо), откатываемся через таймаут
        setTimeout(() => {
            revert();
        }, 150);
    });
}