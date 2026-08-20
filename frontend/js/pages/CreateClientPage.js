import { api } from '/js/api.js';
import { navigateTo } from '/js/router.js';

export function CreateClientPage() {
    return `
        <div class="request-wrapper-global">
            <div class="cf-header">
                <h1 class="cf-title h1">Создание клиента</h1>
                <div class="header-buttons">
                    <button class="action-button" id="backBtn">Назад к списку</button>
                </div>
            </div>
        </div>

        <div class="form-container" style="max-width: 600px; margin: 20px auto; padding: 20px; background: #fff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);">
            <form id="createClientForm">
                <div class="form-group" style="margin-bottom: 15px;">
                    <label for="lastName" style="display: block; margin-bottom: 5px; font-weight: 600;">Фамилия *</label>
                    <input type="text" id="lastName" name="lastName" required style="width: 100%; padding: 8px; box-sizing: border-box;" placeholder="Запара">
                </div>

                <div class="form-group" style="margin-bottom: 15px;">
                    <label for="firstName" style="display: block; margin-bottom: 5px; font-weight: 600;">Имя *</label>
                    <input type="text" id="firstName" name="firstName" required style="width: 100%; padding: 8px; box-sizing: border-box;" placeholder="Иван">
                </div>

                <div class="form-group" style="margin-bottom: 15px;">
                    <label for="middleName" style="display: block; margin-bottom: 5px; font-weight: 600;">Отчество</label>
                    <input type="text" id="middleName" name="middleName" style="width: 100%; padding: 8px; box-sizing: border-box;" placeholder="Нольчетвертович">
                </div>

                <div class="form-actions" style="text-align: right;">
                    <button type="submit" class="action-button" id="submitBtn">Сохранить</button>
                </div>
            </form>
        </div>
    `;
}

export function initCreateClientEvents() {
    const backBtn = document.getElementById('backBtn');
    const form = document.getElementById('createClientForm');

    if (backBtn) {
        backBtn.addEventListener('click', () => navigateTo('/clients'));
    }

    if (form) {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            const payload = {
                FirstName: document.getElementById('lastName').value.trim(),
                LastName: document.getElementById('firstName').value.trim(),
                MiddleName: document.getElementById('middleName').value.trim() || null,
            };
            try {
                await api.post('/clients/create', payload);
                navigateTo('/clients');
            } catch (err) {
                console.error('Ошибка создания клиента:', err);
                alert(err.message || 'Ошибка регистрации')
            }
        });
    }
}