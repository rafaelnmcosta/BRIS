import api from './index';
import endpoints from '../services/Endpoints';

export const granjas = {
  listarGranjas: async () => {
    try {
      const response = await api.get(endpoints.granjas.listar);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar granjas:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarPorAgroindustria: async (id) => {
    try {
      const response = await api.get(endpoints.granjas.listarPorAgroindustria(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao listar granjas por agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarGranjasInativas: async () => {
    try {
      const response = await api.get(endpoints.granjas.listarInativas);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar granjas inativas:', error);
      throw error.response ? error.response.data : error;
    }
  },

  ativarGranja: async (id) => {
    try {
      const response = await api.put(endpoints.granjas.ativar(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao ativar granja:', error);
      throw error.response ? error.response.data : error;
    }
  },

  detalhesGranja: async (id) => {
    try {
      const response = await api.get(endpoints.granjas.detalhes(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar detalhes da granja:', error);
      throw error.response ? error.response.data : error;
    }
  },

  editarGranja: async (id, granjaData) => {
    try {
      const response = await api.put(endpoints.granjas.editar(id), granjaData);
      return response.data;
    } catch (error) {
      console.error('Erro ao editar granja:', error);
      throw error.response ? error.response.data : error;
    }
  },

  cadastrarGranja: async (granjaData) => {
    try {
      const response = await api.post(endpoints.granjas.cadastrar, granjaData);
      return response.data;
    } catch (error) {
      console.error('Erro ao cadastrar granja:', error);
      throw error.response ? error.response.data : error;
    }
  },

  desativarGranja: async (id) => {
    try {
      const response = await api.delete(endpoints.granjas.desativar(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao desativar granja:', error);
      throw error.response ? error.response.data : error;
    }
  }
};