// validation/ValidationContext.js
import React, { createContext, useContext } from 'react';

const ValidationContext = createContext();

export const useValidation = () => useContext(ValidationContext);

export const ValidationProvider = ({ children }) => {
  const validarEmail = (email) => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email) ? '' : 'Email inválido';
  };

  const validarCPF = (cpf) => {
    const cleaned = cpf.replace(/\D/g, '');
    if (cleaned.length !== 11 || /^(\d)\1+$/.test(cleaned)) return 'CPF inválido';

    let sum = 0;
    for (let i = 0; i < 9; i++) sum += parseInt(cleaned[i]) * (10 - i);
    let rest = (sum * 10) % 11;
    if (rest === 10 || rest === 11) rest = 0;
    if (rest !== parseInt(cleaned[9])) return 'CPF inválido';

    sum = 0;
    for (let i = 0; i < 10; i++) sum += parseInt(cleaned[i]) * (11 - i);
    rest = (sum * 10) % 11;
    if (rest === 10 || rest === 11) rest = 0;
    if (rest !== parseInt(cleaned[10])) return 'CPF inválido';

    return '';
  };

  const validarSenha = (senha) => {
    return senha.length >= 6 ? '' : 'A senha deve ter pelo menos 6 caracteres';
  };

  const validarCampoObrigatorio = (valor) => {
    return valor?.trim() ? '' : 'Campo obrigatório';
  };

  return (
    <ValidationContext.Provider
      value={{
        validarEmail,
        validarCPF,
        validarSenha,
        validarCampoObrigatorio
      }}
    >
      {children}
    </ValidationContext.Provider>
  );
};
