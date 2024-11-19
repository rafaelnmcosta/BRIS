import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/',
  withCredentials: true,
});

// // Interceptor para tratamento de erros
// api.interceptors.response.use(
//   (response) => response,
//   (error) => {
//     if (error.response && error.response.status === 401) {
//       window.location.href = '/login'; // Redireciona para a tela de login em caso de não autorizado
//     }
//     return Promise.reject(error);
//   }
// );


export default api;
