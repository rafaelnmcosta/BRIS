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
      console.log("Entrou na checkAuth");

      switch (status) {
        case 'autenticado':
          console.log("autenticado");
          setIsAuthenticated(true);
          setIsLogged(true);
          break;
        case 'logado':
          console.log("logado");
          setIsAuthenticated(false);
          setIsLogged(true);
          break;
        default:
          console.log("default");
          setIsAuthenticated(false);
          setIsLogged(false);
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
        setIsLogged(true); // Usuário está logado
        setIsAuthenticated(false); // Garantir que vínculo ainda não foi escolhido
        await checkAuth(); // Chama a função de verificação de autenticação
        console.log("Login bem-sucedido. Redirecionando para /vinculos...");
        console.log("auth: ", isAuthenticated);
        console.log("logged: ", isLogged);
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
        setIsAuthenticated(true); // Agora o vínculo foi selecionado
        await checkAuth(); // Chama a função de verificação de autenticação
        console.log("Vínculo selecionado. Redirecionando para /home...");
        console.log("auth: ", isAuthenticated);
        console.log("logged: ", isLogged);
        navigate('/home');
      }
    } catch (error) {
      console.error('Erro ao selecionar vínculo:', error);
      throw error;
    }
  };

  const logout = async () => {
    try {
      await autenticacao.logout(); // Chama a função de logout da API
      setIsLogged(false);
      setIsAuthenticated(false);
      await checkAuth(); // Chama a função de verificação de autenticação
      console.log("Usuário deslogado. Redirecionando para /login...");
      console.log("auth: ", isAuthenticated);
      console.log("logged: ", isLogged);
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
