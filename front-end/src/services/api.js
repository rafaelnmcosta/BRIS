import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5206/api',
});

// interceptor
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('jwtToken');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// interceptor para tratamento de erros
api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            // Redireciona para a página de login
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;
