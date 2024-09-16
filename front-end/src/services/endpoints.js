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
    listar: `${API_BASE_URL}/animais`,
    listarInativos: `${API_BASE_URL}/animais/ativar`,
    ativar: (id) => `${API_BASE_URL}/animais/ativar/${id}`,
    detalhes: (id) => `${API_BASE_URL}/animais/${id}`,
    editar: (id) => `${API_BASE_URL}/animais/${id}/editar`,
    cadastrar: `${API_BASE_URL}/animais/cadastrar`,
    desativar: (id) => `${API_BASE_URL}/animais/${id}/desativar`
  },
  autenticacao: {
    cadastro: `${API_BASE_URL}/auth/cadastro`,
    login: `${API_BASE_URL}/auth/login`,
    listarAcessos: (id) => `${API_BASE_URL}/auth/acessos/${id}`,
    receberToken: (id) => `${API_BASE_URL}/auth/acessos/token/${id}`,
    recuperarSenha: `${API_BASE_URL}/auth/recuperar-senha`
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
  gestorAgro: {
    listarAnimais: `${API_BASE_URL}/ga/animais`,
    detalhesAnimal: (id) => `${API_BASE_URL}/ga/animais/${id}`,
    listarUsuarios: `${API_BASE_URL}/ga/usuarios`,
    detalhesUsuario: (id) => `${API_BASE_URL}/ga/usuarios/${id}`,
    editarUsuario: (id) => `${API_BASE_URL}/ga/usuarios/${id}/editar`,
    cadastrarUsuario: `${API_BASE_URL}/ga/usuarios/cadastrar`,
    listarUsuariosInativos: `${API_BASE_URL}/ga/usuarios/ativar`,
    ativarUsuario: (id) => `${API_BASE_URL}/ga/usuarios/ativar/${id}`,
    listarGranjas: `${API_BASE_URL}/ga/granjas`,
    listarGranjasInativas: `${API_BASE_URL}/ga/granjas/ativar`,
    ativarGranja: (id) => `${API_BASE_URL}/ga/granjas/ativar/${id}`,
    detalhesGranja: (id) => `${API_BASE_URL}/ga/granjas/${id}`,
    editarGranja: (id) => `${API_BASE_URL}/ga/granjas/${id}/editar`,
    cadastrarGranja: `${API_BASE_URL}/granjas/cadastrar`,
    desativarGranja: (id) => `${API_BASE_URL}/granjas/${id}/desativar`
  },
  gestorGranja: {
    listarUsuarios: `${API_BASE_URL}/gg/usuarios`,
    detalhesUsuario: (id) => `${API_BASE_URL}/gg/usuarios/${id}`,
    editarUsuario: (id) => `${API_BASE_URL}/gg/usuarios/${id}/editar`,
    cadastrarUsuario: `${API_BASE_URL}/gg/usuarios/cadastrar`,
    listarUsuariosInativos: `${API_BASE_URL}/gg/usuarios/ativar`,
    ativarUsuario: (id) => `${API_BASE_URL}/gg/usuarios/ativar/${id}`,
  },
  granjas: {
    listar: `${API_BASE_URL}/granjas`,
    listarInativos: `${API_BASE_URL}/granjas/ativar`,
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
    listarUsuariosInativos: `${API_BASE_URL}/usuarios/ativar`,
    ativarUsuario: (id) => `${API_BASE_URL}/usuarios/ativar/${id}`,
    editarUsuario: (id) => `${API_BASE_URL}/usuarios/${id}/editar`,
  }
};

export default endpoints;