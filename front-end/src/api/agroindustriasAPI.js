import api from './index';
import endpoints from '../services/Endpoints';

export const agroindustrias = {
  listarAgroindustrias: async () => {
    try {
      const response = await api.get(endpoints.agroindustrias.listar);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar agroindústrias:', error);
      throw error.response ? error.response.data : error;
    }
  },

  listarAgroindustriasInativas: async () => {
    try {
      const response = await api.get(endpoints.agroindustrias.listarInativas);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar agroindústrias inativas:', error);
      throw error.response ? error.response.data : error;
    }
  },

  ativarAgroindustria: async (id) => {
    try {
      const response = await api.put(endpoints.agroindustrias.ativar(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao ativar agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  detalhesAgroindustria: async (id) => {
    try {
      const response = await api.get(endpoints.agroindustrias.detalhes(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar detalhes da agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  editarAgroindustria: async (id, agroindustriaData) => {
    try {
      const response = await api.put(endpoints.agroindustrias.editar(id), agroindustriaData);
      return response.data;
    } catch (error) {
      console.error('Erro ao editar agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  cadastrarAgroindustria: async (agroindustriaData) => {
    try {
      const response = await api.post(endpoints.agroindustrias.cadastrar, agroindustriaData);
      return response.data;
    } catch (error) {
      console.error('Erro ao cadastrar agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  },

  desativarAgroindustria: async (id) => {
    try {
      const response = await api.delete(endpoints.agroindustrias.desativar(id));
      return response.data;
    } catch (error) {
      console.error('Erro ao desativar agroindústria:', error);
      throw error.response ? error.response.data : error;
    }
  }
};