import React from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateAutoCadastro from '../components/templates/TemplateAutoCadastro';
import { autenticacao } from '../api/autenticacaoAPI';

const AutoCadastro = () => {
  const navigate = useNavigate();

  const handleAutoCadastro = async (formData) => {
    try {
      const { nome, email, senha, confirmSenha } = formData;

      if (senha !== confirmSenha) {
        alert('As senhas não coincidem!');
        return;
      }

      await autenticacao.autoCadastro({ nome, email, senha });
      alert('Cadastro realizado com sucesso!');
      navigate('/login'); // Redireciona para a página de login
    } catch (error) {
      console.error('Erro no autocadastro:', error);
      alert('Erro ao realizar o cadastro. Tente novamente.');
    }
  };

  return <TemplateAutoCadastro handleAutoCadastro={handleAutoCadastro} />;
};

export default AutoCadastro;
