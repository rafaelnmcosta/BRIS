import React from 'react';
import { useAuth } from '../services/AuthContext';

const Home = () => {
  const { userType } = useAuth();

  const renderContent = () => {
    switch (userType) {
      case 'ADMIN':
        return <p>Bem-vindo, Administrador!</p>;
      case 'GESTOR_AGRO':
        return <p>Bem-vindo, Gestor de Agroindústria!</p>;
      case 'GESTOR_GRANJA':
        return <p>Bem-vindo, Gestor de Granja!</p>;
      case 'TECNICO':
        return <p>Bem-vindo, Técnico!</p>;
      default:
        return <p>Bem-vindo! (sem role)</p>;
    }
  };

  return (
    <div>
      <h1>Home</h1>
      {renderContent()}
    </div>
  );
};

export default Home;
