import api from './index';
import endpoints from '../services/Endpoints';

export const perfil = {
    acessarPerfil: async () => {
        try {
            const response = await api.get(endpoints.perfil.acessar);
            return response.data;
        } catch (error) {
            console.error('Erro ao buscar dados do perfil:', error);
            throw error.response ? error.response.data : error;
        }
    },

    editarPerfil: async (userData) => {
        try {
            const response = await api.put(
                endpoints.perfil.editar,
                {
                    Nome: userData.nome,
                    Email: userData.email,
                    CPF: userData.cpf,
                    Telefone: userData.telefone,
                    Senha: userData.senha || undefined
                }
            );
            return response.data;
        } catch (error) {
            console.error('Erro ao editar perfil:', error);
            throw error.response ? error.response.data : error;
        }
    }
}    