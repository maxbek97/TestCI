import { api } from '/js/api.js';

export function RegisterPage() {
    return `
        <section id="reg" class="authorization-section">
          <div class="authorization-container">
            <div class="authorization-card">
              <h2 class="authorization-title">Создать аккаунт</h2>
    
              <form class="authorization-form" id="registrationForm">
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
                  <label>Имя пользователя</label>
                  <input
                    id="userLogin"
                    type="text" 
                    required
                    name="userLogin"
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
                    <span class="button-text">Зарегистрироваться</span>
                  </button>
                </div>
              </form>
    
              <p class="login-prompt">
                Уже есть аккаунт? <a href="/auth" data-link>Войти</a>
              </p>
            </div>
          </div>
        </section>
    `;
}

export function initRegisterEvents() {
    const form = document.getElementById('registrationForm');
    if (!form) return;

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        // Формируем payload с именами полей
        const payload = {
            userEmail: form.userEmail.value.trim(),
            userLogin: form.userLogin.value.trim(),
            password: form.password.value
        };

        try {
            await api.post('/auth/register', payload);

            alert('Регистрация успешна! Переходим к авториозации...');

            setTimeout(() => {
                window.navigateTo('/auth');
            }, 2000);
        } catch (err) {
            console.error('Ошибка регистрации:', err);
            alert(err.message || 'Ошибка регистрации');
        }
    });
}