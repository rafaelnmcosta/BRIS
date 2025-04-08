import React from 'react';
import { useAuth } from '../services/AuthContext';
import BotaoPrimario from '../components/atoms/BotaoPrimario';
import FormAutoCadastro from '../components/organisms/FormAutoCadastro';
import FormLogin from '../components/organisms/FormLogin';
import CadastroUsuario from './CadastroUsuario';

const Teste = () => {
  const { isAuthenticated, login, logout } = useAuth();

  const handleLogin = () => {
    const token = 'token-de-teste';
    login(token);
  };

  const handleLogout = () => {
    logout();
  };

  return (
    <div>
      {isAuthenticated ? (
        <BotaoPrimario onClick={handleLogout} texto="Logout"/>
      ) : (
        <BotaoPrimario onClick={handleLogin} texto="Login"/>
      )}
      <FormAutoCadastro/>
      <FormLogin/>

    </div>
  );
};

export default Teste;
