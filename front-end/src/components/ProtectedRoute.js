import React from 'react';
import { Navigate } from 'react-router-dom';

// Função para verificar se o usuário está autenticado
const isAuthenticated = () => {
    const token = localStorage.getItem('jwtToken');
    //return !!token;
    return true;
};

// Componente de rota protegida
const ProtectedRoute = ({ element: Element }) => {
    return isAuthenticated() ? Element : <Navigate to="/login" />;
};

export default ProtectedRoute;
