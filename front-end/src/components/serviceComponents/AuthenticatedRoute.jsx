import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../../services/AuthContext';

const AuthenticatedRoute = () => {
  const { isAuthenticated, isLogged, loading } = useAuth();

  if (loading) return <div>Carregando...</div>;

  return isAuthenticated 
  ? <Outlet /> 
  : isLogged 
    ? <Navigate to="/vinculos" />
    : <Navigate to="/login" />;

};

export default AuthenticatedRoute;
