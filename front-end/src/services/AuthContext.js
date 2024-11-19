import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { autenticacao } from '../api/autenticacaoAPI'; // Importando a API de autenticação

const AuthContext = createContext();

export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true); // Indicador de carregamento
  const navigate = useNavigate();

  useEffect(() => {
    // Verificando autenticação inicial
    const checkAuth = async () => {
      try {
        console.log("Tentando verificar a autenticação inicial...");
        const status = await autenticacao.verificarAutenticacao(); // Usa função da API
        if (status) {
          setIsAuthenticated(true);
        } else {
          setIsAuthenticated(false);
        }
      } catch (error) {
        console.error("Erro ao verificar autenticação inicial:", error);
        setIsAuthenticated(false);
      } finally {
        setLoading(false); // Garante que o estado de carregamento seja desativado
      }
    };

    // Apenas execute se ainda estiver carregando
    if (loading) {
      checkAuth();
    }
  }, [loading])


  const login = async ({ email, senha }) => {
    try {
      const response = await autenticacao.login({ email, senha });
      console.log(response.data);
      if (response.status === 200) {
        setIsAuthenticated(true);
        console.log("Autenticação bem-sucedida. Redirecionando para /vinculos...");
        await navigate('/vinculos');
      }
    } catch (error) {
      console.error('Erro ao fazer login:', error);
      throw error;
    }
  };

  const logout = async () => {
    try {
      await autenticacao.logout(); // Chama a função de logout da API
      setIsAuthenticated(false);
      navigate('/login');
    } catch (error) {
      console.error('Erro ao fazer logout:', error);
    }
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
};
