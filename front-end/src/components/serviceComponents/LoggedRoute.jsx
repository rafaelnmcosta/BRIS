import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';

const LoggedRoute = () => {
  const { isLogged, loading } = useAuth();

  if (loading) return <div>Carregando...</div>;

  return isLogged ? <Outlet /> : <Navigate to="/login" />;
};

export default LoggedRoute;
