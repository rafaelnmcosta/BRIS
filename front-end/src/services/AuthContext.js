import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { autenticacao } from '../api/autenticacaoAPI'; // Importando a API de autenticação

const AuthContext = createContext();

export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [isLogged, setIsLogged] = useState(false); // Usuário fez login?
  const [isAuthenticated, setIsAuthenticated] = useState(false); // Usuário escolheu vínculo?
  const [userType, setUserType] = useState(null); // Estado para armazenar o tipo de usuário
  const [loading, setLoading] = useState(true); // Indicador de carregamento
  const navigate = useNavigate();

  // Chama `checkAuth` na inicialização do componente
  useEffect(() => {
    if (loading) checkAuth();
    if (isLogged && !isAuthenticated) navigate('/vinculos');
  }, [loading, isAuthenticated, isLogged, navigate]);

const checkAuth = async () => {
  try {
    const resposta = await autenticacao.verificarAutenticacao();
    console.log("Resposta da checkAuth: ", resposta)

    switch (resposta.status) {
      case 'autenticado':
        setIsLogged(true);
        setIsAuthenticated(true);
        setUserType(resposta.role); // Atualiza o tipo de usuário
        break;
      case 'logado':
        setIsLogged(true);
        setIsAuthenticated(false);
        break;
      default:
        setIsLogged(false);
        setIsAuthenticated(false);
        break;
    }
  } catch (error) {
    console.error('Erro ao verificar autenticação:', error);
    setIsAuthenticated(false);
    setIsLogged(false);
  } finally {
    setLoading(false);
  }
};

const login = async ({ email, senha }) => {
  try {
    const response = await autenticacao.login({ email, senha });
    if (response.status === 200) {
      await checkAuth();
      navigate('/vinculos');
    }
  } catch (error) {
    console.error('Erro ao fazer login:', error);
    throw error;
  }
};

const escolherVinculo = async (id) => {
  try {
    const response = await autenticacao.escolherVinculo(id);
    if (response.status === 200) {
      await checkAuth();
      navigate('/home');
    }
  } catch (error) {
    console.error('Erro ao selecionar vínculo:', error);
    throw error;
  }
};

const logout = async () => {
  try {
    await autenticacao.logout();
    await checkAuth();
    navigate('/login');
  } catch (error) {
    console.error('Erro ao fazer logout:', error);
  }
};

return (
  <AuthContext.Provider
    value={{
      isLogged,
      isAuthenticated,
      userType,
      loading,
      login,
      escolherVinculo,
      logout,
    }}
  >
    {children}
  </AuthContext.Provider>
);
};
