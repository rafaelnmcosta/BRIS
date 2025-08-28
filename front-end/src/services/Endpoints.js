const API_BASE_URL = '/api';

const endpoints = {
  agroindustrias: {
    listar: `${API_BASE_URL}/agroindustrias`,
    listarInativas: `${API_BASE_URL}/agroindustrias/ativar`,
    ativar: (id) => `${API_BASE_URL}/agroindustrias/ativar/${id}`,
    detalhes: (id) => `${API_BASE_URL}/agroindustrias/${id}`,
    editar: (id) => `${API_BASE_URL}/agroindustrias/${id}/editar`,
    cadastrar: `${API_BASE_URL}/agroindustrias/cadastrar`,
    desativar: (id) => `${API_BASE_URL}/agroindustrias/${id}/desativar`
  },
  animais: {
    listarAtivos: `${API_BASE_URL}/animais/ativos`,
    listarAtivosPorGranja: `${API_BASE_URL}/animais/ativos/granja`,
    listarAtivosPorAgroindustria: `${API_BASE_URL}/animais/ativos/agroindustria`,
    listarInativos: `${API_BASE_URL}/animais/inativos`,
    listarInativosPorGranja: `${API_BASE_URL}/animais/inativos/granja`,
    listarInativosPorAgroindustria: `${API_BASE_URL}/animais/inativos/agroindustria`,
    ativar: (id) => `${API_BASE_URL}/animais/ativar/${id}`,
    detalhes: (id) => `${API_BASE_URL}/animais/${id}`,
    editar: (id) => `${API_BASE_URL}/animais/${id}/editar`,
    cadastrar: `${API_BASE_URL}/animais/cadastrar`,
    desativar: (id) => `${API_BASE_URL}/animais/${id}/desativar`
  },
  autenticacao: {
    cadastro: `${API_BASE_URL}/auth/cadastro`,
    login: `${API_BASE_URL}/auth/login`,
    logout: `${API_BASE_URL}/auth/logout`,
    listarVinculosParaTrocar: `${API_BASE_URL}/auth/trocar-vinculo`,
    listarVinculos: `${API_BASE_URL}/auth/vinculos`,
    escolherVinculo: (id) => `${API_BASE_URL}/auth/vinculos/${id}`,
    recuperarSenha: `${API_BASE_URL}/auth/recuperar-senha`,
    checarStatus: `${API_BASE_URL}/auth/check`
  },
  avaliacoes: {
    listar: `${API_BASE_URL}/avalicoes`,
    listarInterrompidas: `${API_BASE_URL}/avaliacoes/interrompidas`,
    detalhes: (id) => `${API_BASE_URL}/avaliacoes/${id}`,
    novaAvaliacao: (id) => `${API_BASE_URL}/avaliacoes/nova/${id}`,
    novaDose: (id) => `${API_BASE_URL}/avaliacoes/${id}/nova-dose`,
    finalizar: (id) => `${API_BASE_URL}/avaliacoes/finaliza/${id}`,
    interromper: (id) => `${API_BASE_URL}/avaliacoes/interrompe/${id}`,
    reativar: (id) => `${API_BASE_URL}/avaliacoes/${id}/reativar`
  },
  granjas: {
    listar: `${API_BASE_URL}/granjas`,
    listarPorAgroindustria: (id) => `${API_BASE_URL}/granjas/agro/${id}`,
    listarInativas: `${API_BASE_URL}/granjas/ativar`,
    ativar: (id) => `${API_BASE_URL}/granjas/ativar/${id}`,
    detalhes: (id) => `${API_BASE_URL}/granjas/${id}`,
    editar: (id) => `${API_BASE_URL}/granjas/${id}/editar`,
    cadastrar: `${API_BASE_URL}/granjas/cadastrar`,
    desativar: (id) => `${API_BASE_URL}/granjas/${id}/desativar`
  },
  perfil: {
    acessar: `${API_BASE_URL}/perfil`,
    editar: `${API_BASE_URL}/perfil/editar`,
  },
  usuarios: {
    listarUsuarios: `${API_BASE_URL}/usuarios`,
    cadastrarUsuario: `${API_BASE_URL}/usuarios/cadastrar`,
    detalhesUsuario: (id) => `${API_BASE_URL}/usuarios/${id}`,
    editarUsuario: (id) => `${API_BASE_URL}/usuarios/editar/${id}`,
    listarUsuariosInativos: `${API_BASE_URL}/usuarios/inativos`,
    reativarUsuario: (id) => `${API_BASE_URL}/usuarios/reativar/${id}`,
    inativarUsuario: (id) => `${API_BASE_URL}/usuarios/inativar/${id}`,
    listarVinculos: (id) => `${API_BASE_URL}/usuarios/vinculos/${id}`
  },
};

export default endpoints;