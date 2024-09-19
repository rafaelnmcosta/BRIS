import React from 'react';
import { useAuth } from '../../services/AuthContext';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import { MailOutlined } from '@ant-design/icons';

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
        <button onClick={handleLogout}>Logout</button>
      ) : (
        <button onClick={handleLogin}>Login</button>
      )}
      <InputSemBordaComLabel
        label="Testeeeee"
        placeholder="Receba seu placeholder"
        icone={<MailOutlined/>}
      />
    </div>
  );
};

export default Teste;
