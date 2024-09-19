import React from 'react';
import { useAuth } from '../services/AuthContext';

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
    </div>
  );
};

export default Teste;
