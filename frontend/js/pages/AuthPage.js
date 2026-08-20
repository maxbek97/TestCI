import { api } from '/js/api.js';

export function AuthPage() {
    return `
        <section id="auth" class="authorization-section">
            <div class="authorization-container">
              <div class="authorization-card">
                <h2 class="authorization-title">Войти в аккаунт</h2>
      
                <form class="authorization-form" id="loginForm">
                  <div class="input-group">
                    <label>Электронная почта</label>
                    <input
                        id="userEmail"
                        type="email"
                        name="userEmail"
                        placeholder="IvanZapara04@mail.ru"
                        required
                    />
                  </div>
      
                  <div class="input-group">
                    <label>Пароль</label>
                    <input
                        id="password"
                        type="password" 
                        name="password"
                        required 
                    />
                  </div>
      
                  <div class="authorization-footer">
                    <button type="submit" class="action-button">
                        <span class="button-text">Войти</span>
                    </button>
                  </div>
                </form>
                <p class="login-prompt">
                    Нет аккаунта? <a href="/register" data-link>Зарегистрироваться</a>
                </p>
              </div>
            </div>
        </section>
    `;
}

export function initAuthEvents() {
    const form = document.getElementById('loginForm');
    if (!form) return;

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const payload = {
            userEmail: form.userEmail.value.trim(),
            password: form.password.value
        };

        try {
            const data = await api.post('/auth/login', payload);
            console.log('Ответ бэкенда при входе:', data);

            // Ищем токен (проверяем варианты с разным регистром ключа)
            const token = data.accessToken;

            if (token) {
                localStorage.setItem('accessToken', token);
                if (data.refreshToken) {
                    localStorage.setItem('refreshToken', data.refreshToken);
                }
                alert('Успешный вход!');

                setTimeout(() => {
                    window.navigateTo('/clients');
                }, 500);
            } else {
                // console.error('Бэкенд вернул ответ без токена:', data);
                alert('Сервер не прислал токен авторизации');
            }
        } catch (err) {
            // console.error('Ошибка авторизации:', err);
            alert(err.message || 'Ошибка входа');
        }
    });
}