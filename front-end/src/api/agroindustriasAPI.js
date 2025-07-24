import api from './index';
import endpoints from '../services/Endpoints';
import { message } from 'antd';

export const agroindustrias = {
  listarAgroindustrias: async () => {
    try {
      const response = await api.get(endpoints.agroindustrias.listar);
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao listar agroindústrias';
      message.error(mensagem);
      throw mensagem;
    }
  },

  listarAgroindustriasInativas: async () => {
    try {
      const response = await api.get(endpoints.agroindustrias.listarInativas);
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao listar agroindústrias inativas';
      message.error(mensagem);
      throw mensagem;
    }
  },

  ativarAgroindustria: async (id) => {
    try {
      const response = await api.put(endpoints.agroindustrias.ativar(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao ativar agroindústria';
      message.error(mensagem);
      throw mensagem;
    }
  },

  detalhesAgroindustria: async (id) => {
    try {
      const response = await api.get(endpoints.agroindustrias.detalhes(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao buscar detalhes da agroindústria';
      message.error(mensagem);
      throw mensagem;
    }
  },

  editarAgroindustria: async (id, agroindustriaData) => {
    console.log(agroindustriaData)
    try {
      const response = await api.put(endpoints.agroindustrias.editar(id), agroindustriaData);
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao editar agroindústria';
      message.error(mensagem);
      throw mensagem;
    }
  },

  cadastrarAgroindustria: async (agroindustriaData) => {
    try {
      const response = await api.post(endpoints.agroindustrias.cadastrar, agroindustriaData);
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao cadastrar agroindústria';
      message.error(mensagem);
      throw mensagem;
    }
  },

  desativarAgroindustria: async (id) => {
    try {
      const response = await api.delete(endpoints.agroindustrias.desativar(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response.data || 'Erro ao desativar agroindústria';
      message.error(mensagem);
      throw mensagem;
    }
  }
};