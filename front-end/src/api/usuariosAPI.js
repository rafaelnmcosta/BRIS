import api from './index';
import endpoints from '../services/Endpoints';
import { message } from 'antd';

export const usuarios = {
  listarUsuarios: async () => {
    try {
      const response = await api.get(endpoints.usuarios.listarUsuarios);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar usuários';
      message.error(mensagem);
      throw mensagem;
    }
  },

  cadastrarUsuario: async (userData) => {
    try {
      const response = await api.post(endpoints.usuarios.cadastrarUsuario, userData);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao cadastrar usuário';
      message.error(mensagem);
      throw mensagem;
    }
  },

  detalhesUsuario: async (id) => {
    try {
      const response = await api.get(endpoints.usuarios.detalhesUsuario(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao buscar detalhes do usuário';
      message.error(mensagem);
      throw mensagem;
    }
  },

  editarUsuario: async (userData) => {
    try {
      const response = await api.put(
        endpoints.usuarios.editarUsuario(userData.id),
        {
          Nome: userData.nome,
          Email: userData.email,
          CPF: userData.cpf,
          Telefone: userData.telefone,
          Senha: userData.senha || undefined,
          Vinculos: userData.vinculos?.map(v => ({
            RoleId: v.roleId,
            GranjaId: v.granjaId || null,
            AgroindustriaId: v.agroindustriaId || null
          }))
        }
      );
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao editar usuário';
      message.error(mensagem);
      throw mensagem;
    }
  },

  listarUsuariosInativos: async () => {
    try {
      const response = await api.get(endpoints.usuarios.listarUsuariosInativos);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar usuários inativos';
      message.error(mensagem);
      throw mensagem;
    }
  },

  reativarUsuario: async (id, novosVinculos) => {
    try {
      const response = await api.put(endpoints.usuarios.reativarUsuario(id), novosVinculos);
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao reativar usuário';
      message.error(mensagem);
      throw mensagem;
    }
  },

  listarVinculosUsuario: async (id) => {
    try {
      const response = await api.get(endpoints.usuarios.listarVinculosUsuario(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao listar vínculos do usuário';
      message.error(mensagem);
      throw mensagem;
    }
  },

  inativarUsuario: async (id) => {
    try {
      const response = await api.put(endpoints.usuarios.inativarUsuario(id));
      return response.data;
    } catch (error) {
      const mensagem = error.response?.data || 'Erro ao inativar usuário';
      message.error(mensagem);
      throw mensagem;
    }
  }
};
