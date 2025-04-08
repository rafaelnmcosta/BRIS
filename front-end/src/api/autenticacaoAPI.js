import api from './index';  // configuração do axios
import endpoints from '../services/Endpoints'; // endpoints

export const autenticacao = {
    autoCadastro: async (userData) => {
        try {
            const response = await api.post(endpoints.autenticacao.cadastro, userData);
            return response.data;
        } catch (error) {
            console.error('Erro ao cadastrar o usuário:', error);
            throw error.response ? error.response.data : error;
        }
    },

    login: async ({ email, senha }) => {
        try {
            const response = await api.post(endpoints.autenticacao.login, { email, senha });
            return response;
        } catch (error) {
            console.error('Erro ao fazer login:', error);
            throw error.response ? error.response.data : error;
        }
    },

    logout: async () => {
        try {
            const response = await api.get(endpoints.autenticacao.logout);
            return response.data;
        } catch (error) {
            console.error('Erro ao fazer logout:', error);
            throw error.response ? error.response.data : error;
        }
    },

    escolherVinculo: async (id) => {
        try {
            const response = await api.post(endpoints.autenticacao.escolherVinculo(id));
            return response;
        } catch (error) {
            console.error('Erro ao escolher vínculo:', error);
            throw error.response ? error.response.data : error;
        }
    },

    recuperarSenha: async (email) => {
        try {
            const response = await api.post(endpoints.autenticacao.recuperarSenha, { email });
            return response.data;
        } catch (error) {
            console.error('Erro ao recuperar senha:', error);
            throw error.response ? error.response.data : error;
        }
    },

    listarVinculos: async () => {
        try {
            const response = await api.get(endpoints.autenticacao.listarVinculos);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar vínculos:', error);
            throw error.response ? error.response.data : error;
        }
    },

    verificarAutenticacao: async () => {
        try {
            const response = await api.get(endpoints.autenticacao.checarStatus);
            console.log(response.data);
            
            if (response.data.status === 'autenticado') {
                return {
                    status: response.data.status,
                    role: response.data.role,
                    granja: response.data.granja,
                    granjaId: response.data.granjaId,
                    agroindustria: response.data.agroindustria,
                    agroindustriaId: response.data.agroindustriaId,
                    usuarioNome: response.data.usuarioNome
                };
            } else if (response.data.status === 'logado') {
                return {
                    status: response.data.status,
                    userId: response.data.userId
                };
            }
            return response.data;
        } catch (error) {
            return false;
        }
    },
};
