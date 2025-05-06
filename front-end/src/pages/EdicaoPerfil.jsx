import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateEdicaoPerfil from '../components/templates/TemplateEdicaoPerfil';
import { perfil } from '../api/perfilAPI';

const EdicaoPerfil = () => {
  const navigate = useNavigate();
  const [erros, setErros] = useState({});
  const [usuario, setUsuario] = useState(null);

  useEffect(() => {
    const carregarPerfil = async () => {
      try {
        const dados = await perfil.acessarPerfil();
        setUsuario(dados);
      } catch (error) {
        console.error('Erro ao carregar perfil:', error);
        navigate('/perfil', { replace: true });
      }
    };

    carregarPerfil();
  }, [navigate]);

  const handleSubmit = async (formData) => {
    const novosErros = {};
    if (formData.senha && formData.senha !== formData.confirmarSenha) {
      novosErros.confirmarSenha = 'As senhas não coincidem';
    }

    if (Object.keys(novosErros).length > 0) {
      setErros(novosErros);
      return;
    }

    try {
      await perfil.editarPerfil({
        id: usuario.id,
        nome: formData.nome,
        email: formData.email,
        cpf: usuario.cpf,
        telefone: formData.telefone,
        senha: formData.senha || undefined,
      });

      navigate('/perfil');
    } catch (error) {
      console.error('Erro na atualização do perfil:', error);
      setErros(error.response?.data || { message: 'Erro ao atualizar perfil' });
    }
  };

  if (!usuario) {
    return <div className="container mx-auto pt-8">Carregando...</div>;
  }

  return (
    <TemplateEdicaoPerfil
      onSubmit={handleSubmit}
      erros={erros}
      initialData={usuario}
    />
  );
};

export default EdicaoPerfil;
