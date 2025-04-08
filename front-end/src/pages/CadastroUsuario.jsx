import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateCadastroUsuario from '../components/templates/TemplateCadastroUsuario';
import { usuarios } from '../api/usuariosAPI';

const CadastroUsuario = () => {
  const navigate = useNavigate();
  const [erros, setErros] = useState({});
  const [vinculos, setVinculos] = useState([]);

  const handleSubmit = async (formData) => {
    // Validação
    const novosErros = {};
    if (!formData.nome) novosErros.nome = 'Nome é obrigatório';
    if (!formData.email) novosErros.email = 'E-mail é obrigatório';
    if (!formData.cpf) novosErros.cpf = 'CPF é obrigatório';
    if (!formData.senha) novosErros.senha = 'Senha é obrigatória';
    if (formData.senha !== formData.confirmarSenha) novosErros.confirmarSenha = 'Senhas não coincidem';
    if (vinculos.length === 0) novosErros.vinculos = 'Pelo menos um vínculo é necessário';

    if (Object.keys(novosErros).length > 0) {
      setErros(novosErros);
      return;
    }

    // Chamada API
    try {
      await usuarios.cadastrarUsuario({
        ...formData,
        vinculos: vinculos.map(v => ({
          roleId: v.roleId,
          granjaId: v.granjaId,
          agroindustriaId: v.agroindustriaId
        }))
      });
      navigate('/usuarios');
    } catch (error) {
      console.error('Erro no cadastro:', error);
    }
  };

  const handleAdicionarVinculo = (novoVinculo) => {
    setVinculos([...vinculos, novoVinculo]);
  };

  return (
    <TemplateCadastroUsuario
      onSubmit={handleSubmit}
      erros={erros}
      vinculos={vinculos}
      onAdicionarVinculo={handleAdicionarVinculo}
    />
  );
};

export default CadastroUsuario;