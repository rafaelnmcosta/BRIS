import axios from 'axios';

const api = axios.create({
  baseURL: 'http://api:5000/',
  withCredentials: true,
});

// Interceptor para tratamento de erros
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      return Promise.reject(error);
    }
    return Promise.reject(new Error('Erro desconhecido'));
  }
);

export default api;
