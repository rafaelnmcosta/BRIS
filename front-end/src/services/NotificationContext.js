import React, { createContext, useContext } from 'react';
import { notification } from 'antd';

// Criação do contexto
const NotificationContext = createContext();

// Hook para acessar o contexto
export const useNotification = () => useContext(NotificationContext);

// Provider para encapsular as notificações
export const NotificationProvider = ({ children }) => {
  // Função para abrir notificações
  const abrirNotificacao = (type, title, descricao) => {
    notification[type]({
      message: title,
      description: descricao,
      placement: 'top', // Define onde a notificação será exibida
    });
  };

  return (
    <NotificationContext.Provider value={abrirNotificacao}>
      {children}
    </NotificationContext.Provider>
  );
};
