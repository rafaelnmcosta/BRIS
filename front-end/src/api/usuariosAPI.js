import api from './index';
import endpoints from '../services/Endpoints';

export const usuarios = {
  listarUsuarios: async () => {
    try {
      const response = await api.get(endpoints.usuarios.listarUsuarios);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar usuários:', error);
      throw error.response ? error.response.data : error;
    }
  },

  cadastrarUsuario: async (userData) => {
    try {
      const response = await api.post(endpoints.usuarios.cadastrarUsuario, userData);
      return response.data;
    } catch (error) {
      console.error('Erro ao cadastrar usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  detalhesUsuario: async (id) => {
    try {
      const response = await api.get(endpoints.usuarios.detalhesUsuario(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar detalhes do usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  editarUsuario: async (id, userData) => {
    try {
      const response = await api.put(endpoints.usuarios.editarUsuario(id), userData);
      return response.data;
    } catch (error) {
      console.error('Erro ao editar usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarUsuariosInativos: async () => {
    try {
      const response = await api.get(endpoints.usuarios.listarUsuariosInativos);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar usuários inativos:', error);
      throw error.response ? error.response.data : error;
    }
  },

  reativarUsuario: async (id, novosVinculos) => {
    try {
      const response = await api.put(endpoints.usuarios.reativarUsuario(id), { novosVinculos });
      return response.data;
    } catch (error) {
      console.error('Erro ao reativar usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarVinculosUsuario: async (id) => {
    try {
      const response = await api.get(endpoints.usuarios.listarVinculosUsuario(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao listar vínculos do usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  adicionarVinculoUsuario: async (id, vinculoData) => {
    try {
      const response = await api.post(endpoints.usuarios.adicionarVinculoUsuario(id), vinculoData);
      return response.data;
    } catch (error) {
      console.error('Erro ao adicionar vínculo ao usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  desativarUsuario: async (id) => {
    try {
      const response = await api.delete(endpoints.usuarios.desativarUsuario(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao desativar usuário:', error);
      throw error.response ? error.response.data : error;
    }
  },

  inativarUsuario: async (id) => {
    try {
      const response = await api.put(endpoints.usuarios.inativarUsuario(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao inativar usuário:', error);
      throw error.response ? error.response.data : error;
    }
  }
};