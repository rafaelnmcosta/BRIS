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
  const [loading, setLoading] = useState(true); // Indicador de carregamento
  const navigate = useNavigate();

  // Chama `checkAuth` na inicialização do componente
  useEffect(() => {
    if (loading) {
      checkAuth();
    }
  }, [loading]);

const checkAuth = async () => {
  try {
    const status = await autenticacao.verificarAutenticacao();

    switch (status) {
      case 'autenticado':
        console.log("autenticado");
        setIsLogged(true);
        setIsAuthenticated(true);
        break;
      case 'logado':
        console.log("logado");
        setIsLogged(true);
        setIsAuthenticated(false);
        break;
      default:
        console.log("default");
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
      console.log("Login bem-sucedido. Redirecionando para /vinculos...");
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
    console.log("rodou a chamada de api: ", response);
    if (response.status === 200) {
      await checkAuth();
      console.log("Vínculo selecionado. Redirecionando para /home...");
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
    console.log("Usuário deslogado. Redirecionando para /login...");
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
      login,
      escolherVinculo,
      logout,
      loading,
    }}
  >
    {children}
  </AuthContext.Provider>
);
};
