import api from './index';
import endpoints from '../services/Endpoints';
import { message } from 'antd';

export const granjas = {
  listarGranjas: async () => {
    try {
      const response = await api.get(endpoints.granjas.listar);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar granjas';
      message.error(mensagem);
      throw mensagem;
    }
  },

  listarPorAgroindustria: async (id) => {
    try {
      const response = await api.get(endpoints.granjas.listarPorAgroindustria(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar granjas por agroindústria';
      message.error(mensagem);
      throw mensagem;
    }
  },

  listarGranjasInativas: async () => {
    try {
      const response = await api.get(endpoints.granjas.listarInativas);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar granjas inativas';
      message.error(mensagem);
      throw mensagem;
    }
  },

  ativarGranja: async (id) => {
    try {
      const response = await api.put(endpoints.granjas.ativar(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao ativar granja';
      message.error(mensagem);
      throw mensagem;
    }
  },

  detalhesGranja: async (id) => {
    try {
      const response = await api.get(endpoints.granjas.detalhes(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao buscar detalhes da granja';
      message.error(mensagem);
      throw mensagem;
    }
  },

  editarGranja: async (id, granjaData) => {
    try {
      const response = await api.put(endpoints.granjas.editar(id), granjaData);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao editar granja';
      message.error(mensagem);
      throw mensagem;
    }
  },

  cadastrarGranja: async (granjaData) => {
    try {
      const response = await api.post(endpoints.granjas.cadastrar, granjaData);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao cadastrar granja';
      message.error(mensagem);
      throw mensagem;
    }
  },

  desativarGranja: async (id) => {
    try {
      const response = await api.delete(endpoints.granjas.desativar(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao desativar granja';
      message.error(mensagem);
      throw mensagem;
    }
  }
};