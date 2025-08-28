import api from './index';
import endpoints from '../services/Endpoints';

export const animais = {
  listarAtivos: async () => {
    try {
      const response = await api.get(endpoints.animais.listarAtivos);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais ativos:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarAtivosPorGranja: async () => {
    try {
      const response = await api.get(endpoints.animais.listarAtivosPorGranja);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais ativos por granja:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarAtivosPorAgroindustria: async () => {
    try {
      const response = await api.get(endpoints.animais.listarAtivosPorAgroindustria);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais ativos por agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarInativos: async () => {
    try {
      const response = await api.get(endpoints.animais.listarInativos);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais inativos:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarInativosPorGranja: async () => {
    try {
      const response = await api.get(endpoints.animais.listarInativosPorGranja);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais inativos por granja:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarInativosPorAgroindustria: async () => {
    try {
      const response = await api.get(endpoints.animais.listarInativosPorAgroindustria);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar animais inativos por agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  ativarAnimal: async (id) => {
    try {
      const response = await api.put(endpoints.animais.ativar(id));
      return response.data;
    } catch (error) {
      console.error(`Erro ao ativar animal (ID: ${id}):`, error);
      throw error.response ? error.response.data : error;
    }
  },

  desativarAnimal: async (id) => {
    try {
      const response = await api.delete(endpoints.animais.desativar(id));
      return response.data;
    } catch (error) {
      console.error(`Erro ao desativar animal (ID: ${id}):`, error);
      throw error.response ? error.response.data : error;
    }
  },

  detalhesAnimal: async (id) => {
    try {
      const response = await api.get(endpoints.animais.detalhes(id));
      return response.data;
    } catch (error) {
      console.error(`Erro ao buscar detalhes do animal (ID: ${id}):`, error);
      throw error.response ? error.response.data : error;
    }
  },

  editarAnimal: async (id, animalData) => {
    try {
      const response = await api.put(endpoints.animais.editar(id), animalData);
      return response.data;
    } catch (error) {
      console.error(`Erro ao editar animal (ID: ${id}):`, error);
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
  }
};
