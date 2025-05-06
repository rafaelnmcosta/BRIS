import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import TemplateCadastroUsuario from '../components/templates/TemplateCadastroUsuario';
import { usuarios } from '../api/usuariosAPI';

const CadastroUsuario = () => {
  const navigate = useNavigate();
  const [vinculos, setVinculos] = useState([]);

  const handleSubmit = async (formData) => {
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
      vinculos={vinculos}
      onAdicionarVinculo={handleAdicionarVinculo}
    />
  );
};

export default CadastroUsuario;