import api from './index';
import endpoints from '../services/Endpoints';

export const animais = {
  listarAnimais: async () => {
    try {
      const response = await api.get(endpoints.animais.listar);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarAnimaisInativos: async () => {
    try {
      const response = await api.get(endpoints.animais.listarInativos);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais inativos:', error);
      throw error.response ? error.response.data : error;
    }
  },

  ativarAnimal: async (id) => {
    try {
      const response = await api.put(endpoints.animais.ativar(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao ativar animal:', error);
      throw error.response ? error.response.data : error;
    }
  },

  detalhesAnimal: async (id) => {
    try {
      const response = await api.get(endpoints.animais.detalhes(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar detalhes do animal:', error);
      throw error.response ? error.response.data : error;
    }
  },

  editarAnimal: async (id, animalData) => {
    try {
      const response = await api.put(endpoints.animais.editar(id), animalData);
      return response.data;
    } catch (error) {
      console.error('Erro ao editar animal:', error);
      throw error.response ? error.response.data : error;
    }
  },

  cadastrarAnimal: async (animalData) => {
    try {
      const response = await api.post(endpoints.animais.cadastrar, animalData);
      return response.data;
    } catch (error) {
      console.error('Erro ao cadastrar animal:', error);
      throw error.response ? error.response.data : error;
    }
  },

  desativarAnimal: async (id) => {
    try {
      const response = await api.delete(endpoints.animais.desativar(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao desativar animal:', error);
      throw error.response ? error.response.data : error;
    }
  }
};