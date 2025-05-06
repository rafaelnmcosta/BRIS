import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import TemplateAtivarUsuario from '../components/templates/TemplateAtivarUsuario';
import { usuarios } from '../api/usuariosAPI';

const AtivarUsuario = () => {
  const navigate = useNavigate();
  const { id } = useParams(); // pega o id do usuário pela URL
  const [erros, setErros] = useState({});
  const [novosVinculos, setNovosVinculos] = useState([]);

  const handleSubmit = async () => {
    // Validação simples
    if (novosVinculos.length === 0) {
      setErros({ novosVinculos: 'Pelo menos um vínculo é necessário' });
      return;
    }

    try {
      await usuarios.reativarUsuario(id, novosVinculos);
      navigate('/usuarios');
    } catch (error) {
      console.error('Erro ao reativar usuário:', error);
    }
  };

  const handleAdicionarVinculo = (novoVinculo) => {
    setNovosVinculos([...novosVinculos, novoVinculo]);
  };

  return (
    <TemplateAtivarUsuario
      onSubmit={handleSubmit}
      erros={erros}
      novosVinculos={novosVinculos}
      onAdicionarVinculo={handleAdicionarVinculo}
    />
  );
};

export default AtivarUsuario;
