async function refreshToken() {
    const currentRefreshToken = localStorage.getItem('refreshToken');

    if (!currentRefreshToken) {
        throw new Error('Отсутствует refreshToken');
    }

    const response = await fetch('/api/auth/refresh', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ refreshToken: currentRefreshToken })
    });

    if (!response.ok) {
        throw new Error('Не удалось обновить токен');
    }

    const data = await response.json();
    
    localStorage.setItem('accessToken', data.accessToken);
    if (data.refreshToken) {
        localStorage.setItem('refreshToken', data.refreshToken);
    }

    return data.accessToken;
}

function handleSessionExpired() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    window.navigateTo('/auth');
}

async function request(endpoint, options = {}, isRetry = false) {
    const token = localStorage.getItem('accessToken');

    const headers = {
        'Content-Type': 'application/json',
        ...options.headers,
    };

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    const response = await fetch(`/api${endpoint}`, {
        ...options,
        headers,
    });

    // Если получаем 401 и запрос не отправлялся со страниц авторизации
    if (response.status === 401 && !endpoint.includes('/auth/')) {
        if (!isRetry) {
            try {
                const newAccessToken = await refreshToken();

                options.headers = {
                    ...options.headers,
                    'Authorization': `Bearer ${newAccessToken}`
                };
                return await request(endpoint, options, true);

            } catch (refreshErr) {
                handleSessionExpired();
                throw new Error('Сессия истекла. Войдите снова.');
            }
        } else {
            handleSessionExpired();
            throw new Error('Сессия истекла.');
        }
    }

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || errorData.title || `Ошибка: ${response.status}`);
    }

    if (response.status === 204) return null;

    const text = await response.text();
    return text ? JSON.parse(text) : {};
}

export const api = {
    get: (endpoint) => request(endpoint, { method: 'GET' }),
    post: (endpoint, body) => request(endpoint, { method: 'POST', body: JSON.stringify(body) }),
    patch: (endpoint, body) => request(endpoint, { method: 'PATCH', body: JSON.stringify(body) }),
    delete: (endpoint) => request(endpoint, { method: 'DELETE' }),
};