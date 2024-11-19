import React from 'react';
import { useAuth } from '../services/AuthContext';

import TemplateLogin from '../components/templates/TemplateLogin';

const Login = () => {
  const { login } = useAuth();

  const handleLogin = async ({ email, senha }) => {
    login({ email, senha });  // Chama a função login do contexto
  };

  return (
    <TemplateLogin handleLogin={handleLogin} />
  );
};

export default Login;
