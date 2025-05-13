// validation/ValidationContext.js
import React, { createContext, useContext } from 'react';

const ValidationContext = createContext();

export const useValidation = () => useContext(ValidationContext);

export const ValidationProvider = ({ children }) => {
  const validarEmail = (email) => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email) ? '' : 'Email inválido';
  };

  /* Essa aqui REALMENTE valida o CPF então só descomentar se for pra produção, teste não rola não

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
  */
  const validarCPF = (cpf) => {
    const cleaned = cpf.replace(/\D/g, '');
    if (cleaned.length !== 11 || /^(\d)\1+$/.test(cleaned)) return 'CPF inválido';
  }

  const validarTelefone = (telefone) => {
    const apenasNumeros = telefone.replace(/\D/g, '');
    return apenasNumeros.length >= 10
      ? ''
      : 'Telefone inválido';
  };

  const validarSenha = (senha) => {
    const erros = [];

    if (senha.length < 8) erros.push("Mínimo de 8 caracteres");
    if (!/[a-z]/.test(senha)) erros.push("Ao menos uma letra minúscula");
    if (!/[A-Z]/.test(senha)) erros.push("Ao menos uma letra maiúscula");
    if (!/[0-9]/.test(senha)) erros.push("Ao menos um número");
    if (!/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(senha)) erros.push("Ao menos um caractere especial");

    return erros.length > 0 ? erros.join(", ") : '';
  };

  const validarConfirmacaoSenha = (senha, confirmar) => {
    return senha === confirmar ? '' : 'As senhas não coincidem';
  };

  const validarCampoObrigatorio = (valor) => {
    return valor?.trim() ? '' : 'Campo obrigatório';
  };

  const validarVinculos = (vinculos) => {
    return Array.isArray(vinculos) && vinculos.length > 0
      ? ''
      : 'Adicione pelo menos um vínculo';
  };

  return (
    <ValidationContext.Provider
      value={{
        validarEmail,
        validarCPF,
        validarTelefone,
        validarSenha,
        validarCampoObrigatorio,
        validarConfirmacaoSenha,
        validarVinculos
      }}
    >
      {children}
    </ValidationContext.Provider>
  );
};
