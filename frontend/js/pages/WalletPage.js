import { api } from '/js/api.js';
import { WalletRow } from '/js/components/WalletRow.js';

// Перечень доступных статусов для выбора
const STATUS_OPTIONS = ['Actv', 'Clsd', 'Blck'];

export function WalletsPage() {
    const clientName = sessionStorage.getItem('selectedClientName');
    const titleText = clientName ? `Кошелек (${clientName})` : 'Кошелек клиента';

    return `
        <div class="request-wrapper-global">
            <div class="cf-header">
                <h1 class="cf-title h1">Кошельки: ${clientName}</h1>
                <div class="header-buttons">
                    <button class="action-button" id="backToClientsBtn">&laquo; К клиентам</button>
                    <button class="action-button" id="addWalletBtn" style="margin-left: 10px;">+ Создать кошелек</button>
                </div>
            </div>
        </div>

        <div class="table-container">
            <table class="data-table">
                <thead>
                    <tr>
                        <th>ID кошелька</th>
                        <th>Статус</th>
                        <th>Доп. инфо</th>
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
    const clientId = params?.clientId;
    const backBtn = document.getElementById('backToClientsBtn');
    const tbody = document.getElementById('walletsTableBody');

    if (backBtn) {
        backBtn.addEventListener('click', () => window.navigateTo('/clients'));
    }

    if (clientId) {
        loadWallets(clientId);
    }

    // Делегирование клика на ячейку статуса
    if (tbody) {
        tbody.addEventListener('click', (e) => {
            const statusCell = e.target.closest('.status-cell');
            if (statusCell && !statusCell.querySelector('select')) {
                activateStatusSelect(statusCell);
            }
        });
    }
}

async function loadWallets(clientId) {
    const tbody = document.getElementById('walletsTableBody');
    if (!tbody) return;

    try {
        const wallets = await api.get(`/clients/${clientId}/wallets`);

        if (!wallets || wallets.length === 0) {
            tbody.innerHTML = '<tr><td colspan="3" style="text-align:center;">Кошельки не найдены</td></tr>';
            return;
        }

        tbody.innerHTML = wallets.map(wallet => WalletRow(wallet)).join('');
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="3" style="text-align:center; color:red;">${err.message || 'Ошибка'}</td></tr>`;
    }
}

// Превращение текста статуса в Dropdown
function activateStatusSelect(cell) {
    const originalText = cell.querySelector('.status-text').textContent;
    const row = cell.closest('tr');
    const walletId = row.dataset.walletId;

    // Подменяем содержимое ячейки
    cell.innerHTML = `
        <span>Заменить на: </span>
        <select class="status-select">
            <option value="" disabled selected>Выберите...</option>
            ${STATUS_OPTIONS.map(opt => `<option value="${opt}">${opt}</option>`).join('')}
        </select>
    `;

    const select = cell.querySelector('.status-select');
    select.focus();

    // Функция отмены изменений и возврата старого текста
    const revert = () => {
        cell.innerHTML = `<span class="status-text">${originalText}</span>`;
    };

    // Событие выбора нового статуса
    select.addEventListener('change', async (e) => {
        const newStatus = e.target.value;
        try {
            // Эндпоинт обновления статуса (замените путь при необходимости)
            await api.put(`/wallets/${walletId}/status`, { status: newStatus });
            cell.innerHTML = `<span class="status-text">${newStatus}</span>`;
        } catch (err) {
            alert(err.message || 'Не удалось обновить статус');
            revert();
        }
    });

    // Если пользователь кликнул мимо выпадающего списка — отменяем
    select.addEventListener('blur', () => {
        // Небольшая задержка, чтобы успел сработать 'change', если кликнули по пункту
        setTimeout(() => {
            if (document.activeElement !== select) {
                revert();
            }
        }, 150);
    });
}