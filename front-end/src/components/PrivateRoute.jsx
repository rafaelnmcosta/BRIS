import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../services/AuthContext';

const PrivateRoute = ({ children }) => {
  const { isAuthenticated, isLogged, loading } = useAuth();

  if (loading) return <div>Carregando...</div>;

  return isAuthenticated && isLogged ? <Outlet/> : <Navigate to="/login" />;
};


export default PrivateRoute;
