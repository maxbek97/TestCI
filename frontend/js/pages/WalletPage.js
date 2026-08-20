import { api } from '/js/api.js';
import { WalletRow } from '/js/components/WalletRow.js';
import { navigateTo } from '/js/router.js';

const STATUS_OPTIONS = ['Actv', 'Clsd', 'Blck', 'Prcs'];

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
    const clientId = params?.clientId;
    const backBtn = document.getElementById('backToClientsBtn');
    const tbody = document.getElementById('walletsTableBody');

    if (backBtn) {
        backBtn.addEventListener('click', () => navigateTo('/clients'));
    }

    if (clientId) {
        loadWallets(clientId);
    }

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
        const response = await api.get(`/clients/${clientId}/wallets`);
        const wallets = response.wallets || [];
        // console.log(wallets)

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

function activateStatusSelect(cell) {
    const originalText = cell.querySelector('.status-text').textContent;
    const row = cell.closest('tr');
    const walletId = row.dataset.walletId;

    cell.innerHTML = `
        <span>Заменить на: </span>
        <select class="status-select">
            <option value="" disabled selected>Выберите...</option>
            ${STATUS_OPTIONS.map(opt => `<option value="${opt}">${opt}</option>`).join('')}
        </select>
    `;

    const select = cell.querySelector('.status-select');
    select.focus();

    const revert = () => {
        cell.innerHTML = `<span class="status-text">${originalText}</span>`;
    };

    select.addEventListener('change', async (e) => {
        const newStatus = e.target.value;
        try {
            await api.put(`/wallets/${walletId}/status`, { status: newStatus });
            cell.innerHTML = `<span class="status-text">${newStatus}</span>`;
        } catch (err) {
            alert(err.message || 'Не удалось обновить статус');
            revert();
        }
    });

    select.addEventListener('blur', () => {
        setTimeout(() => {
            if (document.activeElement !== select) {
                revert();
            }
        }, 150);
    });
}