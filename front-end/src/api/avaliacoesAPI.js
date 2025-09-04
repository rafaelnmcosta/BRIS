import api from './index';
import endpoints from '../services/Endpoints';
import { message } from 'antd';

export const avaliacoes = {
  listarPorGranja: async (id) => {
    try {
      const response = await api.get(endpoints.avaliacoes.listarPorGranja(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar avaliações da granja';
      message.error(mensagem);
      throw mensagem;
    }
  },

  listarInterrompidas: async () => {
    try {
      const response = await api.get(endpoints.avaliacoes.listarInterrompidas);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar avaliações interrompidas';
      message.error(mensagem);
      throw mensagem;
    }
  },

  detalhes: async (id) => {
    try {
      const response = await api.get(endpoints.avaliacoes.detalhes(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao buscar detalhes da avaliação';
      message.error(mensagem);
      throw mensagem;
    }
  },

  novaAvaliacao: async (id, avaliacaoData) => {
    try {
      const response = await api.post(endpoints.avaliacoes.novaAvaliacao(id), avaliacaoData);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao criar nova avaliação';
      message.error(mensagem);
      throw mensagem;
    }
  },

  novaDose: async (id, doseData) => {
    try {
      const response = await api.put(endpoints.avaliacoes.novaDose(id), doseData);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao inserir nova dose';
      message.error(mensagem);
      throw mensagem;
    }
  },

  finalizar: async (id) => {
    try {
      const response = await api.put(endpoints.avaliacoes.finalizar(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao finalizar a avaliação';
      message.error(mensagem);
      throw mensagem;
    }
  },
  interromper: async (id) => {
    try {
      const response = await api.put(endpoints.avaliacoes.interromper(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao interromper a avaliação';
      message.error(mensagem);
      throw mensagem;
    }
  },

  reativar: async (id) => {
    try {
      const response = await api.put(endpoints.avaliacoes.reativar(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao reativar a avaliação';
      message.error(mensagem);
      throw mensagem;
    }
  }
};
